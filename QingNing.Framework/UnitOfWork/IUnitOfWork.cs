using Microsoft.AspNetCore.Mvc.Filters;

namespace QingNing.UnitOfWork;
public interface IUnitOfWork : IDisposable
{

    void Dispose();


    bool Commit();



}
