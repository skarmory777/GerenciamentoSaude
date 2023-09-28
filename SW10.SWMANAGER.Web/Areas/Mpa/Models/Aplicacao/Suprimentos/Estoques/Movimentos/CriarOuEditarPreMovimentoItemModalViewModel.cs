using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    [AutoMap(typeof(EstoquePreMovimentoItemDto))]
    public class CriarOuEditarPreMovimentoItemModalViewModel : EstoquePreMovimentoItemDto
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

        public CriarOuEditarPreMovimentoItemModalViewModel(EstoquePreMovimentoItemDto output)
        {
            output.MapTo(this);
        }

        public string LotesValidadesJson { get; set; }
        public string NumerosSerieJson { get; set; }
        public decimal QuantidadeSolicitada { get; set; }
    }
}