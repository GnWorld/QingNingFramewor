using FreeSql;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using QingNing.MultiFreeSql.Attributes;

namespace FreeSqlTest.Services;
public interface ITestService
{
    Task Test();
}

public class TestService : ITestService
{
    private readonly IBaseRepository<AppRole> _roleRep;


    public TestService(IBaseRepository<AppRole> roleRep)
    {
        _roleRep = roleRep;
    }

    [UnitOfWork]
    public async Task Test()
    {
        try
        {
            var role = new AppRole()
            {

                RoleName = "test",
                Code = "Test",
            };

            await _roleRep.InsertAsync(role);

            var role2 = new AppRole()
            {
                RoleName = null,
                Code = "Test",
            };

            await _roleRep.InsertAsync(role2);
            //await _roleRep.Context.Ado.ExecuteCommandAsync("delete from AppRole");
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.ToString());
            throw;
        }

    }

    [Table(Name = "app_role")]
    public class AppRole
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [JsonProperty, Column(Name = "role_id", IsPrimary = true, IsIdentity = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [JsonProperty, Column(Name = "role_name", DbType = "varchar(60)")]
        public string RoleName { get; set; } = string.Empty;

        [Column(IsNullable = false)]
        public string Code { get; set; }
    }
}