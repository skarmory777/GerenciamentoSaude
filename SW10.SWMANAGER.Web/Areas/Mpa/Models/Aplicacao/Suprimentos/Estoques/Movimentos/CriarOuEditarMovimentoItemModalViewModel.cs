using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    public class CriarOuEditarMovimentoItemModalViewModel : MovimentoItemDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Produtos { get; set; }
        public SelectList Unidades { get; set; }
        public SelectList Laboratorios { get; set; }
        public SelectList LotesValidades { get; set; }

        public decimal CustoTotal { get; set; }
        public bool ControlaLoteValidade { get; set; }
        public string Hidden { get; set; }

        public long PreMovimentoEstadoId { get; set; }

        public long TransferenciaId { get; set; }

        public long PreMovimentoSaidaId { get; set; }
        public long PreMovimentoEntradaId { get; set; }
        public long TransferenciaItemId { get; set; }

        public bool IsNumeroSerie { get; set; }

        public CriarOuEditarMovimentoItemModalViewModel(MovimentoItemDto output)
        {
            this.CustoUnitario = output.CustoUnitario;
            this.Id = output.Id;
            this.NumeroSerie = output.NumeroSerie;
            this.PerIPI = output.PerIPI;
            this.ProdutoId = output.ProdutoId;
            this.Quantidade = output.Quantidade;
            this.ValorIPI = output.ValorIPI;
            this.ValorICMS = output.ValorICMS;
            this.PerICMS = output.PerICMS;
            this.ProdutoUnidadeId = output.ProdutoUnidadeId;
        }
    }
}