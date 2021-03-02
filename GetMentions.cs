using Google.Cloud.Functions.Framework;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using System.Threading.Tasks;
using Tweetinvi.Models;
using System.Linq;
using System;

public partial class CheckMentions {

    static async Task<List<ITweet>> GetMentions() {
        Print("GetMentions()");

        var tweets = new List<ITweet>();
        var mentionsTimelineIterator = Client.Timelines.GetMentionsTimelineIterator();

        while (!mentionsTimelineIterator.Completed) {
            var page = await mentionsTimelineIterator.NextPageAsync();

            foreach (var item in page) {
                tweets.Add(item);
            }
        }
        Print($"GetMentions: {tweets.Count} total mentions");

        return tweets;
    }
}