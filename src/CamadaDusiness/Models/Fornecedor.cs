

using CamadaBusiness.Models;

namespace CamadaBusiness.Models
    ;

public class Fornecedor : Entity
{
    public string Nome { get; set; }
    public string Documento { get; set; }
    public TipoFornecedor TipoFornecedor { get; set; }
    public Endereco Endereco { get; set; }
    public bool Ativo { get; set; }


    /*Ef Relational*/
    public IEnumerable<Produto> Produtos { get; set; }
}
