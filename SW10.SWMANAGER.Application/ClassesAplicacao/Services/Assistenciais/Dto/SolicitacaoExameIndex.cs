using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class SolicitacaoExameIndex : CamposPadraoCRUDDto
    {
        public DateTime? DataSolicitacao { get; set; }

        public string UnidadeOrganizacional { get; set; }

        public string MedicoSolicitante { get; set; }

        public string Status { get; set; }
    }
}
