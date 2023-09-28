using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Fornecedores
{
    [AutoMapFrom(typeof(SisFornecedorDto))]
    public class CriarOuEditarFornecedorModalViewModel : SisFornecedorDto// CriarOuEditarFornecedor
    {


        //public string NomeCompleto { get; set; }

        //public long? TipoPessoaId { get; set; }

        //public string FisicaJuridica { get; set; }


        //#region colunas Pessoa Física

        //public string Rg { get; set; }

        //public string Emissor { get; set; }

        //public DateTime? EmissaoRg { get; set; }

        //public DateTime? Nascimento { get; set; }

        //public int? Sexo { get; set; }

        //public int? CorPele { get; set; }

        //public long? ProfissaoId { get; set; }

        //public int? Escolaridade { get; set; }

        //public string Cpf { get; set; }

        //public long? NaturalidadeId { get; set; }

        //public long? NacionalidadeId { get; set; }


        //public int? EstadoCivil { get; set; }

        //public string NomeMae { get; set; }

        //public string NomePai { get; set; }

        //public long? ReligiaoId { get; set; }


        //#endregion

        //#region Colunas Pessoa Jurídica

        //public string RazaoSocial { get; set; }
        //public string NomeFantasia { get; set; }
        //public string Cnpj { get; set; }
        //public string InscricaoEstadual { get; set; }
        //public string InscricaoMunicipal { get; set; }

        //#endregion

        //#region Telefones

        //public string Telefone1 { get; set; }

        //public int? TipoTelefone1 { get; set; }

        //public int? DddTelefone1 { get; set; }

        //public string Telefone2 { get; set; }

        //public int? TipoTelefone2 { get; set; }

        //public int? DddTelefone2 { get; set; }

        //public string Telefone3 { get; set; }

        //public int? TipoTelefone3 { get; set; }

        //public int? DddTelefone3 { get; set; }

        //public string Telefone4 { get; set; }

        //public int? TipoTelefone4 { get; set; }

        //public int? DddTelefone4 { get; set; }

        //#endregion

        //#region Emails

        //public string Email { get; set; }

        //public string Email2 { get; set; }

        //public string Email3 { get; set; }

        //public string Email4 { get; set; }

        //#endregion








        //      public UserEditDto UpdateUser { get; set; }

        //public SelectList TiposPessoa { get; set; }

        //public SelectList TiposTelefone { get; set; }

        //public SelectList Estados { get; set; }

        //public SelectList Cidades { get; set; }

        //public SelectList Paises { get; set; }

        //public SelectList Escolaridades { get; set; }

        //public SelectList EstadosCivis { get; set; }

        //public SelectList Sexos { get; set; }

        //public SelectList Religioes { get; set; }

        //public SelectList CoresPele { get; set; }

        //      public SelectList TiposLogradouro { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarFornecedorModalViewModel(SisFornecedorDto output)
        {
            output.MapTo(this);
        }
    }
}