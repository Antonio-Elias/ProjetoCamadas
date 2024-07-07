using FluentValidation;

namespace CamadaBusiness.Models.Validations;

public class FornecedorValidation : AbstractValidator<Fornecedor>
{
    public FornecedorValidation()
    {
        RuleFor(f => f.Nome)
    }
}
