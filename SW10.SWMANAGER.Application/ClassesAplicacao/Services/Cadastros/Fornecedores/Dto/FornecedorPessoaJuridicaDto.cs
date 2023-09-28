using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto
{
    [AutoMap(typeof(FornecedorPessoaJuridica))]
    public class FornecedorPessoaJuridicaDto : PessoaJuridicaDto
    {
        public long FornecedorId { get; set; }
    }
}