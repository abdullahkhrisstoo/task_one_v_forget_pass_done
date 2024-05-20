using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_one_v2.Models;

public partial class Category
{
    public decimal CategoryId { get; set; }
    [Required(ErrorMessage = "Category is required")]
    [DisplayName("Category")]
    public string CategoryName { get; set; } = null!;

    public string? ImageCategory { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "category img required")]
    [DisplayName("category img")]
    public IFormFile FormFile { get; set; }
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
