using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using CamadaBusiness.Models.Validations;

namespace CamadaBusiness.Services;

public class ProdutoService : BaseService, IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    public ProdutoService(IProdutoRepository produtoRepository,
                          INotificador notificador) : base(notificador)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task Adicionar(Produto produto)
    {
        if (!ExecutarValidation(new ProdutoValidation(), produto)) return;

        await _produtoRepository.Adicionar(produto);
    }

    public async Task Atualizar(Produto produto)
    {
        if (!ExecutarValidation(new ProdutoValidation(), produto)) return;

        await _produtoRepository.Atualizar(produto);
    }


    public async Task Remover(Guid id)
    {
        await _produtoRepository.Remover(id);
    }

    public void Dispose()
    {
        _produtoRepository?.Dispose();
    }
}
