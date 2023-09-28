using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto
{
    public class CadastroAgilizadoDto
    {
        public long? EstoqueId { get; set; }
        public long? GrupoId { get; set; }
        public long? DivisaoId { get; set; }
        public List<long> Ids { get; set; }
    }
}
