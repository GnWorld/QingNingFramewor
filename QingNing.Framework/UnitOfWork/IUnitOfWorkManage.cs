using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QingNing.UnitOfWork;
public interface IUnitOfWorkManage
{

    int TranCount { get; }


    void BeginTran();
    void BeginTran(MethodInfo method);
    void CommitTran();
    void CommitTran(MethodInfo method);
    void RollbackTran();
    void RollbackTran(MethodInfo method);

}
