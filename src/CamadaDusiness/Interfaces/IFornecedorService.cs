
using CamadaBusiness.Models;

namespace CamadaBusiness.Interfaces;

public interface IFornecedorService
{
    Task Adicionar(Fornecedor fornecedor);
    Task Atualizar(Fornecedor fornecedor);
    Task Remover(Guid id);
    Task AtualizarEndereco(Endereco endereco);

}