using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.PreAtendimentos
{
    [AutoMapFrom(typeof(CriarOuEditarPreAtendimento))]
    public class CriarOuEditarPreAtendimentoModalViewModel : CriarOuEditarPreAtendimento
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Sexos { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarPreAtendimentoModalViewModel(CriarOuEditarPreAtendimento output)
        {
            output.MapTo(this);
        }
    }
}