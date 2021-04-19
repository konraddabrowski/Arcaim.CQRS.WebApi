using System;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Microsoft.AspNetCore.Builder;

namespace Arcaim.CQRS.WebApi.Interfaces
{
    public interface IWebAction
    {
        IWebAction Action(Func<ICommandDispatcher, IQueryDispatcher, IEndpointConventionBuilder> requestDelegate);
        IWebAction Action(string pattern, Func<ICommandDispatcher, IQueryDispatcher, IEndpointConventionBuilder> requestDelegate);
        IWebAction Command(Func<IEndpointConventionBuilder> requestDelegate);
        IWebAction Command(string pattern, Func<IEndpointConventionBuilder> requestDelegate);
        IWebAction Command(Func<ICommandDispatcher, IEndpointConventionBuilder> requestDelegate);
        IWebAction Command(string pattern, Func<ICommandDispatcher, IEndpointConventionBuilder> requestDelegate);
        IWebAction Query(Func<IEndpointConventionBuilder> requestDelegate);
        IWebAction Query(string pattern, Func<IEndpointConventionBuilder> requestDelegate);
        IWebAction Query(Func<IQueryDispatcher, IEndpointConventionBuilder> requestDelegate);
        IWebAction Query(string pattern, Func<IQueryDispatcher, IEndpointConventionBuilder> requestDelegate);
    }
}