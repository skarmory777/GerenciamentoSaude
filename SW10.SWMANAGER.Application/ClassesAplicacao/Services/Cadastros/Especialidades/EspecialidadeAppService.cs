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
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades
{
    public class EspecialidadeAppService : SWMANAGERAppServiceBase, IEspecialidadeAppService
    {
        private readonly IRepository<Especialidade, long> _especialidadeRepository;
        private readonly IListarEspecialidadesExcelExporter _listarEspecialidadesExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        private readonly IRepository<MedicoEspecialidade, long> _medicoEspecialidadeRepository;

        public EspecialidadeAppService(IRepository<Especialidade, long> especialidadeRepository,
                                       IListarEspecialidadesExcelExporter listarEspecialidadesExcelExporter,
                                       IUnitOfWorkManager unitOfWorkManager,
                                       IUltimoIdAppService ultimoServicoAppService,
                                       IRepository<MedicoEspecialidade, long> medicoEspecialidadeRepository
                                       )
        {
            _especialidadeRepository = especialidadeRepository;
            _listarEspecialidadesExcelExporter = listarEspecialidadesExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
            _ultimoIdAppService = ultimoServicoAppService;
            _medicoEspecialidadeRepository = medicoEspecialidadeRepository;
        }

        [UnitOfWork]
        //public async Task CriarOuEditar(CriarOuEditarEspecialidade input)
        public async Task CriarOuEditar(EspecialidadeDto input)
        {
            try
            {
                var especialidade = input.MapTo<Especialidade>();

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    if (input.Id.Equals(0))
                    {
                        especialidade.Codigo = _ultimoIdAppService.ObterProximoCodigo("Especialidade").Result;
                        await _especialidadeRepository.InsertAsync(especialidade);
                    }
                    else
                    {
                        var _especialidade = await _especialidadeRepository.GetAsync(input.Id);

                        _especialidade.Cbo = input.Cbo;
                        _especialidade.CboId = input.CboId;
                        _especialidade.CboSus = input.CboSus;
                        _especialidade.Codigo = input.Codigo;
                        _especialidade.Descricao = input.Descricao;
                        _especialidade.IsAtivo = input.IsAtivo;
                        _especialidade.IsSistema = input.IsSistema;
                        _especialidade.Nome = input.Nome;

                        await _especialidadeRepository.UpdateAsync(_especialidade);
                    }

                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Excluir(EspecialidadeDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _especialidadeRepository.DeleteAsync(input.Id);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<EspecialidadeDto>> ListarTodos()
        {
            try
            {
                var query = await _especialidadeRepository
                    .GetAllListAsync();

                var especialidadesDto = query.MapTo<List<EspecialidadeDto>>();

                return new ListResultDto<EspecialidadeDto> { Items = especialidadesDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<EspecialidadeDto>> Listar(List<long> ids)
        //public async Task<ListResultDto<Especialidade>> Listar(List<long> ids)
        {
            try
            {
                var myIds = ids.ToArray();
                var query = await _especialidadeRepository
                    //.GetAllListAsync()m => m.Id.IsIn(myIds));
                    .GetAll()
                    .Include(m => m.MedicoEspecialidades)
                    .Where(m => myIds.Contains(m.Id))
                    .ToListAsync();

                var especialidadesDto = query.MapTo<List<EspecialidadeDto>>();
                //var especialidadesDto = query;

                return new ListResultDto<EspecialidadeDto> { Items = especialidadesDto };
                //return new ListResultDto<Especialidade> { Items = especialidadesDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<EspecialidadeDto>> ListarEspecialidades(ListarEspecialidadesInput input)
        {
            var contarEspecialidades = 0;
            List<Especialidade> especialidades;
            List<EspecialidadeDto> especialidadesDtos = new List<EspecialidadeDto>();
            try
            {
                var query = _especialidadeRepository
                    .GetAll()
                    .Include(m => m.SisCbo)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Cbo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.CboSus.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarEspecialidades = await query
                    .CountAsync();

                especialidades = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                especialidadesDtos = especialidades
                    .MapTo<List<EspecialidadeDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<EspecialidadeDto>(
                contarEspecialidades,
                especialidadesDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarEspecialidadesInput input)
        {
            try
            {
                var result = await ListarEspecialidades(input);
                var especialidades = result.Items;
                return _listarEspecialidadesExcelExporter.ExportToFile(especialidades.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ICollection<EspecialidadeDto>> ListarPorMedico(long id)
        {
            List<Especialidade> especialidades;
            List<EspecialidadeDto> especialidadesDtos = new List<EspecialidadeDto>();
            try
            {
                var query = from m in _especialidadeRepository.GetAll()
                            from e in m.MedicoEspecialidades
                            where e.MedicoId == id
                            select m;

                especialidades = await query
                    .AsNoTracking()
                    .ToListAsync();

                especialidadesDtos = especialidades
                    .MapTo<List<EspecialidadeDto>>();

                return especialidadesDtos;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            //return especialidadesDtos.MapTo<ListResultDto<EspecialidadeDto>>();
        }

        //public async Task<CriarOuEditarEspecialidade> Obter(long id)
        public async Task<EspecialidadeDto> Obter(long id)
        {
            try
            {
                //var result = await _especialidadeRepository
                //    .GetAsync(id);

                var result = await _especialidadeRepository
                    .GetAll()
                    .Include(m => m.SisCbo)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var especialidade = result
                    //.FirstOrDefault()
                    //.MapTo<CriarOuEditarEspecialidade>();
                    .MapTo<EspecialidadeDto>();

                return especialidade;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _especialidadeRepository.GetAll()
                        //.Where(m => m.Descricao != null && m.Descricao.Trim() != string.Empty)
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdownPorMedicoTodas(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            // List<MedicoEspecialidadeDto> especialidadesDto = new List<MedicoEspecialidadeDto>();
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
                                id = p.EspecialidadeId,
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
    }
}
