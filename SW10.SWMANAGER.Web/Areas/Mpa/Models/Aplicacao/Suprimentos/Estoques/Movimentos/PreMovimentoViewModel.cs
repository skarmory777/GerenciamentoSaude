using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    public class PreMovimentoViewModel : EstoquePreMovimentoDto
    {
        public string Filtro { get; set; }
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Empresas { get; set; }
        public SelectList Fornecedores { get; set; }
        public SelectList Estoques { get; set; }
        public SelectList TipoMovimentos { get; set; }
        public SelectList TipoOperacaoes { get; set; }

        public PreMovimentoViewModel()
        {

        }

        public PreMovimentoViewModel(EstoquePreMovimentoDto output)
        {
            output.MapTo(this);
        }
    }
}