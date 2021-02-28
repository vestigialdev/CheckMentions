
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi;

public class CheckFireStoreDB {

    static string ProjectID => "alttextnetwork";

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

        try {
            CollectionReference mentions = DB.Collection("mentions");
            var matching = mentions.WhereEqualTo("tweetId", tweet.IdStr);
            QuerySnapshot snapshot = await matching.GetSnapshotAsync();
            System.Console.WriteLine($"There are {snapshot.Count} known tweets with a matching TweetId");
            return snapshot.Count > 0;
        } catch (System.Exception e) {
            System.Console.WriteLine("Problem querying FireStore  DB");
            throw e;
        }
    }

    public static async Task WriteToDB(ITweet tweet) {

        try {
            //Mention is unknown so upload it to the database
            DocumentReference docRef = DB.Collection("mentions").Document(tweet.IdStr);

            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "tweetId", $"{tweet.IdStr}" },
                { "isReply", "null" },
                { "hasMedia", "null" }
            };

            System.Console.WriteLine($"Mention #{tweet.IdStr} written to db.");
            await docRef.SetAsync(user);
        } catch (System.Exception e) {
            System.Console.WriteLine("Problem writing to FireStore  DB");
            throw e;
        }

    }
}
