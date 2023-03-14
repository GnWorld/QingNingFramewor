using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingNing.Consul
{
    public interface IConsulRegister
    {
        Task ConsulRegistAsync();
    }
}
