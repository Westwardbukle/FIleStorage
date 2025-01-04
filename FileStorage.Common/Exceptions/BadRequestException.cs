using System.Net;

namespace FileStorage.Common.Exceptions;

public class BadRequestException : ApiException
{
    public BadRequestException(string message)
        : base(HttpStatusCode.BadRequest, message)
    {
    }
}