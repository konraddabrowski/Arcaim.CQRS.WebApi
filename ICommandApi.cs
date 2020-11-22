using Arcaim.CQRS.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Arcaim.CQRS.WebApi
{
    public interface ICommandApi
    {
        IEndpointConventionBuilder Post<T>() where T : ICommand;
        IEndpointConventionBuilder Post(RequestDelegate requestDelegate);
        IEndpointConventionBuilder Put<T>() where T : ICommand;
        IEndpointConventionBuilder Put(RequestDelegate requestDelegate);
        IEndpointConventionBuilder Delete<T>() where T : ICommand;
        IEndpointConventionBuilder Delete(RequestDelegate requestDelegate);
    }
}