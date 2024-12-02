namespace UniwayBackend.Config
{
    public class AwsS3Config
    {
        // CREDENTIALS AWS
        public string AwsAccessKey { get; set; } = string.Empty;
        public string AwsSecretAccessKey { get; set; } = string.Empty;
        public string AwsSessionToken { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
