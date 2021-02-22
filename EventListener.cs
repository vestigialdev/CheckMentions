#define LOCAL
using System.Threading;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;

#if LOCAL
namespace CheckMentions {
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Text.Json;
    public class LocalIncoming : IHttpFunction {

        public class LocalCloudEvent {
            public LocalMessage message { get; set; }
            public string subscription { get; set; }
        }

        public class LocalMessage {
            public string data { get; set; }
            public string messageId { get; set; }
            public string message_id { get; set; }
            public DateTime publishTime { get; set; }
            public DateTime publish_time { get; set; }
        }
        public async Task HandleAsync(HttpContext context) {

            GetMentionsFromTwitter.Go();

            // using (var inputStream = new StreamReader(context.Request.BodyReader.AsStream())) {
            //     var json = inputStream.ReadToEnd();
            //     var cloudEvent = JsonSerializer.Deserialize<LocalCloudEvent>(json);
            //     System.Console.WriteLine($"Message: {json}");
            //     GetMentionsFromTwitter.Go();
            //     await context.Response.WriteAsync("ack");
            // }

            return;
        }
    }
}
#else
namespace CheckMentions {
    public class Incoming : ICloudEventFunction<MessagePublishedData> {
        public Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken) {
            GetMentionsFromTwitter.Go();
            return Task.CompletedTask;
        }
    }
}
#endif
