using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Imagens
{

    [AutoMap(typeof(LaudoMovimentoItemDto))]
    public class GestaoLaudoViewModel : LaudoMovimentoItemDto
    {
        public GestaoLaudoViewModel(LaudoMovimentoItemDto output)
        {
            output.MapTo(this);
        }

        public UserEditDto UpdateUser { get; set; }
        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public string Filtro { get; set; }
        public bool IsParecer { get; set; }
    }
}