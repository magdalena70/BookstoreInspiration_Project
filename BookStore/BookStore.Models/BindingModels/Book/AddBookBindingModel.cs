﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookStore.Models.EntityModels;

namespace BookStore.Models.BindingModels.Book
{
    public class AddBookBindingModel
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required, MaxLength(10)]
        public string Language { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int NumberOfPages { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required, MaxLength(20)]
        public string ISBN { get; set; }

        public string Authors { get; set; }

        public string Categories { get; set; }
    }
}
