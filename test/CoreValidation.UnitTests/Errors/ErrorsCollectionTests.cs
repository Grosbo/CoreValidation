using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using Xunit;

namespace CoreValidation.UnitTests.Errors
{
    public class ErrorsCollectionTests
    {
        public static IEnumerable<object[]> AddError_Should_ThrowException_When_ErrorIsNull_Data()
        {
            yield return new object[] {null, new Error("x")};
            yield return new object[] {"test", null};
            yield return new object[] {null, null};
        }

        [Theory]
        [MemberData(nameof(AddError_Should_ThrowException_When_ErrorIsNull_Data))]
        public void AddError_Should_ThrowException_When_ErrorIsNull(string memberName, object error)
        {
            var errorsCollection = new ErrorsCollection();

            Assert.Throws<ArgumentNullException>(() => { errorsCollection.AddError(memberName, (Error)error); });
        }

        public static IEnumerable<object[]> AddError_Should_ThrowException_When_CollectionIsNull_Data()
        {
            yield return new object[] {null, new ErrorsCollection()};
            yield return new object[] {"test", null};
            yield return new object[] {null, null};
        }

        [Theory]
        [MemberData(nameof(AddError_Should_ThrowException_When_CollectionIsNull_Data))]
        public void AddError_Should_ThrowException_When_CollectionIsNull(string memberName, ErrorsCollection innerCollection)
        {
            var errorsCollection = new ErrorsCollection();

            Assert.Throws<ArgumentNullException>(() => { errorsCollection.AddError(memberName, innerCollection); });
        }

        private class ErrorsCollectionImplementation : IErrorsCollection
        {
            public bool IsEmpty => (Errors?.Any() == false) && (Members?.Any() == false);

            public IReadOnlyCollection<IError> Errors { get; set; } = new Error[] { };

            public IReadOnlyDictionary<string, IErrorsCollection> Members { get; set; } = new Dictionary<string, IErrorsCollection>();
        }

