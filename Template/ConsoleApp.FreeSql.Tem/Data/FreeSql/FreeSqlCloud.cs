using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Npgsql;

namespace ConsoleApp.FreeSqlTemplate.Data;

public enum DbEnum { IDS, FPL }
public class FreeSqlCloud : FreeSqlCloud<DbEnum>
{
    public FreeSqlCloud() : base(null) { }
    public FreeSqlCloud(string distributeKey) : base(distributeKey) { }
}

