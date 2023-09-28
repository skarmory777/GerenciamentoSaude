using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto
{
    [AutoMap(typeof(FaturamentoConfigConvenio))]
    public class FaturamentoConfigConvenioDto : CamposPadraoCRUDDto
    {
        public long? EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }

        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public long? PlanoId { get; set; }
        public PlanoDto Plano { get; set; }

        public long? GrupoId { get; set; }
        public FaturamentoGrupoDto Grupo { get; set; }

        public long? SubGrupoId { get; set; }
        public FaturamentoSubGrupoDto SubGrupo { get; set; }

        public long? TabelaId { get; set; }
        public FaturamentoTabelaDto Tabela { get; set; }

        public long? ItemId { get; set; }
        public FaturamentoItemDto Item { get; set; }

        public DateTime? DataIncio { get; set; }

        public DateTime? DataFim { get; set; }



        public static FaturamentoConfigConvenioDto Mapear(FaturamentoConfigConvenio faturamentoConfigConvenio)
        {
            var faturamentoConfigConvenioDto = new FaturamentoConfigConvenioDto
            {
                Id = faturamentoConfigConvenio.Id,
                Codigo = faturamentoConfigConvenio.Codigo,
                Descricao = faturamentoConfigConvenio.Descricao,
                EmpresaId = faturamentoConfigConvenio.EmpresaId,
                Empresa = EmpresaDto.Mapear(faturamentoConfigConvenio.Empresa),
                ConvenioId = faturamentoConfigConvenio.ConvenioId,
                Convenio = ConvenioDto.Mapear(faturamentoConfigConvenio.Convenio),
                PlanoId = faturamentoConfigConvenio.PlanoId,
                Plano = PlanoDto.Mapear(faturamentoConfigConvenio.Plano),
                GrupoId = faturamentoConfigConvenio.GrupoId,
                Grupo = FaturamentoGrupoDto.Mapear(faturamentoConfigConvenio.Grupo),
                SubGrupoId = faturamentoConfigConvenio.SubGrupoId,
                SubGrupo = FaturamentoSubGrupoDto.Mapear(faturamentoConfigConvenio.SubGrupo),
                TabelaId = faturamentoConfigConvenio.TabelaId,
                Tabela = FaturamentoTabelaDto.Mapear(faturamentoConfigConvenio.Tabela),
                ItemId = faturamentoConfigConvenio.ItemId,
                Item = FaturamentoItemDto.Mapear(faturamentoConfigConvenio.Item),
                DataIncio = faturamentoConfigConvenio.DataIncio,
                DataFim = faturamentoConfigConvenio.DataFim
            };


            return faturamentoConfigConvenioDto;

        }

        public static FaturamentoConfigConvenio Mapear(FaturamentoConfigConvenioDto faturamentoConfigConvenioDto)
        {
            FaturamentoConfigConvenio faturamentoConfigConvenio = new FaturamentoConfigConvenio
            {
                EmpresaId = faturamentoConfigConvenioDto.EmpresaId,
                ConvenioId = faturamentoConfigConvenioDto.ConvenioId,
                PlanoId = faturamentoConfigConvenioDto.PlanoId,
                GrupoId = faturamentoConfigConvenioDto.GrupoId,
                SubGrupoId = faturamentoConfigConvenioDto.SubGrupoId,
                TabelaId = faturamentoConfigConvenioDto.TabelaId,
                ItemId = faturamentoConfigConvenioDto.ItemId,
                DataIncio = faturamentoConfigConvenioDto.DataIncio,
                DataFim = faturamentoConfigConvenioDto.DataFim,
                Empresa = EmpresaDto.Mapear(faturamentoConfigConvenioDto.Empresa),
                Convenio = ConvenioDto.Mapear(faturamentoConfigConvenioDto.Convenio),
                Plano = PlanoDto.Mapear(faturamentoConfigConvenioDto.Plano),
                Grupo = FaturamentoGrupoDto.Mapear(faturamentoConfigConvenioDto.Grupo),
                SubGrupo = FaturamentoSubGrupoDto.Mapear(faturamentoConfigConvenioDto.SubGrupo),
                Tabela = FaturamentoTabelaDto.Mapear(faturamentoConfigConvenioDto.Tabela),
                Item = FaturamentoItemDto.Mapear(faturamentoConfigConvenioDto.Item),
            };

            return faturamentoConfigConvenio;

        }


        public static List<FaturamentoConfigConvenioDto> Mapear(List<FaturamentoConfigConvenio> faturamentosConfigConvenio)
        {
            var faturamentosDto = new List<FaturamentoConfigConvenioDto>();

            if (faturamentosConfigConvenio == null)
            {
                return faturamentosDto;
            }

            faturamentosDto.AddRange(faturamentosConfigConvenio.Select(Mapear));
            return faturamentosDto;
        }



    }


    public class FaturamentoConfigAtualDto: CamposPadraoCRUDDto
    {
        public long? EmpresaId { get; set; }

        public EmpresaDto Empresa { get; set; }
        public long? ConvenioId { get; set; }

        public ConvenioDto Convenio { get; set; }
        public long? PlanoId { get; set; }

        public PlanoDto Plano { get; set; }

        public long? ItemId { get; set; }

        public long? GrupoId { get; set; }

        public FaturamentoGrupoDto Grupo { get; set; }

        public long? SubGrupoId { get; set; }

        public FaturamentoSubGrupoDto SubGrupo { get; set; }

        public long? TabelaId { get; set; }

        public FaturamentoTabelaDto Tabela { get; set; }

        public long? SisMoedaId { get; set; }

        public SisMoedaDto Moeda { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public long? ItemTabelaId { get; set; }

        public FaturamentoItemDto Item { get; set; }

        public float? ValorTotal { get; set; }

        public float Preco { get; set; }

        public float? COCH { get; set; }

        public float? HMCH { get; set; }

        public float? Auxiliar { get; set; }

        public float? Porte { get; set; }

        public bool IsInclusaoManual { get; set; }
    }
}
