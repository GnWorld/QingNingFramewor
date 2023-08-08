using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.FreeSqlTemplate.Data.Models.Fpl;
[Table(Name = "FPLRoles")]
public class FPLRole : EntityBase
{


    /// <summary>
    /// 角色名称
    /// </summary>
    [StringLength(20, ErrorMessage = "角色名称最多20个字符")]
    public string Name { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    [StringLength(20, ErrorMessage = "角色编码最多20个字符")]
    public string Code { get; set; }

    /// <summary>
    /// 是系统角色还是自定义角色
    /// </summary>
    public bool IsSystemRole { get; set; } = false;

    /// <summary>
    /// 角色关联的权限编码集合
    /// </summary>
    public string[] Permissions { get; set; }


}
