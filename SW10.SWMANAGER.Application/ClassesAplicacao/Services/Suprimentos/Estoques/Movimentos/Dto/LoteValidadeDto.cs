using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(LoteValidade))]
    public class LoteValidadeDto : CamposPadraoCRUDDto
    {
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public long ProdutoId { get; set; }
        public long? ProdutoLaboratorioId { get; set; }

        public ProdutoLaboratorioDto ProdutoLaboratorio { get; set; }
        public ProdutoDto Produto { get; set; }



        public static LoteValidadeDto Mapear(LoteValidade loteValidade)
        {
            if (loteValidade == null)
            {
                return null;
            }

            var loteValidadeDto = new LoteValidadeDto
            {
                Id = loteValidade.Id,
                Codigo = loteValidade.Codigo,
                Descricao = loteValidade.Descricao,
                Lote = loteValidade.Lote,
                Validade = loteValidade.Validade,
                ProdutoId = loteValidade.ProdutoId,
                ProdutoLaboratorioId = loteValidade.EstLaboratorioId,

                ProdutoLaboratorio = ProdutoLaboratorioDto.Mapear(loteValidade.EstoqueLaboratorio),
                Produto = ProdutoDto.Mapear(loteValidade.Produto)
            };


            return loteValidadeDto;
        }


    }
}
