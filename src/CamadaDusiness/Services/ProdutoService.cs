using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using CamadaBusiness.Models.Validations;

namespace CamadaBusiness.Services;

public class ProdutoService : BaseService, IProdutoService
{
    public async Task Adicionar(Produto produto)
    {
        if (!ExecutarValidation(new ProdutoValidation(), produto)) return;
    }

    public async Task Atualizar(Produto produto)
    {
        if (!ExecutarValidation(new ProdutoValidation(), produto)) return;
    }

    public Task Remover(Guid id)
    {
        throw new NotImplementedException();
    }
}
