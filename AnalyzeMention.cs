using Tweetinvi.Models;
using Tweetinvi;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public static class AnalyzeMention {

    static string TwitterImageHostRegexPattern = @"(https?:\/\/pbs.twimg.com\/media\/([a-zA-Z0-9_]){15}.(png|gif|jpg))";
    static int Offset => "http://pbs.twimg.com/media/".Length;

    async public static void Analyze(ITweet tweet) {
        System.Console.WriteLine($"Analyzing {tweet.IdStr}");

        var isAReply = await IsMentionReplyingToMedia(tweet);
        // await isAReply;

        if (!isAReply) {
            //User error
            return;
        }

        if (!IsMentionedMediaNew(tweet)) {
            //Inform the user by replying with the description
            return;
        }
    }

    async static Task<bool> IsMentionReplyingToMedia(ITweet tweet) {

        //Is this even a reply?
        if (null == tweet.InReplyToStatusIdStr) {
            System.Console.WriteLine($"{tweet.IdStr} is not a reply");
            return false;
        }

        //Is the thing this is a reply to, containing media?
        // var result = await tweet.Client.TweetsV2.GetTweetAsync(tweet.InReplyToStatusIdStr);

        foreach (var item in tweet.Entities.Medias) {
            if (string.Compare("photo", item.MediaType, true) == 0) {
                System.Console.WriteLine($"{tweet.IdStr} media entity url {item.MediaURL}");

                //Ensure its  hosted by twitter
                if (!Regex.IsMatch(item.MediaURL, TwitterImageHostRegexPattern)) {
                    System.Console.WriteLine($"Non-twitter hosted image? {item.MediaURL}");
                    return false;
                }

                var imageKey = item.MediaURL.Substring(Offset, 15);
                System.Console.WriteLine($"Image key: {imageKey}");
            }
        }

        return true;
    }

    static bool IsMentionedMediaNew(ITweet tweet) {
        return true;
    }
}