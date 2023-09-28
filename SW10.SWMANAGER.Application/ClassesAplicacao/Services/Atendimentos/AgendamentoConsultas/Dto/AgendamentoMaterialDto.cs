using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class AgendamentoMaterialDto : CamposPadraoCRUD
    {
        public long? AgendamentoCirurgicoId { get; set; }
        public AgendamentoCirurgicoDto AgendamentoCirurgico { get; set; }
        public long FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public decimal Quantidade { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public DateTime? DataPrevista { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public decimal ValorNotaFiscal { get; set; }
        public long FornecedorId { get; set; }
        public SisFornecedorDto Fornecedor { get; set; }
        public bool IsCobrarPeloHospital { get; set; }
        public string Material { get; set; }


        public static AgendamentoMaterialDto Mapear(AgendamentoMaterial agendamentoMaterial)
        {
            AgendamentoMaterialDto agendamentoMaterialDto = new AgendamentoMaterialDto();

            agendamentoMaterialDto.Id = agendamentoMaterial.Id;
            agendamentoMaterialDto.Codigo = agendamentoMaterial.Codigo;
            agendamentoMaterialDto.Descricao = agendamentoMaterial.Descricao;


            agendamentoMaterialDto.AgendamentoCirurgicoId = agendamentoMaterial.AgendamentoCirurgicoId;
            agendamentoMaterialDto.FaturamentoItemId = agendamentoMaterial.FaturamentoItemId ?? 0;
            agendamentoMaterialDto.Quantidade = agendamentoMaterial.Quantidade;
            agendamentoMaterialDto.DataRecebimento = agendamentoMaterial.DataRecebimento;
            agendamentoMaterialDto.DataPrevista = agendamentoMaterial.DataPrevista;
            agendamentoMaterialDto.NumeroNotaFiscal = agendamentoMaterial.NumeroNotaFiscal;
            agendamentoMaterialDto.ValorNotaFiscal = agendamentoMaterial.ValorNotaFiscal;
            agendamentoMaterialDto.FornecedorId = agendamentoMaterial.FornecedorId;
            agendamentoMaterialDto.IsCobrarPeloHospital = agendamentoMaterial.IsCobrarPeloHospital;
            agendamentoMaterialDto.Material = agendamentoMaterial.Material;

            if (agendamentoMaterial.Fornecedor != null)
            {
                agendamentoMaterialDto.Fornecedor = new SisFornecedorDto { Id = agendamentoMaterial.Fornecedor.Id, NomeFantasia = agendamentoMaterial.Fornecedor.SisPessoa.NomeFantasia };
            }

            return agendamentoMaterialDto;
        }

        public static List<AgendamentoMaterialDto> Mapear(List<AgendamentoMaterial> materiais)
        {
            List<AgendamentoMaterialDto> materiaisDto = new List<AgendamentoMaterialDto>();

            if (materiais != null)
            {
                foreach (var item in materiais)
                {
                    materiaisDto.Add(Mapear(item));
                }
            }

            return materiaisDto;
        }


    }
}
