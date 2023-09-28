using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    [AutoMap(typeof(ControleProducao))]
    public class CriarOuEditarControleProducao : CamposPadraoCRUDDto
    {
        [StringLength(500)]
        public string Descricao { get; set; }

        [StringLength(500)]
        public string Observacao { get; set; }

        public int Pontuacao { get; set; }

        public int Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataInicial { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataFinal { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataAprovacao { get; set; }

        public long DesenvolvedorId { get; set; }

        public long UsuarioAprovacaoId { get; set; }

        public long TabelaSistemaId { get; set; }

        [ForeignKey("DesenvolvedorId")]
        public virtual User Desenvolvedor { get; set; }

        [ForeignKey("UsuarioAprovacaoId")]
        public virtual User UsuarioAprovacao { get; set; }

        [ForeignKey("TabelaSistemaId")]
        public virtual ConsultorTabela TabelaSistema { get; set; }
    }
}