using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(ContaAdministrativaEmpresa))]
    public class ContaAdministrativaEmpresaDto : CamposPadraoCRUDDto
    {
        public long? ContaAdministrativaId { get; set; }
        public ContaAdministrativaDto ContaAdministrativa { get; set; }
        public long EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }
    }
}
