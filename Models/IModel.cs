using System.ComponentModel.DataAnnotations.Schema;

namespace RDPMonWebGUI.Models;

public interface IModel
{
    [NotMapped]
    public object Id { get; }
}
