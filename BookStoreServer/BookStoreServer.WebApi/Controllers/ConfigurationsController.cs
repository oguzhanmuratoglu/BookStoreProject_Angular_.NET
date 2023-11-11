using BookStoreServer.WebApi.Context;
using BookStoreServer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreServer.WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ConfigurationsController : ControllerBase
{
    AppDbContext context = new();

    [HttpGet]
    public IActionResult SeedData()
    {
        List<Category> categories = new();
        for (int i = 0; i < 10; i++)
        {
            var category = new Category
            {
                Name = $"Kategori {i + 1}",
                IsActive = true,
                IsDeleted = false
            };
            categories.Add(category);
        }
        context.Categories.AddRange(categories);
        context.SaveChanges();


        List<Book> books = new();
        for (int i = 0; i < 100; i++)
        {
            var book = new Book
            {
                Author = $"Yazar {i + 1}",
                Title = $"Kitap {i + 1}",
                CoverImageUrl = "https://i.dr.com.tr/cache/500x400-0/originals/0000000359758-1.jpg",
                Price = new((i + 1) * 10, "TRY"),
                ISBN = "351-6481245",
                Quantity = (i + 1) * 5,
                Summary = $"Açıklama {i + 1}",
                CreateAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };
            books.Add(book);
        }

        context.Books.AddRange(books);
        context.SaveChanges();


        List<BookCategory> bookCategories = new();
        foreach (var book in books)
        {
            var bookCategory = new BookCategory
            {
                BookId = book.Id,
                CategoryId = categories[new Random().Next(0, 10)].Id
            };
            bookCategories.Add(bookCategory);
        }
        context.BookCategories.AddRange(bookCategories);
        context.SaveChanges();
        return Ok(context.Books.ToList());
    }
    [HttpGet]
    public IActionResult SeedDataBook()
    {
        var result = context.Books.ToList();
        return Ok(result);

    }
    [HttpGet]
    public IActionResult SeedDataBookCategory()
    {
        return Ok(context.BookCategories.ToList());
    }
}
