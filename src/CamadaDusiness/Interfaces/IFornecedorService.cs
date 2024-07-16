
using CamadaBusiness.Models;

namespace CamadaBusiness.Interfaces;

public interface IFornecedorService : IDisposable
{
    Task<bool> Adicionar(Fornecedor fornecedor);
    Task<bool> Atualizar(Fornecedor fornecedor);
    Task<bool> Remover(Guid id);
    Task AtualizarEndereco(Endereco endereco);

}