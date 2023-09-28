using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ConsultorTabelas
{
    [AutoMapFrom(typeof(CriarOuEditarConsultorTabela))]
    public class CriarOuEditarConsultorTabelaModalViewModel : CriarOuEditarConsultorTabela
    {
        public UserEditDto UpdateUser { get; set; }

        public ICollection<ConsultorTabelaCampoDto> Campos { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarConsultorTabelaModalViewModel(CriarOuEditarConsultorTabela output)
        {
            output.MapTo(this);
        }
    }
}