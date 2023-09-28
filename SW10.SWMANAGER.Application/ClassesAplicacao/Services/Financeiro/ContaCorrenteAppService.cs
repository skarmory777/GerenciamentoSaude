using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.ContaTesouraria;
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

    public class ContaCorrenteAppService : SWMANAGERAppServiceBase, IContaCorrenteAppService
    {
        private readonly IRepository<ContaCorrente, long> _contaCorrenteRepository;
        private readonly IRepository<Banco, long> _bancoAgenciasRepositorio;

        public ContaCorrenteAppService(IRepository<ContaCorrente, long> contaCorrenteRepository, IRepository<Banco, long> bancoAgenciasRepositorio)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _bancoAgenciasRepositorio = bancoAgenciasRepositorio;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._contaCorrenteRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarPorEmpresaDropdown(DropdownInput dropdownInput)
        {
            long empresaId;
            long.TryParse(dropdownInput.filtro, out empresaId);

            return await this.ListarDropdownLambda(dropdownInput
                       , this._contaCorrenteRepository
                       , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                          || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                              && (((ContaCorrente)m).EmpresaId == empresaId)

                       , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                       , o => o.Descricao
                   ).ConfigureAwait(false);
        }

        public async Task<DefaultReturn<ContaCorrenteDto>> CriarOuEditar(ContaCorrenteDto input)
        {

            var _retornoPadrao = new DefaultReturn<ContaCorrenteDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var contaCorrente = ContaCorrenteDto.Mapear(input, true);
                if (input.Id.Equals(0))
                {
                    await this._contaCorrenteRepository.InsertOrUpdateAsync(contaCorrente).ConfigureAwait(false);
                    _retornoPadrao.ReturnObject = input;
                }
                else
                {
                    var get = _contaCorrenteRepository.GetAll().FirstOrDefault(t => t.Id == input.Id);
                    get.Codigo = input.Codigo;
                    get.Descricao = input.Descricao;
                    get.DataAbertura = input.DataAbertura;
                    get.NomeGerente = input.NomeGerente;
                    get.LimiteCredito = input.LimiteCredito;
                    get.EmpresaId = input.EmpresaId;
                    get.AgenciaId = input.AgenciaId;
                    get.TipoContaCorrenteId = input.TipoContaCorrenteId;
                    get.IsContaNaoOperacional = input.IsContaNaoOperacional;
                    get.Observacao = input.Observacao;

                    await this._contaCorrenteRepository.UpdateAsync(get).ConfigureAwait(false);
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

        public async Task<DefaultReturn<ContaCorrenteDto>> Excluir(ContaCorrenteDto input)
        {
            var _retornoPadrao = new DefaultReturn<ContaCorrenteDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var get = _contaCorrenteRepository.GetAll().FirstOrDefault(t => t.Id == input.Id);
                await this._contaCorrenteRepository.DeleteAsync(get).ConfigureAwait(false);
                _retornoPadrao.ReturnObject = ContaCorrenteDto.Mapear(get);
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

        public async Task<PagedResultDto<ContaCorrenteDto>> Listar(ContaCorrenteInput input)
        {
            var contarContaCorrente = 0;
            List<ContaCorrente> ContaCorrente;
            List<ContaCorrenteDto> BancoDtos = new List<ContaCorrenteDto>();
            try
            {
                var query = _contaCorrenteRepository
                    .GetAll()
                    .Include(a => a.Agencia)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarContaCorrente = await query
                                          .CountAsync().ConfigureAwait(false);

                ContaCorrente = await query
                                    .AsNoTracking()
                                    .OrderBy(input.Sorting)
                                    .PageBy(input)
                                    .ToListAsync().ConfigureAwait(false);

                BancoDtos = ContaCorrente
                    .MapTo<List<ContaCorrenteDto>>();

                return new PagedResultDto<ContaCorrenteDto>(
                    contarContaCorrente,
                    BancoDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ContaCorrenteDto> Obter(long id)
        {
            try
            {
                var query = _contaCorrenteRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(emp => emp.Empresa)
                    .Include(ag => ag.Agencia)
                    .Include(tipo => tipo.TipoContaCorrente)
                    .FirstOrDefault();

                var banco = _bancoAgenciasRepositorio.GetAll().FirstOrDefault(m => m.Id == query.Agencia.BancoId);

                return ContaCorrenteDto.Mapear(query, banco);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
