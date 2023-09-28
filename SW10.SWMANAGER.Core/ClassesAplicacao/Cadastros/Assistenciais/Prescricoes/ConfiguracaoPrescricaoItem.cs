namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes
{
    using Abp.Domain.Entities.Auditing;
    using SW10.SWMANAGER.ClassesAplicacao;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AssConfiguracaoPrescricaoItem")]
    public class ConfiguracaoPrescricaoItem : FullAuditedEntity<long>
    {
        [ForeignKey("ConfiguracaoPrescricaoItemCampo"), Column("ConfiguracaoPrescricaoItemCampoId")]
        public long ConfiguracaoPrescricaoItemCampoId { get; set; }

        public ConfiguracaoPrescricaoItemCampo ConfiguracaoPrescricaoItemCampo { get; set; }

        [ForeignKey("PrescricaoItem"), Column("PrescricaoItemId")]

        public long? PrescricaoItemId { get; set; }

        public long? DivisaoId { get; set; }

        public PrescricaoItem PrescricaoItem { get; set; }

        public Divisao Divisao { get; set; }

        public bool IsBlock { get; set; }
        public bool IsRequired { get; set; }

        public string DefaultValue { get; set; }

        public string Options { get; set; }

    }

    [Table("AssConfiguracaoPrescricaoItemCampo")]
    public class ConfiguracaoPrescricaoItemCampo : CamposPadraoCRUD
    {
        [Description("Quantidade Por Hor�rio")]
        public static long QtdPorHorario = 1;
        [Description("Unidade")]
        public static long Unidade = 2;
        [Description("Via de Aplica��o")]
        public static long ViaDeAplicacao = 3;
        [Description("Forma de Aplica��o")]
        public static long FormaDeAplicacao = 4;
        [Description("Diluente")]
        public static long Diluente = 5;
        [Description("Volume")]
        public static long Volume = 6;
        [Description("M�dico")]
        public static long Medico = 7;
        [Description("Frequ�ncia")]
        public static long Frequencia = 8;
        [Description("Hora In�cial")]
        public static long HoraInicial = 9;
        [Description("Dia In�cial")]
        public static long DiaInicial = 10;
        [Description("Dias Prov�veis De Uso")]
        public static long DiasProvaveisDeUso = 11;
        [Description("Observa��o")]
        public static long Observacao = 12;
    }
}