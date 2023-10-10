using PetaPoco;
using PetaPoco.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PetaPoco.Tem;
public class AipNaipDbContext : Database
{
    public AipNaipDbContext(IDatabaseBuildConfiguration configuration) : base(configuration)
    {
    }

    public AipNaipDbContext(IDbConnection connection, IMapper defaultMapper = null) : base(connection, defaultMapper)
    {

    }

    public AipNaipDbContext(string connectionString, string providerName, IMapper defaultMapper = null) : base(connectionString, providerName, defaultMapper)
    {
    }

    public AipNaipDbContext(string connectionString, DbProviderFactory factory, IMapper defaultMapper = null) : base(connectionString, factory, defaultMapper)
    {
    }

    public AipNaipDbContext(string connectionString, IProvider provider, IMapper defaultMapper = null) : base(connectionString, provider, defaultMapper)
    {
    }
}
