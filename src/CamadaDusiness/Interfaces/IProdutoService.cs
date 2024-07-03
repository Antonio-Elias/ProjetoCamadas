using CamadaBusiness.Models;

namespace CamadaBusiness.Interfaces;

public interface IProdutoService
{
    Task Adicionar(Produto produto);
    Task Atualizar(Produto produto);
    Task Remover(Guid id);
}
