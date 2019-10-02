using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDPMonWebGUI.Models
{
    public class Session : IModel
    {
        [NotMapped]
        public long _id { get; set; }

        [NotMapped]
        public object Id
        {
            get
            {
                return _id;
            }
        }

        [NotMapped]
        public long Flags { get; set; }

        [DisplayName("Started")]
        public DateTime Start { get; set; }

        public string User { get; set; }

        [DisplayName("Session ID")]
        public long WtsSessionId { get; set; }

    }
}
