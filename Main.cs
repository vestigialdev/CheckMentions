using System.Collections.Generic;
using Google.Cloud.Firestore;
using System.Threading.Tasks;
using Tweetinvi;

public static partial class CheckMentions {
    static string ProjectId => "alttextnetwork";

    static TwitterClient _client;
    static TwitterClient Client => _client ??= Twitter.GetClient(ProjectId).Result;

    static FirestoreDb _FirestoreDb;
    static FirestoreDb FirestoreDb => _FirestoreDb ??= FirestoreDb.Create(ProjectId);

    public static List<long> RecentlyParsed = new List<long>();


    static bool VerboseDebugging = false;

    static void Print(string s, bool force = false) {
        if (VerboseDebugging || force) {
            System.Console.WriteLine(s);
        }
    }

    public static async Task GeneralEntryPoint(bool verboseDebugging = false) {
        VerboseDebugging = verboseDebugging;


        Print("GeneralEntryPoint");
        var recentMentions = await GetMentions();
        var newMentions = await FilterMentions(recentMentions, FirestoreDb);
        WriteNewMentionsToDB(newMentions, FirestoreDb);
        return;
    }
}