using System.Linq;
using System.Web.Mvc;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;
using GameStore.WebUI.Models;

namespace GameStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IGameRepository repository;
        public CartController(IGameRepository repo)
        {
            repository = repo;
        }



        // Нашему специальному связывателю поручается создание объекта Cart,
        // и он делает это, взаимодействуя со средством состояния сеанса.
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
                {
                    Cart = cart,
                    ReturnUrl = returnUrl
                });
        }


        // Применяются имена параметров, которые соответствуют элементам <input>
        // Это позволяет MVC Framework ассоциировать входящие переменные HTTP-запроса POST формы с параметрами
        public RedirectToRouteResult AddToCart(Cart cart, int gameId, string returnUrl)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.GameId == gameId);

            if (game != null)
            {
                cart.AddItem(game,1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }



        public RedirectToRouteResult RemoveFromCart(Cart cart, int gameId, string returnUrl)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.GameId == gameId);
            if (game != null)
            {
                cart.RemoveLine(game);
            }
            return RedirectToAction("Index", new  { returnUrl });
        }


        // Добавление итоговой информации по корзине

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }


        // Добавление процесса оплаты
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            return View(new ShippingDetails());
        }


        // Отправить заказ
        public ViewResult SendAnOrder(Cart cart)
        {
            cart.Clear();
            return View();
        }
    }
}