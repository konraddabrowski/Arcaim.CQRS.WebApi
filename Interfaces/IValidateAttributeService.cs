namespace Arcaim.CQRS.WebApi.Interfaces
{
    internal interface IValidateAttributeService
    {
        bool IsDecorated<T>();
    }
}