using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiros.TipoDocumentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class TipoDocumentoAppService : SWMANAGERAppServiceBase, ITipoDocumentoAppService
    {
        private readonly IRepository<TipoDocumento, long> _tipoDocumentoRepository;

        public TipoDocumentoAppService(IRepository<TipoDocumento, long> tipoDocumentoRepository)
        {
            _tipoDocumentoRepository = tipoDocumentoRepository;
        }

        public async Task<DefaultReturn<TipoDocumentoDto>> CriarOuEditar(TipoDocumentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<TipoDocumentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var contaCorrente = TipoDocumentoDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    await this._tipoDocumentoRepository.InsertOrUpdateAsync(contaCorrente).ConfigureAwait(false);
                    _retornoPadrao.ReturnObject = input;
                }
                else
                {
                    var get = _tipoDocumentoRepository.GetAll().FirstOrDefault(t => t.Id == input.Id);
                    get.Codigo = input.Codigo;
                    get.Descricao = input.Descricao;

                    await this._tipoDocumentoRepository.UpdateAsync(get).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return _retornoPadrao;
        }

        public async Task<DefaultReturn<TipoDocumentoDto>> Excluir(TipoDocumentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<TipoDocumentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var get = _tipoDocumentoRepository.GetAll().Where(t => t.Id == input.Id).FirstOrDefault();
                await this._tipoDocumentoRepository.DeleteAsync(get).ConfigureAwait(false);
                _retornoPadrao.ReturnObject = TipoDocumentoDto.Mapear(get);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return _retornoPadrao;
        }

        public async Task<PagedResultDto<TipoDocumentoDto>> Listar(TipoDocumentoInput input)
        {

            var contarTipoDocumento = 0;
            List<TipoDocumento> TipoLocalChamada;
            List<TipoDocumentoDto> TipoDocumentoDtos = new List<TipoDocumentoDto>();
            try
            {
                var query = _tipoDocumentoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTipoDocumento = await query
                                          .CountAsync().ConfigureAwait(false);

                TipoLocalChamada = await query
                                       .AsNoTracking()
                                       .OrderBy(input.Sorting)
                                       .PageBy(input)
                                       .ToListAsync().ConfigureAwait(false);

                TipoDocumentoDtos = TipoLocalChamada
                    .MapTo<List<TipoDocumentoDto>>();

                return new PagedResultDto<TipoDocumentoDto>(
                    contarTipoDocumento,
                    TipoDocumentoDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            const string whereStatement = "FinTipoDocumento.IsSistema = 0";
            return await this.CreateSelect2(this._tipoDocumentoRepository).AddWhereClause(whereStatement).ExecuteAsync(dropdownInput).ConfigureAwait(false);

        }

        public async Task<TipoDocumentoDto> Obter(long id)
        {
            try
            {
                var query = _tipoDocumentoRepository
                    .GetAll()
                    .FirstOrDefault(m => m.Id == id);

                return TipoDocumentoDto.Mapear(query);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<TipoDocumentoDto> ObterPelaDescricao(string descricao)
        {
            try
            {
                var query = await _tipoDocumentoRepository
                    .GetAll()
                    .FirstOrDefaultAsync(x => x.Descricao.Equals(descricao));

                return TipoDocumentoDto.Mapear(query);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
