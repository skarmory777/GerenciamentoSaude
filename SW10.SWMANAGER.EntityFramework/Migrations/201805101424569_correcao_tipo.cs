namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class correcao_tipo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sis_Ambulatorio", "IDAmbulatorio", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDAtendRevisao", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDAlta", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "StatusProntoAtend", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDUsuarioLiberacao", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDPrioridadeAtendimento", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDUsuarioRevelia", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "CodAmbulatorioSUS", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDSetor", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDAltaAmbulatorial", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDUsuarioAltaInc", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDMedPreAtend", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDMedicoAtendendo", c => c.Long());
            AlterColumn("dbo.Sis_Ambulatorio", "IDProtocoloEmergencia", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDAtendimento", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDEmpresa", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDFilial", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDConvenioAtend", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDOrigem", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDPaciente", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDMedico", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDEspecialidade", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDMedicoIndica", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioInclusao", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioAlteracao", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioCancelamento", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDAteMotCancelamento", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDImportado", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDClinica", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDAtendimentoStatus", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDIndicadorAcidente", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDRevisaoEntrega", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioRecebimento", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioCancelaRecebimento", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioObsRecebimento", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDMedicoConsulta", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "Mes", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "Ano", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "Idade", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDAtendimentoInicial", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioRetorno", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IsEncaminhado", c => c.Long());
            AlterColumn("dbo.Sis_Atendimento", "IDEspecialidadeMedIndica", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDContaMedica", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDAtendimento", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDConvenio", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDPlano", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDGuia", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDFormatoMatricula", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "StatusEntrega", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "DiasAutorizados", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDPendenciaMotivo", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDUsuarioConferencia", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDImportado", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDFilialSin", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDMedico", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDLeitoTipo", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDAlta", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDEmpresaPac", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDUsuarioResponsavel", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "IDUsuarioAlteracao", c => c.Long());
            AlterColumn("dbo.Sis_ContaMedica", "Ordem", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDInternacao", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDLeito", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDLeitoTipo", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDAlta", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDBairroResponsa", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDCidadeResponsa", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDEstadoResponsa", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDEstadoPac", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "StatusPront", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioPront", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioAltaInc", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDCIDObito", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "SeObitoMulher", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "QtdeObitoNeonatalPrecoce", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "QtdeObitoNeonatalTardio", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "QtdeNascVivosTermo", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "QtdeNascMortos", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "QtdeNascVivosPrematuro", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "TvTelefone", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "QtdeAlta", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "QtdeTransf", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDAcompanhante", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "QuantFralda", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioPrevAltaInc", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioPrevAltaAlt", c => c.Long());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioPrevAltaDel", c => c.Long());
            AlterColumn("dbo.Sis_Paciente", "IDPaciente", c => c.Long());
            AlterColumn("dbo.Sis_Paciente", "IDEtnia", c => c.Long());
            AlterColumn("dbo.Sis_Paciente", "IDReligiao", c => c.Long());
            AlterColumn("dbo.Sis_Paciente", "Escolaridade", c => c.Long());
            AlterColumn("dbo.Sis_Paciente", "IDEmpresaPac", c => c.Long());
            AlterColumn("dbo.Sis_Paciente", "IDExterno", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDPessoa", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDPessoaTipo", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDBairro", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDCidade", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDEstado", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDInstrucao", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDProfissao", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDCobranca", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDContaTesouraria", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDMeioPagamento", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDDocumentoTipo", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDCentroCustoLocal", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDNaturalidade", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "NumeroLancamentos", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDUsuarioInclusao", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDUsuarioAlteracao", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDImportado", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDBanco1", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDBanco2", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDFilialSin", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDTipoLogradouro", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "ContaPadrao", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDUsuarioExclusao", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDCNAE", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDExterno", c => c.Long());
            AlterColumn("dbo.Sis_Pessoa", "IDNFDescricao", c => c.Long());
        }

        public override void Down()
        {
            AlterColumn("dbo.Sis_Pessoa", "IDNFDescricao", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDExterno", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDCNAE", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDUsuarioExclusao", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "ContaPadrao", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDTipoLogradouro", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDFilialSin", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDBanco2", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDBanco1", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDImportado", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDUsuarioAlteracao", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDUsuarioInclusao", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "NumeroLancamentos", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDNaturalidade", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDCentroCustoLocal", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDDocumentoTipo", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDMeioPagamento", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDContaTesouraria", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDCobranca", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDProfissao", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDInstrucao", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDEstado", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDCidade", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDBairro", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDPessoaTipo", c => c.Int());
            AlterColumn("dbo.Sis_Pessoa", "IDPessoa", c => c.Int());
            AlterColumn("dbo.Sis_Paciente", "IDExterno", c => c.Int());
            AlterColumn("dbo.Sis_Paciente", "IDEmpresaPac", c => c.Int());
            AlterColumn("dbo.Sis_Paciente", "Escolaridade", c => c.Int());
            AlterColumn("dbo.Sis_Paciente", "IDReligiao", c => c.Int());
            AlterColumn("dbo.Sis_Paciente", "IDEtnia", c => c.Int());
            AlterColumn("dbo.Sis_Paciente", "IDPaciente", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioPrevAltaDel", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioPrevAltaAlt", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioPrevAltaInc", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "QuantFralda", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDAcompanhante", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "QtdeTransf", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "QtdeAlta", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "TvTelefone", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "QtdeNascVivosPrematuro", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "QtdeNascMortos", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "QtdeNascVivosTermo", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "QtdeObitoNeonatalTardio", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "QtdeObitoNeonatalPrecoce", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "SeObitoMulher", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDCIDObito", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioAltaInc", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDUsuarioPront", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "StatusPront", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDEstadoPac", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDEstadoResponsa", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDCidadeResponsa", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDBairroResponsa", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDAlta", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDLeitoTipo", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDLeito", c => c.Int());
            AlterColumn("dbo.Sis_Internacao", "IDInternacao", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "Ordem", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDUsuarioAlteracao", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDUsuarioResponsavel", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDEmpresaPac", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDAlta", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDLeitoTipo", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDMedico", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDFilialSin", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDImportado", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDUsuarioConferencia", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDPendenciaMotivo", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "DiasAutorizados", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "StatusEntrega", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDFormatoMatricula", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDGuia", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDPlano", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDConvenio", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDAtendimento", c => c.Int());
            AlterColumn("dbo.Sis_ContaMedica", "IDContaMedica", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDEspecialidadeMedIndica", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IsEncaminhado", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioRetorno", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDAtendimentoInicial", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "Idade", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "Ano", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "Mes", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDMedicoConsulta", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioObsRecebimento", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioCancelaRecebimento", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioRecebimento", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDRevisaoEntrega", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDIndicadorAcidente", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDAtendimentoStatus", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDClinica", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDImportado", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDAteMotCancelamento", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioCancelamento", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioAlteracao", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDUsuarioInclusao", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDMedicoIndica", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDEspecialidade", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDMedico", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDPaciente", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDOrigem", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDConvenioAtend", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDFilial", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDEmpresa", c => c.Int());
            AlterColumn("dbo.Sis_Atendimento", "IDAtendimento", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDProtocoloEmergencia", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDMedicoAtendendo", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDMedPreAtend", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDUsuarioAltaInc", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDAltaAmbulatorial", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDSetor", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "CodAmbulatorioSUS", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDUsuarioRevelia", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDPrioridadeAtendimento", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDUsuarioLiberacao", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "StatusProntoAtend", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDAlta", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDAtendRevisao", c => c.Int());
            AlterColumn("dbo.Sis_Ambulatorio", "IDAmbulatorio", c => c.Int());
        }
    }
}
