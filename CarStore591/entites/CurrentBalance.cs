using System;
using System.Collections.Generic;

#nullable disable

namespace CarStore591
{
    public partial class CurrentBalance
    {
        public int UserCode { get; set; }
        public int CurrentBalance1 { get; set; }

        public virtual Client UserCodeNavigation { get; set; }
    }
}
