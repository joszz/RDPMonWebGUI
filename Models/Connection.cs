using System;

namespace RDPMonWebGUI.Models
{
    public class Connection
    {
        public string _id { get; set; }
        public string[] UserNames { get; set; }
        public DateTime Last { get; set; }
        public DateTime First{ get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }

    }
}
