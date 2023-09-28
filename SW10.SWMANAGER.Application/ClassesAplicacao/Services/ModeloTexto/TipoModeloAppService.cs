// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TipoModeloAppService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TipoModeloAppService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto
{
    using Abp.Domain.Repositories;
    using AutoMapper;
    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto.Dto;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// The tipo modelo app service.
    /// </summary>
    public class TipoModeloAppService : ITipoModeloAppService
    {
        /// <summary>
        /// The tipo modelo repository.
        /// </summary>
        private readonly IRepository<TipoModelo, long> tipoModeloRepository;

        /// <summary>
        /// The tipo modelo variaveis repository.
        /// </summary>
        private readonly IRepository<TipoModeloVariaveis, long> tipoModeloVariaveisRepository;

        /// <summary>
        /// The tipo modelo variaveis repository.
        /// </summary>
        private readonly IRepository<TamanhoModelo, long> tamanhoModeloRepository;

        /// <summary>
        /// The map.
        /// </summary>
        private readonly IMapper mapper;

        /// <inheritdoc />
        public TipoModeloAppService(
            IRepository<TipoModelo, long> tipoModeloRepository,
            IRepository<TipoModeloVariaveis, long> tipoModeloVariaveisRepository,
            IRepository<TamanhoModelo, long> tamanhoModeloRepository,
            IMapper mapper)
        {
            this.tipoModeloRepository = tipoModeloRepository;
            this.tipoModeloVariaveisRepository = tipoModeloVariaveisRepository;
            this.tamanhoModeloRepository = tamanhoModeloRepository;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<TipoModeloDto> ObterAsync(long id) =>
            this.mapper.Map<TipoModeloDto>(
                await this.tipoModeloRepository
                    .FirstOrDefaultAsync(id).ConfigureAwait(false));

        /// <inheritdoc />
        public async Task<List<TipoModeloVariaveisDto>> ObterVariaveisPorTipoModeloAsync(long tipoModeloId) =>
            this.mapper.Map<List<TipoModeloVariaveisDto>>(
                await this.tipoModeloVariaveisRepository
                    .GetAllListAsync(c => c.TipoModeloId == tipoModeloId).ConfigureAwait(false));

        /// <inheritdoc />
        public async Task<ResultDropdownList> ListarDropdownAsync(DropdownInput input)
        {
            var tipoModelo = this.tipoModeloRepository.GetAll().AsNoTracking()
                .Where(w => string.IsNullOrEmpty(input.filtro) || w.Descricao.ToUpper().Contains(input.filtro));

            var count = await tipoModelo.CountAsync().ConfigureAwait(false);

            var collection = await tipoModelo
                                 .ToListAsync().ConfigureAwait(false);

            return new ResultDropdownList
            {
                TotalCount = count,
                Items = collection.Select(x => new DropdownItems { id = x.Id, text = x.Descricao }).ToList()
            };
        }

        /// <inheritdoc />
        public async Task<ResultDropdownList> ListarTamanhoModeloDropdownAsync(DropdownInput input)
        {
            var tamanhoModelo = this.tamanhoModeloRepository.GetAll().AsNoTracking()
                .Where(w => string.IsNullOrEmpty(input.filtro) || w.Descricao.ToUpper().Contains(input.filtro));

            var count = await tamanhoModelo.CountAsync().ConfigureAwait(false);

            var collection = await tamanhoModelo
                                 .ToListAsync().ConfigureAwait(false);

            return new ResultDropdownList
            {
                TotalCount = count,
                Items = collection.Select(x => new DropdownItems { id = x.Id, text = x.Descricao }).ToList()
            };
        }

        /// <inheritdoc />
        public async Task<TamanhoModeloDto> ObterTamanhoAsync(long? tamanhoId)
        {
            if (tamanhoId == default(long?))
            {
                return null;
            }

            return this.mapper.Map<TamanhoModeloDto>(await this.tamanhoModeloRepository.FirstOrDefaultAsync(tamanhoId.Value).ConfigureAwait(false));
        }
    }
}
