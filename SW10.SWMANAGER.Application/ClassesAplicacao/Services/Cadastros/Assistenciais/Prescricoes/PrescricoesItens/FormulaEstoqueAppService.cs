using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    using Abp.Auditing;
    using SW10.SWMANAGER.Helpers;

    public class FormulaEstoqueAppService : SWMANAGERAppServiceBase, IFormulaEstoqueAppService
    {
        private readonly IRepository<FormulaEstoque, long> _formulaEstoqueRepositorio;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FormulaEstoqueAppService(
            IRepository<FormulaEstoque, long> formulaEstoqueRepositorio,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _formulaEstoqueRepositorio = formulaEstoqueRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task<FormulaEstoqueDto> CriarOuEditar(FormulaEstoqueDto input)
        {
            try
            {
                input.Unidade = null;
                input.Produto = null;
                input.UnidadeRequisicao = null;

                var formulaEstoque = input.MapTo<FormulaEstoque>();
                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await this._formulaEstoqueRepositorio.InsertAndGetIdAsync(formulaEstoque).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        return input;
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await this._formulaEstoqueRepositorio.UpdateAsync(formulaEstoque).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        return input;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(FormulaEstoqueDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await this._formulaEstoqueRepositorio.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormulaEstoqueDto>> Listar(ListarFormulaInput input)
        {
            var contarFormulaEstoque = 0;
            var idGrid = 0;
            List<FormulaEstoque> formulaEstoque;
            List<FormulaEstoqueDto> formulaEstoqueDtos = new List<FormulaEstoqueDto>();
            try
            {
                var query = _formulaEstoqueRepositorio
                    .GetAll()
                    .Where(m => m.PrescricaoItemId == input.PrescricaoItemId)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                contarFormulaEstoque = await query
                                           .CountAsync().ConfigureAwait(false);

                formulaEstoque = await query
                                     .AsNoTracking()
                                     .OrderBy(input.Sorting)
                                     .PageBy(input)
                                     .ToListAsync().ConfigureAwait(false);

                formulaEstoqueDtos = formulaEstoque
                    .MapTo<List<FormulaEstoqueDto>>();

                formulaEstoqueDtos.ForEach(m => m.IdGridFormulasEstoque = ++idGrid);

                return new PagedResultDto<FormulaEstoqueDto>(
                    contarFormulaEstoque,
                    formulaEstoqueDtos
                    );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormulaEstoqueDto>> ListarJson(List<FormulaEstoqueDto> list)
        {
            try
            {
                var count = 0;
                if (list == null)
                {
                    list = new List<FormulaEstoqueDto>();
                }
                //for (int i = 0; i < list.Count(); i++)
                //{
                //    list[i].IdGridFormulasEstoque = i;
                //}
                count = await Task.Run(() => list.Count()).ConfigureAwait(false);

                return new PagedResultDto<FormulaEstoqueDto>(count, list);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPresquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<FormulaEstoqueDto> Obter(long id)
        {
            try
            {
                var result = await this._formulaEstoqueRepositorio
                                 .GetAll()
                                 .Include(m => m.EstoqueOrigem)
                                 //.Include(m => m.PrescricaoItem)
                                 .Include(m => m.Produto)
                                 .Include(m => m.Unidade)
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                var formulaEstoque = result.MapTo<FormulaEstoqueDto>();

                return formulaEstoque;
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormulaEstoqueDto>> ListarTodos()
        {
            try
            {
                var query = _formulaEstoqueRepositorio
                    .GetAll()
                    .OrderBy(m => m.Codigo);

                var formulaEstoque = await query
                                         .AsNoTracking()
                                         .ToListAsync().ConfigureAwait(false);

                var formulasEstoquesDto = formulaEstoque
                    .MapTo<List<FormulaEstoqueDto>>();

                return new ListResultDto<FormulaEstoqueDto>
                {
                    Items = formulasEstoquesDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormulaEstoqueDto>> ListarPorPrescricaoItem(long id)
        {
            try
            {
                var query = _formulaEstoqueRepositorio
                    .GetAll()
                    .Include(i => i.Unidade)
                    .Include(i => i.Produto)
                    .Where(m => m.PrescricaoItemId == id)
                    .OrderBy(m => m.Codigo);

                var formulaEstoque = await query
                                         .AsNoTracking()
                                         .ToListAsync().ConfigureAwait(false);

                var formulasEstoquesDto = formulaEstoque
                    .MapTo<List<FormulaEstoqueDto>>();

                var idGrid = 0;
                formulasEstoquesDto.ForEach(m => m.IdGridFormulasEstoque = ++idGrid);
                return new ListResultDto<FormulaEstoqueDto>
                {
                    Items = formulasEstoquesDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormulaEstoqueDto>> ListarFiltro(string filtro)
        {
            try
            {
                var query = _formulaEstoqueRepositorio
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(filtro) ||
                        m.Descricao.Contains(filtro)
                        );

                var formulaEstoque = await query
                                         .AsNoTracking()
                                         .ToListAsync().ConfigureAwait(false);

                var formulasEstoquesDto = formulaEstoque
                    .MapTo<List<FormulaEstoqueDto>>();

                return new ListResultDto<FormulaEstoqueDto>
                {
                    Items = formulasEstoquesDto
                };
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
            return await this.CreateSelect2(this._formulaEstoqueRepositorio).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}
