using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposLeito
{
    [AutoMapFrom(typeof(CriarOuEditarTipoLeito))]
    public class CriarOuEditarTipoLeitoModalViewModel : CriarOuEditarTipoLeito
    {
        //public TipoAtendimentoDto TipoAtendimento { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarTipoLeitoModalViewModel(CriarOuEditarTipoLeito output)
        {
            output.MapTo(this);
        }


    }
}