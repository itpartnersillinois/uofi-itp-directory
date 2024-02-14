using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.Data {

    public class DirectoryRepository {
        private readonly IDbContextFactory<DirectoryContext> factory;

        public DirectoryRepository(IDbContextFactory<DirectoryContext> factory) {
            this.factory = factory;
        }

        public int Create<T>(T item) where T : BaseDataItem {
            using var context = factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            context.Add(item);
            return context.SaveChanges();
        }

        public async Task<int> CreateAsync<T>(T item) where T : BaseDataItem {
            using var context = factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            context.Add(item);
            return await context.SaveChangesAsync();
        }

        public int Delete<T>(T item) {
            if (item == null) {
                return 0;
            }
            using var context = factory.CreateDbContext();
            _ = context.Remove(item);
            return context.SaveChanges();
        }

        public async Task<int> DeleteAsync<T>(T item) {
            if (item == null) {
                return 0;
            }
            using var context = factory.CreateDbContext();
            _ = context.Remove(item);
            return await context.SaveChangesAsync();
        }

        public int MakeActive<T>(T item, bool active) where T : BaseDataItem {
            using var context = factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            item.IsActive = active;
            context.Update(item);
            return context.SaveChanges();
        }

        public async Task<int> MakeActiveAsync<T>(T item, bool active) where T : BaseDataItem {
            using var context = factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            item.IsActive = active;
            context.Update(item);
            return await context.SaveChangesAsync();
        }

        public T Read<T>(Func<DirectoryContext, T> work) {
            var context = factory.CreateDbContext();
            return work(context);
        }

        public async Task<T> ReadAsync<T>(Func<DirectoryContext, T> work) {
            var context = factory.CreateDbContext();
            return await Task.Run(() => work(context));
        }

        public int Update<T>(T item) where T : BaseDataItem {
            using var context = factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            context.Update(item);
            return context.SaveChanges();
        }

        public async Task<int> UpdateAsync<T>(T item) where T : BaseDataItem {
            using var context = factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            context.Update(item);
            return await context.SaveChangesAsync();
        }
    }
}