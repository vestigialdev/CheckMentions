using System.Threading.Tasks;
using Tweetinvi;

public static class Twitter {

    public static async Task<TwitterClient> GetClient(string projectId) {

        try {
            var ConsumerKey = await GoogleSecrets.GetSecretAsync(projectId, "TWITTER_CONSUMER_ID");
            var ConsumerSecret = await GoogleSecrets.GetSecretAsync(projectId, "TWITTER_CONSUMER_SECRET");
            var BearerToken = await GoogleSecrets.GetSecretAsync(projectId, "TWITTER_BEARER_TOKEN");
            var AccessToken = await GoogleSecrets.GetSecretAsync(projectId, "TWITTER_USER_ACCESS_TOKEN");
            var AccessTokenSecret = await GoogleSecrets.GetSecretAsync(projectId, "TWITTER_USER_ACCESS_TOKEN_SECRET");

            return new TwitterClient(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);

        } catch (System.Exception e) {
            System.Console.WriteLine("Problem getting Twitter credentials through Google Secrets");
            throw e;
        }
    }
}