using Microsoft.AspNetCore.Routing;

namespace Arcaim.CQRS.WebApi
{
    public interface IWebApi : ICommandApi, IQueryApi
    {
        IEndpointRouteBuilder Builder { get; }
        string Pattern { get; }
    }
}