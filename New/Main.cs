using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Threading;
using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Tweetinvi;

public partial class Function : IHttpFunction {
    string ProjectId => "alttextnetwork";
    TwitterClient client;
    public async Task HandleAsync(HttpContext context) {
        await GeneralEntryPoint();
        await context.Response.WriteAsync("");
    }

    async Task GeneralEntryPoint() {

        var ConsumerKey = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_CONSUMER_ID");
        var ConsumerSecret = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_CONSUMER_SECRET");
        var BearerToken = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_BEARER_TOKEN");
        var AccessToken = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_USER_ACCESS_TOKEN");
        var AccessTokenSecret = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_USER_ACCESS_TOKEN_SECRET");

        client = new TwitterClient(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);

        // await PrintUsername();
        // await PostTweet();
        // await GetTimeline();
        await GetMentions();
        return;
    }
}