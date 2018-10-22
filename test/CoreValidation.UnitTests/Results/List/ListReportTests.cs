using System;
using CoreValidation.Results.List;
using Xunit;

namespace CoreValidation.UnitTests.Results.List
{
    public class ListReportTests
    {
        [Fact]
        public void ToString_Should_CombineAllMessages_When_ErrorMessages()
        {
            var listReport = new ListReport
            {
                "Test1",
                "Test2",
                "Test3"
            };

            var expected = $"Test1{Environment.NewLine}Test2{Environment.NewLine}Test3{Environment.NewLine}";

            Assert.Equal(expected, listReport.ToString());
        }

        [Fact]
        public void ToString_Should_CombineAllMessages_When_ErrorMessages_And_WithoutDuplicates()
        {
            var listReport = new ListReport
            {
                "Test1",
                "Test2",
                "Test3",
                "Test3",
                "Test2",
                "Test1"
            };

            var expected = $"Test1{Environment.NewLine}Test2{Environment.NewLine}Test3{Environment.NewLine}";

            Assert.Equal(expected, listReport.ToString());
        }

        [Fact]
        public void ToString_Should_ReturnEmptyString_When_NoErrorMessages()
        {
            var listReport = new ListReport();

            Assert.Equal(string.Empty, listReport.ToString());
        }

        [Fact]
        public void ToString_Should_ReturnSingleMessage_When_SingleErrorMessage()
        {
            var listReport = new ListReport
            {
                "Test1"
            };

            var expected = $"Test1{Environment.NewLine}";

            Assert.Equal(expected, listReport.ToString());
        }
    }
}