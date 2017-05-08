using System.ComponentModel.DataAnnotations.Schema;
using RTDemoProject.Shared.Enums;

namespace RTDemoProject.Entities
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}