using System;
using System.Web.Security;
using GameStore.WebUI.Infrastructure.Abstract;
using GameStore.WebUI.Infrastructure.Concrete;

namespace GameStore.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            bool result = FormsAuthentication.Authenticate(username, password);
#pragma warning restore CS0618 // Type or member is obsolete
            if (result) FormsAuthentication.SetAuthCookie(username, false);
            return result;
        }
    }
}