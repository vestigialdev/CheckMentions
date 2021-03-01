using Google.Events.Protobuf.Cloud.PubSub.V1;
using Google.Cloud.Functions.Framework;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Google.Cloud.Firestore;
using System.Threading.Tasks;
using Tweetinvi;
using CloudNative.CloudEvents;
using System.Threading;

namespace CheckMentionsEntry {
    public partial class HTTPHandler : IHttpFunction {
        public async Task HandleAsync(HttpContext context) {
            context.Response.StatusCode = 200;
            await CheckMentions.GeneralEntryPoint();
        }
    }
    public class Function : ICloudEventFunction<MessagePublishedData> {
        public Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken) {
            CheckMentions.GeneralEntryPoint().GetAwaiter().GetResult();
            return Task.CompletedTask;
        }
    }
}

public static partial class CheckMentions {
    static string ProjectId => "alttextnetwork";

    static TwitterClient _client;
    static TwitterClient Client => _client ??= Twitter.GetClient(ProjectId).Result;

    static FirestoreDb _FirestoreDb;
    static FirestoreDb FirestoreDb => _FirestoreDb ??= FirestoreDb.Create(ProjectId);

    static List<long> RecentlyParsed = new List<long>();



    public static async Task GeneralEntryPoint() {
        var recentMentions = await GetMentions();
        var newMentions = await FilterMentions(recentMentions, FirestoreDb);
        WriteNewMentionsToDB(newMentions, FirestoreDb);
        return;
    }
}