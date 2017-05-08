using System.ComponentModel.DataAnnotations.Schema;
using RTDemoProject.Shared.Enums;

namespace RTDemoProject.Entities.POCOs
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}