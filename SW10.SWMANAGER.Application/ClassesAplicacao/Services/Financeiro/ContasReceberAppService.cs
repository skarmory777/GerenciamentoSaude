using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using SW10.SWMANAGER.Dto;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class ContasReceberAppService : DocumentoAppService, IContasReceberAppService
    {
        public override Task<ListResultDto<LancamentoIndex>> ListarLancamento(ListarDocumentoInput input)
        {
            input.IsCredito = true;
            return base.ListarLancamento(input);
        }

        public override DefaultReturn<DocumentoDto> CriarOuEditar(DocumentoDto input)
        {
            input.IsCredito = true;
            return base.CriarOuEditar(input);
        }

        public async Task<long?> ObterConvenioId(long pessoaId)
        {
            var sisConvenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>();
            var convenio = await sisConvenioRepository.Object
                             .GetAll()
                             .Include(x => x.SisPessoa)
                             .SingleOrDefaultAsync(x => x.SisPessoaId == pessoaId)
                             .ConfigureAwait(false);

            if (convenio != null)
                return convenio.Id;


            return null;
        }

        public async Task<DefaultReturn<DocumentoDto>> Excluir(long id)
        {
            var retornoPadrao = new DefaultReturn<DocumentoDto>();

            try
            {
                using (var lancamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Lancamento, long>>())                
                {
                    var lancamento = await lancamentoRepository.Object.GetAll().AsNoTracking()
                        .Include(i => i.LancamentosQuitacoes).SingleOrDefaultAsync(x => x.Id == id);                    

                    if (lancamento.LancamentosQuitacoes.Any())
                    {
                        retornoPadrao.Errors.Add(ErroDto.Criar("", "Não é possível excluir conta com quitações."));
                        return retornoPadrao;
                    }

                    await lancamentoRepository.Object.DeleteAsync(lancamento.Id);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return retornoPadrao;
        }

    }
}
