using PocketApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            StorageFile itemsStorageFile = await _storageFolder.CreateFileAsync("PocketItems.json", CreationCollisionOption.ReplaceExisting);
            StorageFile lastSyncDateStorageFile = await _storageFolder.CreateFileAsync("SyncDate.json", CreationCollisionOption.ReplaceExisting);

            try
            {
                string itemsString = JsonSerializer.Serialize(pocketCache.PocketItems);
                string lastSyncDateString = JsonSerializer.Serialize(pocketCache.LastSyncDateTime);
                File.WriteAllText(itemsStorageFile.Path, itemsString);
                File.WriteAllText(lastSyncDateStorageFile.Path, lastSyncDateString);
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> LoadCacheAsync(PocketCache pocketCache)
        {
            try
            {
                StorageFile itemsStorageFile = await _storageFolder.GetFileAsync("PocketItems.json");
                string itemsString = File.ReadAllText(itemsStorageFile.Path);
                pocketCache.PocketItems = JsonSerializer.Deserialize<ObservableCollection<PocketItem>>(itemsString);
                
                StorageFile lastSyncDateStorageFile = await _storageFolder.GetFileAsync("SyncDate.json");
                string lastSyncDateString = File.ReadAllText(lastSyncDateStorageFile.Path);
                pocketCache.LastSyncDateTime = JsonSerializer.Deserialize<DateTime>(lastSyncDateString);
                
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
    }
}
