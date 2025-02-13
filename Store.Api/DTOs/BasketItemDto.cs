﻿using System.ComponentModel.DataAnnotations;

namespace Store.Api.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Range(0.1 , double.MaxValue , ErrorMessage ="Price Can not Be Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1,int.MaxValue , ErrorMessage = "Quantity Must Be one item at least")]
        public int Quantity { get; set; }
    }
}