using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(PrescricaoItemResposta))]
    public class PrescricaoItemRespostaDto : CamposPadraoCRUDDto
    {
        public decimal? Quantidade { get; set; }
        public long? UnidadeId { get; set; }
        public UnidadeDto Unidade { get; set; }
        public long? VelocidadeInfusaoId { get; set; }
        public VelocidadeInfusaoDto VelocidadeInfusao { get; set; }
        public long? FormaAplicacaoId { get; set; }
        public FormaAplicacaoDto FormaAplicacao { get; set; }
        public long? FrequenciaId { get; set; }
        public FrequenciaDto Frequencia { get; set; }
        public bool IsSeNecessario { get; set; }
        public bool IsUrgente { get; set; }
        public bool IsDias { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public long? MedicoId { get; set; }
        public MedicoDto Medico { get; set; }
        public DateTime? DataInicial { get; set; }
        public double DiaAtual { get { return DateTime.Now.Subtract(DataInicial.HasValue ? DataInicial.Value : DateTime.Now).TotalDays; } }
        public int? TotalDias { get; set; }
        public string Observacao { get; set; }
        public long? PrescricaoItemId { get; set; }
        public PrescricaoItemDto PrescricaoItem { get; set; }
        public long? PrescricaoMedicaId { get; set; }
        public PrescricaoMedicaDto PrescricaoMedica { get; set; }

        public long? DivisaoId { get; set; }
        public DivisaoDto Divisao { get; set; }

        public long? IdGridPrescricaoItemResposta { get; set; }

        public string Horarios { get; set; }

        public long? PrescricaoItemStatusId { get; set; }
        public PrescricaoStatusDto PrescricaoItemStatus { get; set; }

        public List<PrescricaoItemHoraDto> HorariosPrescricaoItens { get; set; }

        public long? DiluenteId { get; set; }

        public PrescricaoItemDto Diluente { get; set; }

        public double? VolumeDiluente { get; set; }

        public long? AprovadoUserId { get; set; }

        public DateTime? DataAprovado { get; set; }

        public long? LiberadoUserId { get; set; }

        public DateTime? DataLiberado { get; set; }

        public bool IsAcrescimo { get; set; }

        public long? AcrescimoUserId { get; set; }

        public DateTime? DataAcrescimo { get; set; }

        public bool IsSuspenso { get; set; }

        public long? SuspensoUserId { get; set; }

        public DateTime? DataSuspenso { get; set; }

        public bool DoseUnica { get; set; }

        public DateTime DataAgrupamento { get; set; }
        
        public string ObsFrequencia { get; set; }

        public string JustificativaBloqueioDosagemAceitavel { get; set; }

        public long? JustificativaBloqueioId { get; set; }

        public PrescricaoItemRespostaDto()
        {
            DataInicial = DateTime.Now;
        }

        public static PrescricaoItemRespostaDto Mapear(PrescricaoItemResposta input)
        {
            if (input == null)
            {
                return null;
            }

            var result = new PrescricaoItemRespostaDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataInicial = input.DataInicial;
            result.Descricao = input.Descricao;
            result.DivisaoId = input.DivisaoId;
            result.FormaAplicacaoId = input.FormaAplicacaoId;
            result.FrequenciaId = input.FrequenciaId;
            result.Id = input.Id;
            result.IsDias = input.IsDias;
            result.IsSeNecessario = input.IsSeNecessario;
            result.IsSistema = input.IsSistema;
            result.IsUrgente = input.IsUrgente;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.MedicoId = input.MedicoId;
            result.Observacao = input.Observacao;
            result.PrescricaoItemId = input.PrescricaoItemId;
            result.PrescricaoItemStatusId = input.PrescricaoItemStatusId;
            result.PrescricaoMedicaId = input.PrescricaoMedicaId;
            result.Quantidade = input.Quantidade;
            result.TotalDias = input.TotalDias;
            result.UnidadeId = input.UnidadeId;
            result.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
            result.VelocidadeInfusaoId = input.VelocidadeInfusaoId;
            result.VolumeDiluente = input.VolumeDiluente;

            result.AprovadoUserId = input.AprovadoUserId;
            result.DataAprovado = input.DataAprovado;

            result.LiberadoUserId = input.LiberadoUserId;
            result.DataLiberado = input.DataLiberado;

            result.IsAcrescimo = input.IsAcrescimo;
            result.DataAcrescimo = input.DataAcrescimo;
            result.AcrescimoUserId = input.AcrescimoUserId;

            result.IsSuspenso = input.IsSuspenso;
            result.DataSuspenso = input.DataSuspenso;
            result.SuspensoUserId = input.SuspensoUserId;
            result.DoseUnica = input.DoseUnica;
            
            result.DataAgrupamento = input.DataAgrupamento;
            result.ObsFrequencia = input.ObsFrequencia;
            result.JustificativaBloqueioDosagemAceitavel = input.JustificativaBloqueioDosagemAceitavel;
            result.JustificativaBloqueioId = input.JustificativaBloqueioId;

            if (input.Divisao != null)
            {
                result.Divisao = DivisaoDto.Mapear(input.Divisao);
            }
            if (input.FormaAplicacao != null)
            {
                result.FormaAplicacao = FormaAplicacaoDto.Mapear(input.FormaAplicacao);
            }
            if (input.Frequencia != null)
            {
                result.Frequencia = FrequenciaDto.Mapear(input.Frequencia);
            }
            if (input.Medico != null)
            {
                result.Medico = MedicoDto.Mapear(input.Medico);
            }
            if (input.PrescricaoItem != null)
            {
                result.PrescricaoItem = PrescricaoItemDto.Mapear(input.PrescricaoItem);
            }
            if (input.PrescricaoItemStatus != null)
            {
                result.PrescricaoItemStatus = PrescricaoStatusDto.Mapear(input.PrescricaoItemStatus);
            }
            if (input.PrescricaoMedica != null)
            {
                result.PrescricaoMedica = PrescricaoMedicaDto.Mapear(input.PrescricaoMedica);
            }
            if (input.Unidade != null)
            {
                result.Unidade = UnidadeDto.Mapear(input.Unidade);
            }
            if (input.UnidadeOrganizacional != null)
            {
                result.UnidadeOrganizacional = UnidadeOrganizacionalDto.MapearFromCore(input.UnidadeOrganizacional);
            }
            if (input.VelocidadeInfusao != null)
            {
                result.VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(input.VelocidadeInfusao);
            }

            result.DiluenteId = input.DiluenteId;

            if (input.Diluente != null)
            {
                result.Diluente = PrescricaoItemDto.Mapear(input.Diluente);
            }

            return result;
        }

        public static PrescricaoItemResposta Mapear(PrescricaoItemRespostaDto input)
        {
            var result = new PrescricaoItemResposta();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataInicial = input.DataInicial;
            result.Descricao = input.Descricao;
            result.DivisaoId = input.DivisaoId;
            result.FormaAplicacaoId = input.FormaAplicacaoId;
            result.FrequenciaId = input.FrequenciaId;
            result.Id = input.Id;
            result.IsDias = input.IsDias;
            result.IsSeNecessario = input.IsSeNecessario;
            result.IsSistema = input.IsSistema;
            result.IsUrgente = input.IsUrgente;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.MedicoId = input.MedicoId;
            result.Observacao = input.Observacao;
            result.PrescricaoItemId = input.PrescricaoItemId;
            result.PrescricaoItemStatusId = input.PrescricaoItemStatusId;
            result.PrescricaoMedicaId = input.PrescricaoMedicaId;
            result.Quantidade = input.Quantidade;
            result.TotalDias = input.TotalDias;
            result.UnidadeId = input.UnidadeId;
            result.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
            result.VelocidadeInfusaoId = input.VelocidadeInfusaoId;
            result.VolumeDiluente = input.VolumeDiluente;

            result.AprovadoUserId = input.AprovadoUserId;
            result.DataAprovado = input.DataAprovado;

            result.LiberadoUserId = input.LiberadoUserId;
            result.DataLiberado = input.DataLiberado;

            result.IsAcrescimo = input.IsAcrescimo;
            result.DataAcrescimo = input.DataAcrescimo;
            result.AcrescimoUserId = input.AcrescimoUserId;

            result.IsSuspenso = input.IsSuspenso;
            result.DataSuspenso = input.DataSuspenso;
            result.SuspensoUserId = input.SuspensoUserId;

            result.DoseUnica = input.DoseUnica;
            result.DataAgrupamento = input.DataAgrupamento;
            result.ObsFrequencia = input.ObsFrequencia;

            result.JustificativaBloqueioDosagemAceitavel = input.JustificativaBloqueioDosagemAceitavel;
            result.JustificativaBloqueioId = input.JustificativaBloqueioId;

            if (input.Divisao != null)
            {
                result.Divisao = DivisaoDto.Mapear(input.Divisao);
            }
            if (input.FormaAplicacao != null)
            {
                result.FormaAplicacao = FormaAplicacaoDto.Mapear(input.FormaAplicacao);
            }
            if (input.Frequencia != null)
            {
                result.Frequencia = FrequenciaDto.Mapear(input.Frequencia);
            }
            if (input.Medico != null)
            {
                result.Medico = MedicoDto.Mapear(input.Medico);
            }
            if (input.PrescricaoItem != null)
            {
                result.PrescricaoItem = PrescricaoItemDto.Mapear(input.PrescricaoItem);
            }
            if (input.PrescricaoItemStatus != null)
            {
                result.PrescricaoItemStatus = PrescricaoStatusDto.Mapear(input.PrescricaoItemStatus);
            }
            if (input.PrescricaoMedica != null)
            {
                result.PrescricaoMedica = PrescricaoMedicaDto.Mapear(input.PrescricaoMedica);
            }
            if (input.Unidade != null)
            {
                result.Unidade = UnidadeDto.Mapear(input.Unidade);
            }
            if (input.UnidadeOrganizacional != null)
            {
                result.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(input.UnidadeOrganizacional);
            }
            if (input.VelocidadeInfusao != null)
            {
                result.VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(input.VelocidadeInfusao);
            }

            result.DiluenteId = input.DiluenteId;

            if (input.Diluente != null)
            {
                result.Diluente = PrescricaoItemDto.Mapear(input.Diluente);
            }

            return result;
        }

        public static IEnumerable<PrescricaoItemRespostaDto> Mapear(List<PrescricaoItemResposta> input)
        {
            foreach (var item in input)
            {
                var result = new PrescricaoItemRespostaDto();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.DataInicial = item.DataInicial;
                result.Descricao = item.Descricao;
                result.DivisaoId = item.DivisaoId;
                result.FormaAplicacaoId = item.FormaAplicacaoId;
                result.FrequenciaId = item.FrequenciaId;
                result.Id = item.Id;
                result.IsDias = item.IsDias;
                result.IsSeNecessario = item.IsSeNecessario;
                result.IsSistema = item.IsSistema;
                result.IsUrgente = item.IsUrgente;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.MedicoId = item.MedicoId;
                result.Observacao = item.Observacao;
                result.PrescricaoItemId = item.PrescricaoItemId;
                result.PrescricaoItemStatusId = item.PrescricaoItemStatusId;
                result.PrescricaoMedicaId = item.PrescricaoMedicaId;
                result.Quantidade = item.Quantidade;
                result.TotalDias = item.TotalDias;
                result.UnidadeId = item.UnidadeId;
                result.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                result.VelocidadeInfusaoId = item.VelocidadeInfusaoId;
                result.VolumeDiluente = item.VolumeDiluente;

                result.AprovadoUserId = item.AprovadoUserId;
                result.DataAprovado = item.DataAprovado;

                result.LiberadoUserId = item.LiberadoUserId;
                result.DataLiberado = item.DataLiberado;

                result.IsAcrescimo = item.IsAcrescimo;
                result.DataAcrescimo = item.DataAcrescimo;
                result.AcrescimoUserId = item.AcrescimoUserId;

                result.IsSuspenso = item.IsSuspenso;
                result.DataSuspenso = item.DataSuspenso;
                result.SuspensoUserId = item.SuspensoUserId;

                result.DoseUnica = item.DoseUnica;

                result.JustificativaBloqueioDosagemAceitavel = item.JustificativaBloqueioDosagemAceitavel;
                result.JustificativaBloqueioId = item.JustificativaBloqueioId;

                if (item.Divisao != null)
                {
                    result.Divisao = DivisaoDto.Mapear(item.Divisao);
                }
                if (item.FormaAplicacao != null)
                {
                    result.FormaAplicacao = FormaAplicacaoDto.Mapear(item.FormaAplicacao);
                }
                if (item.Frequencia != null)
                {
                    result.Frequencia = FrequenciaDto.Mapear(item.Frequencia);
                }
                if (item.Medico != null)
                {
                    result.Medico = MedicoDto.Mapear(item.Medico);
                }
                if (item.PrescricaoItem != null)
                {
                    result.PrescricaoItem = PrescricaoItemDto.Mapear(item.PrescricaoItem);
                }
                if (item.PrescricaoItemStatus != null)
                {
                    result.PrescricaoItemStatus = PrescricaoStatusDto.Mapear(item.PrescricaoItemStatus);
                }
                if (item.PrescricaoMedica != null)
                {
                    result.PrescricaoMedica = PrescricaoMedicaDto.Mapear(item.PrescricaoMedica);
                }
                if (item.Unidade != null)
                {
                    result.Unidade = UnidadeDto.Mapear(item.Unidade);
                }
                if (item.UnidadeOrganizacional != null)
                {
                    result.UnidadeOrganizacional = UnidadeOrganizacionalDto.MapearFromCore(item.UnidadeOrganizacional);
                }
                if (item.VelocidadeInfusao != null)
                {
                    result.VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(item.VelocidadeInfusao);
                }


                result.DiluenteId = item.DiluenteId;

                if (item.Diluente != null)
                {
                    result.Diluente = PrescricaoItemDto.Mapear(item.Diluente);
                }


                yield return result;
            }
        }

        public static IEnumerable<PrescricaoItemResposta> Mapear(List<PrescricaoItemRespostaDto> input)
        {
            foreach (var item in input)
            {
                var result = new PrescricaoItemResposta();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.DataInicial = item.DataInicial;
                result.Descricao = item.Descricao;
                result.DivisaoId = item.DivisaoId;
                result.FormaAplicacaoId = item.FormaAplicacaoId;
                result.FrequenciaId = item.FrequenciaId;
                result.Id = item.Id;
                result.IsDias = item.IsDias;
                result.IsSeNecessario = item.IsSeNecessario;
                result.IsSistema = item.IsSistema;
                result.IsUrgente = item.IsUrgente;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.MedicoId = item.MedicoId;
                result.Observacao = item.Observacao;
                result.PrescricaoItemId = item.PrescricaoItemId;
                result.PrescricaoItemStatusId = item.PrescricaoItemStatusId;
                result.PrescricaoMedicaId = item.PrescricaoMedicaId;
                result.Quantidade = item.Quantidade;
                result.TotalDias = item.TotalDias;
                result.UnidadeId = item.UnidadeId;
                result.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                result.VelocidadeInfusaoId = item.VelocidadeInfusaoId;
                result.VolumeDiluente = item.VolumeDiluente;

                result.AprovadoUserId = item.AprovadoUserId;
                result.DataAprovado = item.DataAprovado;

                result.LiberadoUserId = item.LiberadoUserId;
                result.DataLiberado = item.DataLiberado;

                result.IsAcrescimo = item.IsAcrescimo;
                result.DataAcrescimo = item.DataAcrescimo;
                result.AcrescimoUserId = item.AcrescimoUserId;

                result.IsSuspenso = item.IsSuspenso;
                result.DataSuspenso = item.DataSuspenso;
                result.SuspensoUserId = item.SuspensoUserId;
                result.DoseUnica = item.DoseUnica;

                result.JustificativaBloqueioDosagemAceitavel = item.JustificativaBloqueioDosagemAceitavel;
                result.JustificativaBloqueioId = item.JustificativaBloqueioId;


                if (item.Divisao != null)
                {
                    result.Divisao = DivisaoDto.Mapear(item.Divisao);
                }
                if (item.FormaAplicacao != null)
                {
                    result.FormaAplicacao = FormaAplicacaoDto.Mapear(item.FormaAplicacao);
                }
                if (item.Frequencia != null)
                {
                    result.Frequencia = FrequenciaDto.Mapear(item.Frequencia);
                }
                if (item.Medico != null)
                {
                    result.Medico = MedicoDto.Mapear(item.Medico);
                }
                if (item.PrescricaoItem != null)
                {
                    result.PrescricaoItem = PrescricaoItemDto.Mapear(item.PrescricaoItem);
                }
                if (item.PrescricaoItemStatus != null)
                {
                    result.PrescricaoItemStatus = PrescricaoStatusDto.Mapear(item.PrescricaoItemStatus);
                }
                if (item.PrescricaoMedica != null)
                {
                    result.PrescricaoMedica = PrescricaoMedicaDto.Mapear(item.PrescricaoMedica);
                }
                if (item.Unidade != null)
                {
                    result.Unidade = UnidadeDto.Mapear(item.Unidade);
                }
                if (item.UnidadeOrganizacional != null)
                {
                    result.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(item.UnidadeOrganizacional);
                }
                if (item.VelocidadeInfusao != null)
                {
                    result.VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(item.VelocidadeInfusao);
                }

                result.DiluenteId = item.DiluenteId;

                if (item.Diluente != null)
                {
                    result.Diluente = PrescricaoItemDto.Mapear(item.Diluente);
                }


                yield return result;
            }
        }

    }

    public class PrescricaoItemRespostaViewModel : CamposPadraoCRUDDto
    {
        public decimal? Quantidade { get; set; }
        public long? UnidadeId { get; set; }
        public string UnidadeSigla { get; set; }
        public long? VelocidadeInfusaoId { get; set; }
        public string VelocidadeInfusaoDescricao { get; set; }
        public long? FormaAplicacaoId { get; set; }
        public string FormaAplicacaoDescricao { get; set; }
        public long? FrequenciaId { get; set; }
        public string FrequenciaDescricao { get; set; }
        public string ObsFrequencia { get; set; }
        public bool IsSeNecessario { get; set; }
        public bool IsUrgente { get; set; }
        public bool IsDias { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public string UnidadeOrganizacionalDescricao { get; set; }
        public long? MedicoId { get; set; }
        public DateTime? DataInicial { get; set; }
        public double DiaAtual { get { return DateTime.Now.Subtract(DataInicial.HasValue ? DataInicial.Value : DateTime.Now).TotalDays; } }
        public int? TotalDias { get; set; }
        public string Observacao { get; set; }
        public long? PrescricaoItemId { get; set; }
        public string PrescricaoItemDescricao { get; set; }
        public long? PrescricaoMedicaId { get; set; }

        public long? DivisaoId { get; set; }
        public string DivisaoDescricao { get; set; }

        public long? IdGridPrescricaoItemResposta { get; set; }

        public string Horarios { get; set; }

        public long? PrescricaoItemStatusId { get; set; }
        public string PrescricaoItemStatusDescricao { get; set; }
        public string PrescricaoItemStatusCor { get; set; }

        public List<PrescricaoItemHoraDto> HorariosPrescricaoItens { get; set; }

        public long? DiluenteId { get; set; }

        public string DiluenteDescricao { get; set; }

        public double? VolumeDiluente { get; set; }

        public long? AprovadoUserId { get; set; }

        public DateTime? DataAprovado { get; set; }

        public long? LiberadoUserId { get; set; }

        public DateTime? DataLiberado { get; set; }

        public bool IsAcrescimo { get; set; }

        public long? AcrescimoUserId { get; set; }

        public DateTime? DataAcrescimo { get; set; }

        public bool IsSuspenso { get; set; }

        public long? SuspensoUserId { get; set; }

        public DateTime? DataSuspenso { get; set; }
        
        public DateTime? DataAgrupamento { get; set; }

        public bool DoseUnica { get; set; }

        public string JustificativaBloqueioDosagemAceitavel { get; set; }

        public long? JustificativaBloqueioId { get; set; }


    }
}
