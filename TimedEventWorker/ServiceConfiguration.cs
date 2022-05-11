using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedEvents
{
    internal class ServiceConfiguration
    {
        public const string ConfigurationKey = "RabbitMq";

        public string Host { get; set; } = String.Empty;

        public ushort Port { get; set; }

        public string VirtualHost { get; set; } = String.Empty;

        public string User { get; set; } = String.Empty;

        public string Password { get; set; } = String.Empty;

    }
}
