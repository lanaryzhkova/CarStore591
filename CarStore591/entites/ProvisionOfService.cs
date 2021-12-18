using System;
using System.Collections.Generic;

#nullable disable

namespace CarStore591
{
    public partial class ProvisionOfService
    {
        public int UserCode { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }

        public virtual CarService NumberNavigation { get; set; }
        public virtual Client UserCodeNavigation { get; set; }
    }
}
