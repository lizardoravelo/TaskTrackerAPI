namespace TaskTrackerAPI.DTOs
{
    public class TaskUpdateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } = "Todo";
        public DateTime? DueDate { get; set; }
    }
}
