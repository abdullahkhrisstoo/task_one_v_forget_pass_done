using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_one_v2.Models;

public partial class Recipe
{

    public decimal RecipeId { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [DisplayName("Title")]
    public string Recipename { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    [DisplayName("Description")]
    public string Description { get; set; } = null!;


    [Required(ErrorMessage = "Ingredients is required")]
    [DisplayName("Ingredients")]
    public string Ingredients { get; set; } = null!;


    [DisplayName("Steps")]
    public string? Procedure { get; set; }




    public decimal ChefId { get; set; }



    [Required(ErrorMessage = "Category is required")]
    [DisplayName("Category")]
    public decimal CategoryId { get; set; }

    [DisplayName("Status")]
    public decimal ApprovalStatusId { get; set; }


    [Required(ErrorMessage = "Price is required")]
    [DisplayName("Price")]
    //[MaxLength(4)]
    public decimal Price { get; set; }


    [DisplayName("CreationDate")]
    public DateTime CreationDate { get; set; }





    public string? ImageRecipe { get; set; }
    [NotMapped]

    [Required(ErrorMessage = "Recipe Image is required")]
    [DisplayName("Recipe Image")]
    public IFormFile? ImageRecipeFile { get; set; } /*= null!;*/





    public virtual RecipeApprovalStatus? ApprovalStatus { get; set; }/* = null!;*/




    public virtual Category? Category { get; set; } /*= null!;*/

    public virtual UserInfo? Chef { get; set; } /*= null!;*/

    public virtual ICollection<RecipeOrder> RecipeOrders { get; set; } = new List<RecipeOrder>();

}




