using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Infrastructure.Configuration
{
    public class QuickBooksSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
        public string BaseUrl { get; set; }
        public string Environment { get; set; }
        public string RealmId { get; set; }
    }
}
