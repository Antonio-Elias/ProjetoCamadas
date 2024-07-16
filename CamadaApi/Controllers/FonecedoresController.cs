using AutoMapper;
using CamadaApi.ViewModels;
using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using Microsoft.AspNetCore.Mvc;

namespace CamadaApi.Controllers;

public class FonecedoresController : MainController
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IMapper _mapper;
    private readonly IFornecedorService _fornecedorService;

    public FonecedoresController(IFornecedorRepository fornecedorRepository,
                                 IMapper mapper,
                                 IFornecedorService fornecedorService)
    {
        _fornecedorRepository = fornecedorRepository;
        _mapper = mapper;
        _fornecedorService = fornecedorService;
    }

    [HttpGet]
    public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
    {
        var fornecedor = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

        return fornecedor;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
    {
        var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

        if (fornecedor != null) return BadRequest();

        return fornecedor;
    }

    [HttpPost]
    public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return BadRequest();

        var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

        var result = await _fornecedorService.Adicionar(fornecedor);

        if (!result) return BadRequest();

        return Ok(fornecedor);

    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
    {
        if (id != fornecedorViewModel.Id) return BadRequest();

        if (!ModelState.IsValid) return BadRequest();

        var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

        var result = await _fornecedorService.Atualizar(fornecedor);

        if (!result) return BadRequest();

        return Ok(fornecedor);

    }

    [HttpDelete]
    public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
    {
        var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterPorId(id));

        if (fornecedor == null) return NotFound();

        var result = await _fornecedorService.Remover(id);
        
        if (!result) return BadRequest();

        return Ok(fornecedor);

    }
}
