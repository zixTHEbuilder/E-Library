using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace E_Library.UnitTests.DataTests
{
    //using "object" here is unsafe because u could provide a "string" in the "int" place and it would not give u an error
    public class StringHelperDataTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [GetDataTest(), 6];
            yield return ["The Blue Fox Jumped \n Over The Brown Bear", 8];
            yield return ["something that has 5 words", 5];
            yield return ["something that has more than 5 words", 7];
            yield return ["some words of wisom", 4];

            StringBuilder sb = new StringBuilder("This is test string number");
            for (int i = 0; i <= 15; i++)
            {
                var wordCount = i;
                sb.Append($" {i}");
                yield return [sb.ToString(),6+i];
            }
        }


        public static string GetDataTest()
        {
            return "Random Data Test For Testing Purposes";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    //we inherit it from the "TheoryData" so that we can give "string" and "int" so that there's an error if wrong values are entered
    public class StringHelperDataTest2 : TheoryData<string,int>
    {
        public StringHelperDataTest2()
        {
            Add(GetDataTest(), 6);
            Add("The Blue Fox Jumped \n Over The Brown Bear", 8);
            Add("something that has 5 words", 5);
            Add("something that has more than 5 words", 7);
            Add("some words of wisom", 4);

            StringBuilder sb = new StringBuilder("This is test string number");
            for (int i = 0; i <= 15; i++)
            {
                var wordCount = i;  
                sb.Append($" {i}");
                Add(sb.ToString(), 6 + i);
            }
        }

        public static string GetDataTest()
        {
            return "Random Data Test For Testing Purposes";
        }
    }
    public class StringHelperDataTestForTrimmer : TheoryData<string?,string?>
    {
        public StringHelperDataTestForTrimmer()
        {
            Add("  goo goo gaa gaa  ", "goo goo gaa gaa");
            Add(" ","");
            Add(" leading", "leading");
            Add("trailing ", "trailing");
            Add(null, null);
        }
    }
}
