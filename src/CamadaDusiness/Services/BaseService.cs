using CamadaBusiness.Models;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CamadaBusiness.Services;

public abstract class BaseService
{
    protected void Notificar( ValidationResult validationResult)
    {
        foreach (var error in validationResult)
        {
            Notificar(error.ErrorMessage);
        }
    }
    protected void Notificar(string message)
    {
        // Propagar esse erro até a camada da apresentacao
    }

    protected bool ExecutarValidation<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validacao.Validate(entidade);

        if (validator.IsValid) return true;

        Notificar(validator);

        return false;
    }
}
