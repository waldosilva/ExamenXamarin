using ExamenXamarin.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenXamarin.Data
{
  public class TestDatabase
  {
    static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
    {
      return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
    });

    static SQLiteAsyncConnection Database => lazyInitializer.Value;
    static bool initialized = false;

    public TestDatabase()
    {
      InitializeAsync().SafeFireAndForget(false);
    }

    async Task InitializeAsync()
    {
      if (!initialized)
      {
        if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Usuario).Name))
        {
          await Database.CreateTablesAsync(CreateFlags.None, typeof(Usuario)).ConfigureAwait(false);
        }
        initialized = true;
      }
    }

    public Task<List<Usuario>> GetUsersAsync()
    {
      return Database.Table<Usuario>().ToListAsync();
    }

    //public Task<List<Usuario>> GetItemsNotDoneAsync()
    //{
    //  return Database.QueryAsync<Usuario>("SELECT * FROM [Usuario] WHERE [Done] = 0");
    //}

    public Task<Usuario> GetUserAsync(int id)
    {
      return Database.Table<Usuario>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public Task<int> SaveUserAsync(Usuario item)
    {
      if (item.Id != 0)
      {
        return Database.UpdateAsync(item);
      }
      else
      {
        return Database.InsertAsync(item);
      }
    }

    public Task<int> DeleteUserAsync(Usuario item)
    {
      return Database.DeleteAsync(item);
    }
    //public Task<int> DeleteUserByIdAsync(int id)
    //{
    //  var user =Database.Table<Usuario>().Where(i => i.Id == id).FirstOrDefaultAsync();
    //  return Database.DeleteAsync(user);
    //}
  }
}

