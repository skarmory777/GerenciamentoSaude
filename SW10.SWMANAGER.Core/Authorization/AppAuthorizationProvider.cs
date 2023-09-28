using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace SW10.SWMANAGER.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));

            var visualAsaImportExport = administration.CreateChildPermission(AppPermissions.Pages_Administration_VisualAsaImportExport, L("VisualAsaImportExport"));
            visualAsaImportExport.CreateChildPermission(AppPermissions.Pages_Administration_VisualAsaImportExport_StartStop, L("VisualAsaImportExportStartStop"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            //permissões dos itens do sistema

            #region Atendimento

            var Atendimento = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento, L("Atendimento"), multiTenancySides: MultiTenancySides.Tenant);
            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Clinico, L("Clinico"), multiTenancySides: MultiTenancySides.Tenant);
            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Emergencia, L("Emergencia"), multiTenancySides: MultiTenancySides.Tenant);
            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AtendimentoExames, L("AtendimentoExames"), multiTenancySides: MultiTenancySides.Tenant);

            var ambEmerg = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AmbulatorioEmergencia, L("AmbulatorioEmergencia"), multiTenancySides: MultiTenancySides.Tenant);
            ambEmerg.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AmbulatorioEmergencia_Create, L("CreateNewAmbulatorioEmergencia"), multiTenancySides: MultiTenancySides.Tenant);
            ambEmerg.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AmbulatorioEmergencia_Edit, L("EditAmbulatorioEmergencia"), multiTenancySides: MultiTenancySides.Tenant);
            ambEmerg.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AmbulatorioEmergencia_Delete, L("DeleteAmbulatorioEmergencia"), multiTenancySides: MultiTenancySides.Tenant);

            var internacoes = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Internacao, L("Internacao"), multiTenancySides: MultiTenancySides.Tenant);
            internacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Internacao_Create, L("CreateNewInternacao"), multiTenancySides: MultiTenancySides.Tenant);
            internacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Internacao_Edit, L("EditInternacao"), multiTenancySides: MultiTenancySides.Tenant);
            internacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Internacao_Delete, L("DeleteInternacao"), multiTenancySides: MultiTenancySides.Tenant);
            internacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Internacao_Alta, L("AltaInternacao"), multiTenancySides: MultiTenancySides.Tenant);

            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_HomeCare, L("HomeCare"), multiTenancySides: MultiTenancySides.Tenant);
            //   Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Orcamentos, L("Orcamento"), multiTenancySides: MultiTenancySides.Tenant);
            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Agendamento, L("Agendamento"), multiTenancySides: MultiTenancySides.Tenant);
            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoExames, L("AgendamentoExames"), multiTenancySides: MultiTenancySides.Tenant);
            // Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias, L("AgendamentoCirurgias"), multiTenancySides: MultiTenancySides.Tenant);

            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_CentralAutorizacao, L("CentralAutorizacao"), multiTenancySides: MultiTenancySides.Tenant);
            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Autorizacao, L("Autorizacao"), multiTenancySides: MultiTenancySides.Tenant);
            Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Prorrogacao, L("Prorrogacao"), multiTenancySides: MultiTenancySides.Tenant);
            var AtendimentoRelatorio = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Relatorio, L("AtendimentoRelatorio"), multiTenancySides: MultiTenancySides.Tenant);
            //Atendimento->Relatorio
            AtendimentoRelatorio.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado, L("RelatorioInternado"), multiTenancySides: MultiTenancySides.Tenant);
            AtendimentoRelatorio.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento, L("RelatorioAtendimento"), multiTenancySides: MultiTenancySides.Tenant);
            AtendimentoRelatorio.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioAgendamentoCirurgico, L("Relatorio Agendamento Cirurgicos"), multiTenancySides: MultiTenancySides.Tenant);
            

            var visitante = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Visitante, L("Visitante"), multiTenancySides: MultiTenancySides.Tenant);
            var crudVisitante = visitante.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Visitante_Create, L("CreateNewVisitante"), multiTenancySides: MultiTenancySides.Tenant);
            crudVisitante = visitante.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Visitante_Edit, L("EditVisitante"), multiTenancySides: MultiTenancySides.Tenant);
            crudVisitante = visitante.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Visitante_Delete, L("DeleteVisitante"), multiTenancySides: MultiTenancySides.Tenant);

            var agendamentoConsulta = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoConsultas, L("AgendamentoConsultas"), multiTenancySides: MultiTenancySides.Tenant);
            var crudAgendamentoConsulta = agendamentoConsulta.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoConsultas_Create, L("CreateNewAgendamentoConsulta"), multiTenancySides: MultiTenancySides.Tenant);
            crudAgendamentoConsulta = agendamentoConsulta.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoConsultas_Edit, L("EditAgendamentoConsulta"), multiTenancySides: MultiTenancySides.Tenant);
            crudAgendamentoConsulta = agendamentoConsulta.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoConsultas_Delete, L("DeleteAgendamentoConsulta"), multiTenancySides: MultiTenancySides.Tenant);

            var agendamentoCirurgias = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias, L("AgendamentoCirurgias"), multiTenancySides: MultiTenancySides.Tenant);

            agendamentoCirurgias.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias_Create, L("CreateNewAgendamentoConsulta"), multiTenancySides: MultiTenancySides.Tenant);
            agendamentoCirurgias.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias_Edit, L("EditAgendamentoConsulta"), multiTenancySides: MultiTenancySides.Tenant);
            agendamentoCirurgias.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias_Delete, L("DeleteAgendamentoConsulta"), multiTenancySides: MultiTenancySides.Tenant);
            agendamentoCirurgias.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias_Desconto, L("Desconto"), multiTenancySides: MultiTenancySides.Tenant);
            agendamentoCirurgias.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias_Orcamento, L("Orcamento"), multiTenancySides: MultiTenancySides.Tenant);
            agendamentoCirurgias.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias_Confirma, L("ConfirmarAtendimento"), multiTenancySides: MultiTenancySides.Tenant);


            var preAtendimentos = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos, L("PreAtendimentos"), multiTenancySides: MultiTenancySides.Tenant);
            var crudPreAtendimento = preAtendimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos_Create, L("CreateNewPreAtendimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudPreAtendimento = preAtendimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos_Edit, L("EditPreAtendimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudPreAtendimento = preAtendimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos_Delete, L("DeletePreAtendimento"), multiTenancySides: MultiTenancySides.Tenant);

            var atendimentoLeitoMov = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AtendimentoLeitoMov, L("AtendimentoLeitoMov"), multiTenancySides: MultiTenancySides.Tenant);
            var crudAtendimentoLeitoMov = atendimentoLeitoMov.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AtendimentoLeitoMov_Create, L("CreateNewAtendimentoLeitoMov"), multiTenancySides: MultiTenancySides.Tenant);
            crudAtendimentoLeitoMov = atendimentoLeitoMov.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AtendimentoLeitoMov_Edit, L("EditAtendimentoLeitoMove"), multiTenancySides: MultiTenancySides.Tenant);
            crudAtendimentoLeitoMov = atendimentoLeitoMov.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_AtendimentoLeitoMov_Delete, L("DeleteAtendimentoLeitoMov"), multiTenancySides: MultiTenancySides.Tenant);

            var terminalSenha = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_TerminalSenha, L("TerminalSenha"), multiTenancySides: MultiTenancySides.Tenant);
            var MonitorPainelSenha = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_PainelSenha, L("PainelSenha"), multiTenancySides: MultiTenancySides.Tenant);


            var classificacoesRisco = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_ClassificacaoRiscos, L("ClassificacoesRisco"), multiTenancySides: MultiTenancySides.Tenant);
            var crudClassificacoesRisco = classificacoesRisco.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_ClassificacaoRiscos_Create, L("CreateNewClassificacaoRisco"), multiTenancySides: MultiTenancySides.Tenant);
            crudClassificacoesRisco = classificacoesRisco.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_ClassificacaoRiscos_Edit, L("EditClassificacaoRisco"), multiTenancySides: MultiTenancySides.Tenant);
            crudClassificacoesRisco = classificacoesRisco.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_ClassificacaoRiscos_Delete, L("DeleteClassificacaoRisco"), multiTenancySides: MultiTenancySides.Tenant);

            var orcamento = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Orcamentos, L("Orcamentos"), multiTenancySides: MultiTenancySides.Tenant);
            var crudOrcamentos = orcamento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Orcamentos_Create, L("CreateNewOrcamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudOrcamentos = orcamento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Orcamentos_Edit, L("EditOrcamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudOrcamentos = orcamento.CreateChildPermission(AppPermissions.Pages_Tenant_Atendimento_Orcamentos_Delete, L("DeleteOrcamento"), multiTenancySides: MultiTenancySides.Tenant);

            var guiaTipo = Atendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_GuiaTipos, L("GuiaTipos"), multiTenancySides: MultiTenancySides.Tenant);
            var crudGuiaTipo = guiaTipo.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Create, L("CreateNewGuiaTipo"), multiTenancySides: MultiTenancySides.Tenant);
            crudGuiaTipo = guiaTipo.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Edit, L("EditGuiaTipo"), multiTenancySides: MultiTenancySides.Tenant);
            crudGuiaTipo = guiaTipo.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Delete, L("DeleteGuiaTipo"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region Assistencial
            //Assistencial
            var assistenciais = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial, L("Assistencial"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialAmbulatorioEmergencia = assistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia, L("AmbulatorioEmergencia"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialInternacao = assistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_Internacao, L("Internacao"), multiTenancySides: MultiTenancySides.Tenant);

            var atestados = assistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_Atestados, L("Atestados"), multiTenancySides: MultiTenancySides.Tenant);
            var atestadosMedicos = atestados.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico, L("AtestadoMedico"), multiTenancySides: MultiTenancySides.Tenant);
            var crudAtestadoMedico = atestadosMedicos.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Create, L("CreateNewAtestadoMedico"), multiTenancySides: MultiTenancySides.Tenant);
            crudAtestadoMedico = atestadosMedicos.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Edit, L("EditAtestadoMedico"), multiTenancySides: MultiTenancySides.Tenant);
            crudAtestadoMedico = atestadosMedicos.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Delete, L("DeleteAtestadoMedico"), multiTenancySides: MultiTenancySides.Tenant);

            var atestadosComparecimento = atestados.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AtestadoComparecimento, L("AtestadoComparecimento"), multiTenancySides: MultiTenancySides.Tenant);
            var crudAtestadoComparecimento = atestadosComparecimento.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AtestadoComparecimento_Create, L("CreateNewAtestadoComparecimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudAtestadoMedico = atestadosMedicos.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AtestadoComparecimento_Edit, L("EditAtestadoComparecimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudAtestadoMedico = atestadosMedicos.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AtestadoComparecimento_Delete, L("DeleteAtestadoComparecimento"), multiTenancySides: MultiTenancySides.Tenant);

            //Assistencial Atendimento Enfermagem
            var assistencialAmbulatorioEmergenciaEnfermagem = assistencialAmbulatorioEmergencia.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem, L("Enfermagem"), multiTenancySides: MultiTenancySides.Tenant);
            var enfermagemAdmissao = assistencialAmbulatorioEmergenciaEnfermagem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Admissao, L("EnfermagemAdmissao"), multiTenancySides: MultiTenancySides.Tenant);
            PermissionFormumalrioDinamico(enfermagemAdmissao,"EnfermagemAdmissao");
            var enfermagemEvoluca = assistencialAmbulatorioEmergenciaEnfermagem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Evolucao, L("EnfermagemEvolucao"), multiTenancySides: MultiTenancySides.Tenant);
            PermissionFormumalrioDinamico(enfermagemEvoluca,"EnfermagemEvolucao");
            assistencialAmbulatorioEmergenciaEnfermagem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_PassagemPlantao, L("PlantaoEnfermagemPassagem"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaEnfermagem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Prescricao, L("EnfermagemPrescricao"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaEnfermagem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Checagem, L("EnfermagemChecagem"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaEnfermagem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_SinaisVitais, L("EnfermagemSinaisVitais"), multiTenancySides: MultiTenancySides.Tenant);
            var bhPermisoes = assistencialAmbulatorioEmergenciaEnfermagem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_ControleBalancoHidrico, L("EnfermagemControleBalancoHidrico"), multiTenancySides: MultiTenancySides.Tenant);
            
            bhPermisoes.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_ControleBalancoHidrico + AppPermissions.BalancoHidrico_Desbloqueio, L("BalancoHidrico_Desbloqueio"), multiTenancySides: MultiTenancySides.Tenant);
            bhPermisoes.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_ControleBalancoHidrico + AppPermissions.BalancoHidrico_Desbloqueio_24hrs, L("BalancoHidrico_Desbloqueio_24hrs"), multiTenancySides: MultiTenancySides.Tenant);
            bhPermisoes.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_ControleBalancoHidrico + AppPermissions.BalancoHidrico_Desbloqueio_Proprio_24hrs, L("BalancoHidrico_Desbloqueio_Proprio_24hrs"), multiTenancySides: MultiTenancySides.Tenant);
            bhPermisoes.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_ControleBalancoHidrico + AppPermissions.BalancoHidrico_ConferenciaTotal, L("BalancoHidrico_ConferenciaTotal"), multiTenancySides: MultiTenancySides.Tenant);

            //Assistencial Atendimento Medico
            var assistencialAmbulatorioEmergenciaMedico = assistencialAmbulatorioEmergencia.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico, L("Medico"), multiTenancySides: MultiTenancySides.Tenant);
            var medicoAdmissao = assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Admissao, L("MedicoAdmissao"), multiTenancySides: MultiTenancySides.Tenant);
            PermissionFormumalrioDinamico(medicoAdmissao,"MedicoAdmissao");
            assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Alta, L("MedicoAlta"), multiTenancySides: MultiTenancySides.Tenant);
            var menuAssistencialAmbulatorioEmergenciaMedicoAnamnese = assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Anamnese, L("MedicoAnamnese"), multiTenancySides: MultiTenancySides.Tenant);
            menuAssistencialAmbulatorioEmergenciaMedicoAnamnese.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Anamnese_ClinicoGeral, L("MedicoAnamneseClinicoGeral"), multiTenancySides: MultiTenancySides.Tenant);
            menuAssistencialAmbulatorioEmergenciaMedicoAnamnese.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Anamnese_Pediatrico, L("MedicoAnamnesePediatrico"), multiTenancySides: MultiTenancySides.Tenant);
            PermissionFormumalrioDinamico(menuAssistencialAmbulatorioEmergenciaMedicoAnamnese,"MedicoAnamnese");
            var evolucaoMedica = assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Evolucao, L("MedicoEvolucao"), multiTenancySides: MultiTenancySides.Tenant);
            PermissionFormumalrioDinamico(evolucaoMedica,"MedicoEvolucao");
            
            assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ParecerEspecialista, L("MedicoParecerEspecialista"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialAmbulatorioEmergenciaMedicoPrescricao = assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao, L("MedicoPrescricao"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoPrescricao.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao_Create, L("CreateNewMedicoPrescricao"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoPrescricao.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao_Edit, L("EditMedicoPrescricao"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoPrescricao.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao_Delete, L("DeleteMedicoPrescricao"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialAmbulatorioEmergenciaMedicoSolicitacaoExame = assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame, L("MedicoSolicitacaoExame"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Create, L("MedicoSolicitacaoExameCreate"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Edit, L("MedicoSolicitacaoExameEdit"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Delete, L("MedicoSolicitacaoExameDelete"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialAmbulatorioEmergenciaMedicoSolicitacaoExameItem = assistencialAmbulatorioEmergenciaMedicoSolicitacaoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExameItem, L("MedicoSolicitacaoExameItem"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExameItem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExameItem_Create, L("MedicoSolicitacaoExameItemCreate"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExameItem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExameItem_Edit, L("MedicoSolicitacaoExameItemEdit"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExameItem.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExameItem_Delete, L("MedicoSolicitacaoExameItemDelete"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialAmbulatorioEmergenciaMedicoSolicitacaoExamePrioridade = assistencialAmbulatorioEmergenciaMedicoSolicitacaoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Prioridade, L("MedicoSolicitacaoExamePrioridade"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExamePrioridade.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Prioridade_Create, L("MedicoSolicitacaoExamePrioridadeCreate"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExamePrioridade.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Prioridade_Edit, L("MedicoSolicitacaoExamePrioridadeEdit"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExamePrioridade.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Prioridade_Delete, L("MedicoSolicitacaoExamePrioridadeDelete"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialAmbulatorioEmergenciaMedicoSolicitacaoExameKit = assistencialAmbulatorioEmergenciaMedicoSolicitacaoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Kit, L("MedicoSolicitacaoExameKit"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExameKit.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Kit_Create, L("MedicoSolicitacaoExameKitCreate"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExameKit.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Kit_Edit, L("MedicoSolicitacaoExameKitEdit"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedicoSolicitacaoExameKit.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Kit_Delete, L("MedicoSolicitacaoExameKitDelete"), multiTenancySides: MultiTenancySides.Tenant);

            assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ResultadoExame, L("MedicoResultadoExame"), multiTenancySides: MultiTenancySides.Tenant);
            var medicoResumoAlta = assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ResumoAlta, L("MedicoResumoAlta"), multiTenancySides: MultiTenancySides.Tenant);
            PermissionFormumalrioDinamico(medicoResumoAlta,"MedicoResumoAlta"); var menuAssistencialAmbulatorioEmergenciaMedicoProcedimentoCirurgico = assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico, L("MedicoProcedimentoCirurgico"), multiTenancySides: MultiTenancySides.Tenant);
            menuAssistencialAmbulatorioEmergenciaMedicoProcedimentoCirurgico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_AtoCirurgico, L("MedicoProcedimentoCirurgicoAtoCirurgico"), multiTenancySides: MultiTenancySides.Tenant);
            menuAssistencialAmbulatorioEmergenciaMedicoProcedimentoCirurgico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_AtoAnestesico, L("MedicoProcedimentoCirurgicoAtoAnestesico"), multiTenancySides: MultiTenancySides.Tenant);
            menuAssistencialAmbulatorioEmergenciaMedicoProcedimentoCirurgico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_FolhaGastoCentroCirurgico, L("MedicoProcedimentoCirurgicoFolhaGastoCentroCirurgico"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_PartogramaRecemNato, L("MedicoPartogramaRecemNato"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaMedico.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Receituario, L("Receituario"), multiTenancySides: MultiTenancySides.Tenant);

            //Assistencial Atendimento Administrativo
            var assistencialAmbulatorioEmergenciaAdministrativo = assistencialAmbulatorioEmergencia.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo, L("Administrativo"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_CAT, L("AdministrativoCAT"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_AltaAdministrativa, L("AdministrativoAlta"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Alergia, L("AdministrativoAlergia"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_DocumentacaoPaciente, L("AdministrativoDocumentacaoPaciente"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialAmbulatorioEmergenciaAdministrativoConfirmacaoAgenda = assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda, L("AdministrativoConfirmacaoAgenda"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativoConfirmacaoAgenda.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Consulta, L("AdministrativoConfirmacaoAgendaConsulta"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativoConfirmacaoAgenda.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Exame, L("AdministrativoConfirmacaoAgendaExame"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativoConfirmacaoAgenda.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Cirurgia, L("AdministrativoConfirmacaoAgendaCirurgia"), multiTenancySides: MultiTenancySides.Tenant);
            var assistencialAmbulatorioEmergenciaAdministrativoTransferencia = assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia, L("AdministrativoTransferencia"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativoTransferencia.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_Leito, L("AdministrativoTransferenciaLeito"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativoTransferencia.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_MedicoResponsavel, L("AdministrativoTransferenciaMedicoResponsavel"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativoTransferencia.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_Setor, L("AdministrativoTransferenciaSetor"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_AlteracaoAtendimento, L("AdministrativoAlteracaoAtendimento"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_PassagemPlantaoEnfermagem, L("AdministrativoPassagemPlantaoEnfermagem"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProrrogacao, L("AdministrativoSolicitacaoProrrogacao"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProdutoSetor, L("AdministrativoSolicitacaoProdutoSetor"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProdutoSOS, L("AdministrativoSolicitacaoProdutoSOS"), multiTenancySides: MultiTenancySides.Tenant);
            assistencialAmbulatorioEmergenciaAdministrativo.CreateChildPermission(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_LiberacaoLeito, L("AdministrativoLiberacaoLeito"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            //Diagnósticos
            var Diagnostico = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos, L("Diagnosticos"), multiTenancySides: MultiTenancySides.Tenant);
            Diagnostico.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Laboratorio, L("Laboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            var imagens = Diagnostico.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Imagens, L("DiagnosticoPorImagem"), multiTenancySides: MultiTenancySides.Tenant);
            imagens.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Imagens_Create, L("CriandoExameImagem"));
            imagens.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Imagens_Edit, L("EditandoExameImagem"));
            imagens.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Imagens_Delete, L("DeletandoExameImagem"));


            var modalidade = Diagnostico.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Cadastros_Modalidade, L("Modalidade"), multiTenancySides: MultiTenancySides.Tenant);
            modalidade.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Cadastros_Modalidade_Create, L("CriandoModalidade"));
            modalidade.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Cadastros_Modalidade_Edit, L("EditandoModalidade"));
            modalidade.CreateChildPermission(AppPermissions.Pages_Tenant_Diagnosticos_Cadastros_Modalidade_Delete, L("DeletandoModalidade"));


            //Suprimentos
            var Suprimentos = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos, L("Suprimentos"), multiTenancySides: MultiTenancySides.Tenant);
            Suprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Almoxarifado, L("Almoxarifado"), multiTenancySides: MultiTenancySides.Tenant);
            Suprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Farmacia, L("Farmacia"), multiTenancySides: MultiTenancySides.Tenant);
            var Compras = Suprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Compras, L("Compras"), multiTenancySides: MultiTenancySides.Tenant);
            var ComprasRequisicao = Compras.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraRequisicao, L("RequisicaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasRequisicao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraRequisicao_Create, L("CreateNewRequisicaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasRequisicao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraRequisicao_Edit, L("EditRequisicaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasRequisicao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraRequisicao_Delete, L("DeleteRequisicaoCompra"), multiTenancySides: MultiTenancySides.Tenant);

            var ComprasAprovacao = Compras.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraAprovacao, L("AprovacaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasAprovacao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraAprovacao_Create, L("CreateNewAprovacaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasAprovacao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraAprovacao_Edit, L("EditAprovacaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasAprovacao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraAprovacao_Delete, L("DeleteAprovacaoCompra"), multiTenancySides: MultiTenancySides.Tenant);

            var ComprasCotacao = Compras.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraCotacao, L("CotacaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasCotacao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraCotacao_Create, L("CreateNewCotacaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasCotacao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraCotacao_Edit, L("EditCotacaoCompra"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasCotacao.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_CompraCotacao_Delete, L("DeleteCotacaoCompra"), multiTenancySides: MultiTenancySides.Tenant);

            var OrdemCompra = Compras.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_OrdemCompra, L("OrdemCompra"), multiTenancySides: MultiTenancySides.Tenant);
            OrdemCompra.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_OrdemCompra_Create, L("CreateNewOrdemCompra"), multiTenancySides: MultiTenancySides.Tenant);
            OrdemCompra.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_OrdemCompra_Edit, L("EditOrdemCompra"), multiTenancySides: MultiTenancySides.Tenant);
            OrdemCompra.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_OrdemCompra_Delete, L("DeleteOrdemCompra"), multiTenancySides: MultiTenancySides.Tenant);

            var ComprasRelatorios = Compras.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio, L("Relatorios"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasRelatorios.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio_CompraRequisicao, L("CompraRequisicao"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasRelatorios.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio_CompraAprovacao, L("CompraAprovacao"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasRelatorios.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio_CompraCotacao, L("CompraCotacao"), multiTenancySides: MultiTenancySides.Tenant);
            ComprasRelatorios.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio_OrdemCompra, L("OrdemCompra"), multiTenancySides: MultiTenancySides.Tenant);

            var SuprimentoEstoque = Suprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque, L("SuprimentoEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            var SuprimentoRelatorio = Suprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Relatorio, L("SuprimentoRelatorio"), multiTenancySides: MultiTenancySides.Tenant);

            //Faturamento
            var Faturamento = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento, L("Faturamento"), multiTenancySides: MultiTenancySides.Tenant);
            Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_FaturamentoConveniosParticular, L("FaturamentoConveniosParticular"), multiTenancySides: MultiTenancySides.Tenant);
            Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_FaturamentoSUSInternacao, L("FaturamentoSUSInternacao"), multiTenancySides: MultiTenancySides.Tenant);
            Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_FaturamentoSUSAmbulatorio, L("FaturamentoSUSAmbulatorio"), multiTenancySides: MultiTenancySides.Tenant);
            Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_Auditoria, L("Auditoria"), multiTenancySides: MultiTenancySides.Tenant);
            Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_RecursoGlosa, L("RecursoGlosa"), multiTenancySides: MultiTenancySides.Tenant);
            Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_CentralAutorizacaoGuias, L("CentralAutorizacaoGuias"), multiTenancySides: MultiTenancySides.Tenant);
            Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_RegrasConveniosParticulares, L("RegrasConveniosParticulares"), multiTenancySides: MultiTenancySides.Tenant);

            //Financeiro
            var Financeiro = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro, L("Financeiro"), multiTenancySides: MultiTenancySides.Tenant);

            var contasPagar = Financeiro.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasPagar, L("ContasPagar"), multiTenancySides: MultiTenancySides.Tenant);

            contasPagar.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasPagar_Create, L("CreateNewContasPagar"), multiTenancySides: MultiTenancySides.Tenant);
            contasPagar.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasPagar_Edit, L("EditContasPagar"), multiTenancySides: MultiTenancySides.Tenant);
            contasPagar.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasPagar_Delete, L("DeleteContasPagar"), multiTenancySides: MultiTenancySides.Tenant);


            //Financeiro.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasReceber, L("ContasReceber"), multiTenancySides: MultiTenancySides.Tenant);


            var contasReceber = Financeiro.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasReceber, L("ContasReceber"), multiTenancySides: MultiTenancySides.Tenant);

            contasReceber.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasReceber_Create, L("CreateNewContasReceber"), multiTenancySides: MultiTenancySides.Tenant);
            contasReceber.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasReceber_Edit, L("EditContasReceber"), multiTenancySides: MultiTenancySides.Tenant);
            contasReceber.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ContasReceber_Delete, L("DeleteContasReceber"), multiTenancySides: MultiTenancySides.Tenant);


            Financeiro.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_ControleBancario, L("ControleBancario"), multiTenancySides: MultiTenancySides.Tenant);
            Financeiro.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_Tesouraria, L("Tesouraria"), multiTenancySides: MultiTenancySides.Tenant);
            Financeiro.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_FluxoCaixa, L("FluxoCaixa"), multiTenancySides: MultiTenancySides.Tenant);
            Financeiro.CreateChildPermission(AppPermissions.Pages_Tenant_Financeiro_RepasseMedico, L("RepasseMedico"), multiTenancySides: MultiTenancySides.Tenant);

            //Controladoria
            var controladoria = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria, L("Controladoria"), multiTenancySides: MultiTenancySides.Tenant);
            controladoria.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria_Orcamentos, L("Orcamentos"), multiTenancySides: MultiTenancySides.Tenant);
            controladoria.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria_Patrimonio, L("Patrimonio"), multiTenancySides: MultiTenancySides.Tenant);
            controladoria.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria_Contabilidade, L("Contabilidade"), multiTenancySides: MultiTenancySides.Tenant);
            controladoria.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria_Custos, L("Custos"), multiTenancySides: MultiTenancySides.Tenant);
            controladoria.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria_NotasFiscais, L("NotasFiscais"), multiTenancySides: MultiTenancySides.Tenant);
            controladoria.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria_Eventos, L("Eventos"), multiTenancySides: MultiTenancySides.Tenant);
            controladoria.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria_Projetos, L("Projetos"), multiTenancySides: MultiTenancySides.Tenant);


            var notasFiscais = controladoria.CreateChildPermission(AppPermissions.Pages_Tenant_Controladoria_NotasFiscais_Sincronizar, L("SincronizarNotas"), multiTenancySides: MultiTenancySides.Tenant);

            //Apoio
            var Apoio = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio, L("Apoio"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_Nutricao, L("Nutricao"), multiTenancySides: MultiTenancySides.Tenant);
           var disparoDeMensagem  = Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Disparo_De_Mensagem, L("Disparo De Mensagem"), multiTenancySides: MultiTenancySides.Tenant);
            disparoDeMensagem.CreateChildPermission(AppPermissions.Pages_Tenant_Disparo_De_Mensagem_Create, L("Criar Disparo De Mensagem"), multiTenancySides: MultiTenancySides.Tenant);
            disparoDeMensagem.CreateChildPermission(AppPermissions.Pages_Tenant_Disparo_De_Mensagem_Edit, L("Editar Disparo De Mensagem"), multiTenancySides: MultiTenancySides.Tenant);
            disparoDeMensagem.CreateChildPermission(AppPermissions.Pages_Tenant_Disparo_De_Mensagem_Delete, L("Delete Disparo De Mensagem"), multiTenancySides: MultiTenancySides.Tenant);
            
            var aviso  = Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Aviso, L("Aviso"), multiTenancySides: MultiTenancySides.Tenant);
            aviso.CreateChildPermission(AppPermissions.Pages_Tenant_Aviso_Create, L("Criar Aviso"), multiTenancySides: MultiTenancySides.Tenant);
            aviso.CreateChildPermission(AppPermissions.Pages_Tenant_Aviso_Edit, L("Editar Aviso"), multiTenancySides: MultiTenancySides.Tenant);
            aviso.CreateChildPermission(AppPermissions.Pages_Tenant_Aviso_Delete, L("Delete Aviso"), multiTenancySides: MultiTenancySides.Tenant);
            
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_CentralMateriais, L("CentralMateriais"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_SolicitacaoAntimicrobianos, L("SolicitacaoAntimicrobianos"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_Esterilizados, L("Esterilizados"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_Manutencao, L("Manutencao"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_Higienizacao, L("Higienizacao"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_PortariaControleAcesso, L("PortariaControleAcesso"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_LavanderiaRouparia, L("LavanderiaRouparia"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_SAC, L("SAC"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_SAME, L("SAME"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_ControleInfeccao, L("ControleInfeccao"), multiTenancySides: MultiTenancySides.Tenant);
            Apoio.CreateChildPermission(AppPermissions.Pages_Tenant_Apoio_Hospitalar, L("Hospitalar"), multiTenancySides: MultiTenancySides.Tenant);

            //Configurações
            var configuracoes = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes, L("Configuracoes"), multiTenancySides: MultiTenancySides.Tenant);
            var empresas = configuracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Empresa, L("Empresas"), multiTenancySides: MultiTenancySides.Tenant);
            var formulariosPersonalizados = configuracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_GeradorFormulario, L("GeradorFormularios"), multiTenancySides: MultiTenancySides.Tenant);
            //Configuracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_ControleUsuarios, L("ControleUsuarios"), multiTenancySides: MultiTenancySides.Tenant);
            //Configuracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_AuditoriaTransacoes, L("AuditoriaTransacoes"), multiTenancySides: MultiTenancySides.Tenant);
            configuracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_GeradorRelatorios, L("GeradorRelatorios"), multiTenancySides: MultiTenancySides.Tenant);
            var modulos = configuracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Modulo, L("Modulos"), multiTenancySides: MultiTenancySides.Tenant);
            modulos.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Modulo_Create, L("CreateNewModulo"), multiTenancySides: MultiTenancySides.Tenant);
            modulos.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Modulo_Edit, L("EditModulo"), multiTenancySides: MultiTenancySides.Tenant);
            modulos.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Modulo_Delete, L("DeleteModulo"), multiTenancySides: MultiTenancySides.Tenant);
            var operacoes = configuracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Operacao, L("Operacoes"), multiTenancySides: MultiTenancySides.Tenant);
            operacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Operacao_Create, L("CreateNewOperacao"), multiTenancySides: MultiTenancySides.Tenant);
            operacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Operacao_Edit, L("EditOperacao"), multiTenancySides: MultiTenancySides.Tenant);
            operacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Operacao_Delete, L("DeleteOperacao"), multiTenancySides: MultiTenancySides.Tenant);

            var crudEmpresas = empresas.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Empresa_Create, L("CreateNewEmpresa"), multiTenancySides: MultiTenancySides.Tenant);
            crudEmpresas.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Empresa_Edit, L("EditEmpresa"), multiTenancySides: MultiTenancySides.Tenant);
            crudEmpresas.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_Empresa_Delete, L("DeleteEmpresa"), multiTenancySides: MultiTenancySides.Tenant);

            var crudGeradorFormularios = formulariosPersonalizados.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_GeradorFormulario_Create, L("CreateNewGeradorFormulario"), multiTenancySides: MultiTenancySides.Tenant);
            crudGeradorFormularios.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_GeradorFormulario_Edit, L("EditGeradorFormulario"), multiTenancySides: MultiTenancySides.Tenant);
            crudGeradorFormularios.CreateChildPermission(AppPermissions.Pages_Tenant_Configuracoes_GeradorFormulario_Delete, L("DeleteGeradorFormulario"), multiTenancySides: MultiTenancySides.Tenant);
            //Cadastros
            var Cadastros = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros, L("Cadastros"), multiTenancySides: MultiTenancySides.Tenant);
            var CadastrosGlobais = Cadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais, L("CadastrosGlobais"), multiTenancySides: MultiTenancySides.Tenant);
            var CadastrosSuprimentos = Cadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos, L("CadastrosSuprimentos"), multiTenancySides: MultiTenancySides.Tenant);
            var cadastrosAssistenciais = Cadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais, L("CadastrosAssistenciais"), multiTenancySides: MultiTenancySides.Tenant);
            var DominioTiss = Cadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss, L("DominioTiss"), multiTenancySides: MultiTenancySides.Tenant);
            //var CadastrosSuprimentos = Cadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos, L("CadastrosSuprimentos"), multiTenancySides: MultiTenancySides.Tenant);

            //Prescricoes
            var cadastrosPrescricoes = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao, L("CadastrosPrescricoes"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosPrescricoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Create, L("CreateNewPrescricao"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosPrescricoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Edit, L("EditPrescricao"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosPrescricoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Delete, L("DeletePrescricao"), multiTenancySides: MultiTenancySides.Tenant);
            //Divisões
            var cadastrosDivisoes = cadastrosPrescricoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao, L("CadastrosDivisoes"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosDivisoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao_Create, L("CreateNewDivisao"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosDivisoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao_Edit, L("EditDivisao"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosDivisoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao_Delete, L("DeleteDivisao"), multiTenancySides: MultiTenancySides.Tenant);


            var cadastrosModeloPrescricao = cadastrosPrescricoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_ModeloPrescricao, L("CadastrosModeloPrescricoes"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosModeloPrescricao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_ModeloPrescricao_Create, L("CreateNewDivisao"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosModeloPrescricao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_ModeloPrescricao_Edit, L("EditDivisao"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosModeloPrescricao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_ModeloPrescricao_Delete, L("DeleteDivisao"), multiTenancySides: MultiTenancySides.Tenant);



            //Tipos de resposta
            var cadastrosTiposRespostas = cadastrosPrescricoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta, L("CadastrosTiposRespostas"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosTiposRespostas.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta_Create, L("CreateNewTipoResposta"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosTiposRespostas.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta_Edit, L("EditTipoResposta"), multiTenancySides: MultiTenancySides.Tenant);
            cadastrosTiposRespostas.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta_Delete, L("DeleteTipoResposta"), multiTenancySides: MultiTenancySides.Tenant);

            //Tipos de resposta
            var tiposRespostasConfiguracoes = cadastrosPrescricoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao, L("TipoRespostaConfiguracoes"), multiTenancySides: MultiTenancySides.Tenant);
            tiposRespostasConfiguracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao_Create, L("CreateNewTipoRespostaConfiguracao"), multiTenancySides: MultiTenancySides.Tenant);
            tiposRespostasConfiguracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao_Edit, L("EditTipoRespostaConfiguracao"), multiTenancySides: MultiTenancySides.Tenant);
            tiposRespostasConfiguracoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao_Delete, L("DeleteTipoRespostaConfiguracao"), multiTenancySides: MultiTenancySides.Tenant);

            //Itens de prescrição
            var prescricoesItens = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem, L("PrescricaoItem"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesItens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Create, L("CreateNewPrescricaoItem"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesItens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Edit, L("EditPrescricaoItem"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesItens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Delete, L("DeletePrescricaoItem"), multiTenancySides: MultiTenancySides.Tenant);

            //Status Itens de prescrição
            var prescricoesItensStatus = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus, L("PrescricaoItemStatus"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesItensStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus_Create, L("CreateNew"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesItensStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus_Edit, L("Edit"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesItensStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus_Delete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //Status Itens de prescrição
            var prescricoesStatus = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus, L("PrescricaoStatus"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus_Create, L("CreateNew"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus_Edit, L("Edit"), multiTenancySides: MultiTenancySides.Tenant);
            prescricoesStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus_Delete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //Velocidade de infusao
            var velocidadesInfusoes = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao, L("VelocidadeInfusao"), multiTenancySides: MultiTenancySides.Tenant);
            velocidadesInfusoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao_Create, L("CreateNewVelocidadeInfusao"), multiTenancySides: MultiTenancySides.Tenant);
            velocidadesInfusoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao_Edit, L("EditVelocidadeInfusao"), multiTenancySides: MultiTenancySides.Tenant);
            velocidadesInfusoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao_Delete, L("DeleteVelocidadeInfusao"), multiTenancySides: MultiTenancySides.Tenant);

            //Forma de aplicação
            var formasAplicacoes = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao, L("FormaAplicacao"), multiTenancySides: MultiTenancySides.Tenant);
            formasAplicacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao_Create, L("CreateNewFormaAplicacao"), multiTenancySides: MultiTenancySides.Tenant);
            formasAplicacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao_Edit, L("EditFormaAplicacao"), multiTenancySides: MultiTenancySides.Tenant);
            formasAplicacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao_Delete, L("DeleteFormaAplicacao"), multiTenancySides: MultiTenancySides.Tenant);

            //Frequencia
            var frequencias = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia, L("Frequencia"), multiTenancySides: MultiTenancySides.Tenant);
            frequencias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia_Create, L("CreateNewFrequencia"), multiTenancySides: MultiTenancySides.Tenant);
            frequencias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia_Edit, L("EditFrequencia"), multiTenancySides: MultiTenancySides.Tenant);
            frequencias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia_Delete, L("DeleteFrequencia"), multiTenancySides: MultiTenancySides.Tenant);

            //Fórmulas de estoque
            var formulasEstoque = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque, L("FormulaEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            formulasEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque_Create, L("CreateNewFormulaEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            formulasEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque_Edit, L("EditFormulaEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            formulasEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque_Delete, L("DeleteFormulaEstoque"), multiTenancySides: MultiTenancySides.Tenant);

            //Itens Fórmulas de estoque
            var formulasEstoqueItens = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoqueItem, L("FormulaEstoqueItem"), multiTenancySides: MultiTenancySides.Tenant);
            formulasEstoqueItens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoqueItem_Create, L("CreateNewFormulaEstoqueItem"), multiTenancySides: MultiTenancySides.Tenant);
            formulasEstoqueItens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoqueItem_Edit, L("EditFormulaEstoqueItem"), multiTenancySides: MultiTenancySides.Tenant);
            formulasEstoqueItens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoqueItem_Delete, L("DeleteFormulaEstoqueItem"), multiTenancySides: MultiTenancySides.Tenant);

            //Formula faturamento
            var formulasFaturamentos = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento, L("FormulaFaturamento"), multiTenancySides: MultiTenancySides.Tenant);
            formulasFaturamentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento_Create, L("CreateNewFormulaFaturamento"), multiTenancySides: MultiTenancySides.Tenant);
            formulasFaturamentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento_Edit, L("EditFormulaFaturamento"), multiTenancySides: MultiTenancySides.Tenant);
            formulasFaturamentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento_Delete, L("DeleteFormulaFaturamento"), multiTenancySides: MultiTenancySides.Tenant);

            //Formula exame laboratorial
            var formulasExamesLaboratoriais = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial, L("FormulaExameLaboratorial"), multiTenancySides: MultiTenancySides.Tenant);
            formulasExamesLaboratoriais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial_Create, L("CreateNewFormulaExameLaboratorial"), multiTenancySides: MultiTenancySides.Tenant);
            formulasExamesLaboratoriais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial_Edit, L("EditFormulaExameLaboratorial"), multiTenancySides: MultiTenancySides.Tenant);
            formulasExamesLaboratoriais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial_Delete, L("DeleteFormulaExameLaboratorial"), multiTenancySides: MultiTenancySides.Tenant);

            //Formula exame imagem
            var formulasExamesImagens = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem, L("FormulaExameImagem"), multiTenancySides: MultiTenancySides.Tenant);
            formulasExamesImagens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem_Create, L("CreateNewFormulaExameImagem"), multiTenancySides: MultiTenancySides.Tenant);
            formulasExamesImagens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem_Edit, L("EditFormulaExameImagem"), multiTenancySides: MultiTenancySides.Tenant);
            formulasExamesImagens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem_Delete, L("DeleteFormulaExameImagem"), multiTenancySides: MultiTenancySides.Tenant);

            //Tipo controle
            var tiposControles = cadastrosAssistenciais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle, L("TipoControle"), multiTenancySides: MultiTenancySides.Tenant);
            tiposControles.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle_Create, L("CreateNewTipoControle"), multiTenancySides: MultiTenancySides.Tenant);
            tiposControles.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle_Edit, L("EditTipoControle"), multiTenancySides: MultiTenancySides.Tenant);
            tiposControles.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle_Delete, L("DeleteTipoControle"), multiTenancySides: MultiTenancySides.Tenant);

            // Dominio TISS
            var tiposTabelaDominio = DominioTiss.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio, L("TipoTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            var gruposTipoTabelaDominio = DominioTiss.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio, L("GrupoTipoTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            var tabelasDominio = DominioTiss.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio, L("TabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            var versoesTiss = DominioTiss.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss, L("VersaoTiss"), multiTenancySides: MultiTenancySides.Tenant);


            var produtos = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto, L("Produto"), multiTenancySides: MultiTenancySides.Tenant);

            //Globais
            var pacientes = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Paciente, L("Paciente"), multiTenancySides: MultiTenancySides.Tenant);
            var medicos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Medico, L("Medico"), multiTenancySides: MultiTenancySides.Tenant);
            var especialidades = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade, L("Especialidade"), multiTenancySides: MultiTenancySides.Tenant);

            var profissoes = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Profissao, L("Profissao"), multiTenancySides: MultiTenancySides.Tenant);
            var origens = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Origem, L("Origem"), multiTenancySides: MultiTenancySides.Tenant);
            var naturalidades = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade, L("Naturalidade"), multiTenancySides: MultiTenancySides.Tenant);
            var nacionalidades = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade, L("Nacionalidade"), multiTenancySides: MultiTenancySides.Tenant);
            var convenios = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Convenio, L("Convenio"), multiTenancySides: MultiTenancySides.Tenant);
            var planos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Plano, L("Plano"), multiTenancySides: MultiTenancySides.Tenant);
            var paises = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Pais, L("Pais"), multiTenancySides: MultiTenancySides.Tenant);
            var estados = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Estado, L("Estado"), multiTenancySides: MultiTenancySides.Tenant);
            var cidades = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cidade, L("Cidade"), multiTenancySides: MultiTenancySides.Tenant);
            var ceps = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cep, L("Cep"), multiTenancySides: MultiTenancySides.Tenant);
            var GruposCentroCustos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos, L("GrupoCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            var TiposGruposCentroCustos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoGrupoCentroCustos, L("TipoGrupoCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            var elementosHtml = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml, L("ElementoHtml"), multiTenancySides: MultiTenancySides.Tenant);
            var elementosHtmlTipos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo, L("ElementoHtmlTipo"), multiTenancySides: MultiTenancySides.Tenant);
            //var CentrosCustos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CentroCustos, L("CentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            //Elementos Html
            elementosHtml.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml_Create, L("CreateNewElementoHtml"), multiTenancySides: MultiTenancySides.Tenant);
            elementosHtml.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml_Edit, L("EditElementoHtml"), multiTenancySides: MultiTenancySides.Tenant);
            elementosHtml.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml_Delete, L("DeleteElementoHtml"), multiTenancySides: MultiTenancySides.Tenant);
            //Elementos Html Tipo
            elementosHtmlTipos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo_Create, L("CreateNewElementoHtmlTipo"), multiTenancySides: MultiTenancySides.Tenant);
            elementosHtmlTipos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo_Edit, L("EditElementoHtmlTipo"), multiTenancySides: MultiTenancySides.Tenant);
            elementosHtmlTipos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo_Delete, L("DeleteElementoHtmlTipo"), multiTenancySides: MultiTenancySides.Tenant);

            var CentrosCustos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos, L("CentroCusto"), multiTenancySides: MultiTenancySides.Tenant);

            var intervalos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo, L("Intervalo"), multiTenancySides: MultiTenancySides.Tenant);
            var tiposAcomodacao = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao, L("TipoAcomodacao"), multiTenancySides: MultiTenancySides.Tenant);
            var fornecedores = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor, L("Fornecedor"), multiTenancySides: MultiTenancySides.Tenant);
            var Regiao = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Regioes, L("Regiao"), multiTenancySides: MultiTenancySides.Tenant);
            var UnidadesOrganizacionais = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_UnidadeOrganizacional, L("UnidadeOrganizacional"), multiTenancySides: MultiTenancySides.Tenant);
            var grausInstrucoes = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao, L("GrauInstrucao"), multiTenancySides: MultiTenancySides.Tenant);
            var indicacoes = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao, L("Indicacao"), multiTenancySides: MultiTenancySides.Tenant);
            var parentescos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco, L("Parentesco"), multiTenancySides: MultiTenancySides.Tenant);
            var feriados = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Feriado, L("Feriado"), multiTenancySides: MultiTenancySides.Tenant);
            var tiposPrestadores = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoPrestador, L("TipoPrestador"), multiTenancySides: MultiTenancySides.Tenant);


            var categorias = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Categoria, L("Categoria"), multiTenancySides: MultiTenancySides.Tenant);
            var bancos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Banco, L("Banco"), multiTenancySides: MultiTenancySides.Tenant);

            var pacientesConveniosBloqueados = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PacienteConvenioBloqueado, L("PacienteConvenioBloqueado"), multiTenancySides: MultiTenancySides.Tenant);
            var gruposProcedimentos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoProcedimento, L("GrupoProcedimento"), multiTenancySides: MultiTenancySides.Tenant);

            var tiposVinculosEmpregativos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio, L("TipoVinculoEmpregativo"), multiTenancySides: MultiTenancySides.Tenant);

            var tiposParticipacoes = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao, L("TipoParticipacao"), multiTenancySides: MultiTenancySides.Tenant);
            var prestadores = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Prestador, L("Prestador"), multiTenancySides: MultiTenancySides.Tenant);
            var prestadoresCredenciamentos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorCredenciamento, L("PrestadorCredenciamento"), multiTenancySides: MultiTenancySides.Tenant);
            var prestadoresGruposProcedimenmtos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorGrupoProcedimento, L("PrestadorGrupoProcedimento"), multiTenancySides: MultiTenancySides.Tenant);
            var tiposSanguineos = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo, L("TipoSanguineo"), multiTenancySides: MultiTenancySides.Tenant);


            //Produto Ação Terapeutica
            var produtoAcaoTerapeutica = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica, L("ProdutoAcaoTerapeutica"), multiTenancySides: MultiTenancySides.Tenant);
            var crudProdutoAcaoTerapeutica = produtoAcaoTerapeutica.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica_Create, L("CreateNewProdutoAcaoTerapeutica"), multiTenancySides: MultiTenancySides.Tenant);
            crudProdutoAcaoTerapeutica = produtoAcaoTerapeutica.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica_Edit, L("EditProdutoAcaoTerapeutica"), multiTenancySides: MultiTenancySides.Tenant);
            crudProdutoAcaoTerapeutica = produtoAcaoTerapeutica.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica_Delete, L("DeleteProdutoAcaoTerapeutica"), multiTenancySides: MultiTenancySides.Tenant);


            //Tipos Logradouro
            var tiposLogradouro = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro, L("TiposLogradouro"), multiTenancySides: MultiTenancySides.Tenant);
            var crudTiposLogradouro = tiposLogradouro.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro_Create, L("CreateNewTipoLogradouro"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposLogradouro = tiposLogradouro.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro_Edit, L("EditTipoLogradouro"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposLogradouro = tiposLogradouro.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro_Delete, L("DeleteTipoLogradouro"), multiTenancySides: MultiTenancySides.Tenant);

            //Palavra Chave
            var palavraChaves = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave, L("PalavraChave"), multiTenancySides: MultiTenancySides.Tenant);
            var crudPalavraChaves = palavraChaves.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Create, L("CreateNewPalavraChave"), multiTenancySides: MultiTenancySides.Tenant);
            crudPalavraChaves = palavraChaves.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Edit, L("EditPalavraChave"), multiTenancySides: MultiTenancySides.Tenant);
            crudPalavraChaves = palavraChaves.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Delete, L("DeletePalavraChave"), multiTenancySides: MultiTenancySides.Tenant);

            var crudPacientes = pacientes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Paciente_Create, L("CreateNewPaciente"), multiTenancySides: MultiTenancySides.Tenant);
            crudPacientes = pacientes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Paciente_Edit, L("EditPaciente"), multiTenancySides: MultiTenancySides.Tenant);
            crudPacientes = pacientes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Paciente_Delete, L("DeletePaciente"), multiTenancySides: MultiTenancySides.Tenant);

            //Produto
            var crudProdutos = produtos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto_Create, L("CreateNewProduto"), multiTenancySides: MultiTenancySides.Tenant);
            crudProdutos = produtos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto_Edit, L("EditProduto"), multiTenancySides: MultiTenancySides.Tenant);
            crudProdutos = produtos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto_Delete, L("DeleteProduto"), multiTenancySides: MultiTenancySides.Tenant);

            //Kit Estoque
            var kitEstoque = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoque, L("KitEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            var crudKitEstoque = kitEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoque_Create, L("CreateNewKitEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            crudKitEstoque = kitEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoque_Edit, L("EditKitEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            crudKitEstoque = kitEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoque_Delete, L("DeleteKitEstoque"), multiTenancySides: MultiTenancySides.Tenant);

            //Kit Estoque Item
            var kitEstoqueItem = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoqueItem, L("KitEstoqueItem"), multiTenancySides: MultiTenancySides.Tenant);
            var crudKitEstoqueItem = kitEstoqueItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoqueItem_Create, L("CreateNewKitEstoqueItem"), multiTenancySides: MultiTenancySides.Tenant);
            crudKitEstoqueItem = kitEstoqueItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoqueItem_Edit, L("EditKitEstoqueItem"), multiTenancySides: MultiTenancySides.Tenant);
            crudKitEstoqueItem = kitEstoqueItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoqueItem_Delete, L("DeleteKitEstoqueItem"), multiTenancySides: MultiTenancySides.Tenant);

            //Acao Terapeutica
            var acoesTerapeutica = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica, L("AcaoTerapeutica"), multiTenancySides: MultiTenancySides.Tenant);
            var crudAcoesTerapeutica = acoesTerapeutica.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica_Create, L("CreateNewAcaoTerapeutica"), multiTenancySides: MultiTenancySides.Tenant);
            crudAcoesTerapeutica = acoesTerapeutica.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica_Edit, L("EditAcaoTerapeutica"), multiTenancySides: MultiTenancySides.Tenant);
            crudAcoesTerapeutica = acoesTerapeutica.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica_Delete, L("DeleteAcaoTerapeutica"), multiTenancySides: MultiTenancySides.Tenant);

            //Grupo
            var grupos = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo, L("Grupo"), multiTenancySides: MultiTenancySides.Tenant);
            var crudGrupos = grupos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Create, L("CreateNewGrupo"), multiTenancySides: MultiTenancySides.Tenant);
            crudGrupos = grupos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Edit, L("EditGrupo"), multiTenancySides: MultiTenancySides.Tenant);
            crudGrupos = grupos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Delete, L("DeleteGrupo"), multiTenancySides: MultiTenancySides.Tenant);

            //Laboratorio
            var produtosLaboratorio = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio, L("Laboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            var crudProdutosLaboratorio = produtosLaboratorio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Create, L("CreateNewLaboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            crudProdutosLaboratorio = produtosLaboratorio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Edit, L("EditLaboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            crudProdutosLaboratorio = produtosLaboratorio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Delete, L("DeleteLaboratorio"), multiTenancySides: MultiTenancySides.Tenant);


            //Portaria
            var portarias = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria, L("Portaria"), multiTenancySides: MultiTenancySides.Tenant);
            var crudPortarias = portarias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Create, L("CreateNewPortaria"), multiTenancySides: MultiTenancySides.Tenant);
            crudPortarias = portarias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Edit, L("EditPortaria"), multiTenancySides: MultiTenancySides.Tenant);
            crudPortarias = portarias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Delete, L("DeletePortaria"), multiTenancySides: MultiTenancySides.Tenant);

            //Grupo Tratamento
            var gruposTratamento = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento, L("GrupoTratamento"), multiTenancySides: MultiTenancySides.Tenant);
            var crudGruposTratamento = gruposTratamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Create, L("CreateNewGrupoTratamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudGruposTratamento = gruposTratamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Edit, L("EditGrupoTratamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudGruposTratamento = gruposTratamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Delete, L("DeleteGrupoTratamento"), multiTenancySides: MultiTenancySides.Tenant);

            //Localizacao Produto
            var localizacoesProduto = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto, L("LocalizacaoProduto"), multiTenancySides: MultiTenancySides.Tenant);
            var crudLocalizacoesProduto = localizacoesProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Create, L("CreateNewLocalizacaoProduto"), multiTenancySides: MultiTenancySides.Tenant);
            crudLocalizacoesProduto = localizacoesProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Edit, L("EditLocalizacaoProduto"), multiTenancySides: MultiTenancySides.Tenant);
            crudLocalizacoesProduto = localizacoesProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Delete, L("DeleteLocalizacaoProduto"), multiTenancySides: MultiTenancySides.Tenant);

            //Unidades
            var unidades = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade, L("Unidade"), multiTenancySides: MultiTenancySides.Tenant);
            var crudUnidades = unidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Create, L("CreateNewUnidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudUnidades = unidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Edit, L("EditUnidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudUnidades = unidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Delete, L("DeleteUnidade"), multiTenancySides: MultiTenancySides.Tenant);

            //TiposUnidade
            var tiposUnidade = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade, L("TipoUnidade"), multiTenancySides: MultiTenancySides.Tenant);
            var crudTiposUnidade = tiposUnidade.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Create, L("CreateNewTipoUnidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposUnidade = tiposUnidade.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Edit, L("EditTipoUnidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposUnidade = tiposUnidade.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Delete, L("DeleteTipoUnidade"), multiTenancySides: MultiTenancySides.Tenant);

            //Codigos Medicamentos
            var codigosMedicamento = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento, L("CodigoMedicamento"), multiTenancySides: MultiTenancySides.Tenant);
            var crudCodigosMedicamento = codigosMedicamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Create, L("CreateNewCodigoMedicamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudCodigosMedicamento = codigosMedicamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Edit, L("EditCodigoMedicamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudCodigosMedicamento = codigosMedicamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Delete, L("DeleteCodigoMedicamento"), multiTenancySides: MultiTenancySides.Tenant);

            //Especie
            var estoques = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque, L("Estoque"), multiTenancySides: MultiTenancySides.Tenant);
            var crudEstoques = estoques.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Create, L("CreateNewEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            crudEstoques = estoques.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Edit, L("EditEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            crudEstoques = estoques.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Delete, L("DeleteEstoque"), multiTenancySides: MultiTenancySides.Tenant);

            //Classe
            var classes = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe, L("Classe"), multiTenancySides: MultiTenancySides.Tenant);
            var crudClasses = classes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Create, L("CreateNewClasse"), multiTenancySides: MultiTenancySides.Tenant);
            crudClasses = classes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Edit, L("EditClasse"), multiTenancySides: MultiTenancySides.Tenant);
            crudClasses = classes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Delete, L("DeleteClasse"), multiTenancySides: MultiTenancySides.Tenant);

            //SubClasse
            var subClasses = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse, L("SubClasse"), multiTenancySides: MultiTenancySides.Tenant);
            var crudSubClasses = subClasses.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Create, L("CreateNewSubClasse"), multiTenancySides: MultiTenancySides.Tenant);
            crudSubClasses = subClasses.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Edit, L("EditSubClasse"), multiTenancySides: MultiTenancySides.Tenant);
            crudSubClasses = subClasses.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Delete, L("DeleteSubClasse"), multiTenancySides: MultiTenancySides.Tenant);

            //Substancia
            var substancias = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia, L("Substancia"), multiTenancySides: MultiTenancySides.Tenant);
            var crudSubstancias = substancias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Create, L("CreateNewSubstancia"), multiTenancySides: MultiTenancySides.Tenant);
            crudSubstancias = substancias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Edit, L("EditSubstancia"), multiTenancySides: MultiTenancySides.Tenant);
            crudSubstancias = substancias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Delete, L("DeleteSubstancia"), multiTenancySides: MultiTenancySides.Tenant);

            var tiposLeito = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito, L("TiposLeito"), multiTenancySides: MultiTenancySides.Tenant);
            var crudTiposLeito = tiposLeito.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Create, L("CreateNewTipoLeito"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposLeito = tiposLeito.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Edit, L("EditTipoLeito"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposLeito = tiposLeito.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Delete, L("DeleteTipoLeito"), multiTenancySides: MultiTenancySides.Tenant);

            var crudMedicos = medicos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Medico_Create, L("CreateNewMedico"), multiTenancySides: MultiTenancySides.Tenant);
            crudMedicos = medicos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Medico_Edit, L("EditMedico"), multiTenancySides: MultiTenancySides.Tenant);
            crudMedicos = medicos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Medico_Delete, L("DeleteMedico"), multiTenancySides: MultiTenancySides.Tenant);

            var crudEspecidalidades = especialidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade_Create, L("CreateNewEspecialidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudEspecidalidades = especialidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade_Edit, L("EditEspecialidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudEspecidalidades = especialidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade_Delete, L("DeleteEspecialidade"), multiTenancySides: MultiTenancySides.Tenant);

            var crudProfissoes = profissoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Profissao_Create, L("CreateNewProfissao"), multiTenancySides: MultiTenancySides.Tenant);
            crudProfissoes = profissoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Profissao_Edit, L("EditProfissao"), multiTenancySides: MultiTenancySides.Tenant);
            crudProfissoes = profissoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Profissao_Delete, L("DeleteProfissao"), multiTenancySides: MultiTenancySides.Tenant);

            //Origem
            var crudOrigens = origens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Origem_Create, L("CreateNewOrigem"), multiTenancySides: MultiTenancySides.Tenant);
            crudOrigens = origens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Origem_Edit, L("EditOrigem"), multiTenancySides: MultiTenancySides.Tenant);
            crudOrigens = origens.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Origem_Delete, L("DeleteOrigem"), multiTenancySides: MultiTenancySides.Tenant);

            var crudNaturalidades = naturalidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade_Create, L("CreateNewNaturalidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudNaturalidades = naturalidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade_Edit, L("EditNaturalidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudNaturalidades = naturalidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade_Delete, L("DeleteNaturalidade"), multiTenancySides: MultiTenancySides.Tenant);

            //Nacionalidade
            var crudNacionalidades = nacionalidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade_Create, L("CreateNewNacionalidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudNacionalidades = nacionalidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade_Edit, L("EditNacionalidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudNacionalidades = nacionalidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade_Delete, L("DeleteNacionalidade"), multiTenancySides: MultiTenancySides.Tenant);

            //Grau de instrução
            var crudGrausIntrocoes = grausInstrucoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao_Create, L("CreateNewNacionalidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudGrausIntrocoes = grausInstrucoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao_Edit, L("EditNacionalidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudGrausIntrocoes = grausInstrucoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao_Delete, L("DeleteNacionalidade"), multiTenancySides: MultiTenancySides.Tenant);

            //Indicacão
            var crudIndicacoes = indicacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao_Create, L("CreateNewGrauInstrucao"), multiTenancySides: MultiTenancySides.Tenant);
            crudIndicacoes = indicacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao_Edit, L("EditGrauInstrucao"), multiTenancySides: MultiTenancySides.Tenant);
            crudIndicacoes = indicacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao_Delete, L("DeleteGrauInstrucao"), multiTenancySides: MultiTenancySides.Tenant);

            //parentesco
            var crudParentescos = parentescos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco_Create, L("CreateNewParentesco"), multiTenancySides: MultiTenancySides.Tenant);
            crudParentescos = parentescos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco_Edit, L("EditParentesco"), multiTenancySides: MultiTenancySides.Tenant);
            crudParentescos = parentescos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco_Delete, L("DeleteParentesco"), multiTenancySides: MultiTenancySides.Tenant);

            //Feriado
            var crudFeriados = feriados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Feriado_Create, L("CreateNewFeriado"), multiTenancySides: MultiTenancySides.Tenant);
            crudFeriados = feriados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Feriado_Edit, L("EditFeriado"), multiTenancySides: MultiTenancySides.Tenant);
            crudFeriados = feriados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Feriado_Delete, L("DeleteFeriado"), multiTenancySides: MultiTenancySides.Tenant);

            //Capitulo Cid
            var capitulosCid = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID, L("CapituloCID"), multiTenancySides: MultiTenancySides.Tenant);
            var crudCapitulosCid = capitulosCid.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID_Create, L("CreateNewTipoCapituloCID"), multiTenancySides: MultiTenancySides.Tenant);
            crudCapitulosCid = capitulosCid.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID_Edit, L("EditTipoCapituloCID"), multiTenancySides: MultiTenancySides.Tenant);
            crudCapitulosCid = capitulosCid.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID_Delete, L("DeleteCapituloCID"), multiTenancySides: MultiTenancySides.Tenant);

            //Grupo Cid
            var gruposCid = CadastrosGlobais.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID, L("GrupoCID"), multiTenancySides: MultiTenancySides.Tenant);
            var crudGruposCid = gruposCid.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID_Create, L("CreateNewTipoGrupoCID"), multiTenancySides: MultiTenancySides.Tenant);
            crudGruposCid = gruposCid.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID_Edit, L("EditTipoGrupoCID"), multiTenancySides: MultiTenancySides.Tenant);
            crudGruposCid = gruposCid.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID_Delete, L("DeleteGrupoCID"), multiTenancySides: MultiTenancySides.Tenant);

            //Categoria
            var crudCategorias = categorias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Categoria_Create, L("CreateNewCategoria"), multiTenancySides: MultiTenancySides.Tenant);
            crudCategorias = categorias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Categoria_Edit, L("EditCategoria"), multiTenancySides: MultiTenancySides.Tenant);
            crudCategorias = categorias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Categoria_Delete, L("DeleteCategoria"), multiTenancySides: MultiTenancySides.Tenant);

            //Tipo Sanguineo
            var crudSanguineos = tiposSanguineos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo_Create, L("CreateNewTipoSanguineo"), multiTenancySides: MultiTenancySides.Tenant);
            crudSanguineos = tiposSanguineos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo_Edit, L("EditTipoSanguineo"), multiTenancySides: MultiTenancySides.Tenant);
            crudSanguineos = tiposSanguineos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo_Delete, L("DeleteTipoSanguineo"), multiTenancySides: MultiTenancySides.Tenant);

            //Paciente Convenio Bloqueado
            var crudPacientesConvenioBloqueados = pacientesConveniosBloqueados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PacienteConvenioBloqueado_Create, L("CreateNewPacienteConvenioBloqueado"), multiTenancySides: MultiTenancySides.Tenant);
            crudPacientesConvenioBloqueados = pacientesConveniosBloqueados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PacienteConvenioBloqueado_Edit, L("EditPacienteConvenioBloqueado"), multiTenancySides: MultiTenancySides.Tenant);
            crudPacientesConvenioBloqueados = pacientesConveniosBloqueados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PacienteConvenioBloqueado_Delete, L("DeletePacienteConvenioBloqueado"), multiTenancySides: MultiTenancySides.Tenant);

            //Grupo Procedimento
            var crudGruposProcedimentos = gruposProcedimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoProcedimento_Create, L("CreateNewGrupoProcedimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudGruposProcedimentos = gruposProcedimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoProcedimento_Edit, L("EditGrupoProcedimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudGruposProcedimentos = gruposProcedimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoProcedimento_Delete, L("DeleteGrupoProcedimento"), multiTenancySides: MultiTenancySides.Tenant);

            //TiposVinculosEmpregativos
            var crudTiposVinculosEmpregativos = tiposVinculosEmpregativos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio_Create, L("CreateNewTipoVinculoEmpregativo"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposVinculosEmpregativos = tiposVinculosEmpregativos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio_Edit, L("EditTipoVinculoEmpregativo"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposVinculosEmpregativos = tiposVinculosEmpregativos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio_Delete, L("DeleteTipoVinculoEmpregativo"), multiTenancySides: MultiTenancySides.Tenant);

            //tipoParticipacao
            var crudTipoParticipacao = tiposParticipacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao_Create, L("CreateNewTipoParticipacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudTipoParticipacao = tiposParticipacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao_Edit, L("EditTipoParticipacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudTipoParticipacao = tiposParticipacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao_Delete, L("DeleteTipoParticipacao"), multiTenancySides: MultiTenancySides.Tenant);

            //Prestador
            var crudPrestador = prestadores.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Prestador_Create, L("CreateNewPrestador"), multiTenancySides: MultiTenancySides.Tenant);
            crudPrestador = prestadores.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Prestador_Edit, L("EditPrestador"), multiTenancySides: MultiTenancySides.Tenant);
            crudPrestador = prestadores.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Prestador_Delete, L("DeletePrestador"), multiTenancySides: MultiTenancySides.Tenant);

            //prestadorCredenciamento
            var crudPrestadorCredenciamento = prestadoresCredenciamentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorCredenciamento_Create, L("CreateNewPrestadorCredenciamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudPrestadorCredenciamento = prestadoresCredenciamentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorCredenciamento_Edit, L("EditPrestadorCredenciamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudPrestadorCredenciamento = prestadoresCredenciamentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorCredenciamento_Delete, L("DeletePrestadorCredenciamento"), multiTenancySides: MultiTenancySides.Tenant);

            //prestadores Grupos Procedimenmtos
            var crudPrestadoresGruposProcedimenmtos = prestadoresGruposProcedimenmtos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorGrupoProcedimento_Create, L("CreateNewPrestadorGrupoProcedimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudPrestadoresGruposProcedimenmtos = prestadoresGruposProcedimenmtos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorGrupoProcedimento_Edit, L("EditPrestadorGrupoProcedimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudPrestadoresGruposProcedimenmtos = prestadoresGruposProcedimenmtos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorGrupoProcedimento_Delete, L("DeletePrestadorGrupoProcedimento"), multiTenancySides: MultiTenancySides.Tenant);

            var crudConvenios = convenios.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Convenio_Create, L("CreateNewConvenio"), multiTenancySides: MultiTenancySides.Tenant);
            crudConvenios = convenios.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Convenio_Edit, L("EditConvenio"), multiTenancySides: MultiTenancySides.Tenant);
            crudConvenios = convenios.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Convenio_Delete, L("DeleteConvenio"), multiTenancySides: MultiTenancySides.Tenant);

            var crudPlanos = planos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Plano_Create, L("CreateNewPlano"), multiTenancySides: MultiTenancySides.Tenant);
            crudPlanos = planos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Plano_Edit, L("EditPlano"), multiTenancySides: MultiTenancySides.Tenant);
            crudPlanos = planos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Plano_Delete, L("DeletePlano"), multiTenancySides: MultiTenancySides.Tenant);

            var crudPaises = paises.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Pais_Create, L("CreateNewPais"), multiTenancySides: MultiTenancySides.Tenant);
            crudPaises = paises.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Pais_Edit, L("EditPais"), multiTenancySides: MultiTenancySides.Tenant);
            crudPaises = paises.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Pais_Delete, L("DeletePais"), multiTenancySides: MultiTenancySides.Tenant);

            var crudEstados = estados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Estado_Create, L("CreateNewEstado"), multiTenancySides: MultiTenancySides.Tenant);
            crudEstados = estados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Estado_Edit, L("EditEstado"), multiTenancySides: MultiTenancySides.Tenant);
            crudEstados = estados.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Estado_Delete, L("DeleteEstado"), multiTenancySides: MultiTenancySides.Tenant);

            var crudCidades = cidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cidade_Create, L("CreateNewCidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudCidades = cidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cidade_Edit, L("EditCidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudCidades = cidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cidade_Delete, L("DeleteCidade"), multiTenancySides: MultiTenancySides.Tenant);

            //CRUD CEP
            var crudCeps = ceps.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cep_Create, L("CreateNewCep"), multiTenancySides: MultiTenancySides.Tenant);
            crudCeps = ceps.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cep_Edit, L("EditCep"), multiTenancySides: MultiTenancySides.Tenant);
            crudCeps = ceps.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cep_Delete, L("DeleteCep"), multiTenancySides: MultiTenancySides.Tenant);

            //CRUD Tipo de Grupo Centro de Custos
            var crudTipoGrupoCentroCustos = TiposGruposCentroCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoGrupoCentroCustos_Create, L("CreateNewTipoGrupoCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            crudTipoGrupoCentroCustos = TiposGruposCentroCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoGrupoCentroCustos_Edit, L("EditTipoGrupoCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            crudTipoGrupoCentroCustos = TiposGruposCentroCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoGrupoCentroCustos_Delete, L("DeleteTipoGrupoCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);

            //CRUD Centro de Custos
            var crudCentrosCustos = CentrosCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Create, L("CreateNewCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            crudCentrosCustos = CentrosCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Edit, L("EditCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            crudCentrosCustos = CentrosCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Delete, L("DeleteCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);

            //CRUD Grupo Centro de Custos
            var crudGrupoCentroCustos = GruposCentroCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos_Create, L("CreateNewGrupoCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            crudGrupoCentroCustos = GruposCentroCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos_Edit, L("EditGrupoCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);
            crudGrupoCentroCustos = GruposCentroCustos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos_Delete, L("DeleteGrupoCentroCusto"), multiTenancySides: MultiTenancySides.Tenant);

            var crudIntervalos = intervalos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo_Create, L("CreateNewIntervalo"), multiTenancySides: MultiTenancySides.Tenant);
            crudIntervalos = intervalos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo_Edit, L("EditIntervalo"), multiTenancySides: MultiTenancySides.Tenant);
            crudIntervalos = intervalos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo_Delete, L("DeleteIntervalo"), multiTenancySides: MultiTenancySides.Tenant);


            var atendimentoCadastros = Cadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento, L("Atendimento"), multiTenancySides: MultiTenancySides.Tenant);
            var tiposAtendimento = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposAtendimento, L("TipoAtendimento"), multiTenancySides: MultiTenancySides.Tenant);

            var instituicaoTransferencia = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia, L("InstituicaoTransferencia"), multiTenancySides: MultiTenancySides.Tenant);
            var motivosCancelamento = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento, L("MotivoCancelamento"), multiTenancySides: MultiTenancySides.Tenant);
            var motivosCaucao = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao, L("MotivoCaucao"), multiTenancySides: MultiTenancySides.Tenant);
            var motivosTransferenciaLeito = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito, L("MotivoTransferenciaLeito"), multiTenancySides: MultiTenancySides.Tenant);

            var agendamentoConsultaMedicoDisponibilidades = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade, L("AgendamentoConsultaMedicoDisponibilidade"), multiTenancySides: MultiTenancySides.Tenant);

            var crudTiposAtendimento = tiposAtendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Create, L("CreateNewTipoAtendimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposAtendimento = tiposAtendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Edit, L("EditTipoAtendimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposAtendimento = tiposAtendimento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Delete, L("DeleteTipoAtendimento"), multiTenancySides: MultiTenancySides.Tenant);

            var crudAgendamentoConsultaMedicoDisponibilidades = agendamentoConsultaMedicoDisponibilidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade_Create, L("CreateNewAgendamentoConsultaMedicoDisponibilidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudAgendamentoConsultaMedicoDisponibilidades = agendamentoConsultaMedicoDisponibilidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade_Edit, L("EditAgendamentoConsultaMedicoDisponibilidade"), multiTenancySides: MultiTenancySides.Tenant);
            crudAgendamentoConsultaMedicoDisponibilidades = agendamentoConsultaMedicoDisponibilidades.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade_Delete, L("DeleteAgendamentoConsultaMedicoDisponibilidade"), multiTenancySides: MultiTenancySides.Tenant);
            var crudRegioes = Regiao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Create, L("CreateNewRegiao"), multiTenancySides: MultiTenancySides.Tenant);
            crudRegioes = Regiao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Edit, L("EditRegiao"), multiTenancySides: MultiTenancySides.Tenant);
            crudRegioes = Regiao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Delete, L("DeleteRegiao"), multiTenancySides: MultiTenancySides.Tenant);

            var crudTiposAcomodacao = tiposAcomodacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Create, L("CreateNewTipoAcomodacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposAcomodacao = tiposAcomodacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Edit, L("EditTipoAcomodacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposAcomodacao = tiposAcomodacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Delete, L("DeleteTipoAcomodacao"), multiTenancySides: MultiTenancySides.Tenant);

            var crudFornecedores = fornecedores.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor_Create, L("CreateNewFornecedor"), multiTenancySides: MultiTenancySides.Tenant);
            crudFornecedores = fornecedores.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor_Edit, L("EditFornecedor"), multiTenancySides: MultiTenancySides.Tenant);
            crudFornecedores = fornecedores.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor_Delete, L("DeleteFornecedor"), multiTenancySides: MultiTenancySides.Tenant);

            var crudMotivosCancelamento = motivosCancelamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Create, L("CreateNewMotivoCancelamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudMotivosCancelamento = motivosCancelamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Edit, L("EditMotivoCancelamento"), multiTenancySides: MultiTenancySides.Tenant);
            crudMotivosCancelamento = motivosCancelamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Delete, L("DeleteMotivoCancelamento"), multiTenancySides: MultiTenancySides.Tenant);

            var crudInstituicaoTransferencia = instituicaoTransferencia.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Create, L("CreateNewInstituicaoTransferencia"), multiTenancySides: MultiTenancySides.Tenant);
            crudInstituicaoTransferencia = instituicaoTransferencia.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Edit, L("EditMotivoInstituicaoTransferencia"), multiTenancySides: MultiTenancySides.Tenant);
            crudInstituicaoTransferencia = instituicaoTransferencia.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Delete, L("DeleteMotivoInstituicaoTransferencia"), multiTenancySides: MultiTenancySides.Tenant);

            var crudMotivosCaucao = motivosCaucao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Create, L("CreateNewMotivoCaucao"), multiTenancySides: MultiTenancySides.Tenant);
            crudMotivosCaucao = motivosCaucao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Edit, L("EditMotivoMotivoCaucao"), multiTenancySides: MultiTenancySides.Tenant);
            crudMotivosCaucao = motivosCaucao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Delete, L("DeleteMotivoMotivoCaucao"), multiTenancySides: MultiTenancySides.Tenant);

            var crudMotivosTransferenciaLeito = motivosTransferenciaLeito.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Create, L("CreateNewMotivoTransferenciaLeito"), multiTenancySides: MultiTenancySides.Tenant);
            crudMotivosTransferenciaLeito = motivosTransferenciaLeito.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Edit, L("EditMotivoMotivoTransferenciaLeito"), multiTenancySides: MultiTenancySides.Tenant);
            crudMotivosTransferenciaLeito = motivosTransferenciaLeito.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Delete, L("DeleteMotivoMotivoTransferenciaLeito"), multiTenancySides: MultiTenancySides.Tenant);


            // Dominio TISS
            var crudTiposTabelaDominio = tiposTabelaDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, L("CreateNewTipoTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposTabelaDominio = tiposTabelaDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit, L("EditTipoTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            crudTiposTabelaDominio = tiposTabelaDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Delete, L("DeleteTipoTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);

            var crudGruposTipoTabelaDominio = gruposTipoTabelaDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Create, L("CreateNewGrupoTipoTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            crudGruposTipoTabelaDominio = gruposTipoTabelaDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Edit, L("EditGrupoTipoTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            crudGruposTipoTabelaDominio = gruposTipoTabelaDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Delete, L("DeleteGrupoTipoTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);

            var crudTabelasDominio = tabelasDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Create, L("CreateNewTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            crudTabelasDominio = tabelasDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Edit, L("EditTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);
            crudTabelasDominio = tabelasDominio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Delete, L("DeleteTabelaDominio"), multiTenancySides: MultiTenancySides.Tenant);

            var crudVersoesTiss = versoesTiss.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Create, L("CreateNewVersaoTiss"), multiTenancySides: MultiTenancySides.Tenant);
            crudVersoesTiss = versoesTiss.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Edit, L("EditVersaoTiss"), multiTenancySides: MultiTenancySides.Tenant);
            crudVersoesTiss = versoesTiss.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Delete, L("DeleteVersaoTiss"), multiTenancySides: MultiTenancySides.Tenant);

            // Cadastros Atendimento
            //     var atendimento = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento, L("Atendimento"), multiTenancySides: MultiTenancySides.Tenant);
            var crudAtendimento = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Create, L("CreateNewAtendimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudAtendimento = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Edit, L("EditAtendimento"), multiTenancySides: MultiTenancySides.Tenant);
            crudAtendimento = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Delete, L("DeleteAtendimento"), multiTenancySides: MultiTenancySides.Tenant);

            var motivosAlta = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MotivosAlta, L("MotivoAlta"), multiTenancySides: MultiTenancySides.Tenant);
            var crudMotivosAlta = motivosAlta.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MotivosAlta_Create, L("CreateNewMotivoAlta"), multiTenancySides: MultiTenancySides.Tenant);
            crudMotivosAlta = motivosAlta.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MotivosAlta_Edit, L("EditMotivoAlta"), multiTenancySides: MultiTenancySides.Tenant);
            crudMotivosAlta = motivosAlta.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MotivosAlta_Delete, L("DeleteMotivoAlta"), multiTenancySides: MultiTenancySides.Tenant);

            var leitos = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Leitos, L("Leito"), multiTenancySides: MultiTenancySides.Tenant);
            var crudLeitos = leitos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Leitos_Create, L("CreateNewLeito"), multiTenancySides: MultiTenancySides.Tenant);
            crudLeitos = leitos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Leitos_Edit, L("EditLeito"), multiTenancySides: MultiTenancySides.Tenant);
            crudLeitos = leitos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Leitos_Delete, L("DeleteLeito"), multiTenancySides: MultiTenancySides.Tenant);

            var leitosStatus = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitosStatus, L("LeitoStatus"), multiTenancySides: MultiTenancySides.Tenant);
            var crudLeitosStatus = leitosStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitosStatus_Create, L("CreateNewLeitoStatus"), multiTenancySides: MultiTenancySides.Tenant);
            crudLeitosStatus = leitosStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitosStatus_Edit, L("EditLeitoStatus"), multiTenancySides: MultiTenancySides.Tenant);
            crudLeitosStatus = leitosStatus.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitosStatus_Delete, L("DeleteLeitoStatus"), multiTenancySides: MultiTenancySides.Tenant);

            var leitoCaracteristicas = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas, L("LeitoCaracteristica"), multiTenancySides: MultiTenancySides.Tenant);
            var crudLeitoCaracteristicas = leitoCaracteristicas.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas_Create, L("CreateNewLeitoCaracteristica"), multiTenancySides: MultiTenancySides.Tenant);
            crudLeitoCaracteristicas = leitoCaracteristicas.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas_Edit, L("EditLeitoCaracteristica"), multiTenancySides: MultiTenancySides.Tenant);
            crudLeitoCaracteristicas = leitoCaracteristicas.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas_Delete, L("DeleteLeitoCaracteristica"), multiTenancySides: MultiTenancySides.Tenant);

            var leitoServicos = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoServicos, L("LeitoServico"), multiTenancySides: MultiTenancySides.Tenant);
            var crudLeitoServicos = leitoServicos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoServicos_Create, L("CreateNewLeitoServico"), multiTenancySides: MultiTenancySides.Tenant);
            crudLeitoServicos = leitoServicos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoServicos_Edit, L("EditLeitoServico"), multiTenancySides: MultiTenancySides.Tenant);
            crudLeitoServicos = leitoServicos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoServicos_Delete, L("DeleteLeitoServico"), multiTenancySides: MultiTenancySides.Tenant);

            var unidadesInternacao = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao, L("UnidadeInternacao"), multiTenancySides: MultiTenancySides.Tenant);
            var crudUnidadesInternacao = unidadesInternacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Create, L("CreateNewUnidadeInternacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudUnidadesInternacao = unidadesInternacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Edit, L("EditUnidadeInternacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudUnidadesInternacao = unidadesInternacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Delete, L("DeleteUnidadeInternacao"), multiTenancySides: MultiTenancySides.Tenant);


            var painelSenha = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha, L("PainelSenha"), multiTenancySides: MultiTenancySides.Tenant);
            var crudpainelSenha = painelSenha.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha_Create, L("CreateNewPainelSenha"), multiTenancySides: MultiTenancySides.Tenant);
            crudpainelSenha = painelSenha.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha_Edit, L("EditPainelSenha"), multiTenancySides: MultiTenancySides.Tenant);
            crudpainelSenha = painelSenha.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha_Delete, L("DeletePainelSenha"), multiTenancySides: MultiTenancySides.Tenant);

            var fila = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila, L("PainelSenha"), multiTenancySides: MultiTenancySides.Tenant);
            var crudfila = fila.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila_Create, L("CreateNewPainelSenha"), multiTenancySides: MultiTenancySides.Tenant);
            crudfila = fila.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila_Edit, L("EditPainelSenha"), multiTenancySides: MultiTenancySides.Tenant);
            crudfila = fila.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila_Delete, L("DeletePainelSenha"), multiTenancySides: MultiTenancySides.Tenant);

            var tipoLocalChamada = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada, L("TipoLocalChamda"), multiTenancySides: MultiTenancySides.Tenant);
            tipoLocalChamada.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada_Create, L("CreateNewTipoLocalChamda"), multiTenancySides: MultiTenancySides.Tenant);
            tipoLocalChamada.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada_Edit, L("EditTipoLocalChamda"), multiTenancySides: MultiTenancySides.Tenant);
            tipoLocalChamada.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada_Delete, L("DeleteTipoLocalChamda"), multiTenancySides: MultiTenancySides.Tenant);

            var movimentoAutomatico = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos, L("MovimentoAutomatico"), multiTenancySides: MultiTenancySides.Tenant);
            var crudmovimentoAutomatico = movimentoAutomatico.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos_Create, L("CreateNewMovimentoAutomatico"), multiTenancySides: MultiTenancySides.Tenant);
            crudmovimentoAutomatico = movimentoAutomatico.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos_Edit, L("EditMovimentoAutomatico"), multiTenancySides: MultiTenancySides.Tenant);
            crudmovimentoAutomatico = movimentoAutomatico.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos_Delete, L("DeleteMovimentoAutomatico"), multiTenancySides: MultiTenancySides.Tenant);

            //ModeloTexto
            var modeloTexto = atendimentoCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_ModeloTextos, L("ModeloTexto"), multiTenancySides: MultiTenancySides.Tenant);
            var crudmodeloTexto = modeloTexto.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_ModeloTextos_Create, L("CreateNewModeloTexto"), multiTenancySides: MultiTenancySides.Tenant);
            crudmodeloTexto = modeloTexto.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_ModeloTextos_Edit, L("EditModeloTexto"), multiTenancySides: MultiTenancySides.Tenant);
            crudmodeloTexto = modeloTexto.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Atendimento_ModeloTextos_Delete, L("DeleteModeloTexto"), multiTenancySides: MultiTenancySides.Tenant);

            //Manutencao
            var manutencao = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao, L("Manutencao"), multiTenancySides: MultiTenancySides.Tenant);
            var consultor = manutencao.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor, L("Consultor"), multiTenancySides: MultiTenancySides.Tenant);
            var templateEmail = manutencao.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_MailingTemplates, L("MailingTemplates"), multiTenancySides: MultiTenancySides.Tenant);
            var submenuConsultor = consultor.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor_Cadastro, L("Cadastro"), multiTenancySides: MultiTenancySides.Tenant);
            submenuConsultor = consultor.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor_Pesquisa, L("Pesquisa"), multiTenancySides: MultiTenancySides.Tenant);
            submenuConsultor = consultor.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor_Favoritos, L("Favoritos"), multiTenancySides: MultiTenancySides.Tenant);
            submenuConsultor = consultor.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor_Indice, L("Indice"), multiTenancySides: MultiTenancySides.Tenant);
            var submenuConsultorTabela = consultor.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela, L("Tabela"), multiTenancySides: MultiTenancySides.Tenant);
            var crudConsultorTabela = submenuConsultorTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Create, L("CreateNewConsultorTabela"), multiTenancySides: MultiTenancySides.Tenant);
            crudConsultorTabela = submenuConsultorTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Edit, L("EditConsultorTabela"), multiTenancySides: MultiTenancySides.Tenant);
            crudConsultorTabela = submenuConsultorTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Delete, L("DeleteConsultorTabela"), multiTenancySides: MultiTenancySides.Tenant);
            var crudTemplateEmail = templateEmail.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_MailingTemplates_Create, L("CreateNewMailingTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            crudTemplateEmail = templateEmail.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_MailingTemplates_Edit, L("EditMailingTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            crudTemplateEmail = templateEmail.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_MailingTemplates_Delete, L("DeleteMailingTemplate"), multiTenancySides: MultiTenancySides.Tenant);

            var bis = manutencao.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_BI, L("BI"), multiTenancySides: MultiTenancySides.Tenant);
            bis.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_BI_Create, L("CreateNewBI"), multiTenancySides: MultiTenancySides.Tenant);
            bis.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_BI_Edit, L("EditBI"), multiTenancySides: MultiTenancySides.Tenant);
            bis.CreateChildPermission(AppPermissions.Pages_Tenant_Manutencao_BI_Delete, L("DeleteBI"), multiTenancySides: MultiTenancySides.Tenant);

            //Entrada-Rogerio
            //var SuprimentosEstoque = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque, L("SuprimentoEstoque"), multiTenancySides: MultiTenancySides.Tenant);
            var Entrada = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada, L("SuprimentoEstoqueEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            var crudEntradas = Entrada.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, L("CreateNewEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            crudEntradas = Entrada.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit, L("EditEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            crudEntradas = Entrada.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Delete, L("DeleteEntrada"), multiTenancySides: MultiTenancySides.Tenant);

            //Cfop-Rogerio
            var cfop = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cfop, L("Cfop"), multiTenancySides: MultiTenancySides.Tenant);
            var crudCfops = cfop.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cfop_Create, L("CreateNewCfop"), multiTenancySides: MultiTenancySides.Tenant);
            crudCfops = cfop.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cfop_Edit, L("EditCfop"), multiTenancySides: MultiTenancySides.Tenant);
            crudCfops = cfop.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cfop_Delete, L("DeleteCfop"), multiTenancySides: MultiTenancySides.Tenant);

            //Entrada-Rogerio
            var tipoDocumento = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento, L("TipoDocumento"), multiTenancySides: MultiTenancySides.Tenant);
            var crudTipoDocumentos = tipoDocumento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Create, L("CreateNewTipoDocumento"), multiTenancySides: MultiTenancySides.Tenant);
            crudTipoDocumentos = tipoDocumento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Edit, L("EditTipoDocumento"), multiTenancySides: MultiTenancySides.Tenant);
            crudTipoDocumentos = tipoDocumento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Delete, L("DeleteTipoDocumento"), multiTenancySides: MultiTenancySides.Tenant);

            //TipoEntrada-Rogerio
            var tipoEntrada = CadastrosSuprimentos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada, L("TipoEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            var crudTipoEntradas = tipoEntrada.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Create, L("CreateNewTipoEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            crudTipoEntradas = tipoEntrada.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Edit, L("EditTipoEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            crudTipoEntradas = tipoEntrada.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Delete, L("DeleteTipoEntrada"), multiTenancySides: MultiTenancySides.Tenant);


            var preMovimento = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_PreMovimento, L("Entrada"), multiTenancySides: MultiTenancySides.Tenant);
            preMovimento.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_PreMovimento_Create, L("CreateNewEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            //var crudTpreMovimento = tipoEntrada.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_PreMovimento_Create, L("CreateNewPreMovimento"), multiTenancySides: MultiTenancySides.Tenant);
            preMovimento.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_PreMovimento_Edit, L("EditEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            preMovimento.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_PreMovimento_Delete, L("DeleteEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            preMovimento.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_PreMovimento_ConfirmarEntrada, L("ConfirmarEntrada"), multiTenancySides: MultiTenancySides.Tenant);


            var saidaProduto = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto, L("Saida"), multiTenancySides: MultiTenancySides.Tenant);
            saidaProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Create, L("CreateNewSaida"), multiTenancySides: MultiTenancySides.Tenant);
            saidaProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Edit, L("EditSaida"), multiTenancySides: MultiTenancySides.Tenant);
            saidaProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Delete, L("DeleteSaida"), multiTenancySides: MultiTenancySides.Tenant);
            saidaProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_ConfirmarSaida, L("ConfirmarSaida"), multiTenancySides: MultiTenancySides.Tenant);

            var transferenciaProduto = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto, L("Transferencia"), multiTenancySides: MultiTenancySides.Tenant);
            transferenciaProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto_Create, L("CreateNewTransferencia"), multiTenancySides: MultiTenancySides.Tenant);
            transferenciaProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto_Edit, L("EditTransferencia"), multiTenancySides: MultiTenancySides.Tenant);
            transferenciaProduto.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto_Delete, L("DeleteTransferencia"), multiTenancySides: MultiTenancySides.Tenant);

            var confirmacaoMovimento = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_PreMovimento_ConfirmarMovimento, L("ConfirmaMovimento"), multiTenancySides: MultiTenancySides.Tenant);
            var baixaVale = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Movimento_BaixaVale, L("BaixaVale"), multiTenancySides: MultiTenancySides.Tenant);
            var baixaConsignado = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Movimento_BaixaConsignado, L("BaixaConsignado"), multiTenancySides: MultiTenancySides.Tenant);
            var devolucaoProduto = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Movimento_DevolucaoProduto, L("Devolucao"), multiTenancySides: MultiTenancySides.Tenant);

            var solicitacoes = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Solicitacao, L("Solicitacaoes"), multiTenancySides: MultiTenancySides.Tenant);
            solicitacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Movimento_SolicitacaoSaida, L("SolicitacaoSaida"), multiTenancySides: MultiTenancySides.Tenant);
            solicitacoes.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Movimento_ConfirmacaoSolicitacao, L("ConfirmacaoSolicitacao"), multiTenancySides: MultiTenancySides.Tenant);

            var emprestimos = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo, L("Emprestimo"), multiTenancySides: MultiTenancySides.Tenant);

            var saida = emprestimos.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida, L("Saida"), multiTenancySides: MultiTenancySides.Tenant);
            saida.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida_Create, L("CreateSaida"), multiTenancySides: MultiTenancySides.Tenant);
            saida.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida_Edit, L("EditSaida"), multiTenancySides: MultiTenancySides.Tenant);
            saida.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida_Delete, L("DeleteSaida"), multiTenancySides: MultiTenancySides.Tenant);

            var entrada = emprestimos.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada, L("Entrada"), multiTenancySides: MultiTenancySides.Tenant);
            entrada.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada_Create, L("CreateEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            entrada.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada_Edit, L("EditEntrada"), multiTenancySides: MultiTenancySides.Tenant);
            entrada.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada_Delete, L("DeleteEntrada"), multiTenancySides: MultiTenancySides.Tenant);

            var etiquetas = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_EmissaoEtiqueta, L("Etiquetas"), multiTenancySides: MultiTenancySides.Tenant);
            var inventarios = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Inventario, L("inventarios"), multiTenancySides: MultiTenancySides.Tenant);
            var estoqueImportacaoProduto = SuprimentoEstoque.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Importacao_Produto, L("Importacao_Produto"), multiTenancySides: MultiTenancySides.Tenant);
            //Suprimento->Relatorio
            SuprimentoRelatorio.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto, L("MovimentacaoProduto"), multiTenancySides: MultiTenancySides.Tenant);
            SuprimentoRelatorio.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_Acuracia, L("Acuracia"), multiTenancySides: MultiTenancySides.Tenant);
            SuprimentoRelatorio.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_Mapa_Dispensacao, L("Mapa Dispensacao"), multiTenancySides: MultiTenancySides.Tenant);
            SuprimentoRelatorio.CreateChildPermission(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, L("SaldoProduto"), multiTenancySides: MultiTenancySides.Tenant);

            // Faturamento
            var conta = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_Contas, L("Conta"), multiTenancySides: MultiTenancySides.Tenant);
            var crudContas = conta.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_Conta_Create, L("CreateNewConta"), multiTenancySides: MultiTenancySides.Tenant);
            crudContas = conta.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_Conta_Edit, L("EditConta"), multiTenancySides: MultiTenancySides.Tenant);
            crudContas = conta.CreateChildPermission(AppPermissions.Pages_Tenant_Faturamento_Conta_Delete, L("DeleteConta"), multiTenancySides: MultiTenancySides.Tenant);
            // Faturamento - Item
            var faturamentoItem = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Itens, L("Item"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoItens = faturamentoItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Item_Create, L("CreateNewItem"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoItens = faturamentoItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Item_Edit, L("EditItem"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoItens = faturamentoItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Item_Delete, L("DeleteItem"), multiTenancySides: MultiTenancySides.Tenant);
            // Faturamento - Tabela
            var faturamentoTabela = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabelas, L("Tabela"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoTabelas = faturamentoTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabela_Create, L("CreateNewTabela"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoTabelas = faturamentoTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabela_Edit, L("EditTabela"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoTabelas = faturamentoTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabela_Delete, L("DeleteTabela"), multiTenancySides: MultiTenancySides.Tenant);

            // Faturamento - ItemTabela
            var faturamentoItemTabela = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_ItensTabela, L("ItemTabela"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoItensTabela = faturamentoItemTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_ItemTabela_Create, L("CreateNewItemTabela"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoItensTabela = faturamentoItemTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_ItemTabela_Edit, L("EditItemTabela"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoItensTabela = faturamentoItemTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_ItemTabela_Delete, L("DeleteItemTabela"), multiTenancySides: MultiTenancySides.Tenant);

            // Faturamento - Cadastros - Grupo
            var faturamentoGrupo = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupos, L("Grupo"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoGrupos = faturamentoGrupo.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupo_Create, L("CreateNewGrupo"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoGrupos = faturamentoGrupo.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupo_Edit, L("EditGrupo"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoGrupos = faturamentoGrupo.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupo_Delete, L("DeleteGrupo"), multiTenancySides: MultiTenancySides.Tenant);
            // Faturamento - BrasPreco
            var faturamentoBrasPreco = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasPrecos, L("BrasPreco"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoBrasPrecos = faturamentoBrasPreco.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasPreco_Create, L("CreateNewBrasPreco"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoBrasPrecos = faturamentoBrasPreco.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasPreco_Edit, L("EditBrasPreco"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoBrasPrecos = faturamentoBrasPreco.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasPreco_Delete, L("DeleteBrasPreco"), multiTenancySides: MultiTenancySides.Tenant);
            // Faturamento - BrasApresentacao
            var faturamentoBrasApresentacao = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasApresentacoes, L("BrasApresentacao"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoBrasApresentacoes = faturamentoBrasApresentacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasApresentacao_Create, L("CreateNewBrasApresentacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoBrasApresentacoes = faturamentoBrasApresentacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasApresentacao_Edit, L("EditBrasApresentacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoBrasApresentacoes = faturamentoBrasApresentacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasApresentacao_Delete, L("DeleteBrasApresentacao"), multiTenancySides: MultiTenancySides.Tenant);
            // Faturamento - BrasLaboratorio
            var faturamentoBrasLaboratorio = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasLaboratorios, L("BrasLaboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoBrasLaboratorios = faturamentoBrasLaboratorio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasLaboratorio_Create, L("CreateNewBrasLaboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoBrasLaboratorios = faturamentoBrasLaboratorio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasLaboratorio_Edit, L("EditBrasLaboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoBrasLaboratorios = faturamentoBrasLaboratorio.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasLaboratorio_Delete, L("DeleteBrasLaboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            // Faturamento - BrasItem
            var faturamentoBrasItem = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasItens, L("BrasItem"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoBrasItens = faturamentoBrasItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasItem_Create, L("CreateNewBrasItem"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoBrasItens = faturamentoBrasItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasItem_Edit, L("EditBrasItem"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoBrasItens = faturamentoBrasItem.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasItem_Delete, L("DeleteBrasItem"), multiTenancySides: MultiTenancySides.Tenant);
            // Faturamento - SisMoeda
            var faturamentoSisMoeda = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_SisMoedas, L("SisMoeda"), multiTenancySides: MultiTenancySides.Tenant);
            var crudFaturamentoSisMoedas = faturamentoSisMoeda.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_SisMoeda_Create, L("CreateNewSisMoeda"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoSisMoedas = faturamentoSisMoeda.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_SisMoeda_Edit, L("EditSisMoeda"), multiTenancySides: MultiTenancySides.Tenant);
            crudFaturamentoSisMoedas = faturamentoSisMoeda.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_SisMoeda_Delete, L("DeleteSisMoeda"), multiTenancySides: MultiTenancySides.Tenant);

            var faturamentoItemAutorizacao = Faturamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao, L("FaturamentoItemAutorizacao"), multiTenancySides: MultiTenancySides.Tenant);
            var crudfaturamentoItemAutorizacao = faturamentoItemAutorizacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao_Create, L("CreateNewFaturamentoItemAutorizacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudfaturamentoItemAutorizacao = faturamentoItemAutorizacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao_Edit, L("EditFaturamentoItemAutorizacao"), multiTenancySides: MultiTenancySides.Tenant);
            crudfaturamentoItemAutorizacao = faturamentoItemAutorizacao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao_Delete, L("DeleteFaturamentoItemAutorizacao"), multiTenancySides: MultiTenancySides.Tenant);


            //Laboratório
            var laboratorios = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio, L("Laboratorio"), multiTenancySides: MultiTenancySides.Tenant);
            var laboratoriosCadastros = laboratorios.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros, L("Cadastros"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosTecnico = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tecnico, L("Tecnico"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTecnico.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tecnico_Create, L("CreateNewTecnico"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTecnico.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tecnico_Edit, L("EditTecnico"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTecnico.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tecnico_Delete, L("DeleteTecnico"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosMaterial = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Material, L("Material"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMaterial.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Material_Create, L("CreateNewMaterial"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMaterial.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Material_Edit, L("EditMaterial"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMaterial.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Material_Delete, L("DeleteMaterial"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosMetodo = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Metodo, L("Metodo"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMetodo.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Metodo_Create, L("CreateNewMetodo"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMetodo.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Metodo_Edit, L("EditMetodo"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMetodo.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Metodo_Delete, L("DeleteMetodo"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosUnidade = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade, L("LaboratorioUnidade"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosUnidade.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade_Create, L("CreateNewLaboratorioUnidade"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosUnidade.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade_Edit, L("EditLaboratorioUnidade"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosUnidade.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade_Delete, L("DeleteLaboratorioUnidade"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosKitExame = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExame, L("KitExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKitExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExame_Create, L("CreateNewKitExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKitExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExame_Edit, L("EditKitExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKitExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExame_Delete, L("DeleteKitExame"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosSetor = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Setor, L("Setor"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosSetor.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Setor_Create, L("CreateNewSetor"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosSetor.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Setor_Edit, L("EditSetor"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosSetor.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Setor_Delete, L("DeleteSetor"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosEquipamento = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento, L("Equipamento"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosEquipamento.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Create, L("CreateNewEquipamento"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosEquipamento.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Edit, L("EditEquipamento"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosEquipamento.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Delete, L("DeleteEquipamento"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosInformacoesExame = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_InformacoesExame, L("InformacoesExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosInformacoesExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_InformacoesExame_Create, L("CreateNewInformacoesExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosInformacoesExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_InformacoesExame_Edit, L("EditInformacoesExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosInformacoesExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_InformacoesExame_Delete, L("DeleteInformacoesExame"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosTabelaResultado = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TabelaResultado, L("TabelaResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTabelaResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TabelaResultado_Create, L("CreateNewTabelaResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTabelaResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TabelaResultado_Edit, L("EditTabelaResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTabelaResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TabelaResultado_Delete, L("DeleteTabelaResultado"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosFormatacao = laboratorios.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame, L("Formatacao"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosFormatacao.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame_Create, L("CreateNewFormatacao"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosFormatacao.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame_Edit, L("EditFormatacao"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosFormatacao.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame_Delete, L("DeleteFormatacao"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosExame = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Exame, L("Exame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Exame_Create, L("CreateNewExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Exame_Edit, L("EditExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Exame_Delete, L("DeleteExame"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosMapa = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Mapa, L("Mapa"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMapa.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Mapa_Create, L("CreateMapa"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMapa.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Mapa_Edit, L("EditMapa"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosMapa.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Mapa_Delete, L("DeleteMapa"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosResultado = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado, L("Resultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado_Create, L("CreateNewResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado_Edit, L("EditResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado_Delete, L("DeleteResultado"), multiTenancySides: MultiTenancySides.Tenant);


            ///TODO: avaliar MENU
            //inicio

            var laboratoriosCadastrosKitExameItem = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExameItem, L("KitExameItem"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKitExameItem.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExameItem_Create, L("CreateNewKitExameItem"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKitExameItem.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExameItem_Edit, L("EditKitExameItem"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKitExameItem.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExameItem_Delete, L("DeleteKitExameItem"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosFormata = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata, L("Formata"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosFormata.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Create, L("CreateNewFormata"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosFormata.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Edit, L("EditFormata"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosFormata.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Delete, L("DeleteFormata"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosFormataItem = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormataItem, L("FormataItem"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosFormataItem.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormataItem_Create, L("CreateNewFormataItem"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosFormataItem.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormataItem_Edit, L("EditKitFormataItem"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosFormataItem.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormataItem_Delete, L("DeleteKitFormataItem"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosItemResultado = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ItemResultado, L("ItemResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosItemResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ItemResultado_Create, L("CreateNewItemResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosItemResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ItemResultado_Edit, L("EditKitItemResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosItemResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ItemResultado_Delete, L("DeleteKitItemResultado"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosKit = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Kit, L("Kit"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKit.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Kit_Create, L("CreateNewKit"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKit.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Kit_Edit, L("EditKitKit"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosKit.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Kit_Delete, L("DeleteKitKit"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosResultadoExame = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoExame, L("ResultadoExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultadoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoExame_Create, L("CreateNewResultadoExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultadoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoExame_Edit, L("EditKitResultadoExame"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultadoExame.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoExame_Delete, L("DeleteKitResultadoExame"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosResultadoLaudo = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo, L("ResultadoLaudo"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultadoLaudo.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo_Create, L("CreateNewResultadoLaudo"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultadoLaudo.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo_Edit, L("EditKitResultadoLaudo"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosResultadoLaudo.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo_Delete, L("DeleteKitResultadoLaudo"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosTabela = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela, L("Tabela"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela_Create, L("CreateNewTabela"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela_Edit, L("EditKitTabela"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTabela.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela_Delete, L("DeleteKitTabela"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosCadastrosTipoResultado = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TipoResultado, L("TipoResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTipoResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TipoResultado_Create, L("CreateNewTipoResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTipoResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TipoResultado_Edit, L("EditKitTipoResultado"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosTipoResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TipoResultado_Delete, L("DeleteKitTipoResultado"), multiTenancySides: MultiTenancySides.Tenant);


            var laboratoriosCadastrosCabecalho = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Cabecalho, L("Cabecalho"), multiTenancySides: MultiTenancySides.Tenant);
            laboratoriosCadastrosCabecalho.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Cabecalho_Edit, L("EditCabecalho"), multiTenancySides: MultiTenancySides.Tenant);

            var laboratoriosConfirmacaoResultados = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_ConfirmacaoResultado, L("ConfirmacaoResultado"), multiTenancySides: MultiTenancySides.Tenant);
            // laboratoriosCadastrosCabecalho.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_ConfirmacaoResultado_Edit, L("EditCabecalho"), multiTenancySides: MultiTenancySides.Tenant);


            //EvolucaoResultado   Laboratório
            var evolucaoResultado = laboratoriosCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_EvolucaoResultado, L("EvolucaoResultado"), multiTenancySides: MultiTenancySides.Tenant);
            //evolucaoResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_EvolucaoResultado_Create, L("CreateNewPrescricaoItem"), multiTenancySides: MultiTenancySides.Tenant);
            //evolucaoResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_EvolucaoResultado_Edit, L("EditPrescricaoItem"), multiTenancySides: MultiTenancySides.Tenant);
            //evolucaoResultado.CreateChildPermission(AppPermissions.Pages_Tenant_Laboratorio_EvolucaoResultado_Delete, L("DeletePrescricaoItem"), multiTenancySides: MultiTenancySides.Tenant);


            //avaliar MENU fim

            //Cadastros Financeiro

            var financeiroCadastros = Cadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro, L("Financeiro"), multiTenancySides: MultiTenancySides.Tenant);

            var formaPagamento = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_FormaPagamento, L("FormaPagamento"), multiTenancySides: MultiTenancySides.Tenant);
            formaPagamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_FormaPagamento_Create, L("CreateNewFormaPagamento"), multiTenancySides: MultiTenancySides.Tenant);
            formaPagamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_FormaPagamento_Edit, L("EditFormaPagamento"), multiTenancySides: MultiTenancySides.Tenant);
            formaPagamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_FormaPagamento_Delete, L("DeleteFormaPagamento"), multiTenancySides: MultiTenancySides.Tenant);

            var grupoDRE = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_DRE, L("GrupoDRE"), multiTenancySides: MultiTenancySides.Tenant);
            grupoDRE.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_DRE_Create, L("CreateNewGrupoDRE"), multiTenancySides: MultiTenancySides.Tenant);
            grupoDRE.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_DRE_Edit, L("EditGrupoDRE"), multiTenancySides: MultiTenancySides.Tenant);
            grupoDRE.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_DRE_Delete, L("DeleteGrupoDRE"), multiTenancySides: MultiTenancySides.Tenant);

            var grupoContaAdministrativa = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa, L("GrupoContaAdministrativa"), multiTenancySides: MultiTenancySides.Tenant);
            grupoContaAdministrativa.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa_Create, L("CreateNewGrupoContaAdministrativa"), multiTenancySides: MultiTenancySides.Tenant);
            grupoContaAdministrativa.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa_Edit, L("EditGrupoContaAdministrativa"), multiTenancySides: MultiTenancySides.Tenant);
            grupoContaAdministrativa.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa_Delete, L("DeleteGrupoContaAdministrativa"), multiTenancySides: MultiTenancySides.Tenant);

            var ContaAdministrativa = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa, L("ContaAdministrativa"), multiTenancySides: MultiTenancySides.Tenant);
            ContaAdministrativa.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa_Create, L("CreateNewContaAdministrativa"), multiTenancySides: MultiTenancySides.Tenant);
            ContaAdministrativa.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa_Edit, L("EditContaAdministrativa"), multiTenancySides: MultiTenancySides.Tenant);
            ContaAdministrativa.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa_Delete, L("DeleteContaAdministrativa"), multiTenancySides: MultiTenancySides.Tenant);

            var SituacaoLancamento = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento, L("SituacaoLancamento"), multiTenancySides: MultiTenancySides.Tenant);
            SituacaoLancamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento_Create, L("CreateNewSituacaoLancamento"), multiTenancySides: MultiTenancySides.Tenant);
            SituacaoLancamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento_Edit, L("EditSituacaoLancamento"), multiTenancySides: MultiTenancySides.Tenant);
            SituacaoLancamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento_Delete, L("DeleteSituacaoLancamento"), multiTenancySides: MultiTenancySides.Tenant);

            var TipoDocumento = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_TipoDocumento, L("TipoDocumento"), multiTenancySides: MultiTenancySides.Tenant);
            TipoDocumento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_TipoDocumento_Create, L("CreateNewTipoDocumento"), multiTenancySides: MultiTenancySides.Tenant);
            TipoDocumento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_TipoDocumento_Edit, L("EditTipoDocumento"), multiTenancySides: MultiTenancySides.Tenant);
            TipoDocumento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_TipoDocumento_Delete, L("DeleteTipoDocumento"), multiTenancySides: MultiTenancySides.Tenant);

            var MeioPagamento = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_MeioPagamento, L("MeioPagamento"), multiTenancySides: MultiTenancySides.Tenant);
            MeioPagamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_MeioPagamento_Create, L("CreateNewMeioPagamento"), multiTenancySides: MultiTenancySides.Tenant);
            MeioPagamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_MeioPagamento_Edit, L("EditMeioPagamento"), multiTenancySides: MultiTenancySides.Tenant);
            MeioPagamento.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_MeioPagamento_Delete, L("DeleteMeioPagamento"), multiTenancySides: MultiTenancySides.Tenant);

            var RateioPadrao = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_RateioPadrao, L("RateioPadrao"), multiTenancySides: MultiTenancySides.Tenant);
            RateioPadrao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_RateioPadrao_Create, L("CreateNewRateioPadrao"), multiTenancySides: MultiTenancySides.Tenant);
            RateioPadrao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_RateioPadrao_Edit, L("EditRateioPadrao"), multiTenancySides: MultiTenancySides.Tenant);
            RateioPadrao.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_RateioPadrao_Delete, L("DeleteRateioPadrao"), multiTenancySides: MultiTenancySides.Tenant);

            var Impostos = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Impostos, L("Impostos"), multiTenancySides: MultiTenancySides.Tenant);
            Impostos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Impostos_Create, L("CreateNewImpostos"), multiTenancySides: MultiTenancySides.Tenant);
            Impostos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Impostos_Edit, L("EditImpostos"), multiTenancySides: MultiTenancySides.Tenant);
            Impostos.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Impostos_Delete, L("DeleteImpostos"), multiTenancySides: MultiTenancySides.Tenant);

            var Servico = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Servico, L("Servico"), multiTenancySides: MultiTenancySides.Tenant);
            Servico.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Servico_Create, L("CreateNewServico"), multiTenancySides: MultiTenancySides.Tenant);
            Servico.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Servico_Edit, L("EditServico"), multiTenancySides: MultiTenancySides.Tenant);
            Servico.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Servico_Delete, L("DeleteServico"), multiTenancySides: MultiTenancySides.Tenant);

            var CodigoFiscal = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_CodigoFiscal, L("CodigoFiscal"), multiTenancySides: MultiTenancySides.Tenant);
            CodigoFiscal.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_CodigoFiscal_Create, L("CreateNewCodigoFiscal"), multiTenancySides: MultiTenancySides.Tenant);
            CodigoFiscal.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_CodigoFiscal_Edit, L("EditCodigoFiscal"), multiTenancySides: MultiTenancySides.Tenant);
            CodigoFiscal.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_CodigoFiscal_Delete, L("DeleteCodigoFiscal"), multiTenancySides: MultiTenancySides.Tenant);


            var bancarioCadastros = financeiroCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario, L("Bancario"), multiTenancySides: MultiTenancySides.Tenant);

            var bancoAgencias = bancarioCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias, L("BancoAgencias"), multiTenancySides: MultiTenancySides.Tenant);
            bancoAgencias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias_Create, L("CreateNewBancoAgencias"), multiTenancySides: MultiTenancySides.Tenant);
            bancoAgencias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias_Edit, L("EditBancoAgencias"), multiTenancySides: MultiTenancySides.Tenant);
            bancoAgencias.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias_Delete, L("DeleteBancoAgencias"), multiTenancySides: MultiTenancySides.Tenant);

            var tipoConta = bancarioCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta, L("TipoConta"), multiTenancySides: MultiTenancySides.Tenant);
            tipoConta.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta_Create, L("CreateNewTipoConta"), multiTenancySides: MultiTenancySides.Tenant);
            tipoConta.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta_Edit, L("EditTipoConta"), multiTenancySides: MultiTenancySides.Tenant);
            tipoConta.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta_Delete, L("DeleteTipoConta"), multiTenancySides: MultiTenancySides.Tenant);

            var contaTasouraria = bancarioCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria, L("ContaTasouraria"), multiTenancySides: MultiTenancySides.Tenant);
            contaTasouraria.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria_Create, L("CreateNewContaTasouraria"), multiTenancySides: MultiTenancySides.Tenant);
            contaTasouraria.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria_Edit, L("EditContaTasouraria"), multiTenancySides: MultiTenancySides.Tenant);
            contaTasouraria.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria_Delete, L("DeleteContaTasouraria"), multiTenancySides: MultiTenancySides.Tenant);

            var talaoCheque = bancarioCadastros.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque, L("TalaoCheque"), multiTenancySides: MultiTenancySides.Tenant);
            talaoCheque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque_Create, L("CreateNewTalaoCheque"), multiTenancySides: MultiTenancySides.Tenant);
            talaoCheque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque_Edit, L("EditTalaoCheque"), multiTenancySides: MultiTenancySides.Tenant);
            talaoCheque.CreateChildPermission(AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque_Delete, L("DeleteTalaoCheque"), multiTenancySides: MultiTenancySides.Tenant);
        }

        public static void CrudPermission(Permission basePermission, string baseDisplayName)
        {
            basePermission.CreateChildPermission(basePermission.Name + "_Create", L("CreateNew" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + "_Edit", L("Edit" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + "_Delete", L("Delete" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
        }
        
        public static void PermissionFormumalrioDinamico(Permission basePermission, string baseDisplayName)
        {
            basePermission.CreateChildPermission(basePermission.Name + "_Create", L("CreateNew" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + AppPermissions.FormularioDinamico_Edit_Proprio_24hrs, L("Edit_Proprio_24hrs_" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + AppPermissions.FormularioDinamico_Edit_24hrs, L("Edit_24hrs_" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + AppPermissions.FormularioDinamico_Edit_Internados_ate_alta_1semana, L("Edit_Internados_ate_alta_1semana_" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + AppPermissions.FormularioDinamico_Edit, L("Edit_" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            
            basePermission.CreateChildPermission(basePermission.Name + AppPermissions.FormularioDinamico_Delete_Proprio_24hrs, L("Delete_Proprio_24hrs_" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + AppPermissions.FormularioDinamico_Delete_24hrs, L("Delete_24hrs_" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + AppPermissions.FormularioDinamico_Delete_Internados_ate_alta_1semana, L("Delete_Internados_ate_alta_1semana_" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + AppPermissions.FormularioDinamico_Delete, L("Delete_" + baseDisplayName), multiTenancySides: MultiTenancySides.Tenant);
            basePermission.CreateChildPermission(basePermission.Name + ".Reativar", L("Reativar"), multiTenancySides: MultiTenancySides.Tenant);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SWMANAGERConsts.LocalizationSourceName);
        }
    }
}
