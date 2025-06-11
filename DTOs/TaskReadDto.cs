namespace TaskTrackerAPI.DTOs
{
    public class TaskReadDto
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string Title { get; set; } = null;
        public string? Description { get; set; }
        public string Status { get; set; } = null;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
    }
}
