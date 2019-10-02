using System.Collections.Generic;

namespace RDPMonWebGUI.ViewModels
{
    public class RecordActionButton
    {
        public string Controller { get; set; } = "Home";

        public string Action { get; set; }

        public string CssClass { get; set; }

        public List<string> Attributes { get; set; } = new List<string>();
    }
}
