using System.Collections.Generic;

namespace CQRS.Application.Operation.Responses.Permission
{
    public class GetAllPermissionsQueryResponse
    {
        public List<GetAllPermissionsQueryResponseItem> Items { get; set; }
    }
}
