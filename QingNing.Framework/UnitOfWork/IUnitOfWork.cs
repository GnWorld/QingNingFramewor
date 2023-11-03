using Microsoft.AspNetCore.Mvc.Filters;

namespace QingNing.DatabaseAccessor;
public interface IUnitOfWork : IDisposable
{

    void Dispose();


    bool Commit();



}
