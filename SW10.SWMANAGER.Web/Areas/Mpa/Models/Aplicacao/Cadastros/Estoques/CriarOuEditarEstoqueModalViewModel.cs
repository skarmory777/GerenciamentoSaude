using Abp.AutoMapper;

using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Estoques
{
    [AutoMap(typeof(EstoqueDto))]
    public class CriarOuEditarEstoqueModalViewModel : EstoqueDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }



        //public SelectList EstoqueGrupo { get; set; }

        public CriarOuEditarEstoqueModalViewModel(EstoqueDto output)
        {
            output.MapTo(this);
        }
    }
}