using Abp.Extensions;
using System.IO;
using System.Threading;
using System.Web;

namespace SW10.SWMANAGER.Web.Bundling
{
    public static class ScriptPaths
    {
        public const string Json2 = "~/libs/json2/json2.min.js";

        public const string JQuery = "~/libs/jquery/jquery.min.js";
        public const string JQuery_Migrate = "~/libs/jquery/jquery-migrate.min.js";
        public const string JQuery_UI = "~/libs/jquery-ui/jquery-ui.min.js";

        public const string JQuery_Slimscroll = "~/libs/jquery-slimscroll/jquery.slimscroll.min.js";
        public const string JQuery_BlockUi = "~/libs/jquery-blockui/jquery.blockui.min.js";
        public const string JQuery_Cookie = "~/libs/jquery-cookie/jquery.cookie.min.js";
        public const string JQuery_Uniform = "~/libs/jquery-uniform/jquery.uniform.min.js";
        public const string JQuery_Ajax_Form = "~/libs/jquery-ajax-form/jquery.form.js";
        public const string JQuery_Sparkline = "~/libs/jquery-sparkline/jquery.sparkline.min.js";
        public const string JQuery_Validation = "~/libs/jquery-validation/js/jquery.validate.min.js";
        public const string JQuery_Mask = "~/Scripts/jquery.mask.min.js";
        public const string JQuery_jTable = "~/libs/jquery-jtable/jquery.jtable.min.js";
        public const string JQuery_Bootstrap_Switch = "~/libs/bootstrap-switch/js/bootstrap-switch.min.js";
        public const string JQuery_Color = "~/libs/jcrop/js/jquery.color.js";
        public const string JQuery_Jcrop = "~/libs/jcrop/js/jquery.Jcrop.min.js";
        public const string JQuery_Timeago = "~/libs/jquery-timeago/jquery.timeago.js";

        public const string Bootstrap = "~/libs/bootstrap/js/bootstrap.min.js";
        public const string Bootstrap_Hover_Dropdown = "~/libs/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js";
        public const string Bootstrap_DateRangePicker = "~/libs/bootstrap-daterangepicker/daterangepicker.js";
        public const string Bootstrap_Select = "~/libs/bootstrap-select/bootstrap-select.min.js";
        public const string Bootstrap_Switch = "~/libs/bootstrap-switch/js/bootstrap-switch.min.js";

        public const string SignalR = "~/Scripts/jquery.signalR-2.4.1.min.js";
        public const string LocalForage = "~/Scripts/localforage/localforage.min.js";


        public const string Morris = "~/libs/morris/morris.min.js";
        public const string Morris_Raphael = "~/libs/morris/raphael-min.js";

        public const string JsTree = "~/libs/jstree/jstree.js";
        public const string SpinJs = "~/libs/spinjs/spin.js";
        public const string SpinJs_JQuery = "~/libs/spinjs/jquery.spin.js";

        public const string SweetAlert = "~/libs/sweetalert/sweet-alert.min.js";
        public const string Toastr = "~/libs/toastr/toastr.min.js";

        public const string MomentJs = "~/Scripts/moment-with-locales.min.js";
        public const string MomentTimezoneJs = "~/Scripts/moment-timezone-with-data.min.js";
        public const string Underscore = "~/Scripts/underscore.min.js";

        public const string MustacheJs = "~/libs/mustachejs/mustache.min.js";


        public const string Abp = "~/Abp/Framework/scripts/abp.js";
        public const string Abp_JQuery = "~/Abp/Framework/scripts/libs/abp.jquery.js";
        public const string Abp_Toastr = "~/Abp/Framework/scripts/libs/abp.toastr.js";
        public const string Abp_BlockUi = "~/Abp/Framework/scripts/libs/abp.blockUI.js";
        public const string Abp_SpinJs = "~/Abp/Framework/scripts/libs/abp.spin.js";
        public const string Abp_SweetAlert = "~/Abp/Framework/scripts/libs/abp.sweet-alert.js";
        public const string Abp_Moment = "~/Abp/Framework/scripts/libs/abp.moment.js";
        public const string Abp_jTable = "~/Abp/Framework/scripts/libs/abp.jtable.js";

        

        // SMWE - Extras
        public const string Select2 = "~/metronic/assets/global/plugins/select2/js/select2.js";
        public const string Select2Min = "~/metronic/assets/global/plugins/select2/js/select2.min.js";
        public const string SmweMultiSelect = "~/libs/multi-select/js/jquery.multi-select.js";
        public const string SmweSavior = "~/libs/smwe/smwe-savior.js";
        public const string CustomConfirmModal = "~/Scripts/CustomConfirmModal/CustomConfirmModal.js";
        // Fim - Smwe Extras

        public static string Angular_Localization
        {
            get
            {
                return GetLocalizationFileForjAngularOrNull(Thread.CurrentThread.CurrentUICulture.Name.ToLower())
                       ?? GetLocalizationFileForjAngularOrNull(Thread.CurrentThread.CurrentUICulture.Name.Left(2).ToLower())
                       ?? "~/Scripts/i18n/angular-locale_en-us.js";
            }
        }

