using Google.Cloud.Functions.Framework;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Tweetinvi.Models;

public partial class CheckMentions {

    static async void WriteNewMentionsToDB(List<ITweet> newMentions, FirestoreDb DB) {
        foreach (var tweet in newMentions) {
            try {
                //Mention is unknown so upload it to the database
                DocumentReference docRef = DB.Collection("mentions").Document(tweet.IdStr);

                Dictionary<string, object> user = new Dictionary<string, object>
                {
                { "tweetId", $"{tweet.IdStr}" },
                { "isReply", "null" },
                { "hasMedia", "null" }
            };

                Print($"Mention #{tweet.IdStr} written to db.");
                await docRef.SetAsync(user);
            } catch (System.Exception e) {
                Print("Problem writing to FireStore  DB");
                Print(e.Message);
            }
        }
    }
}