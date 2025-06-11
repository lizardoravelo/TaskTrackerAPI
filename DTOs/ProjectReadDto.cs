namespace TaskTrackerAPI.DTOs
{
    public class ProjectReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
