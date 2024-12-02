using Amazon.S3;
using Amazon.SecurityToken.Model;
using Amazon.SecurityToken;
using Microsoft.Extensions.Options;
using UniwayBackend.Config;
using Amazon;

namespace UniwayBackend.Helpers
{
    public class AwsCredentialsManager
    {
        private readonly AwsS3Config _awsS3Config;
        private Credentials _currentCredentials;
        private DateTime _tokenExpiryTime;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // Para evitar solicitudes concurrentes

        public AwsCredentialsManager(IOptions<AwsS3Config> appSettings)
        {
            _awsS3Config = appSettings.Value;
        }

        public async Task<(AmazonS3Client, string)> GetS3ClientAsync()
        {
            // Verificar si las credenciales necesitan renovarse
            if (_currentCredentials == null || DateTime.UtcNow >= _tokenExpiryTime.AddMinutes(-5))
            {
                await RenewCredentialsAsync();
            }

            // Retornar el cliente S3 y el nombre del bucket
            return
            (
                new AmazonS3Client(
                    _currentCredentials.AccessKeyId,
                    _currentCredentials.SecretAccessKey,
                    _currentCredentials.SessionToken,
                    RegionEndpoint.GetBySystemName(_awsS3Config.Region)),
                _awsS3Config.BucketName
            );
        }

        private async Task RenewCredentialsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_currentCredentials == null || DateTime.UtcNow >= _tokenExpiryTime.AddMinutes(-5))
                {
                    var stsClient = new AmazonSecurityTokenServiceClient(
                        _awsS3Config.AwsAccessKey,
                        _awsS3Config.AwsSecretAccessKey,
                        RegionEndpoint.GetBySystemName(_awsS3Config.Region)
                    );

                    var sessionTokenResponse = await stsClient.GetSessionTokenAsync(new GetSessionTokenRequest
                    {
                        DurationSeconds = 3600
                    });

                    _currentCredentials = sessionTokenResponse.Credentials;
                    _tokenExpiryTime = _currentCredentials.Expiration;
                }
            }
            catch (AmazonSecurityTokenServiceException ex)
            {
                Console.WriteLine($"AWS STS Error: {ex.Message}");
                Console.WriteLine($"Error Code: {ex.ErrorCode}");
                Console.WriteLine($"Request ID: {ex.RequestId}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

    }

}
