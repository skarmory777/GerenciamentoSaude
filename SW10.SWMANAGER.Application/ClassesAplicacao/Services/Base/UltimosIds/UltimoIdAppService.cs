using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds
{
    public class UltimoIdAppService : SWMANAGERAppServiceBase, IUltimoIdAppService
    {
        private readonly IRepository<UltimoId, long> _ultimoIdRepository;
        // private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UltimoIdAppService(
            IRepository<UltimoId, long> ultimoIdRepository
            //IUnitOfWorkManager unitOfWorkManager
            )
        {
            _ultimoIdRepository = ultimoIdRepository;
            // _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task CriarOuEditar(UltimoIdDto input)
        {
            try
            {
                var ultimoId = input.MapTo<UltimoId>();
                if (input.Id.Equals(0))
                {
                    using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                    {
                        await _ultimoIdRepository.InsertAsync(ultimoId);
                        unitOfWork.Complete();
                        // _unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
                else
                {
                    var _ultimoId = _ultimoIdRepository.FirstOrDefault(input.Id);
                    _ultimoId.Codigo = input.Codigo;
                    using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                    {
                        await _ultimoIdRepository.UpdateAsync(_ultimoId);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        //public async Task Excluir(UltimoIdDto input)
        //{
        //    try
        //    {
        //        await _ultimoIdRepository.DeleteAsync(input.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExcluir"), ex);
        //    }

        //}

        public async Task<ListResultDto<UltimoIdDto>> ListarTodos()
        {
            List<UltimoId> ultimosIds;
            List<UltimoIdDto> ultimosIdsDtos = new List<UltimoIdDto>();
            try
            {
                ultimosIds = await _ultimoIdRepository
                    .GetAll()
                    .ToListAsync();

                ultimosIdsDtos = ultimosIds
                    .MapTo<List<UltimoIdDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<UltimoIdDto> { Items = ultimosIdsDtos };
        }

        [UnitOfWork]
        public async Task<UltimoIdDto> Obter(long id)
        {
            try
            {
                var result = await _ultimoIdRepository.GetAsync(id);
                var ultimoId = result.MapTo<UltimoIdDto>();
                return ultimoId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<string> ObterProximoCodigo(string nomeTabela)
        {
            using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = _unitOfWorkManager.Object.Begin(TransactionScopeOption.RequiresNew))
            {
                string strCodigo = string.Empty;
                var idAtual = _ultimoIdRepository.GetAll().FirstOrDefault(w => w.NomeTabela == nomeTabela);

                if (idAtual == null || idAtual.Id == 0)
                {
                    await this.CriarOuEditar(new UltimoIdDto() { NomeTabela = nomeTabela, Codigo = "0", IsSistema = true }).ConfigureAwait(false);
                    idAtual = _ultimoIdRepository.GetAll().FirstOrDefault(w => w.NomeTabela == nomeTabela);
                }

                long codigo;
                long.TryParse(idAtual.Codigo, out codigo);
                codigo++;
                strCodigo = codigo.ToString();
                idAtual.Codigo = strCodigo;
                await _ultimoIdRepository.UpdateAsync(idAtual).ConfigureAwait(false);

                if (idAtual.TamanhoCampo != null && !string.IsNullOrEmpty(idAtual.complementoEsquerda))
                {
                    strCodigo = strCodigo.PadLeft((int)idAtual.TamanhoCampo, idAtual.complementoEsquerda.ToCharArray()[0]);
                }

                unitOfWork.Complete();
                // _unitOfWorkManager.Object.Current.SaveChanges();
                unitOfWork.Dispose();
                return strCodigo;
            }
        }
    }
}
