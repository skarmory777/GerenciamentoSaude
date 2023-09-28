using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Dto.Configuracoes.Empresas.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto
{
    [AutoMap(typeof(ConvenioEmpresa))]
    public class ConvenioEmpresaDto : CamposPadraoCRUDDto
    {
        public long CodigoCredenciado { get; set; }

        public long ConvenioId { get; set; }
        public virtual ConvenioDto Convenio { get; set; }

        public long EmpresaId { get; set; }
        public virtual EmpresaDto Empresa { get; set; }

    }
}
