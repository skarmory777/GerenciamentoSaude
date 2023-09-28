using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Relatorios
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    public class RelatorioAtendimentoAppService : SWMANAGERAppServiceBase, IRelatorioAtendimentoAppService
    {
        private readonly IRepository<Atendimento, long> _atendimentoRepository;
        private readonly IRepository<Leito, long> _leitoRepository;
        internal IRepository<Empresa, long> _empresaRepositorio { get; private set; }

        private readonly IRepository<UserEmpresa, long> _userEmpresas;

        public RelatorioAtendimentoAppService(
            IRepository<Atendimento, long> atendimentoRepository,
            IRepository<Leito, long> leitoRepository,
            IRepository<Empresa, long> empresaRepositorio,
            IRepository<UserEmpresa, long> userEmpresas)
        {
            _leitoRepository = leitoRepository;
            _atendimentoRepository = atendimentoRepository;
            _empresaRepositorio = empresaRepositorio;
            _userEmpresas = userEmpresas;
        }


        [AbpAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public IList<GenericoIdNome> ListarEmpresaUsuario(long id)
        {
            var result = _userEmpresas
                .GetAll()
                .Where(m => m.UserId == id)
                .Where(m => m.IsDeleted == false);

            //var result = await query
            //    .Select(m => m.Empresa)
            //    .ToListAsync();

            //var result = await _userEmpresas
            //    .Query(q => q.Where(g => g.IsDeleted == false))
            //    .ToListAsync();

            if (result != null)
            {
                return result
                 .Select(s => new GenericoIdNome
                 {
                     Id = s.EmpresaId,
                     Nome = s.Empresa.NomeFantasia
                 })
                 .ToList();
            }

            return new List<GenericoIdNome>();
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<AtendimentoDto>> ListarRelatorio(long empresaId, long convenioId, long medicoId, long especialidadeId)
        {
            List<Atendimento> atendimentos;
            List<AtendimentoDto> atendimentosDtos = new List<AtendimentoDto>();
            try
            {
                var query = _atendimentoRepository
                   .GetAll()
                  .Include(m => m.Paciente)
                  .Include(m => m.Paciente.SisPessoa)
                  .Include(m => m.Medico)
                  .Include(m => m.Medico.SisPessoa)
                  .Include(m => m.AtendimentoTipo)
                  .Include(m => m.Convenio)
                  .Include(i => i.Convenio.SisPessoa)
                  .Include(m => m.Empresa)
                  .Include(m => m.Especialidade)
                  .Include(m => m.Guia)
                  .Include(m => m.Leito)
                  .Include(m => m.Leito.UnidadeOrganizacional)
                  .Include(m => m.Leito.LeitoStatus)
                  .Include(m => m.MotivoAlta)
                  .Include(m => m.Nacionalidade)
                  .Include(m => m.Origem)
                  .Include(m => m.Plano)
                  .Include(m => m.ServicoMedicoPrestado)
                  .Include(m => m.UnidadeOrganizacional)
                  .Where(a => a.IsInternacao == true)
                  .Where(a => a.DataAlta == null)
                  .Where(a => (empresaId == 0 || a.EmpresaId == empresaId)
                    && (convenioId == 0 || a.ConvenioId == convenioId)
                    && (medicoId == 0 || a.MedicoId == medicoId)
                    && (especialidadeId == 0 || a.EspecialidadeId == especialidadeId)
                    && a.DataAlta == null);


                atendimentos = await query
                    .AsNoTracking()
                    .ToListAsync();

                atendimentosDtos = atendimentos.MapTo<List<AtendimentoDto>>();

                var leitosNaoOcupadosDto = _leitoRepository.GetAll()
                    .Include(s => s.LeitoStatus)
                    .Include(uo => uo.UnidadeOrganizacional)
                    .Where(i => i.LeitoStatusId != 1);

                foreach (var leito in leitosNaoOcupadosDto)
                {
                    var atdPlacebo = new AtendimentoDto();
                    atdPlacebo.Leito = leito.MapTo<LeitoDto>();
                    atdPlacebo.LeitoId = leito.Id;
                    atdPlacebo.Paciente = new Cadastros.Pacientes.Dto.PacienteDto();
                    atdPlacebo.Paciente.Nascimento = DateTime.Now;
                    atdPlacebo.Paciente.SisPessoa = new Cadastros.Pessoas.Dto.SisPessoaDto();
                    atdPlacebo.Paciente.SisPessoa.Nascimento = DateTime.Now;

                    if (leito.LeitoStatus.Descricao != "Ocupado")
                    {
                        atdPlacebo.Paciente.NomeCompleto = leito.LeitoStatus?.Descricao;
                        atdPlacebo.Paciente.SisPessoa.NomeCompleto = leito.LeitoStatus?.Descricao;
                    }
                    else
                    {
                        atdPlacebo.Paciente.NomeCompleto = "";
                        atdPlacebo.Paciente.SisPessoa.NomeCompleto = "";
                    }

                    atendimentosDtos.Add(atdPlacebo);
                }

                foreach (var a in atendimentosDtos)
                {
                    if (a.Convenio != null && a.Convenio.NomeFantasia.Length > 13)
                    {
                        a.Convenio.NomeFantasia = a.Convenio.NomeFantasia.Substring(0, 12);
                    }

                    if (a.Leito == null)
                    {
                        var leitoPlacebo = new LeitoDto();
                        leitoPlacebo.UnidadeOrganizacional = new Cadastros.UnidadesOrganizacionais.Dto.UnidadeOrganizacionalDto();
                        leitoPlacebo.UnidadeOrganizacional.Descricao = "  ";
                        a.Leito = leitoPlacebo;
                    }
                }

                return new ListResultDto<AtendimentoDto> { Items = atendimentosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento)]
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<AtendimentoDetalhadoDsDto>> ListarRelatorioAtendimento(long empresaId, long convenioId, long medicoId, long especialidadeId, DateTime StartDate, DateTime EndDate)
        {
            List<Atendimento> atendimentos;
            List<AtendimentoDetalhadoDsDto> AtendimentoDetalhadoDsDto = new List<AtendimentoDetalhadoDsDto>();
            try
            {
                var query = _atendimentoRepository
                   .GetAll()
                  .Include(m => m.Paciente)
                  .Include(m => m.Paciente.SisPessoa)
                  .Include(m => m.Medico)
                  .Include(m => m.Medico.SisPessoa)
                  .Include(m => m.AtendimentoTipo)
                  .Include(m => m.Convenio)
                  .Include(i => i.Convenio.SisPessoa)
                  .Include(m => m.Empresa)
                  .Include(m => m.Especialidade)
                  .Include(m => m.Guia)
                  .Include(m => m.Leito)
                  .Include(m => m.Leito.UnidadeOrganizacional)
                  .Include(m => m.Leito.LeitoStatus)
                  .Include(m => m.MotivoAlta)
                  .Include(m => m.Nacionalidade)
                  .Include(m => m.Origem)
                  .Include(m => m.Plano)
                  .Include(m => m.ServicoMedicoPrestado)
                  .Include(m => m.UnidadeOrganizacional)
                  .Where(a => a.IsInternacao == true)
                  .Where(a => a.DataAlta == null)
                  .Where(a => (empresaId == 0 || a.EmpresaId == empresaId)
                    && (convenioId == 0 || a.ConvenioId == convenioId)
                    && (medicoId == 0 || a.MedicoId == medicoId)
                    && (especialidadeId == 0 || a.EspecialidadeId == especialidadeId)
                    && a.DataAlta == null);


                atendimentos = await query
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var m in atendimentos)
                {
                    AtendimentoDetalhadoDsDto.Add(new AtendimentoDetalhadoDsDto()
                    {
                        Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,
                        DataAtendimento = m.DataRegistro.ToString("dd/MM/yyyy"),
                        Especialidade = m.Especialidade == null ? string.Empty : m.Especialidade.Descricao,
                        Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,
                        Medico = m.Medico == null ? string.Empty : m.Medico.NomeCompleto,
                        TipoAtendimento = m.IsInternacao ? "I" : "A",
                        Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,
                        Unidade = m.UnidadeOrganizacional == null ? string.Empty : m.UnidadeOrganizacional.Descricao
                    });
                }

                //AtendimentoDetalhadoDsDto = atendimentos.MapTo<List<AtendimentoDetalhadoDsDto>>();


                foreach (var a in AtendimentoDetalhadoDsDto)
                {
                    if (a.Convenio != null && a.Convenio.Length > 13)
                    {
                        a.Convenio = a.Convenio.Substring(0, 12);
                    }
                }

                return new ListResultDto<AtendimentoDetalhadoDsDto> { Items = AtendimentoDetalhadoDsDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
