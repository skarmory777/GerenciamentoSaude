// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalancoHidricoItemDto.cs" company="">
//   
// </copyright>
// <summary>
//   The balanco hidrico item dto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    using Abp.AutoMapper;
    using Castle.Core.Internal;
    using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.BalancoHidrico;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The balanco hidrico item dto.
    /// </summary>
    [AutoMap(typeof(BalancoHidrico))]
    public class BalancoHidricoDto : CamposPadraoCRUDDto
    {
        public static int BalancoHoraIntervalo = 6;
        /// <summary>
        /// Gets or sets the atend id.
        /// </summary>
        public long AtendimentoId { get; set; }

        /// <summary>
        /// Gets or sets the data balanco hidrico.
        /// </summary>
        public DateTime DataBalancoHidrico { get; set; }

        public double DiasNaAcomodacao { get; set; }
        
        public string TipoAcomodacao { get; set; }

        /// <summary>
        /// Gets or sets the hora intervalo.
        /// </summary>
        public int HoraIntervalo { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico items.
        /// </summary>
        public ICollection<BalancoHidricoItemDto> BalancoHidricoItems { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico solucoes.
        /// </summary>
        public ICollection<BalancoHidricoSolucoesDto> BalancoHidricoSolucoes { get; set; }

        /// <summary>
        /// Gets or sets the altura.
        /// </summary>
        public string Altura { get; set; }

        /// <summary>
        /// Gets or sets the peso.
        /// </summary>
        public string Peso { get; set; }
        
        public bool ConferidoManha { get; set; }
        
        public long? ConferidoManhaUserId { get; set; }
        
        public string ConferidoManhaUserName { get; set; }
        
        public DateTime? DtConferidoManha { get; set; }
        
        public bool ConferidoNoite { get; set; }
        
        public long? ConferidoNoiteUserId { get; set; }
        
        public string ConferidoNoiteUserName { get; set; }
        
        public DateTime? DtConferidoNoite { get; set; }
        
        public DateTime? DtConferidoTotal { get; set; }
        
        public bool ConferidoTotal { get; set; }
        
        public long? ConferidoTotalUserId { get; set; }
        
        public string ConferidoTotalUserName { get; set; }
        
        public bool DesConferidoManha { get; set; }
        
        public long? DesConferidoManhaUserId { get; set; }
        
        public DateTime? DtDesConferidoManha { get; set; }
        
        public bool DesConferidoNoite { get; set; }
        
        public long? DesConferidoNoiteUserId { get; set; }
        
        public DateTime? DtDesConferidoNoite { get; set; }
        
        public bool DesConferidoTotal { get; set; }
        
        public long? DesConferidoTotalUserId { get; set; }
        
        public DateTime? DtDesConferidoTotal { get; set; }
        
        public bool? Evacuacoes { get; set; }
        
        public string Aspecto { get; set; }
        public bool EnableDesconferir { get; set; }

        public bool CheckConferirTurno()
        {
            return !ConferidoManha || !ConferidoNoite;
        }
        public bool CheckConferirTotal()
        {
            return !CheckConferirTurno() && !ConferidoTotal;
        }
        
        public static IEnumerable<BalancoHidricoDto> Mapear(IEnumerable<BalancoHidrico> entities)
        {
            if(entities.IsNullOrEmpty())
            {
                return null;
            }
            return entities.ToList().Select(MapearEntidade);
        }

        public static BalancoHidricoDto MapearEntidade(BalancoHidrico entity)
        {
            if(entity == null)
            {
                return null;
            }

            var dto = MapearBase<BalancoHidricoDto>(entity);
            dto.AtendimentoId = entity.AtendimentoId;
            dto.DataBalancoHidrico = entity.DataBalancoHidrico;
            dto.DiasNaAcomodacao = entity.DiasNaAcomodacao;
            dto.TipoAcomodacao = entity.TipoAcomodacao;
            dto.HoraIntervalo = entity.HoraIntervalo;
            dto.Evacuacoes = entity.Evacuacoes;
            dto.Aspecto = entity.Aspecto;
            
            dto.ConferidoManha = entity.ConferidoManha;
            dto.DtConferidoManha = entity.DtConferidoManha;
            dto.ConferidoManhaUserId = entity.ConferidoManhaUserId;
            
            dto.ConferidoNoite = entity.ConferidoNoite;
            dto.DtConferidoNoite = entity.DtConferidoNoite;
            dto.ConferidoNoiteUserId = entity.ConferidoNoiteUserId;
            
            dto.ConferidoTotal = entity.ConferidoTotal;
            dto.DtConferidoTotal = entity.DtConferidoTotal;
            dto.ConferidoTotalUserId = entity.ConferidoTotalUserId;

            dto.DesConferidoManha = entity.DesConferidoManha;
            dto.DtDesConferidoManha = entity.DtDesConferidoManha;
            dto.DesConferidoManhaUserId = entity.DesConferidoManhaUserId;
            
            dto.DesConferidoNoite = entity.DesConferidoNoite;
            dto.DtDesConferidoNoite = entity.DtDesConferidoNoite;
            dto.DesConferidoNoiteUserId = entity.DesConferidoNoiteUserId;
            
            dto.DesConferidoTotal = entity.DesConferidoTotal;
            dto.DtDesConferidoTotal = entity.DtDesConferidoTotal;
            dto.DesConferidoTotalUserId = entity.DesConferidoTotalUserId;
            
            dto.BalancoHidricoItems = BalancoHidricoItemDto.Mapear(entity.BalancoHidricoItems);
            dto.BalancoHidricoSolucoes = BalancoHidricoSolucoesDto.Mapear(entity.BalancoHidricoSolucoes);

            return dto;
        }
        
        public static BalancoHidrico Mapear(BalancoHidricoDto dto)
        {
            if(dto == null)
            {
                return null;
            }

            var entity = MapearBase<BalancoHidrico>(dto);
            entity.AtendimentoId = dto.AtendimentoId;
            entity.DataBalancoHidrico = dto.DataBalancoHidrico;
            entity.DiasNaAcomodacao = dto.DiasNaAcomodacao;
            entity.TipoAcomodacao = dto.TipoAcomodacao;
            entity.HoraIntervalo = dto.HoraIntervalo;
            entity.Evacuacoes = dto.Evacuacoes;
            entity.Aspecto = dto.Aspecto;
            
            entity.ConferidoManha = dto.ConferidoManha;
            entity.DtConferidoManha = dto.DtConferidoManha;
            entity.ConferidoManhaUserId = dto.ConferidoManhaUserId;
            
            entity.ConferidoNoite = dto.ConferidoNoite;
            entity.DtConferidoNoite = dto.DtConferidoNoite;
            entity.ConferidoNoiteUserId = dto.ConferidoNoiteUserId;
            
            entity.ConferidoTotal = dto.ConferidoTotal;
            entity.DtConferidoTotal = dto.DtConferidoTotal;
            entity.ConferidoTotalUserId = dto.ConferidoTotalUserId;

            entity.DesConferidoManha = dto.DesConferidoManha;
            entity.DtDesConferidoManha = dto.DtDesConferidoManha;
            entity.DesConferidoManhaUserId = dto.DesConferidoManhaUserId;
            
            entity.DesConferidoNoite = dto.DesConferidoNoite;
            entity.DtDesConferidoNoite = dto.DtDesConferidoNoite;
            entity.DesConferidoNoiteUserId = dto.DesConferidoNoiteUserId;
            
            entity.DesConferidoTotal = dto.DesConferidoTotal;
            entity.DtDesConferidoTotal = dto.DtDesConferidoTotal;
            entity.DesConferidoTotalUserId = dto.DesConferidoTotalUserId;
            
            entity.ConferidoTotalUserId = dto.ConferidoTotalUserId;
            entity.BalancoHidricoItems = BalancoHidricoItemDto.Mapear(dto.BalancoHidricoItems);
            entity.BalancoHidricoSolucoes = BalancoHidricoSolucoesDto.Mapear(dto.BalancoHidricoSolucoes);

            return entity;
        }
        
        public static BalancoHidricoDto Mapear(BalancoHidrico entity)
        {
            if(entity == null)
            {
                return null;
            }

            var dto = MapearBase<BalancoHidricoDto>(entity);
            dto.AtendimentoId = entity.AtendimentoId;
            dto.DataBalancoHidrico = entity.DataBalancoHidrico;
            dto.DiasNaAcomodacao = entity.DiasNaAcomodacao;
            dto.TipoAcomodacao = entity.TipoAcomodacao;
            dto.HoraIntervalo = entity.HoraIntervalo;
            dto.Evacuacoes = entity.Evacuacoes;
            dto.Aspecto = entity.Aspecto;
            
            dto.ConferidoManha = entity.ConferidoManha;
            dto.DtConferidoManha = entity.DtConferidoManha;
            dto.ConferidoManhaUserId = entity.ConferidoManhaUserId;
            
            dto.ConferidoNoite = entity.ConferidoNoite;
            dto.DtConferidoNoite = entity.DtConferidoNoite;
            dto.ConferidoNoiteUserId = entity.ConferidoNoiteUserId;
            
            dto.ConferidoTotal = entity.ConferidoTotal;
            dto.DtConferidoTotal = entity.DtConferidoTotal;
            dto.ConferidoTotalUserId = entity.ConferidoTotalUserId;

            dto.DesConferidoManha = entity.DesConferidoManha;
            dto.DtDesConferidoManha = entity.DtDesConferidoManha;
            dto.DesConferidoManhaUserId = entity.DesConferidoManhaUserId;
            
            dto.DesConferidoNoite = entity.DesConferidoNoite;
            dto.DtDesConferidoNoite = entity.DtDesConferidoNoite;
            dto.DesConferidoNoiteUserId = entity.DesConferidoNoiteUserId;
            
            dto.DesConferidoTotal = entity.DesConferidoTotal;
            dto.DtDesConferidoTotal = entity.DtDesConferidoTotal;
            dto.DesConferidoTotalUserId = entity.DesConferidoTotalUserId;
            
            dto.ConferidoTotalUserId = entity.ConferidoTotalUserId;
            dto.BalancoHidricoItems = BalancoHidricoItemDto.Mapear(entity.BalancoHidricoItems);
            dto.BalancoHidricoSolucoes = BalancoHidricoSolucoesDto.Mapear(entity.BalancoHidricoSolucoes);

            return dto;
        }
    }

    /// <summary>
    /// The balanco hidrico solucoes dto.
    /// </summary>
    [AutoMap(typeof(BalancoHidricoSolucoes))]
    public class BalancoHidricoSolucoesDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Gets or sets the balanco hidrico id.
        /// </summary>
        public long BalancoHidricoId { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico.
        /// </summary>
        public BalancoHidrico BalancoHidrico { get; set; }

        /// <summary>
        /// Gets or sets the indice solucao.
        /// </summary>
        public int IndiceSolucao { get; set; }

        /// <summary>
        /// Gets or sets the valor.
        /// </summary>
        public string Valor { get; set; }

        public static ICollection<BalancoHidricoSolucoesDto> Mapear(IEnumerable<BalancoHidricoSolucoes> entities)
        {
            if (entities.IsNullOrEmpty())
            {
                return null;
            }
            return entities.ToList().Select(MapearEntidade).ToList();
        }
        
        public static ICollection<BalancoHidricoSolucoes> Mapear(IEnumerable<BalancoHidricoSolucoesDto> dto)
        {
            if (dto.IsNullOrEmpty())
            {
                return null;
            }
            return dto.ToList().Select(Mapear).ToList();
        }

        public static BalancoHidricoSolucoesDto MapearEntidade(BalancoHidricoSolucoes entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<BalancoHidricoSolucoesDto>(entity);
            dto.BalancoHidricoId = entity.BalancoHidricoId;
            dto.IndiceSolucao = entity.IndiceSolucao;
            dto.Valor = entity.Valor;

            return dto;
        }
        
        public static BalancoHidricoSolucoes Mapear(BalancoHidricoSolucoesDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<BalancoHidricoSolucoes>(dto);
            entity.BalancoHidricoId = dto.BalancoHidricoId;
            entity.IndiceSolucao = dto.IndiceSolucao;
            entity.Valor = dto.Valor;

            return entity;
        }
    }

    /// <summary>
    /// The balanco hidrico item dto.
    /// </summary>
    [AutoMap(typeof(BalancoHidricoItem))]
    public class BalancoHidricoItemDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Gets or sets the balanco hidrico id.
        /// </summary>
        public long BalancoHidricoId { get; set; }

        /// <summary>
        /// Gets or sets the atend id.
        /// </summary>
        public long AtendimentoId { get; set; }

        /// <summary>
        /// Gets or sets the hora.
        /// </summary>
        public TimeSpan Hora { get; set; }

        public long SinaisVitaisId { get; set; }

        /// <summary>
        /// Gets or sets the sinais vitais.
        /// </summary>
        public BalancoHidricoSinaisVitaisDto SinaisVitais { get; set; } = new BalancoHidricoSinaisVitaisDto();

        /// <summary>
        /// Gets or sets the endovenosos.
        /// </summary>
        public ICollection<BalancoHidricoEndovenosoDto> Endovenosos { get; set; } =
            new List<BalancoHidricoEndovenosoDto>();

        /// <summary>
        /// Gets or sets the sangue derivados.
        /// </summary>
        public string SangueDerivados { get; set; }

        /// <summary>
        /// Gets or sets the ingest vo sne.
        /// </summary>
        public string IngestVoSne { get; set; }

        public string Enteral { get; set; }

        /// <summary>
        /// Gets or sets the diurese.
        /// </summary>
        public string Diurese { get; set; }

        /// <summary>
        /// Gets or sets the hd.
        /// </summary>
        public string Hd { get; set; }

        /// <summary>
        /// Gets or sets the dreno.
        /// </summary>
        public string Dreno { get; set; }

        /// <summary>
        /// Gets or sets the dreno.
        /// </summary>
        public string Dreno2 { get; set; }
        
        
        public string IrrigacaodeEntrada  { get; set; }
        
        public string IrrigacaodeSaida { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether total parcial.
        /// </summary>
        public bool TotalParcial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether total geral.
        /// </summary>
        public bool TotalGeral { get; set; }
        
        public bool TotalTransporte { get; set; }

        /// <summary>
        /// Gets or sets the responsavel assinatura.
        /// </summary>
        public long ResponsavelAssinatura { get; set; }

        public static ICollection<BalancoHidricoItemDto> Mapear(IEnumerable<BalancoHidricoItem> entities)
        {
            if (entities.IsNullOrEmpty())
            {
                return null;
            }
            return entities.ToList().Select(MapearEntidade).ToList();
        }
        
        public static ICollection<BalancoHidricoItem> Mapear(IEnumerable<BalancoHidricoItemDto> dto)
        {
            if (dto.IsNullOrEmpty())
            {
                return null;
            }
            return dto.ToList().Select(Mapear).ToList();
        }

        public static BalancoHidricoItemDto MapearEntidade(BalancoHidricoItem entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<BalancoHidricoItemDto>(entity);
            dto.BalancoHidricoId = entity.BalancoHidricoId;
            dto.Hora = entity.Hora;
            dto.SinaisVitaisId = entity.SinaisVitaisId;
            dto.SinaisVitais = BalancoHidricoSinaisVitaisDto.MapearEntidade(entity.SinaisVitais);
            dto.TotalGeral = entity.TotalGeral;
            dto.TotalParcial = entity.TotalParcial;
            dto.TotalTransporte = entity.TotalTransporte;
            dto.ResponsavelAssinatura = entity.ResponsavelAssinatura;
            dto.IngestVoSne = entity.IngestVoSne;
            dto.Diurese = entity.Diurese;
            dto.Dreno = entity.Dreno;
            dto.Dreno2 = entity.Dreno2;
            dto.IrrigacaodeEntrada = entity.IrrigacaodeEntrada;
            dto.IrrigacaodeSaida = entity.IrrigacaodeSaida;
            dto.Enteral = entity.Enteral;
            dto.SangueDerivados = entity.SangueDerivados;
            dto.Hd = entity.Hd;
            dto.Endovenosos = BalancoHidricoEndovenosoDto.Mapear(entity.Endovenosos);
            return dto;
        }
        
        public static BalancoHidricoItem Mapear(BalancoHidricoItemDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<BalancoHidricoItem>(dto);
            entity.BalancoHidricoId = dto.BalancoHidricoId;
            entity.Hora = dto.Hora;
            entity.SinaisVitaisId = dto.SinaisVitaisId;
            entity.SinaisVitais = BalancoHidricoSinaisVitaisDto.Mapear(dto.SinaisVitais);
            entity.TotalGeral = dto.TotalGeral;
            entity.TotalParcial = dto.TotalParcial;
            entity.TotalTransporte = dto.TotalTransporte;
            entity.ResponsavelAssinatura = dto.ResponsavelAssinatura;
            entity.IngestVoSne = dto.IngestVoSne;
            entity.Diurese = dto.Diurese;
            entity.Dreno = dto.Dreno;
            entity.Dreno2 = dto.Dreno2;
            entity.IrrigacaodeEntrada = dto.IrrigacaodeEntrada;
            entity.IrrigacaodeSaida = dto.IrrigacaodeSaida;
            entity.Enteral = dto.Enteral;
            entity.SangueDerivados = dto.SangueDerivados;
            entity.Hd = dto.Hd;
            entity.Endovenosos = BalancoHidricoEndovenosoDto.Mapear(dto.Endovenosos);
            return entity;
        }
    }

    /// <summary>
    /// The balanco hidrico endovenoso dto.
    /// </summary>
    [AutoMap(typeof(BalancoHidricoEndovenoso))]
    public class BalancoHidricoEndovenosoDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Gets or sets the balanco hidrico item id.
        /// </summary>
        public long BalancoHidricoItemId { get; set; }

        /// <summary>
        /// Gets or sets the indice solucao.
        /// </summary>
        public int IndiceSolucao { get; set; }

        /// <summary>
        /// Gets or sets the valor.
        /// </summary>
        public string Valor { get; set; }

        public static ICollection<BalancoHidricoEndovenosoDto> Mapear(IEnumerable<BalancoHidricoEndovenoso> entities)
        {
            if (entities.IsNullOrEmpty())
            {
                return null;
            }
            return entities.ToList().Select(MapearEntidade).ToList();
        }

        public static BalancoHidricoEndovenosoDto MapearEntidade(BalancoHidricoEndovenoso entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<BalancoHidricoEndovenosoDto>(entity);
            dto.BalancoHidricoItemId = entity.BalancoHidricoItemId;
            dto.IndiceSolucao = entity.IndiceSolucao;
            dto.Valor = entity.Valor;

            return dto;
        }
        
        public static ICollection<BalancoHidricoEndovenoso> Mapear(IEnumerable<BalancoHidricoEndovenosoDto> dtos)
        {
            if (dtos.IsNullOrEmpty())
            {
                return null;
            }
            return dtos.ToList().Select(Mapear).ToList();
        }
        
        public static BalancoHidricoEndovenoso Mapear(BalancoHidricoEndovenosoDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<BalancoHidricoEndovenoso>(dto);
            entity.BalancoHidricoItemId = dto.BalancoHidricoItemId;
            entity.IndiceSolucao = dto.IndiceSolucao;
            entity.Valor = dto.Valor;

            return entity;
        }
    }

    /// <summary>
    /// The balanco hidrico sinais vitais dto.
    /// </summary>
    [AutoMap(typeof(BalancoHidricoSinaisVitais))]
    public class BalancoHidricoSinaisVitaisDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Gets or sets the temperatura.
        /// </summary>
        public string Temperatura { get; set; }

        /// <summary>
        /// Gets or sets the pulso.
        /// </summary>
        public string Pulso { get; set; }

        /// <summary>
        /// Gets or sets the respiracao.
        /// </summary>
        public string Respiracao { get; set; }

        /// <summary>
        /// Gets or sets the pressao sistolica.
        /// </summary>
        public string PressaoSistolica { get; set; }

        /// <summary>
        /// Gets or sets the pressao diastolica.
        /// </summary>
        public string PressaoDiastolica { get; set; }

        /// <summary>
        /// Gets or sets the pressao venosa central.
        /// </summary>
        public string PressaoVenosaCentral { get; set; }

        /// <summary>
        /// Gets or sets the escala de dor.
        /// </summary>
        public string EscalaDeDor { get; set; }

        /// <summary>
        /// Gets or sets the hemoglucoteste.
        /// </summary>
        public string Hemoglucoteste { get; set; }
        
        public string Spo2 { get; set; }
        
        public string Ins { get; set; }
        
        public string PressaoIntracraniana { get; set; }

        public static ICollection<BalancoHidricoSinaisVitaisDto> Mapear(IEnumerable<BalancoHidricoSinaisVitais> entities)
        {
            if (entities.IsNullOrEmpty())
            {
                return null;
            }
            return entities.ToList().Select(MapearEntidade).ToList();
        }
        
        public static ICollection<BalancoHidricoSinaisVitais> Mapear(IEnumerable<BalancoHidricoSinaisVitaisDto> dtos)
        {
            if (dtos.IsNullOrEmpty())
            {
                return null;
            }
            return dtos.ToList().Select(Mapear).ToList();
        }

        public static BalancoHidricoSinaisVitaisDto MapearEntidade(BalancoHidricoSinaisVitais entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<BalancoHidricoSinaisVitaisDto>(entity);
            dto.EscalaDeDor = entity.EscalaDeDor;
            dto.Hemoglucoteste = entity.Hemoglucoteste;
            dto.PressaoDiastolica = entity.PressaoDiastolica;
            dto.PressaoSistolica = entity.PressaoSistolica;
            dto.Temperatura = entity.Temperatura;
            dto.Respiracao = entity.Respiracao;
            dto.Pulso = entity.Pulso;
            dto.PressaoVenosaCentral = entity.PressaoVenosaCentral;
            dto.Ins = entity.Ins;
            dto.Spo2 = entity.Spo2;
            dto.PressaoIntracraniana = entity.PressaoIntracraniana;
            return dto;
        }
        
        public static BalancoHidricoSinaisVitais Mapear(BalancoHidricoSinaisVitaisDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<BalancoHidricoSinaisVitais>(dto);
            entity.EscalaDeDor = dto.EscalaDeDor;
            entity.Hemoglucoteste = dto.Hemoglucoteste;
            entity.PressaoDiastolica = dto.PressaoDiastolica;
            entity.PressaoSistolica = dto.PressaoSistolica;
            entity.Temperatura = dto.Temperatura;
            entity.Respiracao = dto.Respiracao;
            entity.Pulso = dto.Pulso;
            entity.PressaoVenosaCentral = dto.PressaoVenosaCentral;
            entity.Ins = dto.Ins;
            entity.Spo2 = dto.Spo2;
            entity.PressaoIntracraniana = dto.PressaoIntracraniana;
            return entity;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// The time comparer.
    /// </summary>
    public class BalancoHidricoComparer : IComparer<TimeSpan>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BalancoHidricoComparer"/> class.
        /// </summary>
        /// <param name="defaultStep">
        /// The default step.
        /// </param>
        public BalancoHidricoComparer(TimeSpan defaultStep)
        {
            this.DefaultStep = defaultStep.Add(TimeSpan.FromHours(1));
        }

        /// <summary>
        /// Gets the default step.
        /// </summary>
        public TimeSpan DefaultStep { get; }

        /// <inheritdoc />
        public int Compare(TimeSpan x, TimeSpan y)
        {
            var midnight = new TimeSpan(24, 0, 0);
            var elevenPM = midnight.Subtract(TimeSpan.FromHours(1));
            if (x == midnight || y == midnight)
            {
                if (x == elevenPM || y == elevenPM)
                {
                    return TimeSpan.Compare(x, y);
                }

                switch (TimeSpan.Compare(x, y))
                {
                    case 0:
                        return 0;
                    case -1:
                        return 1;
                    case 1:
                        return -1;
                }
            }

            if (x < this.DefaultStep || y < this.DefaultStep)
            {
                if (x < this.DefaultStep && y < this.DefaultStep)
                {
                    return TimeSpan.Compare(x, y);
                }

                switch (TimeSpan.Compare(x, y))
                {
                    case 0:
                        return 0;
                    case -1:
                        return 1;
                    case 1:
                        return -1;
                }
            }

            return TimeSpan.Compare(x, y);
        }
    }
}
