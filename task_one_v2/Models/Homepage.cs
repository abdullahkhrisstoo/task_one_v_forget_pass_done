using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace task_one_v2.Models;

public partial class Homepage
{
    public decimal Homepageid { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Backgroundimage { get; set; }

    public string? Avatarimage { get; set; }

    [NotMapped]
    [DisplayName("avatar image")]
    public IFormFile? AvatarImageFile { get; set; }
    [NotMapped]
    [DisplayName("background image")]
    public IFormFile? BackgroundImageFile { get; set; }

}
