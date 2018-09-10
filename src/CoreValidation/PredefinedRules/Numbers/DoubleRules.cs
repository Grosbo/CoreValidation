using System;
using CoreValidation.Errors.Args;
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

        public static IMemberSpecificationBuilder<TModel, double> CloseTo<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double value, double tolerance = 0.0000001f, string message = null)
            where TModel : class
        {
            return @this.Valid(m => AreClose(m, value, tolerance), message ?? Phrases.Keys.Numbers.CloseTo, new[] {new NumberArg(nameof(value), value), new NumberArg(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecificationBuilder<TModel, double> NotCloseTo<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double value, double tolerance = 0.0000001f, string message = null)
            where TModel : class
        {
            return @this.Valid(m => !AreClose(m, value, tolerance), message ?? Phrases.Keys.Numbers.NotCloseTo, new[] {new NumberArg(nameof(value), value), new NumberArg(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecificationBuilder<TModel, double> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, double> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, double> Between<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double min, double max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}