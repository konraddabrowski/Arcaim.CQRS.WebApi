using Arcaim.CQRS.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Arcaim.CQRS.WebApi.Interfaces;

public interface ICommandApi
{
  IEndpointConventionBuilder Post<T>() where T : ICommand, new();
  IEndpointConventionBuilder Post(RequestDelegate requestDelegate);
  IEndpointConventionBuilder Put<T>() where T : ICommand, new();
  IEndpointConventionBuilder Put(RequestDelegate requestDelegate);
  IEndpointConventionBuilder Delete<T>() where T : ICommand, new();
  IEndpointConventionBuilder Delete(RequestDelegate requestDelegate);
}