using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace task_one_v2.Models;

public partial class UserVerificationStatus
{

    [DisplayName("Approval Status")]
    public decimal VerificationStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
