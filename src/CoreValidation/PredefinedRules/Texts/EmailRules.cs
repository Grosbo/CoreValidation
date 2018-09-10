using System.Net.Mail;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class EmailRules
    {
        public static IMemberSpecificationBuilder<TModel, string> Email<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, string message = null)
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