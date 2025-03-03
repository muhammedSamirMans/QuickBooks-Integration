using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Domain.Entities
{
    public class QBAuth: BaseEntity
    { 
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string RealmId { get; set; }
        public bool IsExpired { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}
