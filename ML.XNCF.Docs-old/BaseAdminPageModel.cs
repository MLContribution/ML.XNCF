using Microsoft.AspNetCore.Mvc;
using ML.Xncf.Docs.Models.VD;
using Senparc.Ncf.AreaBase.Admin;
using Senparc.Ncf.AreaBase.Admin.Filters;
using Senparc.Ncf.Core.Models.VD;
using Senparc.Ncf.Core.WorkContext;
using Senparc.Ncf.XncfBase;

namespace ML.Xncf.Docs
{

    public interface IBaseAdminPageModel : IBasePageModel
    {

    }

    //暂时取消权限验证
    //[ServiceFilter(typeof(AuthenticationAsyncPageFilterAttribute))]
    [AdminAuthorize("AdminOnly")]
    public class BaseAdminPageModel : AdminPageModelBase, IBaseAdminPageModel
    {
        public ML.Xncf.Docs.Register _XncfRegister;
        public ML.Xncf.Docs.Register XncfRegister
        {
            get
            {
                _XncfRegister = _XncfRegister ?? new Register();
                return _XncfRegister;
            }
        }

        public override IActionResult RenderError(string message)
        {
            return base.RenderError(message);
        }
    }
}
