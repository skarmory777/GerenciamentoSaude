using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.UnidadesInternacao
{
    [AutoMapFrom(typeof(CriarOuEditarUnidadeInternacao))]
    public class CriarOuEditarUnidadeInternacaoModalViewModel : CriarOuEditarUnidadeInternacao
    {
        public UserEditDto UpdateUser { get; set; }

        //    public SelectList TiposAlta { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarUnidadeInternacaoModalViewModel(CriarOuEditarUnidadeInternacao output)
        {
            output.MapTo(this);
        }
    }
}