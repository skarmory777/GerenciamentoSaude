using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Taxas
{
    [AutoMapFrom(typeof(FaturamentoTaxaDto))]
    public class FaturamentoTaxaViewModel : FaturamentoTaxaDto
    {
        public UserEditDto UpdateUser { get; set; }

        public List<EmpresaDto> Empresas { get; set; }

        public long? EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }

        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }
        public long? PlanoId { get; set; }
        public PlanoDto Plano { get; set; }
        public long? GrupoId { get; set; }
        public FaturamentoGrupoDto Grupo { get; set; }
        public long? LocalUtilizacaoId { get; set; }
        public UnidadeOrganizacionalDto LocalUtilizacao { get; set; }
        public long? TurnoId { get; set; }
        public TurnoDto Turno { get; set; }
        public long? TipoLeitoId { get; set; }
        public TipoLeitoDto TipoLeito { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public FaturamentoTaxaViewModel(FaturamentoTaxaDto output)
        {
            output.MapTo(this);
        }
    }
}