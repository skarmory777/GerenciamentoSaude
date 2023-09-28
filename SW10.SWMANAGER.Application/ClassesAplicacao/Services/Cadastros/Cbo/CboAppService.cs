using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cbos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cbos
{
    public class CboAppService : SWMANAGERAppServiceBase, ICboAppService
    {
        private readonly IRepository<Cbo, long> _cboRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CboAppService(IRepository<Cbo, long> cboRepository
            , IUnitOfWorkManager unitOfWorkManager)
        {
            _cboRepository = cboRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task CriarOuEditar(CboDto input)
        {
            try
            {
                var cbo = input.MapTo<Cbo>();

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    if (input.Id.Equals(0))
                    {
                        await _cboRepository.InsertAsync(cbo);
                    }
                    else
                    {
                        await _cboRepository.UpdateAsync(cbo);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(CboDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    await _cboRepository.DeleteAsync(input.Id);

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<CboDto>> Listar(ListarCbosInput input)
        {
            var contarCbos = 0;
            List<Cbo> cbos = new List<Cbo>();
            List<CboDto> cbosDtos = new List<CboDto>();
            try
            {
                var query = _cboRepository
                    .GetAll();
                //.WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                //    m.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
                //    m.Uf.ToUpper().Contains(input.Filtro.ToUpper())
                //);

                contarCbos = await query
                    .CountAsync();

                cbos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                cbosDtos = cbos.MapTo<List<CboDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<CboDto>(
                contarCbos,
                cbosDtos
                );
        }

        //public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? paisId)
        //{
        //    try
        //    {
        //        var query = await _cboRepository
        //            .GetAll()
        //            .Include(m => m.Pais)
        //            .WhereIf(!input.IsNullOrEmpty(), m =>
        //               m.Nome.ToUpper().Contains(input.ToUpper()) ||
        //               m.Uf.ToUpper().Contains(input.ToUpper()))
        //            .WhereIf(paisId.HasValue, m =>
        //                m.PaisId == paisId)
        //            .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Nome })
        //            .ToListAsync();

        //        return new ListResultDto<GenericoIdNome> { Items = query };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        //public async Task<FileDto> ListarParaExcel(ListarCbosInput input)
        //{
        //    try
        //    {
        //        var result = await Listar(input);
        //        var cbos = result.Items;
        //        return _listarCbosExcelExporter.ExportToFile(cbos.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }

        //}

        public async Task<CboDto> Obter(long id)
        {
            var query = await _cboRepository
                .GetAll()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            var cbo = query.MapTo<CboDto>();

            return cbo;
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<CboDto> pacientesDtos = new List<CboDto>();
            try
            {
                //se um país estiver selecionado, filtra apenas pelos seus cbos
                long paisId;
                long.TryParse(dropdownInput.filtro, out paisId);

                //get com filtro
                var query = from p in _cboRepository.GetAll()
                        //.WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.PaisId == paisId)

                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) || m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())

                        )
                            orderby p.Codigo, p.Descricao ascending
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

    }
}
