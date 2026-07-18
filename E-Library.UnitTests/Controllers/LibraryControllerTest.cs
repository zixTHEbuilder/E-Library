using E_Library.Controllers;
using E_Library.Data;
using E_Library.Dtos;
using E_Library.Models;
using E_Library.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace E_Library.UnitTests.Controllers
{
    public class LibraryControllerTest
    {
        [Fact]
        public async Task AllBooks_Returns_OkResult()
        {
            //ARRANGE
            var pagedResult = new PagedResult<BookDisplayModel>()
            {
                CurentPage = 1,
                TotalPages = 2,
                Data = new List<BookDisplayModel>()
                {
                    new BookDisplayModel()
                    {
                        Id = 10,
                        PurchasePrice = 499 ,
                        Author = "UnderTheSea",
                        BookAccessCode = "AW670",
                        BookName = "The Walkers"
                    },
                    new BookDisplayModel()
                    {
                        Id = 9,
                        PurchasePrice = 399,
                        BookName = "The Fault In Our Stars",
                        Author = "John Green",
                        BookAccessCode  = "TFIOS79"
                    }
                }
            };
            var libraryServiceMock = new Mock<ILibraryService>();
            //make sure u use "PagedResult" here instead of list because thats how it is in the Service
            //this is for setting up, telling it must return this kind of data
            libraryServiceMock.Setup(x => x.GetAllBooksAsync(1, 2)).ReturnsAsync(pagedResult);
            var controller = new LibraryController(libraryServiceMock.Object);

            //ACT
            var returnedResult = await controller.AllBooks(1, 2);

            //ASSERT
            var okResult = Assert.IsType<OkObjectResult>(returnedResult);

            Assert.Equal(pagedResult, okResult.Value);
            Assert.Equal(200, okResult.StatusCode);


            //var okResult = returnedResult.Should().BeOfType<OkObjectResult>().Subject; 
            //okResult.Value.Should().BeEquivalentTo(pagedResult);
        }

        //For this theory I will be writing Assertion in both FluentAssertion and Assert
        [Theory]
        [InlineData(2, true, "CODE575")]
        [InlineData(10, false, "Purchase the book to get the Token")]
        public async Task GetById_Returns_CorrectAccessCode(int bookId , bool hasPurchased, string expectedToken)
        {
            var expectedBookCode = new BookDisplayModel
            {
                Id = bookId,
                BookAccessCode = hasPurchased? expectedToken : "Purchase the book to get the Token"
            };

            var libraryServiceMock = new Mock<ILibraryService>();
            var testUserId = Guid.NewGuid();
            libraryServiceMock.Setup(x => x.GetByIdAsync(bookId, It.IsAny<Guid>())).ReturnsAsync(expectedBookCode);
            var controller = CreateControllerWithUser(libraryServiceMock.Object, testUserId);

            var result = await controller.GetById(bookId);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            //var okResult = Assert.IsType<OkObjectResult>(result);

            // code below is to unpack the object type so we can access internal properties like .BookAccessCode
            var returnedBook = okResult.Value.Should().BeOfType<BookDisplayModel>().Subject;
            //var returnedBook = Assert.IsAssignableFrom<BookDisplayModel>(okResult.Value);

            //dont use BeEquivalentTo for strings, tokens or accessCodes since it does a case-insensitive comparison
            returnedBook.BookAccessCode.Should().Be(expectedToken);
            //Assert.Equal(expectedToken, returnedBook.BookAccessCode);
        }
        [Fact]
        public async Task BookId_WithInvalidBookId_ReturnsNotFound()
        {
            var libraryServiceMock = new Mock<ILibraryService>();
            var testUserId = Guid.NewGuid();
            libraryServiceMock.Setup(x => x.GetByIdAsync(100, It.IsAny<Guid>)).ReturnsAsync((BookDisplayModel?)null);
            var controller = CreateControllerWithUser(libraryServiceMock.Object,testUserId);

            var result = await controller.GetById(100);

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task PurchaseBook_WithInvalidId_ReturnsBadRequest(int bookId)
        {
            var libraryServiceMock = new Mock<ILibraryService>();
            var testUserId = Guid.NewGuid();
            libraryServiceMock.Setup(x => x.PurchaseBookAsync(bookId, It.IsAny<Guid>())).ReturnsAsync(false);
            var controller = CreateControllerWithUser(libraryServiceMock.Object, testUserId);

            var result = await controller.PurchaseBook(bookId);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public async Task PurchaseBook_WhenFailed_ReturnsBadRequest()
        {
            var libraryServiceMock = new Mock<ILibraryService>();
            var testUserId = Guid.NewGuid();
            libraryServiceMock.Setup(x => x.PurchaseBookAsync(1, It.IsAny<Guid>())).ReturnsAsync(false);
            var controller = CreateControllerWithUser(libraryServiceMock.Object, testUserId);

            var result = await controller.PurchaseBook(1);

            result.Should().BeOfType<BadRequestObjectResult>();
        }





        private LibraryController CreateControllerWithUser(ILibraryService service, Guid userId)
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) });
            return new LibraryController(service)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
                }
            };
        }

    }
}
