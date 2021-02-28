using System;
using Tweetinvi.Models;
using Tweetinvi;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Tweetinvi.Parameters;
using System.Collections.Generic;
using Tweetinvi.Exceptions;
using System.Linq;
using Google.Cloud.SecretManager.V1;
using Tweetinvi.Iterators;

public class GetMentionsFromTwitter {
    static string ProjectID => "alttextnetwork";

    static System.Collections.Generic.List<string> RecentKnowns = new System.Collections.Generic.List<string>();


    static TwitterCredentials TwitterCredentials;
    static TwitterClient UserClient;

    static void Setup() {
        //Build  credentials
        TwitterCredentials = new TwitterCredentials() {
            ConsumerKey = GetSecrets.GetSecret(ProjectID, "TWITTER_CONSUMER_ID"),
            ConsumerSecret = GetSecrets.GetSecret(ProjectID, "TWITTER_CONSUMER_SECRET"),
            AccessToken = GetSecrets.GetSecret(ProjectID, "TWITTER_USER_ACCESS_TOKEN"),
            AccessTokenSecret = GetSecrets.GetSecret(ProjectID, "TWITTER_USER_ACCESS_TOKEN_SECRET"),
            BearerToken = GetSecrets.GetSecret(ProjectID, "TWITTER_BEARER_TOKEN"),
        };

        UserClient = new TwitterClient(TwitterCredentials);
    }

    public async static void Go() {
        Setup();
        ITwitterIterator<ITweet, long?> MentionsIterator;

        try {
            System.Console.WriteLine("Getting mentions...");

            var parameters = new GetMentionsTimelineParameters();
            parameters.IncludeContributorDetails = false;
            parameters.IncludeEntities = false;
            parameters.TrimUser = true;
            parameters.TweetMode = TweetMode.None;

            MentionsIterator = UserClient.Timelines.GetMentionsTimelineIterator(parameters);

        } catch (TwitterException e) {
            System.Console.WriteLine("Problem getting Mentions Timeline iterator");
            throw e;
        }

        while (!MentionsIterator.Completed) {

            var page = await MentionsIterator.NextPageAsync();
            Console.WriteLine("Retrieved " + page.Count() + " mentions on this page");

            foreach (var tweet in page) {
                Console.WriteLine($"Mention contents: {tweet.FullText}");

                //Check if mention has been evaluated recently
                if (RecentKnowns.Contains(tweet.IdStr)) {
                    System.Console.WriteLine("Mention was recently seen in this environment, ignoring");
                    continue;
                }

                System.Console.WriteLine("Mention wasn't seen recently, checking FireStore..");

                //Check if mention has been evaluated ever
                if (await CheckFireStoreDB.IsKnown(tweet)) {
                    System.Console.WriteLine("Mention is known to FireStore DB, adding to recent mentions");
                    RecentKnowns.Add(tweet.IdStr);
                    continue;
                }
                System.Console.WriteLine("Mention is unknown to DB, adding..");
                await CheckFireStoreDB.WriteToDB(tweet);
            }
        }
    }
}