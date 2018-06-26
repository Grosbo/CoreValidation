using System;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class FloatRules
    {
        private static bool AreClose(float a, float b, float tolerance)
        {
            return Math.Abs(a - b) < tolerance;
        }

        public static IMemberSpecification<TModel, float> CloseTo<TModel>(this IMemberSpecification<TModel, float> @this, float value, float tolerance = 0.0000001f, string message = null)
            where TModel : class
        {
            return @this.Valid(m => AreClose(m, value, tolerance), message ?? Phrases.Keys.Numbers.CloseTo, new[] {new NumberArg(nameof(value), value), new NumberArg(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecification<TModel, float> NotCloseTo<TModel>(this IMemberSpecification<TModel, float> @this, float value, float tolerance = 0.0000001f, string message = null)
            where TModel : class
        {
            return @this.Valid(m => !AreClose(m, value, tolerance), message ?? Phrases.Keys.Numbers.NotCloseTo, new[] {new NumberArg(nameof(value), value), new NumberArg(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecification<TModel, float> GreaterThan<TModel>(this IMemberSpecification<TModel, float> @this, float min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, float> LessThan<TModel>(this IMemberSpecification<TModel, float> @this, float max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, float> Between<TModel>(this IMemberSpecification<TModel, float> @this, float min, float max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}