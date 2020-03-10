using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeytecAdministración.Models
{
    public class IndexViewModel : BaseModelo
    {
       // public List<Machines> Machiness { get; set; }
       // public List<EstadoDispositivos> EstDispositivos { get; set; }
        public List<TablaMaquinas> tablaMaquinas { get; set; }
    }
}
