using Microsoft.AspNetCore.Mvc;
using E_Library.Services
namespace E_Library.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController(ILibraryService bookservice) : ControllerBase
    {
        private readonly ILibraryService _books = bookservice;

    }
}
