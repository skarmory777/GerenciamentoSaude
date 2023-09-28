using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;
using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto
{
    [AutoMap(typeof(ResultadoExame))]
    public class ResultadoExameDto : CamposPadraoCRUDDto
    {
        public long? FormataId { get; set; }//int] NULL,
        public long? FaturamentoItemId { get; set; }//int] NULL,
        public long? ResultadoId { get; set; }//int] NULL,
        public long? FaturamentoContaItemId { get; set; }//int] NULL,
        public long? KitExameId { get; set; }//int] NULL,
        public long? UsuarioIncluidoExameId { get; set; }//int] NULL,
        public long? UsuarioConferidoExameId { get; set; }//int] NULL,
        public long? UsuarioDigitadoExameId { get; set; }//int] NULL,
        public long? UsuarioPendenteExameId { get; set; }//int] NULL,
        public long? UsuarioImpressoExameId { get; set; }//int] NULL,
        public long? UsuarioCienteExameId { get; set; }//int] NULL,
        public long? UsuarioImpSolicitaId { get; set; }//int] NULL,
        public long? UsuarioAlteradoExameId { get; set; }//int] NULL,
        public long? MaterialId { get; set; }//int] NULL,
        public long? TabelaId { get; set; }

        public FormataDto Formata { get; set; }
        public User UsuarioIncluidoExame { get; set; }
        public User UsuarioConferidoExame { get; set; }
        public User UsuarioDigitadoExame { get; set; }
        public User UsuarioPendenteExame { get; set; }
        public User UsuarioImpressoExame { get; set; }
        public User UsuarioCienteExame { get; set; }
        public User UsuarioImpSolicita { get; set; }
        public User UsuarioAlteradoExame { get; set; }
        // public ExameDto Exame { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public ResultadoDto Resultado { get; set; }
        public FaturamentoContaItemDto FaturamentoContaItem { get; set; }
        public KitExameDto KitExame { get; set; }
        public MaterialDto Material { get; set; }
        public TabelaDto Tabela { get; set; }

        public bool IsImprime { get; set; }//dbo].[TBitControl] NULL,
        public bool IsSigiloso { get; set; }//dbo].[TBitControl] NULL,
        public bool IsSergioFranco { get; set; }//dbo].[TBitControl] NOT NULL,
        public bool IsCienteExame { get; set; }//dbo].[TBitControl] NOT NULL,

        public int Seq { get; set; }//int] NULL,
        public DateTime? DataInclusao { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataAlteracao { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataExclusao { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataConferidoExame { get; set; }//datetime] NULL,
        public DateTime? DataDigitadoExame { get; set; }//datetime] NULL,        
        public DateTime? DataPendenteExame { get; set; }//datetime] NULL,
        public DateTime? DataImpressoExame { get; set; }//datetime] NULL,
        public DateTime? DataImporta { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataUsuarioCienteExame { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataImpSolicita { get; set; }//datetime] NULL,
        public DateTime? DataAlteradoExame { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataEnvioEmail { get; set; }
        public DateTime? DataColetaBaixa { get; set; }
        
        public long? UsuarioColetaBaixaId { get; set; }
        public User UsuarioColetaBaixa { get; set; }
        
        public string ImpResultado { get; set; }//text] NULL,        
        public int? Quantidade { get; set; }//int] NULL,        
        public string MotivoPendenteExame { get; set; }//varchar](200) NULL,
        public string Observacao { get; set; }//varchar](1000) NULL,
        public string MaqImpSolicita { get; set; }//varchar](100) NULL,
        public string VolumeMaterial { get; set; }//varchar](4) NULL,
        public string MaterialDescricaoLocal { get; set; }
        public string Mneumonico { get; set; }
        public long? ExameStatusId { get; set; }
        
        public ExameStatusDto ExameStatus { get; set; }
        
        
        public long? AutorizacaoProcedimentoItemId { get; set; }
        
        public long? SolicitacaoExameId { get; set; }
        
        public SolicitacaoExameItemDto SolicitacaoExameItem { get; set; }

        public AutorizacaoProcedimentoItemDto AutorizacaoProcedimento { get; set; }
        
        public TecnicoDto TecnicoColeta { get; set; }
        public long? TecnicoColetaId { get; set; }
        public User CreatorUser { get; set; }
        public bool IsPendencia { get; set; }
        public DateTime? PendenciaDateTime { get; set; }
        
        public string MotivoPendencia { get; set; }

        public long? PendenciaUserId { get; set; }
        
        public User PendenciaUser { get; set; }

        #region Mapeamento
        public static ResultadoExame Mapear(ResultadoExameDto resultadoExameDto)
        {
            var resultadoExame = new ResultadoExame();

            resultadoExame.Id = resultadoExameDto.Id;
            resultadoExame.Codigo = resultadoExameDto.Codigo;
            resultadoExame.Descricao = resultadoExameDto.Descricao;
            resultadoExame.FormataId = resultadoExameDto.FormataId;
            resultadoExame.FaturamentoItemId = resultadoExameDto.FaturamentoItemId;
            resultadoExame.ResultadoId = resultadoExameDto.ResultadoId;
            resultadoExame.FaturamentoContaItemId = resultadoExameDto.FaturamentoContaItemId;
            resultadoExame.KitExameId = resultadoExameDto.KitExameId;
            resultadoExame.UsuarioIncluidoExameId = resultadoExameDto.UsuarioIncluidoExameId;
            resultadoExame.UsuarioConferidoExameId = resultadoExameDto.UsuarioConferidoExameId;
            resultadoExame.UsuarioDigitadoExameId = resultadoExameDto.UsuarioDigitadoExameId;
            resultadoExame.UsuarioPendenteExameId = resultadoExameDto.UsuarioPendenteExameId;
            resultadoExame.UsuarioImpressoExameId = resultadoExameDto.UsuarioImpressoExameId;
            resultadoExame.UsuarioCienteExameId = resultadoExameDto.UsuarioCienteExameId;
            resultadoExame.UsuarioImpSolicitaId = resultadoExameDto.UsuarioImpSolicitaId;
            resultadoExame.UsuarioAlteradoExameId = resultadoExameDto.UsuarioAlteradoExameId;
            resultadoExame.MaterialId = resultadoExameDto.MaterialId;
            resultadoExame.TabelaId = resultadoExameDto.TabelaId;
            resultadoExame.Quantidade = resultadoExameDto.Quantidade;
            resultadoExame.MotivoPendenteExame = resultadoExameDto.MotivoPendenteExame;
            resultadoExame.Observacao = resultadoExameDto.Observacao;
            resultadoExame.MaqImpSolicita = resultadoExameDto.MaqImpSolicita;
            resultadoExame.VolumeMaterial = resultadoExameDto.VolumeMaterial;
            resultadoExame.MaterialDescricaoLocal = resultadoExameDto.MaterialDescricaoLocal;
            resultadoExame.Mneumonico = resultadoExameDto.Mneumonico;
            resultadoExame.AutorizacaoProcedimentoItemId = resultadoExameDto.AutorizacaoProcedimentoItemId;
            resultadoExame.TecnicoColetaId = resultadoExameDto.TecnicoColetaId;
            resultadoExame.DataColetaBaixa = resultadoExameDto.DataColetaBaixa;
            resultadoExame.UsuarioColetaBaixaId = resultadoExameDto.UsuarioColetaBaixaId;
            resultadoExame.PendenciaDateTime = resultadoExameDto.PendenciaDateTime;
            resultadoExame.IsPendencia = resultadoExameDto.IsPendencia;
            resultadoExame.MotivoPendencia = resultadoExameDto.MotivoPendencia;
            resultadoExame.PendenciaUserId = resultadoExameDto.PendenciaUserId;

            if (resultadoExameDto.Resultado != null)
            {
                resultadoExame.Resultado = ResultadoDto.Mapear(resultadoExameDto.Resultado);
            }

            if (resultadoExameDto.FaturamentoItem != null)
            {
                resultadoExame.FaturamentoItem = FaturamentoItemDto.Mapear(resultadoExameDto.FaturamentoItem);
            }

            return resultadoExame;
        }

       

        public static ResultadoExameDto Mapear(ResultadoExame resultadoExame)
        {
            var resultadoExameDto = new ResultadoExameDto();

            resultadoExameDto.Id = resultadoExame.Id;
            resultadoExameDto.Codigo = resultadoExame.Codigo;
            resultadoExameDto.Descricao = resultadoExame.Descricao;
            resultadoExameDto.FormataId = resultadoExame.FormataId;
            resultadoExameDto.FaturamentoItemId = resultadoExame.FaturamentoItemId;
            resultadoExameDto.ResultadoId = resultadoExame.ResultadoId;
            resultadoExameDto.FaturamentoContaItemId = resultadoExame.FaturamentoContaItemId;
            resultadoExameDto.KitExameId = resultadoExame.KitExameId;
            resultadoExameDto.UsuarioIncluidoExameId = resultadoExame.UsuarioIncluidoExameId;
            resultadoExameDto.UsuarioConferidoExameId = resultadoExame.UsuarioConferidoExameId;
            resultadoExameDto.UsuarioDigitadoExameId = resultadoExame.UsuarioDigitadoExameId;
            resultadoExameDto.UsuarioPendenteExameId = resultadoExame.UsuarioPendenteExameId;
            resultadoExameDto.UsuarioImpressoExameId = resultadoExame.UsuarioImpressoExameId;
            resultadoExameDto.UsuarioCienteExameId = resultadoExame.UsuarioCienteExameId;
            resultadoExameDto.UsuarioImpSolicitaId = resultadoExame.UsuarioImpSolicitaId;
            resultadoExameDto.UsuarioAlteradoExameId = resultadoExame.UsuarioAlteradoExameId;
            resultadoExameDto.MaterialId = resultadoExame.MaterialId;
            resultadoExameDto.TabelaId = resultadoExame.TabelaId;
            resultadoExameDto.Quantidade = resultadoExame.Quantidade;
            resultadoExameDto.MotivoPendenteExame = resultadoExame.MotivoPendenteExame;
            resultadoExameDto.Observacao = resultadoExame.Observacao;
            resultadoExameDto.MaqImpSolicita = resultadoExame.MaqImpSolicita;
            resultadoExameDto.VolumeMaterial = resultadoExame.VolumeMaterial;
            resultadoExameDto.MaterialDescricaoLocal = resultadoExame.MaterialDescricaoLocal;
            resultadoExameDto.Mneumonico = resultadoExame.Mneumonico;
            resultadoExameDto.AutorizacaoProcedimentoItemId = resultadoExame.AutorizacaoProcedimentoItemId;
            resultadoExameDto.TecnicoColetaId = resultadoExame.TecnicoColetaId;
            resultadoExameDto.DataColetaBaixa = resultadoExame.DataColetaBaixa;
            resultadoExameDto.UsuarioColetaBaixaId = resultadoExame.UsuarioColetaBaixaId;
            resultadoExameDto.PendenciaDateTime = resultadoExame.PendenciaDateTime;
            resultadoExameDto.IsPendencia = resultadoExame.IsPendencia;
            resultadoExameDto.MotivoPendencia = resultadoExame.MotivoPendencia;
            resultadoExameDto.PendenciaUserId = resultadoExame.PendenciaUserId;

            if (resultadoExame.Resultado != null)
            {
                resultadoExameDto.Resultado = ResultadoDto.Mapear(resultadoExame.Resultado);
            }

            if (resultadoExame.FaturamentoItem != null)
            {
                resultadoExameDto.FaturamentoItem = FaturamentoItemDto.Mapear(resultadoExame.FaturamentoItem);
            }

            if (resultadoExame.AutorizacaoProcedimentoItem != null)
            {
                resultadoExameDto.AutorizacaoProcedimento = AutorizacaoProcedimentoItemDto.Mapear(resultadoExame.AutorizacaoProcedimentoItem);
            }

            if (resultadoExame.Formata != null)
            {
                resultadoExameDto.Formata = FormataDto.Mapear(resultadoExame.Formata);
            }

            if (resultadoExame.FaturamentoContaItem != null)
            {
                resultadoExameDto.FaturamentoContaItem = FaturamentoContaItemDto.MapearFromCore(resultadoExame.FaturamentoContaItem);
            }

            if (resultadoExame.KitExame != null)
            {
                resultadoExameDto.KitExame = KitExameDto.Mapear(resultadoExame.KitExame);
            }

            if (resultadoExame.Material != null)
            {
                resultadoExameDto.Material = MaterialDto.Mapear(resultadoExame.Material);
            }

            if (resultadoExame.Tabela != null)
            {
                resultadoExameDto.Tabela = TabelaDto.Mapear(resultadoExame.Tabela);
            }

            if (resultadoExame.TecnicoColeta != null)
            {
                resultadoExameDto.TecnicoColeta = TecnicoDto.Mapear(resultadoExame.TecnicoColeta);
            }

            if (resultadoExame.ExameStatus != null)
            {
                resultadoExameDto.ExameStatus = ExameStatusDto.Mapear(resultadoExame.ExameStatus);
            }
            
            if (resultadoExame.UsuarioColetaBaixa != null)
            {
                resultadoExameDto.UsuarioColetaBaixa = resultadoExame.UsuarioColetaBaixa;
            }
            
            return resultadoExameDto;
        }

        

        public static IEnumerable<ResultadoExame> Mapear(List<ResultadoExameDto> resultadoExameDto)
        {
            foreach (var item in resultadoExameDto)
            {
                var resultadoExame = Mapear(item);

                yield return resultadoExame;
            }
        }

        public static IEnumerable<ResultadoExameDto> Mapear(List<ResultadoExame> resultadoExame)
        {
            foreach (var item in resultadoExame)
            {
                var resultadoExameDto = Mapear(item);

                yield return resultadoExameDto;
            }
        }
        #endregion

        #region Ocorrencias

        public static string OcorrenciaDarBaixaExame(ResultadoExame exame, string userName, string tecnicoColeta, DateTime data) => 
            $"Exame {exame.FaturamentoItem.Descricao} coletado por {tecnicoColeta}.  Cadastrado por {userName} às {data}. Observação: {exame.Observacao}. Local: {exame.MaterialDescricaoLocal} ";

        
        public static string OcorrenciaCriado(ResultadoExame exame, string userName, DateTime data) => 
            $"Exame {exame.FaturamentoItem.Descricao} criado por {userName} às {data} ";
        
        public static string OcorrenciaAlterado(ResultadoExame exame, string userName, DateTime data) => 
            $"Exame {exame.FaturamentoItem.Descricao} alterado por {userName} às {data} ";
        
        public static string OcorrenciaCriarPendencia(ResultadoExame exame, string userName, DateTime data) => 
            $"Exame {exame.FaturamentoItem.Descricao} adicionado pendencia {exame.MotivoPendencia} por {userName} às {data} ";
        
        public static string OcorrenciaResolverPendencia(ResultadoExame exame, string userName, DateTime data) => 
            $"Exame {exame.FaturamentoItem.Descricao}  pendência resolvida por {userName} às {data} ";
        
        public static string OcorrenciaVoltarStatus(ResultadoExame exame, string statusAnterior, string statusAtual, string userName, DateTime data) => 
            $"Exame {exame.FaturamentoItem.Descricao} status alterado de {statusAnterior} para {statusAtual} por {userName} às {data} ";
        
        public static string OcorrenciaColetaImpressa(ResultadoExame exame, string userName, DateTime data) => 
            $"Exame {exame.FaturamentoItem.Descricao}  etiqueta impressa por {userName} às {data} ";
        
        #endregion
    }
}
