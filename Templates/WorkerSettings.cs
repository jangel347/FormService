using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormService.Templates
{
    public class WorkerSettings
    {
        public string time_limit1 { get; set; }
        public string time_limit2 { get; set; }
        public string submit_button { get; set; }
        public DataInput[] data { get; set; }
        public Element[] elements { get; set; }
    }
}
