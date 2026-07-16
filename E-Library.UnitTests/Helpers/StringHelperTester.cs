using E_Library.Helpers;
using E_Library.UnitTests.DataTests;
using FluentAssertions;
using Xunit.Abstractions;
namespace E_Library.UnitTests.Helpers
{
    public class StringHelperTester
    {
        //Shorter way to instantiate dependencies using a field initializer without using a constructor
        private readonly StringHelper _stringHelper = new StringHelper();

        [Fact]//(Skip = "Skipping this test to learn skipping",DisplayName ="NullOrEmptyChecker with an Empty String Should Return False")]
        [Trait("Category", "String Validator")]
        public void NullOrEmptyChecker_WithEmptyString_ShouldReturnFalse()
        {
            //Arrange

            //Act
            var result = StringHelper.NullOrEmptyChecker(" ");
            //Assert
            result.Should().Be(false);
            //Assert.SkipUnless()
        }


        [Theory] //Use "Theory" when u wanna test multiple inputs of the same method

        [InlineData("With Inline Data, U enter all the inputs u want here and it'll pass it via parameters", 17)]
        [InlineData("for testing which will avoid writing lines of code for each input ", 12)]

        [MemberData(nameof(GetRandomSentences))] //if there are many inputs, u can create a method and add all ur inputs there

        [ClassData(typeof(StringHelperDataTest2))] //if u need to add a class u can do that too with "ClassData"

        [CustomStringHelperData]
        public void CountWords_WithMultipleWords_ShouldReturnCorrectWordCount(string text, int expectedCount)
        {
            var result = _stringHelper.CountWords(text);

            result.Should().Be(expectedCount);
        }

        [Theory]
        [ClassData(typeof(StringHelperDataTestForTrimmer))]
        [Trait("Category", "String Validator")]
        public void TrimString_WithLeadingAndTrailingWhiteSpaces_ShouldRemoveWhiteSpaces(string text, string expectedOutput)
        {
            var result = _stringHelper.TrimString(text);
            result.Should().Be(expectedOutput);
        }


        public static IEnumerable<object[]> GetRandomSentences() =>
        [
             ["The Blue Fox Jumped \n Over The Brown Bear",8],
             ["something that has 5 words",5],
             ["something that has more than 5 words",7],
             ["some words of wisom",4]
             //yield return ["something here"] ==> this uses more lines of code
        ];
    }
}
