using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Prestadores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Prestadores
{
    [AutoMap(typeof(Prestador))]
    public class PrestadorDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public byte[] CapturaFoto { get; set; }

        public long? TipoVinculoEmpregaticioId { get; set; }
        public virtual TipoVinculoEmpregaticioDto TipoVinculoEmpregaticio { get; set; }

        public long? TipoParticipacaoId { get; set; }
        public virtual TipoParticipacaoDto TipoParticipacao { get; set; }

        public bool IsCorpoClinico { get; set; }

        public string NomeGuerra { get; set; }

        public DateTime DataNascimento { get; set; }

        public int Identidade { get; set; }

        public int Cnpj { get; set; }

        public int CartaoNacionalSus { get; set; }

        //Comercial
        public long? CepComercialId { get; set; }
        public virtual CepDto CepComercial { get; set; }

        public long? TipoLogradouroComercialId { get; set; }
        public virtual TipoLogradouroDto TipoLogradouroComercial { get; set; }

        public string EnderecoComercial { get; set; }

        public string NumeroComercial { get; set; }

        public string ComplementoComercial { get; set; }

        public string BairroComercial { get; set; }

        public string CidadeComercial { get; set; }

        public string EstadoUfComercial { get; set; }

        public long? TipoPrestadorId { get; set; }
        public virtual TipoPrestadorDto TipoPrestador { get; set; }

        public long? ConselhoId { get; set; }
        public virtual ConselhoDto Conselho { get; set; }

        public int NumeroConselho { get; set; }

        public string Faculdade { get; set; }

        public bool IsAtivo { get; set; }

        public long? UserId { get; set; }
        public virtual UserEditDto User { get; set; }
    }
}
