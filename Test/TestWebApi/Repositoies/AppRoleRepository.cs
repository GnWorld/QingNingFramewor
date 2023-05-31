using QingNing.MultiDbSqlSugar;

namespace TestWebApi.Repositoies
{
    public class AppRoleRepository<T> : SqlSugarRepository<T> where T : class, new()
    {
    }
}
