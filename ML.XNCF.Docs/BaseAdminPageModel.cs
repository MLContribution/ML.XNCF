using Microsoft.AspNetCore.Mvc;
using ML.XNCF.Docs.Models.VD;
using Senparc.Scf.AreaBase.Admin;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core.Models.VD;
using Senparc.Scf.Core.WorkContext;
using Senparc.Scf.XNCFBase;

namespace ML.XNCF.Docs
{

    public interface IBaseAdminPageModel : IBasePageModel
    {

    }

    //暂时取消权限验证
    //[ServiceFilter(typeof(AuthenticationAsyncPageFilterAttribute))]
    [AdminAuthorize("AdminOnly")]
    public class BaseAdminPageModel : AdminPageModelBase, IBaseAdminPageModel
    {
        public ML.XNCF.Docs.Register _XNCFRegister;
        public ML.XNCF.Docs.Register XNCFRegister
        {
            get
            {
                _XNCFRegister = _XNCFRegister ?? new Register();
                return _XNCFRegister;
            }
        }

        public override IActionResult RenderError(string message)
        {
            return base.RenderError(message);
        }
    }
}
