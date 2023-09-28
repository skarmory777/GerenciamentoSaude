using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    public class TipoLocalChamadaDto : CamposPadraoCRUDDto
    {
        public List<LocalChamadaDto> LocalChamadas { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public static TipoLocalChamadaDto Mapear(TipoLocalChamada tipoLocalChamada, bool isService = false)
        {
            TipoLocalChamadaDto tipoLocalChamadaDto = new TipoLocalChamadaDto
            {
                Id = tipoLocalChamada.Id,
                Codigo = tipoLocalChamada.Codigo,
                Descricao = tipoLocalChamada.Descricao,
                UnidadeOrganizacionalId = tipoLocalChamada.UnidadeOrganizacionalId
            };
            if (tipoLocalChamada.UnidadeOrganizacional != null)
            {
                tipoLocalChamadaDto.UnidadeOrganizacional = new UnidadeOrganizacionalDto();
                tipoLocalChamadaDto.UnidadeOrganizacional.Id = tipoLocalChamada.UnidadeOrganizacional.Id;
                tipoLocalChamadaDto.UnidadeOrganizacional.Codigo = tipoLocalChamada.UnidadeOrganizacional.Codigo;
                tipoLocalChamadaDto.UnidadeOrganizacional.Descricao = tipoLocalChamada.UnidadeOrganizacional.Descricao;
            }
            if (isService)
            {
                var list = new List<LocalChamadaDto>();
                foreach (var item in tipoLocalChamada.LocalChamadas)
                {
                    list.Add(LocalChamadaDto.Mapear(item));
                    tipoLocalChamadaDto.LocalChamadas = list;
                }
            }

            return tipoLocalChamadaDto;
        }

        public static TipoLocalChamada Mapear(TipoLocalChamadaDto tipoLocalChamadaDto, bool isService = false)
        {
            TipoLocalChamada tipoLocalChamada = new TipoLocalChamada
            {
                Id = tipoLocalChamadaDto.Id,
                Codigo = tipoLocalChamadaDto.Codigo,
                Descricao = tipoLocalChamadaDto.Descricao,
                UnidadeOrganizacionalId = tipoLocalChamadaDto.UnidadeOrganizacionalId
            };
            if (tipoLocalChamadaDto.UnidadeOrganizacional != null)
            {
                tipoLocalChamada.UnidadeOrganizacional = new UnidadeOrganizacional();
                tipoLocalChamada.UnidadeOrganizacional.Id = tipoLocalChamadaDto.UnidadeOrganizacional.Id;
                tipoLocalChamada.UnidadeOrganizacional.Codigo = tipoLocalChamadaDto.UnidadeOrganizacional.Codigo;
                tipoLocalChamada.UnidadeOrganizacional.Descricao = tipoLocalChamadaDto.UnidadeOrganizacional.Descricao;
            }
            if (isService)
            {
                var list = new List<LocalChamada>();
                foreach (var item in tipoLocalChamadaDto.LocalChamadas)
                {
                    list.Add(LocalChamadaDto.Mapear(item));
                    tipoLocalChamada.LocalChamadas = list;
                }
            }

            return tipoLocalChamada;
        }

        public static List<TipoLocalChamadaDto> Mapear(List<TipoLocalChamada> entityList)
        {
            var dtoList = new List<TipoLocalChamadaDto>();

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
