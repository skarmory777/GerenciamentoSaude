using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaPrecoItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto
{
    [AutoMap(typeof(FaturamentoContaItem))]
    public class FaturamentoContaItemDto : CamposPadraoCRUDDto
    {
        public Guid UniqueId { get; set; } = Guid.NewGuid();
        [StringLength(10)]
        public override string Codigo { get; set; }
        [StringLength(100)]
        public override string Descricao { get; set; }
        public long? FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public long? FaturamentoContaId { get; set; }
        public FaturamentoContaDto FaturamentoConta { get; set; }
        [DataType(DataType.DateTime)]
        public DateTimeOffset? Data { get; set; }
        public float Qtde { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public long? TerceirizadoId { get; set; }
        public Terceirizado Terceirizado { get; set; }
        public long? CentroCustoId { get; set; }
        public CentroCustoDto CentroCusto { get; set; }
        public long? TurnoId { get; set; }
        public TurnoDto Turno { get; set; }
        public long? TipoLeitoId { get; set; }
        public TipoLeitoDto TipoLeito { get; set; }
        public float ValorTemp { get; set; }
        public long? MedicoId { get; set; }
        public MedicoDto Medico { get; set; }
        public bool IsMedCredenciado { get; set; }
        public bool IsGlosaMedico { get; set; }
        public long? MedicoEspecialidadeId { get; set; }
        public MedicoEspecialidadeDto MedicoEspecialidade { get; set; }
        public long? FaturamentoContaKitId { get; set; }
        
        public FaturamentoContaKitDto FaturamentoContaKit { get; set; }
        
        public bool IsCirurgia { get; set; }
        public float ValorAprovado { get; set; }
        public float ValorTaxas { get; set; }
        public bool IsValorItemManual { get; set; }
        public float ValorItem { get; set; }
        public string HMCH { get; set; }
        public float ValorFilme { get; set; }
        public float ValorFilmeAprovado { get; set; }
        public float ValorCOCH { get; set; }
        public float ValorCOCHAprovado { get; set; }
        public float Percentual { get; set; }
        public bool IsInstrCredenciado { get; set; }
        public float ValorTotalRecuperado { get; set; }
        public float ValorTotalRecebido { get; set; }
        public float MetragemFilme { get; set; }
        public float MetragemFilmeAprovada { get; set; }
        public float COCH { get; set; }
        public float COCHAprovado { get; set; }

        public long? StatusId { get; set; }
        public FaturamentoContaStatusDto Status { get; set; }

        public bool IsRecuperaMedico { get; set; }
        public long? Auxiliar1Id { get; set; }
        public MedicoDto Auxiliar1 { get; set; }
        public bool IsAux1Credenciado { get; set; }
        public bool IsRecebeAuxiliar1 { get; set; }
        public bool IsGlosaAuxiliar1 { get; set; }
        public bool IsRecuperaAuxiliar1 { get; set; }
        public long? Auxiliar1EspecialidadeId { get; set; }
        public MedicoEspecialidadeDto Auxiliar1Especialidade { get; set; }
        public long? Auxiliar2Id { get; set; }
        public MedicoDto Auxiliar2 { get; set; }
        public bool IsAux2Credenciado { get; set; }
        public bool IsRecebeAuxiliar2 { get; set; }
        public bool IsGlosaAuxiliar2 { get; set; }
        public bool IsRecuperaAuxiliar2 { get; set; }
        public long? Auxiliar2EspecialidadeId { get; set; }
        public MedicoEspecialidadeDto Auxiliar2Especialidade { get; set; }
        public long? Auxiliar3Id { get; set; }
        public MedicoDto Auxiliar3 { get; set; }
        public bool IsAux3Credenciado { get; set; }
        public bool IsRecebeAuxiliar3 { get; set; }
        public bool IsGlosaAuxiliar3 { get; set; }
        public bool IsRecuperaAuxiliar3 { get; set; }
        public long? Auxiliar3EspecialidadeId { get; set; }
        public MedicoEspecialidadeDto Auxiliar3Especialidade { get; set; }
        public long? InstrumentadorId { get; set; }
        public MedicoDto Instrumentador { get; set; }
        public bool IsRecebeInstrumentador { get; set; }
        public bool IsGlosaInstrumentador { get; set; }
        public bool IsRecuperaInstrumentador { get; set; }
        public long? InstrumentadorEspecialidadeId { get; set; }
        public MedicoEspecialidadeDto InstrumentadorEspecialidade { get; set; }
        public long? AnestesistaId { get; set; }
        public MedicoDto Anestesista { get; set; }
        public long? EspecialidadeAnestesistaId { get; set; }
        public MedicoEspecialidadeDto EspecialidadeAnestesista { get; set; }
        public bool IsAnestCredenciado { get; set; }
        public string Observacao { get; set; }
        public int QtdeRecuperada { get; set; }
        public int QtdeAprovada { get; set; }
        public int QtdeRecebida { get; set; }
        public float ValorMoedaAprovado { get; set; }
        public long? SisMoedaId { get; set; }
        public SisMoedaDto SisMoeda { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataAutorizacao { get; set; }
        public string SenhaAutorizacao { get; set; }
        public string NomeAutorizacao { get; set; }

        public string ObsAutorizacao { get; set; }

        //[ForeignKey("RequisicaoMovItem"), Column("RequisicaoMovItemId")]
        //public long? RequisicaoMovItemId { get; set; }
        //public RequisicaoMovItem RequisicaoMovItem { get; set; }

        public long? PrecoId { get; set; }
        public FaturamentoTabelaPrecoItem Preco { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? HoraIncio { get; set; }
        public DateTime? HoraFim { get; set; }
        public string ViaAcesso { get; set; }
        public string Tecnica { get; set; }
        public string ClinicaId { get; set; }
        public long? FornecedorId { get; set; }
        public FornecedorDto Fornecedor { get; set; }
        public string NumeroNF { get; set; }
        public bool IsImportaEstoque { get; set; }

        public string TabelaUtilizada { get; set; }
        // public string CodigoTabela { get; set; }
        public long? FaturamentoConfigConvenioId { get; set; }

        public FaturamentoConfigConvenioDto FaturamentoConfigConvenioDto { get; set; }

        public long? FaturamentoPacoteId { get; set; }

        public FaturamentoPacoteDto FaturamentoPacote { get; set; }

        public long? FaturamentoItemCobradoId { get; set; }
        public FaturamentoItemDto FaturamentoItemCobrado { get; set; }
        
        public ResumoDetalhamento ResumoDetalhamento { get; set; }

        public static FaturamentoContaItemDto MapearFromCore(FaturamentoContaItem itemCore)
        {
            var itemDto = new FaturamentoContaItemDto
            {
                Id = itemCore.Id,
                Codigo = itemCore.Codigo,
                Descricao = itemCore.Descricao,
                FaturamentoItemId = itemCore.FaturamentoItemId
            };

            if (itemCore.FaturamentoItem != null)
            {
                itemDto.FaturamentoItem = FaturamentoItemDto.Mapear(itemCore.FaturamentoItem);
            }
            itemDto.FaturamentoContaId = itemCore.FaturamentoContaId;
            if (itemCore.FaturamentoConta != null)
            {
                itemDto.FaturamentoConta = FaturamentoContaDto.Mapear(itemCore.FaturamentoConta);
            }
            itemDto.Data = itemCore.Data;
            itemDto.Qtde = itemCore.Qtde;
            itemDto.UnidadeOrganizacionalId = itemCore.UnidadeOrganizacionalId;
            //    itemDto.UnidadeOrganizacional                 = itemCore.UnidadeOrganizacional                 ;
            itemDto.TerceirizadoId = itemCore.TerceirizadoId;
            itemDto.Terceirizado = itemCore.Terceirizado;
            itemDto.CentroCustoId = itemCore.CentroCustoId;
            //    itemDto.CentroCusto                           = itemCore.CentroCusto                           ;
            itemDto.TurnoId = itemCore.TurnoId;
            //     itemDto.Turno                                 = itemCore.Turno                                 ;
            // itemDto.TipoLeitoId = itemCore.TipoLeitoId;
            itemDto.TipoLeitoId = itemCore.TipoAcomodacaoId;
            //    itemDto.TipoLeito                             = itemCore.TipoLeito                             ;
            itemDto.ValorTemp = itemCore.ValorTemp;
            itemDto.MedicoId = itemCore.MedicoId;
            //   itemDto.Medico                                = itemCore.Medico                                ;
            itemDto.IsMedCredenciado = itemCore.IsMedCredenciado;
            itemDto.IsGlosaMedico = itemCore.IsGlosaMedico;
            itemDto.MedicoEspecialidadeId = itemCore.MedicoEspecialidadeId;
            //   itemDto.MedicoEspecialidade                   = itemCore.MedicoEspecialidade                   ;
            itemDto.FaturamentoContaKitId = itemCore.FaturamentoContaKitId;
            itemDto.IsCirurgia = itemCore.IsCirurgia;
            itemDto.ValorAprovado = itemCore.ValorAprovado;
            itemDto.ValorTaxas = itemCore.ValorTaxas;
            itemDto.IsValorItemManual = itemCore.IsValorItemManual;
            itemDto.ValorItem = itemCore.ValorItem;
            itemDto.HMCH = itemCore.HMCH;
            itemDto.ValorFilme = itemCore.ValorFilme;
            itemDto.ValorFilmeAprovado = itemCore.ValorFilmeAprovado;
            itemDto.ValorCOCH = itemCore.ValorCOCH;
            itemDto.ValorCOCHAprovado = itemCore.ValorCOCHAprovado;
            itemDto.Percentual = itemCore.Percentual;
            itemDto.IsInstrCredenciado = itemCore.IsInstrCredenciado;
            itemDto.ValorTotalRecuperado = itemCore.ValorTotalRecuperado;
            itemDto.ValorTotalRecebido = itemCore.ValorTotalRecebido;
            itemDto.MetragemFilme = itemCore.MetragemFilme;
            itemDto.MetragemFilmeAprovada = itemCore.MetragemFilmeAprovada;
            itemDto.COCH = itemCore.COCH;
            itemDto.COCHAprovado = itemCore.COCHAprovado;
            // STATUSNOVO ALTERAR       itemDto.StatusEntrega = itemCore.StatusEntrega;
            itemDto.IsRecuperaMedico = itemCore.IsRecuperaMedico;
            itemDto.Auxiliar1Id = itemCore.Auxiliar1Id;
            //     itemDto.Auxiliar1                             = itemCore.Auxiliar1                             ;
            itemDto.IsAux1Credenciado = itemCore.IsAux1Credenciado;
            itemDto.IsRecebeAuxiliar1 = itemCore.IsRecebeAuxiliar1;
            itemDto.IsGlosaAuxiliar1 = itemCore.IsGlosaAuxiliar1;
            itemDto.IsRecuperaAuxiliar1 = itemCore.IsRecuperaAuxiliar1;
            itemDto.Auxiliar1EspecialidadeId = itemCore.Auxiliar1EspecialidadeId;
            //       itemDto.Auxiliar1Especialidade                = itemCore.Auxiliar1Especialidade                ;
            itemDto.Auxiliar2Id = itemCore.Auxiliar2Id;
            //       itemDto.Auxiliar2                             = itemCore.Auxiliar2                             ;
            itemDto.IsAux2Credenciado = itemCore.IsAux2Credenciado;
            itemDto.IsRecebeAuxiliar2 = itemCore.IsRecebeAuxiliar2;
            itemDto.IsGlosaAuxiliar2 = itemCore.IsGlosaAuxiliar2;
            itemDto.IsRecuperaAuxiliar2 = itemCore.IsRecuperaAuxiliar2;
            itemDto.Auxiliar2EspecialidadeId = itemCore.Auxiliar2EspecialidadeId;
            //   itemDto.Auxiliar2Especialidade                = itemCore.Auxiliar2Especialidade                ;
            itemDto.Auxiliar3Id = itemCore.Auxiliar3Id;
            //    itemDto.Auxiliar3                             = itemCore.Auxiliar3                             ;
            itemDto.IsAux3Credenciado = itemCore.IsAux3Credenciado;
            itemDto.IsRecebeAuxiliar3 = itemCore.IsRecebeAuxiliar3;
            itemDto.IsGlosaAuxiliar3 = itemCore.IsGlosaAuxiliar3;
            itemDto.IsRecuperaAuxiliar3 = itemCore.IsRecuperaAuxiliar3;
            itemDto.Auxiliar3EspecialidadeId = itemCore.Auxiliar3EspecialidadeId;
            //     itemDto.Auxiliar3Especialidade                = itemCore.Auxiliar3Especialidade                ;
            itemDto.InstrumentadorId = itemCore.InstrumentadorId;
            //     itemDto.Instrumentador                        = itemCore.Instrumentador                        ;
            itemDto.IsRecebeInstrumentador = itemCore.IsRecebeInstrumentador;
            itemDto.IsGlosaInstrumentador = itemCore.IsGlosaInstrumentador;
            itemDto.IsRecuperaInstrumentador = itemCore.IsRecuperaInstrumentador;
            //   itemDto.InstrumentadorEspecialidadeId         = itemCore.InstrumentadorEspecialidade           ;
            //     itemDto.InstrumentadorEspecialidade           = itemCore.InstrumentadorEspecialidade           ;
            itemDto.AnestesistaId = itemCore.AnestesistaId;
            //    itemDto.Anestesista                           = itemCore.Anestesista                           ;
            //    itemDto.EspecialidadeAnestesistaId            = itemCore.EspecialidadeAnestesistaId            ;
            //    itemDto.EspecialidadeAnestesista              = itemCore.EspecialidadeAnestesista              ;
            itemDto.IsAnestCredenciado = itemCore.IsAnestCredenciado;
            itemDto.Observacao = itemCore.Observacao;
            itemDto.QtdeRecuperada = itemCore.QtdeRecuperada;
            itemDto.QtdeAprovada = itemCore.QtdeAprovada;
            itemDto.QtdeRecebida = itemCore.QtdeRecebida;
            itemDto.ValorMoedaAprovado = itemCore.ValorMoedaAprovado;
            itemDto.SisMoedaId = itemCore.SisMoedaId;
            //   itemDto.SisMoeda                              = itemCore.SisMoeda                              ;
            itemDto.DataAutorizacao = itemCore.DataAutorizacao;
            itemDto.SenhaAutorizacao = itemCore.SenhaAutorizacao;
            itemDto.NomeAutorizacao = itemCore.NomeAutorizacao;
            itemDto.ObsAutorizacao = itemCore.ObsAutorizacao;
            itemDto.PrecoId = itemCore.PrecoId;
            itemDto.Preco = itemCore.Preco;
            itemDto.HoraIncio = itemCore.HoraIncio;
            itemDto.HoraFim = itemCore.HoraFim;
            itemDto.ViaAcesso = itemCore.ViaAcesso;
            itemDto.Tecnica = itemCore.Tecnica;
            itemDto.ClinicaId = itemCore.ClinicaId;
            itemDto.FornecedorId = itemCore.FornecedorId;
            //   itemDto.Fornecedor                            = itemCore.Fornecedor                            ;
            itemDto.NumeroNF = itemCore.NumeroNF;
            itemDto.IsImportaEstoque = itemCore.IsImportaEstoque;
            itemDto.FaturamentoConfigConvenioId = itemCore.FaturamentoConfigConvenioId;

            itemDto.FaturamentoPacoteId = itemCore.FaturamentoPacoteId;

            if (itemCore.FaturamentoPacote != null)
            {
                itemDto.FaturamentoPacote = FaturamentoPacoteDto.Mapear(itemCore.FaturamentoPacote);
            }

            if (itemCore.FaturamentoConfigConvenio != null)
            {
                itemDto.FaturamentoConfigConvenioDto = FaturamentoConfigConvenioDto.Mapear(itemCore.FaturamentoConfigConvenio);
            }

            #region Anestesista

            if (itemCore.Anestesista != null)
            {
                itemDto.Anestesista = MedicoDto.Mapear(itemCore.Anestesista);
            }

            if (itemCore.AnestesistaEspecialidade != null)
            {
                itemDto.EspecialidadeAnestesista = MedicoEspecialidadeDto.Mapear(itemCore.AnestesistaEspecialidade);
            }

            itemDto.EspecialidadeAnestesistaId = itemCore.AnestesistaEspecialidadeId;

            #endregion

            #region Auxiliar1

            if (itemCore.Auxiliar1 != null)
            {
                itemDto.Auxiliar1 = MedicoDto.Mapear(itemCore.Auxiliar1);
            }

            if (itemCore.Auxiliar1Especialidade != null)
            {
                itemDto.Auxiliar1Especialidade = MedicoEspecialidadeDto.Mapear(itemCore.Auxiliar1Especialidade);
            }

            itemDto.Auxiliar1EspecialidadeId = itemCore.Auxiliar1EspecialidadeId;

            #endregion

            #region Auxiliar2

            if (itemCore.Auxiliar2 != null)
            {
                itemDto.Auxiliar2 = MedicoDto.Mapear(itemCore.Auxiliar2);
            }

            if (itemCore.Auxiliar2Especialidade != null)
            {
                itemDto.Auxiliar2Especialidade = MedicoEspecialidadeDto.Mapear(itemCore.Auxiliar2Especialidade);
            }

            itemDto.Auxiliar2EspecialidadeId = itemCore.Auxiliar2EspecialidadeId;

            #endregion

            #region Auxiliar3

            if (itemCore.Auxiliar3 != null)
            {
                itemDto.Auxiliar3 = MedicoDto.Mapear(itemCore.Auxiliar3);
            }

            if (itemCore.Auxiliar3Especialidade != null)
            {
                itemDto.Auxiliar3Especialidade = MedicoEspecialidadeDto.Mapear(itemCore.Auxiliar3Especialidade);
            }

            itemDto.Auxiliar3EspecialidadeId = itemCore.Auxiliar3EspecialidadeId;

            #endregion

            #region Instrumentador

            if (itemCore.Instrumentador != null)
            {
                itemDto.Instrumentador = MedicoDto.Mapear(itemCore.Instrumentador);
            }

            if (itemCore.InstrumentadorEspecialidade != null)
            {
                itemDto.InstrumentadorEspecialidade = MedicoEspecialidadeDto.Mapear(itemCore.InstrumentadorEspecialidade);
            }

            itemDto.InstrumentadorEspecialidadeId = itemCore.InstrumentadorEspecialidadeId;

            #endregion

            #region Medico

            if (itemCore.Medico != null)
            {
                itemDto.Medico = MedicoDto.Mapear(itemCore.Medico);
            }

            if (itemCore.MedicoEspecialidade != null)
            {
                itemDto.MedicoEspecialidade = MedicoEspecialidadeDto.Mapear(itemCore.MedicoEspecialidade);
            }

            #endregion


            itemDto.FaturamentoItemCobradoId = itemCore.FaturamentoItemCobradoId;

            if (itemCore.FaturamentoItemCobrado != null)
            {
                itemDto.FaturamentoItemCobrado = FaturamentoItemDto.Mapear(itemCore.FaturamentoItemCobrado);
            }

            if (itemCore.FaturamentoConfigConvenio != null)
            {
                itemDto.FaturamentoConfigConvenioDto = FaturamentoConfigConvenioDto.Mapear(itemCore.FaturamentoConfigConvenio);
            }

            if (itemCore.ResumoDetalhamento != null)
            {
                itemDto.ResumoDetalhamento = itemCore.ResumoDetalhamento;
            }


            return itemDto;
        }
    }

    public static class ResumoDetalhamentoExtensions
    {
        public static ResumoDetalhamentoConvenioConfig Mapear(this FaturamentoConfigAtualDto fatConfigConvenioDto)
        {
            if (fatConfigConvenioDto == null)
            {
                return null;
            }

            var config = new ResumoDetalhamentoConvenioConfig
            {
                DataFim = fatConfigConvenioDto.DataFim,
                DataIncio = fatConfigConvenioDto.DataInicio,
                EmpresaId = fatConfigConvenioDto.EmpresaId,
                ConvenioId = fatConfigConvenioDto.ConvenioId,
                PlanoId = fatConfigConvenioDto.PlanoId,
                GrupoId = fatConfigConvenioDto.GrupoId,
                SubGrupoId = fatConfigConvenioDto.SubGrupoId,
                TabelaId = fatConfigConvenioDto.TabelaId,
                ItemId = fatConfigConvenioDto.ItemId
            };

            if (fatConfigConvenioDto.Empresa != null)
            {
                config.Empresa = fatConfigConvenioDto.Empresa.NomeFantasia;
            }
            
            if (fatConfigConvenioDto.Convenio != null)
            {
                config.Convenio = fatConfigConvenioDto.Convenio.Nome;
            }
            
            if (fatConfigConvenioDto.Plano != null)
            {
                config.Plano = fatConfigConvenioDto.Plano.Descricao;
            }
            
            if (fatConfigConvenioDto.Grupo != null)
            {
                config.Grupo = fatConfigConvenioDto.Grupo.Descricao;
            }
            
            if (fatConfigConvenioDto.SubGrupo != null)
            {
                config.SubGrupo = fatConfigConvenioDto.SubGrupo.Descricao;
            }
            
            if (fatConfigConvenioDto.Tabela != null)
            {
                config.Tabela = fatConfigConvenioDto.Tabela.Descricao;
            }
            
            if (fatConfigConvenioDto.Item != null)
            {
                config.Item = fatConfigConvenioDto.Item.Descricao;
            }

            return config;
        }
        
        
        public static ResumoDetalhamentoSisMoedaCotacao Mapear(FaturamentoCotacaoMoedaDto sisMoedaCotacaoDto)
        {
            if (sisMoedaCotacaoDto == null)
            {
                return null;
            }

            var cotacao = new ResumoDetalhamentoSisMoedaCotacao
            {
                DataFinal = sisMoedaCotacaoDto.DataFinal,
                DataInicio = sisMoedaCotacaoDto.DataInicio,
                EmpresaId = sisMoedaCotacaoDto.EmpresaId,
                ConvenioId = sisMoedaCotacaoDto.ConvenioId,
                PlanoId = sisMoedaCotacaoDto.PlanoId,
                GrupoId = sisMoedaCotacaoDto.GrupoId,
                SubGrupoId = sisMoedaCotacaoDto.SubGrupoId,
                SisMoeda = ResumoDetalhamentoExtensions.Mapear(sisMoedaCotacaoDto.SisMoeda),
                Valor = sisMoedaCotacaoDto.Valor,
                IsTodosConvenio = sisMoedaCotacaoDto.IsTodosConvenio,
                IsTodosPlano = sisMoedaCotacaoDto.IsTodosPlano,
                IsTodosItem = sisMoedaCotacaoDto.IsTodosItem
            };

            if (sisMoedaCotacaoDto.Empresa != null)
            {
                cotacao.Empresa = sisMoedaCotacaoDto.Empresa.NomeFantasia;
            }

            if (sisMoedaCotacaoDto.Convenio != null)
            {
                cotacao.Convenio = sisMoedaCotacaoDto.Convenio.Nome;
            }

            if (sisMoedaCotacaoDto.Plano != null)
            {
                cotacao.Plano = sisMoedaCotacaoDto.Plano.Descricao;
            }

            if (sisMoedaCotacaoDto.Grupo != null)
            {
                cotacao.Grupo = sisMoedaCotacaoDto.Grupo.Descricao;
            }

            if (sisMoedaCotacaoDto.SubGrupo != null)
            {
                cotacao.SubGrupo = sisMoedaCotacaoDto.SubGrupo.Descricao;
            }

            return cotacao;
        }

        public static ResumoDetalhamentoHonorarios MapearHonorarios(FaturamentoContaItemDto faturamentoContaItemDto)
        {
            if (faturamentoContaItemDto == null)
            {
                return null;
            }

            return new ResumoDetalhamentoHonorarios(
                faturamentoContaItemDto.MedicoId,
                faturamentoContaItemDto.IsMedCredenciado,
                faturamentoContaItemDto.MedicoEspecialidadeId,
                faturamentoContaItemDto.Auxiliar1Id,
                faturamentoContaItemDto.IsAux1Credenciado,
                faturamentoContaItemDto.Auxiliar1EspecialidadeId,
                faturamentoContaItemDto.Auxiliar2Id,
                faturamentoContaItemDto.IsAux2Credenciado,
                faturamentoContaItemDto.Auxiliar2EspecialidadeId,
                faturamentoContaItemDto.Auxiliar3Id,
                faturamentoContaItemDto.IsAux3Credenciado,
                faturamentoContaItemDto.Auxiliar3EspecialidadeId,
                faturamentoContaItemDto.InstrumentadorId,
                faturamentoContaItemDto.IsInstrCredenciado,
                faturamentoContaItemDto.InstrumentadorEspecialidadeId,
                faturamentoContaItemDto.AnestesistaId,
                faturamentoContaItemDto.IsAnestCredenciado,
                faturamentoContaItemDto.EspecialidadeAnestesistaId,
                // TODO: Trocar para FatContaItemId da conta.
                null,
                faturamentoContaItemDto.Percentual
                );
        }



        public static  ResumoDetalhamentoFaturamentoTabela Mapear(FaturamentoTabelaDto faturamentoTabela)
        {
            if (faturamentoTabela == null)
            {
                return null;
            }

            var faturamentoTabelaDto = CamposPadraoCRUDDto.MapearBase<ResumoDetalhamentoFaturamentoTabela>(faturamentoTabela);

            faturamentoTabelaDto.IsAtualizaBrasindice = faturamentoTabela.IsAtualizaBrasindice;
            faturamentoTabelaDto.TabelaTissItemId = faturamentoTabela.TabelaTissItemId;
            faturamentoTabelaDto.IsCBHPM = faturamentoTabela.IsCBHPM;

            return faturamentoTabelaDto;
        }
        
        
        public static ResumoDetalhamentoMoeda Mapear(SisMoeda entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new ResumoDetalhamentoMoeda
            {
                Id = entity.Id,
                Codigo = entity.Codigo,
                Descricao = entity.Descricao,
                IsDeleted = entity.IsDeleted,
                DeleterUserId = entity.DeleterUserId,
                DeletionTime = entity.DeletionTime,
                CreatorUserId = entity.CreatorUserId,
                CreationTime = entity.CreationTime,
                IsSistema = entity.IsSistema,
                LastModificationTime = entity.LastModificationTime,
                LastModifierUserId = entity.LastModifierUserId,
                ImportaId = entity.ImportaId,
                Tipo = entity.Tipo,
                IsCobraCoch = entity.IsCobraCoch
            };
        }
        public static ResumoDetalhamentoMoeda Mapear(SisMoedaDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            return new ResumoDetalhamentoMoeda
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                Descricao = dto.Descricao,
                IsDeleted = dto.IsDeleted,
                DeleterUserId = dto.DeleterUserId,
                DeletionTime = dto.DeletionTime,
                CreatorUserId = dto.CreatorUserId,
                CreationTime = dto.CreationTime,
                IsSistema = dto.IsSistema,
                LastModificationTime = dto.LastModificationTime,
                LastModifierUserId = dto.LastModifierUserId,
                ImportaId = dto.ImportaId,
                Tipo = dto.Tipo,
                IsCobraCoch = dto.IsCobraCoch
            };
        }
    }
}
