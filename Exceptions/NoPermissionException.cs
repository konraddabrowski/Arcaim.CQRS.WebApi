using Microsoft.AspNetCore.Http;

namespace Arcaim.CQRS.WebApi.Exceptions;

public class NoPermissionException : WebApiException
{
  public override string Code => "no_permission";
  public override int StatusCode => StatusCodes.Status403Forbidden;

  private NoPermissionException(string message) : base(message)
  {
  }

  private static NoPermissionException CreateNoPermissionException(string message)
      => new NoPermissionException(message);
  
  public static NoPermissionException Create()
    => CreateNoPermissionException("The user is not authorized to this content");
}