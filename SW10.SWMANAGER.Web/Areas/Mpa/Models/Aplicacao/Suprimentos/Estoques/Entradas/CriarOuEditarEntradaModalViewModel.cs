using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Entradas
{
    [AutoMapFrom(typeof(CriarOuEditarEntrada))]
    public class CriarOuEditarEntradaModalViewModel : CriarOuEditarEntrada
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Empresas { get; set; }
        public SelectList Fornecedores { get; set; }
        public SelectList TiposDocumento { get; set; }
        public SelectList CentrosCustos { get; set; }
        public SelectList Cfops { get; set; }
        public SelectList Estoques { get; set; }

        public CriarOuEditarEntradaModalViewModel(CriarOuEditarEntrada output)
        {
            output.MapTo(this);
        }
    }
}