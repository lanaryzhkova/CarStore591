using System;
using System.Collections.Generic;

#nullable disable

namespace CarStore591
{
    public partial class Client
    {
        public Client()
        {
            Sales = new HashSet<Sale>();
        }

        public int UserCode { get; set; }
        public int FullNameIndex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public virtual ClientFullName FullNameIndexNavigation { get; set; }
        public virtual CurrentBalance CurrentBalance { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
