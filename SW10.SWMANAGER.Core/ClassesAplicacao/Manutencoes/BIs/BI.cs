using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Modulos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Manutencoes.BIs
{
    [Table("SisBI")]
    public class BI : CamposPadraoCRUD, IDescricao
    {
        [ForeignKey("Modulo"), Column("SisModuloId")]
        public long? ModuloId { get; set; }
        [ForeignKey("Operacao"), Column("SisOperacaoId")]
        public long? OperacaoId { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// Este campo define se o item será exibido no menu do dashboard
        /// </summary>
        public bool IsPublico { get; set; }
        /// <summary>
        /// Se true, exibe o ícone na barra de título do portlet
        /// </summary>
        public bool IsPrincipal { get; set; }

        public Modulo Modulo { get; set; }

        public Operacao Operacao { get; set; }
    }
}
