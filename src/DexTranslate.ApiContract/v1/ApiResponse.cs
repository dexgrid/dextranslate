using System.Net;

namespace DexTranslate.ApiContract.v1
{
    public class ApiResponse
    {
        public int StatusCode { get; }

        public string Message { get; }

        public ApiResponse(HttpStatusCode statusCode, string message)
        {
            StatusCode = (int)statusCode;
            Message = message;
        }
    }
}