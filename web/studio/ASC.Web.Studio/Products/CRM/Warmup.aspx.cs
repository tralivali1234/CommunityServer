﻿
using System.Collections.Generic;

using ASC.Web.Core;

namespace ASC.Web.CRM
{
    public partial class Warmup : WarmupPage
    {
        protected override List<string> Exclude
        {
            get
            {
                return new List<string>(1)
                {
                    "Reports.aspx",
                    "Cases.aspx",
                    "Calls.aspx",
                    "MailViewer.aspx"
                };
            }
        }
    }
}