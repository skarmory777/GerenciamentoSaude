using Abp.Application.Navigation;
using Abp.Localization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Web.Navigation;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Startup
{
    public class MpaNavigationProvider : NavigationProvider
    {
        public const string MenuName = "Mpa";
        public const string AssistencialMenuName = "Mpa/Assistenciais";
        public const string AtendimentosMenuName = "Mpa/Atendimentos";

        public override void SetNavigation(INavigationProviderContext context)
        {
            #region CustomData

            var customData = new MenuItemCustomData
            {
                Target = new KeyValuePair<string, string>("target", "_blank"),
                TargetAssistencial = new KeyValuePair<string, string>("target", "conteudo-assistencial"),
                Metodo = new KeyValuePair<string, string>("onclick", "LerParcial"),
                Parametro = "/Mpa/MedicoAdmissao",
                IsFavorito = false
            }; // default abre em nova aba
            var medicoAdmissaoCustomData = new MenuItemCustomData
            {
                Metodo = new KeyValuePair<string, string>("onclick", "LerParcial"),
                Parametro = "/Mpa/Assistenciais/AdmissaoMedica",
                TargetAssistencial = new KeyValuePair<string, string>("target", "conteudo-assistencial")
            };

            var medicoEvolucaoCustomData = new MenuItemCustomData
            {
                Metodo = new KeyValuePair<string, string>("onclick", "LerParcial"),
                Parametro = "/Mpa/Assistenciais/MedicoEvolucoes",
                TargetAssistencial = new KeyValuePair<string, string>("target", "conteudo-assistencial")
            };

            #endregion customData.

            #region MenuPrincipal

            var menu = context.Manager.Menus[MenuName] =
                new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Tenants,
                        L("Tenants"),
                        url: "Mpa/Tenants",
                        icon: "icon-globe",
                        requiredPermissionName: AppPermissions.Pages_Tenants
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Editions,
                        L("Editions"),
                        url: "Mpa/Editions",
                        icon: "icon-grid",
                        requiredPermissionName: AppPermissions.Pages_Editions
                    )
                )
                //Gestão (Dashboard)
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "Mpa/Dashboard",
                        icon: "fa fa-home",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Dashboard
                    )
                );

            CriarMenuAtendimento(menu, customData);
            CriarMenuAssistencial(menu, customData);
            CriarMenuDiagnostico(menu);
            CriarMenuLaboratorio(menu, customData);
            CriarMenuSuprimentos(menu, customData);
            CriarMenuFaturamento(menu);
            CriarMenuFinanceiro(menu, customData);
            CriarMenuControladoria(menu, customData);
            CriarMenuApoio(menu);
            CriarMenuCadastro(menu, customData);
            CriarMenuConfiguracao(menu);
            CriarMenuManutencao(menu, customData);
            CriarMenuAdministracao(menu);

            #endregion

            #region MenuAssistencial

            var menuAssistencial = context.Manager.Menus[AssistencialMenuName] =
                new MenuDefinition(AssistencialMenuName, new FixedLocalizableString("Assistenciais Menu"));
            menuAssistencial
                .AddItem(
                    new MenuItemDefinition(
                            PageNames.App.Assistenciais.AmbulatoriosEmergencias.Enfermagem,
                            L("Enfermagem"),
                            //url: "_Etiqueta",
                            icon: "fa fa-plus-square",
                            //customData: _customData[AtendimentosMenuName],
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Enfermagens.Admissao,
                                L("EnfermagemAdmissao"),
                                url: "/Mpa/Assistenciais/EnfermagemAdmissao",
                                icon: "fas fa-laptop-medical",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Admissao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Enfermagens.Evolucao,
                                L("Evolucao"),
                                url: "/Mpa/Assistenciais/EnfermagemEvolucao",
                                icon: "fas fa-book-medical",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Evolucao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Enfermagens.PassagemPlantao,
                                L("PassagemPlantao"),
                                url: "/Mpa/Assistenciais/EnfermagemPassagemPlantao",
                                icon: "fas fa-user-nurse",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_PassagemPlantao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Enfermagens.Prescricao,
                                L("Prescricao"),
                                url: "/Mpa/Assistenciais/EnfermagemPrescricao",
                                icon: "fas fa-file-prescription",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Prescricao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Enfermagens.SinaisVitais,
                                L("SinaisVitais"),
                                url: "/Mpa/Assistenciais/EnfermagemSinalVital",
                                icon: "fas fa-heart",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_SinaisVitais
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Enfermagens.Checagem,
                                L("Checagem"),
                                url: "/Mpa/Assistenciais/EnfermagemChecagem",
                                icon: "fas fa-user-nurse",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_Checagem
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Enfermagens.ControleBalancoHidrico,
                                L("ControleBalancoHidrico"),
                                url: "/Mpa/Assistenciais/BalancoHidrico",
                                icon: "fas fa-heartbeat",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Enfermagem_ControleBalancoHidrico
                            )
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                            PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medico,
                            L("Medico"),
                            url: "Medico",
                            icon: "fa fa-user-md",
                            //customData: _customData[AtendimentosMenuName],
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.Admissao,
                                L("Admissao"),
                                url: "/Mpa/Assistenciais/MedicoAdmissao",
                                icon: "fas fa-laptop-medical",
                                customData: medicoAdmissaoCustomData,
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Admissao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.Alta,
                                L("Alta"),
                                url: "/Mpa/Assistenciais/MedicoAlta",
                                icon: "fas fa-file-medical-alt",
                                //customData: null,
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Alta
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.Anamnese,
                                L("Anamnese"),
                                url: "/Mpa/Assistenciais/MedicoAnamnese",
                                icon: "fas fa-notes-medical",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Anamnese
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.Evolucao,
                                L("Evolucao"),
                                //url: "Mpa/Assistenciais/MedicoEvolucoes",
                                url: "/Mpa/Assistenciais/MedicoEvolucao",
                                icon: "fas fa-book-medical",
                                customData: medicoEvolucaoCustomData,
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Evolucao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.ParecerEspecialista,
                                L("ParecerEspecialista"),
                                url: "/Mpa/Assistenciais/MedicoParecerEspecialista",
                                icon: "far fa-id-card",
                                //customData: null,
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ParecerEspecialista
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.Prescricao,
                                L("Prescricao"),
                                url: "/Mpa/Assistenciais/MedicoPrescricao",
                                icon: "fas fa-file-prescription",
                                //customData: null,
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.SolicitacaoExame,
                                L("SolicitacaoExame"),
                                url: "/Mpa/Assistenciais/MedicoSolicitacaoExame",
                                icon: "fas fa-flask",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_SolicitacaoExame
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.ResultadoExame,
                                L("ResultadoExame"),
                                url: "/Mpa/Assistenciais/MedicoResultadoExame",
                                icon: "fas fa-file",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ResultadoExame,
                                customData: customData
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.ResumoAlta,
                                L("ResumoAlta"),
                                url: "/Mpa/Assistenciais/MedicoResumoAlta",
                                icon: "fa fa-check-square",
                                //customData: null,
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ResumoAlta
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                    PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.ProcedimentoCirurgico,
                                    L("ProcedimentoCirurgico"),
                                    //url: "Mpa/MedicoProcedimentosCirurgicos",
                                    icon: "fa fa-medkit",
                                    //customData: _customData[AtendimentosMenuName],
                                    requiredPermissionName: AppPermissions
                                        .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos
                                            .DescricaoAtoCirurgico,
                                        L("DescricaoAtoCirurgico"),
                                        url: "/Mpa/Assistenciais/MedicoDescricaoAtoCirurgico",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_AtoCirurgico
                                    )
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos
                                            .DescricaoAtoAnestesico,
                                        L("DescricaoAtoAnestesico"),
                                        url: "/Mpa/Assistenciais/MedicoDescricaoAtoAnestesico",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_AtoAnestesico
                                    )
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos
                                            .FolhaGastoCentroCirurgico,
                                        L("FolhaGastoCentroCirurgico"),
                                        url: "/Mpa/Assistenciais/MedicoFolhaGastoCentroCirurgico",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_ProcedimentoCirurgico_FolhaGastoCentroCirurgico
                                    )
                                )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.Partograma,
                                L("Partograma"),
                                url: "/Mpa/Assistenciais/MedicoPartograma",
                                icon: "fas fa-chart-line",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_PartogramaRecemNato
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Medicos.Receituario,
                                L("Receituario"),
                                url: "/Mpa/Assistenciais/MedicoReceituario",
                                icon: "fas fa-file-prescription",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Receituario
                            )
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                            PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativo,
                            L("Administrativo"),
                            //url: "_Etiqueta",
                            icon: "fa fa-users",
                            //customData: _customData[AtendimentosMenuName],
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos.CAT,
                                L("CAT"),
                                url: "/Mpa/Assistenciais/AdministrativoCAT",
                                icon: "fas fa-comment-medical",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_CAT
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos.Alergia,
                                L("Alergia"),
                                url: "/Mpa/Assistenciais/AdministrativoAlergia",
                                icon: "fas fa-allergies",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Alergia
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                    .DocumentacaoPaciente,
                                L("DocumentacaoPaciente"),
                                url: "/Mpa/Assistenciais/AdministrativoDocumentacaoPaciente",
                                icon: "fa fa-address-card",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_DocumentacaoPaciente
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                    PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                        .ConfirmacaoAgenda,
                                    L("ConfirmacaoAgenda"),
                                    //url: "Mpa/AdministrativoConfirmacaoAgendas",
                                    icon: "fa fa-headphones",
                                    //customData: _customData[AtendimentosMenuName],
                                    requiredPermissionName: AppPermissions
                                        .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                            .AgendaConsulta,
                                        L("ConfirmacaoAgendaConsulta"),
                                        url: "/Mpa/Assistenciais/AdministrativoConfirmacaoAgendaConsulta",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Consulta
                                    )
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos.AgendaExame,
                                        L("ConfirmacaoAgendaExame"),
                                        url: "/Mpa/Assistenciais/AdministrativoConfirmacaoAgendaExame",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Exame
                                    )
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                            .AgendaCirurgia,
                                        L("ConfirmacaoAgendaCirurgia"),
                                        url: "/Mpa/Assistenciais/AdministrativoConfirmacaoAgendaCirurgia",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_ConfirmacaoAgenda_Cirurgia
                                    )
                                )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                    PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos.Tranferencia,
                                    L("Transferencia"),
                                    //url: "Mpa/AdministrativoAlergias",
                                    icon: "fas fa-ambulance",
                                    //customData: _customData[AtendimentosMenuName],
                                    requiredPermissionName: AppPermissions
                                        .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                            .TranferenciaLeito,
                                        L("TranferenciaLeito"),
                                        url: "/Mpa/Assistenciais/AdministrativoTranferenciaLeito",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_Leito
                                    )
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                            .TransferenciaMedicoResponsavel,
                                        L("TransferenciaMedicoResponsavel"),
                                        url: "/Mpa/Assistenciais/AdministrativoTransferenciaMedicoResponsavel",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_MedicoResponsavel
                                    )
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                            .TransferenciaSetor,
                                        L("TransferenciaSetor"),
                                        url: "/Mpa/Assistenciais/AdministrativoTransferenciaSetor",
                                        icon: "icon-home",
                                        //customData: _customData[AtendimentosMenuName],
                                        requiredPermissionName: AppPermissions
                                            .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_Transferencia_Setor
                                    )
                                )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos.Alta,
                                L("Alta"),
                                url: "/Mpa/Assistenciais/AdministrativoAlta",
                                icon: "fas fa-file-medical-alt",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_AltaAdministrativa
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                    .AlteracaoAtendimento,
                                L("AlteracaoAtendimento"),
                                url: "/Mpa/Assistenciais/AdministrativoAlteracaoAtendimento",
                                icon: "fas fa-undo",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_AlteracaoAtendimento
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                    .PassagemPlantaoEnfermagem,
                                L("PassagemPlantaoEnfermagem"),
                                url: "/Mpa/Assistenciais/AdministrativoPassagemPlantaoEnfermagem",
                                icon: "fas fa-user-nurse",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_PassagemPlantaoEnfermagem
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                    .SolicitacaoProrrogacao,
                                L("SolicitacaoProrrogacao"),
                                url: "/Mpa/Assistenciais/AdministrativoSolicitacaoProrrogacao",
                                icon: "fas fa-weight",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProrrogacao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                    .SolicitacaoProdutoSetor,
                                L("SolicitacaoProdutoSetor"),
                                url: "/Mpa/Assistenciais/AdministrativoSolicitacaoProdutoSetor",
                                icon: "fas fa-tablets",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProdutoSetor
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                    .SolicitacaoProdutoSOS,
                                L("SolicitacaoProdutoSOS"),
                                url: "/Mpa/Assistenciais/AdministrativoSolicitacaoProdutoSOS",
                                icon: "fas fa-vial",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_SolicitacaoProdutoSOS
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.Assistenciais.AmbulatoriosEmergencias.Administrativos
                                    .LiberacaoInterdicaoLeito,
                                L("LiberacaoInterdicaoLeito"),
                                url: "/Mpa/Assistenciais/AdministrativoLiberacaoInterdicaoLeito",
                                icon: "fas fa-procedures",
                                //customData: _customData[AtendimentosMenuName],
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Assistencial_AmbulatorioEmergencia_Administrativo_LiberacaoLeito
                            )
                        )
                );

            #endregion
        }

        #region Menus

        private static void CriarMenuAtendimento(MenuDefinition menu, MenuItemCustomData customData) =>
            menu //atendimento (Attendance)
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Atendimento,
                        L("Atendimento"),
                        icon: "fas fa-user-plus",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.Orcamento,
                            L("Orcamento"),
                            url: "Mpa/Orcamentos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_Orcamentos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.Agendamento,
                            L("Agendamento"),
                            url: "Mpa/Agendamento",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_Agendamento
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.Atendimentos.AgendamentoConsultas,
                                L("Consultas"),
                                url: "Mpa/AgendamentoConsultas",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.Atendimentos.AgendamentoExames,
                                L("Exames"),
                                url: "Mpa/AgendamentoExames",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_AgendamentoExames
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.Atendimentos.AgendamentoCirurgias,
                                L("Cirurgias"),
                                url: "Mpa/AgendamentoCirurgias",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_AgendamentoCirurgias
                            )
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.PreAtendimento,
                            L("PreAtendimento"),
                            url: "Mpa/PreAtendimentos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.ClassificacaoRiscos,
                            L("ClassificacaoRiscoTriagem"),
                            url: "Mpa/ClassificacaoRiscos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_ClassificacaoRiscos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.AmbulatorioEmergencia,
                            L("AmbulatorioEmergencia"),
                            url: "Mpa/AmbulatorioEmergencias",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_AmbulatorioEmergencia,
                            //customData: _customData["novaAba"]
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.Internacao,
                            L("Internacao"),
                            url: "Mpa/Internacoes",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_Internacao,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.CentralAutorizacao,
                            L("CentralAutorizacao"),
                            url: "Mpa/CentralAutorizacao",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_CentralAutorizacao
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.Atendimentos.Autorizacao,
                                L("Autorizacao"),
                                url: "Mpa/CentralAutorizacoes",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_Autorizacao
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.Atendimentos.Prorrogacao,
                                L("Prorrogacao"),
                                url: "Mpa/Prorrogacoes",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_Prorrogacao
                            )
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.HomeCare,
                            L("HomeCare"),
                            url: "Mpa/HomeCare",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_HomeCare
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.PainelSenha,
                            L("TerminalSenhas"),
                            url: "Mpa/TerminalSenhas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_TerminalSenha
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.MonitorPainelSenha,
                            L("MonitorPainelSenhas"),
                            url: "Mpa/MonitorPainelSenhas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_PainelSenha
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Suprimentos.Relatorio,
                            L("Relatorio"),
                            //url: "Mpa/Relatorio",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_Relatorio
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.AtendimentosRelatorio.RelatorioAtendimento,
                                L("RelatorioAtendimento"),
                                url: "Mpa/AtendimentoRelatorio/RelatorioAtendimento",
                                icon: "glyphicon glyphicon-list-alt",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.AtendimentosRelatorio.RelatorioInternacao,
                                L("RelatorioInternacao"),
                                url: "Mpa/AtendimentoRelatorio",
                                icon: "glyphicon glyphicon-list-alt",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.AtendimentosRelatorio.RelatorioAtendimento,
                                L("RelatorioAtendimento"),
                                url: "Mpa/AtendimentoRelatorio/IndexRelatorioAtendimento",
                                icon: "glyphicon glyphicon-list-alt",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.AtendimentosRelatorio.RelatorioAtendimento,
                                L("Relatorio Agendamento Cirurgicos"),
                                url: "Mpa/AtendimentoRelatorio/IndexRelatorioAgendamentoCirurgico",
                                icon: "glyphicon glyphicon-list-alt",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento
                            )
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Atendimentos.AtendimentoLeitoMov,
                            L("AtendimentosLeitosMov"),
                            url: "Mpa/AtendimentoLeitoMov",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Atendimento_AtendimentoLeitoMov,
                            customData: customData
                        )
                    )
                );

        private static void CriarMenuAssistencial(MenuDefinition menu, MenuItemCustomData customData) => menu.AddItem(
            new MenuItemDefinition(
                    PageNames.App.Common.Assistencial,
                    L("Assistencial"),
                    icon: "fas fa-user-md",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Assistencial
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.App.Assistenciais.AssistencialAtendimentos.AmbulatorioEmergencia,
                        L("AmbulatorioEmergencia"),
                        url: "Mpa/Assistenciais/AmbulatoriosEmergencias",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia,
                        //customData: _customData["novaAba"]
                        customData: customData
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.App.Assistenciais.AssistencialAtendimentos.Internacao,
                        L("Internacao"),
                        url: "Mpa/Assistenciais/Internacoes",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Assistencial_Internacao,
                        customData: customData
                    )
                )
        );

        private static void CriarMenuDiagnostico(MenuDefinition menu) => menu.AddItem(new MenuItemDefinition(
                PageNames.App.Common.Diagnosticos,
                L("Diagnosticos"),
                icon: "fa fa-heartbeat",
                requiredPermissionName: AppPermissions.Pages_Tenant_Diagnosticos
            )
            //.AddItem(new MenuItemDefinition(
            //    PageNames.App.Diagnosticos.Laboratorio,
            //    L("Laboratorio"),
            //    url: "Mpa/Laboratorio",
            //    icon: "fas fa-plus-circle",
            //    requiredPermissionName: AppPermissions.Pages_Tenant_Diagnosticos_Laboratorio
            //    )
            //)
            .AddItem(new MenuItemDefinition(
                    PageNames.App.Diagnosticos.Imagens,
                    L("DiagnosticoPorImagem"),
                    url: "Mpa/Imagens",
                    icon: "fas fa-plus-circle",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Diagnosticos_Imagens
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Diagnosticos.RegistroExame,
                        L("RegistroExame"),
                        url: "Mpa/RegistroExames",
                        icon: "fa fa-file-text",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Diagnosticos_Imagens
                    )
                ).AddItem(new MenuItemDefinition(
                        PageNames.App.Diagnosticos.GestaoDeLaudos,
                        L("GestaoDeLaudos"),
                        url: "Mpa/GestaoLaudos",
                        icon: "fa fa-eye",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Diagnosticos_Imagens
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Diagnosticos.DICOM,
                        L("DICOM"),
                        url: "Mpa/Imagens/DICOM",
                        icon: "glyphicon glyphicon-file",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Diagnosticos_Imagens
                    )
                )
            )
        );


        private static void CriarMenuLaboratorio(MenuDefinition menu, MenuItemCustomData customData) =>
            //Laboratório
            ///TODO: LABORATÓRIO MENU/PERMISSÕES *** REVISAR ***
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Laboratorio,
                    L("Laboratorio"),
                    icon: "fa fa-flask",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio
                )
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Laboratorio.Painel,
                    L("Painel"),
                    icon: "fa fa-sliders",
                    url: "LaboratorioPainel",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros
                ))
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Laboratorio.Coletas,
                    L("Coletas"),
                    icon: "fa fa-sliders",
                    url: "Resultados",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros
                ))
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Laboratorio.Resultados,
                    L("Resultados"),
                    icon: "fa fa-sliders",
                    url: "ResultadoLaboratorio",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros
                ))
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Laboratorio.ConfirmacaoResultados,
                    L("Confirmacao"),
                    icon: "fa fa-sliders",
                    url: "ConferenciaResultadoExames",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_ConfirmacaoResultado
                ))
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Laboratorio.EvolucaoResultados,
                    L("EvolucaoResultado"),
                    icon: "fa fa-sliders",
                    url: "Mpa/EvolucaoResultados",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_ConfirmacaoResultado
                ))
            );

        private static void CriarMenuSuprimentos(MenuDefinition menu, MenuItemCustomData customData)
        {
            //Suprimentos (Suprimentos)
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Suprimentos,
                    L("Suprimentos"),
                    icon: "fa fa-cart-plus",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Suprimentos.Compras,
                        L("Compras"),
                        url: "Mpa/Compras",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Compras
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Suprimentos.Relatorio,
                            L("Relatorio"),
                            url: "Mpa/Relatorio",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio
                        ).AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.RequisicaoCompra,
                                L("CompraRequisicao"),
                                url: "Mpa/Relatorios/CompraRequisicao",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio_CompraRequisicao
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                               PageNames.App.SuprimentosEstoque.AprovacaoCompra,
                                L("CompraAprovacao"),
                                url: "Mpa/Relatorios/CompraAprovacao",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio_CompraAprovacao
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.CotacaoCompra,
                                L("CompraCotacao"),
                                url: "Mpa/Relatorios/CompraCotacao",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio_CompraCotacao
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.OrdemCompra,
                                L("OrdemCompra"),
                                url: "Mpa/Relatorios/OrdemCompra",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Compras_Relatorio_OrdemCompra
                            )
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.SuprimentosEstoque.RequisicaoCompra,
                        L("CompraRequisicao"),
                        url: "Mpa/ComprasRequisicao",
                        icon: "fa fa-shopping-cart",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_CompraRequisicao //,
                                                                                                         //customData: _customData
                    ))
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.SuprimentosEstoque.AprovacaoCompra,
                        L("CompraAprovacao"),
                        url: "Mpa/ComprasAprovacao",
                        icon: "fa fa-shopping-cart",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_CompraAprovacao //,
                                                                                                        //customData: _customData
                    ))
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.SuprimentosEstoque.CotacaoCompra,
                        L("CompraCotacao"),
                        url: "Mpa/ComprasCotacao",
                        icon: "fa fa-shopping-cart",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_CompraCotacao //,
                                                                                                      //customData: _customData
                    ))
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.SuprimentosEstoque.OrdemCompra,
                        L("OrdemCompra"),
                        url: "Mpa/OrdemCompra",
                        icon: "fa fa-shopping-cart",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_OrdemCompra //,
                                                                                                    //customData: _customData
                    ))
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Suprimentos.Estoque,
                        L("Estoque"),
                        url: "Mpa/Estoque",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Estoque
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Suprimentos.Estoque_Soliciatacao,
                            L("Solicitacao"),
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Solicitacao
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.AtendimentoSolicitacao,
                                L("BaixaSolicitacao"),
                                url: "Mpa/ConfirmacaoSolicitacoes",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Suprimentos_Estoque_Movimento_ConfirmacaoSolicitacao,
                                customData: customData
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.SolicitacaoSaida,
                                L("Solicitacao"),
                                url: "Mpa/Solicitacao",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Suprimentos_Estoque_Movimento_SolicitacaoSaida,
                                customData: customData
                            )
                        )
                    )
                    .AddItem(new MenuItemDefinition
                    (PageNames.App.SuprimentosEstoque.Emprestimo,
                     L("Emprestimos"),
                     icon: "fas fa-plus-circle",
                     requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo)

                        .AddItem(new MenuItemDefinition(
                        PageNames.App.SuprimentosEstoque.EmprestimoReceberEmprestimoSolicitacao,
                        L("Recebimento"),
                        icon: "fas fa-plus-circle"
                        )
                            .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.EmprestimoReceberEmprestimoSolicitacao,
                            L("SolicitacaoRecebimento"),
                            url: "Mpa/EmprestimoSolicitacao/Recebimento",
                            icon: "glyphicon glyphicon-barcode"
                            )
                            )
                            .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.EmprestimoReceberEmprestimoBaixa,
                            L("BaixaRecebimento"),
                            url: "Mpa/EmprestimoBaixa/Recebimento",
                            icon: "glyphicon glyphicon-barcode"
                            )
                            )
                        )

                        .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.EmprestimoReceberEmprestimoSolicitacao,
                            L("Concessao"),
                            icon: "fas fa-plus-circle"
                            )
                                .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.EmprestimoConcederEmprestimoSolicitacao,
                                L("SolicitacaoConcessao"),
                                url: "Mpa/EmprestimoSolicitacao/Concessao",
                                icon: "glyphicon glyphicon-barcode"
                                )
                                )
                                .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.EmprestimoConcederEmprestimoBaixa,
                                L("BaixaConcessao"),
                                url: "Mpa/EmprestimoBaixa/Concessao",
                                icon: "glyphicon glyphicon-barcode"
                                )
                                )
                        )

                        .AddItem(new MenuItemDefinition(
                        PageNames.App.SuprimentosEstoque.EmprestimoReceberEmprestimoSolicitacao,
                        L("Devolucao"),
                        icon: "fas fa-plus-circle")

                            .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.EmprestimoConsultaDevolucao,
                            L("EmprestimoConsultaDevolucao"),
                            url: "Mpa/EmprestimoDevolucao",
                            icon: "glyphicon glyphicon-barcode"
                            )
                            )
                            .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.EmprestimoBaixaDevolucao,
                            L("EmprestimoBaixaDevolucao"),
                            url: "Mpa/EmprestimoDevolucao/Baixa",
                            icon: "glyphicon glyphicon-barcode"
                            )
                            )
                        )                                               
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.Entrada,
                            L("Entrada"),
                            url: "Mpa/PreMovimentos",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Estoque_PreMovimento,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.Saida,
                            L("Saida"),
                            url: "Mpa/Saidas",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.Transferencia,
                            L("Transferencia"),
                            url: "Mpa/Transferencias",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Suprimentos_Estoque_TransferenciaProduto,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.ConfirmaMovimento,
                            L("ConfirmaMovimento"),
                            url: "Mpa/ConfirmacaoMovimentos",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Suprimentos_Estoque_PreMovimento_ConfirmarMovimento,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.BaixaVale,
                            L("BaixaVale"),
                            url: "Mpa/BaixaVales",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Suprimentos_Estoque_Movimento_BaixaVale,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.BaixaConsignado,
                            L("BaixaConsignado"),
                            url: "Mpa/BaixaConsignados",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Suprimentos_Estoque_Movimento_BaixaConsignado,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.DevolucaoProduto,
                            L("Devolucao"),
                            url: "Mpa/Devolucoes",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Suprimentos_Estoque_Movimento_DevolucaoProduto,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.Estoque_EmissaoEtiqueta,
                            L("EmissaoEtiqueta"),
                            url: "Mpa/Etiquetas",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_EmissaoEtiqueta,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.Estoque_Inventario,
                            L("Inventarios"),
                            url: "Mpa/Inventarios",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Inventario,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.SuprimentosEstoque.Estoque_Importacao_Produto,
                            L("Importacao Produto"),
                            url: "Mpa/EstoqueImportacaoProduto",
                            icon: "glyphicon glyphicon-barcode",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Suprimentos_Estoque_Importacao_Produto,
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Suprimentos.Relatorio,
                            L("Relatorio"),
                            url: "Mpa/Relatorio",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.Suprimentos.Relatorio,
                                L("Consumo"),
                                url: "Mpa/Relatorio/Consumo",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio
                            ).AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.SaldoProduto,
                                L("ConsumoPorPaciente"),
                                url: "Mpa/Relatorios/ConsumoPorPaciente",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto
                            )).AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.SaldoProduto,
                                L("ConsumoPorSetor"),
                                url: "Mpa/Relatorios/ConsumoPorSetor",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto
                            ))
                        ).AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.SaldoProduto,
                                L("PerdaPorEstoque"),
                                url: "Mpa/Relatorios/PerdaPorEstoque",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.SaldoProduto,
                                L("DevolucaoPorEstoque"),
                                url: "Mpa/Relatorios/DevolucaoPorEstoque",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.SaldoProduto,
                                L("DevolucaoPorPaciente"),
                                url: "Mpa/Relatorios/DevolucaoPorPaciente",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.SaldoProduto,
                                L("UltimasCompras"),
                                url: "Mpa/Relatorios/UltimasCompras",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.SaldoProduto,
                                L("UltimasComprasVsAtual"),
                                url: "Mpa/Relatorios/UltimasComprasVsAtual",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.SaldoProduto,
                                L("SaldoProduto"),
                                url: "Mpa/Relatorios/SaldoProduto",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.MovimentacaoProduto,
                                L("MovimentacaoProduto"),
                                url: "Mpa/Relatorios/MovimentacaoProduto",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.Acuracia,
                                L("Acuracia"),
                                url: "Mpa/Relatorios/Acuracia",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Suprimentos_Relatorio_Acuracia
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosRelatorio.Acuracia,
                                L("Mapa Dispensacao"),
                                url: "Mpa/Relatorios/MapaDispensacao",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Suprimentos_Relatorio_Mapa_Dispensacao
                            )
                        )
                    )
                )
            );
        }

        private static void CriarMenuFaturamento(MenuDefinition menu)
        {
            // Faturamento (Faturamento)
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Faturamento,
                    L("Faturamento"),
                    icon: "fa fa-calculator",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento
                ).AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.FaturamentoContasMedicas,
                        L("Faturar Atendimento"),
                        url: "Mpa/FaturarAtendimento",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.FaturamentoAuditoriaInterna,
                        L("Auditoria Interna"),
                        url: "Mpa/FaturarAtendimento/AuditoriaInterna",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.FaturamentoAuditoriaExterna,
                        L("Auditoria Externa"),
                        url: "Mpa/FaturarAtendimento/AuditoriaExterna",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.FaturamentoLotes,
                        L("Lotes"),
                        url: "Mpa/FaturamentoLotes",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento
                    )
                )
                // .AddItem(new MenuItemDefinition(
                //         PageNames.App.Faturamentos.FaturamentoContasMedicas,
                //         L("ContasMedicas"),
                //         url: "Mpa/ContasMedicas",
                //         icon: "fas fa-plus-circle",
                //         requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento
                //     )
                // )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.FaturamentoEntregaContas,
                        L("EntregaContas"),
                        url: "Mpa/EntregaContas",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.FaturamentoSUSInternacao,
                        L("FaturamentoSUSInternacao"),
                        url: "Mpa/FaturamentoSUSInternacao",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento_FaturamentoSUSInternacao
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.FaturamentoSUSAmbulatorio,
                        L("FaturamentoSUSAmbulatorio"),
                        url: "Mpa/FaturamentoSUSAmbulatorio",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento_FaturamentoSUSAmbulatorio
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.Auditoria,
                        L("Auditoria"),
                        url: "Mpa/Auditoria",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento_Auditoria
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.RecursoGlosa,
                        L("RecursoGlosa"),
                        url: "Mpa/RecursoGlosa",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento_RecursoGlosa
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.CentralAutorizacaoGuias,
                        L("CentralAutorizacaoGuias"),
                        url: "Mpa/CentralAutorizacaoGuias",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento_CentralAutorizacaoGuias
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Faturamentos.RegrasConveniosParticulares,
                        L("RegrasConveniosParticulares"),
                        url: "Mpa/RegrasConveniosParticulares",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Faturamento_RegrasConveniosParticulares
                    )
                )
            );
        }


        private static void CriarMenuFinanceiro(MenuDefinition menu, MenuItemCustomData customData)
        {
            //Financeiro (Financeiro)
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Financeiro,
                    L("Financeiro"),
                    icon: "fa fa-money-bill-alt",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Financeiro.ContasPagar,
                        L("ContasPagar"),
                        url: "Mpa/ContasPagar",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro_ContasPagar,
                        customData: customData
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Financeiro.ContasReceber,
                        L("ContasReceber"),
                        url: "Mpa/ContasReceber",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro_ContasReceber,
                        customData: customData
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Financeiro.ControleBancario,
                        L("ControleBancario"),
                        url: "Mpa/ControleBancario",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro_ControleBancario
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Financeiro.QuitacaoPaciente,
                        L("QuitacaoPaciente"),
                        url: "Mpa/QuitacaoPaciente",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Financeiro.FluxoCaixa,
                        L("FluxoCaixa"),
                        url: "Mpa/FluxoCaixa",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro_FluxoCaixa
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Financeiro.RepasseMedico,
                        L("RepasseMedico"),
                        url: "Mpa/RepasseMedico",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro_RepasseMedico
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Financeiro.Relatorios.ContasPagar,
                        L("Relatorios"),
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro_ContasPagar
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Financeiro.Relatorios.ContasPagar,
                            L("ContasPagar"),
                            url: "Mpa/ContasPagar/ContasPagarRelatorio",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro_ContasPagar
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Financeiro.Relatorios.ContasReceber,
                            L("ContasReceber"),
                            url: "Mpa/ContasReceber/ContasReceberRelatorio",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Financeiro_ContasReceber
                        )
                    )
                )
            );
        }

        private static void CriarMenuControladoria(MenuDefinition menu, MenuItemCustomData customData)
        {
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Controladoria,
                    L("Controladoria"),
                    icon: "	fa fa-university",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Controladoria
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Controladoria.Orcamentos,
                        L("Orcamentos"),
                        url: "Mpa/Orcamentos",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Controladoria_Orcamentos
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Controladoria.Patrimonio,
                        L("Patrimonio"),
                        url: "Mpa/Patrimonio",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Controladoria_Patrimonio
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Controladoria.Contabilidade,
                        L("Contabilidade"),
                        url: "Mpa/Contabilidade",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Controladoria_Contabilidade
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Controladoria.Custos,
                        L("Custos"),
                        url: "Mpa/Custos",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Controladoria_Custos
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Controladoria.NotasFiscais,
                        L("NotasFiscais"),
                        url: "Mpa/NotasFiscais",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Controladoria_NotasFiscais,
                        //customData: _customData["novaAba"]
                        customData: customData
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Controladoria.Projetos,
                        L("Projetos"),
                        url: "Mpa/Projetos",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Controladoria_Projetos
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Controladoria.Eventos,
                        L("Eventos"),
                        url: "Mpa/Eventos",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Controladoria_Eventos
                    )
                )
            );
        }

        private static void CriarMenuApoio(MenuDefinition menu)
        {
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Apoio,
                    L("Apoio"),
                    icon: "fa fa-hospital",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Apoio
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.Nutricao,
                        L("Nutricao"),
                        url: "Mpa/Nutricao",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_Nutricao
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.CentralMateriais,
                        L("CentralMateriais"),
                        url: "Mpa/CentralMateriais",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_CentralMateriais
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.DisparoDeMensagem,
                        L("Disparo De Mensagem"),
                        url: "Mpa/DisparoDeMensagem",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Disparo_De_Mensagem
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.Aviso,
                        L("Avisos"),
                        url: "Mpa/avisos",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Aviso
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.SolicitacaoAntimicrobianos,
                        L("Solicitação AntiMicrobianos"),
                        url: "Mpa/SolicitacaoAntimicrobianos",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_SolicitacaoAntimicrobianos
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.Esterilizados,
                        L("Esterilizados"),
                        url: "Mpa/Esterilizados",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_Esterilizados
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.Manutencao,
                        L("Manutencao"),
                        url: "Mpa/Manutencao",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_Manutencao
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.Higienizacao,
                        L("Higienizacao"),
                        url: "Mpa/Higienizacao",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_Higienizacao
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.PortariaControleAcesso,
                        L("PortariaControleAcesso"),
                        url: "Mpa/PortariaControleAcesso",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_PortariaControleAcesso
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.LavanderiaRouparia,
                        L("LavanderiaRouparia"),
                        url: "Mpa/LavanderiaRouparia",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_LavanderiaRouparia
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.SAC,
                        L("SAC"),
                        url: "Mpa/SAC",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_SAC
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.SAME,
                        L("SAME"),
                        url: "Mpa/SAME",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_SAME
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.ControleInfeccao,
                        L("ControleInfeccao"),
                        url: "Mpa/ControleInfeccao",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_ControleInfeccao
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Apoio.Hospitalar,
                        L("Hospitalar"),
                        url: "Mpa/Hospitalar",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Apoio_Hospitalar
                    )
                )
            );
        }

        private static void CriarMenuCadastro(MenuDefinition menu, MenuItemCustomData customData)
        {
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Cadastros,
                    L("Cadastros"),
                    icon: "icon-book-open",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.CadastrosGlobais,
                        L("CadastrosGlobais"),
                        //url: "Mpa/AccessControl",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Paciente,
                            L("Paciente"),
                            url: "Mpa/Pacientes",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Paciente,
                            //customData: _customData["novaAba"]
                            customData: customData
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Medico,
                            L("Medico/ProfissionalSaude"),
                            url: "Mpa/Medicos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Medico
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Especialidade,
                            L("Especialidade"),
                            url: "Mpa/Especialidades",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Profissao,
                            L("Profissao"),
                            url: "Mpa/Profissoes",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Profissao
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Origem,
                            L("Origem"),
                            url: "Mpa/Origens",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Origem
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Naturalidade,
                            L("Naturalidade"),
                            url: "Mpa/Naturalidades",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Nacionalidade,
                            L("Nacionalidade"),
                            url: "Mpa/Nacionalidades",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Convenio,
                            L("Convenio"),
                            url: "Mpa/Convenios",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Convenio
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Plano,
                            L("Plano"),
                            url: "Mpa/Planos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Plano
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.TiposLogradouro,
                            L("TiposLogradouro"),
                            url: "Mpa/TiposLogradouro",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Pais,
                            L("Pais"),
                            url: "Mpa/Paises",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Pais
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Estado,
                            L("Estado"),
                            url: "Mpa/Estados",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Estado
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Cidade,
                            L("Cidade"),
                            url: "Mpa/Cidades",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cidade
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Cep,
                            L("Cep"),
                            url: "Mpa/Ceps",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cep
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.GruposCentroCusto,
                            L("GrupoCentroCusto"),
                            url: "Mpa/GruposCentroCusto",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Intervalo,
                            L("Intervalo"),
                            url: "Mpa/Intervalos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.TipoAcomodacao,
                            L("TipoAcomodacao"),
                            url: "Mpa/TiposAcomodacao",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Fornecedor,
                            L("Fornecedor"),
                            url: "Mpa/Fornecedores",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.ProdutoAcaoTerapeutica,
                            L("ProdutoAcaoTerapeutica"),
                            url: "Mpa/ProdutosAcoesTerapeutica",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_ProdutoAcaoTerapeutica
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.CentroCusto,
                            L("CentroCusto"),
                            url: "Mpa/CentrosCustos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.GrauInstrucao,
                            L("GrauInstrucao"),
                            url: "Mpa/GrausInstrucoes",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Nacionalidade,
                            L("Nacionalidade"),
                            url: "Mpa/Nacionalidades",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Feriado,
                            L("Feriado"),
                            url: "Mpa/Feriados",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Feriado
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.TipoParticipacao,
                            L("TipoParticipacao"),
                            url: "Mpa/TiposParticipacoes",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.TipoVinculoEmpregaticio,
                            L("TipoVinculoEmpregaticio"),
                            url: "Mpa/TiposVinculosEmpregaticios",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.GrupoCID,
                            L("GrupoCID"),
                            url: "Mpa/GruposCID",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.CapituloCID,
                            L("CapituloCID"),
                            url: "Mpa/CapitulosCID",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Parentesco,
                            L("Parentesco"),
                            url: "Mpa/Parentescos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.Indicacao,
                            L("Indicacao"),
                            url: "Mpa/Indicacoes",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.TipoSanguineo,
                            L("TipoSanguineo"),
                            url: "Mpa/TiposSanguineos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.ElementoHtml,
                            L("ElementosHtml"),
                            url: "Mpa/ElementosHtml",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosGlobais.ElementoHtmlTipo,
                            L("ElementosHtmlTipos"),
                            url: "Mpa/ElementosHtmlTipos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtmlTipo
                        )
                    )
                )
                //DominioTiss
                .AddItem(new MenuItemDefinition(
                        PageNames.App.CadastrosDominioTiss.DominioTiss,
                        L("TabelasDominioTiss"),
                        //url: "Mpa/AccessControl",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_DominioTiss
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosDominioTiss.TipoTabelaDominio,
                            L("TipoTabelaDominio"),
                            url: "Mpa/TiposTabelaDominio",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosDominioTiss.GrupoTipoTabelaDominio,
                            L("GrupoTipoTabelaDominio"),
                            url: "Mpa/GruposTipoTabelaDominio",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosDominioTiss.TabelaDominio,
                            L("TabelaDominio"),
                            url: "Mpa/TabelasDominio",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosDominioTiss.VersaoTiss,
                            L("VersaoTiss"),
                            url: "Mpa/VersoesTiss",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.CadastrosAtendimento.Atendimento,
                        L("Atendimento"),
                        //url: "Mpa/AccessControl",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Atendimento
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.TiposAtendimento,
                            L("TiposAtendimento"),
                            url: "Mpa/TiposAtendimento",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_TiposAtendimento
                        )
                    ).AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.TipoLocalChamada,
                            L("TipoLocalChamada"),
                            url: "Mpa/TipoLocalChamada",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_TiposLocalChamada
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.AgendamentoConsultaMedicoDisponibilidades
                                .AgendamentoConsultaMedicoDisponibilidade,
                            L("AgendamentoConsultaMedicoDisponibilidade"),
                            url: "Mpa/AgendamentoConsultaMedicoDisponibilidades",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.MotivoAlta,
                            L("MotivoAlta"),
                            url: "Mpa/MotivosAlta",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Atendimento_MotivosAlta
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.Leito,
                            L("Leito"),
                            url: "Mpa/Leitos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Atendimento_Leitos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.LeitoStatus,
                            L("LeitoStatus"),
                            url: "Mpa/LeitosStatus",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitosStatus
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.LeitoCaracteristica,
                            L("LeitoCaracteristica"),
                            url: "Mpa/LeitoCaracteristicas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.LeitoServico,
                            L("LeitoServico"),
                            url: "Mpa/LeitoServicos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoServicos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.UnidadeInternacao,
                            L("UnidadeInternacao"),
                            url: "Mpa/UnidadesInternacao",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.PainelSenha,
                            L("PainelSenha"),
                            url: "Mpa/PainelSenhas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_CadastrosAtendimentos_PainelSenha
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.Fila,
                            L("Fila"),
                            url: "Mpa/Filas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosAtendimentos_Fila
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.UnidadeInternacaoTipo,
                            L("UnidadeInternacaoTipo"),
                            url: "Mpa/UnidadeInternacaoTipos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.MovimentoAutomatico,
                            L("MovimentoAutomatico"),
                            url: "Mpa/MovimentosAutomaticos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_MovimentosAutomaticos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosAtendimento.ModeloTexto,
                            L("ModeloTexto"),
                            url: "Mpa/ModeloTexto",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Atendimento_ModeloTextos
                        )
                    )
                )

                //Assistencial
                .AddItem(
                    new MenuItemDefinition(
                            PageNames.App.CadastrosAssistenciais.Assistenciais,
                            L("Assistenciais"),
                            icon: "fa fa-user-md",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.Atestados,
                                L("Atestados"),
                                url: "Mpa/Atestados",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Assistencial_Atestados
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.TiposAtestados,
                                L("TiposAtestados"),
                                url: "Mpa/TiposAtestados",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Assistencial_Atestados
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.ModelosAtestados,
                                L("ModelosAtestados"),
                                url: "Mpa/ModelosAtestados",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Assistencial_Atestados
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.Divisoes,
                                L("Divisoes"),
                                url: "Mpa/Divisoes",
                                icon: "fa fa-columns",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_Divisao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.TiposRespostas,
                                L("TiposRespostas"),
                                url: "Mpa/TiposRespostas",
                                icon: "fa fa-pencil-square-o",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoResposta
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.TiposRespostasConfiguracoes,
                                L("TiposRespostasConfiguracoes"),
                                url: "Mpa/TiposRespostasConfiguracoes",
                                icon: "fa fa-cog",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoRespostaConfiguracao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.TiposControles,
                                L("TiposControles"),
                                url: "Mpa/TiposControles",
                                icon: "fa fa-sliders",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.PrescricoesItens,
                                L("PrescricoesItens"),
                                url: "Mpa/PrescricoesItens",
                                icon: "fa fa-file-text-o",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.PrescricoesStatus,
                                L("PrescricoesStatus"),
                                url: "Mpa/PrescricoesStatus",
                                icon: "fa fa-file-text-o",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.PrescricoesItensStatus,
                                L("PrescricoesItensStatus"),
                                url: "Mpa/PrescricoesItensStatus",
                                icon: "fa fa-file-text-o",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.VelocidadesInfusoes,
                                L("VelocidadeInfusao"),
                                url: "Mpa/VelocidadesInfusoes",
                                icon: "fa fa-info",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.FormasAplicacoes,
                                L("FormaAplicacao"),
                                url: "Mpa/FormasAplicacoes",
                                icon: "fa fa-check-square-o",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosAssistenciais.Frequencias,
                                L("Frequencia"),
                                url: "Mpa/Frequencias",
                                icon: "far fa-clock",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia
                            )
                        )
                )

            #region Cadastros - Laudo

                // Laudos
                .AddItem(
                    new MenuItemDefinition(
                            PageNames.App.CadastrosDiagnostico.Diagnostico,
                            L("Diagnostico"),
                            icon: "fa fa-heartbeat"
                        //,
                        //requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Laudo_Modelos
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosDiagnostico.ModeloLaudo,
                                L("ModelosLaudo"),
                                url: "Mpa/ModelosLaudos",
                                icon: "fas fa-plus-circle"
                            //,
                            //    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupos
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosDiagnostico.LaudoGrupo,
                                L("Grupo"),
                                url: "Mpa/LaudoGrupos",
                                icon: "fas fa-plus-circle"
                            //,
                            //    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupos
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosDiagnostico.Modalidade,
                                L("Modalidade"),
                                url: "Mpa/Modalidades",
                                icon: "fas fa-plus-circle"
                            //,
                            //    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupos
                            )
                        )
                )
                // fim Cadastros/Faturamentos

            #endregion cadastros - laudo.

            #region Cadastros - Faturamento

                // Faturamentos
                .AddItem(
                    new MenuItemDefinition(
                            PageNames.App.Faturamentos.Faturamento,
                            L("Faturamento"),
                            icon: "fa fa-calculator",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosFaturamento.Item,
                                L("Itens"),
                                url: "Mpa/FaturamentoItens",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Itens
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosFaturamento.Kit,
                                L("Kits"),
                                url: "Mpa/FaturamentoKits",
                                icon: "fas fa-plus-circle"
                            // ,
                            // requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Kits
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosFaturamento.Tabela,
                                L("TabelaPreco"),
                                url: "Mpa/FaturamentoTabelas",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabelas
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosFaturamento.TabelaPrecoConvenio,
                                L("TabelaPrecoConvenio"),
                                url: "Mpa/FaturamentoTabelaPrecoConvenios",
                                icon: "fas fa-plus-circle"
                            //,
                            //requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabelas
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosFaturamento.Autorizacao,
                                L("Autorizacoes"),
                                url: "Mpa/FaturamentoAutorizacoes",
                                icon: "fa fa-unlock-alt"
                            // , requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabelas
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosFaturamento.Guia,
                                L("Guias"),
                                url: "Mpa/FaturamentoGuias",
                                icon: "fas fa-plus-circle"
                            // , requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabelas
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosFaturamento.Grupo,
                                L("Grupos"),
                                url: "Mpa/FaturamentoGrupos",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupos
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                    PageNames.App.CadastrosFaturamento.Brasindice,
                                    L("Brasindice"),
                                    icon: "fas fa-plus-circle"
                                //,
                                //    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupos
                                )
                                .AddItem(
                                    new MenuItemDefinition(
                                        PageNames.App.CadastrosFaturamento.Brasindice,
                                        L("Preco"),
                                        url: "Mpa/FaturamentoBrasPrecos",
                                        icon: "fas fa-plus-circle"
                                    //,
                                    //    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupos
                                    )
                                )
                                .AddItem(new MenuItemDefinition(
                                        //PageNames.App.CadastrosGlobais.Laboratorio,
                                        "Laboratório",
                                        L("Laboratorio"),
                                        url: "Mpa/FaturamentoBrasLaboratorios",
                                        icon: "fas fa-plus-circle"
                                    //,
                                    //requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos
                                    )
                                )
                        )
                        .AddItem(new MenuItemDefinition(
                                //PageNames.App.CadastrosGlobais.Laboratorio,
                                "Itens Brasíndice",
                                L("ItemBrasindice"),
                                url: "Mpa/FaturamentoBrasItens",
                                icon: "fas fa-plus-circle"
                            //,
                            //requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                //PageNames.App.CadastrosGlobais.Laboratorio,
                                "Itens Brasíndice",
                                L("ApresentacaoBrasindice"),
                                url: "Mpa/FaturamentoBrasApresentacoes",
                                icon: "fas fa-plus-circle"
                            //,
                            //requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                "Moeda", //PageNames.App.CadastrosFaturamento.Brasindice,
                                L("Moeda"),
                                url: "Mpa/FaturamentoSisMoedas",
                                icon: "fas fa-plus-circle"
                            //,
                            //    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupos
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                "FaturamentoItemAutorizacao", //PageNames.App.CadastrosFaturamento.Brasindice,
                                L("FaturamentoItemAutorizacao"),
                                url: "Mpa/FaturamentoItensAutorizacoes",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Faturamento_FaturamentoItemAutorizacao
                            )
                        )
                )
                // fim Cadastros/Faturamentos

            #endregion cadastros - faturamento.

            #region CadastrosLaboratorios

                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Laboratorio,
                        L("Laboratorio"),
                        //url: "Mpa/AccessControl",
                        icon: "fa fa-flask",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Tecnico,
                            L("Tecnico"),
                            url: "Mpa/Tecnicos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tecnico
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Material,
                            L("Material"),
                            url: "Mpa/Materiais",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Material
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Metodo,
                            L("Metodo"),
                            url: "Mpa/Metodos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Metodo
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Unidade,
                            L("Unidade"),
                            url: "Mpa/LaboratoriosUnidades",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.KitExame,
                            L("KitExame"),
                            url: "Mpa/KitsExames",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_KitExame
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Setor,
                            L("Setor"),
                            url: "Mpa/Setores",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Setor
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.EquipamentoInterfaceamento,
                            L("Equipamentos"),
                            url: "Mpa/Equipamentos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento
                        )
                    )
                    //.AddItem(new MenuItemDefinition(
                    //    PageNames.App.CadastrosLaboratorio.InformacoesExame,
                    //    L("InformacoesExame"),
                    //    url: "Mpa/InformacoesExame",
                    //    icon: "fas fa-plus-circle",
                    //    requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_InformacoesExame
                    //    )
                    //)
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Tabela,
                            L("Tabelas"),
                            url: "Mpa/Tabelas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.FormatacaoExame,
                            L("FormatacaoExame"),
                            url: "Mpa/Formatas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Laboratorio_Cadastros_FormatacaoExame
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Exame,
                            L("Exame"),
                            url: "Mpa/Exames",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Exame
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Mapas,
                            L("Mapas"),
                            url: "Mpa/Mapas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Mapa
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Resultado,
                            L("Resultado"),
                            url: "Mpa/Resultados",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.ItemResultado,
                            L("ItemResultado"),
                            url: "Mpa/ItensResultados",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ItemResultado
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastrosLaboratorio.Cabecalho,
                            L("Cabecalho"),
                            url: "Mpa/Cabecalhos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Cabecalho
                        )
                    )
                )

            #endregion

            #region CadastrosSuprimentos

                .AddItem(new MenuItemDefinition(
                            PageNames.App.Common.CadastrosSuprimentos,
                            L("CadastrosSuprimentos"),
                            //url: "Mpa/AccessControl",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.Produto,
                                L("Produto"),
                                url: "Mpa/Produtos",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto
                            )
                        )

                        //---------------------
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoPalavraChave,
                                L("Palavra"),
                                url: "Mpa/ProdutosPalavrasChave",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoAcaoTerapeutica,
                                L("ProdutoAcaoTerapeutica"),
                                url: "Mpa/ProdutosAcoesTerapeutica",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.Grupo,
                                L("Grupo"),
                                url: "Mpa/Grupos",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoLaboratorio,
                                L("ProdutoLaboratorio"),
                                url: "Mpa/ProdutosLaboratorio",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoPortaria,
                                L("ProdutoPortaria"),
                                url: "Mpa/ProdutosPortaria",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoGrupoTratamento,
                                L("ProdutoGrupoTratamento"),
                                url: "Mpa/ProdutosGruposTratamento",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoLocalizacao,
                                L("ProdutoLocalizacao"),
                                url: "Mpa/Produtoslocalizacao",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoUnidade,
                                L("Unidade"),
                                url: "Mpa/Unidades",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoTipoUnidade,
                                L("TipoUnidade"),
                                url: "Mpa/ProdutosTiposUnidade",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoCodigoMedicamento,
                                L("ProdutoCodigoMedicamento"),
                                url: "Mpa/ProdutosCodigosMedicamento",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoEstoque,
                                L("ProdutoEstoque"),
                                url: "Mpa/ProdutosEstoque",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque
                            )
                        )
                        //.AddItem(new MenuItemDefinition(
                        //    PageNames.App.CadastrosSuprimentos.ProdutoClasse,
                        //    L("ProdutoClasse"),
                        //    url: "Mpa/ProdutosClasse",
                        //    icon: "fas fa-plus-circle",
                        //    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe
                        //   )
                        //)
                        //.AddItem(new MenuItemDefinition(
                        //    PageNames.App.CadastrosSuprimentos.ProdutoSubClasse,
                        //    L("ProdutoSubClasse"),
                        //    url: "Mpa/ProdutosSubClasse",
                        //    icon: "fas fa-plus-circle",
                        //    requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse
                        //   )
                        //)
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.ProdutoSubstancia,
                                L("ProdutoSubstancia"),
                                url: "Mpa/ProdutosSubstancia",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.TipoEntrada,
                                L("TipoEntrada"),
                                url: "Mpa/TiposEntrada",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.SuprimentosEstoque.TipoDocumento,
                                L("TipoDocumento"),
                                url: "Mpa/TiposDocumento",
                                icon: "glyphicon glyphicon-barcode",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.CadastrosSuprimentos.Kit,
                                L("KitEstoque"),
                                url: "Mpa/EstoqueKits",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Suprimentos_KitEstoque
                            )
                        )
                //---------------------
                )

            #endregion

                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.CadastrosFinanceiros,
                        L("Financeiro"),
                        //url: "Mpa/AccessControl",
                        icon: "fa fa-money-bill-alt",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.FormaPagamento,
                            L("FormaPagamento"),
                            url: "Mpa/FormaPagamentos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_FormaPagamento
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.GrupoDRE,
                            L("GrupoDRE"),
                            url: "Mpa/GrupoDREs",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_Grupo_DRE
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.GrupoContaAdministrativa,
                            L("GrupoContaAdministrativa"),
                            url: "Mpa/GrupoContasAdministrativas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Financeiro_Grupo_ContaAdministrativa
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.ContaAdministrativa,
                            L("ContaAdministrativa"),
                            url: "Mpa/contasAdministrativas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Financeiro_ContaAdministrativa
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.SituacaoLancamento,
                            L("SituacaoLancamento"),
                            url: "Mpa/SituacaoLancamentos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions
                                .Pages_Tenant_Cadastros_Financeiro_SituacaoLancamento
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.TipoDocumento,
                            L("TipoDocumento"),
                            url: "Mpa/TipoDocumento",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_TipoDocumento
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.MeioPagamento,
                            L("MeioPagamento"),
                            url: "Mpa/MeiosPagamentos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_MeioPagamento
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.RateioPadrao,
                            L("RateioPadrao"),
                            url: "Mpa/RateioCentroCustos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_RateioPadrao
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.Impostos,
                            L("Impostos"),
                            url: "Mpa/ContasPagar",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_Impostos
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.Servico,
                            L("Servico"),
                            url: "Mpa/ContasPagar",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_Servico
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.CadastroFinanceiro.CodigoFiscal,
                            L("CodigoFiscal"),
                            url: "Mpa/ContasPagar",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_CodigoFiscal
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Common.CadastrosFinanceirosBancarios,
                            L("Bancario"),
                            //url: "Mpa/AccessControl",
                            icon: "fa fa-money-bill-alt",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Cadastros_Financeiro_Bancario
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastroFinanceiro.BancoAgencias,
                                L("BancoAgencias"),
                                url: "Mpa/BancoAgencias",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Financeiro_Bancario_Banco_Agencias
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastroFinanceiro.TipoConta,
                                L("TipoConta"),
                                url: "Mpa/TipoContaCorrente",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Financeiro_Bancario_TipoConta
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastroFinanceiro.ContaTasouraria,
                                L("ContaTasouraria"),
                                url: "Mpa/ContaCorrente",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Financeiro_Bancario_ContaTasouraria
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PageNames.App.CadastroFinanceiro.TalaoCheque,
                                L("TalaoCheque"),
                                url: "Mpa/TalaoCheque",
                                icon: "fas fa-plus-circle",
                                requiredPermissionName: AppPermissions
                                    .Pages_Tenant_Cadastros_Financeiro_Bancario_TalaoCheque
                            )
                        )
                    )
                )
            );
        }

        private static void CriarMenuConfiguracao(MenuDefinition menu)
        {
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Configuracoes,
                    L("Configuracoes"),
                    icon: "fa fa-desktop",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Configuracoes
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Configuracoes.Empresa,
                        L("Empresas"),
                        url: "Mpa/Empresas",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Configuracoes_Empresa
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.App.Configuracoes.GeradorFormulario,
                        L("GeradorFormulario"),
                        url: "Mpa/GeradorFormularios",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Configuracoes_GeradorFormulario
                    )
                )
                //.AddItem(new MenuItemDefinition(
                //    PageNames.App.Configuracoes.AuditoriaTransacoes,
                //    L("AuditoriaTransacoes"),
                //    url: "Mpa/AuditoriaTransacoes",
                //    icon: "fas fa-plus-circle",
                //    requiredPermissionName: AppPermissions.Pages_Tenant_Configuracoes_AuditoriaTransacoes
                //    )
                //)
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Configuracoes.GeradorRelatorios,
                        L("GeradorRelatorios"),
                        url: "Mpa/GeradorRelatorios",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Configuracoes_GeradorRelatorios
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Configuracoes.Modulo,
                        L("Modulos"),
                        url: "Mpa/Modulos",
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Configuracoes_Modulo
                    )
                )
            );
        }


        private static void CriarMenuManutencao(MenuDefinition menu, MenuItemCustomData customData) =>
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Manutencao,
                    L("Manutencao"),
                    icon: "glyphicon glyphicon-cog",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Manutencao.Consultor,
                        L("Consultor"),
                        icon: "fas fa-plus-circle",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao_Consultor
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Manutencao.ConsultorTabela,
                            L("TabelaConsultor"),
                            url: "Mpa/ConsultorTabelas",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PageNames.App.Manutencao.ConsultorTabela,
                            L("TabelaConsultorCampos"),
                            url: "Mpa/ConsultorTabelaCampos",
                            icon: "fas fa-plus-circle",
                            requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Manutencao.MailingTemplates,
                        L("MailingTemplates"),
                        url: "Mpa/MailingTemplates",
                        icon: "icon-envelope",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao_MailingTemplates
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Manutencao.Guias,
                        L("Guias"),
                        //url: "Mpa/Guias",
                        url: "Mpa/GuiasFinal",
                        icon: "glyphicon glyphicon-list-alt"
                    //,
                    //requiredPermissionName: AppPermissions.Guias
                    )
                )
                .AddItem(new MenuItemDefinition(
                            "Desenvolvimento",
                            L("Desenvolvimento"),
                            icon: "fas fa-plus-circle"
                        //requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao_Consultor
                        )
                        .AddItem(new MenuItemDefinition(
                                "Documentacao",
                                L("Documentacao"),
                                url: "Mpa/DocItens",
                                icon: "fas fa-plus-circle"
                            //   requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela
                            )
                        )
                //.AddItem(new MenuItemDefinition(
                //    "Projetos",
                //    L("Projetos"),
                //    url: "Mpa/Projetos",
                //    icon: "fas fa-plus-circle"
                //    //   requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela
                //    )
                //)
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Manutencao.BIs,
                        L("BIs"),
                        url: "Mpa/BIs",
                        icon: "fa fa-pie-chart",
                        requiredPermissionName: AppPermissions.Pages_Tenant_Manutencao_BI
                    )
                )
            );


        private static void CriarMenuAdministracao(MenuDefinition menu)
        {
            menu.AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Administration,
                    L("Administration"),
                    icon: "icon-wrench"
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OrganizationUnits,
                        L("OrganizationUnits"),
                        url: "Mpa/OrganizationUnits",
                        icon: "icon-layers",
                        requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.VisualAsaImportExport,
                        L("VisualAsaImportExport"),
                        url: "Mpa/VisualAsaImportExport",
                        icon: "fa fa-database",
                        requiredPermissionName: AppPermissions.Pages_Administration_VisualAsaImportExport
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Roles,
                        L("Roles"),
                        url: "Mpa/Roles",
                        icon: "icon-briefcase",
                        requiredPermissionName: AppPermissions.Pages_Administration_Roles
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Users,
                        L("Users"),
                        url: "Mpa/Users",
                        icon: "icon-users",
                        requiredPermissionName: AppPermissions.Pages_Administration_Users
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Parametrizations,
                        L("Parametrizations"),
                        url: "Mpa/Parametrizacoes",
                        icon: "fa fa-tools",
                        requiredPermissionName: AppPermissions.Pages_Administration_Users
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Languages,
                        L("Languages"),
                        url: "Mpa/Languages",
                        icon: "icon-flag",
                        requiredPermissionName: AppPermissions.Pages_Administration_Languages
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.AuditLogs,
                        L("AuditLogs"),
                        url: "Mpa/AuditLogs",
                        icon: "icon-lock",
                        requiredPermissionName: AppPermissions.Pages_Administration_AuditLogs
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Maintenance,
                        L("Maintenance"),
                        url: "Mpa/Maintenance",
                        icon: "icon-wrench",
                        requiredPermissionName: AppPermissions.Pages_Administration_Host_Maintenance
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Settings,
                        L("Settings"),
                        url: "Mpa/HostSettings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Host_Settings
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PageNames.App.Tenant.Settings,
                        L("Settings"),
                        url: "Mpa/Settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Tenant_Settings
                    )
                )
            );
        }

        #endregion


        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SWMANAGERConsts.LocalizationSourceName);
        }
    }

    public class MenuItemCustomData
    {
        public KeyValuePair<string, string> Target { get; set; }
        public KeyValuePair<string, string> TargetAssistencial { get; set; }
        public KeyValuePair<string, string> Metodo { get; set; }
        public string Parametro { get; set; }
        public bool IsFavorito { get; set; }

        public MenuItemCustomData()
        {
            Target = new KeyValuePair<string, string>(string.Empty, string.Empty);
            //TargetAssistencial = new KeyValuePair<string, string>(string.Empty, string.Empty);
            //Metodo = new KeyValuePair<string, string>(string.Empty, string.Empty);
            IsFavorito = false;
        }
    }
}