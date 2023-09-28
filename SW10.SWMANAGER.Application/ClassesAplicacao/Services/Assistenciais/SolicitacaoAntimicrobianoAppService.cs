
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;
using RestSharp;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.Helper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public class SolicitacaoAntimicrobianoAppService : SWMANAGERAppServiceBase, ISolicitacaoAntimicrobianoAppService
    {
        public async Task<PagedResultDto<SolicitacaoAntimicrobianoDto>> Listar(ListarInput input)
        {
            using (var solicitacaoAntimicrobianoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoAntimicrobiano, long>>())
            {
                var dataIQ = solicitacaoAntimicrobianoRepository.Object
                    .GetAll()
                    .Include(x => x.Atendimento)
                    .Include(x => x.Atendimento.Paciente)
                    .Include(x => x.Atendimento.Paciente.SisPessoa)
                    .Include(x => x.Medico)
                    .Include(x => x.Medico.SisPessoa)
                    .Include(x => x.PrescricaoItem)
                    .Include(x => x.Frequencia)
                    .Include(x => x.Unidade)
                    .Include(x => x.VelocidadeInfusao)
                    .Include(x => x.FormaAplicacao);

                var totalCount =await  dataIQ.CountAsync();
                var result = await dataIQ.SortBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<SolicitacaoAntimicrobianoDto>
                {
                    Items = result.Select(SolicitacaoAntimicrobianoDto.MapearEntidadeParaDto).ToList(),
                    TotalCount = totalCount
                };
            }
        }

        public SolicitacaoAntimicrobianoListDto CriarSolicitacoesParaPreencherPorAtendimentoEPrescricaoItems(long atendimentoId, List<long> PrescricaoItemIds)
        {
            using (var solicitacaoAntimicrobianoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoAntimicrobiano, long>>())
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            using (var prescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
            {
                var nowDate = DateTime.Now;
                var items = prescricaoItemRepository.Object.GetAll().AsNoTracking().Where(x => PrescricaoItemIds.Contains(x.Id) && x.IsExigeJustificativa).ToList();
                var itemIds = items.Select(x => x.Id).ToList();
                var SolicitacaoAntimicrobianos = solicitacaoAntimicrobianoRepository.Object.GetAll().AsNoTracking()
                    .Where(x => x.AtendimentoId == atendimentoId && itemIds.Contains(x.PrescricaoItemId ?? 0))
                    .Where(x => DbFunctions.TruncateTime(x.DataSolicitacao) <= nowDate.Date && DbFunctions.TruncateTime(x.DataMaximaTempoProvavel) >= nowDate.Date)
                    .Select(x => x.PrescricaoItemId ?? 0)
                    .ToList();
                var result = new SolicitacaoAntimicrobianoListDto
                {
                    SolicitacaoAntimicrobianos = new List<SolicitacaoAntimicrobianoDto>()
                };
                foreach (var item in items.Where(x => !SolicitacaoAntimicrobianos.Contains(x.Id)))
                {
                    result.SolicitacaoAntimicrobianos.Add(new SolicitacaoAntimicrobianoDto
                    {
                        AtendimentoId = atendimentoId,
                        PrescricaoItemId = item.Id,
                        PrescricaoItem = PrescricaoItemDto.Mapear(item),
                        FrequenciaId =  item.FrequenciaId,
                        DataSolicitacao = nowDate,
                        TempoProvavelUso = 0,
                        SolicitacaoAntimicrobianosCulturas = new List<SolicitacaoAntimicrobianosCulturaDto>(),
                        SolicitacaoAntimicrobianosIndicacoes = new List<SolicitacaoAntimicrobianosIndicacaoDto>()
                    });
                }
                return result;
            }
        }
        
        public SolicitacaoAntimicrobianoListDto CriarSolicitacoesParaPreencherPorAtendimentoEPrescricaoId(long atendimentoId, long prescricaoId)
        {
            using (var solicitacaoAntimicrobianoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoAntimicrobiano, long>>())
            using(var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica,long>>())
            using(var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta,long>>())
            {
                var prescricaoMedica = prescricaoMedicaRepository.Object.GetAll().AsNoTracking().Where(x => x.Id == prescricaoId).FirstOrDefault();
                if (prescricaoMedica != null && prescricaoMedica.Id == 0)
                {
                    return new SolicitacaoAntimicrobianoListDto
                    {
                        SolicitacaoAntimicrobianos = new List<SolicitacaoAntimicrobianoDto>()
                    };
                }
                var prescricaoItems = prescricaoItemRespostaRepository.Object.GetAll().AsNoTracking()
                    .Include(x=> x.PrescricaoItem)
                    .Include(x=> x.Frequencia)
                    .Include(x => x.Unidade)
                    .Include(x => x.VelocidadeInfusao)
                    .Include(x => x.FormaAplicacao)
                    .Where(x => x.PrescricaoMedicaId == prescricaoId && x.PrescricaoItem != null && x.PrescricaoItem.IsExigeJustificativa).ToList();
                var nowDate = DateTime.Now;
                var itemIds = prescricaoItems.Select(x => x.PrescricaoItemId).ToList();
                var SolicitacaoAntimicrobianos = solicitacaoAntimicrobianoRepository.Object.GetAll().AsNoTracking()
                    .Where(x => x.AtendimentoId == atendimentoId && itemIds.Contains(x.PrescricaoItemId ?? 0))
                    .Where(x => DbFunctions.TruncateTime(x.DataSolicitacao) <= nowDate.Date && DbFunctions.TruncateTime(x.DataMaximaTempoProvavel) >= prescricaoMedica.DataPrescricao.Date)
                    .Select(x => x.PrescricaoItemId ?? 0)
                    .ToList();
                var result = new SolicitacaoAntimicrobianoListDto
                {
                    SolicitacaoAntimicrobianos = new List<SolicitacaoAntimicrobianoDto>()
                };
                foreach (var item in prescricaoItems.Where(x => !SolicitacaoAntimicrobianos.Contains(x.PrescricaoItemId.Value)))
                {
                    result.SolicitacaoAntimicrobianos.Add(new SolicitacaoAntimicrobianoDto
                    {
                        AtendimentoId = atendimentoId,
                        PrescricaoItemId = item.PrescricaoItemId,
                        PrescricaoItem = PrescricaoItemDto.Mapear(item.PrescricaoItem),
                        FrequenciaId =  item.FrequenciaId,
                        Frequencia =  FrequenciaDto.Mapear(item.Frequencia),
                        UnidadeId = item.UnidadeId,
                        Unidade = UnidadeDto.Mapear(item.Unidade),
                        FormaAplicacaoId = item.FormaAplicacaoId,
                        FormaAplicacao = FormaAplicacaoDto.Mapear(item.FormaAplicacao),
                        VelocidadeInfusaoId = item.VelocidadeInfusaoId,
                        VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(item.VelocidadeInfusao),
                        Qtd = item.Quantidade,
                        DataSolicitacao = nowDate,
                        TempoProvavelUso = 0,
                        PrescricaoItemRespostaId = item.Id,
                        SolicitacaoAntimicrobianosCulturas = new List<SolicitacaoAntimicrobianosCulturaDto>(),
                        SolicitacaoAntimicrobianosIndicacoes = new List<SolicitacaoAntimicrobianosIndicacaoDto>()
                    });
                }
                return result;
            }
        }

        public ValidacaoSolicitacaoDto ValidaSolicitacaoAntimicrobianoPorPrescricao(long atendimentoId, long prescricaoId)
        {
            using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
            {
                var prescricaoItemIds = prescricaoItemRespostaRepository.Object.GetAll().Where(x => x.PrescricaoMedicaId == prescricaoId).Select(x => x.PrescricaoItemId ?? 0).ToList();
                var result = this.CriarSolicitacoesParaPreencherPorAtendimentoEPrescricaoId(atendimentoId, prescricaoId);

                return new ValidacaoSolicitacaoDto
                {
                    NecessitaSolicitacao = result.SolicitacaoAntimicrobianos.Any(),
                    PrescricaoItemIds = prescricaoItemIds
                };
            }
        }

        public bool ValidaSolicitacaoAntimicrobiano(long atendimentoId, List<long> prescricaoItemIds)
        {
            if (prescricaoItemIds.IsNullOrEmpty())
            {
                return false;
            }

            return CriarSolicitacoesParaPreencherPorAtendimentoEPrescricaoItems(atendimentoId, prescricaoItemIds).SolicitacaoAntimicrobianos.Any();
        }

        public async Task<SolicitacaoAntimicrobianoDto> ObterSolicitacaoPorId(long id)
        {
            using (var solicitacaoAntimicrobianoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoAntimicrobiano, long>>())
            {
                return SolicitacaoAntimicrobianoDto.MapearEntidadeParaDto(await solicitacaoAntimicrobianoRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    .Include(x => x.Atendimento)
                    .Include(x => x.Atendimento.Paciente)
                    .Include(x => x.Atendimento.Paciente.SisPessoa)
                    .Include(x => x.Atendimento.Leito)
                    .Include(x => x.Atendimento.Leito.UnidadeOrganizacional)
                    .Include(x => x.Atendimento.UnidadeOrganizacional)
                    .Include(x => x.Medico)
                    .Include(x => x.Medico.SisPessoa)
                    .Include(x => x.PrescricaoItem)
                    .Include(x => x.Frequencia)
                    .Include(x => x.Unidade)
                    .Include(x => x.VelocidadeInfusao)
                    .Include(x => x.FormaAplicacao)
                    .Include(x => x.SolicitacaoAntimicrobianosCulturas)
                    .Include(x => x.SolicitacaoAntimicrobianosCulturas.Select(z=> z.Tipo))
                    .Include(x => x.SolicitacaoAntimicrobianosCulturas.Select(z => z.SolicitacaoAntimicrobianosResultados))
                    .Include(x => x.SolicitacaoAntimicrobianosIndicacoes)
                    .FirstOrDefaultAsync(x => x.Id == id));
            }
        }

        public async Task<ResultSolicitacaoAntimicrobianoDto> SalvarSolicitacoes(SolicitacaoAntimicrobianoListDto input)
        {
            var result = new ResultSolicitacaoAntimicrobianoDto();
            if (input.SolicitacaoAntimicrobianos.IsNullOrEmpty())
            {
                return result;
            }

            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            using (var solicitacaoAntimicrobianoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoAntimicrobiano, long>>())
            using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
            {
                var usuario = await usuarioRepository.Object.GetAll().Include(i => i.Medico).Include(i => i.Medico.MedicoEspecialidades).AsNoTracking().FirstOrDefaultAsync(w => w.Id == this.AbpSession.UserId);
                var medicoId = usuario?.MedicoId ?? null;

                foreach (var solicitacaoAntimicrobianoDto in input.SolicitacaoAntimicrobianos)
                {
                    var tempoProvavelUso = solicitacaoAntimicrobianoDto.TempoProvavelUso;
                    if (tempoProvavelUso > 0)
                    {
                        tempoProvavelUso -= 1;
                    }
                    solicitacaoAntimicrobianoDto.DataMaximaTempoProvavel = solicitacaoAntimicrobianoDto.DataSolicitacao.AddDays(tempoProvavelUso);

                    var solicitacaoAntimicrobiano = SolicitacaoAntimicrobianoDto.MapearDtoParaEntidade(solicitacaoAntimicrobianoDto);
                    solicitacaoAntimicrobiano.MedicoId = medicoId;
                    solicitacaoAntimicrobiano.Id = solicitacaoAntimicrobianoRepository.Object.InsertOrUpdateAndGetId(solicitacaoAntimicrobiano);
                    result.Ids.Add(solicitacaoAntimicrobiano.Id);
                }

                result.Successo = result.Ids.Count() == input.SolicitacaoAntimicrobianos.Count();
                return result;
            }
        }
        
        
        public async Task<SolicitacaoAntimicrobianosViewModel> SolicitacaoAntimicrobianoModal(long atendimentoId, long? prescricaoId)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var tipoSolicitacaoAntimicrobianosIndicacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoSolicitacaoAntimicrobianosIndicacao, long>>())
            using (var tipoSolicitacaoAntimicrobianosResultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoSolicitacaoAntimicrobianosResultado, long>>())
            using (var tipoSolicitacaoAntimicrobianosCulturasRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoSolicitacaoAntimicrobianosCultura, long>>())
            using (var solicitacaoAntimicrobianosAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoAntimicrobianoAppService>())
            using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
            
            {
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);
                if (atendimento == null || !prescricaoId.HasValue)
                {
                    throw new UserFriendlyException("Não é possivel fazer solicitação antimicrobiano sem atendimento ou prescricao");
                }

                var viewModel = new SolicitacaoAntimicrobianosViewModel
                {
                    AtendimentoId = atendimento.Id,
                    CodigoAtendimento = atendimento.Codigo,
                    Leito = atendimento.Leito?.Descricao,
                    NomePaciente = atendimento.Paciente?.NomeCompleto,
                    PrescricaoId = prescricaoId,
                    UnidadeOrganizacional = atendimento.UnidadeOrganizacional.Descricao,
                    SolicitacaoAntimicrobianos = solicitacaoAntimicrobianosAppService.Object.CriarSolicitacoesParaPreencherPorAtendimentoEPrescricaoId(atendimentoId, prescricaoId.Value).SolicitacaoAntimicrobianos
                };

                foreach (var solicitacaoAntimicrobiano in viewModel.SolicitacaoAntimicrobianos)
                {
                    var resposta = prescricaoItemRespostaRepository.Object.FirstOrDefault(x => x.PrescricaoMedicaId == prescricaoId && 
                                                                                          x.Id == solicitacaoAntimicrobiano.PrescricaoItemRespostaId);
                    if (resposta != null)
                    {
                        solicitacaoAntimicrobiano.TempoProvavelUso = resposta.TotalDias ?? 0;
                    }
                }

                viewModel.TipoIndicacoes = tipoSolicitacaoAntimicrobianosIndicacaoRepository.Object.GetAll().ToList().Select(CamposPadraoCRUDDto.MapearBase<TipoSolicitacaoAntimicrobianosIndicacaoDto>).ToList();
                viewModel.TipoResultados = tipoSolicitacaoAntimicrobianosResultadoRepository.Object.GetAll().ToList().Select(CamposPadraoCRUDDto.MapearBase<TipoSolicitacaoAntimicrobianosResultadoDto>).ToList();
                viewModel.TipoCulturas = tipoSolicitacaoAntimicrobianosCulturasRepository.Object.GetAll().ToList().Select(CamposPadraoCRUDDto.MapearBase<TipoSolicitacaoAntimicrobianosCulturaDto>).ToList();

                return viewModel;
            }
        }


        public byte[] RetornaArquivoSolicitacaoAntimicrobiano(List<long> ids)
        {
            var pdfFiles = new List<byte[]>();
            foreach (var solicitacaoAntimicrobianoId in ids)
            {
                pdfFiles.Add(this.CreateJasperReport("SolicitacaoAntimicrobiano")
                    .AddParameter("solicitacaoAntimicrobianoId", solicitacaoAntimicrobianoId.ToString())
                    .AddParameter("Dominio", this.GetConnectionStringName())
                    .GenerateReport());
            }

            return FileHelper.ConcatAndAddContent(pdfFiles);
        }        
    }
}
