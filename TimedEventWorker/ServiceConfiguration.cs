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

        public string Host { get; set; }

        public ushort Port { get; set; }

        public string VirtualHost { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

    }
}
