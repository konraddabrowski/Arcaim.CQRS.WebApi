using Arcaim.CQRS.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Arcaim.CQRS.WebApi.Interfaces
{
    public interface ICommandApi
    {
        IEndpointConventionBuilder Post<T>() where T : class, ICommand;
        IEndpointConventionBuilder Post(RequestDelegate requestDelegate);
        IEndpointConventionBuilder Put<T>() where T : class, ICommand;
        IEndpointConventionBuilder Put(RequestDelegate requestDelegate);
        IEndpointConventionBuilder Delete<T>() where T : class, ICommand;
        IEndpointConventionBuilder Delete(RequestDelegate requestDelegate);
    }
}