        [Fact]
        public void AddError_Should_Add_SingleError()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError(new Error("test123"));

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"test123"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_Add_When_MultipleErrors()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError(new Error("test123"));
            errorsCollection.AddError(new Error("test321"));
            errorsCollection.AddError(new Error("foo"));
            errorsCollection.AddError(new Error("bar"));

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"test123", "test321", "foo", "bar"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_Add_WithDuplicates()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError(new Error("test123"));
            errorsCollection.AddError(new Error("test123"));
            errorsCollection.AddError(new Error("foo"));
            errorsCollection.AddError(new Error("foo"));

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"test123", "test123", "foo", "foo"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddInnerCollection()
        {
            var errorsCollection = new ErrorsCollection();

            var innerCollection = new ErrorsCollection();
            innerCollection.AddError(new Error("test123"));
            innerCollection.AddError(new Error("test321"));
            innerCollection.AddError(new Error("foo"));
            innerCollection.AddError(new Error("bar"));

            errorsCollection.AddError("test", innerCollection);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"], new[] {"test123", "test321", "foo", "bar"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddInnerCollection_When_MemberErrors()
        {
            var innerCollection = new ErrorsCollection();
            innerCollection.AddError("inner", new Error("test123"));
            innerCollection.AddError("inner", new Error("test321"));
            innerCollection.AddError("inner2", new Error("foo"));
            innerCollection.AddError("inner2", new Error("bar"));

            var errorsCollection = new ErrorsCollection();
            errorsCollection.AddError("test", innerCollection);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test"});

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["test"], new[] {"inner", "inner2"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"].Members["inner"], new[] {"test123", "test321"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"].Members["inner2"], new[] {"foo", "bar"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddInnerCollection_When_Multiple_To_MultipleMembers()
        {
            var errorsCollection = new ErrorsCollection();

            var innerCollection = new ErrorsCollection();
            innerCollection.AddError("inner", new Error("test123"));
            innerCollection.AddError("inner", new Error("test321"));

            var innerCollection2 = new ErrorsCollection();
            innerCollection2.AddError("inner2", new Error("foo"));
            innerCollection2.AddError("inner2", new Error("bar"));

            errorsCollection.AddError("test", innerCollection);
            errorsCollection.AddError("test2", innerCollection2);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test", "test2"});

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["test"], new[] {"inner"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"].Members["inner"], new[] {"test123", "test321"});

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["test2"], new[] {"inner2"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test2"].Members["inner2"], new[] {"foo", "bar"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddInnerCollection_When_Multiple_To_SameMember()
        {
            var errorsCollection = new ErrorsCollection();

            var innerCollection = new ErrorsCollection();
            innerCollection.AddError("inner", new Error("test123"));
            innerCollection.AddError("inner", new Error("test321"));

            var innerCollection2 = new ErrorsCollection();
            innerCollection.AddError("inner2", new Error("foo"));
            innerCollection.AddError("inner2", new Error("bar"));

            errorsCollection.AddError("test", innerCollection);
            errorsCollection.AddError("test", innerCollection2);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test"});
            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["test"], new[] {"inner", "inner2"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"].Members["inner"], new[] {"test123", "test321"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"].Members["inner2"], new[] {"foo", "bar"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddInnerCollection_When_Multiple_To_SameMember_SingleLevel_WithDuplicates()
        {
            var errorsCollection = new ErrorsCollection();

            var innerCollection = new ErrorsCollection();
            innerCollection.AddError(new Error("test123"));
            innerCollection.AddError(new Error("test321"));
            innerCollection.AddError(new Error("foo"));

            var innerCollection2 = new ErrorsCollection();
            innerCollection2.AddError(new Error("foo"));
            innerCollection2.AddError(new Error("bar"));
            innerCollection2.AddError(new Error("test123"));

            errorsCollection.AddError("test", innerCollection);
            errorsCollection.AddError("test", innerCollection2);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"], new[] {"test123", "test321", "foo", "foo", "bar", "test123"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddInnerCollection_When_Multiple_To_SameMember_WithDuplicates()
        {
            var errorsCollection = new ErrorsCollection();

            var innerCollection = new ErrorsCollection();
            innerCollection.AddError("inner", new Error("test321"));
            innerCollection.AddError("inner", new Error("foo"));
            innerCollection.AddError("inner", new Error("bar"));

            var innerCollection2 = new ErrorsCollection();
            innerCollection2.AddError("inner", new Error("test123"));
            innerCollection2.AddError("inner", new Error("test321"));
            innerCollection2.AddError("inner", new Error("foo"));

            errorsCollection.AddError("test", innerCollection);
            errorsCollection.AddError("test", innerCollection2);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test"});
            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["test"], new[] {"inner"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"].Members["inner"], new[] {"test321", "foo", "bar", "test123", "test321", "foo"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddNothing_When_EmptyCollection()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("test", new ErrorsCollection());

            Assert.NotNull(errorsCollection.Members);
            Assert.Empty(errorsCollection.Members);
            Assert.True(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddToMember_When_MultipleErrors()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("test", new Error("test123"));
            errorsCollection.AddError("test", new Error("test321"));
            errorsCollection.AddError("test", new Error("foo"));
            errorsCollection.AddError("test", new Error("bar"));

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test"});

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"], new[] {"test123", "test321", "foo", "bar"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddToMember_When_MultipleMembers_And_MultipleErrors()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("test", new Error("test123"));
            errorsCollection.AddError("test", new Error("test321"));
            errorsCollection.AddError("test2", new Error("foo"));
            errorsCollection.AddError("test2", new Error("bar"));

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test", "test2"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"], new[] {"test123", "test321"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test2"], new[] {"foo", "bar"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddToMember_When_SingleErrors()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("test", new Error("test123"));

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test"});

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"], new[] {"test123"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_AddToMember_WithDuplicates()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("test", new Error("test123"));
            errorsCollection.AddError("test", new Error("test123"));
            errorsCollection.AddError("test", new Error("test321"));
            errorsCollection.AddError("test", new Error("test321"));

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"test"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["test"], new[] {"test123", "test123", "test321", "test321"});

            Assert.False(errorsCollection.IsEmpty);
        }

        [Fact]
        public void AddError_Should_ThrowException_When_AddingCollection_And_MemberIsEmptyString()
        {
            var errorsCollection = new ErrorsCollection();

            Assert.Throws<ArgumentException>(() => { errorsCollection.AddError(string.Empty, new ErrorsCollection()); });
        }

        [Fact]
        public void AddError_Should_ThrowException_When_AddingError_And_MemberIsEmptyString()
        {
            var errorsCollection = new ErrorsCollection();

            Assert.Throws<ArgumentException>(() => { errorsCollection.AddError(string.Empty, new Error("test123")); });
        }

        [Fact]
        public void AddError_Should_ThrowException_When_NullCollection()
        {
            var errorsCollection = new ErrorsCollection();

            Assert.Throws<ArgumentNullException>(() => { errorsCollection.AddError("test", null as IErrorsCollection); });
        }

        [Fact]
        public void Constructor_Should_Initialize_Internals()
        {
            var errorsCollection = new ErrorsCollection();

            Assert.NotNull(errorsCollection);
            Assert.NotNull(errorsCollection.Errors);
            Assert.Empty(errorsCollection.Errors);
            Assert.NotNull(errorsCollection.Members);
            Assert.Empty(errorsCollection.Members);
            Assert.True(errorsCollection.IsEmpty);
        }

        [Fact]
        public void Include_Should_Include_When_BothSingleLevel()
        {
            var errorsCollection = new ErrorsCollection();
            errorsCollection.AddError(new Error("test1"));
            errorsCollection.AddError(new Error("test2"));

            var another = new ErrorsCollection();
            errorsCollection.AddError(new Error("foo"));
            errorsCollection.AddError(new Error("bar"));

            errorsCollection.Include(another);

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"test1", "test2", "foo", "bar"});
        }

        [Fact]
        public void Include_Should_Include_When_ContainsMultipleNestedLevels()
        {
            var nested11 = new ErrorsCollection();

            nested11.AddError("arg11", new Error("val111"));
            nested11.AddError("arg11", new Error("val112"));

            var nested12 = new ErrorsCollection();

            nested12.AddError("arg12", new Error("val121"));
            nested12.AddError("arg12", new Error("val122"));

            var nested1 = new ErrorsCollection();

            nested1.AddError("arg1", new Error("val11"));
            nested1.AddError("arg1", new Error("val12"));

            nested1.AddError("arg1", nested11);
            nested1.AddError("arg1", nested12);

            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("foo", nested1);

            var anotherNested12 = new ErrorsCollection();

            anotherNested12.AddError("arg12", new Error("val122"));
            anotherNested12.AddError("arg12", new Error("val123"));

            var anotherNested13 = new ErrorsCollection();

            anotherNested13.AddError("arg13", new Error("val131"));
            anotherNested13.AddError("arg13", new Error("val132"));

            var anotherNested1 = new ErrorsCollection();

            anotherNested1.AddError("arg1", new Error("val12"));
            anotherNested1.AddError("arg1", new Error("val13"));
            anotherNested1.AddError("arg1", anotherNested12);
            anotherNested1.AddError("arg1", anotherNested13);

            var another = new ErrorsCollection();

            another.AddError("foo", anotherNested1);

            errorsCollection.Include(another);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"foo"});

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["foo"], new[] {"arg1"});

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"].Members["arg1"], new[] {"val11", "val12", "val12", "val13"});
            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["foo"].Members["arg1"], new[] {"arg11", "arg12", "arg13"});

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"].Members["arg1"].Members["arg11"], new[] {"val111", "val112"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"].Members["arg1"].Members["arg12"], new[] {"val121", "val122", "val122", "val123"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"].Members["arg1"].Members["arg13"], new[] {"val131", "val132"});
        }

        [Fact]
        public void Include_Should_Include_When_ContainsNestedLevel()
        {
            var nested = new ErrorsCollection();

            nested.AddError("foo", new Error("test1"));
            nested.AddError("foo", new Error("test2"));
            nested.AddError("bar", new Error("test3"));
            nested.AddError("bar", new Error("test4"));
            nested.AddError("Y", new Error("555"));

            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("X", nested);

            var anotherNested = new ErrorsCollection();

            anotherNested.AddError("foo", new Error("111"));
            anotherNested.AddError("foo", new Error("222"));

            anotherNested.AddError("bar", new Error("333"));
            anotherNested.AddError("bar", new Error("444"));
            anotherNested.AddError("Z", new Error("666"));

            var another = new ErrorsCollection();

            another.AddError("X", anotherNested);

            errorsCollection.Include(another);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"X"});
            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["X"], new[] {"foo", "bar", "Y", "Z"});

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["X"].Members["foo"], new[] {"test1", "test2", "111", "222"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["X"].Members["bar"], new[] {"test3", "test4", "333", "444"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["X"].Members["Y"], new[] {"555"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["X"].Members["Z"], new[] {"666"});
        }

        [Fact]
        public void Include_Should_Include_When_ContainsSingleLevel()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("foo", new Error("test1"));
            errorsCollection.AddError("foo", new Error("test2"));

            errorsCollection.AddError("bar", new Error("test3"));
            errorsCollection.AddError("bar", new Error("test4"));

            var another = new ErrorsCollection();

            another.AddError("foo", new Error("111"));
            another.AddError("foo", new Error("222"));
            another.AddError("bar", new Error("333"));
            another.AddError("bar", new Error("444"));
            another.AddError("X", new Error("555"));
            another.AddError("Y", new Error("666"));

            errorsCollection.Include(another);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"foo", "bar", "X", "Y"});

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"], new[] {"test1", "test2", "111", "222"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["bar"], new[] {"test3", "test4", "333", "444"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["X"], new[] {"555"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Y"], new[] {"666"});
        }

        [Fact]
        public void Include_Should_IncludeDifferentImplementation_When_ContainsMultipleNestedLevels()
        {
            var nested11 = new ErrorsCollectionImplementation
            {
                Members = new Dictionary<string, IErrorsCollection>
                {
                    {"arg11", new ErrorsCollectionImplementation {Errors = new[] {new Error("val111"), new Error("val112")}}}
                }
            };

            var nested12 = new ErrorsCollectionImplementation
            {
                Members = new Dictionary<string, IErrorsCollection>
                {
                    {"arg12", new ErrorsCollectionImplementation {Errors = new[] {new Error("val121"), new Error("val122")}}}
                }
            };

            var nested1 = new ErrorsCollection();

            nested1.AddError("arg1", new Error("val11"));
            nested1.AddError("arg1", new Error("val12"));
            nested1.AddError("arg1", nested11);
            nested1.AddError("arg1", nested12);

            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("foo", nested1);

            var anotherNested12 = new ErrorsCollectionImplementation
            {
                Members = new Dictionary<string, IErrorsCollection>
                {
                    {"arg12", new ErrorsCollectionImplementation {Errors = new[] {new Error("val122"), new Error("val123")}}}
                }
            };

            var anotherNested13 = new ErrorsCollectionImplementation
            {
                Members = new Dictionary<string, IErrorsCollection>
                {
                    {"arg13", new ErrorsCollectionImplementation {Errors = new[] {new Error("val131"), new Error("val132")}}}
                }
            };

            var anotherNested1 = new ErrorsCollection();

            anotherNested1.AddError("arg1", new Error("val12"));
            anotherNested1.AddError("arg1", new Error("val13"));
            anotherNested1.AddError("arg1", anotherNested12);
            anotherNested1.AddError("arg1", anotherNested13);

            var another = new ErrorsCollection();

            another.AddError("foo", anotherNested1);

            errorsCollection.Include(another);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"foo"});

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["foo"], new[] {"arg1"});

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"].Members["arg1"], new[] {"val11", "val12", "val12", "val13"});
            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["foo"].Members["arg1"], new[] {"arg11", "arg12", "arg13"});

            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"].Members["arg1"].Members["arg11"], new[] {"val111", "val112"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"].Members["arg1"].Members["arg12"], new[] {"val121", "val122", "val122", "val123"});
            ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["foo"].Members["arg1"].Members["arg13"], new[] {"val131", "val132"});
        }

        [Fact]
        public void Include_Should_ThrowException_When_NullArgument()
        {
            var errorsCollection = new ErrorsCollection();

            Assert.Throws<ArgumentNullException>(() => { errorsCollection.Include(null); });
        }
    }
}