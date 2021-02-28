// using System.Threading;
// using System.Threading.Tasks;
// using CloudNative.CloudEvents;
// using Google.Cloud.Functions.Framework;
// using Google.Events.Protobuf.Cloud.PubSub.V1;
// using Microsoft.AspNetCore.Http;
// using Tweetinvi;

// namespace CheckMentions {
//     public class Incoming : IHttpFunction {

//         TwitterClient client;
//         public Task HandleAsync(HttpContext context) {
//             var g = new GetMentionsFromTwitter();
//             // await g.Go();
//             return Task.CompletedTask;
//         }
//         string ProjectId => "alttextnetwork";

//         async Task GeneralEntryPoint() {

//             // var ConsumerKey = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_CONSUMER_ID");
//             // var ConsumerSecret = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_CONSUMER_SECRET");
//             // var BearerToken = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_BEARER_TOKEN");
//             // var AccessToken = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_USER_ACCESS_TOKEN");
//             // var AccessTokenSecret = await GetSecret.GetSecretAsync(ProjectId, "TWITTER_USER_ACCESS_TOKEN_SECRET");

//             // client = new TwitterClient(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);

//             // await PrintUsername();
//             // await PostTweet();
//             // await GetTimeline();
//             // await GetMentions();
//             return;
//         }
//     }
//     // public class Incoming : ICloudEventFunction<MessagePublishedData> {

//     //     public Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken) {
//     //         var g = new GetMentionsFromTwitter();
//     //         g.Go();
//     //         return Task.CompletedTask;
//     //     }
//     // }
// }