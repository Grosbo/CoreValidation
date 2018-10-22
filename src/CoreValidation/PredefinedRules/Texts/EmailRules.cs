using System.Net.Mail;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class EmailRules
    {
        public static IMemberSpecificationBuilder<TModel, string> Email<TModel>(this IMemberSpecificationBuilder<TModel, string> @this)
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
            }, Phrases.Keys.Texts.Email);
        }
    }
}