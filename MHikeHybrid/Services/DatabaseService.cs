
using MHikeHybrid.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MHikeHybrid.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        private bool _isInitialized = false;

        public DatabaseService()
        {
          
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "mhike_hybrid.db");
            _database = new SQLiteAsyncConnection(dbPath);
        }

       
        private async Task Init()
        {
            if (_isInitialized)
                return;

          
            await _database.CreateTableAsync<Hike>();

            _isInitialized = true;
        }

        
        public async Task<int> SaveHikeAsync(Hike hike)
        {
            await Init(); 

            if (hike.Id != 0)
            {
               
                return await _database.UpdateAsync(hike);
            }
            else
            {
               
                return await _database.InsertAsync(hike);
            }
        }

   
        public async Task<List<Hike>> GetHikesAsync()
        {
            await Init();
            return await _database.Table<Hike>().ToListAsync();
        }

       
        public async Task<Hike> GetHikeAsync(int id)
        {
            await Init();
            return await _database.Table<Hike>().Where(h => h.Id == id).FirstOrDefaultAsync();
        }

       
        public async Task<int> DeleteHikeAsync(Hike hike)
        {
            await Init();
            return await _database.DeleteAsync(hike);
        }

        
        public async Task DeleteAllHikesAsync()
        {
            await Init();
            await _database.DeleteAllAsync<Hike>();
        }
    }
}