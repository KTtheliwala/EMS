using System;

namespace TheTecniQ.API.Infrastructure.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DontValidateAttribute : Attribute
    {
    }
}