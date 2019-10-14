using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ProductsAndCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}

        [Required(ErrorMessage = "Name is required!")]
        public string Name {get;set;}

        public string Description {get;set;}

        [Required(ErrorMessage = "Price is required!")]
        public float Price {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Association> Categories {get;set;}
    }
}