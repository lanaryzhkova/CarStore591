using System;
using System.Collections.Generic;

#nullable disable

namespace CarStore591
{
    public partial class Sale
    {
        public int SaleId { get; set; }
        public int UserCode { get; set; }
        public int CarId { get; set; }
        public DateTime Date { get; set; }

        public virtual Car Car { get; set; }
        public virtual Client UserCodeNavigation { get; set; }
    }
}
