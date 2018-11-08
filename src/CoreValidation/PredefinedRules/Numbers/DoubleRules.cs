using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DoubleRules
    {
        private static bool AreClose(double a, double b, double tolerance)
        {
            return Math.Abs(a - b) < tolerance;
        }

        public static IMemberSpecificationBuilder<TModel, double> CloseTo<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double value, double tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.Valid(m => AreClose(m, value, tolerance), Phrases.Keys.Numbers.CloseTo, new[] {Arg.Number(nameof(value), value), Arg.Number(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecificationBuilder<TModel, double> NotCloseTo<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double value, double tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.Valid(m => !AreClose(m, value, tolerance), Phrases.Keys.Numbers.NotCloseTo, new[] {Arg.Number(nameof(value), value), Arg.Number(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecificationBuilder<TModel, double> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, double> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, double> Between<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double min, double max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }
    }
}