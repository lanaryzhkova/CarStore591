using System;
using System.Collections.Generic;

#nullable disable

namespace CarStore591
{
    public partial class Car
    {
        public Car()
        {
            Sales = new HashSet<Sale>();
        }

        public int CarId { get; set; }
        public string CarName { get; set; }
        public decimal CarCost { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
