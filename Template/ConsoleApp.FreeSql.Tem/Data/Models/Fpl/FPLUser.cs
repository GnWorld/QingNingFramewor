using ConsoleApp.FreeSqlTemplate.Data.Models.Fpl;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.FreeSqlTemplate.Data.Models;
/// <summary>
/// 用户实体
/// </summary>
[Table(Name = "FPLUsers")]
public class FPLUser : EntityBase
{
    public FPLUser()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }


    /// <summary>
    /// IDS中该用户的登录账号
    /// </summary>
    [StringLength(256, ErrorMessage = "用户名最多256个字符")]
    public string IdsUserName { get; set; }

    /// <summary>
    /// 用户名称，用于显示
    /// </summary>
    [StringLength(20, ErrorMessage = "用户名称最多20个字符")]
    public string UserName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [StringLength(20, ErrorMessage = "手机号最多20个字符")]
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [StringLength(30, ErrorMessage = "邮箱最多30个字符")]
    public string Email { get; set; }


    public string ConcurrencyStamp { get; set; }

}