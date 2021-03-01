using Google.Cloud.Firestore;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using System.Collections.Generic;

public partial class Function : IHttpFunction {

    static string ProjectId => "alttextnetwork";

    static TwitterClient _client;
    static TwitterClient Client => _client ??= Twitter.GetClient(ProjectId).Result;

    static FirestoreDb _FirestoreDb;
    static FirestoreDb FirestoreDb => _FirestoreDb ??= FirestoreDb.Create(ProjectId);

    static List<long> RecentlyParsed = new List<long>();

    public async Task HandleAsync(HttpContext context) {
        context.Response.StatusCode = 200;
        await GeneralEntryPoint();
    }

    async Task GeneralEntryPoint() {
        var recentMentions = await GetMentions();
        var newMentions = await FilterMentions(recentMentions, FirestoreDb);
        WriteNewMentionsToDB(newMentions, FirestoreDb);
        return;
    }
}