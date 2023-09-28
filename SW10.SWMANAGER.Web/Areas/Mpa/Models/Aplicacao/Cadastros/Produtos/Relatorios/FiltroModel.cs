using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos.Relatorios
{
    public class FiltroModel
    {
        public string Titulo { get; set; }
        public string NomeHospital { get; set; }
        public string NomeUsuario { get; set; }
        public string DataHora { get; set; }
        public IList<SelectListItem> Empresas { get; set; }
        public List<RespostaDto> Respostas { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        //public string Paciente { get; set; }
        //public string Nascimento { get; set; }
        //public long? PrescricaoId { get; set; }
        //public string Prescricao { get; set; }
        //public string PrescricaoItem { get; set; }
        //public string Atendimento { get; set; }
        //public string Convenio { get; set; }
        //public string Internacao { get; set; }
        //public string UnidadeOrganizacional { get; set; }
        //public string Leito { get; set; }
        //public long Empresa { get; set; }
        //public string Medico { get; set; }
        //public string CRM { get; set; }
    }
}