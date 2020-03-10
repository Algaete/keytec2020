using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class Acholiday
    {
        public DateTime? Begindate { get; set; }
        public DateTime? Enddate { get; set; }
        public int? Holidayid { get; set; }
        public int Primaryid { get; set; }
        public int? Timezone { get; set; }
    }
}
