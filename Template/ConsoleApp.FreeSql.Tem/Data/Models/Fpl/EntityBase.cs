using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.FreeSqlTemplate.Data.Models.Fpl;
public class EntityBase
{

    public Guid Id { get; set; }

    /// <summary>
    /// 航空公司id
    /// </summary>
    public Guid? TenantId { get; set; }
    /// <summary>
    ///  创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 创建人
    /// </summary>
    public string CreateUser { get; set; }

    /// <summary>
    ///  更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string UpdateUser { get; set; }
}
