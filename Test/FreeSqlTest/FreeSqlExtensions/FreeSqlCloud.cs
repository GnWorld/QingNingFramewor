using FreeSql;

namespace FreeSqlTest;

public enum DbEnum { safiDb }


public class FreeSqlCloud : FreeSqlCloud<DbEnum>
{
    public FreeSqlCloud() : base(null) { }
    public FreeSqlCloud(string distributeKey) : base(distributeKey) { }
}

