using QingNing.MultiDbSqlSugar;

namespace TestWebApi.Entity.SeedData
{
    public class AppRoleSeedData : ISqlSugarEntitySeedData<AppRole>
    {
        public IEnumerable<AppRole> HasData()
        {
            var list = new List<AppRole>()
            {
                new AppRole{RoleId=1,RoleName="超级管理员",Code="SuperAdmin"}
            };
            return list;
        }
    }
}
