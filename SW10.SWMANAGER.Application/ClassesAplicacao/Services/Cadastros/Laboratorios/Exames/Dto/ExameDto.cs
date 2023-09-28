using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto
{
    //  [AutoMap(typeof(Exame))]
    public class ExameDto : CamposPadraoCRUDDto
    {

        #region Exame

        public bool IsExameSimples { get; set; }
        public bool IsPeso { get; set; }
        public bool IsTesta100 { get; set; }
        public bool IsAltura { get; set; }
        public bool IsCor { get; set; }
        public bool IsMestruacao { get; set; }
        public bool IsNacionalidade { get; set; }
        public bool IsNaturalidade { get; set; }
        public bool IsImpReferencia { get; set; }
        public bool IsCultura { get; set; }
        public bool IsPendente { get; set; }
        public bool IsRepete { get; set; }
        public bool IsLibera { get; set; }
        public string Mneumonico { get; set; }
        public int? OrdemImp { get; set; }
        public int? Prazo { get; set; }
        public byte[] Interpretacao { get; set; }
        public byte[] Extra1 { get; set; }
        public byte[] Extra2 { get; set; }
        public string InterpretacaoStr { get; set; }
        public string Extra1Str { get; set; }
        public string Extra2Str { get; set; }
        public int? QtdFatura { get; set; }
        public string MapaExame { get; set; }
        public int? OrdemResul { get; set; }
        public int? OrdemResumo { get; set; }
        public int? OrdemMapaResultado { get; set; }
        public long? EquipamentoId { get; set; }
        public long? ExameIncluiId { get; set; }
        public long? SetorId { get; set; }
        public long? MaterialId { get; set; }
        public long? MetodoId { get; set; }
        public long? UnidadeId { get; set; }
        public long? FormataId { get; set; }
        public long? MapaId { get; set; }

        public EquipamentoDto Equipamento { get; set; }

        public FaturamentoItemDto ExameInclui { get; set; }

        public SetorDto Setor { get; set; }

        public MaterialDto Material { get; set; }

        public MetodoDto Metodo { get; set; }

        public LaboratorioUnidadeDto Unidade { get; set; }

        public FormataDto Formata { get; set; }

        public MapaDto Mapa { get; set; }

        #endregion

        public static ExameDto Mapear(FaturamentoItem faturamentoItem)
        {
            ExameDto exameDto = new ExameDto();

            exameDto.Id = faturamentoItem.Id;
            exameDto.Codigo = faturamentoItem.Codigo;
            exameDto.Descricao = faturamentoItem.Descricao;
            exameDto.FormataId = faturamentoItem.FormataId;
            exameDto.QtdFatura = faturamentoItem.QtdFatura;
            exameDto.Mneumonico = faturamentoItem.Mneumonico;
            exameDto.IsExameSimples = faturamentoItem.IsExameSimples;
            exameDto.IsPeso = faturamentoItem.IsPeso;
            exameDto.IsTesta100 = faturamentoItem.IsTesta100;
            exameDto.IsAltura = faturamentoItem.IsAltura;
            exameDto.IsCor = faturamentoItem.IsCor;
            exameDto.IsMestruacao = faturamentoItem.IsMestruacao;
            exameDto.IsNacionalidade = faturamentoItem.IsNacionalidade;
            exameDto.IsNaturalidade = faturamentoItem.IsNaturalidade;
            exameDto.IsImpReferencia = faturamentoItem.IsImpReferencia;
            exameDto.IsCultura = faturamentoItem.IsCultura;
            exameDto.IsPendente = faturamentoItem.IsPendente;
            exameDto.IsRepete = faturamentoItem.IsRepete;
            exameDto.IsLibera = faturamentoItem.IsLibera;
            exameDto.OrdemImp = faturamentoItem.OrdemImp;
            exameDto.Prazo = faturamentoItem.Prazo;
            exameDto.Interpretacao = faturamentoItem.Interpretacao;
            exameDto.Extra1 = faturamentoItem.Extra1;
            exameDto.Extra2 = faturamentoItem.Extra2;
            exameDto.MapaExame = faturamentoItem.MapaExame;
            exameDto.OrdemResul = faturamentoItem.OrdemResul;
            exameDto.OrdemResumo = faturamentoItem.OrdemResumo;
            exameDto.OrdemMapaResultado = faturamentoItem.OrdemMapaResultado;
            exameDto.EquipamentoId = faturamentoItem.EquipamentoId;
            exameDto.ExameIncluiId = faturamentoItem.ExameIncluiId;
            exameDto.SetorId = faturamentoItem.SetorId;
            exameDto.MaterialId = faturamentoItem.MaterialId;
            exameDto.MetodoId = faturamentoItem.MetodoId;
            exameDto.UnidadeId = faturamentoItem.UnidadeId;
            exameDto.MapaId = faturamentoItem.MapaId;

            if (faturamentoItem.Formata != null)
            {
                exameDto.Formata = new FormataDto { Id = faturamentoItem.Formata.Id, Codigo = faturamentoItem.Formata.Codigo, Descricao = faturamentoItem.Formata.Descricao };
            }

            exameDto.MaterialId = faturamentoItem.MaterialId;

            if (faturamentoItem.Material != null)
            {
                exameDto.Material = new MaterialDto { Id = faturamentoItem.Material.Id, Codigo = faturamentoItem.Material.Codigo, Descricao = faturamentoItem.Material.Descricao };
            }

            if (faturamentoItem.Equipamento != null)
            {
                exameDto.Equipamento = new EquipamentoDto { Id = faturamentoItem.Equipamento.Id, Codigo = faturamentoItem.Equipamento.Codigo, Descricao = faturamentoItem.Equipamento.Descricao, DiretorioOrdem = faturamentoItem.Equipamento.DiretorioOrdem, DiretorioResultado = faturamentoItem.Equipamento.DiretorioResultado, TipoLayout = faturamentoItem.Equipamento.TipoLayout };
            }
            if (faturamentoItem.ExameInclui != null)
            {
                exameDto.ExameInclui = new FaturamentoItemDto
                {
                    Id = faturamentoItem.ExameInclui.Id,
                    Codigo = faturamentoItem.ExameInclui.Codigo,
                    Descricao = faturamentoItem.ExameInclui.Descricao,
                    EquipamentoId = faturamentoItem.ExameInclui.EquipamentoId,
                    ExameIncluiId = faturamentoItem.ExameInclui.ExameIncluiId,
                    Extra1 = faturamentoItem.ExameInclui.Extra1,
                    Extra2 = faturamentoItem.ExameInclui.Extra2,
                    FormataId = faturamentoItem.ExameInclui.FormataId,
                    Interpretacao = faturamentoItem.ExameInclui.Interpretacao,
                    IsAltura = faturamentoItem.ExameInclui.IsAltura,
                    IsCor = faturamentoItem.ExameInclui.IsCor,
                    IsCultura = faturamentoItem.ExameInclui.IsCultura,
                    IsExameSimples = faturamentoItem.ExameInclui.IsExameSimples,
                    IsImpReferencia = faturamentoItem.ExameInclui.IsImpReferencia,
                    IsLibera = faturamentoItem.ExameInclui.IsLibera,
                    IsMestruacao = faturamentoItem.ExameInclui.IsMestruacao,
                    IsNacionalidade = faturamentoItem.ExameInclui.IsNacionalidade,
                    IsNaturalidade = faturamentoItem.ExameInclui.IsNaturalidade,
                    IsPendente = faturamentoItem.ExameInclui.IsPendente,
                    IsPeso = faturamentoItem.ExameInclui.IsPeso,
                    IsTesta100 = faturamentoItem.ExameInclui.IsTesta100,
                    MapaExame = faturamentoItem.ExameInclui.MapaExame,
                    IsRepete = faturamentoItem.ExameInclui.IsRepete,
                    MapaId = faturamentoItem.ExameInclui.MapaId,
                    MaterialId = faturamentoItem.ExameInclui.MaterialId,
                    MetodoId = faturamentoItem.ExameInclui.MetodoId,
                    Mneumonico = faturamentoItem.ExameInclui.Mneumonico,
                    OrdemImp = faturamentoItem.ExameInclui.OrdemImp,
                    OrdemMapaResultado = faturamentoItem.ExameInclui.OrdemMapaResultado,
                    OrdemResul = faturamentoItem.ExameInclui.OrdemResul,
                    OrdemResumo = faturamentoItem.ExameInclui.OrdemResumo,
                    Prazo = faturamentoItem.ExameInclui.Prazo,
                    QtdFatura = faturamentoItem.ExameInclui.QtdFatura,
                    SetorId = faturamentoItem.ExameInclui.SetorId,
                    UnidadeId = faturamentoItem.ExameInclui.UnidadeId
                };
            }
            if (faturamentoItem.Setor != null)
            {
                exameDto.Setor = new SetorDto
                {
                    Codigo = faturamentoItem.Setor.Codigo,
                    Descricao = faturamentoItem.Setor.Descricao,
                    Id = faturamentoItem.Setor.Id,
                    OrdemSetor = faturamentoItem.Setor.OrdemSetor
                };
            }
            if (faturamentoItem.Metodo != null)
            {
                exameDto.Metodo = new MetodoDto
                {
                    Codigo = faturamentoItem.Metodo.Codigo,
                    Descricao = faturamentoItem.Metodo.Descricao,
                    Id = faturamentoItem.Metodo.Id
                };
            }
            if (faturamentoItem.Unidade != null)
            {
                exameDto.Unidade = new LaboratorioUnidadeDto
                {
                    Codigo = faturamentoItem.Unidade.Codigo,
                    Descricao = faturamentoItem.Unidade.Descricao,
                    Id = faturamentoItem.Unidade.Id
                };
            }
            if (faturamentoItem.Mapa != null)
            {
                exameDto.Mapa = new MapaDto
                {
                    Codigo = faturamentoItem.Mapa.Codigo,
                    Descricao = faturamentoItem.Mapa.Descricao,
                    Id = faturamentoItem.Mapa.Id,
                    Cabec2 = faturamentoItem.Mapa.Cabec2,
                    Cabec3 = faturamentoItem.Mapa.Cabec3,
                    Cabec4 = faturamentoItem.Mapa.Cabec4,
                    Cabec5 = faturamentoItem.Mapa.Cabec5,
                    Cabec1 = faturamentoItem.Mapa.Cabec1,
                    Cabec6 = faturamentoItem.Mapa.Cabec6,
                    Cabec7 = faturamentoItem.Mapa.Cabec7,
                    Cabec8 = faturamentoItem.Mapa.Cabec8,
                    Cabec9 = faturamentoItem.Mapa.Cabec9,
                    Cabec10 = faturamentoItem.Mapa.Cabec10,
                    Cabec11 = faturamentoItem.Mapa.Cabec11,
                    Cabec12 = faturamentoItem.Mapa.Cabec12,
                    Cabec13 = faturamentoItem.Mapa.Cabec13,
                    Cabec14 = faturamentoItem.Mapa.Cabec14,
                    Cabec15 = faturamentoItem.Mapa.Cabec15,
                    Cabec16 = faturamentoItem.Mapa.Cabec16,
                    Cabec17 = faturamentoItem.Mapa.Cabec17,
                    Cabec18 = faturamentoItem.Mapa.Cabec18,
                    Cabec19 = faturamentoItem.Mapa.Cabec19,
                    Cabec20 = faturamentoItem.Mapa.Cabec20,
                    Exame1ID = faturamentoItem.Mapa.Exame1ID,
                    Exame2ID = faturamentoItem.Mapa.Exame2ID,
                    Exame3ID = faturamentoItem.Mapa.Exame3ID,
                    Exame4ID = faturamentoItem.Mapa.Exame4ID,
                    Exame5ID = faturamentoItem.Mapa.Exame5ID,
                    Exame6ID = faturamentoItem.Mapa.Exame6ID,
                    Exame7ID = faturamentoItem.Mapa.Exame7ID,
                    Exame8ID = faturamentoItem.Mapa.Exame8ID,
                    Exame9ID = faturamentoItem.Mapa.Exame9ID,
                    Exame10ID = faturamentoItem.Mapa.Exame10ID,
                    Exame11ID = faturamentoItem.Mapa.Exame11ID,
                    Exame12ID = faturamentoItem.Mapa.Exame12ID,
                    Exame13ID = faturamentoItem.Mapa.Exame13ID,
                    Exame14ID = faturamentoItem.Mapa.Exame14ID,
                    Exame15ID = faturamentoItem.Mapa.Exame15ID,
                    Exame16ID = faturamentoItem.Mapa.Exame16ID,
                    Exame17ID = faturamentoItem.Mapa.Exame17ID,
                    Exame18ID = faturamentoItem.Mapa.Exame18ID,
                    Exame19ID = faturamentoItem.Mapa.Exame19ID,
                    Exame20ID = faturamentoItem.Mapa.Exame20ID,

                };
            }

            return exameDto;
        }

        public static List<ExameDto> Mapear(List<FaturamentoItem> faturamentosItens)
        {
            List<ExameDto> examesDto = new List<ExameDto>();

            foreach (var item in faturamentosItens)
            {
                examesDto.Add(ExameDto.Mapear(item));
            }

            return examesDto;
        }

    }
}
