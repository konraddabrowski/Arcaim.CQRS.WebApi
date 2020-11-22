using Arcaim.CQRS.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Arcaim.CQRS.WebApi
{
    public interface IQueryApi
    {
        IEndpointConventionBuilder Get<TQuery, TResult>()
            where TQuery : IQuery<TResult>
            where TResult : class;
        IEndpointConventionBuilder Get(RequestDelegate requestDelegate);
    }
}