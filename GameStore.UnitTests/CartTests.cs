using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameStore.Domain.Entities;
using System.Linq;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.WebUI.Controllers;
using System.Web.Mvc;
using GameStore.WebUI.Models;

namespace GameStore.UnitTests
{
    /// <summary>
    /// Tests for valid work of Cart) p.s. Aimmee one love
    /// </summary>
    [TestClass]
    public class CartTests
    {
        public CartTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //arrange
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };

            Cart cart = new Cart();

            //action
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();
            //assert
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);

        }
        [TestMethod]
        public void Can_Add_Quantity()
        {
            //arrange
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };

            Cart cart = new Cart();
            //action
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            List<CartLine> results = cart.Lines.OrderBy(p => p.Game.GameId).ToList();

            //assert

            Assert.IsTrue(results.Count() == 2);
            Assert.AreEqual(results[0].Quantity, 6);
            Assert.AreEqual(results[1].Quantity, 1);

        }
        [TestMethod]
        public void Can_Remove_From_Cart()
        {
            //Assert
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Game game3 = new Game { GameId = 3, Name = "Game3" };

            Cart cart = new Cart();

            cart.AddItem(game1, 1);
            cart.AddItem(game2, 4);
            cart.AddItem(game3, 2);
            cart.AddItem(game2, 1);

            //action
            cart.RemoveLine(game2);

            //Assert
            Assert.AreEqual(cart.Lines.Where(p => p.Game == game2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);

        }
        [TestMethod]
        public void Can_Compute_Total_Cost()
        {
            //arrange
            Game game1 = new Game { GameId = 1, Name = "Game1", Price = 100m };
            Game game2 = new Game { GameId = 2, Name = "Game2", Price = 43.4m };
            Cart cart = new Cart();
            cart.AddItem(game1, 2);
            cart.AddItem(game2, 1);
            //action
            decimal result = cart.ComputeTotalValue();
            //assert
            Assert.AreEqual(result, 243.4m);
        }
        [TestMethod]
        public void Can_Crear_Cart()
        {
            //arrange
            Game game1 = new Game { GameId = 1, Name = "Game1", Price = 100m };
            Game game2 = new Game { GameId = 2, Name = "Game2", Price = 43.4m };
            Cart cart = new Cart();
            cart.AddItem(game1, 2);
            cart.AddItem(game2, 1);
            //action
            cart.Clear();
            //assert
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
        /// <summary>
        /// Проверяем добавление в корзину, как в полноценный модуль
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object,null);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
        }
        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object,null);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null,null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        ///<summary>
        ///Проверяем, запрещает ли контроллер отправку пустой корзины
        ///</summary>
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController controller = new CartController(null, mock.Object);
            //action
            ViewResult result = controller.Checkout(cart, shippingDetails);
            //assert
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        ///<summary>
        ///Тестирует метод Checkout() на недопуск неверных данных
        /// </summary>
        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);
            CartController controller = new CartController(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");
            //action
            ViewResult result = controller.Checkout(cart, new ShippingDetails());
            //assert - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            //assert - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);
            //assert - проверка, чот представлению передана неверная информация Shipping details
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        ///<summary>
        ///Проверка на пропуск валидных запросов.
        ///При этом, данные для отправки не проверяются, так как их
        ///проверяет биндер.
        /// </summary>
        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);
            CartController controller = new CartController(null, mock.Object);
            //act
            ViewResult result = controller.Checkout(cart, new ShippingDetails());
            //assert
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
        
    }
}
