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
            await CheckMentions.GeneralEntryPoint(true);
        }
    }
    public class CloudFunction : ICloudEventFunction<MessagePublishedData> {
        public async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken) {
            bool verbose = false;
            if (data.Message.Attributes != null) {

                //Clear local mentions cache?
                if (data.Message.Attributes.ContainsKey("clearCache")) {
                    System.Console.WriteLine("Clearing recently parsed local cache");
                    CheckMentions.RecentlyParsed.Clear();
                }

                //Set log level
                if (data.Message.Attributes.ContainsKey("verbose")) {
                    System.Console.WriteLine("Setting verbose logging");
                    verbose = true;
                }
            }

            await CheckMentions.GeneralEntryPoint(verbose);
            return;
        }
    }
}