using BookStoreServer.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.WebApi.Context;

public sealed class AppDbContext : DbContext
{
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookStoreDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(p => p.Username).IsUnique();

        modelBuilder.Entity<OrderStatus>().HasIndex(p => new
        {
            p.Status,
            p.OrderNumber
        }).IsUnique();

        modelBuilder.Entity<Book>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5); // Assuming you want a max length for Currency
        });//Value Object

        modelBuilder.Entity<ShoppingCart>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5); // Assuming you want a max length for Currency
        });//Value Object

        modelBuilder.Entity<Order>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5); // Assuming you want a max length for Currency
        });//Value Object

        modelBuilder.Entity<BookCategory>().HasKey(p => new { p.BookId, p.CategoryId });

        //#region SeedData

        //List<Category> categories = new();
        //List<Book> books = new();
        //List<BookCategory> bookCategories = new();

        //for (int i = 0; i < 50; i++)
        //{
        //    var book = new Book
        //    {
        //        Id = i + 1,
        //        Author = $"Yazar {i + 1}",
        //        Title = $"Kitap {i + 1}",
        //        CoverImageUrl = "",
        //        Price = new((i + 1) * 10, "TRY"),
        //        ISBN = "351-6481245",
        //        Quantity = (i + 1) * 5,
        //        Summary = $"Açıklama {i + 1}",
        //        CreateAt = DateTime.Now,
        //        IsActive = true,
        //        IsDeleted = false,
        //    };
        //    books.Add(book);
        //}
        //modelBuilder.Entity<Book>().HasData(books);

        //for (int i = 0; i < 10; i++)
        //{
        //    var category = new Category
        //    {
        //        Id = i + 1,
        //        Name = $"Kategori {i + 1}"
        //    };
        //    categories.Add(category);
        //}
        //modelBuilder.Entity<Category>().HasData(categories);



        //foreach (var book in books)
        //{
        //    var bookCategory = new BookCategory
        //    {
        //        BookId = book.Id,
        //        CategoryId = categories[new Random().Next(0, 10)].Id
        //    };
        //    bookCategories.Add(bookCategory);
        //}


        //modelBuilder.Entity<BookCategory>().HasData(bookCategories);

        //#endregion SeedData


    }
}