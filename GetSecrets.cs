
using Google.Cloud.SecretManager.V1;

public static class GetSecrets {

    static SecretManagerServiceClient _GoogleSecretsClient;
    static SecretManagerServiceClient GoogleSecretsClient {
        get {
            if (null == _GoogleSecretsClient) {
                _GoogleSecretsClient = SecretManagerServiceClient.Create();
            }
            return _GoogleSecretsClient;
        }
    }

    public static string GetSecret(string projectID, string secretName) {
        try {
            var result = GoogleSecretsClient.AccessSecretVersion($"projects/{projectID}/secrets/{secretName}/versions/latest");
            var secretData = result.Payload.Data.ToString(System.Text.Encoding.UTF8);
            return secretData;
        } catch (System.Exception) {
            System.Console.WriteLine("Error with accessing secrets");
            throw;
        }
    }
}