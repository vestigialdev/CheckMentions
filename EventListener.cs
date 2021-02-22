using System.Threading;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;


namespace CheckMentions {
    public class Incoming : ICloudEventFunction<MessagePublishedData> {

        public Incoming() {
            //If running locally, perform the mentions-getting early, 
            //because  there will be a failure before the true HandleAsync method is called
            if (string.Compare("true", System.Environment.GetEnvironmentVariable("LOCAL")) == 0) {
                GetMentionsFromTwitter.Go();
            }
        }
        public Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken) {
            GetMentionsFromTwitter.Go();
            return Task.CompletedTask;
        }
    }
}