using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Orcamentos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Orcamentos
{
    [AutoMapFrom(typeof(CriarOuEditarOrcamento))]
    public class CriarOuEditarOrcamentoModalViewModel : CriarOuEditarOrcamento
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Convenios { get; set; }

        public SelectList Planos { get; set; }

        public SelectList Prestadores { get; set; }

        public SelectList Empresas { get; set; }

        public SelectList CentrosCusto { get; set; }

        public SelectList UnidadesOrganizacional { get; set; }

        public SelectList Pacientes { get; set; }


        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarOrcamentoModalViewModel(CriarOuEditarOrcamento output)
        {
            output.MapTo(this);
        }
    }
}