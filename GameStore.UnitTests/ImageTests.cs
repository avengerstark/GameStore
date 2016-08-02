using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        public void Can_Retrieve_Image_Data()
        {
            // Arrange
            Game game = new Game
            {
                GameId = 2,
                Name = "Игра2",
                ImageData = new byte[]{ },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Игра1" },
                new Game { GameId = 3, Name = "Игра3" }
            }.AsQueryable());

            // Организация - создание контроллера
            GameController controller = new GameController(mock.Object);

            // Act
            ActionResult result = controller.GetImage(2);

            // Assert

            // Проверяем получение результата FileResult из метода действия и
            // соответствие типа содержимого типу имитированных данных.
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(game.ImageMimeType,((FileResult)result).ContentType);
        }


        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game {GameId = 1, Name = "Игра1"},
                new Game {GameId = 2, Name = "Игра2"}
            }.AsQueryable());

            // Организация - создание контроллера
            GameController controller = new GameController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение

            // В случае запроса недопустимого идентификатора товара мы просто проверяем,
            // что результатом является null.
            Assert.IsNull(result);
        }
    }
}
