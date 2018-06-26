﻿using System.Net.Mail;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class EmailRules
    {
        public static IMemberSpecification<TModel, string> Email<TModel>(this IMemberSpecification<TModel, string> @this, string message = null)
            where TModel : class
        {
            return @this.Valid(m =>
            {
                try
                {
                    var addr = new MailAddress(m);

                    return addr.Address == m;
                }
                catch
                {
                    return false;
                }
            }, message ?? Phrases.Keys.Texts.Email);
        }
    }
}