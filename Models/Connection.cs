using RDPMonWebGUI.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RDPMonWebGUI.Models
{
    public class Connection : IModel
    {
        [DisplayName("IP")]
        public string _id { get; set; }

        [NotMapped]
        [JsonIgnore]
        public object Id
        {
            get
            {
                return _id;
            }
        }

        [NotMapped]
        [JsonIgnore]
        public long Type { get; set; }

        [NotMapped]
        [JsonIgnore]
        public long Flags { get; set; }

        [NotMapped]
        [JsonIgnore]
        public long Prot { get; set; }

        [DisplayName("Failures")]
        public int FailCount { get; set; }

        [DisplayName("Success")]
        public int SuccessCount { get; set; }

        [DisplayName("First attempt")]
        public DateTime First { get; set; }

        [DisplayName("Last attempt")]
        public DateTime Last { get; set; }

        [DisplayName("Logins")]
        [DisableSort]
        public string[] UserNames { get; set; }
    }
}
