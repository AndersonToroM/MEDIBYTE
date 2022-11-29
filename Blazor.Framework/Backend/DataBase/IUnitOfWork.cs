using Microsoft.EntityFrameworkCore.Storage;

namespace Dominus.Backend.DataBase
{
    public class IUnitOfWork
    {
        public DataBaseSetting Settings { get; protected set; }

        private IDbContextTransaction transaction;

        public DContext DbContext { get; protected set; }

        public IUnitOfWork(DataBaseSetting confg)
        {
            Settings = confg;
        }

        public IUnitOfWork(DContext confg)
        {
            DbContext = confg;
        }

        public void BeginTransaction()
        {
            if (transaction == null)
                transaction = DbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
        }

        public BaseRepository<T> Repository<T>() where T : BaseEntity
        {
            return new BaseRepository<T>(this);
        }

    }
}
