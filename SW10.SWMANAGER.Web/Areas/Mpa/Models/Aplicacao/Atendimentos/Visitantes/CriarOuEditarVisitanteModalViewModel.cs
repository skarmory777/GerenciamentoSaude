using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Visitantes
{
    [AutoMap(typeof(VisitanteDto))]
    public class CriarOuEditarVisitanteModalViewModel : VisitanteDto
    {
        public string Filtro { get; set; }



        //public bool IsEmergencia { get; set; }

        //public bool IsInternado { get; set; }

        //public bool IsSetor { get; set; }

        //public bool IsFornecedor { get; set; }

        //public long FornecedorId { get; set; }

        //public long AtendimentoId { get; set; }
        //public long? PacienteId { get; set; }
        //public bool UnidadeOrganizacionalId { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarVisitanteModalViewModel(VisitanteDto output)
        {
            output.MapTo(this);
        }
    }
}

