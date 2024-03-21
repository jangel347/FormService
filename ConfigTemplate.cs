using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormService
{
    public class ConfigTemplate
    {
        public ConfigTemplate() { }
        public string time { get; set; }
        public string[] days { get; set; }
        public string path { get; set; }
        public DataTemplate[] data { get; set; } = new DataTemplate[0]; // Initialized empty array
    }
}
