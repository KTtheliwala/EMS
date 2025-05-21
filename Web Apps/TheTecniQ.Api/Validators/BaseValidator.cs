using System.Linq;
using FluentValidation;

namespace TheTecniQ.API.Validators;

/// <summary>
/// Base class for validators
/// </summary>
/// <typeparam name="TModel">Type of model being validated</typeparam>
public abstract partial class BaseValidator<TModel> : AbstractValidator<TModel> where TModel : class
{
    #region Ctor

    protected BaseValidator()
    {
        PostInitialize();
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Developers can override this method in custom partial classes in order to add some custom initialization code to constructors
    /// </summary>
    protected virtual void PostInitialize()
    {

    }

    #endregion
}