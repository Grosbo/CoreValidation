using System;
using System.Linq;
using CoreValidation.Errors;
using Xunit;

namespace CoreValidation.UnitTests.Errors
{
    public class MessageVariablesParserTests
    {
        [Theory]
        [InlineData("{single stuff")]
        [InlineData("single stuff}")]
        [InlineData("single }{ stuff")]
        [InlineData("single stuff")]
        [InlineData("")]
        public void Should_NotParse_When_NoVariable(string message)
        {
            var variables = MessageVariablesParser.Parse(message);

            Assert.Empty(variables);
        }

        [Theory]
        [InlineData("abc  {invalid|param1=value1param1=value2} def")]
        [InlineData("abc  {invalid|param1value1param1value2} def")]
        [InlineData("abc  {invalid|=} def")]
        [InlineData("abc  {invalid||} def")]
        public void Should_NotParse_When_InvalidParameters(string message)
        {
            var variables = MessageVariablesParser.Parse(message);

            Assert.Empty(variables);
        }


        [Theory]
        [InlineData("abc  {|param1=value1} def")]
        [InlineData("abc  {  |param1=value1} def")]
        public void Should_NotParse_When_EmptyName(string message)
        {
            var variables = MessageVariablesParser.Parse(message);

            Assert.Empty(variables);
        }

        [Fact]
        public void Should_NotParse_When_DuplicateParameter()
        {
            var variables = MessageVariablesParser.Parse("abc  {invalid|param1=value1|param1=value2} def");

            Assert.Empty(variables);
        }

        [Fact]
        public void Should_Parse_And_SquashDuplicates()
        {
            var variables = MessageVariablesParser.Parse("abc {single|param1=value1}  def {single|param1=value1}");

            Assert.Single(variables);

            Assert.Equal("{single|param1=value1}", variables.Single().Key);
            Assert.Equal("single", variables.Single().Value.Name);
            Assert.Equal(1, variables.Single().Value.Parameters.Count);
            Assert.Equal("value1", variables.Single().Value.Parameters["param1"]);
        }

        [Fact]
        public void Should_Parse_When_ManySameVariables_With_DifferentParameters()
        {
            var variables = MessageVariablesParser.Parse("abc {first|p1=v1} {first|p21=v21|p22=v22} def {first}");

            Assert.Equal(3, variables.Count);

            Assert.Equal("{first|p1=v1}", variables.ElementAt(0).Key);
            Assert.Equal("first", variables.ElementAt(0).Value.Name);

            Assert.Equal(1, variables.ElementAt(0).Value.Parameters.Count);
            Assert.Equal("v1", variables.ElementAt(0).Value.Parameters["p1"]);

            Assert.Equal("{first|p21=v21|p22=v22}", variables.ElementAt(1).Key);
            Assert.Equal("first", variables.ElementAt(1).Value.Name);
            Assert.Equal(2, variables.ElementAt(1).Value.Parameters.Count);
            Assert.Equal("v21", variables.ElementAt(1).Value.Parameters["p21"]);
            Assert.Equal("v22", variables.ElementAt(1).Value.Parameters["p22"]);

            Assert.Equal("{first}", variables.ElementAt(2).Key);
            Assert.Equal("first", variables.ElementAt(2).Value.Name);
            Assert.Null(variables.ElementAt(2).Value.Parameters);
        }

        [Fact]
        public void Should_Parse_When_ManyVariables()
        {
            var variables = MessageVariablesParser.Parse("abc {first} {second} def {third}");

            Assert.Equal(3, variables.Count);

            Assert.Equal("{first}", variables.ElementAt(0).Key);
            Assert.Equal("first", variables.ElementAt(0).Value.Name);
            Assert.Null(variables.ElementAt(0).Value.Parameters);

            Assert.Equal("{second}", variables.ElementAt(1).Key);
            Assert.Equal("second", variables.ElementAt(1).Value.Name);
            Assert.Null(variables.ElementAt(1).Value.Parameters);

            Assert.Equal("{third}", variables.ElementAt(2).Key);
            Assert.Equal("third", variables.ElementAt(2).Value.Name);
            Assert.Null(variables.ElementAt(2).Value.Parameters);
        }

