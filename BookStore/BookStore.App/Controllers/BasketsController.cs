﻿using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using BookStore.Models.EntityModels;
using BookStore.Models.BindingModels.Basket;
using BookStore.Models.ViewModels.Basket;
using BookStore.Services.Interfaces;

namespace BookStore.App.Controllers
{
    [Authorize]
    public class BasketsController : Controller
    {
        private IBasketService basketService;

        public BasketsController(IBasketService service)
        {
            this.basketService = service;
        }

        // GET: Baskets/Details
        public ActionResult Details()
        {
            var ownerId = User.Identity.GetUserId();
            BasketViewModel viewModel = this.basketService.GetBasketDetails(ownerId);
            if (viewModel == null)
            {
                this.TempData["Info"] = "You have no basket. Select any books before and add them to basket.";
                return RedirectToAction("UserProfile", "Users");
            }

            return View(viewModel);
        }

        [HttpPost, ActionName("AddToBasket")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBasketAndAddBookInIt([Bind(Include = "Id")] AddBookToBasketBindingModel book)
        {
            Book currBook = this.basketService.GetCurrentBook(book.Id);
            User currUser = this.basketService.GetCurrentUser(User.Identity.GetUserId());         
            if (currBook.Quantity == 0)
            {
                this.TempData["Info"] = $"You can not add book '{currBook.Title}' in basket. it's not on stock.";
            }

            if (currBook.Quantity > 0)
            {
                this.basketService.AddBookToBasket(currUser, currBook);
                this.TempData["Success"] = $"You added book: '{currBook.Title}' in basket.";
            }

            return RedirectToAction("Details", "Baskets");
        }

        [HttpPost, ActionName("RemoveOneOfThisFromBasket")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveOneOfThisBookFromBasket([Bind(Include = "BookId")] RemoveBookFromBasketBindingModel book)
        {
            Book currentBook = this.basketService.GetCurrentBook(book.BookId);
            User currUser = this.basketService.GetCurrentUser(User.Identity.GetUserId());
            if (currentBook != null)
            {
                this.basketService.RemoveOneOfThisFromBasket(currentBook, currUser);
            }


            this.TempData["Success"] = $"You removed book '{currentBook.Title}' from your basket.";
            return RedirectToAction("Details", "Baskets");
        }

        [HttpPost, ActionName("RemoveAllOfThisFromBasket")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAllOfThisBookFromBasket([Bind(Include = "BookId, Count")] RemoveBooksFromBasketBindingModel book)
        {
            Book currentBook = this.basketService.GetCurrentBook(book.BookId);
            User currUser = this.basketService.GetCurrentUser(User.Identity.GetUserId());
            if (currentBook != null)
            {
                this.basketService.RemoveAllOfThisFromBasket(currentBook, currUser, book.Count);
            }


            this.TempData["Success"] = $"You removed {book.Count} books '{currentBook.Title}' from your basket.";
            return RedirectToAction("Details", "Baskets");
        }

        [HttpPost, ActionName("EditBookQuantityInBasket")]
        [ValidateAntiForgeryToken]
        public ActionResult EditBookQuantityInBasket([Bind(Include = "BookId, Count, NewCount")] EditBookQuantityInBasketBindingModel book)
        {
            Book currentBook = this.basketService.GetCurrentBook(book.BookId);
            if (currentBook == null)
            {
                return RedirectToAction("Details", "Basket");
            }

            User currUser = this.basketService.GetCurrentUser(User.Identity.GetUserId());
            int currQty = book.Count;
            if (currentBook != null && book.NewCount > 0 && book.NewCount <= (currentBook.Quantity + currQty))
            {
                this.basketService.EditBookQuantityInBasket(currentBook, currUser, currQty, book.NewCount);
                this.TempData["Success"] = $"You edited Qty of book '{currentBook.Title}' successfully. New Qty: {book.NewCount}";
            }
            else
            {

                this.TempData["Info"] = $"Invalid value. Value must be in range: [1 - {currentBook.Quantity + book.Count}]";

            }

            return RedirectToAction("Details", "Baskets");
        }

        [HttpPost, ActionName("ClearBasket")]
        [ValidateAntiForgeryToken]
        public ActionResult ClearBasket()
        {
            User currUser = this.basketService.GetCurrentUser(User.Identity.GetUserId());
            this.basketService.ClearBasket(currUser);
            
            this.TempData["Success"] = "Your Basket is empty! :(";
            return RedirectToAction("Details", "Baskets");
        }

        [HttpPost, ActionName("Buy")]
        [ValidateAntiForgeryToken]
        public ActionResult BuyBooks()
        {
            User currUser = this.basketService.GetCurrentUser(User.Identity.GetUserId());
            this.basketService.BuyBooks(currUser);
            
            this.TempData["Success"] = "Your order is accepted!";
            return RedirectToAction("Details", "Baskets");
        }
    }
}
