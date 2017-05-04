﻿using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Microsoft.Owin.Security;

namespace BookStore.Models.ViewModels.Manage
{
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
}
