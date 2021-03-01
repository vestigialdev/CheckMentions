using Google.Cloud.SecretManager.V1;
using System.Threading.Tasks;

public static class GoogleSecrets {

    static SecretManagerServiceClient _Client;
    static SecretManagerServiceClient Client => _Client ??= SecretManagerServiceClient.CreateAsync().Result;

    public static async Task<string> GetSecretAsync(string projectId, string secretName) {

        try {
            var secretsPath = $"projects/{projectId}/secrets/{secretName}/versions/latest";
            System.Console.WriteLine($"GoogleSecrets getting: {secretsPath}");

            var result = await Client.AccessSecretVersionAsync(secretsPath);
            var secretData = result.Payload.Data.ToString(System.Text.Encoding.UTF8);
            return secretData;
        } catch (System.Exception e) {
            System.Console.WriteLine("Error accessing Google Secrets");
            throw e;
        }
    }
}