using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    public class RegistroExameIndex : CamposPadraoCRUDDto
    {
        public string Exame { get; set; }
        public string PacienteDescricao { get; set; }
        public string ConvenioDescricao { get; set; }
        public int Status { get; set; }
        public string LoteContraste { get; set; }
        public bool IsContraste { get; set; }
        public int? QtdContraste { get; set; }
        public string InternacaoAmbulatorio { get; set; }
        public long? AtendimentoId { get; set; }
        public DateTime? DataAtendimento { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public string Medico { get; set; }
        public string Leito { get; set; }
        public string TipoLeito { get; set; }
        public string UnidadeOrganizacional { get; set; }
        public string TipoAtendimento { get; set; }
        public string AccessNumber { get; set; }
        public DateTime Data { get; set; }
    }
}
