using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiros.TalaoCheque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Transactions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.TalaoCheques
{
    public class TalaoChequeAppService : SWMANAGERAppServiceBase, ITalaoChequeAppService
    {
        private readonly IRepository<TalaoCheque, long> _talaoChequeRepository;
        private readonly IRepository<Cheque, long> _chequeRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TalaoChequeAppService(
            IRepository<TalaoCheque, long> talaoChequeRepository,
            IRepository<Cheque, long> chequeRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _talaoChequeRepository = talaoChequeRepository;
            _chequeRepository = chequeRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<DefaultReturn<TalaoChequeDto>> CriarOuEditar(TalaoChequeDto input)
        {
            var _retornoPadrao = new DefaultReturn<TalaoChequeDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                ////////////////////////////////////////////////////////////////////////////////////////////
                var isValidNum = ValidarNumerosContas(input);
                if (isValidNum)
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar("00001", "Intervalo de números já consta no banco "));
                    return _retornoPadrao;
                }
                ////////////////////////////////////////////////////////////////////////////////////////////       

                if (input.Id.Equals(0))
                {
                    var retorno = TalaoECheque(input, true);
                    _retornoPadrao.ReturnObject = input;
                }
                else
                {
                    var retornoValido = await TalaoECheque(input, false);
                    if (retornoValido != "")
                    {
                        _retornoPadrao.Errors.Add(ErroDto.Criar("00002", retornoValido));
                        return _retornoPadrao;
                    }
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

        public async Task<DefaultReturn<TalaoChequeDto>> Excluir(TalaoChequeDto input)
        {
            var _retornoPadrao = new DefaultReturn<TalaoChequeDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var strChequeUsado = ValidarExclusao(input);
                if (strChequeUsado != "")
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////
                 
                    _retornoPadrao.Errors.Add(ErroDto.Criar("00002", "Talão de cheques não pode ser excluído, porque já possui cheque(s) usado(s) número(s) do(s) cheque(s):" + strChequeUsado));
                    return _retornoPadrao;
                    ////////////////////////////////////////////////////////////////////////////////////////////
                }

                var get = _talaoChequeRepository.GetAll().FirstOrDefault(t => t.Id == input.Id);
                await _talaoChequeRepository.DeleteAsync(get);
                _retornoPadrao.ReturnObject = TalaoChequeDto.Mapear(get);

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

        public async Task<PagedResultDto<TalaoChequeDto>> Listar(TalaoChequeInput input)
        {
            var contarTalaoCheque = 0;
            List<TalaoCheque> TalaoCheque;
            List<TalaoChequeDto> TalaoChequeDtos = new List<TalaoChequeDto>();
            try
            {
                var query = _talaoChequeRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTalaoCheque = await query
                    .CountAsync();

                TalaoCheque = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                TalaoChequeDtos = TalaoCheque
                    .MapTo<List<TalaoChequeDto>>();

                return new PagedResultDto<TalaoChequeDto>(
                    contarTalaoCheque,
                    TalaoChequeDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<TalaoChequeDto> Obter(long id)
        {
            try
            {
                var query = _talaoChequeRepository
                    .GetAll()
                    .Include(c => c.ContaCorrente)
                    .Where(m => m.Id == id)
                    .FirstOrDefault();

                return TalaoChequeDto.Mapear(query);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        private bool ValidarNumerosContas(TalaoChequeDto input)
        {
            try
            {

                var LstTalaoCheques = _talaoChequeRepository.GetAll().Where(w => w.ContaCorrenteId == input.ContaCorrenteId).ToList();
                bool isInsertEdit = false;

                if (input.Id.Equals(0))
                {
                    isInsertEdit = true;
                }
                else
                {
                    if (LstTalaoCheques.Count() > 0)
                    {
                        isInsertEdit = true;
                    }
                }
                if (isInsertEdit)
                    if (LstTalaoCheques != null)
                    {
                        foreach (var item in LstTalaoCheques)
                        {
                            if (input.Id != item.Id)
                                if (input.NumeroInicial <= item.NumeroInicial && (input.NumeroFinal >= item.NumeroInicial && (input.NumeroFinal >= item.NumeroFinal || input.NumeroFinal <= item.NumeroFinal)))
                                {
                                    return true;
                                }
                                else if ((input.NumeroInicial >= item.NumeroInicial && input.NumeroInicial <= item.NumeroFinal) && (input.NumeroFinal <= item.NumeroFinal || input.NumeroFinal >= item.NumeroFinal))
                                {
                                    return true;
                                }
                        }
                    }
                return false;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private string ValidarExclusao(TalaoChequeDto input)
        {
            string strCheque = "";
            var LstCheques = _chequeRepository.GetAll().Where(w => w.TalaoChequeId.Equals(input.Id)).ToList();
            foreach (var item in LstCheques)
            {
                if (item.Data != null)
                    strCheque = strCheque + ", " + item.Numero.ToString();
            }
            if (strCheque.Length > 0)
                return strCheque.Substring(1, strCheque.Length - 1);
            else
                return "";
        }

        private async Task<String> TalaoECheque(TalaoChequeDto input, bool isNew)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin(TransactionScopeOption.Required))
            {

                string str = "";

                if (isNew)
                {
                    var talaoCheque = TalaoChequeDto.Mapear(input);
                    await _talaoChequeRepository.InsertOrUpdateAsync(talaoCheque);

                    for (int i = input.NumeroInicial; i <= input.NumeroFinal; i++)
                    {
                        var chequeEntiti = new Cheque();
                        chequeEntiti.TalaoChequeId = talaoCheque.Id;
                        chequeEntiti.Numero = i;
                        await _chequeRepository.InsertOrUpdateAsync(chequeEntiti);
                    }
                }
                else
                {
                    var get = _talaoChequeRepository.GetAll().Where(t => t.Id == input.Id).FirstOrDefault();
                    get.Codigo = input.Codigo;
                    get.Descricao = input.Descricao;
                    get.ContaCorrenteId = input.ContaCorrenteId;
                    get.DataAbertura = input.DataAbertura;
                    //get.NumeroInicial = input.NumeroInicial;
                    //get.NumeroFinal = input.NumeroFinal;

                    var lstNumerosChequesBD = new List<long>();
                    var lstNumerosChequesUsadosBD = new List<long>();
                    var LstCheques = _chequeRepository.GetAll().Where(w => w.TalaoChequeId.Equals(input.Id)).ToList();

                    foreach (var item in LstCheques)
                    {
                        if (item.Data != null)
                            lstNumerosChequesUsadosBD.Add(item.Numero);
                        lstNumerosChequesBD.Add(item.Numero);
                    }

                    lstNumerosChequesBD.Sort();
                    lstNumerosChequesUsadosBD.Sort();

                    bool validRangeCheq = true;

                    if (get.NumeroInicial == input.NumeroInicial && get.NumeroFinal == input.NumeroFinal)
                    {
                        validRangeCheq = false;
                    }

                    if (input.NumeroInicial == 0)
                    {
                        validRangeCheq = false;
                        str = "Número inicial deve ser maior que Zero!";
                    }

                    if (!(input.NumeroInicial <= lstNumerosChequesUsadosBD[0]))
                    {
                        validRangeCheq = false;
                        str = "Os números iniciais não podem ser alterdados, porquê o cheque de nº " + lstNumerosChequesUsadosBD[0] + " já foi utilizado!";
                    }

                    if (!(input.NumeroFinal >= lstNumerosChequesUsadosBD[lstNumerosChequesUsadosBD.Count - 1]))
                    {
                        validRangeCheq = false;
                        if (str != "")
                            str = "Os números iniciais e finais não podem ser alterdados, porquê os cheques de nº " + lstNumerosChequesUsadosBD[0] + " e  " + lstNumerosChequesUsadosBD[lstNumerosChequesUsadosBD.Count - 1] + " já foram utilizados!";
                        else
                            str = "Os números finais não podem ser alterdados, porquê o cheque de nº " + lstNumerosChequesUsadosBD[lstNumerosChequesUsadosBD.Count - 1] + " já foi utilizado!";
                    }

                    if (validRangeCheq)
                        if (lstNumerosChequesUsadosBD.Count > 0)
                        {

                            if (get.NumeroInicial != input.NumeroInicial)
                            {
                                get.NumeroInicial = input.NumeroInicial;
                                lstNumerosChequesUsadosBD.Sort();

                                if (input.NumeroInicial <= lstNumerosChequesUsadosBD[0])
                                {
                                    //posso alterar
                                    long inicial = input.NumeroInicial;
                                    long final = lstNumerosChequesBD[0];
                                    if (input.NumeroInicial > lstNumerosChequesBD[0])
                                    {
                                        inicial = lstNumerosChequesBD[0];
                                        final = input.NumeroInicial;

                                        for (var i = inicial; i < final; i++)
                                        {
                                            foreach (var item in LstCheques)
                                            {
                                                if (item.Numero == i)
                                                    await _chequeRepository.DeleteAsync(item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (var i = inicial; i < final; i++)
                                        {
                                            var chequeEntiti = new Cheque();
                                            chequeEntiti.TalaoChequeId = input.Id;
                                            chequeEntiti.Numero = i;
                                            await _chequeRepository.InsertOrUpdateAsync(chequeEntiti);
                                        }
                                    }
                                }
                                else if (input.NumeroInicial > lstNumerosChequesUsadosBD[0])
                                {
                                    //não pode alterar                            
                                }

                            }

                            if (input.NumeroFinal >= lstNumerosChequesUsadosBD[lstNumerosChequesUsadosBD.Count - 1])
                            {
                                if (get.NumeroFinal != input.NumeroFinal)
                                {
                                    get.NumeroFinal = input.NumeroFinal;

                                    //posso alterar
                                    long inicial = input.NumeroFinal;
                                    long final = lstNumerosChequesBD[lstNumerosChequesBD.Count - 1];

                                    if (inicial > final)
                                    {
                                        inicial = lstNumerosChequesBD[lstNumerosChequesBD.Count - 1];
                                        final = input.NumeroFinal;

                                        for (var i = inicial + 1; i <= final; i++)
                                        {
                                            var chequeEntiti = new Cheque();
                                            chequeEntiti.TalaoChequeId = input.Id;
                                            chequeEntiti.Numero = i;
                                            await _chequeRepository.InsertOrUpdateAsync(chequeEntiti);
                                        }

                                    }
                                    else
                                    {
                                        for (var i = final; i > inicial; i--)
                                        {
                                            foreach (var item in LstCheques)
                                            {
                                                if (item.Numero == i)
                                                    await _chequeRepository.DeleteAsync(item);
                                            }
                                        }
                                    }

                                }

                            }
                            else if (input.NumeroFinal < lstNumerosChequesUsadosBD[lstNumerosChequesUsadosBD.Count - 1])
                            {
                                //não pode diminuir                               
                            }

                        }
                        else
                        {

                            get.NumeroInicial = input.NumeroInicial;
                            get.NumeroFinal = input.NumeroFinal;

                            if (get.NumeroInicial != input.NumeroInicial || get.NumeroFinal != input.NumeroFinal)
                            {
                                foreach (var item in LstCheques)
                                {
                                    await _chequeRepository.DeleteAsync(item);
                                }

                                for (int i = input.NumeroInicial; i <= input.NumeroFinal; i++)
                                {
                                    var chequeEntiti = new Cheque();
                                    chequeEntiti.TalaoChequeId = input.Id;
                                    chequeEntiti.Numero = i;
                                    await _chequeRepository.InsertOrUpdateAsync(chequeEntiti);
                                }
                            }

                        }

                }

                unitOfWork.Complete();
                _unitOfWorkManager.Current.SaveChanges();
                unitOfWork.Dispose();
                return str;
            }

        }

    }
}
