using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class RetornoCopiarPrescricaoMedica
    {
        public ICollection<PrescricaoItemRespostaDto> PrescricaoItens { get; set; }

        public ICollection<string> Mensagens { get; set; }

        public RetornoCopiarPrescricaoMedica()
        {
            PrescricaoItens = new List<PrescricaoItemRespostaDto>();
            Mensagens = new List<string>();
        }
    }
}
