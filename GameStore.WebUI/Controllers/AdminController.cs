﻿using System.Web.Mvc;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System.Linq;

namespace GameStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IGameRepository repository;

        public AdminController(IGameRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Games);
        }

        public ViewResult Edit(int gameId)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.GameId == gameId);
            return View(game);
        }

        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Game game)
        {
            if (ModelState.IsValid)
            {
                repository.SaveGame(game);


                // Если перезагрузить страницу, сообщение исчезнет, потому что после чтения данных объект TempData удаляется.
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(game);
            }
        }

        // Метод Create() не визуализирует свое стандартное представление. Вместо этого в нем указано,
        // что должно применяться представление Edit.
        public ViewResult Create()
        {
            return View("Edit", new Game());
        }


        [HttpPost]
        public ActionResult Delete(int gameId)
        {
            Game deleteGame = repository.DeleteGame(gameId);
            if (deleteGame != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена",
                                                deleteGame.Name);
            }
            return RedirectToAction("Index");
        }
    }
}