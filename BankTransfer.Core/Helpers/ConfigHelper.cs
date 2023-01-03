using BankTransfer.Core.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Helpers
{
    public sealed class ConfigHelper
    {
        private readonly IConfiguration _config;

        public ConfigHelper(IConfiguration config)
        {
            _config = config;
        }

        public string GetBaseUrl(ProviderType? providerType)
        {
            switch (providerType)
            {
                case ProviderType.PAYSTACK:
                    return _config.GetSection("Paystack").GetSection("BaseUrl").Value ?? string.Empty;

                case ProviderType.FLUTTERAVE:
                    return _config.GetSection("Flutterwave").GetSection("BaseUrl").Value ?? string.Empty;

                default: return _config.GetSection("Paystack").GetSection("BaseUrl").Value ?? string.Empty;
            }
        }

        public string GetSecreteKey(ProviderType? providerType)
        {
            switch (providerType)
            {
                case ProviderType.PAYSTACK:
                    return _config.GetSection("Paystack").GetSection("secreteKey").Value ?? String.Empty;

                case ProviderType.FLUTTERAVE:
                    return _config.GetSection("Flutterwave").GetSection("secreteKey").Value ?? String.Empty;

                default: return _config.GetSection("Paystack").GetSection("secreteKey").Value ?? String.Empty;
            }
        }

        public string GetDefaultProvider()
        {
            return _config.GetSection("Provider").GetSection("DefaultProvider").Value ?? string.Empty;
        }
    }
}