        [Fact]
        public void Should_Parse_When_ManyVariables_With_Parameters()
        {
            var variables = MessageVariablesParser.Parse("abc {first|p1=v1} {second|p21=v21|p22=v22} def {third|p31=v31|p32=v32|p33=v33}");

            Assert.Equal(3, variables.Count);

            Assert.Equal("{first|p1=v1}", variables.ElementAt(0).Key);
            Assert.Equal("first", variables.ElementAt(0).Value.Name);

            Assert.Equal(1, variables.ElementAt(0).Value.Parameters.Count);
            Assert.Equal("v1", variables.ElementAt(0).Value.Parameters["p1"]);

            Assert.Equal("{second|p21=v21|p22=v22}", variables.ElementAt(1).Key);
            Assert.Equal("second", variables.ElementAt(1).Value.Name);
            Assert.Equal(2, variables.ElementAt(1).Value.Parameters.Count);
            Assert.Equal("v21", variables.ElementAt(1).Value.Parameters["p21"]);
            Assert.Equal("v22", variables.ElementAt(1).Value.Parameters["p22"]);

            Assert.Equal("{third|p31=v31|p32=v32|p33=v33}", variables.ElementAt(2).Key);
            Assert.Equal("third", variables.ElementAt(2).Value.Name);
            Assert.Equal(3, variables.ElementAt(2).Value.Parameters.Count);
            Assert.Equal("v31", variables.ElementAt(2).Value.Parameters["p31"]);
            Assert.Equal("v32", variables.ElementAt(2).Value.Parameters["p32"]);
            Assert.Equal("v33", variables.ElementAt(2).Value.Parameters["p33"]);
        }

        [Fact]
        public void Should_Parse_When_SingleVariable()
        {
            var variables = MessageVariablesParser.Parse("abc {single} def");

            Assert.Single(variables);

            Assert.Equal("{single}", variables.Single().Key);
            Assert.Equal("single", variables.Single().Value.Name);
            Assert.Null(variables.Single().Value.Parameters);
        }

        [Fact]
        public void Should_Parse_When_SingleVariable_With_ManyParameters()
        {
            var variables = MessageVariablesParser.Parse("abc {single|param1=value1|param2=value2|param3=value3} def");

            Assert.Single(variables);

            Assert.Equal("{single|param1=value1|param2=value2|param3=value3}", variables.Single().Key);
            Assert.Equal("single", variables.Single().Value.Name);
            Assert.Equal(3, variables.Single().Value.Parameters.Count);
            Assert.Equal("value1", variables.Single().Value.Parameters["param1"]);
            Assert.Equal("value2", variables.Single().Value.Parameters["param2"]);
            Assert.Equal("value3", variables.Single().Value.Parameters["param3"]);
        }

        [Fact]
        public void Should_Parse_When_SingleVariable_With_SingleParameter()
        {
            var variables = MessageVariablesParser.Parse("abc {single|param1=value1} def");

            Assert.Single(variables);

            Assert.Equal("{single|param1=value1}", variables.Single().Key);
            Assert.Equal("single", variables.Single().Value.Name);
            Assert.Equal(1, variables.Single().Value.Parameters.Count);
            Assert.Equal("value1", variables.Single().Value.Parameters["param1"]);
        }

        [Fact]
        public void Should_ParseOnlyValidOnes()
        {
            var variables = MessageVariablesParser.Parse("{valid} abc {invalid|param1=value1|param1=value2} {valid2|param=value} def {invalid|param1=value1param1=value2} xyz {invalid|param1value1param1value2}");

            Assert.Equal(2, variables.Count);

            Assert.Equal("{valid}", variables.ElementAt(0).Key);
            Assert.Equal("valid", variables.ElementAt(0).Value.Name);
            Assert.Null(variables.ElementAt(0).Value.Parameters);

            Assert.Equal(1, variables.ElementAt(1).Value.Parameters.Count);
            Assert.Equal("{valid2|param=value}", variables.ElementAt(1).Key);
            Assert.Equal("valid2", variables.ElementAt(1).Value.Name);
            Assert.Equal("value", variables.ElementAt(1).Value.Parameters["param"]);
        }

        [Fact]
        public void Should_ThrowException_When_NullMessage()
        {
            Assert.Throws<ArgumentNullException>(() => { MessageVariablesParser.Parse(null); });
        }
    }
}