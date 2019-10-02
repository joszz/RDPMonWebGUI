using System;

namespace RDPMonWebGUI.Models
{
    public class ExecInfo
    {
        public long SessionId { get; set; }
        public DateTime Start { get; set; }
        public UInt32 Pid { get; set; }
        public UInt32 ParentPid { get; set; }
        public long Flags { get; set; }
    }
}
