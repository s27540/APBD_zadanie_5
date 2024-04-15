using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Animal;

public class CreateAnimalDTO
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }
}