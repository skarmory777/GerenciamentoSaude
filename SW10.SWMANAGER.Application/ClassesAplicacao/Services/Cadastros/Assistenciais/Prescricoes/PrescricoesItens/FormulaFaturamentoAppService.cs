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
    public class FormulaFaturamentoAppService : SWMANAGERAppServiceBase, IFormulaFaturamentoAppService
    {
        private readonly IRepository<FormulaFaturamento, long> _formulaFaturamentoRepositorio;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FormulaFaturamentoAppService(
            IRepository<FormulaFaturamento, long> formulaFaturamentoRepositorio,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _formulaFaturamentoRepositorio = formulaFaturamentoRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task<FormulaFaturamentoDto> CriarOuEditar(FormulaFaturamentoDto input)
        {
            try
            {
                var formulaFaturamento = input.MapTo<FormulaFaturamento>();
                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await _formulaFaturamentoRepositorio.InsertAndGetIdAsync(formulaFaturamento);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        return input;
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _formulaFaturamentoRepositorio.UpdateAsync(formulaFaturamento);
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
        public async Task Excluir(FormulaFaturamentoDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _formulaFaturamentoRepositorio.DeleteAsync(input.Id);
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
        public async Task<PagedResultDto<FormulaFaturamentoDto>> Listar(ListarFormulaInput input)
        {
            var contarFormulaFaturamento = 0;
            List<FormulaFaturamento> formulaFaturamento;
            List<FormulaFaturamentoDto> FormulaFaturamentoDtos = new List<FormulaFaturamentoDto>();
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .Where(m => m.PrescricaoItemId == input.PrescricaoItemId)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                contarFormulaFaturamento = await query
                    .CountAsync();

                formulaFaturamento = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                FormulaFaturamentoDtos = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                return new PagedResultDto<FormulaFaturamentoDto>(
                    contarFormulaFaturamento,
                    FormulaFaturamentoDtos
                    );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormulaFaturamentoDto>> ListarFatItem(ListarFormulaInput input)
        {
            var contarFormulaFaturamento = 0;
            var idGrid = 0;
            List<FormulaFaturamento> formulaFaturamento;
            List<FormulaFaturamentoDto> formulaFaturamentoDtos = new List<FormulaFaturamentoDto>();
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .Include(m => m.FaturamentoItem)
                    .Where(m =>
                        m.PrescricaoItemId == input.PrescricaoItemId)// &&
                    .Where(m =>
                        !m.FaturamentoItem.IsRequisicaoExame &&
                        !m.FaturamentoItem.IsLaudo
                    )
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                contarFormulaFaturamento = await query
                    .CountAsync();

                formulaFaturamento = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                formulaFaturamentoDtos = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                formulaFaturamentoDtos.ForEach(m => m.IdGridFormulasFaturamento = ++idGrid);

                return new PagedResultDto<FormulaFaturamentoDto>(
                    contarFormulaFaturamento,
                    formulaFaturamentoDtos
                    );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormulaFaturamentoDto>> ListarExameImagem(ListarFormulaInput input)
        {
            var contarFormulaFaturamento = 0;
            var idGrid = 0;
            List<FormulaFaturamento> formulaFaturamento;
            List<FormulaFaturamentoDto> formulaFaturamentoDtos = new List<FormulaFaturamentoDto>();
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .Include(m => m.FaturamentoItem)
                    .Where(m =>
                        m.PrescricaoItemId == input.PrescricaoItemId) // &&
                    .Where(m => m.FaturamentoItem.IsLaudo)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                contarFormulaFaturamento = await query
                    .CountAsync();

                formulaFaturamento = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                formulaFaturamentoDtos = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                formulaFaturamentoDtos.ForEach(m => m.IdGridFormulasExameImagem = ++idGrid);

                return new PagedResultDto<FormulaFaturamentoDto>(
                    contarFormulaFaturamento,
                    formulaFaturamentoDtos
                    );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormulaFaturamentoDto>> ListarExameLaboratorial(ListarFormulaInput input)
        {
            var contarFormulaFaturamento = 0;
            var idGrid = 0;
            List<FormulaFaturamento> formulaFaturamento;
            List<FormulaFaturamentoDto> formulaFaturamentoDtos = new List<FormulaFaturamentoDto>();
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .Include(m => m.FaturamentoItem)
                    .Where(m =>
                        m.PrescricaoItemId == input.PrescricaoItemId) // &&
                    .Where(m => m.FaturamentoItem.IsRequisicaoExame)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                contarFormulaFaturamento = await query
                    .CountAsync();

                formulaFaturamento = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                formulaFaturamentoDtos = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                formulaFaturamentoDtos.ForEach(m => m.IdGridFormulasExameLaboratorial = ++idGrid);

                return new PagedResultDto<FormulaFaturamentoDto>(
                    contarFormulaFaturamento,
                    formulaFaturamentoDtos
                    );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormulaFaturamentoDto>> ListarFaturamentoPorPrescricaoItem(long id)
        {
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .Where(m => m.PrescricaoItemId == id
                       && (!m.PrescricaoItem.FaturamentoItem.IsLaboratorio && !m.PrescricaoItem.FaturamentoItem.Grupo.IsLaboratorio)
                       && (!m.PrescricaoItem.FaturamentoItem.IsLaudo && !m.PrescricaoItem.FaturamentoItem.Grupo.IsLaudo)
                       )
                    //&& (m.FaturamentoItem.GrupoId != 128 && m.FaturamentoItem.GrupoId != 132)) // !m.FaturamentoItem.IsRequisicaoExame && !m.FaturamentoItem.IsLaudo && !m.FaturamentoItem.IsLaboratorio)
                    .OrderBy(m => m.Codigo);

                var formulaFaturamento = await query
                    .AsNoTracking()
                    .ToListAsync();

                var formulasFaturamentosDto = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                var idGrid = 0;
                formulasFaturamentosDto.ForEach(m => m.IdGridFormulasFaturamento = ++idGrid);
                return new ListResultDto<FormulaFaturamentoDto>
                {
                    Items = formulasFaturamentosDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormulaFaturamentoDto>> ListarExameLaboratorialPorPrescricaoItem(long id)
        {
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .Where(m => m.PrescricaoItemId == id && m.FaturamentoItem.GrupoId == 128) // m.FaturamentoItem.IsRequisicaoExame && m.FaturamentoItem.IsLaboratorio)
                    .OrderBy(m => m.Codigo);

                var formulaFaturamento = await query
                    .AsNoTracking()
                    .ToListAsync();

                var formulasFaturamentosDto = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                var idGrid = 0;
                formulasFaturamentosDto.ForEach(m => m.IdGridFormulasExameLaboratorial = ++idGrid);
                return new ListResultDto<FormulaFaturamentoDto>
                {
                    Items = formulasFaturamentosDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormulaFaturamentoDto>> ListarExameImagemPorPrescricaoItem(long id)
        {
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .Where(m => m.PrescricaoItemId == id && m.FaturamentoItem.GrupoId == 132) // m.FaturamentoItem.IsRequisicaoExame && m.FaturamentoItem.IsLaudo)
                    .OrderBy(m => m.Codigo);

                var formulaFaturamento = await query
                    .AsNoTracking()
                    .ToListAsync();

                var formulasFaturamentosDto = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                var idGrid = 0;
                formulasFaturamentosDto.ForEach(m => m.IdGridFormulasExameImagem = ++idGrid);
                return new ListResultDto<FormulaFaturamentoDto>
                {
                    Items = formulasFaturamentosDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormulaFaturamentoDto>> ListarFaturamentoJson(List<FormulaFaturamentoDto> list)
        {
            try
            {
                var count = 0;
                if (list == null)
                {
                    list = new List<FormulaFaturamentoDto>();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].IdGridFormulasFaturamento = i + 1;
                }

                count = await Task.Run(() => list.Count());

                return new PagedResultDto<FormulaFaturamentoDto>(
                      count,
                      list
                      );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar", ex));
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormulaFaturamentoDto>> ListarExameLaboratorialJson(List<FormulaFaturamentoDto> list)
        {
            try
            {
                var count = 0;
                if (list == null)
                {
                    list = new List<FormulaFaturamentoDto>();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].IdGridFormulasExameLaboratorial = i + 1;
                }

                count = await Task.Run(() => list.Count());

                return new PagedResultDto<FormulaFaturamentoDto>(
                      count,
                      list
                      );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar", ex));
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormulaFaturamentoDto>> ListarExameImagemJson(List<FormulaFaturamentoDto> list)
        {
            try
            {
                var count = 0;
                if (list == null)
                {
                    list = new List<FormulaFaturamentoDto>();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].IdGridFormulasExameImagem = i + 1;
                }

                count = await Task.Run(() => list.Count());

                return new PagedResultDto<FormulaFaturamentoDto>(
                      count,
                      list
                      );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar", ex));
            }
        }

        [UnitOfWork(false)]
        public async Task<FormulaFaturamentoDto> Obter(long id)
        {
            try
            {
                var result = await _formulaFaturamentoRepositorio
                    .GetAll()
                    .Include(m => m.PrescricaoItem)
                    .Include(m => m.FaturamentoItem)
                    .Include(m => m.Material)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();
                var formulaFaturamento = result.MapTo<FormulaFaturamentoDto>();

                return formulaFaturamento;
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormulaFaturamentoDto>> ListarTodos()
        {
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .OrderBy(m => m.Codigo);

                var formulaFaturamento = await query
                    .AsNoTracking()
                    .ToListAsync();

                var tiposControlesDto = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                return new ListResultDto<FormulaFaturamentoDto>
                {
                    Items = tiposControlesDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormulaFaturamentoDto>> ListarFiltro(string filtro)
        {
            try
            {
                var query = _formulaFaturamentoRepositorio
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(filtro) ||
                        m.Descricao.Contains(filtro)
                        );

                var formulaFaturamento = await query
                    .AsNoTracking()
                    .ToListAsync();

                var tiposControlesDto = formulaFaturamento
                    .MapTo<List<FormulaFaturamentoDto>>();

                return new ListResultDto<FormulaFaturamentoDto>
                {
                    Items = tiposControlesDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarFormulaFaturamentoDropdown(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(
                dropdownInput,
                _formulaFaturamentoRepositorio,
                m => !m.FaturamentoItem.IsRequisicaoExame
                    && !m.FaturamentoItem.IsLaudo
                    && m.FaturamentoItem.IsAtivo,
                s => new DropdownItems
                {
                    id = s.Id,
                    text = string.Concat(s.Codigo, " - ", s.Descricao)
                },
                m => m.Descricao
                );
            //return await ListarCodigoDescricaoDropdown(dropdownInput, _formulaFaturamentoRepositorio);
        }

        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarFormulaExameLaboratorialDropdown(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(
                dropdownInput,
                _formulaFaturamentoRepositorio,
                m => m.FaturamentoItem.IsRequisicaoExame
                    && !m.FaturamentoItem.IsLaudo
                    && m.FaturamentoItem.IsAtivo,
                s => new DropdownItems
                {
                    id = s.Id,
                    text = string.Concat(s.Codigo, " - ", s.Descricao)
                },
                m => m.Descricao
                );
        }

        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarFormulaExameImagemDropdown(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(
                dropdownInput,
                _formulaFaturamentoRepositorio,
                m => m.FaturamentoItem.IsRequisicaoExame
                    && m.FaturamentoItem.IsLaudo
                    && m.FaturamentoItem.IsAtivo,
                s => new DropdownItems
                {
                    id = s.Id,
                    text = string.Concat(s.Codigo, " - ", s.Descricao)
                },
                m => m.Descricao
                );
        }

    }
}
