using System;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Arcaim.CQRS.WebApi.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Arcaim.CQRS.WebApi;

public class WebAction : IWebAction
{
  private readonly IWebApi _webApi;
  private readonly ICommandDispatcher _commandDispatcher;
  private readonly IQueryDispatcher _queryDispatcher;

  public WebAction(IWebApi webApi)
  {
    _webApi = webApi;
    _commandDispatcher = _webApi.EndpointRouteBuilder.ServiceProvider.GetService<ICommandDispatcher>();
    _queryDispatcher = _webApi.EndpointRouteBuilder.ServiceProvider.GetService<IQueryDispatcher>();
  }

  public IWebAction Action(Func<ICommandDispatcher, IQueryDispatcher, IEndpointConventionBuilder> requestDelegate)
  {
    requestDelegate.Invoke(_commandDispatcher, _queryDispatcher);

    return this;
  }

  public IWebAction Action(string pattern, Func<ICommandDispatcher, IQueryDispatcher, IEndpointConventionBuilder> requestDelegate)
  {
    this.SetPatternAction(pattern);
    this.Action(requestDelegate);
    this.ClearPatternAction();

    return this;
  }

  public IWebAction Command(Func<IEndpointConventionBuilder> requestDelegate)
  {
    requestDelegate.Invoke();

    return this;
  }

  public IWebAction Command(string pattern, Func<IEndpointConventionBuilder> requestDelegate)
  {
    this.SetPatternAction(pattern);
    this.Command(requestDelegate);
    this.ClearPatternAction();

    return this;
  }

  public IWebAction Command(Func<ICommandDispatcher, IEndpointConventionBuilder> requestDelegate)
  {
    requestDelegate.Invoke(_commandDispatcher);

    return this;
  }

  public IWebAction Command(string pattern, Func<ICommandDispatcher, IEndpointConventionBuilder> requestDelegate)
  {
    this.SetPatternAction(pattern);
    this.Command(requestDelegate);
    this.ClearPatternAction();

    return this;
  }

  public IWebAction Query(Func<IEndpointConventionBuilder> requestDelegate)
  {
    requestDelegate.Invoke();

    return this;
  }

  public IWebAction Query(string pattern, Func<IEndpointConventionBuilder> requestDelegate)
  {
    this.SetPatternAction(pattern);
    this.Query(requestDelegate);
    this.ClearPatternAction();

    return this;
  }

  public IWebAction Query(Func<IQueryDispatcher, IEndpointConventionBuilder> requestDelegate)
  {
    requestDelegate.Invoke(_queryDispatcher);

    return this;
  }

  public IWebAction Query(string pattern, Func<IQueryDispatcher, IEndpointConventionBuilder> requestDelegate)
  {
    this.SetPatternAction(pattern);
    this.Query(requestDelegate);
    this.ClearPatternAction();

    return this;
  }

  private void SetPatternAction(string pattern)
  {
    if (string.IsNullOrWhiteSpace(pattern))
    {
        throw new ArgumentException();
    }

    _webApi.PatternAction = $"{pattern}";
  }

  private void ClearPatternAction() => _webApi.PatternAction = "";
}