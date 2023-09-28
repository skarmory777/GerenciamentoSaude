using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto
{
    [AutoMap(typeof(FaturamentoItem))]
    public class FaturamentoItemDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }

        public FaturamentoBrasItemDto BrasItem { get; set; }
        public long? BrasItemId { get; set; }

        public FaturamentoGrupoDto Grupo { get; set; }
        public long? GrupoId { get; set; }

        public FaturamentoSubGrupoDto SubGrupo { get; set; }
        public long? SubGrupoId { get; set; }

        public LaudoGrupoDto LaudoGrupo { get; set; }
        public long? LaudoGrupoId { get; set; }

        public string DescricaoTuss { get; set; }

        public string Observacao { get; set; }

        public string CodAmb { get; set; }

        public string CodTuss { get; set; }

        public string CodCbhpm { get; set; }

        public float DivideBrasindice { get; set; }

        public string Referencia { get; set; }

        public string ReferenciaSihSus { get; set; }

        public int Sexo { get; set; }

        public int QtdLaudo { get; set; }

        public int TipoLaudo { get; set; }

        public int DuracaoMinima { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsObrigaMedico { get; set; }

        public bool IsTaxaUrgencia { get; set; }

        public bool IsPediatria { get; set; }

        public bool IsProcedimentoSerie { get; set; }

        public bool IsRequisicaoExame { get; set; }

        public bool IsPermiteRevisao { get; set; }

        public bool IsPrecoManual { get; set; }

        public bool IsAutorizacao { get; set; }

        public bool IsInternacao { get; set; }

        public bool IsAmbulatorio { get; set; }

        public bool IsCirurgia { get; set; }

        public bool IsPorte { get; set; }

        public bool IsConsultor { get; set; }

        public bool IsLaboratorio { get; set; }

        public bool IsPlantonista { get; set; }

        public bool IsOpme { get; set; }

        public bool IsExtraCaixa { get; set; }

        public bool IsLaudo { get; set; }

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

        #region Agendamento

        public bool IsAgendaConsulta { get; set; }
        public bool IsAgendaCirurgia { get; set; }
        public bool IsAgendaExame { get; set; }
        public int QuantidadeMinutos { get; set; }
        public bool IsAgendaMaterial { get; set; }

        #endregion


        #region Mapeamento

        public static FaturamentoItemDto Mapear(FaturamentoItem input)
        {
            if (input == null)
            {
                return null;
            }

            FaturamentoItemDto result = new FaturamentoItemDto
            {
                GrupoId = input.GrupoId,
                SubGrupoId = input.SubGrupoId,
                CodTuss = input.CodTuss,
                DescricaoTuss = input.DescricaoTuss,
                Id = input.Id,
                Codigo = input.Codigo,
                Descricao = input.Descricao,
                EquipamentoId = input.EquipamentoId,
                ExameIncluiId = input.ExameIncluiId,
                Extra1 = input.Extra1,
                Extra2 = input.Extra2,
                FormataId = input.FormataId,
                Interpretacao = input.Interpretacao,

                IsAltura = input.IsAltura,
                IsCor = input.IsCor,
                IsCultura = input.IsCultura,
                IsExameSimples = input.IsExameSimples,
                IsImpReferencia = input.IsImpReferencia,
                IsLibera = input.IsLibera,
                IsMestruacao = input.IsMestruacao,
                IsNacionalidade = input.IsNacionalidade,
                IsNaturalidade = input.IsNaturalidade,
                IsPendente = input.IsPendente,
                IsPeso = input.IsPeso,
                IsTesta100 = input.IsTesta100,

                IsAmbulatorio = input.IsAmbulatorio,
                IsAtivo = input.IsAtivo,
                IsAutorizacao = input.IsAutorizacao,
                IsCirurgia = input.IsCirurgia,
                IsConsultor = input.IsConsultor,
                IsExtraCaixa = input.IsExtraCaixa,
                IsInternacao = input.IsInternacao,
                IsLaboratorio = input.IsLaboratorio,
                IsLaudo = input.IsLaudo,
                IsObrigaMedico = input.IsObrigaMedico,
                IsOpme = input.IsOpme,
                IsPediatria = input.IsPediatria,
                IsPermiteRevisao = input.IsPermiteRevisao,
                IsPlantonista = input.IsPlantonista,
                IsPorte = input.IsPorte,
                IsPrecoManual = input.IsPrecoManual,
                IsProcedimentoSerie = input.IsProcedimentoSerie,
                IsRepete = input.IsRepete,
                IsRequisicaoExame = input.IsRequisicaoExame,
                IsTaxaUrgencia = input.IsTaxaUrgencia,
                Referencia = input.Referencia,
                ReferenciaSihSus = input.ReferenciaSihSus,
                Observacao = input.Observacao
            };
            result.IsTesta100 = input.IsTesta100;
            result.LaudoGrupoId = input.LaudoGrupoId;

            result.MapaExame = input.MapaExame;
            result.IsRepete = input.IsRepete;
            result.MapaId = input.MapaId;
            result.MaterialId = input.MaterialId;
            result.MetodoId = input.MetodoId;
            result.Mneumonico = input.Mneumonico;
            result.OrdemImp = input.OrdemImp;
            result.OrdemMapaResultado = input.OrdemMapaResultado;
            result.OrdemResul = input.OrdemResul;
            result.OrdemResumo = input.OrdemResumo;
            result.Prazo = input.Prazo;
            result.QtdFatura = input.QtdFatura;
            result.SetorId = input.SetorId;
            result.UnidadeId = input.UnidadeId;

            result.IsAgendaConsulta = input.IsAgendaConsulta;
            result.IsAgendaCirurgia = input.IsAgendaCirurgia;
            result.IsAgendaExame = input.IsAgendaExame;
            result.QuantidadeMinutos = input.QuantidadeMinutos;
            result.IsAgendaMaterial = input.IsAgendaMaterial;

            if (input.Material != null)
            {
                result.Material = MaterialDto.Mapear(input.Material);
            }


            if (input.Grupo != null)
            {
                result.Grupo = FaturamentoGrupoDto.Mapear(input.Grupo);
            }

            if (input.SubGrupo != null)
            {
                result.SubGrupo = FaturamentoSubGrupoDto.Mapear(input.SubGrupo);
            }

            if (input.ExameInclui != null)
            {
                result.ExameInclui = new FaturamentoItemDto
                {
                    GrupoId = input.ExameInclui.GrupoId,
                    SubGrupoId = input.ExameInclui.SubGrupoId,
                    CodTuss = input.ExameInclui.CodTuss,
                    DescricaoTuss = input.ExameInclui.DescricaoTuss,
                    Id = input.ExameInclui.Id,
                    Codigo = input.ExameInclui.Codigo,
                    Descricao = input.ExameInclui.Descricao,
                    EquipamentoId = input.ExameInclui.EquipamentoId,
                    ExameIncluiId = input.ExameInclui.ExameIncluiId,
                    Extra1 = input.ExameInclui.Extra1,
                    Extra2 = input.ExameInclui.Extra2,
                    FormataId = input.ExameInclui.FormataId,
                    Interpretacao = input.ExameInclui.Interpretacao,
                    IsAltura = input.ExameInclui.IsAltura,
                    IsCor = input.ExameInclui.IsCor,
                    IsCultura = input.ExameInclui.IsCultura,
                    IsExameSimples = input.ExameInclui.IsExameSimples,
                    IsImpReferencia = input.ExameInclui.IsImpReferencia,
                    IsLibera = input.ExameInclui.IsLibera,
                    IsMestruacao = input.ExameInclui.IsMestruacao,
                    IsNacionalidade = input.ExameInclui.IsNacionalidade,
                    IsNaturalidade = input.ExameInclui.IsNaturalidade,
                    IsPendente = input.ExameInclui.IsPendente,
                    IsPeso = input.ExameInclui.IsPeso,
                    IsTesta100 = input.ExameInclui.IsTesta100,
                    IsAmbulatorio = input.ExameInclui.IsAmbulatorio,
                    IsAtivo = input.ExameInclui.IsAtivo,
                    IsAutorizacao = input.ExameInclui.IsAutorizacao,
                    IsCirurgia = input.ExameInclui.IsCirurgia,
                    IsConsultor = input.ExameInclui.IsConsultor,
                    IsExtraCaixa = input.ExameInclui.IsExtraCaixa,
                    IsInternacao = input.ExameInclui.IsInternacao,
                    IsLaboratorio = input.ExameInclui.IsLaboratorio,
                    IsLaudo = input.ExameInclui.IsLaudo,
                    IsObrigaMedico = input.ExameInclui.IsObrigaMedico,
                    IsOpme = input.ExameInclui.IsOpme,
                    IsPediatria = input.ExameInclui.IsPediatria,
                    IsPermiteRevisao = input.ExameInclui.IsPermiteRevisao,
                    IsPlantonista = input.ExameInclui.IsPlantonista,
                    IsPorte = input.ExameInclui.IsPorte,
                    IsPrecoManual = input.ExameInclui.IsPrecoManual,
                    IsProcedimentoSerie = input.ExameInclui.IsProcedimentoSerie,
                    IsRepete = input.ExameInclui.IsRepete,
                    IsRequisicaoExame = input.ExameInclui.IsRequisicaoExame,
                    IsTaxaUrgencia = input.ExameInclui.IsTaxaUrgencia,
                    LaudoGrupoId = input.ExameInclui.LaudoGrupoId,
                    MapaExame = input.ExameInclui.MapaExame,
                    MapaId = input.ExameInclui.MapaId,
                    MaterialId = input.ExameInclui.MaterialId,
                    MetodoId = input.ExameInclui.MetodoId,
                    Mneumonico = input.ExameInclui.Mneumonico,
                    OrdemImp = input.ExameInclui.OrdemImp,
                    OrdemMapaResultado = input.ExameInclui.OrdemMapaResultado,
                    OrdemResul = input.ExameInclui.OrdemResul,
                    OrdemResumo = input.ExameInclui.OrdemResumo,
                    Prazo = input.ExameInclui.Prazo,
                    QtdFatura = input.ExameInclui.QtdFatura,
                    SetorId = input.ExameInclui.SetorId,
                    UnidadeId = input.ExameInclui.UnidadeId
                };
            }

            if (input.Formata != null)
            {
                result.Formata = FormataDto.Mapear(input.Formata);
            }

            if (input.Material != null)
            {
                result.Material = MaterialDto.Mapear(input.Material);
            }

            if (input.Equipamento != null)
            {
                result.Equipamento = EquipamentoDto.Mapear(input.Equipamento);
            }

            if (input.Setor != null)
            {
                result.Setor = SetorDto.Mapear(input.Setor);
            }

            if (input.Metodo != null)
            {
                result.Metodo = MetodoDto.Mapear(input.Metodo);
            }

            if (input.Unidade != null)
            {
                result.Unidade = LaboratorioUnidadeDto.Mapear(input.Unidade);
            }

            if (input.Mapa != null)
            {
                result.Mapa = MapaDto.Mapear(input.Mapa);
            }

            return result;
        }

        public static FaturamentoItem Mapear(FaturamentoItemDto input)
        {
            if (input == null)
            {
                return null;
            }

            return new FaturamentoItem
            {
                GrupoId = input.GrupoId,
                SubGrupoId = input.SubGrupoId,
                CodTuss = input.CodTuss,
                DescricaoTuss = input.DescricaoTuss,
                Id = input.Id,
                Codigo = input.Codigo,
                Descricao = input.Descricao,
                EquipamentoId = input.EquipamentoId,
                ExameIncluiId = input.ExameIncluiId,
                Extra1 = input.Extra1,
                Extra2 = input.Extra2,
                FormataId = input.FormataId,
                Interpretacao = input.Interpretacao,
                IsAltura = input.IsAltura,
                IsCor = input.IsCor,
                IsCultura = input.IsCultura,
                IsExameSimples = input.IsExameSimples,
                IsImpReferencia = input.IsImpReferencia,
                IsLibera = input.IsLibera,
                IsMestruacao = input.IsMestruacao,
                IsNacionalidade = input.IsNacionalidade,
                IsNaturalidade = input.IsNaturalidade,
                IsPendente = input.IsPendente,
                IsPeso = input.IsPeso,
                IsTesta100 = input.IsTesta100,
                IsAmbulatorio = input.IsAmbulatorio,
                IsAtivo = input.IsAtivo,
                IsAutorizacao = input.IsAutorizacao,
                IsCirurgia = input.IsCirurgia,
                IsConsultor = input.IsConsultor,
                IsExtraCaixa = input.IsExtraCaixa,
                IsInternacao = input.IsInternacao,
                IsLaboratorio = input.IsLaboratorio,
                IsLaudo = input.IsLaudo,
                IsObrigaMedico = input.IsObrigaMedico,
                IsOpme = input.IsOpme,
                IsPediatria = input.IsPediatria,
                IsPermiteRevisao = input.IsPermiteRevisao,
                IsPlantonista = input.IsPlantonista,
                IsPorte = input.IsPorte,
                IsPrecoManual = input.IsPrecoManual,
                IsProcedimentoSerie = input.IsProcedimentoSerie,
                IsRepete = input.IsRepete,
                IsRequisicaoExame = input.IsRequisicaoExame,
                IsTaxaUrgencia = input.IsTaxaUrgencia,
                LaudoGrupoId = input.LaudoGrupoId,
                MapaExame = input.MapaExame,
                MapaId = input.MapaId,
                MaterialId = input.MaterialId,
                MetodoId = input.MetodoId,
                Mneumonico = input.Mneumonico,
                OrdemImp = input.OrdemImp,
                OrdemMapaResultado = input.OrdemMapaResultado,
                OrdemResul = input.OrdemResul,
                OrdemResumo = input.OrdemResumo,
                Prazo = input.Prazo,
                QtdFatura = input.QtdFatura,
                SetorId = input.SetorId,
                UnidadeId = input.UnidadeId,
                IsAgendaConsulta = input.IsAgendaConsulta,
                IsAgendaCirurgia = input.IsAgendaCirurgia,
                IsAgendaExame = input.IsAgendaExame,
                QuantidadeMinutos = input.QuantidadeMinutos,
                Referencia = input.Referencia,
                ReferenciaSihSus = input.ReferenciaSihSus,
                Observacao = input.Observacao
            };
        }

        public static IEnumerable<FaturamentoItemDto> Mapear(List<FaturamentoItem> faturamentoItem)
        {
            foreach (var item in faturamentoItem)
            {
                yield return Mapear(item);
            }
        }

        public static IEnumerable<FaturamentoItem> Mapear(List<FaturamentoItemDto> faturamentoItemDto)
        {
            foreach (var item in faturamentoItemDto)
            {
                yield return Mapear(item);
            }
        }
        #endregion
    }
}
