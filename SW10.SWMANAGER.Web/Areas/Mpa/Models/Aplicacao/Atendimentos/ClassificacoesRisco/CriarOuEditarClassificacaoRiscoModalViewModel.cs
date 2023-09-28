using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.ClassificacoesRisco
{
    [AutoMapFrom(typeof(CriarOuEditarClassificacaoRisco))]
    public class CriarOuEditarClassificacaoRiscoModalViewModel : CriarOuEditarClassificacaoRisco
    {
        public UserEditDto UpdateUser { get; set; }

        public int Sexo { get; set; }

        public SelectList Sexos { get; set; }

        public SelectList Especialidades { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarClassificacaoRiscoModalViewModel(CriarOuEditarClassificacaoRisco output)
        {
            output.MapTo(this);
        }
    }
}