using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.BaseEntity
{
    public abstract class Product
    {
        public string ProductId { get; set; }
        public Process Process { get; set; }    
    }
}
