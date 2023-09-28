using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(PessoaFisica))]
    public abstract class PessoaFisicaDto : PessoaDto
    {
        public string NomeCompleto { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Nascimento { get; set; }

        // public int? Sexo { get; set; }

        // public int? CorPele { get; set; }

        public virtual ProfissaoDto Profissao { get; set; }
        public long? ProfissaoId { get; set; }

        //public int? Escolaridade { get; set; }

        public string Rg { get; set; }

        public string Emissor { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Emissao { get; set; }

        public string Cpf { get; set; }

        public virtual NaturalidadeDto Naturalidade { get; set; }
        public long? NaturalidadeId { get; set; }

        public long? NacionalidadeId { get; set; }
        public virtual NacionalidadeDto Nacionalidade { get; set; }


        // public int? EstadoCivil { get; set; }

        public string NomeMae { get; set; }

        public string NomePai { get; set; }

        //  public int? Religiao { get; set; }

        public byte[] Foto { get; set; }

        public string FotoMimeType { get; set; }




    }
}
