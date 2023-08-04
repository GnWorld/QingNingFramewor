using FreeSql.DataAnnotations;

namespace ConsoleApp.FreeSqlTemplate.Data.Models;

[Table(Name ="Test")]
public class Test
{

    public string Code { get; set; }

    public string Name { get; set; }

}
