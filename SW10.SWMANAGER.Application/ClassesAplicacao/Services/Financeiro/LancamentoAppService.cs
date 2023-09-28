using Abp.AutoMapper;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class LancamentoAppService : SWMANAGERAppServiceBase, ILancamentoAppService
    {
        private readonly IRepository<Lancamento, long> _lancamentoRepository;
        private readonly IRepository<LancamentoQuitacao, long> _lancamentoQuitacaoRepository;

        public LancamentoAppService(IRepository<Lancamento, long> lancamentoRepository
            , IRepository<LancamentoQuitacao, long> lancamentoQuitacaoRepository)
        {
            _lancamentoRepository = lancamentoRepository;
            _lancamentoQuitacaoRepository = lancamentoQuitacaoRepository;
        }

        public List<LancamentoDto> ObterLancamentos(List<long> Ids)
        {


            List<LancamentoDto> lancamentos = new List<LancamentoDto>();

            try
            {

                var query = _lancamentoRepository.GetAll()
                                                 .Where(w => Ids.Any(a => a == w.Id))
                                                 .Include(i => i.Documento)
                                                 .Include(i => i.Documento.Pessoa)
                                                 .ToList();

                foreach (var item in query)
                {
                    var itemDto = item.MapTo<LancamentoDto>();

                    var quitacoes = _lancamentoQuitacaoRepository.GetAll()
                                                                 .Where(w => w.LancamentoId == item.Id)
                                                                 .ToList();

                    if (quitacoes.Count > 0)
                    {
                        var totalQuitacao = quitacoes.Sum(s => s.ValorEfetivo);
                        itemDto.ValorRestante = itemDto.ValorLancamento - totalQuitacao;
                    }
                    else
                    {
                        itemDto.ValorRestante = itemDto.ValorLancamento;
                    }

                    lancamentos.Add(itemDto);
                }
            }
            catch (Exception)
            {

            }

            return lancamentos;
        }
    }
}
