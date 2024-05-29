using DiplomIvanova.DataBase.Context;
using DiplomIvanova.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiplomIvanova.Helpers
{
    public static class DataBaseHelper
    {
        public static async Task<List<T>> GetItemsAsync<T>() where T : class, IEntityBase, new()
        {
            using var db = new AppDbContext();
            var dbSet = GetDbSet<T>(db, typeof(T).Name);
            if (dbSet is not null)
            {
                return await dbSet.ToListAsync();
            }
            return [];
        }
        public static async Task<bool> AddItemAsync<T>(T item) where T : class, IEntityBase, new()
        {
            using var db = new AppDbContext();
            var dbSet = GetDbSet<T>(db, typeof(T).Name);
            if (dbSet is not null)
            {
                await dbSet.AddAsync(item);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        private static DbSet<T>? GetDbSet<T>(AppDbContext db,string typeName) where T : class,IEntityBase, new()
        {
            var dbSet=db.GetType().GetProperties()
                .FirstOrDefault(x=>x.PropertyType.IsGenericType
                && x.PropertyType.GenericTypeArguments!.Any(tp=>tp.Name==typeName));
            return (DbSet<T>?)dbSet?.GetValue(db, null);
        }
    }
}
