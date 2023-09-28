using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto
{
    [AutoMap(typeof(ResultadoLaudo))]
    public class ResultadoLaudoDto : CamposPadraoCRUDDto
    {
        public long? ResultadoExameId { get; set; }
        public long? ItemResultadoId { get; set; }
        public long? TabelaResultadoId { get; set; }
        public long? UnidadeId { get; set; }
        public long? UsuarioLaudoId { get; set; }
        public long? TipoResultadoId { get; set; }
        public double Numerico { get; set; }
        public DateTime? DataDigitadoLaudo { get; set; }
        public string VersaoAtual { get; set; }
        public bool IsInterface { get; set; }
        public long? RegistroArquivoId { get; set; }

        /// <summary>
        /// campos para exibição do evolução de resultados
        /// </summary>
        public string Resultado { get; set; }
        public string Referencia { get; set; }
        public string ItemDescricao { get; set; }
        public string ItemInfo { get; set; }
        public DateTime? DataColeta { get; set; }
        public bool IsAmbulatorioEmergencia { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsAtendimento { get; set; }
        public bool IsPaciente { get; set; }

        public int? SexoId { get; set; }

        public ResultadoExameDto ResultadoExame { get; set; }
        public ItemResultadoDto ItemResultado { get; set; }
        public TabelaResultadoDto TabelaResultado { get; set; }
        public LaboratorioUnidadeDto LaboratorioUnidade { get; set; }
        public TipoResultadoDto TipoResultado { get; set; }

        public int? CasaDecimal { get; set; }

        public string Formula { get; set; }
        public int? Ordem { get; set; }
        public int? OrdemRegistro { get; set; }

        public int? OrdemMapaResultado { get; set; }

        public decimal? MinimoAceitavelMasculino { get; set; }
        public decimal? MaximoAceitavelMasculino { get; set; }
        public decimal? MinimoMasculino { get; set; }
        public decimal? MaximoMasculino { get; set; }
        public decimal? NormalMasculino { get; set; }

        public decimal? MinimoAceitavelFeminino { get; set; }
        public decimal? MaximoAceitavelFeminino { get; set; }
        public decimal? MinimoFeminino { get; set; }
        public decimal? MaximoFeminino { get; set; }
        public decimal? NormalFeminino { get; set; }

        public int FormataOrdem { get; set; }


        #region Mapeamento
        public static ResultadoLaudoDto Mapear(ResultadoLaudo input)
        {
            var result = new ResultadoLaudoDto();

            result.Codigo = input.Codigo;
            result.CasaDecimal = input.CasaDecimal;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataDigitadoLaudo = input.DataDigitadoLaudo;
            result.Descricao = input.Descricao;
            result.Formula = input.Formula;
            result.Id = input.Id;
            result.IsInterface = input.IsInterface;
            result.IsSistema = input.IsSistema;
            result.ItemResultadoId = input.ItemResultadoId;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.MaximoAceitavelFeminino = input.MaximoAceitavelFeminino;
            result.MaximoAceitavelMasculino = input.MaximoAceitavelMasculino;
            result.MaximoFeminino = input.MaximoFeminino;
            result.MaximoMasculino = input.MaximoMasculino;
            result.MinimoAceitavelFeminino = input.MinimoAceitavelFeminino;
            result.MinimoAceitavelMasculino = input.MinimoAceitavelMasculino;
            result.MinimoFeminino = input.MinimoFeminino;
            result.MinimoMasculino = input.MinimoMasculino;
            result.NormalFeminino = input.NormalFeminino;
            result.NormalMasculino = input.NormalMasculino;
            result.Numerico = input.Numerico;
            result.Ordem = input.Ordem;
            result.OrdemRegistro = input.OrdemRegistro;
            result.Referencia = input.Referencia;
            result.Resultado = input.Resultado;
            result.ResultadoExameId = input.ResultadoExameId;
            result.TabelaResultadoId = input.TabelaResultadoId;
            result.TipoResultadoId = input.TipoResultadoId;
            result.UnidadeId = input.UnidadeId;
            result.UsuarioLaudoId = input.UsuarioLaudoId;
            result.VersaoAtual = input.VersaoAtual;

            if (input.ItemResultado != null)
            {
                result.ItemResultado = ItemResultadoDto.Mapear(input.ItemResultado);
            }

            if (input.ResultadoExame != null)
            {
                result.ResultadoExame = ResultadoExameDto.Mapear(input.ResultadoExame);
            }

            if (input.TabelaResultado != null)
            {
                result.TabelaResultado = TabelaResultadoDto.Mapear(input.TabelaResultado);
            }

            if (input.TipoResultado != null)
            {
                result.TipoResultado = TipoResultadoDto.Mapear(input.TipoResultado);
            }

            return result;
        }

        public static ResultadoLaudo Mapear(ResultadoLaudoDto input)
        {
            var result = new ResultadoLaudo();

            result.Codigo = input.Codigo;
            result.CasaDecimal = input.CasaDecimal;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataDigitadoLaudo = input.DataDigitadoLaudo;
            result.Descricao = input.Descricao;
            result.Formula = input.Formula;
            result.Id = input.Id;
            result.IsInterface = input.IsInterface;
            result.IsSistema = input.IsSistema;
            result.ItemResultadoId = input.ItemResultadoId;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.MaximoAceitavelFeminino = input.MaximoAceitavelFeminino;
            result.MaximoAceitavelMasculino = input.MaximoAceitavelMasculino;
            result.MaximoFeminino = input.MaximoFeminino;
            result.MaximoMasculino = input.MaximoMasculino;
            result.MinimoAceitavelFeminino = input.MinimoAceitavelFeminino;
            result.MinimoAceitavelMasculino = input.MinimoAceitavelMasculino;
            result.MinimoFeminino = input.MinimoFeminino;
            result.MinimoMasculino = input.MinimoMasculino;
            result.NormalFeminino = input.NormalFeminino;
            result.NormalMasculino = input.NormalMasculino;
            result.Numerico = input.Numerico;
            result.Ordem = input.Ordem;
            result.OrdemRegistro = input.OrdemRegistro;
            result.Referencia = input.Referencia;
            result.Resultado = input.Resultado;
            result.ResultadoExameId = input.ResultadoExameId;
            result.TabelaResultadoId = input.TabelaResultadoId;
            result.TipoResultadoId = input.TipoResultadoId;
            result.UnidadeId = input.UnidadeId;
            result.UsuarioLaudoId = input.UsuarioLaudoId;
            result.VersaoAtual = input.VersaoAtual;

            if (input.ItemResultado != null)
            {
                result.ItemResultado = ItemResultadoDto.Mapear(input.ItemResultado);
            }

            if (input.ResultadoExame != null)
            {
                result.ResultadoExame = ResultadoExameDto.Mapear(input.ResultadoExame);
            }

            if (input.TabelaResultado != null)
            {
                result.TabelaResultado = TabelaResultadoDto.Mapear(input.TabelaResultado);
            }

            if (input.TipoResultado != null)
            {
                result.TipoResultado = TipoResultadoDto.Mapear(input.TipoResultado);
            }

            return result;
        }

        public static IEnumerable<ResultadoLaudoDto> Mapear(List<ResultadoLaudo> list)
        {
            foreach (var input in list)
            {
                var result = new ResultadoLaudoDto();

                result.Codigo = input.Codigo;
                result.CasaDecimal = input.CasaDecimal;
                result.CreationTime = input.CreationTime;
                result.CreatorUserId = input.CreatorUserId;
                result.DataDigitadoLaudo = input.DataDigitadoLaudo;
                result.Descricao = input.Descricao;
                result.Formula = input.Formula;
                result.Id = input.Id;
                result.IsInterface = input.IsInterface;
                result.IsSistema = input.IsSistema;
                result.ItemResultadoId = input.ItemResultadoId;
                result.LastModificationTime = input.LastModificationTime;
                result.LastModifierUserId = input.LastModifierUserId;
                result.MaximoAceitavelFeminino = input.MaximoAceitavelFeminino;
                result.MaximoAceitavelMasculino = input.MaximoAceitavelMasculino;
                result.MaximoFeminino = input.MaximoFeminino;
                result.MaximoMasculino = input.MaximoMasculino;
                result.MinimoAceitavelFeminino = input.MinimoAceitavelFeminino;
                result.MinimoAceitavelMasculino = input.MinimoAceitavelMasculino;
                result.MinimoFeminino = input.MinimoFeminino;
                result.MinimoMasculino = input.MinimoMasculino;
                result.NormalFeminino = input.NormalFeminino;
                result.NormalMasculino = input.NormalMasculino;
                result.Numerico = input.Numerico;
                result.Ordem = input.Ordem;
                result.OrdemRegistro = input.OrdemRegistro;
                result.Referencia = input.Referencia;
                result.Resultado = input.Resultado;
                result.ResultadoExameId = input.ResultadoExameId;
                result.TabelaResultadoId = input.TabelaResultadoId;
                result.TipoResultadoId = input.TipoResultadoId;
                result.UnidadeId = input.UnidadeId;
                result.UsuarioLaudoId = input.UsuarioLaudoId;
                result.VersaoAtual = input.VersaoAtual;

                if (input.ItemResultado != null)
                {
                    result.ItemResultado = ItemResultadoDto.Mapear(input.ItemResultado);
                }

                if (input.ResultadoExame != null)
                {
                    result.ResultadoExame = ResultadoExameDto.Mapear(input.ResultadoExame);
                }

                if (input.TabelaResultado != null)
                {
                    result.TabelaResultado = TabelaResultadoDto.Mapear(input.TabelaResultado);
                }

                if (input.TipoResultado != null)
                {
                    result.TipoResultado = TipoResultadoDto.Mapear(input.TipoResultado);
                }

                yield return result;
            }
        }

        public static IEnumerable<ResultadoLaudo> Mapear(List<ResultadoLaudoDto> list)
        {
            foreach (var input in list)
            {
                var result = new ResultadoLaudo();

                result.Codigo = input.Codigo;
                result.CasaDecimal = input.CasaDecimal;
                result.CreationTime = input.CreationTime;
                result.CreatorUserId = input.CreatorUserId;
                result.DataDigitadoLaudo = input.DataDigitadoLaudo;
                result.Descricao = input.Descricao;
                result.Formula = input.Formula;
                result.Id = input.Id;
                result.IsInterface = input.IsInterface;
                result.IsSistema = input.IsSistema;
                result.ItemResultadoId = input.ItemResultadoId;
                result.LastModificationTime = input.LastModificationTime;
                result.LastModifierUserId = input.LastModifierUserId;
                result.MaximoAceitavelFeminino = input.MaximoAceitavelFeminino;
                result.MaximoAceitavelMasculino = input.MaximoAceitavelMasculino;
                result.MaximoFeminino = input.MaximoFeminino;
                result.MaximoMasculino = input.MaximoMasculino;
                result.MinimoAceitavelFeminino = input.MinimoAceitavelFeminino;
                result.MinimoAceitavelMasculino = input.MinimoAceitavelMasculino;
                result.MinimoFeminino = input.MinimoFeminino;
                result.MinimoMasculino = input.MinimoMasculino;
                result.NormalFeminino = input.NormalFeminino;
                result.NormalMasculino = input.NormalMasculino;
                result.Numerico = input.Numerico;
                result.Ordem = input.Ordem;
                result.OrdemRegistro = input.OrdemRegistro;
                result.Referencia = input.Referencia;
                result.Resultado = input.Resultado;
                result.ResultadoExameId = input.ResultadoExameId;
                result.TabelaResultadoId = input.TabelaResultadoId;
                result.TipoResultadoId = input.TipoResultadoId;
                result.UnidadeId = input.UnidadeId;
                result.UsuarioLaudoId = input.UsuarioLaudoId;
                result.VersaoAtual = input.VersaoAtual;

                if (input.ItemResultado != null)
                {
                    result.ItemResultado = ItemResultadoDto.Mapear(input.ItemResultado);
                }

                if (input.ResultadoExame != null)
                {
                    result.ResultadoExame = ResultadoExameDto.Mapear(input.ResultadoExame);
                }

                if (input.TabelaResultado != null)
                {
                    result.TabelaResultado = TabelaResultadoDto.Mapear(input.TabelaResultado);
                }

                if (input.TipoResultado != null)
                {
                    result.TipoResultado = TipoResultadoDto.Mapear(input.TipoResultado);
                }

                yield return result;
            }
        }

        #endregion
    }
}
