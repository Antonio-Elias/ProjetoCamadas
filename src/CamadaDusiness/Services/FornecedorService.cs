﻿using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using CamadaBusiness.Models.Validations;

namespace CamadaBusiness.Services;

public class FornecedorService : BaseService, IFornecedorService
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IEnderecoRepository _enderecoRepository;

    public FornecedorService(IFornecedorRepository fornecedorRepository,
                             IEnderecoRepository enderecoRepository,
                             INotificador notificador) : base(notificador)
    {
        _fornecedorRepository = fornecedorRepository;
        _enderecoRepository = enderecoRepository;
    }

    public async Task<bool> Adicionar(Fornecedor fornecedor)
    {
        //Validar o estado da entidade
        if (!ExecutarValidation(new FornecedorValidation(), fornecedor)
            || !ExecutarValidation(new EnderecoValidation(), fornecedor.Endereco)) return false;

        if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
        {
            Notificar("Ja existe um fornecedor com este documento informado.");

            return false;
        }

        await _fornecedorRepository.Adicionar(fornecedor);
        return true;
    }

    public async Task<bool> Atualizar(Fornecedor fornecedor)
    {
        if (!ExecutarValidation(new FornecedorValidation(), fornecedor)) return false;

        if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
        {
            Notificar("Já existej um fornecedor com o documento informado!");

            return false;
        }

        await _fornecedorRepository.Atualizar(fornecedor);
        return true;
    }

    public async Task AtualizarEndereco(Endereco endereco)
    {
        if (!ExecutarValidation(new EnderecoValidation(), endereco)) return;

        await _enderecoRepository.Atualizar(endereco);
    }


    public async Task<bool> Remover(Guid id)
    {
        if (_fornecedorRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
        {
            Notificar("O fornecedor possui produtos cadastrados!");
            return false;
        }

        var endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);

        if (endereco != null)
        {
            await _enderecoRepository.Remover(endereco.Id);
        }

        await _fornecedorRepository.Remover(id);
        return true;
    }

    public void Dispose()
    {
        _enderecoRepository?.Dispose();
        _fornecedorRepository?.Dispose();
    }
}
