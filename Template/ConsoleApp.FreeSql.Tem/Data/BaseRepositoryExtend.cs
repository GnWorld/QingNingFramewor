using ConsoleApp.FreeSqlTemplate.Data;
using FreeSql;
using System.Linq.Expressions;

namespace QingNing.FreeSqlTemplate.Data
{
    public class BaseRepositoryExtend<T, TKey> : BaseRepository<T, TKey> where T : class, new()
    {
        public IInsert<T> InsertDiy => Orm.Insert<T>();
        public BaseRepositoryExtend(IFreeSql fsql, Expression<Func<T, bool>> filter = null, Func<string, string> asTable = null) : base(fsql, filter, asTable)
        {
        }
        /// <summary>
        /// 扩展执行主库的insert和update方法,测试
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public int ExecuteSQLInMaster(string strSQL)
        {
            return Orm.Ado.ExecuteNonQuery(strSQL);
        }
        /// <summary>
        /// 扩展批量插入方法，采用分割分批执行
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="valuesLimit"></param>
        /// <param name="parameterLimit"></param>
        /// <returns></returns>
        public List<T> BatchInsert(IEnumerable<T> entitys, int valuesLimit = 5000, int parameterLimit = 3000, bool enablePgCopy = false)
        {
            var iInsert = InsertDiy.AppendData(entitys).NoneParameter().BatchOptions(valuesLimit, parameterLimit);
            if (enablePgCopy)
                iInsert.ExecutePgCopy();
            return iInsert.ExecuteInserted();
        }

        /// <summary>
        /// 异步扩展批量插入方法，采用分割分批执行
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="valuesLimit"></param>
        /// <param name="parameterLimit"></param>
        /// <returns></returns>
        public async Task<List<T>> BatchInsertAsync(IEnumerable<T> entitys, int valuesLimit = 5000, int parameterLimit = 3000, bool enablePgCopy = false)
        {
            var iInsert = InsertDiy.AppendData(entitys).NoneParameter().BatchOptions(valuesLimit, parameterLimit);
            if (enablePgCopy)
                await iInsert.ExecutePgCopyAsync();
            return await iInsert.ExecuteInsertedAsync();
        }
        /// <summary>
        /// 批量更新集合对应的字段，并返回更新后的数据集合，此方法只有 Postgresql/SqlServer 有效果
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="strsFields"></param>
        /// <returns></returns>
        public async Task<List<T>> BatchUpdateWithFieldsAsync(IEnumerable<T> entitys, string[] strsFields)
        {
            return await UpdateDiy.SetSource(entitys).UpdateColumns(strsFields).ExecuteUpdatedAsync();
        }
    }
}
