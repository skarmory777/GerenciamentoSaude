namespace SW10.SWMANAGER.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        
        public const string Pages_Administration_Parametrizations = "Pages.Administration.Parametrizations";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_VisualAsaImportExport = "Pages.Administration.VisualAsaImportExport";
        public const string Pages_Administration_VisualAsaImportExport_StartStop = "Pages.Administration.VisualAsaImportExport.StartStop";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";

        //Premissões itens da aplicação

        #region Atendimento

        public const string Pages_Tenant_Atendimento = "Pages.Tenant.Atendimento";
        public const string Pages_Tenant_Atendimento_Clinico = "Pages.Tenant.Atendimento.Clinico";
        public const string Pages_Tenant_Atendimento_Agendamento = "Pages.Tenant.Atendimento.Agendamento";
        public const string Pages_Tenant_Atendimento_AmbulatorioEmergencia = "Pages.Tenant.Atendimento.AmbulatorioEmergencia";
        public const string Pages_Tenant_Atendimento_AmbulatorioEmergencia_Create = "Pages.Tenant.Atendimento.AmbulatorioEmergencia.Create";
        public const string Pages_Tenant_Atendimento_AmbulatorioEmergencia_Edit = "Pages.Tenant.Atendimento.AmbulatorioEmergencia.Edit";
        public const string Pages_Tenant_Atendimento_AmbulatorioEmergencia_Delete = "Pages.Tenant.Atendimento.AmbulatorioEmergencia.Delete";
        public const string Pages_Tenant_Atendimento_Emergencia = "Pages.Tenant.Atendimento.Emergencia";
        public const string Pages_Tenant_Atendimento_Autorizacao = "Pages.Tenant.Atendimento.Autorizacao";
        public const string Pages_Tenant_Atendimento_AtendimentoExames = "Pages.Tenant.Atendimento.AtendimentoExames";
        public const string Pages_Tenant_Atendimento_ClassificacaoRiscos = "Pages.Tenant.Atendimento.ClassificacaoRiscos";
        public const string Pages_Tenant_Atendimento_Internacao = "Pages.Tenant.Atendimento.Internacao";
        public const string Pages_Tenant_Atendimento_Internacao_Create = "Pages.Tenant.Atendimento.Internacao.Create";
        public const string Pages_Tenant_Atendimento_Internacao_Edit = "Pages.Tenant.Atendimento.Internacao.Edit";
        public const string Pages_Tenant_Atendimento_Internacao_Delete = "Pages.Tenant.Atendimento.Internacao.Delete";
        public const string Pages_Tenant_Atendimento_Internacao_Alta = "Pages.Tenant.Atendimento.Internacao.Alta";
        public const string Pages_Tenant_Atendimento_CentralAutorizacao = "Pages.Tenant.Atendimento.CentralAutorizacao";
        public const string Pages_Tenant_Atendimento_Prorrogacao = "Pages.Tenant.Atendimento.Prorrogacao";
        public const string Pages_Tenant_Atendimento_HomeCare = "Pages.Tenant.Atendimento.HomeCare";
        public const string Pages_Tenant_Atendimento_Relatorio = "Pages.Tenant.Atendimento.Relatorio";


        public const string Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado = "Pages.Tenant.Atendimento.Relatorio.RelatorioIntenado";
        public const string Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento = "Pages.Tenant.Atendimento.Relatorio.RelatorioAtendimento";
        public const string Pages_Tenant_Atendimento_Relatorio_RelatorioAgendamentoCirurgico = "Pages.Tenant.Atendimento.Relatorio.RelatorioAgendamentoCirurgico";

        public const string Pages_Tenant_Atendimento_AgendamentoConsultas = "Pages.Tenant.Atendimento.AgendamentoConsultas";
        public const string Pages_Tenant_Atendimento_AgendamentoConsultas_Create = "Pages.Tenant.Atendimento.AgendamentoConsultas.Create";
        public const string Pages_Tenant_Atendimento_AgendamentoConsultas_Edit = "Pages.Tenant.Atendimento.AgendamentoConsultas.Edit";
        public const string Pages_Tenant_Atendimento_AgendamentoConsultas_Delete = "Pages.Tenant.Atendimento.AgendamentoConsultas.Delete";
        // public const string Pages_Tenant_Atendimento_AgendamentoConsultas_Desconto = "Pages.Tenant.Atendimento.AgendamentoConsultas.Desconto";


        public const string Pages_Tenant_Atendimento_AgendamentoCirurgias = "Pages.Tenant.Atendimento.AgendamentoCirurgias";
        public const string Pages_Tenant_Atendimento_AgendamentoCirurgias_Create = "Pages.Tenant.Atendimento.AgendamentoCirurgias.Create";
        public const string Pages_Tenant_Atendimento_AgendamentoCirurgias_Edit = "Pages.Tenant.Atendimento.AgendamentoCirurgias.Edit";
        public const string Pages_Tenant_Atendimento_AgendamentoCirurgias_Delete = "Pages.Tenant.Atendimento.AgendamentoCirurgias.Delete";
        public const string Pages_Tenant_Atendimento_AgendamentoCirurgias_Desconto = "Pages.Tenant.Atendimento.AgendamentoCirurgias.Desconto";
        public const string Pages_Tenant_Atendimento_AgendamentoCirurgias_Orcamento = "Pages.Tenant.Atendimento.AgendamentoCirurgias.Orcamento";
        public const string Pages_Tenant_Atendimento_AgendamentoCirurgias_Confirma = "Pages.Tenant.Atendimento.AgendamentoCirurgias.Confirma";


        public const string Pages_Tenant_Atendimento_Visitante = "Pages.Tenant.Atendimento.Visitante";
        public const string Pages_Tenant_Atendimento_Visitante_Create = "Pages.Tenant.Atendimento.Visitante.Create";
        public const string Pages_Tenant_Atendimento_Visitante_Edit = "Pages.Tenant.Atendimento.Visitante.Edit";
        public const string Pages_Tenant_Atendimento_Visitante_Delete = "Pages.Tenant.Atendimento.Visitante.Delete";

        public const string Pages_Tenant_Atendimento_AtendimentoLeitoMov = "Pages.Tenant.Atendimento.AtendimentoLeitoMov";
        public const string Pages_Tenant_Atendimento_AtendimentoLeitoMov_Create = "Pages.Tenant.Atendimento.AtendimentoLeitoMov.Create";
        public const string Pages_Tenant_Atendimento_AtendimentoLeitoMov_Edit = "Pages.Tenant.Atendimento.AtendimentoLeitoMov.Edit";
        public const string Pages_Tenant_Atendimento_AtendimentoLeitoMov_Delete = "Pages.Tenant.Atendimento.AtendimentoLeitoMov.Delete";


        public const string Pages_Tenant_Atendimento_AgendamentoExames = "Pages.Tenant.Atendimento.AgendamentoExames";
        //public const string Pages_Tenant_Atendimento_AgendamentoCirurgias = "Pages.Tenant.Atendimento.AgendamentoCirurgias";

        public const string Pages_Tenant_Atendimento_PreAtendimentos = "Pages.Tenant.Atendimento.PreAtendimento";
        public const string Pages_Tenant_Atendimento_PreAtendimentos_Create = "Pages.Tenant.Atendimento.PreAtendimento.Create";
        public const string Pages_Tenant_Atendimento_PreAtendimentos_Edit = "Pages.Tenant.Atendimento.PreAtendimento.Edit";
        public const string Pages_Tenant_Atendimento_PreAtendimentos_Delete = "Pages.Tenant.Atendimento.PreAtendimento.Delete";

        public const string Pages_Tenant_Atendimento_ClassificacaoRiscos_Create = "Pages.Tenant.Atendimento.ClassificacaoRisco.Create";
        public const string Pages_Tenant_Atendimento_ClassificacaoRiscos_Edit = "Pages.Tenant.Atendimento.ClassificacaoRisco.Edit";
        public const string Pages_Tenant_Atendimento_ClassificacaoRiscos_Delete = "Pages.Tenant.Atendimento.ClassificacaoRisco.Delete";

        public const string Pages_Tenant_Atendimento_TerminalSenha = "Pages.Tenant.Atendimento.TerminalSenha";
        public const string Pages_Tenant_Atendimento_PainelSenha = "Pages.Tenant.Atendimento.PainelSenha";

        public const string Pages_Tenant_Cadastros_Atendimento = "Pages.Tenant.Cadastros.Atendimento";
        public const string Pages_Tenant_Cadastros_Atendimento_Create = "Pages.Tenant.Cadastros.Atendimento.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_Edit = "Pages.Tenant.Cadastros.Atendimento.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_Delete = "Pages.Tenant.Cadastros.Atendimento.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_TiposAtendimento = "Pages.Tenant.Cadastros.Atendimento.TipoAtendimento";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Create = "Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Edit = "Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Delete = "Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada = "Pages.Tenant.Cadastros.Atendimento.TipoLocalChamada";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada_Create = "Pages.Tenant.Cadastros.Atendimento.TipoLocalChamada.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada_Edit = "Pages.Tenant.Cadastros.Atendimento.TipoLocalChamada.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada_Delete = "Pages.Tenant.Cadastros.Atendimento.TipoLocalChamada.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_GuiaTipos = "Pages.Tenant.Cadastros.Atendimento.GuiaTipos";
        public const string Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Create = "Pages.Tenant.Cadastros.Atendimento.GuiaTipos.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Edit = "Pages.Tenant.Cadastros.Atendimento.GuiaTipos.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Delete = "Pages.Tenant.Cadastros.Atendimento.GuiaTipos.Delete";

        //pablo
        public const string Pages_Tenant_Cadastros_Atendimento_TiposPrestadores = "Pages.Tenant.Cadastros.Atendimento.TiposPrestadore";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposPrestadores_Create = "Pages.Tenant.Cadastros.Atendimento.TiposPrestadore.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposPrestadores_Edit = "Pages.Tenant.Cadastros.Atendimento.TiposPrestadores.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_TiposPrestadores_Delete = "Pages.Tenant.Cadastros.Atendimento.TiposPrestadores.Delete";


        public const string Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade = "Pages.Tenant.Cadastros.Atendimento.AgendamentoConsultaMedicoDisponibilidade";
        public const string Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade_Create = "Pages.Tenant.Cadastros.Atendimento.AgendamentoConsultaMedicoDisponibilidade.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade_Edit = "Pages.Tenant.Cadastros.Atendimento.AgendamentoConsultaMedicoDisponibilidade.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade_Delete = "Pages.Tenant.Cadastros.Atendimento.AgendamentoConsultaMedicoDisponibilidade.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_MotivosAlta = "Pages.Tenant.Cadastros.Atendimento.MotivoAlta";
        public const string Pages_Tenant_Cadastros_Atendimento_MotivosAlta_Create = "Pages.Tenant.Cadastros.Atendimento.MotivoAlta.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_MotivosAlta_Edit = "Pages.Tenant.Cadastros.Atendimento.MotivoAlta.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_MotivosAlta_Delete = "Pages.Tenant.Cadastros.Atendimento.MotivoAlta.Delete";

        public const string Pages_Tenant_Atendimento_Orcamentos = "Pages.Tenant.Cadastros.Atendimento.Orcamento";
        public const string Pages_Tenant_Atendimento_Orcamentos_Create = "Pages.Tenant.Cadastros.Atendimento.Orcamento.Create";
        public const string Pages_Tenant_Atendimento_Orcamentos_Edit = "Pages.Tenant.Cadastros.Atendimento.Orcamento.Edit";
        public const string Pages_Tenant_Atendimento_Orcamentos_Delete = "Pages.Tenant.Cadastros.Atendimento.Orcamento.Delete";

        public const string Pages_Tenant_Atendimento_SolicitacaoAutorizacaoProcedimento = "Pages.Tenant.Atendimento.SolicitacaoAutorizacaoProcedimento";
        public const string Pages_Tenant_Atendimento_SolicitacaoAutorizacaoProcedimento_Create = "Pages.Tenant.Atendimento.SolicitacaoAutorizacaoProcedimento.Create";
        public const string Pages_Tenant_Atendimento_SolicitacaoAutorizacaoProcedimento_Edit = "Pages.Tenant.Atendimento.SolicitacaoAutorizacaoProcedimento.Edit";
        public const string Pages_Tenant_Atendimento_SolicitacaoAutorizacaoProcedimento_Delete = "Pages.Tenant.Atendimento.SolicitacaoAutorizacaoProcedimento.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos = "Pages.Tenant.Cadastros.Atendimento.MovimentosAutomaticos";
        public const string Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos_Create = "Pages.Tenant.Cadastros.Atendimento.MovimentosAutomaticos.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos_Edit = "Pages.Tenant.Cadastros.Atendimento.MovimentosAutomaticos.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos_Delete = "Pages.Tenant.Cadastros.Atendimento.MovimentosAutomaticos.Delete";

        //ModeloTexto
        public const string Pages_Tenant_Cadastros_Atendimento_ModeloTextos = "Pages.Tenant.Cadastros.Atendimento.ModeloTextos";
        public const string Pages_Tenant_Cadastros_Atendimento_ModeloTextos_Create = "Pages.Tenant.Cadastros.Atendimento.ModeloTextos.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_ModeloTextos_Edit = "Pages.Tenant.Cadastros.Atendimento.ModeloTextos.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_ModeloTextos_Delete = "Pages.Tenant.Cadastros.Atendimento.ModeloTextos.Delete";

        #endregion

        #region Assistencial

        //Assistencial
        public const string Pages_Tenant_Assistencial = "Pages.Tenant.Assistencial";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia = "Pages.Tenant.Assistencial.AmbulatorioEmergencia";
        public const string Pages_Tenant_Assistencial_Internacao = "Pages.Tenant.Assistencial.Internacao";

        #region Prescricao
        public const string Pages_Tenant_Cadastros_Assistenciais = "Pages.Tenant.Cadastros.Assistenciais";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao = "Pages.Tenant.Cadastros.Assistenciais.Prescricao";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Delete";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Delete";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoResposta";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoResposta.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoResposta.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoResposta.Delete";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_ModeloPrescricao = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.ModeloPrescricao";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_ModeloPrescricao_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.ModeloPrescricao.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_ModeloPrescricao_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.ModeloPrescricao.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_ModeloPrescricao_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.ModeloPrescricao.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItem";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItem.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItem.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItem.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItemStatus";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItemStatus.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItemStatus.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItemStatus.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoStatus";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoStatus.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoStatus.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoStatus.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.VelocidadeInfusao";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.VelocidadeInfusao.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.VelocidadeInfusao.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.VelocidadeInfusao.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormaAplicacao";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormaAplicacao.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormaAplicacao.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormaAplicacao.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Frequencia";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Frequencia.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Frequencia.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.Frequencia.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoqueItem = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoqueItem";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoqueItem_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoqueItem.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoqueItem_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoqueItem.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoqueItem_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoqueItem.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaFaturamento";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaFaturamento.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaFaturamento.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaFaturamento.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameLaboratorial";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameLaboratorial.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameLaboratorial.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameLaboratorial.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameImagem";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameImagem.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameImagem.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameImagem.Delete";

        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoControle";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle_Create = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoControle.Create";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle_Edit = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoControle.Edit";
        public const string Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle_Delete = "Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoControle.Delete";

        #endregion

        #endregion

        //Atestados
        public const string Pages_Tenant_Assistencial_Atestados = "Pages.Tenant.Assistencial.Atestados";
        public const string Pages_Tenant_Assistencial_AtestadoMedico = "Pages.Tenant.Assistencial.AtestadoMedico";
        public const string Pages_Tenant_Assistencial_AtestadoMedico_Create = "Pages.Tenant.Assistencial.AtestadoMedico.Create";
        public const string Pages_Tenant_Assistencial_AtestadoMedico_Edit = "Pages.Tenant.Assistencial.AtestadoMedico.Edit";
        public const string Pages_Tenant_Assistencial_AtestadoMedico_Delete = "Pages.Tenant.Assistencial.AtestadoMedico.Delete";
        public const string Pages_Tenant_Assistencial_AtestadoComparecimento = "Pages.Tenant.Assistencial.AtestadoComparecimento";
        public const string Pages_Tenant_Assistencial_AtestadoComparecimento_Create = "Pages.Tenant.Assistencial.AtestadoComparecimento.Create";
        public const string Pages_Tenant_Assistencial_AtestadoComparecimento_Edit = "Pages.Tenant.Assistencial.AtestadoComparecimento.Edit";
        public const string Pages_Tenant_Assistencial_AtestadoComparecimento_Delete = "Pages.Tenant.Assistencial.AtestadoComparecimento.Delete";



        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem = "Pages.Tenant.Assistencial.Atendimentos.AmbulatorioEmergencia.Enfermagem";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico = "Pages.Tenant.Assistencial.Atendimentos.AmbulatorioEmergencia.Medico";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo = "Pages.Tenant.Assistencial.Atendimentos.AmbulatorioEmergencia.Administrativo";
        public const string Pages_Tenant_Assistencial_Atendimentos_AmbulatorioEmergencia_Enfermagem = "Pages.Tenant.Assistencial.Atendimentos.AmbulatorioEmergencia.Enfermagem";
        public const string Pages_Tenant_Assistencial_Atendimentos_AmbulatorioEmergencia_Medico = "Pages.Tenant.Assistencial.Atendimentos.AmbulatorioEmergencia.Medico";
        public const string Pages_Tenant_Assistencial_Atendimentos_AmbulatorioEmergencia_Administrativo = "Pages.Tenant.Assistencial.Atendimentos.AmbulatorioEmergencia.Administrativo";

        //Assistencial Enfermagem
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Admissao = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Enfermagem.Admissao";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Evolucao = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Enfermagem.Evolucao";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_PassagemPlantao = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Enfermagem.PassagemPlantao";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Prescricao = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Enfermagem.Prescricao";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Checagem = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Enfermagem.Checagem";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_SinaisVitais = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Enfermagem.SinaisVitais";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_ControleBalancoHidrico = "Pages.Tenant.Atendimentos.AmbulatorioEmergencia.Enfermagem.ControleBalancoHidrico";

        //Assistencial Medico
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Admissao = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Admissao";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Alta = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Alta";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Anamnese = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Anamnese";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Anamnese_ClinicoGeral = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Anamnese.ClinicoGeral";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Anamnese_Pediatrico = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Pediatrico";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Evolucao = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Evolucao";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ParecerEspecialista = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Parecerespecialista";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao_Create = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Create";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao_Edit = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Edit";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao_Delete = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Delete";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ResultadoExame = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.ResultadoExame";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Create = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame.Create";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Edit = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame.Edit";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Delete = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame.Delete";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExameItem = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExameItem_Create = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Create";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExameItem_Edit = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Edit";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExameItem_Delete = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Delete";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Prioridade = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame.Prioridade";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Prioridade_Create = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Prioridade.Create";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Prioridade_Edit = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Prioridade.Edit";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Prioridade_Delete = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Prioridade.Delete";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Kit = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Kit";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Kit_Create = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Kit.Create";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Kit_Edit = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Kit.Edit";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame_Kit_Delete = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExameItem.Kit.Delete";

        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Exame_Laboratorial = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Exame.Laboratorial";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Exame_Imagem = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Exame.Imagem";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ResumoAlta = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.ResumoAlta";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.ProcedimentoCirurgico";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_AtoCirurgico = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.ProcedimentoCirurgico.AtoCirurgico";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_AtoAnestesico = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.ProcedimentoCirurgico.AtoAnestesico";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_FolhaGastoCentroCirurgico = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.ProcedimentoCirurgico.FolhaGastoCentroCirurgico";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_PartogramaRecemNato = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.PartogramaRecemNato";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Receituario = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Receituario";

        //Assistencial Admnistrativo
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_CAT = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.CAT";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Alergia = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.Alergia";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_DocumentacaoPaciente = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.DocumentacaoPaciente";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.ConfirmacaoAgenda";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Consulta = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.ConfirmacaoAgenda.Consulta";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Exame = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.ConfirmacaoAgenda.Exame";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Cirurgia = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.ConfirmacaoAgenda.Cirurgia";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.Transferencia";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_Leito = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.Transferencia.Leito";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_MedicoResponsavel = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.Transferencia.MedicoResponsavel";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_Setor = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.Transferencia.Setor";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_AltaAdministrativa = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.AltaAdministrativa";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_AlteracaoAtendimento = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.AlteracaoAtendimento";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_PassagemPlantaoEnfermagem = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.PassagemPlantaoEnfermagem";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProrrogacao = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.SolicitacaoProrrogacao";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProdutoSetor = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.SolicitacaoProdutoSetor";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProdutoSOS = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.SolicitacaoProdutoSOS";
        public const string Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_LiberacaoLeito = "Pages.Tenant.Assistencial.AmbulatorioEmergencia.Administrativo.LiberacaoLeito";


        //// Palavra Chave
        //public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave = "Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave";
        //public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Create";
        //public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Edit";
        //public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Delete";

        //Suprimentos
        public const string Pages_Tenant_Suprimentos = "Pages.Tenant.Suprimentos";
        public const string Pages_Tenant_Suprimentos_Compras = "Pages.Tenant.Suprimentos.Compras";

        public const string Pages_Tenant_Suprimentos_CompraRequisicao = "Pages.Tenant.Suprimentos.CompraRequisicao";
        public const string Pages_Tenant_Suprimentos_CompraRequisicao_Create = "Pages.Tenant.Suprimentos.CompraRequisicao.Create";
        public const string Pages_Tenant_Suprimentos_CompraRequisicao_Edit = "Pages.Tenant.Suprimentos.CompraRequisicao.Edit";
        public const string Pages_Tenant_Suprimentos_CompraRequisicao_Delete = "Pages.Tenant.Suprimentos.CompraRequisicao.Delete";

        public const string Pages_Tenant_Suprimentos_Compras_Relatorio = "Pages.Tenant.Suprimentos.Compras.Relatorio";
        public const string Pages_Tenant_Suprimentos_Compras_Relatorio_CompraRequisicao = "Pages.Tenant.Suprimentos.Compras.Relatorio.CompraRequisicao";
        public const string Pages_Tenant_Suprimentos_Compras_Relatorio_CompraAprovacao = "Pages.Tenant.Suprimentos.Compras.Relatorio.CompraAprovacao";
        public const string Pages_Tenant_Suprimentos_Compras_Relatorio_CompraCotacao = "Pages.Tenant.Suprimentos.Compras.Relatorio.CompraCotacao";
        public const string Pages_Tenant_Suprimentos_Compras_Relatorio_OrdemCompra = "Pages.Tenant.Suprimentos.Compras.Relatorio.OrdemCompra";

        public const string Pages_Tenant_Suprimentos_CompraAprovacao = "Pages.Tenant.Suprimentos.CompraAprovacao";
        public const string Pages_Tenant_Suprimentos_CompraAprovacao_Create = "Pages.Tenant.Suprimentos.CompraAprovacao.Create";
        public const string Pages_Tenant_Suprimentos_CompraAprovacao_Edit = "Pages.Tenant.Suprimentos.CompraAprovacao.Edit";
        public const string Pages_Tenant_Suprimentos_CompraAprovacao_Delete = "Pages.Tenant.Suprimentos.CompraAprovacao.Delete";

        public const string Pages_Tenant_Suprimentos_CompraCotacao = "Pages.Tenant.Suprimentos.CompraCotacao";
        public const string Pages_Tenant_Suprimentos_CompraCotacao_Create = "Pages.Tenant.Suprimentos.CompraCotacao.Create";
        public const string Pages_Tenant_Suprimentos_CompraCotacao_Edit = "Pages.Tenant.Suprimentos.CompraCotacao.Edit";
        public const string Pages_Tenant_Suprimentos_CompraCotacao_Delete = "Pages.Tenant.Suprimentos.CompraCotacao.Delete";

        public const string Pages_Tenant_Suprimentos_OrdemCompra = "Pages.Tenant.Suprimentos.OrdemCompra";
        public const string Pages_Tenant_Suprimentos_OrdemCompra_Create = "Pages.Tenant.Suprimentos.OrdemCompra.Create";
        public const string Pages_Tenant_Suprimentos_OrdemCompra_Edit = "Pages.Tenant.Suprimentos.OrdemCompra.Edit";
        public const string Pages_Tenant_Suprimentos_OrdemCompra_Delete = "Pages.Tenant.Suprimentos.OrdemCompra.Delete";

        public const string Pages_Tenant_Suprimentos_Almoxarifado = "Pages.Tenant.Suprimentos.Almoxarifado";
        public const string Pages_Tenant_Suprimentos_Farmacia = "Pages.Tenant.Suprimentos.Farmacia";
        public const string Pages_Tenant_Suprimentos_Estoque = "Pages.Tenant.Suprimentos.Estoque";
        public const string Pages_Tenant_Suprimentos_Relatorio = "Pages.Tenant.Suprimentos.Relatorio";
        public const string Pages_Tenant_Suprimentos_Solicitacao = "Pages.Tenant.Suprimentos.Solicitacao";
        public const string Pages_Tenant_Suprimentos_EmissaoEtiqueta = "Pages.Tenant.Suprimentos.EmissaoEtiqueta";
        public const string Pages_Tenant_Suprimentos_Inventario = "Pages.Tenant.Suprimentos.Inventario";
        public const string Pages_Tenant_Suprimentos_Inventario_Fechar = "Pages.Tenant.Suprimentos.Inventario.Fechar";

        public const string Pages_Tenant_Suprimentos_Estoque_Importacao_Produto = "Pages.Tenant.Suprimentos.EstoqueImportacaoProduto";
        public const string Pages_Tenant_Suprimentos_Estoque_Importacao_Produto_Create = "Pages.Tenant.Suprimentos.EstoqueImportacaoProduto.Create";
        public const string Pages_Tenant_Suprimentos_Estoque_Importacao_Produto_Edit = "Pages.Tenant.Suprimentos.EstoqueImportacaoProduto.Edit";
        public const string Pages_Tenant_Suprimentos_Estoque_Importacao_Produto_Delete = "Pages.Tenant.Suprimentos.EstoqueImportacaoProduto.Delete";

        #region Faturamento

        //Faturamento
        public const string Pages_Tenant_Faturamento = "Pages.Tenant.Faturamento";
        public const string Pages_Tenant_Faturamento_FaturamentoConveniosParticular = "Pages.Tenant.Faturamento.FaturamentoConveniosParticular";
        public const string Pages_Tenant_Faturamento_FaturamentoSUSInternacao = "Pages.Tenant.Faturamento.FaturamentoSUSInternacao";
        public const string Pages_Tenant_Faturamento_FaturamentoSUSAmbulatorio = "Pages.Tenant.Faturamento.FaturamentoSUSAmbulatorio";
        public const string Pages_Tenant_Faturamento_Auditoria = "Pages.Tenant.Faturamento.Auditoria";
        public const string Pages_Tenant_Faturamento_RecursoGlosa = "Pages.Tenant.Faturamento.RecursoGlosa";
        public const string Pages_Tenant_Faturamento_CentralAutorizacaoGuias = "Pages.Tenant.Faturamento.CentralAutorizacaoGuias";
        public const string Pages_Tenant_Faturamento_RegrasConveniosParticulares = "Pages.Tenant.Faturamento.RegrasConveniosParticulares";
        public const string Pages_Tenant_Faturamento_Contas = "Pages.Tenant.Faturamento.Contas";
        public const string Pages_Tenant_Faturamento_Conta_Create = "Pages.Tenant.Faturamento.Contas.Create";
        public const string Pages_Tenant_Faturamento_Conta_Edit = "Pages.Tenant.Faturamento.Contas.Edit";
        public const string Pages_Tenant_Faturamento_Conta_Delete = "Pages.Tenant.Faturamento.Contas.Delete";
        // Faturamento - Cadastros
        public const string Pages_Tenant_Cadastros_Faturamento_Grupos = "Pages.Tenant.Cadastros.Faturamento.Grupos";
        public const string Pages_Tenant_Cadastros_Faturamento_Grupo_Create = "Pages.Tenant.Cadastros.Faturamento.Grupos.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_Grupo_Edit = "Pages.Tenant.Cadastros.Faturamento.Grupos.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_Grupo_Delete = "Pages.Tenant.Cadastros.Faturamento.Grupos.Delete";
        public const string Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao = "Pages.Tenant.Cadastros.Faturamento.FaturamentoItemAutorizacao";
        public const string Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao_Create = "Pages.Tenant.Cadastros.Faturamento.FaturamentoItemAutorizacao.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao_Edit = "Pages.Tenant.Cadastros.Faturamento.FaturamentoItemAutorizacao.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao_Delete = "Pages.Tenant.Cadastros.Faturamento.FaturamentoItemAutorizacao.Delete";

        #endregion Faturamento

        //Financeiro
        public const string Pages_Tenant_Financeiro = "Pages.Tenant.Financeiro";
        public const string Pages_Tenant_Financeiro_ContasPagar = "Pages.Tenant.Financeiro.ContasPagar";
        public const string Pages_Tenant_Financeiro_ContasPagar_Create = "Pages.Tenant.Financeiro.ContasPagar.Create";
        public const string Pages_Tenant_Financeiro_ContasPagar_Edit = "Pages.Tenant.Financeiro.ContasPagar.Edit";
        public const string Pages_Tenant_Financeiro_ContasPagar_Delete = "Pages.Tenant.Financeiro.ContasPagar.Delete";

        public const string Pages_Tenant_Financeiro_ContasReceber = "Pages.Tenant.Financeiro.ContasReceber";
        public const string Pages_Tenant_Financeiro_ContasReceber_Create = "Pages.Tenant.Financeiro.ContasReceber.Create";
        public const string Pages_Tenant_Financeiro_ContasReceber_Edit = "Pages.Tenant.Financeiro.ContasReceber.Edit";
        public const string Pages_Tenant_Financeiro_ContasReceber_Delete = "Pages.Tenant.Financeiro.ContasReceber.Delete";

        public const string Pages_Tenant_Financeiro_ControleBancario = "Pages.Tenant.Financeiro.ControleBancario";
        public const string Pages_Tenant_Financeiro_QuitacaoPaciente = "Pages.Tenant.Financeiro.QuitacaoPaciente";
        public const string Pages_Tenant_Financeiro_Tesouraria = "Pages.Tenant.Financeiro.Tesouraria";
        public const string Pages_Tenant_Financeiro_FluxoCaixa = "Pages.Tenant.Financeiro.FluxoCaixa";
        public const string Pages_Tenant_Financeiro_RepasseMedico = "Pages.Tenant.Financeiro.RepasseMedico";


        //Cadastro Financeiro

        public const string Pages_Tenant_Cadastros_Financeiro = "Pages.Tenant.Cadastros.Financeiro";

        public const string Pages_Tenant_Cadastros_Financeiro_FormaPagamento = "Pages.Tenant.Cadastros.Financeiro.FormaPagamento";
        public const string Pages_Tenant_Cadastros_Financeiro_FormaPagamento_Create = "Pages.Tenant.Cadastros.Financeiro.FormaPagamento.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_FormaPagamento_Edit = "Pages.Tenant.Cadastros.Financeiro.FormaPagamento.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_FormaPagamento_Delete = "Pages.Tenant.Cadastros.Financeiro.FormaPagamento.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_Grupo_DRE = "Pages.Tenant.Cadastros.Financeiro.GrupoDRE";
        public const string Pages_Tenant_Cadastros_Financeiro_Grupo_DRE_Create = "Pages.Tenant.Cadastros.Financeiro.GrupoDRE.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_Grupo_DRE_Edit = "Pages.Tenant.Cadastros.Financeiro.GrupoDRE.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_Grupo_DRE_Delete = "Pages.Tenant.Cadastros.Financeiro.GrupoDRE.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa = "Pages.Tenant.Cadastros.Financeiro.GrupoContaAdministrativa";
        public const string Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa_Create = "Pages.Tenant.Cadastros.Financeiro.GrupoContaAdministrativa.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa_Edit = "Pages.Tenant.Cadastros.Financeiro.GrupoContaAdministrativa.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa_Delete = "Pages.Tenant.Cadastros.Financeiro.GrupoContaAdministrativa.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa = "Pages.Tenant.Cadastros.Financeiro.ContaAdministrativa";
        public const string Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa_Create = "Pages.Tenant.Cadastros.Financeiro.ContaAdministrativa.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa_Edit = "Pages.Tenant.Cadastros.Financeiro.ContaAdministrativa.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa_Delete = "Pages.Tenant.Cadastros.Financeiro.ContaAdministrativa.Delete";


        public const string Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento = "Pages.Tenant.Cadastros.Financeiro.SituacaoLancamento";
        public const string Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento_Create = "Pages.Tenant.Cadastros.Financeiro.SituacaoLancamento.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento_Edit = "Pages.Tenant.Cadastros.Financeiro.SituacaoLancamento.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento_Delete = "Pages.Tenant.Cadastros.Financeiro.SituacaoLancamento.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_TipoDocumento = "Pages.Tenant.Cadastros.Financeiro.TipoDocumento";
        public const string Pages_Tenant_Cadastros_Financeiro_TipoDocumento_Create = "Pages.Tenant.Cadastros.Financeiro.TipoDocumento.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_TipoDocumento_Edit = "Pages.Tenant.Cadastros.Financeiro.TipoDocumento.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_TipoDocumento_Delete = "Pages.Tenant.Cadastros.Financeiro.TipoDocumento.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_MeioPagamento = "Pages.Tenant.Cadastros.Financeiro.MeioPagamento";
        public const string Pages_Tenant_Cadastros_Financeiro_MeioPagamento_Create = "Pages.Tenant.Cadastros.Financeiro.MeioPagamento.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_MeioPagamento_Edit = "Pages.Tenant.Cadastros.Financeiro.MeioPagamento.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_MeioPagamento_Delete = "Pages.Tenant.Cadastros.Financeiro.MeioPagamento.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_RateioPadrao = "Pages.Tenant.Cadastros.Financeiro.RateioPadrao";
        public const string Pages_Tenant_Cadastros_Financeiro_RateioPadrao_Create = "Pages.Tenant.Cadastros.Financeiro.RateioPadrao.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_RateioPadrao_Edit = "Pages.Tenant.Cadastros.Financeiro.RateioPadrao.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_RateioPadrao_Delete = "Pages.Tenant.Cadastros.Financeiro.RateioPadrao.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_Impostos = "Pages.Tenant.Cadastros.Financeiro.Impostos";
        public const string Pages_Tenant_Cadastros_Financeiro_Impostos_Create = "Pages.Tenant.Cadastros.Financeiro.Impostos.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_Impostos_Edit = "Pages.Tenant.Cadastros.Financeiro.Impostos.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_Impostos_Delete = "Pages.Tenant.Cadastros.Financeiro.Impostos.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_Servico = "Pages.Tenant.Cadastros.Financeiro.Servico";
        public const string Pages_Tenant_Cadastros_Financeiro_Servico_Create = "Pages.Tenant.Cadastros.Financeiro.Servico.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_Servico_Edit = "Pages.Tenant.Cadastros.Financeiro.Servico.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_Servico_Delete = "Pages.Tenant.Cadastros.Financeiro.Servico.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_CodigoFiscal = "Pages.Tenant.Cadastros.Financeiro.CodigoFiscal";
        public const string Pages_Tenant_Cadastros_Financeiro_CodigoFiscal_Create = "Pages.Tenant.Cadastros.Financeiro.CodigoFiscal.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_CodigoFiscal_Edit = "Pages.Tenant.Cadastros.Financeiro.CodigoFiscal.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_CodigoFiscal_Delete = "Pages.Tenant.Cadastros.Financeiro.CodigoFiscal.Delete";


        public const string Pages_Tenant_Cadastros_Financeiro_Bancario = "Pages.Tenant.Cadastros.Financeiro.Bancario";

        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias = "Pages.Tenant.Cadastros.Financeiro.Bancario.BancoAgencias";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias_Create = "Pages.Tenant.Cadastros.Financeiro.Bancario.BancoAgencias.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias_Edit = "Pages.Tenant.Cadastros.Financeiro.Bancario.BancoAgencias.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias_Delete = "Pages.Tenant.Cadastros.Financeiro.Bancario.BancoAgencias.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta = "Pages.Tenant.Cadastros.Financeiro.Bancario.TipoConta";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta_Create = "Pages.Tenant.Cadastros.Financeiro.Bancario.TipoConta.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta_Edit = "Pages.Tenant.Cadastros.Financeiro.Bancario.TipoConta.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta_Delete = "Pages.Tenant.Cadastros.Financeiro.Bancario.TipoConta.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria = "Pages.Tenant.Cadastros.Financeiro.Bancario.ContaTasouraria";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria_Create = "Pages.Tenant.Cadastros.Financeiro.Bancario.ContaTasouraria.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria_Edit = "Pages.Tenant.Cadastros.Financeiro.Bancario.ContaTasouraria.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria_Delete = "Pages.Tenant.Cadastros.Financeiro.Bancario.ContaTasouraria.Delete";

        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque = "Pages.Tenant.Cadastros.Financeiro.Bancario.TalaoCheque";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque_Create = "Pages.Tenant.Cadastros.Financeiro.Bancario.TalaoCheque.Create";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque_Edit = "Pages.Tenant.Cadastros.Financeiro.Bancario.TalaoCheque.Edit";
        public const string Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque_Delete = "Pages.Tenant.Cadastros.Financeiro.Bancario.TalaoCheque.Delete";



        //Controladoria
        public const string Pages_Tenant_Controladoria = "Pages.Tenant.Controladoria";
        public const string Pages_Tenant_Controladoria_Orcamentos = "Pages.Tenant.Controladoria.Orcamentos";
        public const string Pages_Tenant_Controladoria_Patrimonio = "Pages.Tenant.Controladoria.Patrimonio";
        public const string Pages_Tenant_Controladoria_Contabilidade = "Pages.Tenant.Controladoria.Contabilidade";
        public const string Pages_Tenant_Controladoria_Custos = "Pages.Tenant.Controladoria.Custos";
        public const string Pages_Tenant_Controladoria_NotasFiscais = "Pages.Tenant.Controladoria.NotasFiscais";
        public const string Pages_Tenant_Controladoria_NotasFiscais_Sincronizar = "Pages.Tenant.Controladoria.NotasFiscais.Sincronizar";
        public const string Pages_Tenant_Controladoria_Eventos = "Pages.Tenant.Controladoria.Eventos";
        public const string Pages_Tenant_Controladoria_Projetos = "Pages.Tenant.Controladoria.Projetos";

        //Apoio
        public const string Pages_Tenant_Apoio = "Pages.Tenant.Apoio";
        public const string Pages_Tenant_Apoio_Nutricao = "Pages.Tenant.Apoio.Nutricao";
        public const string Pages_Tenant_Apoio_CentralMateriais = "Pages.Tenant.Apoio.CentralMateriais";
        public const string Pages_Tenant_Apoio_Esterilizados = "Pages.Tenant.Apoio.Esterilizados";
        public const string Pages_Tenant_Apoio_SolicitacaoAntimicrobianos = "Pages.Tenant.Apoio.SolicitacaoAntimicrobianos";
        public const string Pages_Tenant_Apoio_Manutencao = "Pages.Tenant.Apoio.Manutencao";
        public const string Pages_Tenant_Apoio_Higienizacao = "Pages.Tenant.Apoio.Higienizacao";
        public const string Pages_Tenant_Apoio_PortariaControleAcesso = "Pages.Tenant.Apoio.PortariaControleAcesso";
        public const string Pages_Tenant_Apoio_LavanderiaRouparia = "Pages.Tenant.Apoio.LavanderiaRouparia";
        public const string Pages_Tenant_Apoio_SAC = "Pages.Tenant.Apoio.SAC";
        public const string Pages_Tenant_Apoio_SAME = "Pages.Tenant.Apoio.SAME";
        public const string Pages_Tenant_Apoio_ControleInfeccao = "Pages.Tenant.Apoio.ControleInfeccao";
        public const string Pages_Tenant_Apoio_Hospitalar = "Pages.Tenant.Apoio.Hospitalar";

        //DOMINIO CONNFIGURAÇÕES
        public const string Pages_Tenant_Configuracoes = "Pages.Tenant.Configuracoes";
        public const string Pages_Tenant_Configuracoes_Empresa = "Pages.Tenant.Configuracoes.Empresa";
        public const string Pages_Tenant_Configuracoes_Empresa_Create = "Pages.Tenant.Configuracoes.Empresa.Create";
        public const string Pages_Tenant_Configuracoes_Empresa_Edit = "Pages.Tenant.Configuracoes.Empresa.Edit";
        public const string Pages_Tenant_Configuracoes_Empresa_Delete = "Pages.Tenant.Configuracoes.Empresa.Delete";
        public const string Pages_Tenant_Configuracoes_GeradorFormulario = "Pages.Tenant.Configuracoes.GeradorFormulario";
        public const string Pages_Tenant_Configuracoes_GeradorFormulario_Create = "Pages.Tenant.Configuracoes.GeradorFormulario.Create";
        public const string Pages_Tenant_Configuracoes_GeradorFormulario_Edit = "Pages.Tenant.Configuracoes.GeradorFormulario.Edit";
        public const string Pages_Tenant_Configuracoes_GeradorFormulario_Delete = "Pages.Tenant.Configuracoes.GeradorFormulario.Delete";
        //public const string Pages_Tenant_Configuracoes_ControleUsuarios = "Pages.Tenant.Configuracoes.ControleUsuarios";
        //public const string Pages_Tenant_Configuracoes_AuditoriaTransacoes = "Pages.Tenant.Configuracoes.AuditoriaTransacoes";
        public const string Pages_Tenant_Configuracoes_GeradorRelatorios = "Pages.Tenant.Configuracoes.GeradorRelatorios";

        public const string Pages_Tenant_Configuracoes_Modulo = "Pages.Tenant.Configuracoes.Modulo";
        public const string Pages_Tenant_Configuracoes_Modulo_Create = "Pages.Tenant.Configuracoes.Modulo.Create";
        public const string Pages_Tenant_Configuracoes_Modulo_Edit = "Pages.Tenant.Configuracoes.Modulo.Edit";
        public const string Pages_Tenant_Configuracoes_Modulo_Delete = "Pages.Tenant.Configuracoes.Modulo.Delete";

        public const string Pages_Tenant_Configuracoes_Operacao = "Pages.Tenant.Configuracoes.Operacao";
        public const string Pages_Tenant_Configuracoes_Operacao_Create = "Pages.Tenant.Configuracoes.Operacao.Create";
        public const string Pages_Tenant_Configuracoes_Operacao_Edit = "Pages.Tenant.Configuracoes.Operacao.Edit";
        public const string Pages_Tenant_Configuracoes_Operacao_Delete = "Pages.Tenant.Configuracoes.Operacao.Delete";



        #region Cadastros

        //Cadastros
        public const string Pages_Tenant_Cadastros = "Pages.Tenant.Cadastros";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais = "Pages.Tenant.Cadastros.CadastrosGlobais";
        //TipoLeito
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoLeito = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoLeito_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoLeito_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoLeito_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Delete";
        //Paciente
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Paciente = "Pages.Tenant.Cadastros.CadastrosGlobais.Paciente";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Paciente_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Paciente_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Paciente_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Delete";
        //Medico
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Medico = "Pages.Tenant.Cadastros.CadastrosGlobais.Medico";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Medico_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Medico.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Medico_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Medico.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Medico_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Medico.Delete";
        //Especialidade
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade = "Pages.Tenant.Cadastros.CadastrosGlobais.Especialidade";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Especialidade.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Especialidade.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Especialidade.Delete";
        //Intervalo
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo = "Pages.Tenant.Cadastros.CadastrosGlobais.Intervalo";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Intervalo.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Intervalo.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Intervalo.Delete";
        //Profissao
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Profissao = "Pages.Tenant.Cadastros.CadastrosGlobais.Profissao";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Profissao_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Profissao.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Profissao_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Profissao.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Profissao_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Profissao.Delete";
        //Origem
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Origem = "Pages.Tenant.Cadastros.CadastrosGlobais.Origem";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Origem_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Origem.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Origem_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Origem.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Origem_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Origem.Delete";
        //Naturalidade
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade = "Pages.Tenant.Cadastros.CadastrosGlobais.Naturalidade";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Naturalidade.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Naturalidade.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Naturalidade.Delete";
        //Plano
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Plano = "Pages.Tenant.Cadastros.CadastrosGlobais.Plano";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Plano_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Plano_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Plano_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Delete";
        //Pais
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Pais = "Pages.Tenant.Cadastros.CadastrosGlobais.Pais";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Pais_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Pais.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Pais_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Pais.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Pais_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Pais.Delete";
        //Estado
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Estado = "Pages.Tenant.Cadastros.CadastrosGlobais.Estado";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Estado_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Estado.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Estado_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Estado.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Estado_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Estado.Delete";
        //Cisdade
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cidade = "Pages.Tenant.Cadastros.CadastrosGlobais.Cidade";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cidade_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Cidade.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cidade_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Cidade.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cidade_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Cidade.Delete";
        //Cep
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cep = "Pages.Tenant.Cadastros.CadastrosGlobais.Cep";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cep_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cep_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cep_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Delete";
        //Centro de Custo Pablo
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos = "Pages.Tenant.Cadastros.CadastrosGlobais.CentroCustos";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.CentroCustos.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.CentroCustos.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.CentroCustos.Delete";
        // Tipo de Grupo Centro de Custo
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoGrupoCentroCustos = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoGrupoCentroCustos";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoGrupoCentroCustos_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoGrupoCentroCustos.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoGrupoCentroCustos_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoGrupoCentroCustos.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoGrupoCentroCustos_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoGrupoCentroCustos.Delete";
        // Grupo Centro de Custo
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCentroCustos";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCentroCustos.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCentroCustos.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCentroCustos.Delete";
        //TiposAcomodacao
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoAcomodacao";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoAcomodacao.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoAcomodacao.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoAcomodacao.Delete";
        ////Centro de Custo
        //public const string Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos = "Pages.Tenant.Cadastros.CadastrosGlobais.CentrosCustos";
        //public const string Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.CentrosCustos.Create";
        //public const string Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.CentrosCustos.Edit";
        //public const string Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.CentrosCustos.Delete";
        // UnidadeOrganizacional
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_UnidadeOrganizacional = "Pages.Tenant.Cadastros.CadastrosSuprimentos.UnidadeOrganizacional";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_UnidadeOrganizacional_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.UnidadeOrganizacional.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_UnidadeOrganizacional_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.UnidadeOrganizacional.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_UnidadeOrganizacional_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.UnidadeOrganizacional.Delete";
        //Nacionalidade
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade = "Pages.Tenant.Cadastros.CadastrosGlobais.Nacionalidade";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Nacionalidade.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Nacionalidade.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Nacionalidade.Delete";
        //Grau de instrução
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao = "Pages.Tenant.Cadastros.CadastrosGlobais.GrauInstrucao";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.GrauInstrucao.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.GrauInstrucao.Delete";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.GrauInstrucao.Create";
        //Indicação
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Indicacao.Delete";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Indicacao.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao = "Pages.Tenant.Cadastros.CadastrosGlobais.Indicacao";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Indicacao.Edit";
        //Parentesco
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco = "Pages.Tenant.Cadastros.CadastrosGlobais.Parentesco";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Parentesco.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Parentesco.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Parentesco.Delete";
        //Feriado
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Feriado = "Pages.Tenant.Cadastros.CadastrosGlobais.Feriado";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Feriado_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Feriado.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Feriado_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Feriado.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Feriado_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Feriado.Delete";
        //Capitulo Cid
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID = "Pages.Tenant.Cadastros.CadastrosGlobais.CapituloCID";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.CapituloCID.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.CapituloCID.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.CapituloCID.Delete";
        //Grupo Cid
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCID";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCID.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCID.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCID.Delete";
        //Tipo de prestador 
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoPrestador = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoPrestador";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoPrestador_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoPrestador.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoPrestador_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoPrestador.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoPrestador_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoPrestador.Delete";
        //Categoria
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Categoria = "Pages.Tenant.Cadastros.CadastrosGlobais.Categoria";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Categoria_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Categoria.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Categoria_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Categoria.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Categoria_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Categoria.Delete";
        //Tipo Sanguineo
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Delete";
        //Bancos 
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Banco = "Pages.Tenant.Cadastros.CadastrosGlobais.Banco";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Banco_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Banco";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Banco_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Banco";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Banco_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Banco";
        //Convênios
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Convenio = "Pages.Tenant.Cadastros.CadastrosGlobais.Convenio";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Convenio_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Convenio_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Convenio_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Delete";
        //Planos
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Planos = "Pages.Tenant.Cadastros.CadastrosGlobais.Planos";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Planos_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Planos.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Planos_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Planos.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Planos_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Planos.Delete";
        //Paciente por Convenio Bloqueado
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PacienteConvenioBloqueado = "Pages.Tenant.Cadastros.CadastrosGlobais.PacienteConvenioBloqueado";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PacienteConvenioBloqueado_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.PacienteConvenioBloqueado.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PacienteConvenioBloqueado_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.PacienteConvenioBloqueado.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PacienteConvenioBloqueado_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.PacienteConvenioBloqueado.Delete";
        //Grupos de procedimento
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoProcedimento = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoProcedimento";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoProcedimento_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoProcedimento.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoProcedimento_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoProcedimento.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_GrupoProcedimento_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.GrupoProcedimento.Delete";
        //tipo vinculo impregaticio
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoVinculoEmpregativo";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoVinculoEmpregativo.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoVinculoEmpregativo.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoVinculoEmpregativo.Delete";

        //tipo participação -- Gustavo Rosa
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoParticipacao";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoParticipacao.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoParticipacao.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoParticipacao.Delete";
        //Prestador
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Prestador = "Pages.Tenant.Cadastros.CadastrosGlobais.Prestador";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Prestador_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Prestador.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Prestador_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Prestador.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Prestador_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Prestador.Delete";
        //Prestador Credenciamento
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorCredenciamento = "Pages.Tenant.Cadastros.CadastrosGlobais.PrestadorCredenciamento";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorCredenciamento_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.PrestadorCredenciamento.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorCredenciamento_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.PrestadorCredenciamento.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorCredenciamento_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.PrestadorCredenciamento.Delete";
        //Prestador Grupos de Procedimentos
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorGrupoProcedimento = "Pages.Tenant.Cadastros.CadastrosGlobais.PrestadorGruposProcedimentos";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorGrupoProcedimento_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.PrestadorGruposProcedimentos.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorGrupoProcedimento_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.PrestadorGruposProcedimentos.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_PrestadorGrupoProcedimento_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.PrestadorGruposProcedimentos.Delete";

        //Empresa
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Empresa = "Pages.Tenant.Cadastros.CadastrosGlobais.Empresa";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Empresa_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Empresa.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Empresa_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Empresa.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Empresa_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Empresa.Delete";
        //Regiao
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Regioes = "Pages.Tenant.Cadastros.CadastrosGlobais.Regiao";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Regiao.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Regiao.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Regiao.Delete";
        //Fornrcedor
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor = "Pages.Tenant.Cadastros.CadastrosGlobais.Fornecedor";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Fornecedor.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Fornecedor.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Fornecedor.Delete";
        //Movimentos Cancelamentos
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCancelamento";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCancelamento.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCancelamento.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCancelamento.Delete";
        //Istituição Tranferencia
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia = "Pages.Tenant.Cadastros.CadastrosGlobais.InstituicaoTransferencia";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.InstituicaoTransferencia.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.InstituicaoTransferencia.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.InstituicaoTransferencia.Delete";
        //Motivos Caução
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCaucao";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCaucao.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCaucao.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCaucao.Delete";
        //Motivos Transferencia Leito
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoTransferenciaLeito";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoTransferenciaLeito.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoTransferenciaLeito.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.MotivoTransferenciaLeito.Delete";
        //ProdutoAcaoTerapeutica
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica = "Pages.Tenant.Cadastros.CadastrosGlobais.ProdutoAcaoTerapeutica";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.ProdutoAcaoTerapeutica.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.ProdutoAcaoTerapeutica.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.ProdutoAcaoTerapeutica.Delete";
        //Elemento Html
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml = "Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtml";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtml.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtml.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtml.Delete";
        //Elemento Html Tipo
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo = "Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtmlTipo";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtmlTipo.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtmlTipo.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtmlTipo.Delete";
        #endregion

        // Dominio Tipo de Logradouros
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro = "Pages.Tenant.Cadastros.CadastrosGlobais.TiposLogradouro";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TiposLogradouro.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TiposLogradouro.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TiposLogradouro.Delete";

        //Painel de senha
        public const string Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha = "Pages.Tenant.Cadastros.CadastrosAtendimentos.PainelSenha";
        public const string Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha_Create = "Pages.Tenant.Cadastros.CadastrosAtendimentos.PainelSenha.Create";
        public const string Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha_Edit = "Pages.Tenant.Cadastros.CadastrosAtendimentos.PainelSenha.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha_Delete = "Pages.Tenant.Cadastros.CadastrosAtendimentos.PainelSenha.Delete";

        //Fila
        public const string Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila = "Pages.Tenant.Cadastros.CadastrosAtendimentos.Fila";
        public const string Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila_Create = "Pages.Tenant.Cadastros.CadastrosAtendimentos.Fila.Create";
        public const string Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila_Edit = "Pages.Tenant.Cadastros.CadastrosAtendimentos.Fila.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila_Delete = "Pages.Tenant.Cadastros.CadastrosAtendimentos.Fila.Delete";

        //TerminalSenha


        #region TISS

        // Dominio TISS
        public const string Pages_Tenant_Cadastros_DominioTiss = "Pages.Tenant.Cadastros.DominioTiss";
        public const string Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio = "Pages.Tenant.Cadastros.DominioTiss.TipoTabelaDominio";
        public const string Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create = "Pages.Tenant.Cadastros.DominioTiss.TipoTabelaDominio.Create";
        public const string Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit = "Pages.Tenant.Cadastros.DominioTiss.TipoTabelaDominio.Edit";
        public const string Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Delete = "Pages.Tenant.Cadastros.DominioTiss.TipoTabelaDominio.Delete";
        public const string Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio = "Pages.Tenant.Cadastros.DominioTiss.GrupoTipoTabelaDominio";
        public const string Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Create = "Pages.Tenant.Cadastros.DominioTiss.GrupoTipoTabelaDominio.Create";
        public const string Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Edit = "Pages.Tenant.Cadastros.DominioTiss.GrupoTipoTabelaDominio.Edit";
        public const string Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Delete = "Pages.Tenant.Cadastros.DominioTiss.GrupoTipoTabelaDominio.Delete";
        public const string Pages_Tenant_Cadastros_DominioTiss_TabelasDominio = "Pages.Tenant.Cadastros.DominioTiss.TabelaDominio";
        public const string Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Create = "Pages.Tenant.Cadastros.DominioTiss.TabelaDominio.Create";
        public const string Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Edit = "Pages.Tenant.Cadastros.DominioTiss.TabelaDominio.Edit";
        public const string Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Delete = "Pages.Tenant.Cadastros.DominioTiss.TabelaDominio.Delete";

        public const string Pages_Tenant_Cadastros_DominioTiss_VersoesTiss = "Pages.Tenant.Cadastros.DominioTiss.VersaoTiss";
        public const string Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Create = "Pages.Tenant.Cadastros.DominioTiss.VersaoTiss.Create";
        public const string Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Edit = "Pages.Tenant.Cadastros.DominioTiss.VersaoTiss.Edit";
        public const string Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Delete = "Pages.Tenant.Cadastros.DominioTiss.VersaoTiss.Delete";

        #endregion

        //Suprimentos
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos = "Pages.Tenant.Cadastros.CadastrosSuprimentos";

        // Grupo
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Grupo";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Grupo.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Grupo.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Grupo.Delete";

        // Laboratório
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Laboratorio";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Laboratorio.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Laboratorio.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Laboratorio.Delete";

        //Diagnóstico
        public const string Pages_Tenant_Diagnosticos = "Pages.Tenant.Diagnosticos";
        public const string Pages_Tenant_Diagnosticos_Laboratorio = "Pages.Tenant.Diagnosticos.Laboratorio";

        //Diagnóstico/Imagem
        public const string Pages_Tenant_Diagnosticos_Imagens = "Pages.Tenant.Diagnosticos.Imagens";
        public const string Pages_Tenant_Diagnosticos_Imagens_Create = "Pages.Tenant.Diagnosticos.Imagens.Create";
        public const string Pages_Tenant_Diagnosticos_Imagens_Edit = "Pages.Tenant.Diagnosticos.Imagens.Edit";
        public const string Pages_Tenant_Diagnosticos_Imagens_Delete = "Pages.Tenant.Diagnosticos.Imagens.Delete";

        // Portaria
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Portaria";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Portaria.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Portaria.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Portaria.Delete";

        //Grupo Tratamento
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento = "Pages.Tenant.Cadastros.CadastrosSuprimentos.GrupoTratamento";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.GrupoTratamento.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.GrupoTratamento.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.GrupoTratamento.Delete";

        //LocalizacaoProduto
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto = "Pages.Tenant.Cadastros.CadastrosSuprimentos.LocalizacaoProduto";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.LocalizacaoProduto.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.LocalizacaoProduto.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.LocalizacaoProduto.Delete";

        // Unidades
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Unidade";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Unidade.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Unidade.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Unidade.Delete";

        // Empresas
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Empresa = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Empresa";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Empresa_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Empresa.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Empresa_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Empresa.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Empresa_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Empresa.Delete";

        // TiposUnidade
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade = "Pages.Tenant.Cadastros.CadastrosSuprimentos.TipoUnidade";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.TipoUnidade.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.TipoUnidade.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.TipoUnidade.Delete";

        // Codigo Medciamento
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento = "Pages.Tenant.Cadastros.CadastrosSuprimentos.ProdutosCodigoMedicamento";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.ProdutosCodigoMedicamento.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.ProdutosCodigoMedicamento.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.ProdutosCodigoMedicamento.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_Leitos = "Pages.Tenant.Cadastros.Atendimento.Leito";
        public const string Pages_Tenant_Cadastros_Atendimento_Leitos_Create = "Pages.Tenant.Cadastros.Atendimento.Leito.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_Leitos_Edit = "Pages.Tenant.Cadastros.Atendimento.Leito.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_Leitos_Delete = "Pages.Tenant.Cadastros.Atendimento.Leito.Delete";

        // Estoque
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Delete";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitosStatus = "Pages.Tenant.Cadastros.Atendimento.LeitoStatus";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitosStatus_Create = "Pages.Tenant.Cadastros.Atendimento.LeitoStatus.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitosStatus_Edit = "Pages.Tenant.Cadastros.Atendimento.LeitoStatus.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitosStatus_Delete = "Pages.Tenant.Cadastros.Atendimento.LeitoStatus.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas = "Pages.Tenant.Cadastros.Atendimento.LeitoCaracteristica";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas_Create = "Pages.Tenant.Cadastros.Atendimento.LeitoCaracteristica.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas_Edit = "Pages.Tenant.Cadastros.Atendimento.LeitoCaracteristica.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas_Delete = "Pages.Tenant.Cadastros.Atendimento.LeitoCaracteristica.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_LeitoServicos = "Pages.Tenant.Cadastros.Atendimento.LeitoServico";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitoServicos_Create = "Pages.Tenant.Cadastros.Atendimento.LeitoServico.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitoServicos_Edit = "Pages.Tenant.Cadastros.Atendimento.LeitoServico.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_LeitoServicos_Delete = "Pages.Tenant.Cadastros.Atendimento.LeitoServico.Delete";

        public const string Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao = "Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao";
        public const string Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Create = "Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Create";
        public const string Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Edit = "Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Edit";
        public const string Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Delete = "Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Delete";
        // Classe
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Classe";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Classe.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Classe.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Classe.Delete";

        // subClasse
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse = "Pages.Tenant.Cadastros.CadastrosSuprimentos.SubClasse";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.SubClasse.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.SubClasse.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.SubClasse.Delete";

        // Substancia
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Substancia";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Substancia.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Substancia.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.Substancia.Delete";

        // Faturamento
        public const string Pages_Tenant_Cadastros_Faturamento_Itens = "Pages.Tenant.Cadastros.Faturamento.Itens";
        public const string Pages_Tenant_Cadastros_Faturamento_Item_Create = "Pages.Tenant.Cadastros.Faturamento.Itens.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_Item_Edit = "Pages.Tenant.Cadastros.Faturamento.Itens.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_Item_Delete = "Pages.Tenant.Cadastros.Faturamento.Itens.Delete";
        public const string Pages_Tenant_Cadastros_Faturamento_Tabelas = "Pages.Tenant.Cadastros.Faturamento.Tabelas";
        public const string Pages_Tenant_Cadastros_Faturamento_Tabela_Create = "Pages.Tenant.Cadastros.Faturamento.Tabelas.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_Tabela_Edit = "Pages.Tenant.Cadastros.Faturamento.Tabelas.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_Tabela_Delete = "Pages.Tenant.Cadastros.Faturamento.Tabelas.Delete";

        public const string Pages_Tenant_Cadastros_Faturamento_ItensTabela = "Pages.Tenant.Cadastros.Faturamento.ItensTabela";
        public const string Pages_Tenant_Cadastros_Faturamento_ItemTabela_Create = "Pages.Tenant.Cadastros.Faturamento.ItensTabela.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_ItemTabela_Edit = "Pages.Tenant.Cadastros.Faturamento.ItensTabela.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_ItemTabela_Delete = "Pages.Tenant.Cadastros.Faturamento.ItensTabela.Delete";

        public const string Pages_Tenant_Cadastros_Faturamento_BrasPrecos = "Pages.Tenant.Cadastros.Faturamento.BrasPrecos";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasPreco_Create = "Pages.Tenant.Cadastros.Faturamento.BrasPrecos.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasPreco_Edit = "Pages.Tenant.Cadastros.Faturamento.BrasPrecos.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasPreco_Delete = "Pages.Tenant.Cadastros.Faturamento.BrasPrecos.Delete";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasApresentacoes = "Pages.Tenant.Cadastros.Faturamento.BrasApresentacoes";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasApresentacao_Create = "Pages.Tenant.Cadastros.Faturamento.BrasApresentacoes.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasApresentacao_Edit = "Pages.Tenant.Cadastros.Faturamento.BrasApresentacoes.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasApresentacao_Delete = "Pages.Tenant.Cadastros.Faturamento.BrasApresentacoes.Delete";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasLaboratorios = "Pages.Tenant.Cadastros.Faturamento.BrasLaboratorios";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasLaboratorio_Create = "Pages.Tenant.Cadastros.Faturamento.BrasLaboratorios.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasLaboratorio_Edit = "Pages.Tenant.Cadastros.Faturamento.BrasLaboratorios.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasLaboratorio_Delete = "Pages.Tenant.Cadastros.Faturamento.BrasLaboratorios.Delete";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasItens = "Pages.Tenant.Cadastros.Faturamento.BrasItens";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasItem_Create = "Pages.Tenant.Cadastros.Faturamento.BrasItens.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasItem_Edit = "Pages.Tenant.Cadastros.Faturamento.BrasItens.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_BrasItem_Delete = "Pages.Tenant.Cadastros.Faturamento.BrasItens.Delete";
        public const string Pages_Tenant_Cadastros_Faturamento_SisMoedas = "Pages.Tenant.Cadastros.Faturamento.SisMoedas";
        public const string Pages_Tenant_Cadastros_Faturamento_SisMoeda_Create = "Pages.Tenant.Cadastros.Faturamento.SisMoedas.Create";
        public const string Pages_Tenant_Cadastros_Faturamento_SisMoeda_Edit = "Pages.Tenant.Cadastros.Faturamento.SisMoedas.Edit";
        public const string Pages_Tenant_Cadastros_Faturamento_SisMoeda_Delete = "Pages.Tenant.Cadastros.Faturamento.SisMoedas.Delete";

        // Leito
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Leito = "Pages.Tenant.Cadastros.CadastrosGlobais.Leito";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Leito_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Leito.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Leito_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Leito.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Leito_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Leito.Delete";

        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Delete";

        // Manutencao 
        public const string Pages_Tenant_Manutencao = "Pages.Tenant.Manutencao";
        public const string Pages_Tenant_Manutencao_Consultor = "Pages.Tenant.Manutencao.Consultor";
        public const string Pages_Tenant_Manutencao_MailingTemplates = "Pages.Tenant.Manutencao.MailingTemplates";
        public const string Pages_Tenant_Manutencao_MailingTemplates_Create = "Pages.Tenant.Manutencao.MailingTemplates.Create";
        public const string Pages_Tenant_Manutencao_MailingTemplates_Edit = "Pages.Tenant.Manutencao.MailingTemplates.Edit";
        public const string Pages_Tenant_Manutencao_MailingTemplates_Delete = "Pages.Tenant.Manutencao.MailingTemplates.Delete";

        public const string Pages_Tenant_Manutencao_BI = "Pages.Tenant.Manutencao.BI";
        public const string Pages_Tenant_Manutencao_BI_Create = "Pages.Tenant.Manutencao.BI.Create";
        public const string Pages_Tenant_Manutencao_BI_Edit = "Pages.Tenant.Manutencao.BI.Edit";
        public const string Pages_Tenant_Manutencao_BI_Delete = "Pages.Tenant.Manutencao.BI.Delete";


        // Consultor       
        public const string Pages_Tenant_Manutencao_Consultor_Cadastro = "Pages.Tenant.Manutencao.Consultor.Cadastro";
        public const string Pages_Tenant_Manutencao_Consultor_Pesquisa = "Pages.Tenant.Manutencao.Consultor.Pesquisa";
        public const string Pages_Tenant_Manutencao_Consultor_Indice = "Pages.Tenant.Manutencao.Consultor.Indice";
        public const string Pages_Tenant_Manutencao_Consultor_Favoritos = "Pages.Tenant.Manutencao.Consultor.Favoritos";
        public const string Pages_Tenant_Manutencao_Consultor_Tabela = "Pages.Tenant.Manutencao.Consultor.Tabela";

        public const string Pages_Tenant_Manutencao_Consultor_Tabela_Create = "Pages.Tenant.Manutencao.Consultor.Tabela.Create";
        public const string Pages_Tenant_Manutencao_Consultor_Tabela_Edit = "Pages.Tenant.Manutencao.Consultor.Tabela.Edit";
        public const string Pages_Tenant_Manutencao_Consultor_Tabela_Delete = "Pages.Tenant.Manutencao.Consultor.Tabela.Delete";

        // Suprimentos
        //public const string Pages_Tenant_Cadastros_Suprimentos = "Pages.Tenant.Cadastros.Suprimentos";

        public const string Pages_Tenant_Cadastros_Suprimentos_Produto = "Pages.Tenant.Cadastros.Suprimentos.Produto";
        public const string Pages_Tenant_Cadastros_Suprimentos_Produto_Create = "Pages.Tenant.Cadastros.Suprimentos.Produto.Create";
        public const string Pages_Tenant_Cadastros_Suprimentos_Produto_Edit = "Pages.Tenant.Cadastros.Suprimentos.Produto.Edit";
        public const string Pages_Tenant_Cadastros_Suprimentos_Produto_Delete = "Pages.Tenant.Cadastros.Suprimentos.Produto.Delete";

        // Kit de Estoque
        public const string Pages_Tenant_Cadastros_Suprimentos_KitEstoqueItem = "Pages.Tenant.Cadastros.Suprimentos.KitEstoqueItem";
        public const string Pages_Tenant_Cadastros_Suprimentos_KitEstoqueItem_Create = "Pages.Tenant.Cadastros.Suprimentos.KitEstoqueItem.Create";
        public const string Pages_Tenant_Cadastros_Suprimentos_KitEstoqueItem_Edit = "Pages.Tenant.Cadastros.Suprimentos.KitEstoqueItem.Edit";
        public const string Pages_Tenant_Cadastros_Suprimentos_KitEstoqueItem_Delete = "Pages.Tenant.Cadastros.Suprimentos.KitEstoqueItem.Delete";

        public const string Pages_Tenant_Cadastros_Suprimentos_KitEstoque = "Pages.Tenant.Cadastros.Suprimentos.KitEstoque";
        public const string Pages_Tenant_Cadastros_Suprimentos_KitEstoque_Create = "Pages.Tenant.Cadastros.Suprimentos.KitEstoque.Create";
        public const string Pages_Tenant_Cadastros_Suprimentos_KitEstoque_Edit = "Pages.Tenant.Cadastros.Suprimentos.KitEstoque.Edit";
        public const string Pages_Tenant_Cadastros_Suprimentos_KitEstoque_Delete = "Pages.Tenant.Cadastros.Suprimentos.KitEstoque.Delete";

        // Palavra Chave
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave = "Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Delete";

        // Acao Terapeutica
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica = "Pages.Tenant.Cadastros.CadastrosSuprimentos.AcaoTerapeutica";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica_Create = "Pages.Tenant.Cadastros.CadastrosSuprimentos.AcaoTerapeutica.Create";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica_Edit = "Pages.Tenant.Cadastros.CadastrosSuprimentos.AcaoTerapeutica.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica_Delete = "Pages.Tenant.Cadastros.CadastrosSuprimentos.AcaoTerapeutica.Delete";

        //Entrada/Rogerio
        //public const string Pages_Tenant_Suprimentos_Estoque = "Pages.Tenant.Cadastros.Suprimentos.Estoque";
        public const string Pages_Tenant_Suprimentos_Estoque_Entrada = "Pages.Tenant.Suprimentos.Estoque.Entrada";
        public const string Pages_Tenant_Suprimentos_Estoque_Entrada_Create = "Pages.Tenant.Suprimentos.Estoque.Entrada.Create";
        public const string Pages_Tenant_Suprimentos_Estoque_Entrada_Edit = "Pages.Tenant.Suprimentos.Estoque.Entrada.Edit";
        public const string Pages_Tenant_Suprimentos_Estoque_Entrada_Delete = "Pages.Tenant.Suprimentos.Estoque.Entrada.Delete";
        //Cfop/Rogerio
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cfop = "Pages.Tenant.Cadastros.CadastrosGlobais.Cfop";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cfop_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.Cfop.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cfop_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.Cfop.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_Cfop_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.Cfop.Delete";
        //TipoDocumento/Rogerio
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoDocumento";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoDocumento.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoDocumento.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoDocumento.Delete";
        //TipoEntrada/Rogerio
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoEntrada";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Create = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoEntrada.Create";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Edit = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoEntrada.Edit";
        public const string Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Delete = "Pages.Tenant.Cadastros.CadastrosGlobais.TipoEntrada.Delete";

        public const string Pages_Tenant_Suprimentos_Estoque_PreMovimento = "Pages.Tenant.Suprimentos.Estoque.PreMovimento";
        public const string Pages_Tenant_Suprimentos_Estoque_PreMovimento_Create = "Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create";
        public const string Pages_Tenant_Suprimentos_Estoque_PreMovimento_Edit = "Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit";
        public const string Pages_Tenant_Suprimentos_Estoque_PreMovimento_Delete = "Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete";
        public const string Pages_Tenant_Suprimentos_Estoque_PreMovimento_ConfirmarEntrada = "Pages.Tenant.Suprimentos.Estoque.PreMovimento.ConfirmarEntrada";

        public const string Pages_Tenant_Suprimentos_Estoque_SaidaProduto = "Pages.Tenant.Suprimentos.Estoque.SaidaProduto";
        public const string Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Create = "Pages.Tenant.Suprimentos.Estoque.SaidaProduto.Create";
        public const string Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Edit = "Pages.Tenant.Suprimentos.Estoque.SaidaProduto.Edit";
        public const string Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Delete = "Pages.Tenant.Suprimentos.Estoque.SaidaProduto.Delete";
        public const string Pages_Tenant_Suprimentos_Estoque_SaidaProduto_ConfirmarSaida = "Pages.Tenant.Suprimentos.Estoque.SaidaProduto.ConfirmarSaida";

        public const string Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto = "Pages.Tenant.Suprimentos.Estoque.TransferenciaProduto";
        public const string Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto_Create = "Pages.Tenant.Suprimentos.Estoque.TransferenciaProduto.Create";
        public const string Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto_Edit = "Pages.Tenant.Suprimentos.Estoque.TransferenciaProduto.Edit";
        public const string Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto_Delete = "Pages.Tenant.Suprimentos.Estoque.TransferenciaProduto.Delete";

        public const string Pages_Tenant_Suprimentos_Relatorio_SaldoProduto = "Pages.Tenant.Suprimentos.Relatorio.SaldoProduto";
        public const string Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto = "Pages.Tenant.Suprimentos.Relatorio.MovimentacaoProduto";
        public const string Pages_Tenant_Suprimentos_Relatorio_Acuracia = "Pages.Tenant.Suprimentos.Relatorio.Acuracia";
        public const string Pages_Tenant_Suprimentos_Relatorio_Mapa_Dispensacao = "Pages.Tenant.Suprimentos.Relatorio.MapaDispensacao";

        public const string Pages_Tenant_Suprimentos_Estoque_PreMovimento_ConfirmarMovimento = "Pages.Tenant.Suprimentos.Estoque.PreMovimento.ConfirmarMovimento";
        public const string Pages_Tenant_Suprimentos_Estoque_Movimento_BaixaVale = "Pages.Tenant.Suprimentos.Estoque.Movimento.BaixaVale";
        public const string Pages_Tenant_Suprimentos_Estoque_Movimento_BaixaConsignado = "Pages.Tenant.Suprimentos.Estoque.Movimento.BaixaConsignado";
        public const string Pages_Tenant_Suprimentos_Estoque_Movimento_DevolucaoProduto = "Pages.Tenant.Suprimentos.Estoque.Movimento.DevolucaoProduto";
        public const string Pages_Tenant_Suprimentos_Estoque_Movimento_SolicitacaoSaida = "Pages.Tenant.Suprimentos.Estoque.Movimento.SolicitacaoSaida";
        public const string Pages_Tenant_Suprimentos_Estoque_Movimento_ConfirmacaoSolicitacao = "Pages.Tenant.Suprimentos.Estoque.Movimento.ConfirmacaoSolicitacao";

        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo = "Pages.Tenant.Suprimentos.Estoque.Emprestimo";
        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida = "Pages.Tenant.Suprimentos.Estoque.Emprestimo.Saida";
        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida_Create = "Pages.Tenant.Suprimentos.Estoque.Emprestimo.Saida.Create";
        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida_Edit = "Pages.Tenant.Suprimentos.Estoque.Emprestimo.Saida.Edit";
        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida_Delete = "Pages.Tenant.Suprimentos.Estoque.Emprestimo.Saida.Delete";

        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada = "Pages.Tenant.Suprimentos.Estoque.Emprestimo.Entrada";
        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada_Create = "Pages.Tenant.Suprimentos.Estoque.Emprestimo.Entrada.Create";
        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada_Edit = "Pages.Tenant.Suprimentos.Estoque.Emprestimo.Entrada.Edit";
        public const string Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada_Delete = "Pages.Tenant.Suprimentos.Estoque.Emprestimo.Entrada.Delete";


        public const string Pages_Tenant_Disparo_De_Mensagem = "Pages.Tenant.DisparoDeMensagem";
        public const string Pages_Tenant_Disparo_De_Mensagem_Create = "Pages.Tenant.DisparoDeMensagem.Create";
        public const string Pages_Tenant_Disparo_De_Mensagem_Edit = "Pages.Tenant.DisparoDeMensagem.Edit";
        public const string Pages_Tenant_Disparo_De_Mensagem_Delete = "Pages.Tenant.DisparoDeMensagem.Delete";
        
        public const string Pages_Tenant_Aviso = "Pages.Tenant.Aviso";
        public const string Pages_Tenant_Aviso_Create = "Pages.Tenant.Aviso.Create";
        public const string Pages_Tenant_Aviso_Edit = "Pages.Tenant.Aviso.Edit";
        public const string Pages_Tenant_Aviso_Delete = "Pages.Tenant.Aviso.Delete";

        public const string FormularioDinamico_Edit = "_Edit";
        public const string FormularioDinamico_Edit_Proprio_24hrs = "_Edit_Proprio_24hrs";
        public const string FormularioDinamico_Edit_24hrs = "_Edit_24hrs";
        public const string FormularioDinamico_Edit_Internados_ate_alta_1semana = "_Edit_Internados_ate_alta_1semana";
        
        public const string FormularioDinamico_Delete = "_Delete";
        public const string FormularioDinamico_Delete_Proprio_24hrs = "_Delete_Proprio_24hrs";
        public const string FormularioDinamico_Delete_24hrs = "_Delete_24hrs";
        public const string FormularioDinamico_Delete_Internados_ate_alta_1semana = "_Delete_Internados_ate_alta_1semana";
        
        
        public const string BalancoHidrico_Desbloqueio = "BalancoHidrico_Desbloqueio";
        public const string BalancoHidrico_ConferenciaTotal = "BalancoHidrico_Conferencia_total";
        public const string BalancoHidrico_Desbloqueio_Proprio_24hrs = "BalancoHidrico_Desbloqueio_Proprio_24hrs";
        public const string BalancoHidrico_Desbloqueio_24hrs = "BalancoHidrico_Desbloqueio_24hrs";

        #region Laboratorio
        public const string Pages_Tenant_Laboratorio = "Pages.Tenant.Laboratorio";


        public const string Pages_Tenant_Laboratorio_ConfirmacaoResultado = "Pages.Tenant.Laboratorio.ConfirmacaoResultado";

        public const string Pages_Tenant_Laboratorio_EvolucaoResultado = "Pages.Tenant.Laboratorio.EvolucaoResultado";
        //public const string Pages_Tenant_Laboratorio_EvolucaoResultado_Create = "Pages.Tenant.Laboratorio.EvolucaoResultado.Create";
        //public const string Pages_Tenant_Laboratorio_EvolucaoResultado_Edit = "Pages.Tenant.Laboratorio.EvolucaoResultado.Edit";
        //public const string Pages_Tenant_Laboratorio_EvolucaoResultado_Delete = "Pages.Tenant.Laboratorio.EvolucaoResultado.Delete";


        public const string Pages_Tenant_Laboratorio_Cadastros = "Pages.Tenant.Laboratorio.Cadastros";

        public const string Pages_Tenant_Laboratorio_Cadastros_Metodo = "Pages.Tenant.Laboratorio.Cadastros.Metodo";
        public const string Pages_Tenant_Laboratorio_Cadastros_Metodo_Create = "Pages.Tenant.Laboratorio.Cadastros.Metodo.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Metodo_Edit = "Pages.Tenant.Laboratorio.Cadastros.Metodo.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Metodo_Delete = "Pages.Tenant.Laboratorio.Cadastros.Metodo.Delete";

        //public const string Pages_Tenant_Laboratorio_Cadastros_Unidade = "Pages.Tenant.Laboratorio.Cadastros.Unidade";
        //public const string Pages_Tenant_Laboratorio_Cadastros_Unidade_Create = "Pages.Tenant.Laboratorio.Cadastros.Unidade.Create";
        //public const string Pages_Tenant_Laboratorio_Cadastros_Unidade_Edit = "Pages.Tenant.Laboratorio.Cadastros.Unidade.Edit";
        //public const string Pages_Tenant_Laboratorio_Cadastros_Unidade_Delete = "Pages.Tenant.Laboratorio.Cadastros.Unidade.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_Material = "Pages.Tenant.Laboratorio.Cadastros.Material";
        public const string Pages_Tenant_Laboratorio_Cadastros_Material_Create = "Pages.Tenant.Laboratorio.Cadastros.Material.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Material_Edit = "Pages.Tenant.Laboratorio.Cadastros.Material.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Material_Delete = "Pages.Tenant.Laboratorio.Cadastros.Material.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_Tecnico = "Pages.Tenant.Laboratorio.Cadastros.Tecnico";
        public const string Pages_Tenant_Laboratorio_Cadastros_Tecnico_Create = "Pages.Tenant.Laboratorio.Cadastros.Tecnico.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Tecnico_Edit = "Pages.Tenant.Laboratorio.Cadastros.Tecnico.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Tecnico_Delete = "Pages.Tenant.Laboratorio.Cadastros.Tecnico.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame = "Pages.Tenant.Laboratorio.Cadastros.FormatacaoExame";
        public const string Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame_Create = "Pages.Tenant.Laboratorio.Cadastros.FormatacaoExame.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame_Edit = "Pages.Tenant.Laboratorio.Cadastros.FormatacaoExame.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame_Delete = "Pages.Tenant.Laboratorio.Cadastros.FormatacaoExame.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_InformacoesExame = "Pages.Tenant.Laboratorio.Cadastros.InformacoesExame";
        public const string Pages_Tenant_Laboratorio_Cadastros_InformacoesExame_Create = "Pages.Tenant.Laboratorio.Cadastros.InformacoesExame.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_InformacoesExame_Edit = "Pages.Tenant.Laboratorio.Cadastros.InformacoesExame.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_InformacoesExame_Delete = "Pages.Tenant.Laboratorio.Cadastros.InformacoesExame.Delete";


        // Novaes


        public const string Pages_Tenant_Laboratorio_Cadastros_KitExameItem = "Pages.Tenant.Laboratorio.Cadastros.KitExameItem";
        public const string Pages_Tenant_Laboratorio_Cadastros_KitExameItem_Create = "Pages.Tenant.Laboratorio.Cadastros.KitExameItem.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_KitExameItem_Edit = "Pages.Tenant.Laboratorio.Cadastros.KitExameItem.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_KitExameItem_Delete = "Pages.Tenant.Laboratorio.Cadastros.KitExameItem.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_KitExame = "Pages.Tenant.Laboratorio.Cadastros.KitExame";
        public const string Pages_Tenant_Laboratorio_Cadastros_KitExame_Create = "Pages.Tenant.Laboratorio.Cadastros.KitExame.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_KitExame_Edit = "Pages.Tenant.Laboratorio.Cadastros.KitExame.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_KitExame_Delete = "Pages.Tenant.Laboratorio.Cadastros.KitExame.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento = "Pages.Tenant.Laboratorio.Cadastros.EquipamentoInterfaceamento";
        public const string Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Create = "Pages.Tenant.Laboratorio.Cadastros.EquipamentoInterfaceamento.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Edit = "Pages.Tenant.Laboratorio.Cadastros.EquipamentoInterfaceamento.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Delete = "Pages.Tenant.Laboratorio.Cadastros.EquipamentoInterfaceamento.Delete";


        public const string Pages_Tenant_Laboratorio_Cadastros_Formata = "Pages.Tenant.Laboratorio.Cadastros.Formata";
        public const string Pages_Tenant_Laboratorio_Cadastros_Formata_Create = "Pages.Tenant.Laboratorio.Cadastros.Formata.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Formata_Edit = "Pages.Tenant.Laboratorio.Cadastros.Formata.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Formata_Delete = "Pages.Tenant.Laboratorio.Cadastros.Formata.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_FormataItem = "Pages.Tenant.Laboratorio.Cadastros.FormataItem";
        public const string Pages_Tenant_Laboratorio_Cadastros_FormataItem_Create = "Pages.Tenant.Laboratorio.Cadastros.FormataItem.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_FormataItem_Edit = "Pages.Tenant.Laboratorio.Cadastros.FormataItem.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_FormataItem_Delete = "Pages.Tenant.Laboratorio.Cadastros.FormataItem.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_ItemResultado = "Pages.Tenant.Laboratorio.Cadastros.ItemResultado";
        public const string Pages_Tenant_Laboratorio_Cadastros_ItemResultado_Create = "Pages.Tenant.Laboratorio.Cadastros.ItemResultado.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_ItemResultado_Edit = "Pages.Tenant.Laboratorio.Cadastros.ItemResultado.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_ItemResultado_Delete = "Pages.Tenant.Laboratorio.Cadastros.ItemResultado.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_Resultado = "Pages.Tenant.Laboratorio.Cadastros.Resultado";
        public const string Pages_Tenant_Laboratorio_Cadastros_Resultado_Create = "Pages.Tenant.Laboratorio.Cadastros.Resultado.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Resultado_Edit = "Pages.Tenant.Laboratorio.Cadastros.Resultado.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Resultado_Delete = "Pages.Tenant.Laboratorio.Cadastros.Resultado.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_Kit = "Pages.Tenant.Laboratorio.Cadastros.Kit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Kit_Create = "Pages.Tenant.Laboratorio.Cadastros.Kit.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Kit_Edit = "Pages.Tenant.Laboratorio.Cadastros.Kit.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Kit_Delete = "Pages.Tenant.Laboratorio.Cadastros.Kit.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_Mapa = "Pages.Tenant.Laboratorio.Cadastros.Mapa";
        public const string Pages_Tenant_Laboratorio_Cadastros_Mapa_Create = "Pages.Tenant.Laboratorio.Cadastros.Mapa.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Mapa_Edit = "Pages.Tenant.Laboratorio.Cadastros.Mapa.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Mapa_Delete = "Pages.Tenant.Laboratorio.Cadastros.Mapa.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_ResultadoExame = "Pages.Tenant.Laboratorio.Cadastros.ResultadoExame";
        public const string Pages_Tenant_Laboratorio_Cadastros_ResultadoExame_Create = "Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_ResultadoExame_Edit = "Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_ResultadoExame_Delete = "Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo = "Pages.Tenant.Laboratorio.Cadastros.ResultadoLaudo";
        public const string Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo_Create = "Pages.Tenant.Laboratorio.Cadastros.ResultadoLaudo.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo_Edit = "Pages.Tenant.Laboratorio.Cadastros.ResultadoLaudo.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo_Delete = "Pages.Tenant.Laboratorio.Cadastros.ResultadoLaudo.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_Tabela = "Pages.Tenant.Laboratorio.Cadastros.Tabela";
        public const string Pages_Tenant_Laboratorio_Cadastros_Tabela_Create = "Pages.Tenant.Laboratorio.Cadastros.Tabela.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Tabela_Edit = "Pages.Tenant.Laboratorio.Cadastros.Tabela.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Tabela_Delete = "Pages.Tenant.Laboratorio.Cadastros.Tabela.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_TipoResultado = "Pages.Tenant.Laboratorio.Cadastros.TipoResultado";
        public const string Pages_Tenant_Laboratorio_Cadastros_TipoResultado_Create = "Pages.Tenant.Laboratorio.Cadastros.TipoResultado.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_TipoResultado_Edit = "Pages.Tenant.Laboratorio.Cadastros.TipoResultado.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_TipoResultado_Delete = "Pages.Tenant.Laboratorio.Cadastros.TipoResultado.Delete";

        //Gustavo 28.03.2018
        public const string Pages_Tenant_Laboratorio_Cadastros_Setor = "Pages.Tenant.Laboratorio.Cadastros.Setor";
        public const string Pages_Tenant_Laboratorio_Cadastros_Setor_Create = "Pages.Tenant.Laboratorio.Cadastros.Setor.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Setor_Edit = "Pages.Tenant.Laboratorio.Cadastros.Setor.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Setor_Delete = "Pages.Tenant.Laboratorio.Cadastros.Setor.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade = "Pages.Tenant.Laboratorio.Cadastros.LaboratorioUnidade";
        public const string Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade_Create = "Pages.Tenant.Laboratorio.Cadastros.LaboratorioUnidade.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade_Edit = "Pages.Tenant.Laboratorio.Cadastros.LaboratorioUnidade.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade_Delete = "Pages.Tenant.Laboratorio.Cadastros.LaboratorioUnidade.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_Exame = "Pages.Tenant.Laboratorio.Cadastros.Exame";
        public const string Pages_Tenant_Laboratorio_Cadastros_Exame_Create = "Pages.Tenant.Laboratorio.Cadastros.Exame.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Exame_Edit = "Pages.Tenant.Laboratorio.Cadastros.Exame.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_Exame_Delete = "Pages.Tenant.Laboratorio.Cadastros.Exame.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_TabelaResultado = "Pages.Tenant.Laboratorio.Cadastros.TabelaResultado";
        public const string Pages_Tenant_Laboratorio_Cadastros_TabelaResultado_Create = "Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_TabelaResultado_Edit = "Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Edit";
        public const string Pages_Tenant_Laboratorio_Cadastros_TabelaResultado_Delete = "Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Delete";

        public const string Pages_Tenant_Diagnosticos_Cadastros_Modalidade = "Pages.Tenant.Laboratorio.Cadastros.Modalidade";
        public const string Pages_Tenant_Diagnosticos_Cadastros_Modalidade_Create = "Pages.Tenant.Laboratorio.Cadastros.Modalidade.Create";
        public const string Pages_Tenant_Diagnosticos_Cadastros_Modalidade_Edit = "Pages.Tenant.Laboratorio.Cadastros.Modalidade.Edit";
        public const string Pages_Tenant_Diagnosticos_Cadastros_Modalidade_Delete = "Pages.Tenant.Laboratorio.Cadastros.Modalidade.Delete";

        public const string Pages_Tenant_Laboratorio_Cadastros_Cabecalho = "Pages.Tenant.Laboratorio.Cadastros.Cabecalho";
        // public const string Pages_Tenant_Laboratorio_Cadastros_Cabecalho_Create = "Pages.Tenant.Laboratorio.Cadastros.Cabecalho.Create";
        public const string Pages_Tenant_Laboratorio_Cadastros_Cabecalho_Edit = "Pages.Tenant.Laboratorio.Cadastros.Cabecalho.Edit";
        // public const string Pages_Tenant_Laboratorio_Cadastros_Cabecalho_Delete = "Pages.Tenant.Laboratorio.Cadastros.Cabecalho.Delete";


        #endregion
    }
}