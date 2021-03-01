using Microsoft.AspNetCore.Http;
using CloudNative.CloudEvents;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Tweetinvi;
using System;
using Google.Cloud.Functions.Framework;

public partial class Function : IHttpFunction {

    async Task PrintUsername() {
        var user = await Client.Users.GetAuthenticatedUserAsync();
        Console.WriteLine("Printing Username of Twitter account:");
        Console.WriteLine(user);
    }
    async Task PostTweet() {
        var tweet = await Client.Tweets.PublishTweetAsync("Posting from Google Cloud Function!");
        Console.WriteLine("You published the tweet : " + tweet);
    }
    async Task GetTimeline() {
        var userTimelineIterator = Client.Timelines.GetUserTimelineIterator("tweetinviapi");

        while (!userTimelineIterator.Completed) {
            var page = await userTimelineIterator.NextPageAsync();
            Console.WriteLine("Retrieved " + page.Count() + " tweets!");
        }

        Console.WriteLine("We have now retrieved all the tweets!");
    }
    async Task GetMentions() {
        var mentionsTimelineIterator = Client.Timelines.GetMentionsTimelineIterator();
        // var userTimelineIterator = client.Timelines.GetUserTimelineIterator("tweetinviapi");

        while (!mentionsTimelineIterator.Completed) {
            var page = await mentionsTimelineIterator.NextPageAsync();
            Console.WriteLine("Retrieved " + page.Count() + " mentions!");

            foreach (var item in page) {
                Console.WriteLine($"Mention contents: {item.FullText}");
            }
        }

        Console.WriteLine("We have now retrieved all the tweets!");
    }
}