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
        private static PocketCacheSaver _pocketCacheSaver = new PocketCacheSaver(Windows.Storage.ApplicationData.Current.LocalFolder);
        private static PocketClient _pocketClient = new PocketClient(Secrets.AccessToken);
        private static PocketCache _pocketCache = new PocketCache(_pocketClient);

        public PocketCacheSaverTest()
        {

        }

        [TestMethod]
        public async Task TestSave()
        {
            await _pocketCache.SyncArticlesAsync();

            bool result = await _pocketCacheSaver.SaveCacheAsync(_pocketCache);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TestLoad()
        {
            bool result = await _pocketCacheSaver.LoadCacheAsync(_pocketCache);

            Assert.IsNotNull(_pocketCache);
            Assert.AreNotEqual(0, _pocketCache.PocketItems.Count);
            Assert.IsNotNull(_pocketCache.LastSyncDateTime);
            Assert.AreNotEqual(new DateTime(1970, 1, 1), _pocketCache.LastSyncDateTime);
        }
    }
}
