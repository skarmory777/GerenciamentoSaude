using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.TiposAtendimento
{
    [AutoMapFrom(typeof(TipoAtendimentoDto))]
    public class CriarOuEditarTipoAtendimentoModalViewModel : TipoAtendimentoDto
    {
        //public TipoAtendimentoDto TipoAtendimento { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarTipoAtendimentoModalViewModel(TipoAtendimentoDto output)
        {
            output.MapTo(this);
        }
    }
}