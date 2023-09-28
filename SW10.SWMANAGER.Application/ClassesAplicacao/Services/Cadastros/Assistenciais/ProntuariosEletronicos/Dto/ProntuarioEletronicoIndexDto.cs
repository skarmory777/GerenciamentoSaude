using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto
{
    public class ProntuarioEletronicoIndexDto : CamposPadraoCRUDDto, ITablePermissions
    {
        public string CodigoAtendimento { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public string UnidadeOrganizacional { get; set; }
        public string Empresa { get; set; }
        public string Formulario { get; set; }
        public long? FormRespostaId { get; set; }

        public string Usuario { get; set; }
        
        
        public bool EnableEdit { get; set; }
        public bool EnableDelete { get; set; }
    }

    public interface ITablePermissions
    {
        bool EnableEdit { get; set; }
        
        bool EnableDelete { get; set; }
    }
}
