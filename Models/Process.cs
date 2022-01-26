using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RDPMonWebGUI.Models;

public class Process : IModel
{
    [NotMapped]
    public byte[] _id { get; set; }

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
    public byte HashType { get; set; }

    [NotMapped]
    [JsonIgnore]
    public ExecInfo[] ExecInfos { get; set; }

    [NotMapped]
    [JsonIgnore]
    public long Flags { get; set; }

    public string Path { get; set; }
}
