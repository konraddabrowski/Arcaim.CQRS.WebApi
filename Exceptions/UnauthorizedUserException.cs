using Microsoft.AspNetCore.Http;

namespace Arcaim.CQRS.WebApi.Exceptions;

public class UnauthorizedUserException : WebApiException
{
  public override string Code => "unauthorized_user";
  public override int StatusCode => StatusCodes.Status401Unauthorized;

  private UnauthorizedUserException(string message) : base(message)
  {
  }

  private static UnauthorizedUserException CreateUnauthorizedUserException(string message)
    => new UnauthorizedUserException(message);
  
  public static UnauthorizedUserException Create()
    => CreateUnauthorizedUserException("The user is not authorized");
}