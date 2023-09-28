using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class QuitacaoContasReceberAppService : QuitacaoAppService, IQuitacaoContasReceberAppService
    {

        private readonly IRepository<Quitacao, long> _quitacaoRepository;
        private readonly IRepository<LancamentoQuitacao, long> _lancamentoQuitacaoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Lancamento, long> _lancamentoRepository;
        private readonly IRepository<Cheque, long> _chequeRepository;
        private readonly IRepository<MeioPagamento, long> _meioPagamentoRepository;


        public QuitacaoContasReceberAppService(IRepository<Quitacao, long> quitacaoRepository
                                          , IRepository<LancamentoQuitacao, long> lancamentoQuitacaoRepository
            , IUnitOfWorkManager unitOfWorkManager
            , IRepository<Lancamento, long> lancamentoRepository
            , IRepository<Cheque, long> chequeRepository
            , IRepository<MeioPagamento, long> meioPagamentoRepository) : base(quitacaoRepository
                                                                            , lancamentoQuitacaoRepository
                                                                            , unitOfWorkManager
                                                                            , lancamentoRepository
                                                                            , chequeRepository
                                                                            , meioPagamentoRepository)
        { }

        public override DefaultReturn<QuitacaoDto> CriarOuEditar(QuitacaoDto input)
        {
            input.IsCredito = true;
            return base.CriarOuEditar(input);
        }
    }
}
