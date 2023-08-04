using System;

namespace QingNing.MultiFreeSql
{
    public interface IBaseMultiFreeSql<TDBKey>
    {
        public IFreeSql Get(TDBKey dbkey);
        public IBaseMultiFreeSql<TDBKey> Register(TDBKey dbkey, Func<IFreeSql> create);
    }
    public class BaseMultiFreeSql<TDBKey> : IBaseMultiFreeSql<TDBKey>
    {
        internal IdleBus<TDBKey, IFreeSql> _ib;
        public BaseMultiFreeSql()
        {
            _ib = new IdleBus<TDBKey, IFreeSql>(TimeSpan.MaxValue);
            _ib.Notice += (_, __) => { };
        }
        public IFreeSql Get(TDBKey dbkey)
        {
            return _ib.Get(dbkey);
        }

        public IBaseMultiFreeSql<TDBKey> Register(TDBKey dbkey, Func<IFreeSql> create)
        {
            if (!_ib.TryRegister(dbkey, create))
            {
                throw new Exception("注册失败！");
            }
            return this;
        }
    }
}
