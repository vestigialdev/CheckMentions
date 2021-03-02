using Google.Cloud.Functions.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Tweetinvi.Models;

public partial class CheckMentions {

    static async Task<List<ITweet>> FilterMentions(List<ITweet> tweets, FirestoreDb DB) {
        Print("FilterMentions");

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
                Print("Problem querying FireStore DB for known mentions", true);
                Print(e.Message, true);
            }
        }

        Print($"New mentions {newMentions.Count}");

        return newMentions;
    }
}