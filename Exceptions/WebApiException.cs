namespace Arcaim.CQRS.WebApi.Exceptions
{
    public abstract class WebApiException : System.Exception
    {
        public abstract string Code { get; }
        public abstract int StatusCode { get; }
        public override string Source { get => "Arcaim.CQRS.WebApi"; }

        public WebApiException(string message) : base(message)
        {
        }
    }
}