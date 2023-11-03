namespace MultiSugarTestApi;

public interface IBaseService<T> where T : class, new()
{
}

public class BaseService<T> : IBaseService<T> where T : class, new()
{


}