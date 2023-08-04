﻿using ConsoleApp.FreeSqlTemplate.Data;
using ConsoleApp.FreeSqlTemplate.Data.Models;

namespace ConsoleApp.FreeSqlTemplate.Services;
public class TestServices
{
    /// <summary>
    ///仓储测试
    /// </summary>
    private readonly BaseRepositoryExtend<Test, long> _rep;
    /// <summary>
    /// FreeSqlCloud
    /// </summary>
    private readonly FreeSqlCloud _cloud;


    public TestServices(BaseRepositoryExtend<Test, long> rep, FreeSqlCloud cloud)
    {
        _rep = rep;
        _cloud = cloud;
    }

    public async Task TestAsync()
    {
        var list = await _rep.Orm.Select<Test>().WithSql("select  * from Test").ToListAsync();
        var a = await _cloud.Select<Test>().WithSql("select  * from Test").ToListAsync();
    }


}
