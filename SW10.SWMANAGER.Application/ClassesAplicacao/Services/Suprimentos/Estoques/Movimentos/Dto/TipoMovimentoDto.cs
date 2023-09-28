using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(TipoMovimento))]
    public class TipoMovimentoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
        public bool IsEntrada { get; set; }
        public bool IsOrdemCompra { get; set; }
        public bool IsPessoa { get; set; }
        public bool IsOrdemCompraObrigatoria { get; set; }
        public bool IsFiscal { get; set; }
        public bool IsFrete { get; set; }
        public bool IsFinanceiro { get; set; }


        public static TipoMovimentoDto Mapear(TipoMovimento tipoMovimento)
        {
            var tipoMovimentoDto = new TipoMovimentoDto();

            tipoMovimentoDto.Id = tipoMovimento.Id;
            tipoMovimentoDto.Codigo = tipoMovimento.Codigo;
            tipoMovimentoDto.Descricao = tipoMovimento.Descricao;
            tipoMovimentoDto.IsEntrada = tipoMovimento.IsEntrada;
            tipoMovimentoDto.IsOrdemCompra = tipoMovimento.IsOrdemCompra;
            tipoMovimentoDto.IsPessoa = tipoMovimento.IsPessoa;
            tipoMovimentoDto.IsOrdemCompraObrigatoria = tipoMovimento.IsOrdemCompraObrigatoria;
            tipoMovimentoDto.IsFiscal = tipoMovimento.IsFiscal;
            tipoMovimentoDto.IsFrete = tipoMovimento.IsFrete;
            tipoMovimentoDto.IsFinanceiro = tipoMovimento.IsFinanceiro;

            return tipoMovimentoDto;
        }


        public static TipoMovimento Mapear(TipoMovimentoDto tipoMovimentoDto)
        {
            var tipoMovimento = new TipoMovimento();

            tipoMovimento.Id = tipoMovimentoDto.Id;
            tipoMovimento.Codigo = tipoMovimentoDto.Codigo;
            tipoMovimento.Descricao = tipoMovimentoDto.Descricao;
            tipoMovimento.IsEntrada = tipoMovimentoDto.IsEntrada;
            tipoMovimento.IsOrdemCompra = tipoMovimentoDto.IsOrdemCompra;
            tipoMovimento.IsPessoa = tipoMovimentoDto.IsPessoa;
            tipoMovimento.IsOrdemCompraObrigatoria = tipoMovimentoDto.IsOrdemCompraObrigatoria;
            tipoMovimento.IsFiscal = tipoMovimentoDto.IsFiscal;
            tipoMovimento.IsFrete = tipoMovimentoDto.IsFrete;
            tipoMovimento.IsFinanceiro = tipoMovimentoDto.IsFinanceiro;

            return tipoMovimento;
        }

        public static List<TipoMovimentoDto> Mapear(List<TipoMovimento> tiposMovimentos)
        {
            if (tiposMovimentos == null)
            {
                return null;
            }

            var tiposDto = new List<TipoMovimentoDto>();

            foreach (var item in tiposMovimentos)
            {
                tiposDto.Add(Mapear(item));
            }

            return tiposDto;
        }

    }
}
