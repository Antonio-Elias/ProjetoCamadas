

using CamadaBusiness.Interfaces;
using CamadaBusiness.Models;
using CamadaData.Context;
using Microsoft.EntityFrameworkCore;

namespace CamadaData.Repository;

public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(MeuDbContext context) : base(context){}

    public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
    {
        return await Db.Enderecos.AsNoTracking()
            .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
    }
}
