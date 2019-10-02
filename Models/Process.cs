using System.ComponentModel.DataAnnotations.Schema;

namespace RDPMonWebGUI.Models
{
    public class Process : IModel
    {
        [NotMapped]
        public byte[] _id { get; set; }

        [NotMapped]
        public object Id
        {
            get
            {
                return _id;
            }
        }

        [NotMapped]
        public byte HashType { get; set; }

        [NotMapped]
        public ExecInfo[] ExecInfos { get; set; }

        [NotMapped]
        public long Flags { get; set; }

        public string Path { get; set; }
    }
}
