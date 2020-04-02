using PocketApi;
using PocketApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPock.UWP
{
    public class PocketCache
    {
        private PocketClient _pocketClient;
        public List<PocketItem> PocketItems { get; set; }
        public DateTime LastSyncDateTime { get; private set; }

        public PocketCache(PocketClient pocketClient)
        {
            _pocketClient = pocketClient;
            LastSyncDateTime = new DateTime(1970, 1, 1);
            PocketItems = new List<PocketItem>();
        }

        public async Task SyncArticlesAsync()
        {
            foreach (PocketItem pocketItem in await _pocketClient.GetPocketItemsAsync(LastSyncDateTime))
                    PocketItems.Add(pocketItem);
            
            LastSyncDateTime = DateTime.Now;
        }
    }
}
