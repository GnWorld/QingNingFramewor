using ConsoleApp.FreeSqlTemplate.Data;
using ConsoleApp.FreeSqlTemplate.Data.Models;
using ConsoleApp.FreeSqlTemplate.Data.Models.Fpl;
using ConsoleApp.FreeSqlTemplate.Services.Dtos.Ids;
using FreeSql;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Data;

namespace ConsoleApp.FreeSqlTemplate.Services;


/// <summary>
/// 同步用户数据
/// </summary>
public class IdsSyncUserService
{
    private readonly FreeSqlCloud _cloud;
    private readonly ILogger<IdsSyncUserService> _logger;

    public IdsSyncUserService(FreeSqlCloud cloud, ILogger<IdsSyncUserService> logger)
    {
        _cloud = cloud;
        _logger = logger;
    }

    /// <summary>
    /// 同步用户
    /// </summary>
    /// <returns></returns>
    public async Task SyncUser()
    {
        var idsUsers = await _cloud.Select<IdsUserDto>().WithSql(@"Select * From AbpUsers as u WHERE u.IsDeleted=0 AND TenantId is not null").ToListAsync();
        _logger.LogInformation($"从Ids查询到{idsUsers.Count}个用户");
        var i = 1;
        foreach (var idsUser in idsUsers)
        {
            _logger.LogInformation($"******************正在处理第{i}条{idsUser.UserName},租户{idsUser.TenantId}********************");
            try
            {
                //切换到ids库
                _cloud.Change(DbEnum.ids);
                var tennant = await _cloud.Select<TenantDto>().WithSql($"select * from AbpTenants where Id='{idsUser.TenantId.ToString()}'").FirstAsync();

                var idsRoles = await _cloud
                                                    .Select<IdsRoleDto>()
                                                    .WithSql(@$"Select r.* from AbpRoles as r 
                                                            Left Join AbpUserRoles as ur On r.Id=ur.RoleId 
                                                            Where ur.UserId='{idsUser.Id}'")
                                                    .ToListAsync();


                //切换到fpl 库
                _cloud.Change(DbEnum.fpl);
                var test = _cloud.Select<FPLUser>().ToList();

                var user = new FPLUser()
                {
                    Id = idsUser.Id,
                    TenantId = idsUser.TenantId,
                    Email = idsUser.Email,
                    Phone = idsUser.PhoneNumber,
                    IdsUserName = idsUser.UserName,
                    UserName = idsUser.Name,
                    CreateTime = DateTime.Now,
                    CreateUser = "System",
                    UpdateTime = DateTime.Now,
                    UpdateUser = "System",
                };
                if (tennant != null && (idsRoles.Any(x => x.Name.ToLower() == "admin" || x.Name.ToLower() == "companyadmin")))
                {
                    var role = await GetFPLRole(tennant.Id);

                    var userRole = new UserRoles()
                    {
                        Id = Guid.NewGuid(),
                        TenantId = user.TenantId,
                        UserId = user.Id,
                        RoleId = role.Id,
                        CreateTime = DateTime.Now,
                        CreateUser = "System",
                    };
                    _cloud.Insert(userRole);
                    _logger.LogInformation("配置用户角色为航司管理员");
                }

                var a = _cloud.Insert(user).ExecuteIdentity();

            }
            catch (Exception ex)
            {
                _logger.LogError($"处理{idsUser.UserName}时发生异常：{ex.Message}");
                continue;

            }
            finally
            {
                i++;
            }
            _logger.LogInformation($"end用户{idsUser.UserName}同步成功");
        }
    }

    private async Task<FPLRole> GetFPLRole(Guid tenantId)
    {
        _cloud.Change(DbEnum.fpl);
        var role = await _cloud.Select<FPLRole>().Where(x => x.TenantId == tenantId && (x.Code == "SystemAdmin" || x.Code == "CompanyAdmin")).FirstAsync();
        //创建航司管理员  ,  超级管理员是肯定存在的
        if (role == null)
        {
            var permissions = new List<string> { "AircraftManagement", "CompanyRouteManagement", "AirportManagement", "ReclearManagement", "EmergencyManagement", "FlightPlanManagement", "UserManagement", "SelfManagement", "RoleMangement", "AircraftCreate", "AircraftUpdate", "AircraftView", "AircrafDelete", "CompanyRouteCreate", "CompanyRouteUpdate", "CompanyRouteView", "CompanyRouteDelete", "AirportCreate", "AirportUpdate", "AirportView", "AirportDelete", "ReclearCreate", "ReclearUpdate", "ReclearView", "ReclearDelete", "EmergencyCreate", "EmergencyUpdate", "EmergencyView", "EmergencyDelete", "FlightPlanCreate", "FlightPlanView", "FlightPlanDelete", "DiversionCreate", "UserCreate", "UserUpdate", "UserView", "UserDelete", "SelfView", "SelfUpdate", "RoleCreate", "RoleUpdate", "RoleView", "RoleDelete" };

            var dto = new FPLRole();
            dto.Id = Guid.NewGuid();
            dto.Name = "航司管理员";
            dto.Code = "CompanyAdmin";
            dto.IsSystemRole = true;
            dto.TenantId = tenantId;
            dto.Permissions = permissions.ToArray();
            dto.CreateTime = DateTime.Now;
            dto.CreateUser = "System";
            _cloud.Insert(dto);
            role = dto;
        }
        return role;
    }

}
