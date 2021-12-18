using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore591
{
    class Command
    {
        public string command { get; set; }
        public int steps { get; set; }
        public int counter { get; set; }

        public Command()
        {

        }

        public Command((string command, int counter, int steps) tuple)
        {
            this.command = tuple.command;
            this.steps = tuple.steps;
            this.counter = tuple.counter;
        }
    }
}
