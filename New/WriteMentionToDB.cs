using Google.Cloud.Functions.Framework;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Tweetinvi.Models;

public partial class Function : IHttpFunction {

    async void WriteNewMentionsToDB(List<ITweet> newMentions, FirestoreDb DB) {
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

                System.Console.WriteLine($"Mention #{tweet.IdStr} written to db.");
                await docRef.SetAsync(user);
            } catch (System.Exception e) {
                System.Console.WriteLine("Problem writing to FireStore  DB");
                throw e;
            }
        }
    }
}