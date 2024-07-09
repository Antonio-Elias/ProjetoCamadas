using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using CamadaBusiness.Models.Validations;

namespace CamadaBusiness.Services;

public class FornecedorService : BaseService, IFornecedorService
{
    private readonly IFornecedorRepository _forneceorRepository;
    private readonly IEnderecoRepository _enderecoRepository;

    public FornecedorService(IFornecedorRepository fornecedorRepository, IEnderecoRepository enderecoRepository)
    {
        _forneceorRepository = fornecedorRepository;
        _enderecoRepository = enderecoRepository;
    }

    public async Task Adicionar(Fornecedor fornecedor)
    {
        //Validar o estado da entidade
        if (!ExecutarValidation(new FornecedorValidation(), fornecedor)
            && !ExecutarValidation(new EnderecoValidation(), fornecedor.Endereco)) return;

        if(_forneceorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
        {
            Notificar("Ja existe um fornecedor com este documento informado.");

            return;
        }

        await _forneceorRepository.Adicionar(fornecedor);
    }

    public async Task Atualizar(Fornecedor fornecedor)
    {
        if (!ExecutarValidation(new FornecedorValidation(), fornecedor)) return;

        if(_forneceorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
        {
            Notificar("Já existej um fornecedor com o documento informado!");
        }

        await _forneceorRepository.Atualizar(fornecedor);
    }

    public async Task AtualizarEndereco(Endereco endereco)
    {
        if (!ExecutarValidation(new EnderecoValidation(), endereco)) return;

        await _enderecoRepository.Atualizar(endereco);
    }


    public async Task Remover(Guid id)
    {
        if (_forneceorRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
        {
            Notificar("O fornecedor possui produtos cadastrados!");
            return;
        }

        await _forneceorRepository.Remover(id);
    }

    public void Dispose()
    {
        _enderecoRepository.Dispose();
        _forneceorRepository.Dispose();
    }
}
