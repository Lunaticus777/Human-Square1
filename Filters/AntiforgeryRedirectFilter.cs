using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Human_Evolution.Filters
{
    /// <summary>
    /// Quand le jeton anti-fraude d'un formulaire n'est plus valide (page ouverte avant
    /// un redémarrage du serveur), on renvoie le visiteur sur la page avec un message
    /// au lieu d'afficher une erreur 400.
    /// </summary>
    public class AntiforgeryRedirectFilter : IAlwaysRunResultFilter
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public AntiforgeryRedirectFilter(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is not IAntiforgeryValidationFailedResult) return;

            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            tempData["ErrorMessage"] = "La page avait expiré. Merci de renvoyer votre message.";

            var target = "/";
            var referer = context.HttpContext.Request.Headers.Referer.ToString();
            if (Uri.TryCreate(referer, UriKind.Absolute, out var uri)
                && string.Equals(uri.Host, context.HttpContext.Request.Host.Host, StringComparison.OrdinalIgnoreCase))
            {
                target = uri.PathAndQuery;
            }

            context.Result = new RedirectResult(target);
        }

        public void OnResultExecuted(ResultExecutedContext context) { }
    }
}
