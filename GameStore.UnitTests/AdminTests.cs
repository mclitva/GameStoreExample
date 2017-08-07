using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Conteins_All_Games()
        {
            //arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Игра1"},
                new Game { GameId = 2, Name = "Игра2"},
                new Game { GameId = 3, Name = "Игра3"},
                new Game { GameId = 4, Name = "Игра4"},
                new Game { GameId = 5, Name = "Игра5"}
            });

            AdminController controller = new AdminController(mock.Object);

            //Action

            List<Game> result = ((IEnumerable<Game>)controller.Index().
                ViewData.Model).ToList();

            //Assert

            Assert.AreEqual(result.Count(), 5);
            for(int i=1;i<=5;i++)
            {
                Assert.AreEqual("Игра" + i, result[i-1].Name);
            }
            
        }
        ///<summary>
        ///Проверка валидной работы метода Edit, контроллера Admin
        /// </summary>
        [TestMethod]
        public void Can_Edit_Game()
        {
            //arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
            {
                new Game{GameId = 1, Name = "G1"},
                new Game{GameId = 2, Name = "G2"},
                new Game{GameId = 3, Name = "G3"}
            }.AsQueryable());
            AdminController target = new AdminController(mock.Object);
            //action
            Game g1 = target.Edit(1).ViewData.Model as Game;
            Game g2 = target.Edit(2).ViewData.Model as Game;
            Game g3 = target.Edit(3).ViewData.Model as Game;
            //assert
            Assert.AreEqual(1, g1.GameId);
            Assert.AreEqual(2, g2.GameId);
            Assert.AreEqual(3, g3.GameId);
        }

        ///<summary>
        ///Проверяем на запрос с несуществующим ID для метода Edit
        /// </summary>
        [TestMethod]
        public void Cannot_Edit_Nonexistent_game()
        {
            //arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
            {
                new Game{GameId = 1, Name = "G1"},
                new Game{GameId = 2, Name = "G2"},
                new Game{GameId = 3, Name = "G3"}
            }.AsQueryable());
            AdminController target = new AdminController(mock.Object);
            //action
            Game result = (Game)target.Edit(4).ViewData.Model;
            //assert
            Assert.IsNull(result);
        }
        ///<summary>
        ///Тестируем, может ли POST метод Edit сохранять валидные данные
        /// </summary>
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            AdminController target = new AdminController(mock.Object);
            Game game = new Game { Name = "Test" };
            //Aсtion
            ActionResult result = target.Edit(game,null);
            //Assert
            mock.Verify(m => m.SaveGame(game));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }
        
        [TestMethod]
        public void Cannot_Save_Invalid_Canges()
        {
            //arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            AdminController target = new AdminController(mock.Object);
            Game game = new Game { Name = "Test" };
            target.ModelState.AddModelError("error", "error");
            //action
            ActionResult result = target.Edit(game, null);
            //Assert
            mock.Verify(m => m.SaveGame(It.IsAny<Game>()), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            //arrange
            Game game = new Game { GameId = 2, Name = "Test" };
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
            {
                new Game{GameId = 1, Name = "G1"},
                new Game{GameId = 3, Name = "G3"}
            }.AsQueryable());
            AdminController target = new AdminController(mock.Object);
            //action
            target.Delete(game.GameId);
            //assert
            mock.Verify(m => m.DeleteGame(game.GameId));

        }
    }
}
