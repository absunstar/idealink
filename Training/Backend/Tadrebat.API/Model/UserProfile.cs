using System;
using System.Collections.Generic;

namespace SecuringAngularApps.API.Model
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        //public List<UserPermission> UserPermissions { get; set; }
        public string Role { get; set; }
    }
}
