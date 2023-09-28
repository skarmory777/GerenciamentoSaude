using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AtendimentosLeitosMov
{
    [AutoMapFrom(typeof(AtendimentoLeitoMovDto))]
    public class CriarOuEditarAtendimentoLeitoMovModalViewModel : AtendimentoLeitoMovDto
    {
        public List<TipoAcomodacaoDto> TiposLeito { get; set; }

        public string Filtro { get; set; }

        public string NomePaciente { get; set; }

        public string LeitoPaciente { get; set; }

        public List<UnidadeOrganizacionalDto> UnidadesOrganizacionais { get; set; }
        public bool IsAlta { get; set; }


        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarAtendimentoLeitoMovModalViewModel(AtendimentoLeitoMovDto output)
        {
            output.MapTo(this);
        }
    }
}

