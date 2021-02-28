using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Cloud.SecretManager.V1;

public static class GetSecret {
    static SecretManagerServiceClient GoogleSecretsClient;

    public static async Task<string> GetSecretAsync(string projectPath, string secretName) {
        if (null == GoogleSecretsClient) {
            //Create our way to retrieve Twitter API keys from google
            GoogleSecretsClient = await SecretManagerServiceClient.CreateAsync();
        }

        try {
            var secretsPath = $"projects/{projectPath}/secrets/{secretName}/versions/latest";
            // System.Console.WriteLine($"SecretsPAth: {secretsPath}");

            var result = await GoogleSecretsClient.AccessSecretVersionAsync(secretsPath);
            var secretData = result.Payload.Data.ToString(System.Text.Encoding.UTF8);
            return secretData;
        } catch (System.Exception) {
            System.Console.WriteLine("Error with accessing secrets");
            throw;
        }
    }
}