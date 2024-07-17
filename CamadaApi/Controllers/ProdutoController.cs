using AutoMapper;
using CamadaApi.ViewModels;
using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using Microsoft.AspNetCore.Mvc;

namespace CamadaApi.Controllers;

[Route("api/produtos")]
public class ProdutoController : MainController
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutoService _produtoService;
    private readonly IMapper _mapper;
    public ProdutoController(INotificador notificador, 
                             IProdutoRepository produtoRepository,
                             IProdutoService produtoService,
                             IMapper mapper) : base(notificador)
    {
        _produtoRepository = produtoRepository;
        _produtoService = produtoService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
    }

    [HttpGet("{id:guid}")]
    private async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
    {
        var produtoViewModel = await ObterProduto(id);

        if(produtoViewModel == null) return NotFound();

        return produtoViewModel;
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
    {
        if (!ModelState.IsValid) return CustonResponse(ModelState);

        var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
        if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
        {
            return CustonResponse(produtoViewModel);
        }

        produtoViewModel.Imagem = imagemNome;
        await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

        return CustonResponse(produtoViewModel);
    }


    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
    {
        var produto = await ObterProduto(id);

        if(produto == null) return NotFound();

        return CustonResponse(produto);
    }

    private bool UploadArquivo(string arquivo, string imgNome)
    {
        var imageDataByteArray = Convert.FromBase64String(arquivo);

        if (string.IsNullOrEmpty(arquivo))
        {
            NotificarErro("Forneça uma imagem para este produto!");
            return false;
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

        if (System.IO.File.Exists(filePath))
        {
            NotificarErro("Já existe um arquivo com este nome!");
            return false;
        }

        System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

        return true;
    }

    private async Task<ProdutoViewModel> ObterProduto(Guid id)
    {
        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
    }
     
}
