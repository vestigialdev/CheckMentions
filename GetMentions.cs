using Google.Cloud.SecretManager.V1;
using Tweetinvi.Models;
using Tweetinvi;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

public static class GetMentionsFromTwitter {
    static string ProjectID => "enduring-badge-305203";
    static System.Collections.Generic.List<string> RecentKnowns = new System.Collections.Generic.List<string>();

    #region Tokens
    static string ProjectPath => @"projects/enduring-badge-305203";
    static string AppConsumerID => GetSecret("TWITTER_CONSUMER_ID");
    static string AppConsumerSecretKey => GetSecret("TWITTER_CONSUMER_SECRET");
    static string BearerToken => GetSecret("TWITTER_BEARER_TOKEN");
    static string UserAccessToken => GetSecret("TWITTER_USER_ACCESS_TOKEN");
    static string UserAccessTokenSecret => GetSecret("TWITTER_USER_ACCESS_TOKEN_SECRET");
    #endregion

    static SecretManagerServiceClient GoogleSecretsClient;
    static TwitterCredentials TwitterCredentials;
    static TwitterClient UserClient;
    static CompareToKnownMentions CompareToKnownMentions;

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
        CompareToKnownMentions = new CompareToKnownMentions();
    }
    public async static void Go() {
        Setup();
        // System.Console.WriteLine($"GetMentionsFromTwitter.Go(): AppConsumerID is {AppConsumerID}");
        var recentMentions = await GoGetMentionsFromTwitter();

        foreach (var tweet in recentMentions) {


            if (RecentKnowns.Contains(tweet.IdStr)) {
                System.Console.WriteLine("Mention was recently seen in this environment, ignoring");
                continue;
            }

            System.Console.WriteLine("Mention wasn't seen recently, checking DB..");

            //Skip over known mentions
            if (await CompareToKnownMentions.IsKnown(tweet)) {
                System.Console.WriteLine("Mention is known to DB, adding to recent mentions");
                RecentKnowns.Add(tweet.IdStr);
                continue;
            }

            //Mention is unknown so upload it to the database
            System.Console.WriteLine("Mention is unknown to DB, adding");
            await CompareToKnownMentions.WriteToDB(tweet);
        }
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
    async static Task<ITweet[]> GoGetMentionsFromTwitter() {
        System.Console.WriteLine($"CheckMentions.GetMentionsFromTwitter()");
        var results = await UserClient.Timelines.GetMentionsTimelineAsync();
        return results;

    }
}