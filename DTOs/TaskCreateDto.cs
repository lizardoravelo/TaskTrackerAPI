namespace TaskTrackerAPI.DTOs
{
    public class TaskCreateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } = "Todo";
        public DateTime? DueDate { get; set; }
    }
}
