using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CoffeBook.Model;
using CoffeBook.Views;
using Facebook;

namespace CoffeBook.Facebook
{
    class FacebookIntegration
    {
        private string appId = "574708729398919";
        private string appSecret = "5e3da227006e0d338a1427355615ebf8";
        private WebBrowser webBrowser;
        private BrowserWindow browserWindow;

        public FacebookClient FbClient { get; set; } = new FacebookClient();

        public bool FbAccountLoggedIn { get; set; }

        public Uri GetLoginUrl()
        {
            var parameters = new Dictionary<string, object>
            {
                ["client_id"] = appId,
                ["redirect_uri"] = "https://www.facebook.com/connect/login_success.html",
                ["response_type"] = "token",
                ["display"] = "popup",
                ["scope"] = "user_posts,publish_actions"
            };
            var client = new FacebookClient();
            return client.GetLoginUrl(parameters);
        }

        public void Share(object propertiesObject)
        {
            if (!FbAccountLoggedIn)
            {
                try
                {
                    BrowserWindow browser = new BrowserWindow();
                    browserWindow = browser;
                    webBrowser = browser.webBrowser;
                    browser.Show();
                    var uri = GetLoginUrl();
                    browser.webBrowser.Navigate(uri);
                    browser.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unexpected error during web navigation: " + e.Message, "Navigation error");
                }
            }
            else
            {
                var parameters = new Dictionary<string, object>();
                var recipeToPost = propertiesObject as Recipe;
                if (recipeToPost == null) return;
                parameters["message"] = recipeToPost.Description;
                try
                {
                    FbClient.Post("me/feed", parameters);
                    MessageBox.Show("Recipe posted.", "Post");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unexpected error during post action: " + e.Message, "Post error");
                }
            }
        }

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                var callback = webBrowser.Source;
                var oauth = FbClient.ParseOAuthCallbackUrl(callback);
                FbClient.AccessToken = oauth.AccessToken;
                FbAccountLoggedIn = true;
                browserWindow.Close();
                MessageBox.Show("Authentication successful. You can now post recipes to Facebook.");
            }
            catch (Exception ex)
            {
                return;
            }

        }
    }
}
