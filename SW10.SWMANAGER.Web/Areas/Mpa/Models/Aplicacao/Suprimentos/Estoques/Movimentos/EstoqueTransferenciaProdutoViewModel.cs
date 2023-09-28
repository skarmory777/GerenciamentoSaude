using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    public class EstoqueTransferenciaProdutoViewModel : EstoqueTransferenciaProdutoDto
    {
        public UserEditDto UpdateUser { get; set; }
        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList EstoquesSaida { get; set; }
        public SelectList EstoquesEntrada { get; set; }
    }
}