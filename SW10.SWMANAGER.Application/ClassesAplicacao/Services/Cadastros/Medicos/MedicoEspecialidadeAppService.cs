using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos
{
    using Abp.Auditing;
    using SW10.SWMANAGER.Helpers;
    using System.Text;

    public class MedicoEspecialidadeAppService : SWMANAGERAppServiceBase, IMedicoEspecialidadeAppService
    {
        private readonly IRepository<MedicoEspecialidade, long> _medicoEspecialidadeRepository;
        private readonly IRepository<Especialidade, long> _especialidadeRepository;

        public MedicoEspecialidadeAppService(IRepository<MedicoEspecialidade, long> medicoEspecialidadeRepository
                                            , IRepository<Especialidade, long> especialidadeRepository)
        {
            _medicoEspecialidadeRepository = medicoEspecialidadeRepository;
            _especialidadeRepository = especialidadeRepository;
        }

        public async Task CriarOuEditar(MedicoEspecialidadeDto input)
        {
            try
            {
                var medicoEspecialidade = new MedicoEspecialidade();
                medicoEspecialidade = input.MapTo<MedicoEspecialidade>();
                if (input.Id.Equals(0))
                {
                    await this._medicoEspecialidadeRepository.InsertAsync(medicoEspecialidade).ConfigureAwait(false);
                }
                else
                {

                    var ori = await this._medicoEspecialidadeRepository.GetAsync(input.Id).ConfigureAwait(false);

                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.EspecialidadeId = input.EspecialidadeId;
                    ori.IsSistema = input.IsSistema;
                    ori.MedicoId = input.MedicoId;

                    await this._medicoEspecialidadeRepository.UpdateAsync(ori).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Excluir(MedicoEspecialidadeDto input)
        {
            try
            {
                await this._medicoEspecialidadeRepository.DeleteAsync(input.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<MedicoEspecialidadeDto>> Listar(ListarInput input)
        {
            var contarMedicoEspecialidade = 0;
            var idGrid = 0;
            List<MedicoEspecialidade> formulaEstoque;
            List<MedicoEspecialidadeDto> formulaEstoqueDtos = new List<MedicoEspecialidadeDto>();
            try
            {
                var query = _medicoEspecialidadeRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Id.ToString().Contains(input.Filtro.ToUpper()));

                contarMedicoEspecialidade = await query
                                                .CountAsync().ConfigureAwait(false);

                formulaEstoque = await query
                                     .AsNoTracking()
                                     .OrderBy(input.Sorting)
                                     .PageBy(input)
                                     .ToListAsync().ConfigureAwait(false);

                formulaEstoqueDtos = formulaEstoque
                    .MapTo<List<MedicoEspecialidadeDto>>();

                formulaEstoqueDtos.ForEach(m => m.IdGridMedicoEspecialidade = ++idGrid);

                return new PagedResultDto<MedicoEspecialidadeDto>(
                    contarMedicoEspecialidade,
                    formulaEstoqueDtos);
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<MedicoEspecialidadeDto>> ListarJson(List<MedicoEspecialidadeDto> list)
        {
            try
            {
                var count = 0;
                if (list == null)
                {
                    list = new List<MedicoEspecialidadeDto>();
                }
                //for (int i = 0; i < list.Count(); i++)
                //{
                //    list[i].IdGridFormulasEstoque = i;
                //}
                count = await Task.Run(() => list.Count()).ConfigureAwait(false);

                return new PagedResultDto<MedicoEspecialidadeDto>(count, list);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPresquisar"), ex);
            }
        }

        public async Task<ListResultDto<MedicoEspecialidadeDto>> ListarMedicoEspecialidadePorMedico(long id)
        {
            try
            {
                var query = _medicoEspecialidadeRepository
                    .GetAll()
                    .Include(x => x.Especialidade)
                    .Where(m => m.MedicoId == id) // m.FaturamentoItem.IsRequisicaoExame && m.FaturamentoItem.IsLaudo)
                    .OrderBy(m => m.Codigo);

                var medicoEspecialidade = await query
                                              .AsNoTracking()
                                              .ToListAsync().ConfigureAwait(false);

                var medicosEspecialidadesDto = medicoEspecialidade
                    .MapTo<List<MedicoEspecialidadeDto>>();

                var idGrid = 0;
                medicosEspecialidadesDto.ForEach(m => m.IdGridMedicoEspecialidade = ++idGrid);
                return new ListResultDto<MedicoEspecialidadeDto>
                {
                    Items = medicosEspecialidadesDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<MedicoEspecialidadeDto> Obter(long id)
        {
            try
            {
                var result = await this._medicoEspecialidadeRepository
                                 .GetAll()
                                 .Include(m => m.Especialidade)
                                 .Include(m => m.Medico)
                                 .Include(m => m.Medico.SisPessoa)
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);


                var medicoEspecialidade = result
                    //.FirstOrDefault()
                    .MapTo<MedicoEspecialidadeDto>();

                return medicoEspecialidade;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<GenericoIdNome> ObterSomenteUmaEspecialidade(long medicoId)
        {
            try
            {
                var especialidades = await this._medicoEspecialidadeRepository.GetAll()
                                         .Include(i => i.Especialidade)
                                         .Where(w => w.MedicoId == medicoId)
                                         .ToListAsync().ConfigureAwait(false);


                if (especialidades != null && especialidades.Count == 1)
                {
                    return new GenericoIdNome { Id = especialidades[0].Id, Nome = especialidades[0].Especialidade?.Descricao };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._medicoEspecialidadeRepository)
                       .AddIdField("SisMedicoEspecialidade.Id")
                       .EnableDistinct()
                       .AddTextField("CONCAT(SisEspecialidade.Codigo, ' - ', SisEspecialidade.Descricao)")
                       .AddFromClause("SisMedicoEspecialidade INNER JOIN SisEspecialidade ON SisMedicoEspecialidade.SisEspecialidadeId = SisEspecialidade.id")
                       .AddWhereMethod(
                           (input, dapperParameters) =>
                               {
                                   dapperParameters.Add("deleted", false);
                                   var whereBuilder = new StringBuilder();
                                   whereBuilder.Append(" AND SisMedicoEspecialidade.IsDeleted = @deleted");
                                   whereBuilder.Append(" AND SisEspecialidade.IsDeleted = @deleted");

                                   whereBuilder.WhereIf(
                                       !input.search.IsNullOrEmpty(),
                                       " AND (SisEspecialidade.Descricao LIKE '%' + @search + '%' OR SisEspecialidade.Codigo LIKE '%' + @search + '%')");
                                   return whereBuilder.ToString();
                               })
                       .AddOrderByClause("CONCAT(SisEspecialidade.Codigo, ' - ', SisEspecialidade.Descricao)")
                       .AddDefaultErrorMessage(L("ErroPesquisar"))
                       .ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownPorMedico(DropdownInput dropdownInput)
        {
            if (dropdownInput.filtro == null)
            {
                return new ResultDropdownList() { Items = new List<DropdownItems>(), TotalCount = 0 };
            }

            return await this.CreateSelect2(this._medicoEspecialidadeRepository)
                       .AddIdField("SisMedicoEspecialidade.Id")
                       .EnableDistinct()
                       .AddTextField("CONCAT(SisEspecialidade.Codigo, ' - ', SisEspecialidade.Descricao)")
                       .AddFromClause("SisMedicoEspecialidade INNER JOIN SisEspecialidade ON SisMedicoEspecialidade.SisEspecialidadeId = SisEspecialidade.id")
                       .AddWhereMethod(
                           (input, dapperParameters) =>
                               {
                                   dapperParameters.Add("deleted", false);

                                   dapperParameters.Add("medicoId", input.filtro);

                                   var whereBuilder = new StringBuilder();
                                   whereBuilder.Append(" AND SisMedicoEspecialidade.IsDeleted = @deleted");
                                   whereBuilder.Append(" AND SisEspecialidade.IsDeleted = @deleted");

                                   whereBuilder.WhereIf(!input.filtro.IsNullOrEmpty(), " AND SisMedicoEspecialidade.SisMedicoId = @medicoId");

                                   whereBuilder.WhereIf(
                                       !input.search.IsNullOrEmpty(),
                                       " AND (SisEspecialidade.Descricao LIKE '%' + @search + '%' OR SisEspecialidade.Codigo LIKE '%' + @search + '%')");
                                   return whereBuilder.ToString();
                               })
                       .AddOrderByClause("CONCAT(SisEspecialidade.Codigo, ' - ', SisEspecialidade.Descricao)")
                       .AddDefaultErrorMessage(L("ErroPesquisar"))
                       .ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public async Task<IResultDropdownList<long>> ListarDropdownPorMedicoTodas(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<MedicoEspecialidadeDto> especialidadesDto = new List<MedicoEspecialidadeDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }


                IEnumerable<DropdownItems> query;


                if (!string.IsNullOrEmpty(dropdownInput.filtro))
                {


                    query = from p in _medicoEspecialidadeRepository.GetAll()
                            .Include(e => e.Especialidade)
                            .Include(e => e.Medico)
                            .Include(e => e.Medico.SisPessoa)
                            .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), me =>
                                me.Medico.Id.ToString().Equals(dropdownInput.filtro)
                            )
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Especialidade.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Especialidade.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )

                            orderby p.Especialidade.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Especialidade.Codigo, " - ", p.Especialidade.Nome)
                            };
                }
                else
                {
                    query = from p in _especialidadeRepository.GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Nome)
                            };
                }



                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = queryResultPage.ToList();//  await .ToListAsync();

                int total = query.Count();  // await CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<MedicoEspecialidadeDto>> ListarPorMedico(ListarInput input)
        {
            var contarMedicoEspecialidade = 0;
            var idGrid = 0;
            List<MedicoEspecialidade> formulaEstoque;
            List<MedicoEspecialidadeDto> formulaEstoqueDtos = new List<MedicoEspecialidadeDto>();
            try
            {
                var query = _medicoEspecialidadeRepository
                    .GetAll()
                    .Include(x => x.Especialidade)
                                        .WhereIf(
                                !input.Filtro.IsNullOrEmpty(), m => m.MedicoId.ToString().Equals(input.Filtro.ToUpper())
                            );

                contarMedicoEspecialidade = await query
                                                .CountAsync().ConfigureAwait(false);

                formulaEstoque = await query
                                     .AsNoTracking()
                                     .OrderBy(input.Sorting)
                                     .PageBy(input)
                                     .ToListAsync().ConfigureAwait(false);

                formulaEstoqueDtos = formulaEstoque
                    .MapTo<List<MedicoEspecialidadeDto>>();

                formulaEstoqueDtos.ForEach(m => m.IdGridMedicoEspecialidade = ++idGrid);

                return new PagedResultDto<MedicoEspecialidadeDto>(
                    contarMedicoEspecialidade,
                    formulaEstoqueDtos
                    );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
