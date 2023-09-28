using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    [AutoMap(typeof(EstoqueGrupo))]
    public class EstoqueGrupoDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Estoque.
        /// </summary>
        public EstoqueDto Estoque { get; set; }
        public long? EstoqueId { get; set; }

        /// <summary>
        /// Grupo de Produtos.
        /// </summary>
        public GrupoDto Grupo { get; set; }
        public long? GrupoId { get; set; }

        /// <summary>
        /// IsTodosItens.
        /// </summary>
        public bool? IsTodosItens { get; set; }


        public static EstoqueGrupoDto Mapear(EstoqueGrupo estoqueGrupo)
        {
            var estoqueGrupoDto = MapearBase<EstoqueGrupoDto>(estoqueGrupo);

            estoqueGrupoDto.EstoqueId = estoqueGrupo.EstoqueId;
            estoqueGrupoDto.GrupoId = estoqueGrupo.GrupoId;
            estoqueGrupoDto.IsTodosItens = estoqueGrupo.IsTodosItens;
            estoqueGrupoDto.Estoque = MapearBase<EstoqueDto>(estoqueGrupo.Estoque);
            estoqueGrupoDto.Grupo = MapearBase<GrupoDto>(estoqueGrupo.Grupo);

            return estoqueGrupoDto;
        }

        public static EstoqueGrupo Mapear(EstoqueGrupoDto estoqueGrupoDto)
        {
            var estoqueGrupo = MapearBase<EstoqueGrupo>(estoqueGrupoDto);

            estoqueGrupo.EstoqueId = estoqueGrupoDto.EstoqueId;
            estoqueGrupo.GrupoId = estoqueGrupoDto.GrupoId;
            estoqueGrupo.IsTodosItens = estoqueGrupoDto.IsTodosItens;
            estoqueGrupo.Estoque = MapearBase<Estoque>(estoqueGrupoDto.Estoque);
            estoqueGrupo.Grupo = MapearBase<Grupo>(estoqueGrupoDto.Grupo);

            return estoqueGrupo;
        }

    }
}