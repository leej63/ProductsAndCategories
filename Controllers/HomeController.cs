using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {
        private ProductContext dbContext;

        public HomeController(ProductContext context)
        {
            dbContext = context;
        }

        // **************************************************************************************************

        // Show all existing products
        [HttpGet("")]
        public IActionResult Products()
        {
            List<Product> AllProducts = dbContext.Products.ToList();
            ViewBag.allProducts = AllProducts;
            return View("Products");
        }

        // Show one product with its categories
        [HttpGet("products/{productId}")]
        public IActionResult Product(int productId)
        {
            Product selectedProduct = dbContext.Products
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.ProductId == productId);
            ViewBag.product = selectedProduct;
            List<Category> allCategories = dbContext.Categories.ToList();
            List<Category> containsCategories = new List<Category>();
            foreach(Association association in selectedProduct.Categories)
            {
                containsCategories.Add(association.Category);
            }
            List<Category> excludesCategories = new List<Category>();
            foreach(Category category in allCategories)
            {
                if(!containsCategories.Contains(category))
                {
                    excludesCategories.Add(category);
                }
            }
            ViewBag.categories = excludesCategories;
            return View("Product");
        }

        // Show all existing categories
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            List<Category> AllCategories = dbContext.Categories.ToList();
            ViewBag.allCategories = AllCategories;
            return View("Categories");
        }
        
        // Show one category with its products
        [HttpGet("categories/{categoryId}")]
        public IActionResult Category(int categoryId)
        {
            Category selectedCategory = dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CategoryId == categoryId);
            ViewBag.category = selectedCategory;
            List<Product> allProducts = dbContext.Products.ToList();
            List<Product> containsProducts = new List<Product>();
            foreach(Association association in selectedCategory.Products)
            {
                containsProducts.Add(association.Product);
            }
            List<Product> excludesProducts = new List<Product>();
            foreach(Product product in allProducts)
            {
                if(!containsProducts.Contains(product))
                {
                    excludesProducts.Add(product);
                }
            }
            ViewBag.products = excludesProducts;
            return View("Category");
        }

        // **************************************************************************************************

        // (form) add new product
        [HttpPost("product/new")]
        public IActionResult CreateProduct(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newProduct);
                dbContext.SaveChanges();
                return RedirectToAction("Products");
            }
            List<Product> AllProducts = dbContext.Products.ToList();
            ViewBag.allProducts = AllProducts;
            return View("Products");
        }

        // (form) add new category
        [HttpPost("category/new")]
        public IActionResult CreateCategory(Category newCategory)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newCategory);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            }
            List<Category> AllCategories = dbContext.Categories.ToList();
            ViewBag.allCategories = AllCategories;
            return View("Categories");
        }

        // (form) add product to category
        [HttpPost("add/product")]
        public IActionResult AddProduct(Association newAssociation)
        {
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            return Redirect($"/categories/{newAssociation.CategoryId}");
            // return RedirectToAction("Category", new { newAssociation.CategoryId });
        }

        // (form) add category to product
        [HttpPost("add/category")]
        public IActionResult AddCategory(Association newAssociation)
        {
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            return Redirect($"/products/{newAssociation.ProductId}");
            // return RedirectToAction("Product", new { newAssociation.ProductId });
        }

        // **************************************************************************************************

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
