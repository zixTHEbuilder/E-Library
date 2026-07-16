using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace E_Library.UnitTests.DataTests
{
    internal class CustomStringHelperData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var data = new TheoryData<string,int>
            {
                {GetDataTest(), 6 },
                { "The Blue Fox Jumped \n Over The Brown Bear", 8 },
                {"something that has 5 words", 5 },
                {"something that has more than 5 words", 7},
                {"some words of wisom", 4 }
            };

            StringBuilder sb = new StringBuilder("This is test string number");
            for (int i = 0; i <= 15; i++)
            {
                sb.Append($" {i}");
                data.Add(sb.ToString(),6+i);
            }
            return data;
        }
        public static string GetDataTest()
        {
            return "Random Data Test For Testing Purposes";
        }
    }
}
