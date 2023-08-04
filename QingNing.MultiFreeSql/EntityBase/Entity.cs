using FreeSql.DataAnnotations;

namespace QingNing.MultiFreeSql;
public class Entity
{
    [Column(IsPrimary = true, Name = "id")]
    public long Id { get; set; }
}
