using AutoMapper;
using CamadaApi.Extensions;
using CamadaApi.ViewModels;
using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CamadaApi.Controllers;

[Authorize]
[Route("api/fornecedores")]
public class FonecedoresController : MainController
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IMapper _mapper;
    private readonly IFornecedorService _fornecedorService;
    private readonly IEnderecoRepository _enderecoRepository;

    public FonecedoresController(IFornecedorRepository fornecedorRepository,
                                 IMapper mapper,
                                 IFornecedorService fornecedorService,
                                 INotificador notificador,
                                 IEnderecoRepository enderecoRepository,
                                 IUser user) : base(notificador, user)
    {
        _fornecedorRepository = fornecedorRepository;
        _mapper = mapper;
        _fornecedorService = fornecedorService;
        _enderecoRepository = enderecoRepository;
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

    [HttpGet("obter-endereco/{id:guid}")]
    public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id)
    {
        return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("atualizar-endereco/{id:guid}")]
    public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
    {
        if (id != enderecoViewModel.Id)
        {
            NotificarErro("O id informado não é o mesmo que foi passaedo na query!");
            return CustonResponse(enderecoViewModel);
        }

        if (!ModelState.IsValid) return CustonResponse(ModelState);

        await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));

        return CustonResponse(enderecoViewModel);
    }

    [ClaimsAuthorize("Fornecedor", "Adicionar")]
    [HttpPost]
    public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return CustonResponse(ModelState);

        await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustonResponse(fornecedorViewModel);

    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
    {
        if (id != fornecedorViewModel.Id)
        {
            NotificarErro("O id informado não é o mesmo que foi passaedo na query!");
            return CustonResponse(fornecedorViewModel);
        }

        if (!ModelState.IsValid) return BadRequest();

        await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustonResponse(fornecedorViewModel);

    }

    [ClaimsAuthorize("Fornecedor", "Excluir")]
    [HttpDelete]
    public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
    {
        var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterPorId(id));

        if (fornecedor == null) return NotFound();

        await _fornecedorService.Remover(id);

        return CustonResponse();

    }
}
