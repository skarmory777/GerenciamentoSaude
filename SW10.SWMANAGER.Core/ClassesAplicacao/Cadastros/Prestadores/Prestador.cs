using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CEP;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Conselhos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposParticipacoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposPrestadores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposVinculosEmpregaticios;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Prestadores
{
    [Table("SisPrestador")]
    public class Prestador : PessoaFisica
    {
        public byte[] CapturaFoto { get; set; }

        [ForeignKey("TipoVinculoEmpregaticio"), Column("SisTipoVinculoEmpregaticioId")]
        public long? TipoVinculoEmpregaticioId { get; set; }

        public TipoVinculoEmpregaticio TipoVinculoEmpregaticio { get; set; }

        [ForeignKey("TipoParticipacao"), Column("SisParticipacaoId")]
        public long? TipoParticipacaoId { get; set; }

        public TipoParticipacao TipoParticipacao { get; set; }

        public bool IsCorpoClinico { get; set; }

        public string NomeGuerra { get; set; }

        public DateTime DataNascimento { get; set; }

        public int Identidade { get; set; }

        public int Cnpj { get; set; }

        public int CartaoNacionalSus { get; set; }

        //Comercial
        [ForeignKey("CepComercial"), Column("SisCepComercialId")]
        public long? CepComercialId { get; set; }

        public Cep CepComercial { get; set; }

        [ForeignKey("TipoLogradouroComercial"), Column("SisLogradouroComercialId")]
        public long? TipoLogradouroComercialId { get; set; }

        public TipoLogradouro TipoLogradouroComercial { get; set; }

        [StringLength(255)]
        public string EnderecoComercial { get; set; }

        public string NumeroComercial { get; set; }

        public string ComplementoComercial { get; set; }

        public string BairroComercial { get; set; }

        public string CidadeComercial { get; set; }

        public string EstadoUfComercial { get; set; }

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
