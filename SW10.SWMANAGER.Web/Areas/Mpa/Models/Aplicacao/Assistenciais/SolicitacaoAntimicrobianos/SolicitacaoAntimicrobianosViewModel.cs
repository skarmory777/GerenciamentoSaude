using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.SolicitacaoAntimicrobianos
{
    public class SolicitacaoAntimicrobianosViewModel: SolicitacaoAntimicrobianoListDto
    {
        public long AtendimentoId { get; set; }

        public string CodigoAtendimento { get; set; }
        public string NomePaciente { get; set; }

        public string UnidadeOrganizacional { get; set; }

        public string Leito { get; set; }

        public List<TipoSolicitacaoAntimicrobianosIndicacaoDto> TipoIndicacoes { get; set; }

        public List<TipoSolicitacaoAntimicrobianosResultadoDto> TipoResultados { get; set; }

        public List<TipoSolicitacaoAntimicrobianosCulturaDto> TipoCulturas { get; set; }

        public long? PrescricaoId { get; set; }


        public static bool CheckIndicacao(SolicitacaoAntimicrobianoDto solicitacaoAntimicrobiano, TipoSolicitacaoAntimicrobianosIndicacaoDto indicacao)
        {
            if (solicitacaoAntimicrobiano == null || solicitacaoAntimicrobiano.SolicitacaoAntimicrobianosIndicacoes.IsNullOrEmpty())
            {
                return false;
            }

            return solicitacaoAntimicrobiano.SolicitacaoAntimicrobianosIndicacoes.Any(x => x.TipoSolicitacaoAntimicrobianosIndicacaoId == indicacao.Id);
        }

        public static bool CheckResultado(SolicitacaoAntimicrobianosCulturaDto solicitacaoAntimicrobianosCultura, TipoSolicitacaoAntimicrobianosResultadoDto resultado)
        {
            if (solicitacaoAntimicrobianosCultura == null || solicitacaoAntimicrobianosCultura.SolicitacaoAntimicrobianosResultados.IsNullOrEmpty())
            {
                return false;
            }

            return solicitacaoAntimicrobianosCultura.SolicitacaoAntimicrobianosResultados.Any(x => x.TipoSolicitacaoAntimicrobianosResultadoId == resultado.Id);
        }

        public static bool CheckTipoInfeccao(SolicitacaoAntimicrobianoDto solicitacaoAntimicrobiano, string tipo)
        {
            return solicitacaoAntimicrobiano.TipoInfeccao == tipo;
        }

        public static bool CheckCultura(SolicitacaoAntimicrobianoDto solicitacaoAntimicrobiano, string tipo)
        {
            return solicitacaoAntimicrobiano.TipoCultura == tipo;
        }
    }
}