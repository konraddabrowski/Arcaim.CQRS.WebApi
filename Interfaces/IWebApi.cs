using Microsoft.AspNetCore.Routing;

namespace Arcaim.CQRS.WebApi.Interfaces
{
    public interface IWebApi : ICommandApi, IQueryApi
    {
        IEndpointRouteBuilder EndpointRouteBuilder { get; }
        string Pattern { get; init; }
        string PatternAction { get; set; }
    }
}
