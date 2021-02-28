using System.Threading;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using Microsoft.AspNetCore.Http;

namespace CheckMentions {
    public class Incoming : IHttpFunction {

        public Task HandleAsync(HttpContext context) {
            var g = new GetMentionsFromTwitter();
            g.Go();
            return Task.CompletedTask;
        }
    }
    // public class Incoming : ICloudEventFunction<MessagePublishedData> {

    //     public Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken) {
    //         var g = new GetMentionsFromTwitter();
    //         g.Go();
    //         return Task.CompletedTask;
    //     }
    // }
}