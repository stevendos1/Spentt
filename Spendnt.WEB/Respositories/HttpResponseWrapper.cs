using System.Net;

namespace Spendnt.WEB.Repositories
{
#nullable enable
    public sealed class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; }

        public T? Response { get; }

        public HttpResponseMessage HttpResponseMessage { get; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var codigoEstatus = HttpResponseMessage.StatusCode;
            if (codigoEstatus == HttpStatusCode.NotFound)//404
            {
                return "Recurso no encontrado";
            }
            else if (codigoEstatus == HttpStatusCode.BadRequest)//400
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (codigoEstatus == HttpStatusCode.Unauthorized)//401
            {
                return " Debes loguearte para realizar esta acción";
            }
            else if (codigoEstatus == HttpStatusCode.Forbidden)//403
            {
                return " No tienes permisos para ejecutar esta acción";
            }

            return "Ha ocurrido un error inesperado";
        }
    }
}
