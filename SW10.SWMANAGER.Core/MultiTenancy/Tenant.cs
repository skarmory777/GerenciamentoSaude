﻿using Abp.MultiTenancy;
using SW10.SWMANAGER.Authorization.Users;

namespace SW10.SWMANAGER.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant in the system.
    /// A tenant is a isolated customer for the application
    /// which has it's own users, roles and other application entities.
    /// </summary>
    public class Tenant : AbpTenant<User>
    {
        //Can add application specific tenant properties here

        protected Tenant()
        {

        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {

        }
    }
}