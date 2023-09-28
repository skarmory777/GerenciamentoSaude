using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto
{
    [AutoMap(typeof(PrescricaoItem))]
    public class PrescricaoItemDto : CamposPadraoCRUDDto
    {
        public long? DivisaoId { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsAlertaDuplicidade { get; set; }

        public bool IsExigeJustificativa { get; set; }

        public string Justificativa { get; set; }

        public long? TipoPrescricaoId { get; set; }

        public long? TipoControleId { get; set; }

        public bool IsAlteraQuantidade { get; set; }

        public long TotalDias { get; set; }

        public decimal? Quantidade { get; set; }

        public long? UnidadeId { get; set; }

        public long? FormaAplicacaoId { get; set; }

        public long? FrequenciaId { get; set; }

        public long? VelocidadeInfusaoId { get; set; }

        public long? UnidadeRequisicaoId { get; set; }

        public long? ProdutoId { get; set; }
        public ProdutoDto Produto { get; set; }

        public long? FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }

        public long? EstoqueId { get; set; }
        public EstoqueDto Estoque { get; set; }

        public DivisaoDto Divisao { get; set; }

        public TipoPrescricaoDto TipoPrescricao { get; set; }

        public TipoControleDto TipoControle { get; set; }

        public UnidadeDto Unidade { get; set; }

        public FormaAplicacaoDto FormaAplicacao { get; set; }

        public FrequenciaDto Frequencia { get; set; }

        public VelocidadeInfusaoDto VelocidadeInfusao { get; set; }

        public UnidadeDto UnidadeRequisicao { get; set; }

        public string FormulaEstoqueList { get; set; }

        public string FormulaFaturamentoList { get; set; }

        public string FormulaExameImagemList { get; set; }

        public string FormulaExameLaboratorialList { get; set; }

        public string FormulaEstoqueKitJson { get; set; }

        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }
        
        public bool IsDiluente { get; set; }
        
        public long? PrescricaoItemId { get; set; }
        
        public bool HasParent { get; set; }

        public bool IsControleDosagem { get; set; }
        public decimal? MinimoAceitavel { get; set; }
        public decimal? MaximoAceitavel { get; set; }
        public decimal? MinimoBloqueio { get; set; }
        public decimal? MaximoBloqueio { get; set; }

        public PrescricaoItemDto PrescricaoItemParent { get; set; }

        public IList<ConfiguracaoPrescricaoItemDto> ConfiguracaoPrescricaoItems { get; set; }

        public static PrescricaoItemDto Mapear(PrescricaoItem input)
        {
            if (input == null)
            {
                return null;
            }

            var result = CamposPadraoCRUDDto.MapearBase<PrescricaoItemDto>(input);

            result.IsAlertaDuplicidade = input.IsAlertaDuplicidade;
            result.IsAlteraQuantidade = input.IsAlteraQuantidade;
            result.IsAtivo = input.IsAtivo;
            result.IsExigeJustificativa = input.IsExigeJustificativa;
            result.Justificativa = input.Justificativa;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Quantidade = input.Quantidade;
            result.TotalDias = input.TotalDias;

            result.UnidadeId = input.UnidadeId;
            result.UnidadeRequisicaoId = input.UnidadeRequisicaoId;
            result.VelocidadeInfusaoId = input.VelocidadeInfusaoId;
            result.DivisaoId = input.DivisaoId;
            result.EstoqueId = input.EstoqueId;
            result.FaturamentoItemId = input.FaturamentoItemId;
            result.FormaAplicacaoId = input.FormaAplicacaoId;
            result.FrequenciaId = input.FrequenciaId;
            result.ProdutoId = input.ProdutoId;
            result.TipoControleId = input.TipoControleId;
            result.TipoPrescricaoId = input.TipoPrescricaoId;

            result.Unidade = UnidadeDto.Mapear(input.Unidade);
            result.UnidadeRequisicao = UnidadeDto.Mapear(input.UnidadeRequisicao);
            result.VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(input.VelocidadeInfusao);
            result.Divisao = DivisaoDto.Mapear(input.Divisao);
            result.Estoque = EstoqueDto.Mapear(input.Estoque);
            result.FaturamentoItem = FaturamentoItemDto.Mapear(input.FaturamentoItem);
            result.FormaAplicacao = FormaAplicacaoDto.Mapear(input.FormaAplicacao);
            result.Frequencia = FrequenciaDto.Mapear(input.Frequencia);
            result.Produto = ProdutoDto.Mapear(input.Produto);
            result.TipoControle = MapearBase<TipoControleDto>(input.TipoControle);
            result.TipoPrescricao = TipoPrescricaoDto.Mapear(input.TipoPrescricao);

            result.IsNegrito = input.IsNegrito;
            result.IsItalico = input.IsItalico;
            result.IsDiluente = input.IsDiluente;

            result.IsControleDosagem =input.IsControleDosagem;
            result.MinimoBloqueio = input.MinimoBloqueio;
            result.MinimoAceitavel = input.MinimoAceitavel;
            result.MaximoBloqueio = input.MaximoBloqueio;
            result.MaximoAceitavel = input.MaximoAceitavel;

            result.ConfiguracaoPrescricaoItems = ConfiguracaoPrescricaoItemDto.MapearLista(input.ConfiguracaoPrescricaoItems);

            result.PrescricaoItemId = input.PrescricaoItemId;
            if (input.PrescricaoItemParent != null)
            {
                result.PrescricaoItemParent = Mapear(input.PrescricaoItemParent);
            }
            return result;
        }

        public static PrescricaoItem Mapear(PrescricaoItemDto input)
        {
            if (input == null)
            {
                return null;
            }
            
            var result = CamposPadraoCRUDDto.MapearBase<PrescricaoItem>(input);
            result.DivisaoId = input.DivisaoId;
            result.EstoqueId = input.EstoqueId;
            result.FaturamentoItemId = input.FaturamentoItemId;
            result.FormaAplicacaoId = input.FormaAplicacaoId;
            result.FrequenciaId = input.FrequenciaId;
            result.IsAlertaDuplicidade = input.IsAlertaDuplicidade;
            result.IsAlteraQuantidade = input.IsAlteraQuantidade;
            result.IsAtivo = input.IsAtivo;
            result.IsExigeJustificativa = input.IsExigeJustificativa;
            result.Justificativa = input.Justificativa;
            result.ProdutoId = input.ProdutoId;
            result.Quantidade = input.Quantidade;
            result.TipoControleId = input.TipoControleId;
            result.TipoPrescricaoId = input.TipoPrescricaoId;
            result.TotalDias = input.TotalDias;
            result.UnidadeId = input.UnidadeId;
            result.UnidadeRequisicaoId = input.UnidadeRequisicaoId;
            result.VelocidadeInfusaoId = input.VelocidadeInfusaoId;
            result.IsNegrito = input.IsNegrito;
            result.IsItalico = input.IsItalico;

            result.IsControleDosagem = input.IsControleDosagem;
            result.MinimoBloqueio = input.MinimoBloqueio;
            result.MinimoAceitavel = input.MinimoAceitavel;
            result.MaximoBloqueio = input.MaximoBloqueio;
            result.MaximoAceitavel = input.MaximoAceitavel;

            result.Unidade = UnidadeDto.Mapear(input.Unidade);
            result.UnidadeRequisicao = UnidadeDto.Mapear(input.UnidadeRequisicao);
            result.VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(input.VelocidadeInfusao);
            result.Divisao = DivisaoDto.Mapear(input.Divisao);
            result.Estoque = EstoqueDto.Mapear(input.Estoque);
            result.FaturamentoItem = FaturamentoItemDto.Mapear(input.FaturamentoItem);
            result.FormaAplicacao = FormaAplicacaoDto.Mapear(input.FormaAplicacao);
            result.Frequencia = FrequenciaDto.Mapear(input.Frequencia);
            result.Produto = ProdutoDto.Mapear(input.Produto);
            result.TipoControle = MapearBase<TipoControle>(input.TipoControle);
            result.TipoPrescricao = TipoPrescricaoDto.Mapear(input.TipoPrescricao);

            result.ConfiguracaoPrescricaoItems =
                ConfiguracaoPrescricaoItemDto.MapearLista(input.ConfiguracaoPrescricaoItems);
            
            result.PrescricaoItemId = input.PrescricaoItemId;
            if (input.PrescricaoItemParent != null)
            {
                result.PrescricaoItemParent = Mapear(input.PrescricaoItemParent);
            }

            return result;

        }

        public static IEnumerable<PrescricaoItemDto> Mapear(List<PrescricaoItem> input)
        {
            foreach (var item in input)
            {
                yield return Mapear(item);
            }

        }

        public static IEnumerable<PrescricaoItem> Mapear(List<PrescricaoItemDto> input)
        {
            foreach (var item in input)
            {
                yield return Mapear(item);
            }
        }
    }


    public class SubPrescricaoItemDto: CamposPadraoCRUDDto
    {
        public string Quantidade { get; set; }
        
        public string Unidade { get; set; }
        
        public string FormaDeAplicacao { get; set; }
        
        public string Frequencia { get; set; }
        
        public string ViaDeAplicacao { get; set; }
        
        public string Diluente { get; set; }
        
        public string Volume { get; set; }
        
        public string Observacao { get; set; }

        public static SubPrescricaoItemDto Mapear (PrescricaoItem item)
        {
            return CamposPadraoCRUDDto.MapearBase<SubPrescricaoItemDto>(item);
        }
        
        public static IEnumerable<SubPrescricaoItemDto> Mapear(List<PrescricaoItem> input)
        {
            foreach (var item in input)
            {
                yield return Mapear(item);
            }
        }
    }
}
