﻿using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using BookStore.Models.EntityModels;
using BookStore.Models.BindingModels.Book;
using BookStore.Models.ViewModels.User;
using System;
using BookStore.Services.Interfaces;

namespace BookStore.App.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private IUserService userService;

        public UsersController(IUserService service)
        {
            this.userService = service;
        }

        // GET: Users/UserProfile
        public ActionResult UserProfile()
        {
            User currentUser = this.userService.GetCurrentUser(User.Identity.GetUserId());
            if (currentUser == null)
            {
                this.TempData["Error"] = "Log in, please!";
                return RedirectToAction("Login", "Account");
            }

            UserProfileViewModel viewModel = this.userService.GetUserProfileViewModel(currentUser);            
            return View(viewModel);
        }

        // GET: Users/FavoriteBooks
        public ActionResult FavoriteBooks()
        {
            var currentUser = this.userService.GetCurrentUser(User.Identity.GetUserId());
            UserFavoriteBooksViewModel viewModel = this.userService.GetFavorite(currentUser);
            return View(viewModel);
        }

        // POST: Users/FavoriteBooks
        [HttpPost, ActionName("FavoriteBooks")]
        [ValidateAntiForgeryToken]
        public ActionResult AddBookToFavoriteBooks([Bind(Include = "Id")] FavoriteBookBindingModel book)
        {
            User currentUser = this.userService.GetCurrentUser(User.Identity.GetUserId());
            this.userService.AddBookToFavoriteBooks(currentUser, book.Id);
            
            return RedirectToAction("FavoriteBooks", "Users");
        }

        // POST: Users/RemoveFromFavorite
        [HttpPost, ActionName("RemoveFromFavorite")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveBookFromFavoriteBooks([Bind(Include = "Id")] FavoriteBookBindingModel book)
        {
            User currentUser = this.userService.GetCurrentUser(User.Identity.GetUserId());
            this.userService.RemoveBookFromFavoriteBooks(currentUser, book.Id);

            this.TempData["Success"] = $"You removed one book from Favorite Books.";
            return RedirectToAction("FavoriteBooks", "Users");
        }

        // GET: Users/EditProfile
        public ActionResult EditProfile()
        {
            User currentUser = this.userService.GetCurrentUser(User.Identity.GetUserId());
            if (currentUser == null)
            {
                throw new Exception("Invalid URL - You can not edit this profile!");
            }

            EditUserProfileViewModel viewModel = this.userService.GetEditUserProfileViewModel(currentUser);
            return View(viewModel);
        }

        // POST: Users/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditUserProfileBindingModel bindingModel)
        {
            User currentUser = this.userService.GetCurrentUser(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                this.userService.EditUserProfile(currentUser, bindingModel);
                return RedirectToAction("UserProfile", "Users");
            }

            return this.View(currentUser);
        }
    }
}
