using System;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class FloatRules
    {
        private static bool AreClose(float a, float b, float tolerance)
        {
            return Math.Abs(a - b) < tolerance;
        }

        public static IMemberSpecificationBuilder<TModel, float> CloseTo<TModel>(this IMemberSpecificationBuilder<TModel, float> @this, float value, float tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.Valid(m => AreClose(m, value, tolerance), Phrases.Keys.Numbers.CloseTo, new[] {NumberArg.Create(nameof(value), value), NumberArg.Create(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecificationBuilder<TModel, float> NotCloseTo<TModel>(this IMemberSpecificationBuilder<TModel, float> @this, float value, float tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.Valid(m => !AreClose(m, value, tolerance), Phrases.Keys.Numbers.NotCloseTo, new[] {NumberArg.Create(nameof(value), value), NumberArg.Create(nameof(tolerance), tolerance)});
        }

        public static IMemberSpecificationBuilder<TModel, float> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, float> @this, float min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, float> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, float> @this, float max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, float> Between<TModel>(this IMemberSpecificationBuilder<TModel, float> @this, float min, float max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }
    }
}