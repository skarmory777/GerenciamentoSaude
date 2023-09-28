using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto
{
    [AutoMap(typeof(FaturamentoConfigConvenioGlobal))]
    public class FaturamentoConfigConvenioGlobalDto : CamposPadraoCRUDDto
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
        public long? TabelaGlobalId { get; set; }
        public FaturamentoGlobalDto TabelaGlobal { get; set; }
        public long? ItemId { get; set; }
        public FaturamentoItemDto Item { get; set; }
        public DateTime? DataIncio { get; set; }
        public DateTime? DataFim { get; set; }

        public static FaturamentoConfigConvenioGlobalDto Mapear(FaturamentoConfigConvenioGlobal entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<FaturamentoConfigConvenioGlobalDto>(entity);
            dto.EmpresaId = entity.EmpresaId;
            dto.Empresa = EmpresaDto.Mapear(entity.Empresa);
            dto.EmpresaId = entity.ConvenioId;
            dto.Convenio = ConvenioDto.Mapear(entity.Convenio);
            dto.PlanoId = entity.PlanoId;
            dto.Plano = PlanoDto.Mapear(entity.Plano);
            dto.GrupoId = entity.GrupoId;
            dto.Grupo = FaturamentoGrupoDto.Mapear(entity.Grupo);
            dto.SubGrupoId = entity.SubGrupoId;
            dto.SubGrupo = FaturamentoSubGrupoDto.Mapear(entity.SubGrupo);
            dto.TabelaGlobalId = entity.TabelaGlobalId;
            dto.TabelaGlobal = MapearBase<FaturamentoGlobalDto>(entity.TabelaGlobal);
            dto.ItemId = entity.ItemId;
            dto.Item = FaturamentoItemDto.Mapear(entity.Item);
            dto.DataIncio = entity.DataIncio;
            dto.DataFim = entity.DataFim;

            return dto;
        }

        public static List<FaturamentoConfigConvenioGlobalDto> Mapear(List<FaturamentoConfigConvenioGlobal> entityList)
        {
            var dtoList = new List<FaturamentoConfigConvenioGlobalDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }
    }
}
