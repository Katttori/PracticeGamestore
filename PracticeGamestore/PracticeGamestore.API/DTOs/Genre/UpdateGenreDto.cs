using System.ComponentModel.DataAnnotations;

namespace PracticeGamestore.DTOs.Genre;

public class UpdateGenreDto
{
    [Required]
    [MinLength(2), MaxLength(50)]
    public required string Name { get; set; }
    
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}