using Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration
{
    /// <summary>
    /// Register the configuration for each game.
    /// </summary>
    public static class ConfigurationRegistration
    {
        /// <summary>
        /// Register Coinche section from appsettings.json
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddCoincheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var coincheConfiguration = new CoincheConfiguration();
            configuration.GetSection(CoincheConfiguration.ConfigurationKey).Bind(coincheConfiguration);

            services.AddSingleton(coincheConfiguration);
        }
    }
}
