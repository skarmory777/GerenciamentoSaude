using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public class ControleProducaoAppService : SWMANAGERAppServiceBase, IControleProducaoAppService
    {
        private readonly IRepository<ControleProducao, long> _pacienteRepository;
        //private readonly IListarControleProducaosExcelExporter _listarControleProducaosExcelExporter;

        public ControleProducaoAppService(
            IRepository<ControleProducao, long> pacienteRepository
        //    IListarControleProducaosExcelExporter listarControleProducaosExcelExporter
            )
        {
            _pacienteRepository = pacienteRepository;
            //    _listarControleProducaosExcelExporter = listarControleProducaosExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarControleProducao input)
        {
            try
            {
                var paciente = input.MapTo<ControleProducao>();
                if (input.Id.Equals(0))
                {
                    await _pacienteRepository.InsertAsync(paciente);
                }
                else
                {
                    await _pacienteRepository.UpdateAsync(paciente);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarControleProducao input)
        {
            try
            {
                await _pacienteRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        //public async Task<PagedResultDto<ControleProducaoDto>> Listar(ListarControleProducaosInput input)
        //{
        //    var contarControleProducaos = 0;
        //    List<ControleProducao> pacientes;
        //    List<ControleProducaoDto> pacientesDtos = new List<ControleProducaoDto>();
        //    try
        //    {
        //        var query = _pacienteRepository
        //            .GetAll()
        //            .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
        //            m.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Rg.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Cpf.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Nascimento.ToString().ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.NomeMae.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.NomePai.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Logradouro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Numero.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Complemento.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Estado.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Estado.Uf.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Pais.Sigla.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.ControleProducaoPlanos.FirstOrDefault().Plano.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.ControleProducaoConvenios.FirstOrDefault().Convenio.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Cep.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Observacao.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Pendencia.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Prontuario.ToString().ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Cns.ToString().ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Indicacao.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Telefone4.ToUpper().Contains(input.Filtro.ToUpper())
        //            );

        //        contarControleProducaos = await query
        //            .CountAsync();

        //        pacientes = await query
        //            .AsNoTracking()
        //            .OrderBy(input.Sorting)
        //            .PageBy(input)
        //            .ToListAsync();

        //        pacientesDtos = pacientes
        //            .MapTo<List<ControleProducaoDto>>();

        //        return new PagedResultDto<ControleProducaoDto>(
        //        contarControleProducaos,
        //        pacientesDtos
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        public async Task<PagedResultDto<ControleProducaoDto>> ListarTodos()
        {
            try
            {
                var pacientes = await _pacienteRepository
                    .GetAll()
                    .Include(m => m.Desenvolvedor)
                    .Include(m => m.TabelaSistema)
                    .Include(m => m.UsuarioAprovacao)
                    .AsNoTracking()
                    .ToListAsync();

                var pacientesDtos = pacientes
                    .MapTo<List<ControleProducaoDto>>();

                return new PagedResultDto<ControleProducaoDto> { Items = pacientesDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<CriarOuEditarControleProducao> Obter(long id)
        {
            try
            {
                var query = await _pacienteRepository
                    .GetAll()
                    .Include(m => m.Desenvolvedor)
                    .Include(m => m.TabelaSistema)
                    .Include(m => m.UsuarioAprovacao)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var paciente = query.MapTo<CriarOuEditarControleProducao>();

                return paciente;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
