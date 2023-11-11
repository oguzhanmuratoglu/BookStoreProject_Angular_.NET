using AutoMapper;
using BookStoreServer.WebApi.Context;
using BookStoreServer.WebApi.Dtos;
using BookStoreServer.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class BooksController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public BooksController(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetBestSellingBooks()
    {
        var bestSellingBooks = _context.Orders
            .Include(o => o.Book)
            .GroupBy(o => o.Book)
            .OrderByDescending(g => g.Sum(o => o.Quantity))
            .Take(10)
            .Select(g => new
            {
                BookId = g.Key.Id,
                Title = g.Key.Title,
                TotalSales = g.Sum(o => o.Quantity),
                AverageRaiting = Math.Round((decimal)g.Average(o => o.Raiting), 1),
                CoverImgUrl = g.Key.CoverImageUrl
            })
            .ToList();

        return Ok(bestSellingBooks);
    }

    [HttpPost]
    public IActionResult GetAll(RequestDto request)
    {        
        List<Book> books = new();
        if (request.CategoryId == null)
        {
            books = _context.Books
            .Where(p => p.IsActive == true && p.IsDeleted == false)
            .Where(p => p.Title.ToLower().Contains(request.Search.ToLower()) || p.ISBN.Contains(request.Search))
            .OrderByDescending(p => p.CreateAt)
            .Take(request.PageSize)
            .ToList();
        }
        else
        {
            books = _context.BookCategories
                .Where(p=> p.CategoryId == request.CategoryId)
                .Include(p=> p.Book)
                .Select(s=> s.Book)
                .Where(p => p.IsActive == true && p.IsDeleted == false)
                .Where(p => p.Title.ToLower().Contains(request.Search.ToLower()) || p.ISBN.Contains(request.Search))
                .OrderByDescending(p => p.CreateAt)
                .Take(request.PageSize)
                .ToList();
        }

        List<BookDto> requestDto = new();
        foreach (var book in books)
        {
            BookDto bookDto = _mapper.Map<BookDto>(book);
            bookDto.Categories = _context.BookCategories
                                .Where(p => p.BookId == book.Id)
                                .Include(p => p.Category)
                                .Select(s => s.Category.Name)
                                .ToList();

            //var bookDto = new BookDto()
            //{
            //    Title = book.Title,
            //    ISBN = book.ISBN,
            //    Id = book.Id,
            //    Author = book.Author,
            //    Categories =
            //        context.BookCategories
            //        .Where(p => p.BookId == book.Id)
            //        .Include(p => p.Category)
            //        .Select(s => s.Category.Name)
            //        .ToList(),
            //    CoverImageUrl = book.CoverImageUrl,
            //    CreateAt = book.CreateAt,
            //    IsActive = book.IsActive,
            //    IsDeleted = book.IsDeleted,
            //    Price = book.Price,
            //    Quantity = book.Quantity,
            //    Summary = book.Summary
            //};
            requestDto.Add(bookDto);
        }

        return Ok(requestDto);
    }
}