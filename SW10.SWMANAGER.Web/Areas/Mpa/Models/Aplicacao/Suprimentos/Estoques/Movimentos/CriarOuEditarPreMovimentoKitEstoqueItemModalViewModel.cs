using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    public class CriarOuEditarPreMovimentoKitEstoqueItemModalViewModel
    {
        public UserEditDto UpdateUser { get; set; }
        public int KitEstoqueId { get; set; }
        public int Quantidade { get; set; }
        public SelectList KitsEstoque { get; set; }
        public long PreMovimentoId { get; set; }
    }
}