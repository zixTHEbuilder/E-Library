using E_Library.Dtos;
using E_Library.Models;
using E_Library.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Library.UnitTests.Controllers
{
    public class LibraryControllerTest
    {
        [Fact]
        public async Task<IActionResult> AllBooks_Returns_OkResult()
        {
            var libraryServiceMock = new Mock<ILibraryService>();

            //make sure u use "PagedResult" here instead of list because thats how it is in the Service
            libraryServiceMock.Setup(x => x.GetAllBooksAsync(1, 2)).ReturnsAsync(new PagedResult<BookDisplayModel>());
        }
    }
}
