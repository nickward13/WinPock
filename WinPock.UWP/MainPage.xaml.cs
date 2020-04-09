using PocketApi;
using PocketApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinPock.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private PocketClient pocketClient;
        private PocketCache pocketCache;
        private PocketCacheSaver cacheSaver = new PocketCacheSaver(Windows.Storage.ApplicationData.Current.LocalFolder);

        public MainPage()
        {
            this.InitializeComponent();
            this.InitializePocketCache();
        }

        private async void InitializePocketCache()
        {
            await this.AuthPocketAsync();
            pocketCache = new PocketCache(pocketClient);
            await cacheSaver.LoadCacheAsync(pocketCache);
            await SyncPocketCacheAsync();
        }

        private async Task SyncPocketCacheAsync()
        {
            await pocketCache.SyncArticlesAsync();
            await cacheSaver.SaveCacheAsync(pocketCache);
        }

        private async Task AuthPocketAsync()
        {
            if (!AuthPocketViaSavedAccessToken())
            {
                pocketClient = new PocketClient(Secrets.PocketAPIConsumerKey);
                Uri callbackUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
                RequestToken requestToken = await pocketClient.ObtainRequestTokenAsync(
                    callbackUri);

                Uri requestUri = pocketClient.ObtainAuthorizeRequestTokenRedirectUri(requestToken, callbackUri);
                WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateSilentlyAsync(requestUri);
                if (result.ResponseStatus != WebAuthenticationStatus.Success)
                    result = await WebAuthenticationBroker.AuthenticateAsync(
                        WebAuthenticationOptions.None,
                        requestUri);

                AccessToken token = await pocketClient.ObtainAccessTokenAsync(requestToken);
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                string tokenString = JsonSerializer.Serialize(token);
                localSettings.Values.Add("accessToken", tokenString);
            }
        }
        
        private bool AuthPocketViaSavedAccessToken()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.TryGetValue("accessToken", out object accessTokenObject))
            {
                AccessToken accessToken = JsonSerializer.Deserialize<AccessToken>(accessTokenObject.ToString());
                pocketClient = new PocketClient(accessToken);
                return true;
            }
            return false;
        }
    }
}
