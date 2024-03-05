namespace ToDo_List.Models.API.Responses
{
    public class ErrorResponse
    {
        public string ErrorMessage { get; set; }
        public int Code { get; set; }
        public object Body { get; set; }
    }
}
