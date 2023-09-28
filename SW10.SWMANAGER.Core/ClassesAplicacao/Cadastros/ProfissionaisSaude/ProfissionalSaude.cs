using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Conselhos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposParticipacoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposPrestadores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposVinculosEmpregaticios;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProfissionaisSaude
{
    [Table("SisProfissionalSaude")]
    public class ProfissionalSaude : CamposPadraoCRUD
    {
        [ForeignKey("SisPessoa"), Column("SisPessoaId")]
        public long? SisPessoaId { get; set; }
        public SisPessoa SisPessoa { get; set; }

        [ForeignKey("TipoVinculoEmpregaticio"), Column("SisTipoVinculoEmpregaticioId")]
        public long? TipoVinculoEmpregaticioId { get; set; }

        public TipoVinculoEmpregaticio TipoVinculoEmpregaticio { get; set; }

        [ForeignKey("TipoParticipacao"), Column("SisParticipacaoId")]
        public long? TipoParticipacaoId { get; set; }

        public TipoParticipacao TipoParticipacao { get; set; }

        //Pessoa public byte[] Foto { get; set; }

        public bool IsCorpoClinico { get; set; }

        //Pessoa public string Apelido { get; set; }
        [Index("Sis_Idx_DataNascimento")]
        public DateTime DataNascimento { get; set; }

        //Pessoa public int RG { get; set; }

        //Pessoa public int Cnpj { get; set; }

        public string CNS { get; set; }

        //Pessoa//Comercial
        //Pessoa[ForeignKey("CepComercial"), Column("SisCepComercialId")]
        //Pessoapublic long? CepComercialId { get; set; }
        //Pessoapublic Cep CepComercial { get; set; }

        //Pessoa[ForeignKey("TipoLogradouroComercial"), Column("SisLogradouroComercialId")]
        //Pessoapublic long? TipoLogradouroComercialId { get; set; }
        //Pessoapublic TipoLogradouro TipoLogradouroComercial { get; set; }

        //Pessoa[StringLength(255)]
        //Pessoapublic string EnderecoComercial { get; set; }

        //Pessoapublic string NumeroComercial { get; set; }

        //Pessoapublic string ComplementoComercial { get; set; }

        //Pessoapublic string BairroComercial { get; set; }

        //Pessoapublic string CidadeComercial { get; set; }

        //Pessoapublic string EstadoUfComercial { get; set; }

        [ForeignKey("TipoPrestador"), Column("SisTipoPrestadorId")]
        public long? TipoPrestadorId { get; set; }

        public TipoPrestador TipoPrestador { get; set; }

        [ForeignKey("Conselho"), Column("SisConselhoId")]
        public long? ConselhoId { get; set; }

        public Conselho Conselho { get; set; }

        public int NumeroConselho { get; set; }

        public string Faculdade { get; set; }

        public bool IsAtivo { get; set; }

        [ForeignKey("User"), Column("SisUserId")]
        public long? UserId { get; set; }

        public User User { get; set; }

    }
}
