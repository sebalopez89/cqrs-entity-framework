using System.Collections.Generic;

namespace CQRS.Application.Operation.Responses
{
    public class BaseCommandResponse
    {
        public BaseCommandResponse(int id) => Id = id;
        public int Id { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
