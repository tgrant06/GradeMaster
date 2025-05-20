using System.ComponentModel.DataAnnotations;

namespace GradeMaster.Common.Entities;

public class Note
{
    public int Id
    {
        get; set;
    }

    [Required]
    [MaxLength(255, ErrorMessage = "Title may not exceed 255 characters.")]
    public string Title
    {
        get; set;
    }

    [MaxLength(10_000, ErrorMessage = "Content may not exceed 10000 characters.")]
    public string? Content
    {
        get; set;
    }

    //[MaxLength(1020, ErrorMessage = "Tags may not exceed a combined string length of 1020.")]
    //public string? Tags
    //{
    //    get; set;
    //}

    // alternatively add a Short description

    [Required]
    public DateTime CreatedAt
    {
        get; set;
    }

    public DateTime UpdatedAt
    {
        get; set;
    }

    [Required]
    public bool IsPinned
    {
        get; set;
    }

    [Required]
    public bool IsArchived
    {
        get; set;
    }

    [Required]
    public Color Color { get; set; }
    public int ColorId { get; set; }
}