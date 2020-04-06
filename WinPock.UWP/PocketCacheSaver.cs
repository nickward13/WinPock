using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinPock.UWP
{
    public class PocketCacheSaver
    {
        private StorageFolder _storageFolder;

        public PocketCacheSaver(StorageFolder storageFolder)
        {
            _storageFolder = storageFolder;
        }
        
        public async Task<bool> SaveCacheAsync(PocketCache pocketCache)
        {
            StorageFile storageFile = await _storageFolder.CreateFileAsync("PocketCache.json", CreationCollisionOption.ReplaceExisting);
            try
            {
                string pocketCacheString = JsonSerializer.Serialize(pocketCache);
                File.WriteAllText(storageFile.Path, pocketCacheString);
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<PocketCache> LoadCacheAsync(PocketCache pocketCache)
        {
            try
            {
                StorageFile storageFile = await _storageFolder.GetFileAsync("PocketCache.json");
                string pocketCacheString = File.ReadAllText(storageFile.Path);
                pocketCache = JsonSerializer.Deserialize<PocketCache>(pocketCacheString);
                return pocketCache;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
