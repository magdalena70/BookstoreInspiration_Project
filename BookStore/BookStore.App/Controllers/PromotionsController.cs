﻿using System.Web.Mvc;
using System.Collections.Generic;
using System;
using BookStore.Models.ViewModels.Promotion;
using BookStore.Services.Interfaces;

namespace BookStore.App.Controllers
{
    [AllowAnonymous]
    public class PromotionsController : Controller
    {
        private IPromotionService promotionService;

        public PromotionsController(IPromotionService service)
        {
            this.promotionService = service;
        }

        // GET: Promotions
        public ActionResult AllPromotions()
        {
            IEnumerable<PromotionsViewModel> viewModel = this.promotionService.GetCurrentAndUpcoming();
            return View(viewModel);
        }

        // GET: Promotions/Details/5
        [ActionName("Details")]
        public ActionResult PromotionDetails(int? id)
        {
            if (id == null)
            {
                throw new Exception("Invalid URL - promotion's id can not be null");
            }

            PromotionsViewModel viewModel = this.promotionService.GetDetails(id);
            if (viewModel == null)
            {
                throw new Exception($"Invalid URL - there is no promotion with id {id}");
            }

            if (viewModel.AreThereBooks == false)
            {
                this.TempData["Info"] = "No books.";
            }

            if (viewModel.EndDate < DateTime.Now)
            {
                this.TempData["Info"] = "Expired promotion";
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }
    }
}
