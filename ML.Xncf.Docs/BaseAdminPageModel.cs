//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using ML.Xncf.Docs.Models.VD;
//using Senparc.Ncf.AreaBase.Admin;
//using Senparc.Ncf.AreaBase.Admin.Filters;
//using Senparc.Ncf.Core;
//using Senparc.Ncf.Core.Models.VD;
//using Senparc.Ncf.Core.WorkContext;
//using Senparc.Ncf.XncfBase;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ML.Xncf.Docs
//{

//  public interface IBaseAdminPageModel : IBasePageModel
//  {

//  }

//  //暂时取消权限验证
//  //[ServiceFilter(typeof(AuthenticationResultFilterAttribute))]
//  [AdminAuthorize("AdminOnly")]
//  public class BaseAdminPageModel : AdminPageModelBase, IBaseAdminPageModel
//  {
//    public ML.Xncf.Docs.Register _XncfRegister;
//    public ML.Xncf.Docs.Register XncfRegister
//    {
//      get
//      {
//        _XncfRegister = _XncfRegister ?? new Register();
//        return _XncfRegister;
//      }
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="context"></param>
//    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
//    {
//      //context
//      if (!context.ModelState.IsValid)
//      {
//        //全局模型验证
//        var state = context.ModelState
//            .Where(_ => _.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
//            .Select(_ => new { _.Key, Errors = _.Value.Errors.Select(__ => __.ErrorMessage) });
//        context.Result = BadRequest(new AjaxReturnModel<object>(state));
//      }
//      base.OnPageHandlerExecuting(context);
//    }

//    public override IActionResult RenderError(string message)
//    {
//      return base.RenderError(message);
//    }
//  }
//}
