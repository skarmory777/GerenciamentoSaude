using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto
{
    [AutoMap(typeof(TipoCadastroExistente))]
    public class TipoCadastroExistenteDto : EntityDto<long>
    {
        public string Descricao { get; set; }
    }
}