using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.Leitos
{
    public class LeitosViewModel
    {
        //   public List<string> LeitoStatus { get; set; }

        public List<LeitoStatusDto> LeitoStatus { get; set; }

        public string Filtro { get; set; }
    }
}