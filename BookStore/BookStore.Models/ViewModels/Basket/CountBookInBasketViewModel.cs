﻿using BookStore.Models.ViewModels.Book;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.ViewModels.Basket
{
    public class CountBookInBasketViewModel
    {
        public int BookId { get; set; }

        [Display(Name ="Quantity")]
        public int Count { get; set; }

        public int NewCount { get; set; }

        public BookDetailsViewModel Book { get; set; }

        [Display(Name = "Promo Discount")]
        [DisplayFormat(DataFormatString = "{0:0.00} %", ApplyFormatInEditMode = true)]
        public decimal PromotionDiscount { get; set; }

        [Display(Name = "New Price")]
        [DisplayFormat(DataFormatString = "{0:0.00} BGN", ApplyFormatInEditMode = true)]
        public decimal NewPrice { get; set; }
    }
}
