﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;
using Moq;
using GameStore.WebUI.Controllers;

namespace GameStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        // Тестирование добавления игр в корзину
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };

            // Act
            Cart cart = new Cart();
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Assert

            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);
        }

        

        // Тестирование увеличение заказов
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };

            Cart cart = new Cart();

            // Act
            // Действие
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            // Assert
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        // Тест удаления товаров из корзины
        [TestMethod]
        public void Can_Remove_Line()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };
            Game game3 = new Game { GameId = 3, Name = "Игра3" };

            Cart cart = new Cart();
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 4);
            cart.AddItem(game3, 2);
            cart.AddItem(game2, 1);


            // Act
            cart.RemoveLine(game2);

            // Assert
            Assert.AreEqual(cart.Lines.Where(c=>c.Game == game2).Count(),0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }



        // Тестируем отчистку 
        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            cart.Clear();

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 0);
        }


        // Тестируем добавление в корзину
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            mock.Setup(m => m.Games).Returns(new List<Game>
                {
                    new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
                }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object);

            // Act
            controller.AddToCart(cart,1,null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(),1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
        }
    }
}
