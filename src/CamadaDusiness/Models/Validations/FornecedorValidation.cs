﻿using CamadaBusiness.Models.Validations.Docs;
using FluentValidation;

namespace CamadaBusiness.Models.Validations;

public class FornecedorValidation : AbstractValidator<Fornecedor>
{
    public FornecedorValidation()
    {
        RuleFor(f => f.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {maxLength} caracteres");

        When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, () =>
        {
            RuleFor(f => f.Documento.Length).Equal(CpfValidation.TamanhoCpf)
                .WithMessage("O campo Documento precisa ter {ComparisionValue} caracteres e foi fornedido {PropertyValue}.");
            RuleFor(f => CpfValidation.Validar(f.Documento)).Equal(true).
                WithMessage("O documento fornecido é inválido.");
        });

        When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, () =>
        {
            RuleFor(f => f.Documento.Length).Equal(CnpjValidation.TamanhoCnpj)
                .WithMessage("O campo Documento precisa ter {ComparisionValue} caracteres e foi fornedido {PropertyValue}.");
            RuleFor(f => CnpjValidation.Validar(f.Documento)).Equal(true).
                WithMessage("O documento fornecido é inválido.");
        });
    }
}
