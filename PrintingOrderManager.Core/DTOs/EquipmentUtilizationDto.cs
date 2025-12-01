public class EquipmentUtilizationDto
{
    public int EquipmentId { get; set; }
    public string EquipmentName { get; set; } = null!;
    public int TotalTasks { get; set; } // количество позиций
    public int TotalItems { get; set; } // сумма Quantity
}