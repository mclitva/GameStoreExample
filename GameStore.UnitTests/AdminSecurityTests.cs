using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Infrastructure.Abstract;
using GameStore.WebUI.Models;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "12345")).Returns(true);
            LoginViewModel model = new LoginViewModel { UserName = "admin", Password = "12345" };
            AccountController target = new AccountController(mock.Object);
            //Action
            ActionResult result = target.Login(model, "/MyUrl");
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("invalidUser", "invalidPass")).Returns(false);
            LoginViewModel model = new LoginViewModel
            {
                UserName = "invalidUser",
                Password = "invalidPass"
            };
            AccountController target = new AccountController(mock.Object);
            //Action
            ActionResult result = target.Login(model, "/MyUrl");
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
