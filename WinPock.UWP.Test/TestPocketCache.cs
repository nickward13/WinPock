using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketApi;
using PocketApi.Models;
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
        private PocketClient pocketClient;
        private string testArticleUrl = "https://www.theage.com.au/culture/music/inside-the-private-world-of-nick-cave-love-life-and-doodles-20200326-p54e4z.html";

        public TestPocketCache()
        {
            pocketClient = new PocketClient(Secrets.AccessToken);
        }

        [TestMethod]
        public async Task TestSyncArticlesAsync()
        {
            PocketCache pocketCache = new PocketCache(pocketClient);

            await pocketCache.SyncArticlesAsync();

            Assert.IsNotNull(pocketCache);
            Assert.AreNotEqual(0, pocketCache.PocketItems.Count);
            Assert.IsNotNull(pocketCache.LastSyncDateTime);
        }

        [TestMethod]
        public async Task TestAddArticleAsync()
        {
            PocketCache pocketCache = new PocketCache(pocketClient);
            await pocketCache.SyncArticlesAsync();

            if (pocketCache.PocketItems.Any(pi => pi.GivenUrl == testArticleUrl))
                await pocketClient.DeletePocketItemAsync(pocketCache.PocketItems.First(pi => pi.GivenUrl == testArticleUrl));

            int articleCountStart = pocketCache.PocketItems.Count;
            await pocketCache.AddArticleAsync(new Uri(testArticleUrl));
            int articleCountEnd = pocketCache.PocketItems.Count;

            Assert.AreNotEqual(articleCountStart, articleCountEnd);
            Assert.IsTrue(articleCountEnd > articleCountStart);
            Assert.IsTrue(articleCountEnd == articleCountStart + 1);
        }

        [TestMethod]
        public async Task TestDeleteArticleAsync()
        {
            PocketCache pocketCache = new PocketCache(pocketClient);
            await pocketCache.SyncArticlesAsync();

            if (!pocketCache.PocketItems.Any(pi => pi.GivenUrl == testArticleUrl))
                await pocketClient.AddPocketItemAsync(new Uri(testArticleUrl));

            int articleCountStart = pocketCache.PocketItems.Count;
            PocketItem pocketItem = pocketCache.PocketItems.First(pi => pi.GivenUrl == "https://www.theage.com.au/culture/music/inside-the-private-world-of-nick-cave-love-life-and-doodles-20200326-p54e4z.html");
            await pocketCache.DeleteArticleAsync(pocketItem);
            int articleCountEnd = pocketCache.PocketItems.Count;

            Assert.AreNotEqual(articleCountStart, articleCountEnd);
            Assert.IsTrue(articleCountEnd < articleCountStart);
            Assert.IsTrue(articleCountEnd == articleCountStart - 1);
        }
    }
}
