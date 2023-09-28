using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto
{
    [AutoMap(typeof(FaturamentoEntregaConta))]
    public class FaturamentoEntregaContaDto : CamposPadraoCRUDDto
    {
        public long? ContaMedicaId { get; set; }
        public FaturamentoContaDto ContaMedica { get; set; }
        public long? EntregaLoteId { get; set; }
        public FaturamentoEntregaLoteDto EntregaLote { get; set; }

        public float ValorConta { get; set; }
        public float ValorTaxas { get; set; }
        public float ValorFranquia { get; set; }
        public float ValorProduzido { get; set; }
        public float ValorProduzidoTaxas { get; set; }
        public float ValorRecebido { get; set; }
        public float ValorRecebidoTemp { get; set; }

        public bool IsGlosa { get; set; }
        public bool IsRecebe { get; set; }
        public bool IsRecebeTudo { get; set; }
        public bool IsErroGuia { get; set; }

        public DateTime? DataEntrega { get; set; }
        public DateTime? DataFinalEntrega { get; set; }
        public DateTime? DataUsuarioEntrega { get; set; }
        public DateTime? DataUsuarioTemp { get; set; }

        public long? UsuarioTempId { get; set; }
        public long? UsuarioEntregaId { get; set; }


        public static FaturamentoEntregaContaDto Mapear(FaturamentoEntregaConta faturamentoEntregaConta)
        {
            FaturamentoEntregaContaDto faturamentoEntregaContaDto = new FaturamentoEntregaContaDto();

            faturamentoEntregaContaDto.Id = faturamentoEntregaConta.Id;
            faturamentoEntregaContaDto.Codigo = faturamentoEntregaConta.Codigo;
            faturamentoEntregaContaDto.Descricao = faturamentoEntregaConta.Descricao;

            faturamentoEntregaContaDto.ContaMedicaId = faturamentoEntregaConta.ContaMedicaId;
            faturamentoEntregaContaDto.EntregaLoteId = faturamentoEntregaConta.ContaMedicaId;
            // public FaturamentoEntregaLoteDto EntregaLote { get; set; }

            faturamentoEntregaContaDto.ValorConta = faturamentoEntregaConta.ValorConta;
            faturamentoEntregaContaDto.ValorTaxas = faturamentoEntregaConta.ValorTaxas;
            faturamentoEntregaContaDto.ValorFranquia = faturamentoEntregaConta.ValorFranquia;
            faturamentoEntregaContaDto.ValorProduzido = faturamentoEntregaConta.ValorProduzido;
            faturamentoEntregaContaDto.ValorProduzidoTaxas = faturamentoEntregaConta.ValorProduzidoTaxas;
            faturamentoEntregaContaDto.ValorRecebido = faturamentoEntregaConta.ValorRecebido;
            faturamentoEntregaContaDto.ValorRecebidoTemp = faturamentoEntregaConta.ValorRecebidoTemp;
            faturamentoEntregaContaDto.IsGlosa = faturamentoEntregaConta.IsGlosa;
            faturamentoEntregaContaDto.IsRecebe = faturamentoEntregaConta.IsRecebe;
            faturamentoEntregaContaDto.IsRecebeTudo = faturamentoEntregaConta.IsRecebeTudo;
            faturamentoEntregaContaDto.IsErroGuia = faturamentoEntregaConta.IsErroGuia;
            faturamentoEntregaContaDto.DataEntrega = faturamentoEntregaConta.DataEntrega;
            faturamentoEntregaContaDto.DataFinalEntrega = faturamentoEntregaConta.DataFinalEntrega;
            faturamentoEntregaContaDto.DataUsuarioEntrega = faturamentoEntregaConta.DataUsuarioEntrega;
            faturamentoEntregaContaDto.DataUsuarioTemp = faturamentoEntregaConta.DataUsuarioTemp;
            faturamentoEntregaContaDto.UsuarioTempId = faturamentoEntregaConta.UsuarioTempId;
            faturamentoEntregaContaDto.UsuarioEntregaId = faturamentoEntregaConta.UsuarioEntregaId;

            if (faturamentoEntregaConta.ContaMedica != null)
            {
                faturamentoEntregaContaDto.ContaMedica = FaturamentoContaDto.Mapear(faturamentoEntregaConta.ContaMedica);
            }

            if (faturamentoEntregaConta.EntregaLote != null)
            {
                faturamentoEntregaContaDto.EntregaLote = FaturamentoEntregaLoteDto.Mapear(faturamentoEntregaConta.EntregaLote);
            }


            return faturamentoEntregaContaDto;
        }

        public static List<FaturamentoEntregaContaDto> Mapear(List<FaturamentoEntregaConta> itens)
        {
            List<FaturamentoEntregaContaDto> lista = new List<FaturamentoEntregaContaDto>();
            foreach (var item in itens)
            {
                lista.Add(Mapear(item));
            }
            return lista;
        }

    }
}
