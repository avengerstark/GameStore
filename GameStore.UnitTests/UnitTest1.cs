using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models;
using System;

namespace GameStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        // Тестируем Метод List, постороничный вывод
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
    {
        new Game { GameId = 1, Name = "Игра1"},
        new Game { GameId = 2, Name = "Игра2"},
        new Game { GameId = 3, Name = "Игра3"},
        new Game { GameId = 4, Name = "Игра4"},
        new Game { GameId = 5, Name = "Игра5"}
    });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            GamesListViewModel result = (GamesListViewModel)controller.List(null,2).Model;

            // Утверждение
            List<Game> games = result.Games.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Игра4");
            Assert.AreEqual(games[1].Name, "Игра5");
        }




        // Необходимо удостовериться, что контроллер отправляет
        // представлению правильную информацию о разбиении на страницы.
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)

            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                 new Game { GameId = 1, Name = "Игра1"},
                 new Game { GameId = 2, Name = "Игра2"},
                 new Game { GameId = 3, Name = "Игра3"},
                 new Game { GameId = 4, Name = "Игра4"},
                 new Game { GameId = 5, Name = "Игра5"}
            });

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 4;

            // Act
            GamesListViewModel result = (GamesListViewModel)controller.List(null, 3).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 3);
            Assert.AreEqual(pageInfo.ItemsPerPage, 4);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }




        // Тестируем фильтрацию по категории
        [TestMethod]
        public void Can_Filter_Games()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Игра1", Category="Cat1"},
                new Game { GameId = 2, Name = "Игра2", Category="Cat2"},
                new Game { GameId = 3, Name = "Игра3", Category="Cat1"},
                new Game { GameId = 4, Name = "Игра4", Category="Cat2"},
                new Game { GameId = 5, Name = "Игра5", Category="Cat3"}
            });

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Game> result = ((GamesListViewModel)controller.List("Cat2", 1).Model).Games.ToList();

            //Assert

            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Игра2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Игра4" && result[1].Category == "Cat2");
        }

        // Тестируем генерацию списка категорий
        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
                {
                     new Game { GameId = 1, Name = "Игра1", Category="Симулятор"},
                     new Game { GameId = 2, Name = "Игра2", Category="Симулятор"},
                     new Game { GameId = 3, Name = "Игра3", Category="Шутер"},
                     new Game { GameId = 4, Name = "Игра4", Category="RPG"},
                });

            NavController target = new NavController(mock.Object);

            // Act
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Assert
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "RPG");
            Assert.AreEqual(results[1], "Симулятор");
            Assert.AreEqual(results[2], "Шутер");

        }




        // Тестируем счетчик товаров определенной категории
        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> 
            {
                new Game { GameId = 1, Name = "Игра1", Category="Cat1"},
                new Game { GameId = 2, Name = "Игра2", Category="Cat2"},
                new Game { GameId = 3, Name = "Игра3", Category="Cat1"},
                new Game { GameId = 4, Name = "Игра4", Category="Cat2"},
                new Game { GameId = 5, Name = "Игра5", Category="Cat3"}
            });

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Act
            int res1 = ((GamesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((GamesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((GamesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((GamesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;


            // Assert

            Assert.AreEqual(res1,2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
 
    }
}
