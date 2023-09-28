using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas
{
    public class ResumoDetalhamento
    {
        public string GetTabela() => Tabela != null ? Tabela.Descricao : "";

        public ResumoDetalhamentoMoeda Moeda { get; set; }
        public ResumoDetalhamentoConvenioConfig ConfigAtual { get; set; }
        
        //TODO: AJEITAR ISSO
        //public List<FaturamentoTaxaDto> Taxas { get; set; }

        public ResumoDetalhamentoFaturamentoTabela Tabela { get; set; }

        public float Preco { get; set; } = 0f;

        public ResumoDetalhamentoSisMoedaCotacao CotacaoPorte { get; set; }

        public ResumoDetalhamentoSisMoedaCotacao CotacaoPorteFilme { get; set; }

        public float ValorFilme { get; set; }

        public float MetragemFilme { get; set; }

        public float TaxasFilme { get; set; }

        public float TaxasPorte { get; set; }


        public float ValorPorte { get; set; }

        public float ValorTaxas { get; set; }

        public float ValorCOCH { get; set; }

        public float ValorHMCH { get; set; }

        public bool PossuiCOCH { get { return ValorCOCH != 0; } }

        public bool PossuiHMCH { get { return ValorHMCH != 0; } }

        public long? FaturamentoItemCobradoId { get; set; }

        public float Valor { get; set; }

        public float TaxasValor { get; set; }

        public float ValorTotal { get; set; }
        public double Percentual { get; set; }

        public List<ResumoDetalhamentoTaxa> Taxas { get; set; }

        public ResumoDetalhamentoHonorarios Honorarios { get; set; }
        public float Qtde { get; set; }
        public float HMCH { get; set; }
        public float COCH { get; set; }
    }

    public class ResumoDetalhamentoHonorariosPessoaDados
    {
        public string Nome { get; set; }

        public string Conselho { get; set; }

        public string NumeroConselho { get; set; }

        public string Cpf { get; set; }
        public string Identidade { get; set; }

    }

    public class ResumoDetalhamentoTaxa : CamposPadraoCRUD
    {
        [Index("Fat_Idx_HoraInicio")]
        public DateTime? DataInicio { get; set; }
        [Index("Fat_Idx_HoraDataFim")]
        public DateTime? DataFim { get; set; }

        public long? Nivel { get; set; } = 1;

        public double Percentual { get; set; }

        public bool IsIncideFilme { get; set; }

        public bool IsIncidePrecoItem { get; set; }

        public bool IsIncidePorte { get; set; }

        public bool IsIncideManual { get; set; }

        public bool IsImplicita { get; set; }

        public float Valor { get; set; }
    }

    public class ResumoDetalhamentoConvenioConfig
    {
        
        public long? EmpresaId { get; set; }
        
        public string Empresa { get; set; }
        
        public long? ConvenioId { get; set; }
        
        public string Convenio { get; set; }
        
        public long? PlanoId { get; set; }
        
        public string Plano { get; set; }
        
        public long? GrupoId { get; set; }
        
        public string Grupo { get; set; }
        
        public long? SubGrupoId { get; set; }
        
        public string SubGrupo { get; set; }
        
        public long? TabelaId { get; set; }
        
        public string Tabela { get; set; }
        
        public long? ItemId { get; set; }
        
        public string Item { get; set; }
        [Index("Fat_Idx_DataIncio")]
        public DateTime? DataIncio { get; set; }
        [Index("Fat_Idx_DataFim")]
        public DateTime? DataFim { get; set; }
        
    }

    public class ResumoDetalhamentoSisMoedaCotacao
    {
        public ResumoDetalhamentoMoeda SisMoeda { get; set; }
        public long? SisMoedaId { get; set; }

        public string Empresa { get; set; }
        public long? EmpresaId { get; set; }

        public string Convenio { get; set; }
        public long? ConvenioId { get; set; }

        public string Plano { get; set; }
        public long? PlanoId { get; set; }

        public string Grupo { get; set; }
        public long? GrupoId { get; set; }

        public string SubGrupo { get; set; }
        public long? SubGrupoId { get; set; }
        [Index("Fat_Idx_DataInicio")]
        public DateTime DataInicio { get; set; }
        [Index("Fat_Idx_DataFinal")]
        public DateTime? DataFinal { get; set; }

        public float Valor { get; set; }

        public bool IsTodosConvenio { get; set; }
        public bool IsTodosPlano { get; set; }
        public bool IsTodosItem { get; set; }
    }

    public class ResumoDetalhamentoMoeda : CamposPadraoCRUD
    {
        public int Tipo { get; set; }

        public bool IsCobraCoch { get; set; }
    }

    public class ResumoDetalhamentoFaturamentoTabela : CamposPadraoCRUD
    {
        public bool IsAtualizaBrasindice { get; set; }

        public long? TabelaTissItemId { get; set; }

        public bool IsCBHPM { get; set; }
    }

    public class ResumoDetalhamentoHonorarios
    {
        public ResumoDetalhamentoHonorarios()
        {

        }

        public ResumoDetalhamentoHonorarios(
            long? medicoId, bool medicoIsCredenciado, long? medicoEspecialidadeId, 
            long? auxiliar1Id, bool auxiliar1IsCredenciado, long? auxiliar1EspecialidadeId, 
            long? auxiliar2Id, bool auxiliar2IsCredenciado, long? auxiliar2EspecialidadeId, 
            long? auxiliar3Id, bool auxiliar3IsCredenciado, long? auxiliar3EspecialidadeId, 
            long? instrumentadorId, bool instrumentadorIsCredenciado, long? instrumentadorEspecialidadeId, 
            long? anestesistaId, bool anestesistaIsCredenciado, long? anestesistaEspecialidadeId, 
            long? procedimentoPrincipalId, float percFaturamento)
        {
            MedicoId = medicoId;
            MedicoIsCredenciado = medicoIsCredenciado;
            MedicoEspecialidadeId = medicoEspecialidadeId;
            Auxiliar1Id = auxiliar1Id;
            Auxiliar1IsCredenciado = auxiliar1IsCredenciado;
            Auxiliar1EspecialidadeId = auxiliar1EspecialidadeId;
            Auxiliar2Id = auxiliar2Id;
            Auxiliar2IsCredenciado = auxiliar2IsCredenciado;
            Auxiliar2EspecialidadeId = auxiliar2EspecialidadeId;
            Auxiliar3Id = auxiliar3Id;
            Auxiliar3IsCredenciado = auxiliar3IsCredenciado;
            Auxiliar3EspecialidadeId = auxiliar3EspecialidadeId;
            InstrumentadorId = instrumentadorId;
            InstrumentadorIsCredenciado = instrumentadorIsCredenciado;
            InstrumentadorEspecialidadeId = instrumentadorEspecialidadeId;
            AnestesistaId = anestesistaId;
            AnestesistaIsCredenciado = anestesistaIsCredenciado;
            AnestesistaEspecialidadeId = anestesistaEspecialidadeId;
            ProcedimentoPrincipalId = procedimentoPrincipalId;
            PercFaturamento = percFaturamento;
        }

        public static ResumoDetalhamentoHonorariosPessoaDados MapearDadosProfissional(Medico medico)
        {
            if(medico == null)
            {
                return null;
            }


            return new ResumoDetalhamentoHonorariosPessoaDados
            {
                Conselho = medico.Conselho?.Codigo,
                NumeroConselho = medico.NumeroConselho.ToString(),
                Nome = medico.NomeCompleto,
                Cpf = medico.Cpf,
                Identidade = medico.Rg
            };

        }

        public long? MedicoId { get; set; }
        public bool MedicoIsCredenciado { get; set; }

        public long? MedicoEspecialidadeId { get; set; }

        public float MedicoValor { get; set; }

        public bool HasMedico { get => MedicoId.HasValue && MedicoId.Value != 0; }

        public long? Auxiliar1Id { get; set; }

        public bool Auxiliar1IsCredenciado { get; set; }

        public long? Auxiliar1EspecialidadeId { get; set; }

        public float Auxiliar1Valor { get; set; }

        public bool HasAuxiliar1 { get => Auxiliar1Id.HasValue && Auxiliar1Id.Value != 0; }

        public long? Auxiliar2Id { get; set; }

        public bool Auxiliar2IsCredenciado { get; set; }

        public long? Auxiliar2EspecialidadeId { get; set; }

        public float Auxiliar2Valor { get; set; }

        public bool HasAuxiliar2 { get => Auxiliar2Id.HasValue && Auxiliar2Id.Value != 0; }

        public long? Auxiliar3Id { get; set; }

        public bool Auxiliar3IsCredenciado { get; set; }

        public long? Auxiliar3EspecialidadeId { get; set; }

        public float Auxiliar3Valor { get; set; }

        public bool HasAuxiliar3 { get => Auxiliar3Id.HasValue && Auxiliar3Id.Value != 0; }

        public long? InstrumentadorId { get; set; }

        public bool InstrumentadorIsCredenciado { get; set; }

        public long? InstrumentadorEspecialidadeId { get; set; }

        public float InstrumentadorValor { get; set; }

        public bool HasInstrumentador { get => InstrumentadorId.HasValue && InstrumentadorId.Value != 0; }

        public long? AnestesistaId { get; set; }

        public bool AnestesistaIsCredenciado { get; set; }

        public long? AnestesistaEspecialidadeId { get; set; }

        public float AnestesistaValor { get; set; }

        public bool HasAnestesista { get => AnestesistaId.HasValue && AnestesistaId.Value != 0; }

        public long? ProcedimentoPrincipalId { get; set; }

        public float PercFaturamento { get; set; }
        public ResumoDetalhamentoHonorariosPessoaDados DadosMedico { get; set; }
        public ResumoDetalhamentoHonorariosPessoaDados DadosAuxiliar1 { get; set; }
        public ResumoDetalhamentoHonorariosPessoaDados DadosAuxiliar2 { get; set; }

        public ResumoDetalhamentoHonorariosPessoaDados DadosAuxiliar3 { get; set; }

        public ResumoDetalhamentoHonorariosPessoaDados DadosInstrumentador { get; set; }

        public ResumoDetalhamentoHonorariosPessoaDados DadosAnestesista { get; set; }

        public  static List<long> RetornaProfissionaisDaSaude(ResumoDetalhamentoHonorarios honorarios)
        {
            var result = new List<long>();

            if (honorarios == null)
            {
                return result;
            }

            if(honorarios.HasMedico) {
                result.Add(honorarios.MedicoId.Value);
            }

            if (honorarios.HasAuxiliar1)
            {
                result.Add(honorarios.Auxiliar1Id.Value);
            }

            if (honorarios.HasAuxiliar2)
            {
                result.Add(honorarios.Auxiliar2Id.Value);
            }

            if (honorarios.HasAuxiliar3)
            {
                result.Add(honorarios.Auxiliar3Id.Value);
            }

            if (honorarios.HasInstrumentador)
            {
                result.Add(honorarios.InstrumentadorId.Value);
            }

            if (honorarios.HasAnestesista)
            {
                result.Add(honorarios.AnestesistaId.Value);
            }

            return result;
        }
    }
}