        private static string GetLocalizationFileForjAngularOrNull(string cultureCode)
        {
            try
            {
                var relativeFilePath = "~/Scripts/i18n/angular-locale_" + cultureCode + ".js";
                var physicalFilePath = HttpContext.Current.Server.MapPath(relativeFilePath);
                if (File.Exists(physicalFilePath))
                {
                    return relativeFilePath;
                }
            }
            catch { }

            return null;
        }


        public static string JQuery_Validation_Localization
        {
            get
            {
                return GetLocalizationFileForjQueryValidationOrNull(Thread.CurrentThread.CurrentUICulture.Name.ToLower().Replace("-", "_"))
                       ?? GetLocalizationFileForjQueryValidationOrNull(Thread.CurrentThread.CurrentUICulture.Name.Left(2).ToLower())
                       ?? "~/libs/jquery-validation/js/localization/_messages_empty.js";
            }
        }

        private static string GetLocalizationFileForjQueryValidationOrNull(string cultureCode)
        {
            try
            {
                var relativeFilePath = "~/libs/jquery-validation/js/localization/messages_" + cultureCode + ".min.js";
                var physicalFilePath = HttpContext.Current.Server.MapPath(relativeFilePath);
                if (File.Exists(physicalFilePath))
                {
                    return relativeFilePath;
                }
            }
            catch { }

            return null;
        }

        public static string JQuery_JTable_Localization
        {
            get
            {
                return GetLocalizationFileForJTableOrNull(Thread.CurrentThread.CurrentUICulture.Name.ToLower())
                       ?? GetLocalizationFileForJTableOrNull(Thread.CurrentThread.CurrentUICulture.Name.Left(2).ToLower())
                       ?? "~/libs/jquery-jtable/localization/_jquery.jtable.empty.js";
            }
        }

        private static string GetLocalizationFileForJTableOrNull(string cultureCode)
        {
            try
            {
                var relativeFilePath = "~/libs/jquery-jtable/localization/jquery.jtable." + cultureCode + ".js";
                var physicalFilePath = HttpContext.Current.Server.MapPath(relativeFilePath);
                if (File.Exists(physicalFilePath))
                {
                    return relativeFilePath;
                }
            }
            catch { }

            return null;
        }

        public static string Bootstrap_Select_Localization
        {
            get
            {
                return GetLocalizationFileForBootstrapSelect(Thread.CurrentThread.CurrentUICulture.Name.ToLower())
                       ?? GetLocalizationFileForBootstrapSelect(Thread.CurrentThread.CurrentUICulture.Name.Left(2).ToLower())
                       ?? "~/libs/bootstrap-select/i18n/defaults-en_US.js";
            }
        }

        private static string GetLocalizationFileForBootstrapSelect(string cultureCode)
        {
            var localizationFileList = new[]
            {
                "ar_AR",
                "bg_BG",
                "cs_CZ",
                "da_DK",
                "de_DE",
                "en_US",
                "es_CL",
                "eu",
                "fa_IR",
                "fi_FI",
                "fr_FR",
                "hu_HU",
                "id_ID",
                "it_IT",
                "ko_KR",
                "nb_NO",
                "nl_NL",
                "pl_PL",
                "pt_BR",
                "pt_PT",
                "ro_RO",
                "ru_RU",
                "sk_SK",
                "sl_SL",
                "sv_SE",
                "tr_TR",
                "ua_UA",
                "zh_CN",
                "zh_TW"
            };

            try
            {
                cultureCode = cultureCode.Replace("-", "_");

                foreach (var localizationFile in localizationFileList)
                {
                    if (localizationFile.StartsWith(cultureCode))
                    {
                        return "~/libs/bootstrap-select/i18n/defaults-" + localizationFile + ".js";
                    }
                }
            }
            catch { }

            return null;
        }

        public static string JQuery_Timeago_Localization
        {
            get
            {
                return GetLocalizationFileForjQueryTimeagoOrNull(Thread.CurrentThread.CurrentUICulture.Name.ToLower().Replace("-", "_"))
                       ?? GetLocalizationFileForjQueryTimeagoOrNull(Thread.CurrentThread.CurrentUICulture.Name.Left(2).ToLower())
                       ?? "~/libs/jquery-timeago/locales/jquery.timeago.en.js";
            }
        }

        private static string GetLocalizationFileForjQueryTimeagoOrNull(string cultureCode)
        {
            try
            {
                var relativeFilePath = "~/libs/jquery-timeago/locales/jquery.timeago." + cultureCode + ".js";
                var physicalFilePath = HttpContext.Current.Server.MapPath(relativeFilePath);
                if (File.Exists(physicalFilePath))
                {
                    return relativeFilePath;
                }
            }
            catch { }

            return null;
        }
    }
}
