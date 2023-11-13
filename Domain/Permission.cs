using System;
using System.ComponentModel.DataAnnotations;

namespace CQRS.Domain
{
    public class Permission : BaseEntity
    {
        [MaxLength(100)]
        public string EmployeeForename { get; set; }

        [MaxLength(100)]
        public string EmployeeSurname { get; set; }

        public PermissionType PermissionType { get; set; }

        public DateTime PermissionDate { get; set; }
    }
}