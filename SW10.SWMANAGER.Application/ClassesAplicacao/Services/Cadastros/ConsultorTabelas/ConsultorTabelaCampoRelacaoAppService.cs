using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas
{
    public class ConsultorTabelaCampoRelacaoAppService : SWMANAGERAppServiceBase, IConsultorTabelaCampoRelacaoAppService
    {
        private readonly IRepository<ConsultorTabelaCampoRelacao, long> _consultorTabelaCampoRelacaoRepository;
        private readonly IRepository<ConsultorTabelaCampo, long> _consultorTabelaCampoRepository;

        public ConsultorTabelaCampoRelacaoAppService(
            IRepository<ConsultorTabelaCampoRelacao, long> consultorTabelaCampoRelacaoRepository,
            IRepository<ConsultorTabelaCampo, long> consultorTabelaCampoRepository
            )
        {
            _consultorTabelaCampoRelacaoRepository = consultorTabelaCampoRelacaoRepository;
            _consultorTabelaCampoRepository = consultorTabelaCampoRepository;
        }

        public async Task CriarOuEditar(ConsultorTabelaCampoRelacaoDto input)
        {
            try
            {
                var consultorTabelaCampoRelacao = new ConsultorTabelaCampoRelacao();
                consultorTabelaCampoRelacao = input.MapTo<ConsultorTabelaCampoRelacao>();
                if (input.Id.Equals(0))
                {
                    await _consultorTabelaCampoRelacaoRepository.InsertAsync(consultorTabelaCampoRelacao);
                }
                else
                {

                    await _consultorTabelaCampoRelacaoRepository.UpdateAsync(consultorTabelaCampoRelacao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ConsultorTabelaCampoRelacaoDto input)
        {
            try
            {
                await _consultorTabelaCampoRelacaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ConsultorTabelaCampoRelacaoDto> Obter(long id)
        {
            try
            {
                var result = await _consultorTabelaCampoRelacaoRepository
                    .GetAll()
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTabelaCampo)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var consultorTabelaCampoRelacao = result
                    //.FirstOrDefault()
                    .MapTo<ConsultorTabelaCampoRelacaoDto>();

                return consultorTabelaCampoRelacao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarCombo(long id)
        {
            var contarConsultorTabelaCampos = 0;
            //    List<ConsultorTabelaCampo> consultorTabelaCampos;
            List<ConsultorTabelaCampoDto> consultorTabelaCamposDtos = new List<ConsultorTabelaCampoDto>();

            try
            {
                var queryPertencente = _consultorTabelaCampoRelacaoRepository
                    .GetAll()
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTabelaCampo)
                    .Where(d => d.ConsultorTabelaId == id)
                    .Select(m => m.ConsultorTabelaCampo);

                var queryDosOutros = _consultorTabelaCampoRepository
                    .GetAll();

                var pertencente = await queryPertencente
                    .AsNoTracking()
                    .ToListAsync();

                var dosOutros = await queryDosOutros
                    .AsNoTracking()
                    .ToListAsync();

                var disponiveis = dosOutros.Except(pertencente);

                //consultorTabelaCampos = await query
                //    .AsNoTracking()
                //    .ToListAsync();

                //consultorTabelaCamposDtos = consultorTabelaCampos
                //    .MapTo<List<ConsultorTabelaCampoDto>>();

                consultorTabelaCamposDtos = disponiveis
               .MapTo<List<ConsultorTabelaCampoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ConsultorTabelaCampoDto>(
                contarConsultorTabelaCampos,
                consultorTabelaCamposDtos
                );
        }

        public async Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarTabela(long id)
        {
            var contarConsultorTabelaCampos = 0;
            List<ConsultorTabelaCampoDto> consultorTabelaCamposDtos = new List<ConsultorTabelaCampoDto>();

            try
            {
                var queryPertencente = _consultorTabelaCampoRelacaoRepository
                    .GetAll()
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTabelaCampo)
                    .Where(d => d.ConsultorTabelaId == id)
                    .Select(m => m.ConsultorTabelaCampo);

                var pertencente = await queryPertencente
                    //  .AsNoTracking()
                    .ToListAsync();


                consultorTabelaCamposDtos = pertencente
               .MapTo<List<ConsultorTabelaCampoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ConsultorTabelaCampoDto>(
                contarConsultorTabelaCampos,
                consultorTabelaCamposDtos
                );
        }
    }
}
