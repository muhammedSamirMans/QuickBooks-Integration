using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Domain.Common
{
    public class ApiResponse
    {
        public QueryResponse CompanyInfo { get; set; } // Updated to match the JSON structure
        public DateTime Time { get; set; }
    }

    public class QueryResponse
    {
        public string CompanyName { get; set; }
        public string LegalName { get; set; }
        public Address CompanyAddr { get; set; }
        public Address CustomerCommunicationAddr { get; set; }
        public Address LegalAddr { get; set; }
        public Email CustomerCommunicationEmailAddr { get; set; }
        public object PrimaryPhone { get; set; }
        public string CompanyStartDate { get; set; }
        public string FiscalYearStartMonth { get; set; }
        public string Country { get; set; }
        public Email Email { get; set; }
        public object WebAddr { get; set; }
        public string SupportedLanguages { get; set; }
        public string DefaultTimeZone { get; set; }
        public List<NameValue> NameValue { get; set; }
        public string Domain { get; set; }
        public bool Sparse { get; set; }
        public string Id { get; set; }
        public string SyncToken { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class Address
    {
        public string Id { get; set; }
        public string Line1 { get; set; }
        public string City { get; set; }
        public string CountrySubDivisionCode { get; set; }
        public string PostalCode { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }

    public class Email
    {
        public string Address { get; set; }
    }

    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class MetaData
    {
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
