namespace HR.LeaveManagement.Application.Models.Email
{
    public class EmailMessage
    {
        public required List<string> To { get; set; }

        public string? Subject { get; set; }

        public required string Body { get; set; }
    }

}
