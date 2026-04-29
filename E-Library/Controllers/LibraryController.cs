using E_Library.Dtos;
using E_Library.Models;
using E_Library.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace E_Library.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController(ILibraryService bookservice) : ControllerBase
    {
        private readonly ILibraryService _books = bookservice;

        [HttpGet("All")]
        public async Task<IActionResult> AllBooks(int pageNumber = 1, int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 30)
                return BadRequest("Page number or Page size can't be less than 1 and Page size can't be greater than 30");

            var books = _books.GetAllBooksAsync(pageNumber, pageSize);

            return Ok(books);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int bookId)
        {
            if (bookId < 1) return BadRequest("id cannot be less than 1");
            //guid parse method is used to get the uid of the logged in user and convert it to guid format
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var book = _books.GetByIdAsync(bookId, userId);
            if (book is null) return NotFound("The entered ID does not match any books in our library");

            return Ok(book);
        }
        [HttpGet("Author/{author:string}")]
        public async Task<IActionResult> GetByAuthor(string author)
        {
            if (NullOrEmptyChecker(author)) return BadRequest("Author name cannot be empty");

            var booksByAuthor = await _books.GetByAuthorAsync(author);
            if (booksByAuthor is null) return NotFound("Author not found");

            return Ok(booksByAuthor);
        }
        [HttpGet("Purchase/{id: int}")]
        public async Task<IActionResult> PurchaseBook(int id)
        {
            if (id < 1) return BadRequest("ID can't be less than 1");

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var purchasedBook = await _books.PurchaseBookAsync(id, userId);

            if (purchasedBook is null) return Unauthorized("Purchase Failed");

            return Ok(purchasedBook);
        }
        [HttpGet("{bookId:int}/{accessToken:string}")]
        public async Task<IActionResult> ReadBook(int bookId, string accessToken)
        {
            if (NullOrEmptyChecker(accessToken) || bookId < 1) return BadRequest("Book ID or Book Access Token can't be empty");

            var bookcontent = await _books.ReadBookAsync(accessToken);

            if (bookcontent is null) return Unauthorized("Book not found or Incorrect Access Token");

            return Ok(bookcontent);
        }
        private bool NullOrEmptyChecker(params string[] values) => values.Any(string.IsNullOrEmpty);
    }
}
