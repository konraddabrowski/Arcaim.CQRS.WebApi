using Arcaim.CQRS.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Arcaim.CQRS.WebApi.Interfaces;

public interface IQueryApi
{
  IEndpointConventionBuilder Get<TQuery, TResult>()
    where TQuery : IQuery<TResult>, new()
    where TResult : new();
  
  IEndpointConventionBuilder Get(RequestDelegate requestDelegate);
}