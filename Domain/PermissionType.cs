using System.ComponentModel.DataAnnotations;

namespace CQRS.Domain
{
    public class PermissionType : BaseEntity
    {
        [MaxLength(100)]
        public string Description { get; set; }
    }
}