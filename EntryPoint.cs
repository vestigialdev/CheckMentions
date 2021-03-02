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
            System.Console.WriteLine("HTTP entrypoint");
            await CheckMentions.GeneralEntryPoint();
        }
    }
    public class CloudFunction : ICloudEventFunction<MessagePublishedData> {
        public async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken) {
            System.Console.WriteLine("CloudEvent entrypoint");

            if (data.Message.Attributes != null) {
                System.Console.WriteLine($"Has {data.Message.Attributes.Count} attributes");

                if (data.Message.Attributes.ContainsKey("clearCache")) {
                    System.Console.WriteLine("Clearing recently parsed local cache");
                    CheckMentions.RecentlyParsed.Clear();
                } else {
                    System.Console.WriteLine("Clear cache attribute not detected");
                }
            }

            await CheckMentions.GeneralEntryPoint();
            return;
        }
    }
}