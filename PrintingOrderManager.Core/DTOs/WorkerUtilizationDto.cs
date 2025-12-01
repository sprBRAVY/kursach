public class WorkerUtilizationDto
{
    public int WorkerId { get; set; }
    public string WorkerFullName { get; set; } = null!;
    public int TotalTasks { get; set; }
    public int TotalItems { get; set; }
}