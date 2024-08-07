﻿using AutoMapper;
using CamadaApi.Extensions;
using CamadaApi.ViewModels;
using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CamadaApi.Controllers;

[Authorize]
[Route("api/produtos")]
public class ProdutoController : MainController
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutoService _produtoService;
    private readonly IMapper _mapper;
    public ProdutoController(INotificador notificador,
                             IProdutoRepository produtoRepository,
                             IProdutoService produtoService,
                             IMapper mapper,
                             IUser user) : base(notificador, user)
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

        if (produtoViewModel == null) return NotFound();

        return produtoViewModel;
    }
    [ClaimsAuthorize("Produto", "Adicionar")]
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

    [ClaimsAuthorize("Produto", "Atualizar")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
    {
        if (id != produtoViewModel.Id)
        {
            NotificarErro("Os ids informados não são iguais!");
            return CustonResponse();
        }

        var produtoAtualizacao = await ObterProduto(id);

        if (string.IsNullOrEmpty(produtoViewModel.Imagem))
            produtoViewModel.Imagem = produtoAtualizacao.Imagem;

        if (!ModelState.IsValid) return CustonResponse(ModelState);

        if (produtoViewModel.ImagemUpload != null)
        {
            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
            if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            {
                return CustonResponse(ModelState);
            }

            produtoAtualizacao.Imagem = imagemNome;
        }

        produtoAtualizacao.FornecedorId = produtoViewModel.FornecedorId;
        produtoAtualizacao.Nome = produtoViewModel.Nome;
        produtoAtualizacao.Descricao = produtoViewModel.Descricao;
        produtoAtualizacao.Valor = produtoViewModel.Valor;
        produtoAtualizacao.Ativo = produtoViewModel.Ativo;

        await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

        return CustonResponse(produtoViewModel);
    }

    [ClaimsAuthorize("Produto", "Excluir")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
    {
        var produto = await ObterProduto(id);

        if (produto == null) return NotFound();

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
    // upload com IFormFile e Json
    #region UploadAlternativo
    [HttpPost("Adicionar")]
    public async Task<ActionResult<ProdutoViewModel>> AdicionarAlternativo(
            // Binder personalizado para envio de IFormFile e ViewModel dentro de um FormData compatível com .NET Core 3.1 ou superior (system.text.json)
            [ModelBinder(BinderType = typeof(ProdutoModelBinder))]
            ProdutoImagemViewModel produtoViewModel)
    {
        if (!ModelState.IsValid) return CustonResponse(ModelState);

        var imgPrefixo = Guid.NewGuid() + "_";
        if (!await UploadArquivoAlternativo(produtoViewModel.ImagemUpload, imgPrefixo))
        {
            return CustonResponse(ModelState);
        }

        produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
        await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

        return CustonResponse(produtoViewModel);
    }

    private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string imgPrefixo)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            NotificarErro("Forneça uma imagem para este produto!");
            return false;
        }

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPrefixo + arquivo.FileName);

        if (System.IO.File.Exists(path))
        {
            NotificarErro("Já existe um arquivo com este nome!");
            return false;
        }

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        return true;
    }

    #endregion

    private async Task<ProdutoViewModel> ObterProduto(Guid id)
    {
        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
    }

}
