using System;
using System.Collections.Generic;

namespace task_one_v2.Models;

public partial class Chatroom
{
    public decimal Chatroomid { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Chefid { get; set; }

    public string? Messagetext { get; set; }

    public DateTime? Times { get; set; }

    public virtual UserInfo? Chef { get; set; }

    public virtual UserInfo? User { get; set; }
}
