using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas
{
    public class ConsultorTabelaCampoAppService : SWMANAGERAppServiceBase, IConsultorTabelaCampoAppService
    {
        private readonly IRepository<ConsultorTabelaCampo, long> _consultorTabelaCampoRepository;
        private readonly IRepository<ConsultorTabelaCampoRelacao, long> _consultorTabelaCampoRelacaoRepository;

        public ConsultorTabelaCampoAppService(
            IRepository<ConsultorTabelaCampo, long> consultorTabelaCampoRepository,
            IRepository<ConsultorTabelaCampoRelacao, long> consultorTabelaCampoRelacaoRepository
            )
        {
            _consultorTabelaCampoRepository = consultorTabelaCampoRepository;
            _consultorTabelaCampoRelacaoRepository = consultorTabelaCampoRelacaoRepository;
        }

        public async Task CriarOuEditar(CriarOuEditarConsultorTabelaCampo input)
        {
            try
            {
                var consultorTabelaCampo = new ConsultorTabelaCampo();
                consultorTabelaCampo = input.MapTo<ConsultorTabelaCampo>();

                if (input.Id.Equals(0))
                {
                    var campoSalvo = await _consultorTabelaCampoRepository.InsertAsync(consultorTabelaCampo);

                    if (input.ConsultorTabelaId != null)
                    {
                        ConsultorTabelaCampoRelacao campoRelacao = new ConsultorTabelaCampoRelacao();
                        campoRelacao.ConsultorTabelaCampoId = campoSalvo.Id;
                        campoRelacao.ConsultorTabelaId = (long)campoSalvo.ConsultorTabelaId;
                        await _consultorTabelaCampoRelacaoRepository.InsertAsync(campoRelacao);
                    }
                }
                else
                {
                    //var campo = _consultorTabelaCampoRepository.Get(input.Id);
                    //var campo = input.MapTo<ConsultorTabelaCampo>();
                    // campo.ConsultorTabelaId = input.ConsultorTabelaId;

                    consultorTabelaCampo.ConsultorTabelaId = input.ConsultorTabelaId;
                    var campoAtualizado = await _consultorTabelaCampoRepository.UpdateAsync(consultorTabelaCampo);

                    // Se foi removida uma relacao ou ja nao existia
                    if (campoAtualizado.ConsultorTabelaId == null)
                    {
                        var listaCampoRelacao = await _consultorTabelaCampoRelacaoRepository.GetAllListAsync();
                        var campoRelacao = listaCampoRelacao.Find(c => c.ConsultorTabelaId == campoAtualizado.ConsultorTabelaId && c.ConsultorTabelaCampoId == input.Id);

                        // Se existia uma relacao, remove
                        if (campoRelacao != null)
                        {
                            await _consultorTabelaCampoRelacaoRepository.DeleteAsync(campoRelacao);
                        }
                    }
                    // Se foi inserida uma relacao ou ja existia
                    else
                    {
                        var listaCampoRelacao = await _consultorTabelaCampoRelacaoRepository.GetAllListAsync();
                        var campoRelacao = listaCampoRelacao.Find(c => c.ConsultorTabelaId == campoAtualizado.ConsultorTabelaId && c.ConsultorTabelaCampoId == input.Id);

                        // Se ja existia, atualiza
                        if (campoRelacao != null)
                        {
                            campoRelacao.ConsultorTabelaId = (long)input.ConsultorTabelaId;
                            await _consultorTabelaCampoRelacaoRepository.UpdateAsync(campoRelacao);
                        }
                        // Se nao existia, insere
                        else
                        {
                            campoRelacao = new ConsultorTabelaCampoRelacao();
                            campoRelacao.ConsultorTabelaCampoId = input.Id;
                            campoRelacao.ConsultorTabelaId = (long)input.ConsultorTabelaId;
                            await _consultorTabelaCampoRelacaoRepository.InsertAsync(campoRelacao);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarConsultorTabelaCampo input)
        {
            try
            {
                await _consultorTabelaCampoRepository.DeleteAsync(input.Id);

                var listaCampoRelacao = await _consultorTabelaCampoRelacaoRepository.GetAllListAsync();

                foreach (var cr in listaCampoRelacao.FindAll(c => c.ConsultorTabelaCampoId == input.Id))
                {
                    await _consultorTabelaCampoRelacaoRepository.DeleteAsync(cr);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task RemoverRelacaoTabelaCampo(CriarOuEditarConsultorTabelaCampo input)
        {
            try
            {
                var campo = await _consultorTabelaCampoRepository
                    .GetAll()
                    .Include(m => m.ConsultorOcorrencia)
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTipoDadoNF)
                    .Where(m => m.Id == input.Id)
                    .FirstOrDefaultAsync();
                campo.ConsultorTabelaId = null;
                await _consultorTabelaCampoRepository.UpdateAsync(campo);
                var campoRelacoes = await _consultorTabelaCampoRelacaoRepository.GetAllListAsync();
                var campoRelacao = campoRelacoes.Find(c => c.ConsultorTabelaId == input.ConsultorTabelaId && c.ConsultorTabelaCampoId == input.Id);
                await _consultorTabelaCampoRelacaoRepository.DeleteAsync(campoRelacao);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<CriarOuEditarConsultorTabelaCampo> Obter(long id)
        {
            try
            {
                var result = await _consultorTabelaCampoRepository
                    .GetAll()
                    .Include(m => m.ConsultorOcorrencia)
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTipoDadoNF)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var consultorTabelaCampo = result
                    //.FirstOrDefault()
                    .MapTo<CriarOuEditarConsultorTabelaCampo>();

                return consultorTabelaCampo;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ConsultorTabelaCampoDto>> Listar(ListarConsultorTabelaCamposInput input)
        {
            var contarConsultorTabelaCampos = 0;
            List<ConsultorTabelaCampo> consultorTabelaCampos;
            List<ConsultorTabelaCampoDto> consultorTabelaCamposDtos = new List<ConsultorTabelaCampoDto>();
            try
            {
                var query = _consultorTabelaCampoRepository
                    .GetAll()
                    .Include(m => m.ConsultorOcorrencia)
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTipoDadoNF);


                contarConsultorTabelaCampos = await query
                    .CountAsync();

                consultorTabelaCampos = await query
                    //   .AsNoTracking()
                    .ToListAsync();

                consultorTabelaCamposDtos = consultorTabelaCampos
                    .MapTo<List<ConsultorTabelaCampoDto>>();

                return new PagedResultDto<ConsultorTabelaCampoDto>(
               contarConsultorTabelaCampos,
               consultorTabelaCamposDtos
               );

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarTodos()
        {
            var contarConsultorTabelaCampos = 0;
            List<ConsultorTabelaCampo> consultorTabelaCampos;
            List<ConsultorTabelaCampoDto> consultorTabelaCamposDtos = new List<ConsultorTabelaCampoDto>();
            try
            {
                var query = _consultorTabelaCampoRepository
                    .GetAll()
                    .Include(m => m.ConsultorOcorrencia)
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTipoDadoNF);

                contarConsultorTabelaCampos = await query
                    .CountAsync();

                consultorTabelaCampos = await query
                    .ToListAsync();

                consultorTabelaCamposDtos = consultorTabelaCampos
                    .MapTo<List<ConsultorTabelaCampoDto>>();

                return new PagedResultDto<ConsultorTabelaCampoDto>(
               contarConsultorTabelaCampos,
               consultorTabelaCamposDtos
               );

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarConsultorTabelaCampos(ListarConsultorTabelaCamposInput input)
        {
            var contarConsultorTabelaCampos = 0;
            List<ConsultorTabelaCampo> consultorTabelaCampos;
            List<ConsultorTabelaCampoDto> consultorTabelaCamposDtos = new List<ConsultorTabelaCampoDto>();
            try
            {
                var query = _consultorTabelaCampoRepository
                    .GetAll()
                    .Include(m => m.ConsultorOcorrencia)
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTipoDadoNF)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro)
                    );

                contarConsultorTabelaCampos = await query
                    .CountAsync();

                consultorTabelaCampos = await query
                    .AsNoTracking()
                    //     .OrderBy(input.Sorting)
                    //     .PageBy(input)
                    .ToListAsync();

                consultorTabelaCamposDtos = consultorTabelaCampos
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

        public async Task<PagedResultDto<ConsultorTabelaCampoDto>> ListarPorConsultorTabela(long id)
        {
            var contarConsultorTabelaCampos = 0;
            List<ConsultorTabelaCampo> consultorTabelaCampos;
            List<ConsultorTabelaCampoDto> consultorTabelaCamposDtos = new List<ConsultorTabelaCampoDto>();
            try
            {
                var query = _consultorTabelaCampoRepository
                    .GetAll()
                    .Include(m => m.ConsultorOcorrencia)
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTipoDadoNF)
                    .Where(c => c.ConsultorTabelaId == id);

                contarConsultorTabelaCampos = await query
                    .CountAsync();

                consultorTabelaCampos = await query
                    .AsNoTracking()
                    .ToListAsync();

                consultorTabelaCamposDtos = consultorTabelaCampos
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

        public async Task<PagedResultDto<ConsultorTabelaCampoDto>> ComboCampos(long id)
        {
            var contarConsultorTabelaCampos = 0;
            List<ConsultorTabelaCampo> consultorTabelaCampos;
            List<ConsultorTabelaCampoDto> consultorTabelaCamposDtos = new List<ConsultorTabelaCampoDto>();
            try
            {
                var query = _consultorTabelaCampoRepository
                    .GetAll()
                    .Include(m => m.ConsultorOcorrencia)
                    .Include(m => m.ConsultorTabela)
                    .Include(m => m.ConsultorTipoDadoNF)
                    .Where(c => c.ConsultorTabelaId != id);

                contarConsultorTabelaCampos = await query
                    .CountAsync();

                consultorTabelaCampos = await query
                    .AsNoTracking()
                    .ToListAsync();

                consultorTabelaCamposDtos = consultorTabelaCampos
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
