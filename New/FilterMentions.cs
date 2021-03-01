using Google.Cloud.Functions.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Tweetinvi.Models;

public partial class CheckMentions {

    static async Task<List<ITweet>> FilterMentions(List<ITweet> tweets, FirestoreDb DB) {
        System.Console.WriteLine("FilterMentions");

        var newMentions = new List<ITweet>();
        foreach (var tweet in tweets) {

            //local cache
            if (RecentlyParsed.Contains(tweet.Id)) {
                continue;
            } else {
                RecentlyParsed.Add(tweet.Id);
            }

            try {
                CollectionReference mentions = DB.Collection("mentions");
                var query = mentions.WhereEqualTo("tweetId", tweet.IdStr);

                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                // System.Console.WriteLine($"There are {snapshot.Count} known tweets with a matching TweetId");
                if (snapshot.Count == 0) {
                    newMentions.Add(tweet);
                }
            } catch (System.Exception e) {
                System.Console.WriteLine("Problem querying FireStore DB for known mentions");
                throw e;
            }
        }
        System.Console.WriteLine($"New mentions {newMentions.Count}");

        return newMentions;
    }
}