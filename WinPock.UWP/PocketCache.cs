using PocketApi;
using PocketApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPock.UWP
{
    public class PocketCache
    {
        private PocketClient _pocketClient;
        public ObservableCollection<PocketItem> PocketItems { get; set; }
        public DateTime LastSyncDateTime { get; set; }

        public PocketCache(PocketClient pocketClient)
        {
            _pocketClient = pocketClient;
            LastSyncDateTime = new DateTime(1970, 1, 1);
            PocketItems = new ObservableCollection<PocketItem>();
        }

        public async Task SyncArticlesAsync()
        {
            DateTime newSyncDateTime = DateTime.UtcNow;
            IEnumerable<PocketItem> pocketItems = await _pocketClient.GetPocketItemsAsync(LastSyncDateTime);

            foreach (PocketItem pocketItem in pocketItems)
            {
                switch(pocketItem.Status)
                {
                    case "0":
                        PocketItems.Add(pocketItem);
                        break;
                    case "1":
                        if (PocketItems.Any(pi => pi.Id == pocketItem.Id))
                            PocketItems.First(pi => pi.Id == pocketItem.Id).Status = "1";
                        else
                            PocketItems.Add(pocketItem);                        
                        break;
                    case "2":
                        PocketItems.Remove(PocketItems.First(pi => pi.Id == pocketItem.Id));
                        break;
                }
            }

            LastSyncDateTime = newSyncDateTime;
        }

        public async Task AddArticleAsync(Uri uri)
        {
            PocketItem pocketItem = await _pocketClient.AddPocketItemAsync(uri);
            await SyncArticlesAsync();
        }

        public async Task DeleteArticleAsync(PocketItem pocketItem)
        {
            await _pocketClient.DeletePocketItemAsync(pocketItem);
            await SyncArticlesAsync();
        }

    }
}
