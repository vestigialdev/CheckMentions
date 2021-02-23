using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public static class AnalyzeMention {

    static string TwitterImageHostRegexPattern = @"(https?:\/\/pbs.twimg.com\/media\/([a-zA-Z0-9_]){15}.(png|gif|jpg))";
    static int Offset => "https://pbs.twimg.com/media/".Length;

    async public static void Analyze(ITweet tweet) {

        if (ContainsValidMedia(tweet)) {
            ParseMedia(tweet);
        } else if (IsAReply(tweet)) {
            var parentTweet = GetReplyParent(tweet);
            Analyze(parentTweet.Result);
        }

    }
    async static Task<ITweet> GetReplyParent(ITweet tweet) {
        var result = await tweet.Client.Tweets.GetTweetAsync((long)tweet.InReplyToStatusId);
        return result;
    }
    static void ParseMedia(ITweet tweet) {
        foreach (var item in tweet.Entities.Medias) {
            if (string.Compare("photo", item.MediaType, true) == 0) {
                // System.Console.WriteLine($"{tweet.IdStr} media entity url {item.MediaURLHttps}");

                //Ensure its  hosted by twitter
                if (!Regex.IsMatch(item.MediaURL, TwitterImageHostRegexPattern)) {
                    System.Console.WriteLine($"Non-twitter hosted image? {item.MediaURLHttps}");
                    return;
                }

                var imageKey = item.MediaURL.Substring(Offset, 15);
                System.Console.WriteLine($"Image key: {imageKey}");
            }
        }
    }
    static bool ContainsValidMedia(ITweet tweet) {
        foreach (var item in tweet.Entities.Medias) {
            if (string.Compare("photo", item.MediaType, true) == 0) {
                // System.Console.WriteLine($"{tweet.IdStr} media entity url {item.MediaURLHttps}");

                //Ensure its  hosted by twitter
                if (!Regex.IsMatch(item.MediaURL, TwitterImageHostRegexPattern)) {
                    // System.Console.WriteLine($"Non-twitter hosted image? {item.MediaURLHttps}");
                    return false;
                }

                var imageKey = item.MediaURL.Substring(Offset, 15);
                // System.Console.WriteLine($"Image key: {imageKey}");
                return true;
            }
        }

        return false;
    }
    static bool IsAReply(ITweet tweet) {
        return null != tweet.InReplyToStatusIdStr;
    }
}