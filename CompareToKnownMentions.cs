
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi;

public class CompareToKnownMentions {

    static string ProjectID => "enduring-badge-305203";
    static FirestoreDb _DB;
    static FirestoreDb DB {
        get {
            if (null == _DB) {
                _DB = FirestoreDb.Create(ProjectID);
            }

            return _DB;
        }
    }

    public static async Task<bool> IsKnown(ITweet tweet) {
        CollectionReference mentions = DB.Collection("mentions");
        var matching = mentions.WhereEqualTo("tweetId", tweet.IdStr);
        QuerySnapshot snapshot = await matching.GetSnapshotAsync();
        System.Console.WriteLine($"There are {snapshot.Count} known tweets with a matching TweetId");

        return snapshot.Count > 0;
    }

    public static async Task WriteToDB(ITweet tweet) {

        DocumentReference docRef = DB.Collection("mentions").Document(tweet.IdStr);

        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "tweetId", $"{tweet.IdStr}" },
                { "isReply", "null" },
                { "hasMedia", "null" }
            };

        System.Console.WriteLine($"Mention #{tweet.IdStr} written to db.");
        await docRef.SetAsync(user);
    }
}
