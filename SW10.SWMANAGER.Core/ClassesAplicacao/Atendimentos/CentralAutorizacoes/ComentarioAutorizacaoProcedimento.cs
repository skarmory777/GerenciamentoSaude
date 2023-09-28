using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes
{
    [Table("AteComentarioAutorizacaoProcedimento")]
    public class ComentarioAutorizacaoProcedimento : CamposPadraoCRUD
    {
        public long AutorizacaoProcedimentoId { get; set; }
        [Index("Ate_Idx_DataRegistro")]
        public DateTime DataRegistro { get; set; }
        public string Conteudo { get; set; }
        public long? UsuarioId { get; set; }

        [ForeignKey("AutorizacaoProcedimentoId")]
        public AutorizacaoProcedimento AutorizacaoProcedimento { get; set; }
    }
}
