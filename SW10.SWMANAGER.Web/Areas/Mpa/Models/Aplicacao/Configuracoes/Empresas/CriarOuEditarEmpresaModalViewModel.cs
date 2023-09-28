using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.Empresas
{
    [AutoMap(typeof(EmpresaDto))]
    public class CriarOuEditarEmpresaModalViewModel : EmpresaDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }

        public SelectList EstoquesMaster { get; set; }

        public SelectList Convenios { get; set; }

        public SelectList Planos { get; set; }

        public SelectList TiposTelefone { get; set; }

        public SelectList TiposLogradouro { get; set; }

        public SelectList Cidades { get; set; }

        public SelectList Estados { get; set; }

        public SelectList Paises { get; set; }


        public CriarOuEditarEmpresaModalViewModel(EmpresaDto output)
        {
            output.MapTo(this);
        }
    }
}