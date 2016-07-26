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
            GamesListViewModel result = (GamesListViewModel)controller.List(2).Model;

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
            GamesListViewModel result = (GamesListViewModel)controller.List(3).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 3);
            Assert.AreEqual(pageInfo.ItemsPerPage, 4);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
 
    }
}
