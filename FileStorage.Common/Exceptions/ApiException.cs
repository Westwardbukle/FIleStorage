using System.Net;

namespace FileStorage.Common.Exceptions;

public class ApiException : Exception
{
    private readonly HttpStatusCode _httpStatusCode;
    
    public ApiException(HttpStatusCode statusCode,string message) : base(message)
    {
        _httpStatusCode = statusCode;
    }

    public HttpStatusCode GetStatusCode()
    {
        return _httpStatusCode;
    }
}