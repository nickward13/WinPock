using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPock.UWP.Test
{
    [TestClass]
    public class PocketCacheSaverTest
    {
        [TestMethod]
        public async Task TestSave()
        {
            PocketClient pocketClient = new PocketClient(Secrets.AccessToken);
            PocketCache pocketCache = new PocketCache(pocketClient);
            await pocketCache.SyncArticlesAsync();

            PocketCacheSaver pocketCacheSaver = new PocketCacheSaver(Windows.Storage.ApplicationData.Current.LocalFolder);
            bool result = await pocketCacheSaver.SaveCacheAsync(pocketCache);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TestLoad()
        {
            PocketClient pocketClient = new PocketClient(Secrets.AccessToken);
            PocketCache pocketCache = new PocketCache(pocketClient);

            PocketCacheSaver pocketCacheSaver = new PocketCacheSaver(Windows.Storage.ApplicationData.Current.LocalFolder);
            pocketCache = await pocketCacheSaver.LoadCacheAsync(pocketCache);

            Assert.IsNotNull(pocketCache);
            Assert.AreNotEqual(0, pocketCache.PocketItems.Count);
            Assert.IsNotNull(pocketCache.LastSyncDateTime);
        }
    }
}
