using Google.Cloud.Firestore;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tweetinvi;

public partial class Function : IHttpFunction {

    static string ProjectId => "alttextnetwork";

    static TwitterClient _client;
    static TwitterClient Client => _client ??= Twitter.GetClient(ProjectId).Result;

    static FirestoreDb _FirestoreDb;
    static FirestoreDb FirestoreDb => _FirestoreDb ??= FireStore.GetClient(ProjectId);

    public async Task HandleAsync(HttpContext context) {
        await GeneralEntryPoint();
        context.Response.StatusCode = 200;
    }

    async Task GeneralEntryPoint() {
        // await PrintUsername();
        // await PostTweet();
        // await GetTimeline();
        await GetMentions();
        return;
    }
}