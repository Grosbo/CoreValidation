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

        public static IMemberSpecificationBuilder<TModel, double> CloseTo<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double value, double tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.Valid(m => AreClose(m, value, tolerance), Phrases.Keys.Numbers.CloseTo, new[] {NumberArg.Create(nameof(value), value), NumberArg.Create(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecificationBuilder<TModel, double> NotCloseTo<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double value, double tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.Valid(m => !AreClose(m, value, tolerance), Phrases.Keys.Numbers.NotCloseTo, new[] {NumberArg.Create(nameof(value), value), NumberArg.Create(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecificationBuilder<TModel, double> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, double> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, double> Between<TModel>(this IMemberSpecificationBuilder<TModel, double> @this, double min, double max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }
    }
}