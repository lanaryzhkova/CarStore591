using System;
using System.Collections.Generic;

#nullable disable

namespace CarStore591
{
    public partial class CreditingDebiting
    {
        public int UserCode { get; set; }
        public int CreditingDebiting1 { get; set; }
        public DateTime Date { get; set; }

        public virtual Client UserCodeNavigation { get; set; }
    }
}
