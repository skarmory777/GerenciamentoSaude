using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosPerdaProdutos;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(MotivoPerdaProduto))]
    public class MotivoPerdaProdutoDto : CamposPadraoCRUDDto
    {
        public static MotivoPerdaProdutoDto Mapear(MotivoPerdaProduto motivoPerdaProduto)
        {
            if (motivoPerdaProduto == null) return null;

            var motivoPerdaProdutoDto = new MotivoPerdaProdutoDto
            {
                Id = motivoPerdaProduto.Id,
                Codigo = motivoPerdaProduto.Codigo,
                Descricao = motivoPerdaProduto.Descricao
            };

            return motivoPerdaProdutoDto;
        }


        public static MotivoPerdaProduto Mapear(MotivoPerdaProdutoDto motivoPerdaProdutoDto)
        {
            var motivoPerdaProduto = new MotivoPerdaProduto();

            motivoPerdaProduto.Id = motivoPerdaProdutoDto.Id;
            motivoPerdaProduto.Codigo = motivoPerdaProdutoDto.Codigo;
            motivoPerdaProduto.Descricao = motivoPerdaProdutoDto.Descricao;

            return motivoPerdaProduto;
        }

        public static List<MotivoPerdaProdutoDto> Mapear(List<MotivoPerdaProduto> motivosPerdasProdutos)
        {
            if (motivosPerdasProdutos != null)
            {
                return null;
            }

            var motivosDto = new List<MotivoPerdaProdutoDto>();

            foreach (var item in motivosPerdasProdutos)
            {
                motivosDto.Add(Mapear(item));
            }


            return motivosDto;
        }

    }
}
