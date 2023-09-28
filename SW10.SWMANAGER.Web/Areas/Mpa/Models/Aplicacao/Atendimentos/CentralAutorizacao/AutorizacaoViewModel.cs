using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.CentralAutorizacao
{
    [AutoMap(typeof(AutorizacaoProcedimentoDto))]
    public class AutorizacaoViewModel : AutorizacaoProcedimentoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public AutorizacaoViewModel()
        {
        }

        public AutorizacaoViewModel(AutorizacaoProcedimentoDto output)
        {
            output.MapTo(this);
        }

        public string Filtro { get; set; }


        public SelectList Status { get; set; }
        public long? StatusId { get; set; }
    }
}