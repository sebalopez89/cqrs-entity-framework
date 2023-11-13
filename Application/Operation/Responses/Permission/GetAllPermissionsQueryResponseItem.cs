namespace CQRS.Application.Operation.Responses.Permission
{
    public class GetAllPermissionsQueryResponseItem
    {
        public GetAllPermissionsQueryResponseItem(
            int id,
            string employeeForename,
            string employeeSurname,
            int permissionTypeid,
            string permissionTypeName
        )
        {
            Id = id;
            EmployeeForename = employeeForename;
            EmployeeSurname = employeeSurname;
            PermissionTypeid = permissionTypeid;
            PermissionTypeName = permissionTypeName;
        }
        public int Id { get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeid { get; set; }
        public string PermissionTypeName { get; set; }
    }
}
