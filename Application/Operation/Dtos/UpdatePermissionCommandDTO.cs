using System.ComponentModel.DataAnnotations;

namespace CQRS.Application.Operation.Dtos
{
    public class UpdatePermissionCommandDTO
    {
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string? EmployeeForename { get; set; }
        
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string? EmployeeSurname { get; set; }

        public int? PermissionTypeId { get; set; }
    }
}
