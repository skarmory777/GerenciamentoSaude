using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Internacoes.Home
{
    public class AssistenciaisViewModel
    {
        public string Filtro { get; set; }

        public ICollection<AtendimentoDto> Atendimentos { get; set; }

    }
}