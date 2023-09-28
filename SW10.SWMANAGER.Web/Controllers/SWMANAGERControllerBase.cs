using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using Microsoft.AspNet.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace SW10.SWMANAGER.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// Add your methods to this class common for all controllers.
    /// </summary>
    public abstract class SWMANAGERControllerBase : AbpController
    {
        protected SWMANAGERControllerBase()
        {
            LocalizationSourceName = SWMANAGERConsts.LocalizationSourceName;
        }

        protected void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public virtual DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            if (data != null)
            {
                foreach (T item in data)
                {
                    try
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }

            return table;
        }
    }
}