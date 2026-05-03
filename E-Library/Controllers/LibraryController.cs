using E_Library.Dtos;
using E_Library.Models;
using E_Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace E_Library.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController(ILibraryService bookservice) : ControllerBase
    {
        private readonly ILibraryService _books = bookservice;
        [Authorize]
        [HttpGet("All")]
        public async Task<IActionResult> AllBooks(int pageNumber = 1, int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 30)
                return BadRequest("Page number or Page size can't be less than 1 and Page size can't be greater than 30");

            var books = await _books.GetAllBooksAsync(pageNumber, pageSize);

            return books is null ? NotFound("No books found") : Ok(books);
        }
        [Authorize]
        [HttpGet("{bookId:int}")]
        public async Task<IActionResult> GetById(int bookId)
        {
            Console.WriteLine("Method entered with ID: " + bookId);
            if (bookId < 1) return BadRequest("id cannot be less than 1");
            //guid parse method is used to get the uid of the logged in user and convert it to guid format
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var book = await _books.GetByIdAsync(bookId, userId);
            if (book is null) return NotFound("The entered ID does not match any books in our library");

            return Ok(book);
        }
        [Authorize]
        [HttpGet("Author/{author}")]
        public async Task<IActionResult> GetByAuthor(string author)
        {
            if (NullOrEmptyChecker(author)) return BadRequest("Author name cannot be empty");

            var booksByAuthor = await _books.GetByAuthorAsync(author);
            if (booksByAuthor is null) return NotFound("Author not found");

            return Ok(booksByAuthor);
        }
        [Authorize]
        [HttpGet("Purchase/{id:int}")]
        public async Task<IActionResult> PurchaseBook(int id)
        {
            if (id < 1) return BadRequest("ID can't be less than 1");

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var purchasedBook = await _books.PurchaseBookAsync(id, userId);

            if (purchasedBook is false) 
                return BadRequest("Purchase Failed! Please check that the book ID is correct, you have sufficient Book Credits and that u don't already own it");

            return Ok("Book Purchased");
        }
        [Authorize]
        [HttpGet("{bookId:int}/{accessToken}")]
        public async Task<IActionResult> ReadBook(int bookId, string accessToken)
        {
            if (NullOrEmptyChecker(accessToken) || bookId < 1) return BadRequest("Book ID or Book Access Token can't be empty");

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var bookcontent = await _books.ReadBookAsync(userId,bookId,accessToken);

            if (bookcontent is null) return Unauthorized("Book not found,Incorrect Access Token (Please make sure you own the book)");

            return Ok(bookcontent);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddBook(CreateBookDto dto)
        {
            if (NullOrEmptyChecker(dto.Author, dto.Body, dto.BookName)) return BadRequest("Fields can't be empty. Please enter the name of the Book, its Author, Contents of the book and Purchase Price");

            var created = await _books.CreateBookAsync(dto);

            return created? Ok("Book added to library") : BadRequest("Something went wrong");
        }
        private bool NullOrEmptyChecker(params string[] values) => values.Any(string.IsNullOrEmpty);
    }
}
