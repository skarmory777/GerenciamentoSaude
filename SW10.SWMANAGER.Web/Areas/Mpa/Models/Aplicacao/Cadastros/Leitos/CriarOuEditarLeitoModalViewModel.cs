using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.Leitos
{
    [AutoMap(typeof(LeitoDto))]
    public class CriarOuEditarLeitoModalViewModel : LeitoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Sexos { get; set; }

        public SelectList UnidadesInternacao { get; set; }

        public SelectList TiposAcomodacao { get; set; }

        public SelectList ItensTabelaDominio { get; set; }

        public List<SelectListItem> LeitosStatus { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarLeitoModalViewModel(LeitoDto output)
        {
            output.MapTo(this);
        }
    }
}