using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Filas
{
    public class TermialSenhaViewModel
    {
        public string UrlPath { get; set; }
        public List<FilaTerminalIndex> Filas { get; set; }
    }

    public class EscolherTermialSenhaViewModel
    {
        public string UrlPath { get; set; }
        public List<TipoLocalChamadaIndex> Locais { get; set; }
    }
}