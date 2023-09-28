using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;

using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    public class InternacoesViewModel : CriarOuEditarAtendimento
    {
        public List<UnidadeOrganizacionalDto> UnidadesOrganizacionais { get; set; }

        public Empresa UserEmpresa { get; set; }

        public SelectList Empresas { get; set; }

        public string Filtro { get; set; }

        public string TipoAtendimento { get; set; }

        public long? AgendamentoId { get; set; }



    }
}