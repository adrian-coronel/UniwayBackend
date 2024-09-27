﻿namespace UniwayBackend.Config
{
    public class AppSettings
    {
        // https://jwtsecret.com/generate -> 128 length
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

    }
}
