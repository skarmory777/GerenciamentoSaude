using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ConsultorTabelas
{
    public class ConsultorTabelaCamposViewModel
    {
        public string Filtro { get; set; }
        public ICollection<ConsultorTabelaCampoDto> ConsultorTabelaCampos { get; set; }

        public long TabelaId { get; set; }
    }
}