﻿using OpenQA.Selenium.DevTools.V120.FedCm;
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
        public int time_to_wait { get; set; }
        public string url { get; set; }
        public string submit_button { get; set; }
        public string save_button { get; set; }
        public Account account { get; set; }
        public DataInput[] data { get; set; }
        public Element[] elements { get; set; }
    }
}
