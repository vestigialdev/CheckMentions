using Google.Cloud.SecretManager.V1;
using Tweetinvi.Models;
using Tweetinvi;

public static class GetMentionsFromTwitter {

    #region Tokens
    static string ProjectPath => @"projects/enduring-badge-305203";
    static string AppConsumerID => GetSecret("TWITTER_CONSUMER_ID");
    static string AppConsumerSecretKey => GetSecret("TWITTER_CONSUMER_SECRET");
    static string BearerToken => GetSecret("TWITTER_BEARER_TOKEN");
    static string UserAccessToken => GetSecret("TWITTER_USER_ACCESS_TOKEN");
    static string UserAccessTokenSecret => GetSecret("TWITTER_USER_ACCESS_TOKEN_SECRET");
    // string NGROKAddress = "https://268761ad3537.ngrok.io/";
    // long alt_network_id = 1362289253686870016;
    #endregion

    static SecretManagerServiceClient GoogleSecretsClient;
    static TwitterCredentials TwitterCredentials;
    static TwitterClient UserClient;
    static void Setup() {
        System.Console.WriteLine($"CheckMentions.Setup()");

        //Create our way to retrieve Twitter API keys from google
        GoogleSecretsClient = SecretManagerServiceClient.Create();

        //Build  credentials
        TwitterCredentials = new TwitterCredentials() {
            ConsumerKey = AppConsumerID,
            ConsumerSecret = AppConsumerSecretKey,
            AccessToken = UserAccessToken,
            AccessTokenSecret = UserAccessTokenSecret,
            BearerToken = BearerToken,
        };

        UserClient = new TwitterClient(TwitterCredentials);
    }
    public static void Go() {
        Setup();
        // System.Console.WriteLine($"GetMentionsFromTwitter.Go(): AppConsumerID is {AppConsumerID}");
        GoGetMentionsFromTwitter();
    }

    static string GetSecret(string secretName) {
        try {
            var result = GoogleSecretsClient.AccessSecretVersion($"{ProjectPath}/secrets/{secretName}/versions/latest");
            var secretData = result.Payload.Data.ToString(System.Text.Encoding.UTF8);
            return secretData;
        } catch (System.Exception) {
            System.Console.WriteLine("Error with accessing secrets");
            throw;
        }
    }
    static void GoGetMentionsFromTwitter() {
        System.Console.WriteLine($"CheckMentions.GetMentionsFromTwitter()");
        var results = UserClient.Timelines.GetMentionsTimelineAsync();

        foreach (var item in results.Result) {
            // await context.Response.WriteAsync($"Full text: {item.FullText}");
            System.Console.WriteLine($"Full text: {item.FullText}");
        }
    }
}