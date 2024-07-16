using CamadaBusiness.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace CamadaApi.ViewModels;

public class FonecedorViewModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatorio")]
    [StringLength(100, ErrorMessage = "O campo {0} Precisa ter entre{2} e {1} caracteres", MinimumLength = 2)]
    public string Nome { get; set; }
    
    [Required(ErrorMessage ="O campo {0} é obrigatorio")]
    [StringLength(14, ErrorMessage ="O campo {0} Precisa ter entre{2} e {1} caracteres", MinimumLength = 11)]
    public string Documento { get; set; }
    
    public int TipoFornecedor { get; set; }
    
    public EnderecoViewModel Endereco { get; set; }
    
    public bool Ativo { get; set; }
    
    public IEnumerable<ProdutoViewModel> Produtos { get; set; }
}
