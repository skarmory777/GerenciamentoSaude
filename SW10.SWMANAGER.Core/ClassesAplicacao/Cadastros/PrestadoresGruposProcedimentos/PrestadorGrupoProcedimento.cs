using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposProcedimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Prestadores;
using System.ComponentModel.DataAnnotations.Schema;


namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.PrestadoresGruposProcedimentos
{
    [Table("PrestadorGrupoProcedimento")]
    public class PrestadorGrupoProcedimento : CamposPadraoCRUD
    {
        public long? PrestadorId { get; set; }
        [ForeignKey("PrestadorId")]
        public Prestador Prestador { get; set; }

        public long? GrupoProcedimentoId { get; set; }
        [ForeignKey("GrupoProcedimentoId")]
        public GrupoProcedimento GrupoProcedimento { get; set; }
    }
}
