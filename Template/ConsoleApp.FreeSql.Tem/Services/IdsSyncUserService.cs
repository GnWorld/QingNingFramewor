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
        //IDS库
        _cloud.Change(DbEnum.IDS);
        var idsUsers = await _cloud.Select<IdsUserDto>().WithSql(@"Select * From AbpUsers as u WHERE u.IsDeleted=0 AND TenantId is not null").ToListAsync();
        var idsTenants = await _cloud.Select<TenantDto>().WithSql("select * from AbpTenants").ToListAsync();
        var idsRoles = await _cloud
                                    .Select<IdsRoleDto>()
                                    .WithSql(@$"Select r.*,ur.UserId from AbpRoles as r 
                                                            Left Join AbpUserRoles as ur On r.Id=ur.RoleId").ToListAsync();
        _logger.LogInformation($"从Ids查询到{idsUsers.Count}个用户");
        var i = 1;
        //fpl库
        _cloud.Change(DbEnum.FPL);
        foreach (var idsUser in idsUsers)
        {
            _logger.LogInformation($"******************正在处理第{i}条{idsUser.UserName},租户{idsUser.TenantId}");
            try
            {
                var tennant = idsTenants.FirstOrDefault(x => x.Id == idsUser.TenantId);

                var userRoles = idsRoles.Where(x => x.UserId == idsUser.Id);
                using (var ctx = _cloud.CreateDbContext())
                {
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
                    if (tennant != null && (userRoles.Any(x => x.Name.ToLower() == "admin" || x.Name.ToLower() == "companyadmin")))
                    {
                        var role = await _cloud.Select<FPLRole>().Where(x => x.TenantId == tennant.Id && (x.Code == "SystemAdmin" || x.Code == "CompanyAdmin")).FirstAsync();
                        //创建航司管理员
                        if (role == null)
                        {
                            var permissions = new List<string> { "AircraftManagement", "CompanyRouteManagement", "AirportManagement", "ReclearManagement", "EmergencyManagement", "FlightPlanManagement", "UserManagement", "SelfManagement", "RoleMangement", "AircraftCreate", "AircraftUpdate", "AircraftView", "AircrafDelete", "CompanyRouteCreate", "CompanyRouteUpdate", "CompanyRouteView", "CompanyRouteDelete", "AirportCreate", "AirportUpdate", "AirportView", "AirportDelete", "ReclearCreate", "ReclearUpdate", "ReclearView", "ReclearDelete", "EmergencyCreate", "EmergencyUpdate", "EmergencyView", "EmergencyDelete", "FlightPlanCreate", "FlightPlanView", "FlightPlanDelete", "DiversionCreate", "UserCreate", "UserUpdate", "UserView", "UserDelete", "SelfView", "SelfUpdate", "RoleCreate", "RoleUpdate", "RoleView", "RoleDelete" };

                            role = new FPLRole();
                            role.Id = Guid.NewGuid();
                            role.Name = "航司管理员";
                            role.Code = "CompanyAdmin";
                            role.IsSystemRole = true;
                            role.TenantId = tennant.Id;
                            role.Permissions = permissions.ToArray();
                            role.CreateTime = DateTime.Now;
                            role.CreateUser = "System";
                            await ctx.AddAsync(role);
                        }
                        var userRole = new FPLUserRoles()
                        {
                            Id = Guid.NewGuid(),
                            TenantId = user.TenantId,
                            UserId = user.Id,
                            RoleId = role.Id,
                            CreateTime = DateTime.Now,
                            CreateUser = "System",
                        };
                        await ctx.AddAsync(userRole);
                        _logger.LogInformation("配置用户角色为航司管理员");
                    }

                    await ctx.AddAsync(user);
                    await ctx.SaveChangesAsync();
                }
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

}
