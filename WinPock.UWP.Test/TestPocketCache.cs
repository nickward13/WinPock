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
    public class TestPocketCache
    {
        [TestMethod]
        public async Task TestInitializationAsync()
        {
            PocketClient pocketClient = new PocketClient(Secrets.AccessToken);
            PocketCache pocketCache = new PocketCache(pocketClient);
            await TestSyncArticlesAsync(pocketCache);
            Assert.IsNotNull(pocketCache);
            Assert.AreNotEqual(0, pocketCache.PocketItems.Count);
            Assert.IsNotNull(pocketCache.LastSyncDateTime);
        }

        private static async Task TestSyncArticlesAsync(PocketCache pocketCache)
        {
            await pocketCache.SyncArticlesAsync();
        }
    }
}
