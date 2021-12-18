using System;
using System.Collections.Generic;

#nullable disable

namespace CarStore591
{
    public partial class ClientFullName
    {
        public ClientFullName()
        {
            Clients = new HashSet<Client>();
        }

        public int NameIndex { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int UserCode { get; set; }
        public virtual Client UserCodeNavigation { get; set; }


        public virtual ICollection<Client> Clients { get; set; }
    }
}
