using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.SisMoedas
{
    [AutoMapFrom(typeof(SisMoedaDto))]
    public class CriarOuEditarSisMoedaModalViewModel : SisMoedaDto
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Tipos { get; set; }

        public long? Sel2 { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarSisMoedaModalViewModel(SisMoedaDto output)
        {
            output.MapTo(this);
        }
    }
}