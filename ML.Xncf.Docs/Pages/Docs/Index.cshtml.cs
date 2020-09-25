using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML.Xncf.Docs.Models.VD;
using Senparc.Ncf.Core.Cache;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.Service;

namespace ML.Xncf.Docs
{
  public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        //public IndexModel(Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
        //{
        //}

        public async Task<IActionResult> OnGetAsync()
    {
      return Page();
    }
  }
}