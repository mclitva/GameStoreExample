using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrive_Imdage_Data()
        {
            //arrange
            Game game = new Game
            {
                GameId = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
            {
                new Game{GameId = 1, Name = "G1"},
                game,
                new Game{GameId = 3, Name = "G3"}
            }.AsQueryable());
            GameController target = new GameController(mock.Object);
            //Action
            ActionResult result = target.GetImage(2);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(game.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrive_Image_Data_For_Invalid_Id()
        {
            //Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
            {
                new Game{GameId = 1, Name = "G1"},
                new Game{GameId = 2, Name = "G2"}
            }.AsQueryable());
            GameController target = new GameController(mock.Object);
            //Action
            ActionResult result = target.GetImage(5);
            //Assert
            Assert.IsNull(result);
        }
    }
}
