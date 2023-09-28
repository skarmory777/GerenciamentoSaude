using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos
{
    [Table("AteLeito")]
    public class Leito : CamposPadraoCRUD
    {
        public string DescricaoResumida { get; set; }

        public string LeitoAih { get; set; }

        public string Ramal { get; set; }

        public int? Sexo { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }

        [ForeignKey("TipoAcomodacao"), Column("SisTipoAcomodacaoId")]
        public long? TipoAcomodacaoId { get; set; }

        [ForeignKey("TabelaDominio"), Column("SisTabelaItemTissId")]
        public long? TabelaItemTissId { get; set; }

        public long? TabelaItemSusId { get; set; }

        [ForeignKey("LeitoStatus"), Column("AteLeitoStatusId")]
        public long? LeitoStatusId { get; set; }

        [Index("Ate_Idx_DataAtualizacao")]
        [DataType(DataType.DateTime)]
        public DateTime DataAtualizacao { get; set; }

        public bool Extra { get; set; }

        public bool HospitalDia { get; set; }

        public bool Ativo { get; set; }

        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        public TipoAcomodacao TipoAcomodacao { get; set; }

        public TabelaDominio TabelaDominio { get; set; }

        public LeitoStatus LeitoStatus { get; set; }
    }
}

