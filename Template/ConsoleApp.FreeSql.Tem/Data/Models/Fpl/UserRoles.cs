﻿using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.FreeSqlTemplate.Data.Models.Fpl;

[Table(Name = "FPLUserRoles")]
public class UserRoles : EntityBase
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    public Guid RoleId { get; set; }
}
