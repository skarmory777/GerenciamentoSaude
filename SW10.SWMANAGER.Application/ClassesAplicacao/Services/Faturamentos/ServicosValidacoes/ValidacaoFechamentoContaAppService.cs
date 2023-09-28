using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ServicosValidacoes
{
    public class ValidacaoFechamentoContaAppService
    {
        private readonly IRepository<Atendimento, long> _atendimentoRepository;


        public ValidacaoFechamentoContaAppService(IRepository<Atendimento, long> atendimentoRepository)
        {
            _atendimentoRepository = atendimentoRepository;
        }

        DefaultReturn<FaturamentoContaDto> _retornoPadrao = new DefaultReturn<FaturamentoContaDto>();
        public DefaultReturn<FaturamentoContaDto> Validar(FaturamentoContaDto conta)
        {
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            ValidarDataAlta(conta);

            return _retornoPadrao;
        }

        void ValidarDataAlta(FaturamentoContaDto conta)
        {
            var atendimento = _atendimentoRepository.GetAll()
                                                    .Where(w => w.Id == conta.AtendimentoId)
                                                    .FirstOrDefault();

            if (atendimento != null)
            {
                if (atendimento.DataAlta != null && ((DateTime)atendimento.DataAlta).Date < conta.DataFim)
                {
                    _retornoPadrao.Errors.Add(new ErroDto { Descricao = "Data de fechamento da conta não deve ser posterior a data de alta." });
                }
            }
        }
    }
}
