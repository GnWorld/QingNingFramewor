using PetaPoco;

namespace ConsoleApp.PetaPoco.Tem;
public class TestService
{
    private readonly AipNaipDbContext _db;

    public TestService(AipNaipDbContext db)
    {
        _db = db;
    }

    public async Task Test()
    {
        var sql = Sql.Builder.From(nameof(AppRole));
        var appRole = _db.Query<AppRole>(sql);
    }


}
public class AppRole
{

    public long Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }
}
