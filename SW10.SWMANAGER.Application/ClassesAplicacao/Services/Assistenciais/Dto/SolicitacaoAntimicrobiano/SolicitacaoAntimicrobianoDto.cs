using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using System;
using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class SolicitacaoAntimicrobianoDto : CamposPadraoCRUDDto
    {
        public long? AtendimentoId { get; set; }

        public long? MedicoId { get; set; }

        public long? PrescricaoItemId { get; set; }
        
        public long? FrequenciaId { get; set; }
        
        public FrequenciaDto Frequencia { get; set; }
        
        public long? UnidadeId { get; set; }
        
        public UnidadeDto Unidade { get; set; }
        
        public long? FormaAplicacaoId { get; set; }
        
        public FormaAplicacaoDto FormaAplicacao { get; set; }
        
        public long? VelocidadeInfusaoId { get; set; }
        
        public VelocidadeInfusaoDto VelocidadeInfusao { get; set; }
        
        public decimal? Qtd { get; set; }

        public AtendimentoDto Atendimento { get; set; }

        public MedicoDto Medico { get; set; }

        public PrescricaoItemDto PrescricaoItem { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public DateTime DataMaximaTempoProvavel { get; set; }

        public int TempoProvavelUso { get; set; }

        public string TipoCultura { get; set; }

        public string TipoInfeccao { get; set; }

        public string OutrasIndicacoes { get; set; }

        public string OutrosResultados { get; set; }

        public long? PrescricaoItemRespostaId { get; set; }

        public ICollection<SolicitacaoAntimicrobianosIndicacaoDto> SolicitacaoAntimicrobianosIndicacoes { get; set; }

        public ICollection<SolicitacaoAntimicrobianosCulturaDto> SolicitacaoAntimicrobianosCulturas { get; set; }

        public static SolicitacaoAntimicrobiano MapearDtoParaEntidade(SolicitacaoAntimicrobianoDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<SolicitacaoAntimicrobiano>(dto);
            entity.AtendimentoId = dto.AtendimentoId;
            entity.MedicoId = dto.MedicoId;
            entity.PrescricaoItemId = dto.PrescricaoItemId;
            entity.DataSolicitacao = dto.DataSolicitacao;
            entity.DataMaximaTempoProvavel = dto.DataMaximaTempoProvavel;
            entity.TempoProvavelUso = dto.TempoProvavelUso;
            entity.TipoInfeccao = dto.TipoInfeccao;
            entity.OutrasIndicacoes = dto.OutrasIndicacoes;
            entity.OutrosResultados = dto.OutrosResultados;
            entity.TipoCultura = dto.TipoCultura;
            entity.FrequenciaId = dto.FrequenciaId;
            entity.VelocidadeInfusaoId = dto.VelocidadeInfusaoId;
            entity.FormaAplicacaoId = dto.FormaAplicacaoId;
            entity.UnidadeId = dto.UnidadeId;
            
            entity.Qtd = dto.Qtd;
            entity.PrescricaoItemRespostaId = dto.PrescricaoItemRespostaId;

            entity.SolicitacaoAntimicrobianosIndicacoes = new List<SolicitacaoAntimicrobianosIndicacao>();
            if (!dto.SolicitacaoAntimicrobianosIndicacoes.IsNullOrEmpty())
            {
                foreach (var indicacaoDto in dto.SolicitacaoAntimicrobianosIndicacoes)
                {
                    entity.SolicitacaoAntimicrobianosIndicacoes.Add(new SolicitacaoAntimicrobianosIndicacao
                    {
                        TipoSolicitacaoAntimicrobianosIndicacaoId = indicacaoDto.TipoSolicitacaoAntimicrobianosIndicacaoId,
                        SolicitacaoAntimicrobianoId = indicacaoDto.SolicitacaoAntimicrobianoId,
                        TipoIndicacao = MapearBase<TipoSolicitacaoAntimicrobianosIndicacao>(indicacaoDto.TipoIndicacao)
                    });
                }
            }

            entity.SolicitacaoAntimicrobianosCulturas = new List<SolicitacaoAntimicrobianosCulturas>();
            if (!dto.SolicitacaoAntimicrobianosCulturas.IsNullOrEmpty())
            {
                foreach (var culturaDto in dto.SolicitacaoAntimicrobianosCulturas)
                {
                    var cultura = new SolicitacaoAntimicrobianosCulturas
                    {
                        StatusResultado = culturaDto.StatusResultado,
                        SolicitacaoAntimicrobianoId = culturaDto.SolicitacaoAntimicrobianoId,
                        TipoId = culturaDto.TipoId,
                        Tipo = MapearBase<TipoSolicitacaoAntimicrobianosCultura>(culturaDto.Tipo),
                        DataCultura = culturaDto.DataCultura,
                        OutrosResultados =  culturaDto.OutrosResultados
                    };

                    if (cultura.SolicitacaoAntimicrobianosResultados.IsNullOrEmpty())
                    {
                        cultura.SolicitacaoAntimicrobianosResultados = new List<SolicitacaoAntimicrobianosResultados>();
                    }

                    if (!culturaDto.SolicitacaoAntimicrobianosResultados.IsNullOrEmpty())
                    {
                        foreach (var resultadoDto in culturaDto.SolicitacaoAntimicrobianosResultados)
                        {
                            cultura.SolicitacaoAntimicrobianosResultados.Add(new SolicitacaoAntimicrobianosResultados
                            {
                                
                                CulturaId = resultadoDto.CulturaId,
                                TipoSolicitacaoAntimicrobianosResultadoId = resultadoDto.TipoSolicitacaoAntimicrobianosResultadoId,
                                TipoResultado = MapearBase<TipoSolicitacaoAntimicrobianosResultado>(resultadoDto.TipoResultado),
                                Valor = resultadoDto.Valor,
                            });
                        }
                    }

                    entity.SolicitacaoAntimicrobianosCulturas.Add(cultura);
                }
            }

            return entity;
        }

        public static SolicitacaoAntimicrobianoDto MapearEntidadeParaDto(SolicitacaoAntimicrobiano entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<SolicitacaoAntimicrobianoDto>(entity);
            dto.AtendimentoId = entity.AtendimentoId;
            dto.MedicoId = entity.MedicoId;
            dto.PrescricaoItemId = entity.PrescricaoItemId;
            dto.DataSolicitacao = entity.DataSolicitacao;
            dto.DataMaximaTempoProvavel = entity.DataMaximaTempoProvavel;
            dto.TempoProvavelUso = entity.TempoProvavelUso;
            dto.TipoInfeccao = entity.TipoInfeccao;
            dto.OutrasIndicacoes = entity.OutrasIndicacoes;
            dto.OutrosResultados = entity.OutrosResultados;
            dto.TipoCultura = entity.TipoCultura;
            dto.FrequenciaId = entity.FrequenciaId;
            dto.VelocidadeInfusaoId = entity.VelocidadeInfusaoId;
            dto.FormaAplicacaoId = entity.FormaAplicacaoId;
            dto.UnidadeId = entity.UnidadeId;
            dto.Qtd = dto.Qtd;
            dto.PrescricaoItemRespostaId = entity.PrescricaoItemRespostaId;

            dto.SolicitacaoAntimicrobianosIndicacoes = new List<SolicitacaoAntimicrobianosIndicacaoDto>();
            if (!entity.SolicitacaoAntimicrobianosIndicacoes.IsNullOrEmpty())
            {
                foreach (var indicacao in entity.SolicitacaoAntimicrobianosIndicacoes)
                {
                    dto.SolicitacaoAntimicrobianosIndicacoes.Add(new SolicitacaoAntimicrobianosIndicacaoDto
                    {
                        TipoSolicitacaoAntimicrobianosIndicacaoId = indicacao.TipoSolicitacaoAntimicrobianosIndicacaoId,
                        SolicitacaoAntimicrobianoId = indicacao.SolicitacaoAntimicrobianoId,
                        TipoIndicacao = MapearBase<TipoSolicitacaoAntimicrobianosIndicacaoDto>(indicacao.TipoIndicacao)
                    });
                }
            }

            dto.SolicitacaoAntimicrobianosCulturas = new List<SolicitacaoAntimicrobianosCulturaDto>();
            if (!entity.SolicitacaoAntimicrobianosCulturas.IsNullOrEmpty())
            {
                foreach (var cultura in entity.SolicitacaoAntimicrobianosCulturas)
                {
                    var culturaDto = new SolicitacaoAntimicrobianosCulturaDto
                    {
                        StatusResultado = cultura.StatusResultado,
                        SolicitacaoAntimicrobianoId = cultura.SolicitacaoAntimicrobianoId,
                        TipoId = cultura.TipoId,
                        Tipo = MapearBase<TipoSolicitacaoAntimicrobianosCulturaDto>(cultura.Tipo),
                        DataCultura = cultura.DataCultura,
                        OutrosResultados =  cultura.OutrosResultados
                    };

                    if (culturaDto.SolicitacaoAntimicrobianosResultados.IsNullOrEmpty())
                    {
                        culturaDto.SolicitacaoAntimicrobianosResultados = new List<SolicitacaoAntimicrobianosResultadoDto>();
                    }

                    foreach (var resultado in cultura.SolicitacaoAntimicrobianosResultados)
                    {
                        culturaDto.SolicitacaoAntimicrobianosResultados.Add(new SolicitacaoAntimicrobianosResultadoDto
                        {
                            CulturaId = resultado.CulturaId,
                            TipoSolicitacaoAntimicrobianosResultadoId = resultado.TipoSolicitacaoAntimicrobianosResultadoId,
                            TipoResultado = MapearBase<TipoSolicitacaoAntimicrobianosResultadoDto>(resultado.TipoResultado),
                            Valor = resultado.Valor,
                        });
                    }

                    dto.SolicitacaoAntimicrobianosCulturas.Add(culturaDto);
                }
            }

            if (entity.Atendimento != null)
            {
                dto.Atendimento = AtendimentoDto.Mapear(entity.Atendimento);
            }

            if (entity.Medico != null)
            {
                dto.Medico = MedicoDto.Mapear(entity.Medico);
            }

            if (entity.PrescricaoItem != null)
            {
                dto.PrescricaoItem = PrescricaoItemDto.Mapear(entity.PrescricaoItem);
            }

            if (entity.Frequencia != null)
            {
                dto.Frequencia = FrequenciaDto.Mapear(entity.Frequencia);
            }
            
            if (entity.Unidade != null)
            {
                dto.Unidade = UnidadeDto.Mapear(entity.Unidade);
            }
            
            if (entity.FormaAplicacao != null)
            {
                dto.FormaAplicacao = FormaAplicacaoDto.Mapear(entity.FormaAplicacao);
            }
            
            if (entity.VelocidadeInfusao != null)
            {
                dto.VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(entity.VelocidadeInfusao);
            }

            return dto;
        }
    }

    public class SolicitacaoAntimicrobianoListDto
    {
        public List<SolicitacaoAntimicrobianoDto> SolicitacaoAntimicrobianos { get; set; }
    }

    public class ResultSolicitacaoAntimicrobianoDto
    {
        public bool Successo { get; set; }
        public List<long> Ids { get; set; } = new List<long>();
    }
}
