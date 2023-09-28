namespace SW10.SWMANAGER.Web.Navigation
{
    public static class PageNames
    {
        public static class App
        {
            public static class Common
            {
                public const string Administration = "Administration";
                public const string Roles = "Administration.Roles";
                public const string Users = "Administration.Users";
                public const string Parametrizations = "Administration.Parametrizations";
                public const string AuditLogs = "Administration.AuditLogs";
                public const string OrganizationUnits = "Administration.OrganizationUnits";
                public const string Languages = "Administration.Languages";
                public const string VisualAsaImportExport = "Administration.VisualAsaImportExport";

                //itens do menu da aplica��o
                public const string Atendimento = "Atendimento";
                public const string Assistencial = "Assistencial";
                public const string Diagnosticos = "Diagnosticos";
                public const string Suprimentos = "Suprimentos";
                public const string SuprimentosEstoque = "Suprimentos.Estoque";
                public const string Faturamento = "Faturamento";
                public const string Manutencao = "Manutencao";
                public const string Financeiro = "Financeiro";
                public const string Controladoria = "Controladoria";
                public const string Apoio = "Apoio";
                public const string Configuracoes = "Configuracoes";
                public const string Cadastros = "Cadastros";
                public const string CadastrosGlobais = "Cadastros.CadastrosGlobais";
                public const string CadastrosDominioTiss = "Cadastros.DominioTiss";
                public const string CadastrosSuprimentos = "Cadastros.Suprimentos";
                public const string CadastrosLaboratorio = "Cadastros.Laboratorios";
                //public const string TiposAtendimento = "Cadastros.Atendimento";
                public const string Welcome = "Welcome";
                public const string Laboratorio = "Laboratorio";
                public const string CadastrosFinanceiros = "Cadastros.Financeiros";
                public const string CadastrosFinanceirosBancarios = "Cadastros.Financeiros.Bancarios";
            }

            public static class Host
            {
                public const string Tenants = "Tenants";
                public const string Editions = "Editions";
                public const string Maintenance = "Administration.Maintenance";
                public const string Settings = "Administration.Settings.Host";
            }

            public static class Tenant
            {
                public const string Dashboard = "Tenant.Dashboard";
                public const string Settings = "Tenant.Administration.Settings";
            }

            public static class Atendimentos
            {
                public const string Atendimento = "Atendimento";
                public const string Clinico = "Atendimento.Clinico";
                public const string Emergencia = "Atendimento.Emergencia";
                public const string CentralAutorizacao = "Atendimento.CentralAutorizacao";
                public const string AmbulatorioEmergencia = "Atendimento.AmbulatorioEmergencia";
                public const string AtendimentoExames = "Atendimento.Exames";
                public const string ClassificacaoRiscos = "Atendimento.ClassificacaoRiscos";
                public const string Internacao = "Atendimento.Internacao";
                public const string Prorrogacao = "Atendimento.Prorrogacao";
                public const string HomeCare = "Atendimento.HomeCare";
                public const string AgendamentoConsultas = "Atendimento.AgendamentoConsultas";
                public const string AgendamentoExames = "Atendimento.AgendamentoExames";
                public const string AgendamentoCirurgias = "Atendimento.AgendamentoCirurgias";
                public const string PreAtendimento = "Atendimento.PreAtendimento";
                public const string PesquisaPacientes = "Atendimento.PesquisaPacientes";
                public const string FluxoAtendimento = "Atendimento.FluxoAtendimento";
                public const string IdentificacaoPaciente = "Atendimento.IdentificacaoPaciente";
                public const string Orcamento = "Atendimento.Orcamento";
                public const string Agendamento = "Atendimento.Agendamento";
                public const string Triagem = "Atendimento.Triagem";
                public const string Autorizacao = "Atendimento.Autorizacao";
                public const string ControleVisitantes = "Atendimento.ControleVisitantes";
                public const string ControleSenhas = "Atendimento.ControleSenhas";
                public const string ControleCaixa = "Atendimento.ControleCaixa";
                public const string Relatorio = "Atendimento.Relatorio";
                public const string AtendimentoLeitoMov = "Atendimento.AtendimentoLeitoMov";
                public const string PainelSenha = "Atendimento.PainelSenha";
                public const string MonitorPainelSenha = "Atendimento.MonitorPainelSenha";

                // Parciais
                public const string ClassificacaoRisco = "Atendimento.ClassificacaoRisco";

                public const string PesquisaPreAtendimentos = "Atendimento.PesquisaAtendimentos";

                //Central de Autoriza��es
                public const string SolicitacaoAutorizacaoProcedimento = "Atendimento.SolicitacaoAutorizacaoProcedimento";
            }

            public static class AtendimentosRelatorio
            {
                public const string RelatorioInternacao = "Atendimentos.Relatorio.RelatorioInternacao";
                public const string RelatorioAtendimento = "Atendimentos.Relatorio.RelatorioAtendimento";
            }

            public static class Assistenciais
            {
                public const string Assistencial = "Assistencial";
                public const string Atestados = "Assistencial.Atestados";
                public const string AtestadoMedico = "Assistencial.AtestadoMedico";
                public const string AtestadoComparecimento = "Assistencial.AtestadoComparecimento";
                public const string ProntuarioEletronico = "Assistencial.ProntuarioEletronico";



                public static class AssistencialAtendimentos
                {
                    public const string Atendimentos = "Assistencial.Atendimentos";
                    public const string Atendimento = "Assistencial.Atendimento";
                    public const string AmbulatorioEmergencia = "Assistencial.AmbulatorioEmergencia";
                    public const string Internacao = "Assistencial.Internacao";
                }

                public static class AmbulatoriosEmergencias
                {
                    public const string Enfermagem = "Assistencial.AmbulatorioEmergencia.Enfermagem";
                    public const string Medico = "Assistencial.AmbulatorioEmergencia.Medico";
                    public const string Administrativo = "Assistencial.AmbulatorioEmergencia.Administrativo";

                    public static class Enfermagens
                    {
                        public const string Admissao = "Assistencial.AmbulatorioEmergencia.Enfermagem.Admissao";
                        public const string Evolucao = "Assistencial.AmbulatorioEmergencia.Enfermagem.Evolucao";
                        public const string PassagemPlantao = "Assistencial.AmbulatorioEmergencia.Enfermagem.PassagemPlantao";
                        public const string Prescricao = "Assistencial.AmbulatorioEmergencia.Enfermagem.Prescricao";
                        public const string Checagem = "Assistencial.AmbulatorioEmergencia.Enfermagem.Checagem";
                        public const string SinaisVitais = "Assistencial.AmbulatorioEmergencia.Enfermagem.SinaisVitais";
                        public const string ControleBalancoHidrico = "Assistencial.AmbulatorioEmergencia.Enfermagem.ControleBalancoHidrico";
                    }

                    public static class Medicos
                    {
                        public const string Admissao = "Assistencial.AmbulatorioEmergencia.Medico.Admissao";
                        public const string Alta = "Assistencial.AmbulatorioEmergencia.Medico.Alta";
                        public const string Anamnese = "Assistencial.AmbulatorioEmergencia.Medico.Anamnese";
                        public const string AnamneseClinicoGeral = "Assistencial.AmbulatorioEmergencia.Medico.AnamneseClinicoGeral";
                        public const string AnamnesePediatria = "Assistencial.AmbulatorioEmergencia.Medico.AnamnesePediatria";
                        public const string Evolucao = "Assistencial.AmbulatorioEmergencia.Medico.Evolucao";
                        public const string ParecerEspecialista = "Assistencial.AmbulatorioEmergencia.Medico.ParecerEspecialista";
                        public const string Prescricao = "Assistencial.AmbulatorioEmergencia.Medico.Prescricao";
                        public const string SolicitacaoExame = "Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoExame";
                        public const string SolicitacaoAntimicrobiano = "Assistencial.AmbulatorioEmergencia.Medico.SolicitacaoAntimicrobiano";
                        public const string ResultadoExame = "Assistencial.AmbulatorioEmergencia.Medico.ResultadoExame";
                        public const string ResumoAlta = "Assistencial.AmbulatorioEmergencia.Medico.ResumoAlta";
                        public const string ProcedimentoCirurgico = "Assistencial.AmbulatorioEmergencia.Medico.ProcedimentoCirurgico";
                        public const string DescricaoAtoCirurgico = "Assistencial.AmbulatorioEmergencia.Medico.DescricaoAtoCirurgico";
                        public const string DescricaoAtoAnestesico = "Assistencial.AmbulatorioEmergencia.Medico.DescricaoAtoAnestesico";
                        public const string FolhaGastoCentroCirurgico = "Assistencial.AmbulatorioEmergencia.Medico.FolhaGastoCentroCirurgico";
                        public const string Partograma = "Assistencial.AmbulatorioEmergencia.Medico.Partograma";
                        public const string Receituario = "Assistencial.AmbulatorioEmergencia.Medico.Receituario";
                    }

                    public static class Administrativos
                    {
                        public const string Administrativo = "Assistencial.AmbulatorioEmergencia.Administrativo";
                        public const string CAT = "Assistencial.AmbulatorioEmergencia.Administrativo.CAT";
                        public const string Alergia = "Assistencial.AmbulatorioEmergencia.Administrativo.Alergia";
                        public const string DocumentacaoPaciente = "Assistencial.AmbulatorioEmergencia.Administrativo.DocumentacaoPaciente";
                        public const string ConfirmacaoAgenda = "Assistencial.AmbulatorioEmergencia.Administrativo.ConfirmacaoAgenda";
                        public const string AgendaConsulta = "Assistencial.AmbulatorioEmergencia.Administrativo.ConfirmacaoAgenda.AgendaConsulta";
                        public const string AgendaExame = "Assistencial.AmbulatorioEmergencia.Administrativo.ConfirmacaoAgenda.AgendaExame";
                        public const string AgendaCirurgia = "Assistencial.AmbulatorioEmergencia.Administrativo.ConfirmacaoAgenda.AgendaCirurgia";
                        public const string Tranferencia = "Assistencial.AmbulatorioEmergencia.Administrativo.Transferencia";
                        public const string TranferenciaLeito = "Assistencial.AmbulatorioEmergencia.Administrativo.TransferenciaLeito";
                        public const string TransferenciaMedicoResponsavel = "Assistencial.AmbulatorioEmergencia.Administrativo.TransferenciaMedicoResponsavel";
                        public const string TransferenciaSetor = "Assistencial.AmbulatorioEmergencia.Administrativo.TransferenciaSetor";
                        public const string ExameInterno = "Assistencial.AmbulatorioEmergencia.Administrativo.TransferenciaSetor.ExameInterno";
                        public const string ExameExterno = "Assistencial.AmbulatorioEmergencia.Administrativo.TransferenciaSetor.ExameExterno";
                        public const string Cirurgia = "Assistencial.AmbulatorioEmergencia.Administrativo.TransferenciaSetor.Cirurgia";
                        public const string Alta = "Assistencial.AmbulatorioEmergencia.Administrativo.Alta";
                        public const string AlteracaoAtendimento = "Assistencial.AmbulatorioEmergencia.Administrativo.AlteracaoAtendimento";
                        public const string PassagemPlantaoEnfermagem = "Assistencial.AmbulatorioEmergencia.Administrativo.PassagemPlantaoEnfermagem";
                        public const string SolicitacaoProrrogacao = "Assistencial.AmbulatorioEmergencia.Administrativo.SolicitacaoProrrogacao";
                        public const string SolicitacaoProdutoSetor = "Assistencial.AmbulatorioEmergencia.Administrativo.SolicitacaoProdutoSetor";
                        public const string SolicitacaoProdutoSOS = "Assistencial.AmbulatorioEmergencia.Administrativo.SolicitacaoProdutoSOS";
                        public const string LiberacaoInterdicaoLeito = "Assistencial.AmbulatorioEmergencia.Administrativo.LiberacaoInterdicaoLeito";
                    }
                }

                public static class Internacoes
                {
                    public const string Enfermagem = "Assistencial.Internacao.Enfermagem";
                    public const string Medico = "Assistencial.Internacao.Medico";
                    public const string Administrativo = "Assistencial.Internacao.Administrativo";
                    public const string BalancoHidrico = "Assistencial.Internacao.BalancoHidrico";

                    public static class Enfermagens
                    {
                        public const string Admissao = "Assistencial.Internacao.Enfermagem.Admissao";
                        public const string Evolucao = "Assistencial.Internacao.Enfermagem.Evolucao";
                        public const string PassagemPlantao = "Assistencial.Internacao.Enfermagem.PassagemPlantao";
                        public const string Prescricao = "Assistencial.Internacao.Enfermagem.Prescricao";
                        public const string Checagem = "Assistencial.Internacao.Enfermagem.Checagem";
                        public const string SinaisVitais = "Assistencial.Internacao.Enfermagem.SinaisVitais";
                        public const string ControleBalancoHidrico = "Assistencial.Internacao.Enfermagem.ControleBalancoHidrico";
                    }

                    public static class Medicos
                    {
                        public const string Admissao = "Assistencial.Internacao.Medico.Admissao";
                        public const string Alta = "Assistencial.Internacao.Medico.Alta";
                        public const string Anamnese = "Assistencial.Internacao.Medico.Anamnese";
                        public const string AnamneseClinicoGeral = "Assistencial.Internacao.Medico.AnamneseClinicoGeral";
                        public const string AnamnesePediatria = "Assistencial.Internacao.Medico.AnamnesePediatria";
                        public const string Evolucao = "Assistencial.Internacao.Medico.Evolucao";
                        public const string ParecerEspecialista = "Assistencial.Internacao.Medico.ParecerEspecialista";
                        public const string Prescricao = "Assistencial.Internacao.Medico.Prescricao";
                        public const string ResultadoExame = "Assistencial.Internacao.Medico.ResultadoExame";
                        public const string Laboratorial = "Assistencial.Internacao.Medico.ResultadoExame.Laboratorial";
                        public const string Imagem = "Assistencial.Internacao.Medico.ResultadoExame.Imagem";
                        public const string ResumoAlta = "Assistencial.Internacao.Medico.ResumoAlta";
                        public const string ProcedimentoCirurgico = "Assistencial.Internacao.Medico.ProcedimentoCirurgico";
                        public const string DescricaoAtoCirurgico = "Assistencial.Internacao.Medico.DescricaoAtoCirurgico";
                        public const string DescricaoAtoAnestesico = "Assistencial.Internacao.Medico.DescricaoAtoAnestesico";
                        public const string FolhaGastoCentroCirurgico = "Assistencial.Internacao.Medico.FolhaGastoCentroCirurgico";
                        public const string Partograma = "Assistencial.Internacao.Medico.Partograma";
                    }

                    public static class Administrativos
                    {
                        public const string Administrativo = "Assistencial.Internacao.Administrativo.CAT";
                        public const string Alergia = "Assistencial.Internacao.Administrativo.Alergia";
                        public const string DocumentacaoPaciente = "Assistencial.Internacao.Administrativo.DocumentacaoPaciente";
                        public const string ConfirmacaoAgenda = "Assistencial.Internacao.Administrativo.ConfirmacaoAgenda";
                        public const string AgendaConsulta = "Assistencial.Internacao.Administrativo.ConfirmacaoAgenda.AgendaConsulta";
                        public const string AgendaExame = "Assistencial.Internacao.Administrativo.ConfirmacaoAgenda.AgendaExame";
                        public const string AgendaCirurgia = "Assistencial.Internacao.Administrativo.ConfirmacaoAgenda.AgendaCirurgia";
                        public const string TranferenciaLeito = "Assistencial.Internacao.Administrativo.TransferenciaLeito";
                        public const string TransferenciaMedicoResponsavel = "Assistencial.Internacao.Administrativo.TransferenciaMedicoResponsavel";
                        public const string TransferenciaSetor = "Assistencial.Internacao.Administrativo.TransferenciaSetor";
                        public const string ExameInterno = "Assistencial.Internacao.Administrativo.TransferenciaSetor.ExameInterno";
                        public const string ExameExterno = "Assistencial.Internacao.Administrativo.TransferenciaSetor.ExameExterno";
                        public const string Cirurgia = "Assistencial.Internacao.Administrativo.TransferenciaSetor.Cirurgia";
                        public const string Alta = "Assistencial.Internacao.Administrativo.Alta";
                        public const string AlteracaoAtendimento = "Assistencial.Internacao.Administrativo.AlteracaoAtendimento";
                        public const string PassagemPlantaoEnfermagem = "Assistencial.Internacao.Administrativo.PassagemPlantaoEnfermagem";
                        public const string SolicitacaoProrrogacao = "Assistencial.Internacao.Administrativo.SolicitacaoProrrogacao";
                        public const string SolicitacaoProdutoSetor = "Assistencial.Internacao.Administrativo.SolicitacaoProdutoSetor";
                        public const string SolicitacaoProdutoSOS = "Assistencial.Internacao.Administrativo.SolicitacaoProdutoSOS";
                        public const string LiberacaoInterdicaoLeito = "Assistencial.Internacao.Administrativo.LiberacaoInterdicaoLeito";
                    }
                }
            }

            public static class Diagnosticos
            {
                public const string Laboratorio = "Diagnosticos.Laboratorio";
                public const string Imagens = "Diagnosticos.Imagens.DiagnosticoPorImagem";
                public const string GestaoDeLaudos = "Diagnosticos.Imagens.GestaoDeLaudos";
                public const string RegistroExame = "Diagnosticos.Imagens.RegistroExame";
                public const string DICOM = "Diagnosticos.Imagens.DICOM";
            }

            public static class Suprimentos
            {
                public const string Compras = "Suprimentos.Compras";
                public const string ComprasRequisicao = "Suprimentos.Compras.ComprasRequisicao";
                public const string ComprasAprovacao = "Suprimentos.Compras.ComprasAprovacao";
                public const string ComprasCotacao = "Suprimentos.Compras.ComprasCotacao";

                public const string Almoxarifado = "Suprimentos.Almoxarifado";
                public const string Farmacia = "Suprimentos.Farmacia";
                public const string Estoque = "Suprimentos.Estoque";
                public const string Relatorio = "Suprimentos.Relatorio";
                public const string Estoque_Soliciatacao = "Suprimentos.Estoque.Solicitacao";
                
            }

            public static class SuprimentosRelatorio
            {
                public const string SaldoProduto = "Suprimentos.Relatorio.SaldoProduto";
                public const string MovimentacaoProduto = "Suprimentos.Relatorio.MovimentacaoProduto";
                public const string Acuracia = "Suprimentos.Relatorio.Acuracia";
            }

            public static class SuprimentosEstoque
            {
                public const string Entrada = "Suprimentos.SuprimentosEstoque.Entrada";
                public const string TipoEntrada = "Suprimentos.SuprimentosEstoque.TipoEntrada";
                public const string TipoDocumento = "Suprimentos.SuprimentosEstoque.TipoDocumento";
                public const string PreMovimento = "Suprimentos.SuprimentosEstoque.PreMovimento";
                public const string PreMovimentoItem = "Suprimentos.SuprimentosEstoque.PreMovimentoItem";
                public const string Saida = "Suprimentos.SuprimentosEstoque.Saida";
                public const string Transferencia = "Suprimentos.SuprimentosEstoque.Transferencia";
                public const string ConfirmaMovimento = "Suprimentos.SuprimentosEstoque.ConfirmaMovimento";
                public const string BaixaVale = "Suprimentos.SuprimentosEstoque.BaixaVale";
                public const string BaixaConsignado = "Suprimentos.SuprimentosEstoque.BaixaConsignado";
                public const string DevolucaoProduto = "Suprimentos.SuprimentosEstoque.DevolucaoProduto";
                public const string SolicitacaoSaida = "Suprimentos.SuprimentosEstoque.SolicitacaoSaida";
                public const string RequisicaoCompra = "Suprimentos.SuprimentosEstoque.RequisicaoCompra";
                public const string AprovacaoCompra = "Suprimentos.SuprimentosEstoque.AprovacaoCompra";
                public const string CotacaoCompra = "Suprimentos.SuprimentosEstoque.CotacaoCompra";
                public const string OrdemCompra = "Suprimentos.SuprimentosEstoque.OrdemCompra";
                public const string AtendimentoSolicitacao = "Suprimentos.SuprimentosEstoque.AtendimentoSolicitacao";
                public const string MontagemKit = "Suprimentos.SuprimentosEstoque.MontagemKit";
                public const string Estoque_EmissaoEtiqueta = "Suprimentos.Estoque.EmissaoEtiqueta";
                public const string Estoque_Inventario = "Suprimentos.Estoque.Inventario";
                public const string Estoque_Importacao_Produto = "Suprimentos.Estoque.ImportacaoProduto";
                public const string Emprestimo = "Suprimentos.Estoque.Emprestimo";
                public const string EmprestimoSaida = "Suprimentos.Estoque.Emprestimo.Saida";
                public const string EmprestimoEntrada = "Suprimentos.Estoque.Emprestimo.Entrada";
                public const string EmprestimoReceberEmprestimo = "Suprimentos.Estoque.Emprestimo.EmprestimoRecebimento";
                public const string EmprestimoReceberEmprestimoSolicitacao = "Suprimentos.Estoque.Emprestimo.ReceberEmprestimo.Solicitacao";
                public const string EmprestimoReceberEmprestimoBaixaSolicitacao = "Suprimentos.Estoque.Emprestimo.ReceberEmprestimo.BaixaSolicitacao";
                public const string EmprestimoConcederEmprestimoSolicitacao = "Suprimentos.Estoque.Emprestimo.ConcederEmprestimo.Solicitacao";
                public const string EmprestimoReceberEmprestimoBaixa = "Suprimentos.Estoque.Emprestimo.ReceberEmprestimo.Baixa";
                public const string EmprestimoConcederEmprestimoBaixa = "Suprimentos.Estoque.Emprestimo.ConcederEmprestimo.Baixa";
                public const string EmprestimoConsultaDevolucao = "Suprimentos.Estoque.Emprestimo.ConsultaDevolucao";
                public const string EmprestimoBaixaDevolucao = "Suprimentos.Estoque.Emprestimo.BaixaDevolucao";
                public const string EstoqueKits = "Suprimentos.Estoque.Kit";
            }

            public static class Faturamentos
            {
                public const string Faturamento = "Faturamento";
                public const string FaturamentoContasMedicas = "Faturamento.ContasMedicas";
                public const string FaturamentoAuditoriaInterna = "Faturamento.AuditoriaInterna";
                public const string FaturamentoAuditoriaExterna = "Faturamento.AuditoriaExterna";
                public const string FaturamentoEntregaContas = "Faturamento.EntregaContas";
                public const string FaturamentoLotes = "Faturamento.Lotes";
                public const string FaturamentoConveniosParticular = "Faturamento.FaturamentoConveniosParticular";
                public const string FaturamentoSUSInternacao = "Faturamento.FaturamentoSUSInternacao";
                public const string FaturamentoSUSAmbulatorio = "Faturamento.FaturamentoSUSAmbulatorio";
                public const string Auditoria = "Faturamento.Auditoria";
                public const string RecursoGlosa = "Faturamento.RecursoGlosa";
                public const string CentralAutorizacaoGuias = "Faturamento.CentralAutorizacaoGuias";
                public const string RegrasConveniosParticulares = "Faturamento.RegrasConveniosParticulares";
            }

            public static class Financeiro
            {
                public const string ContasPagar = "Financeiro.ContasPagar";
                public const string ContasReceber = "Financeiro.ContasReceber";
                public const string ControleBancario = "Financeiro.ControleBancario";
                public const string QuitacaoPaciente = "Financeiro.QuitacaoPaciente";
                public const string Tesouraria = "Financeiro.Tesouraria";
                public const string FluxoCaixa = "Financeiro.FluxoCaixa";
                public const string RepasseMedico = "Financeiro.RepasseMedico";

                public static class Relatorios
                {
                    public const string ContasPagar = "Financeiro.Relatorios.ContasPagar";
                    public const string ContasReceber = "Financeiro.Relatorios.ContasReceber";
                    public const string ControleBancario = "Financeiro.Relatorios.ControleBancario";
                    public const string Tesouraria = "Financeiro.Relatorios.Tesouraria";
                    public const string FluxoCaixa = "Financeiro.Relatorios.FluxoCaixa";
                    public const string RepasseMedico = "Financeiro.Relatorios.RepasseMedico";
                }

            }

            public static class CadastroFinanceiro
            {
                public const string FormaPagamento = "CadastroFinanceiro.FormaPagamento";
                public const string GrupoDRE = "CadastroFinanceiro.GrupoDRE";
                public const string GrupoContaAdministrativa = "CadastroFinanceiro.GrupoContaAdministrativa";
                public const string ContaAdministrativa = "CadastroFinanceiro.ContaAdministrativa";
                public const string SituacaoLancamento = "CadastroFinanceiro.SituacaoLancamento";
                public const string TipoDocumento = "CadastroFinanceiro.TipoDocumento";
                public const string MeioPagamento = "CadastroFinanceiro.MeioPagamento";
                public const string RateioPadrao = "CadastroFinanceiro.RateioPadrao";
                public const string Impostos = "CadastroFinanceiro.Impostos";
                public const string Servico = "CadastroFinanceiro.Servico";
                public const string CodigoFiscal = "CadastroFinanceiro.CodigoFiscal";
                public const string BancoAgencias = "Financeiro.Bancario.BancoAgencias";
                public const string TipoConta = "Financeiro.Bancario.TipoConta";
                public const string ContaTasouraria = "Financeiro.Bancario.ContaTasouraria";
                public const string TalaoCheque = "Financeiro.Bancario.TalaoCheque";

            }



            public static class Controladoria
            {
                public const string Orcamentos = "Controladoria.Orcamentos";
                public const string Patrimonio = "Controladoria.Patrimonio";
                public const string Contabilidade = "Controladoria.Contabilidade";
                public const string Custos = "Controladoria.Custos";
                public const string NotasFiscais = "Controladoria.NotasFiscais";
                public const string Eventos = "Controladoria.Eventos";
                public const string Projetos = "Controladoria.Projetos";
            }

            public static class Apoio
            {
                public const string Nutricao = "Apoio.Nutricao";
                public const string CentralMateriais = "Apoio.CentralMateriais";
                public const string Esterilizados = "Apoio.Esterilizados";
                public const string SolicitacaoAntimicrobianos = "Apoio.SolicitacaoAntimicrobianos";
                public const string Manutencao = "Apoio.Manutencao";
                public const string Higienizacao = "Apoio.Higienizacao";
                public const string PortariaControleAcesso = "Apoio.PortariaControleAcesso";
                public const string LavanderiaRouparia = "Apoio.LavanderiaRouparia";
                public const string SAC = "Apoio.SAC";
                public const string SAME = "Apoio.SAME";
                public const string ControleInfeccao = "Apoio.ControleInfeccao";
                public const string Hospitalar = "Apoio.Hospitalar";
                public const string DisparoDeMensagem = "Apoio.DisparoDeMensagem";
                public const string Aviso = "Apoio.Aviso";
            }

            public static class Configuracoes
            {
                public const string Empresa = "Configuracoes.Empresa";
                public const string Empresas = "Configuracoes.Empresas";
                public const string GeradorFormulario = "Configuracoes.GeradorFormulario";
                public const string GeradorFormularios = "Configuracoes.GeradorFormularios";
                //public const string ControleUsuarios = "Configuracoes.ControleUsuarios";
                //public const string AuditoriaTransacoes = "Configuracoes.AuditoriaTransacoes";
                public const string GeradorRelatorios = "Configuracoes.GeradorRelatorios";
                public const string Modulo = "Configuracoes.Modulo";
                public const string Modulos = "Configuracoes.Modulos";
                public const string Operacao = "Configuracoes.Operacao";
                public const string Operacoes = "Configuracoes.Operacoes";
            }

            public static class CadastrosGlobais
            {
                public const string Paciente = "Cadastros.CadastrosGlobais.Patientes";
                public const string Medico = "Cadastros.CadastrosGlobais.Medicos";
                public const string Especialidade = "Cadastros.CadastrosGlobais.Especialidades";
                public const string Intervalo = "Cadastros.CadastrosGlobais.Intervalos";
                public const string Profissao = "Cadastros.CadastrosGlobais.Profissoes";
                public const string Origem = "Cadastros.CadastrosGlobais.Origens";
                public const string Naturalidade = "Cadastros.CadastrosGlobais.Naturalidades";
                public const string Nacionalidade = "Cadastros.CadastrosGlobais.Naturalidades";
                public const string Convenio = "Cadastros.CadastrosGlobais.Convenios";
                public const string Plano = "Cadastros.CadastrosGlobais.Planos";
                public const string Pais = "Cadastros.CadastrosGlobais.Paises";
                public const string Estado = "Cadastros.CadastrosGlobais.Estados";
                public const string Cidade = "Cadastros.CadastrosGlobais.Cidades";
                public const string Cep = "Cadastros.CadastrosGlobais.Ceps";
                public const string GruposCentroCusto = "Cadastros.CadastrosGlobais.GruposCentroCustos";
                public const string TipoAcomodacao = "Cadastros.CadastrosGlobais.TiposAcomodacao";
                public const string Fornecedor = "Cadastros.CadastrosGlobais.Fornecedores";
                public const string TiposLogradouro = "Cadastros.CadastrosGlobais.TiposLogradouro";
                public const string MotivosCancelamento = "Cadastros.CadastrosGlobais.MotivosCancelamento";
                public const string InstituicoesTransferencia = "Cadastros.CadastrosGlobais.InstituicoesTransferencia";
                public const string MotivosCaucao = "Cadastros.CadastrosGlobais.MotivosCaucao";
                public const string MotivosTransferenciaLeito = "Cadastros.CadastrosGlobais.MotivosTransferenciaLeito";
                public const string Leito = "Cadastros.CadastrosGlobais.Leito";
                public const string TiposLeito = "Cadastros.CadastrosGlobais.TiposLeito";
                public const string Regiao = "Cadastros.CadastrosGlobais.Regioes";
                public const string Feriado = "Cadastros.CadastrosGlobais.Feriados";
                public const string ProdutoAcaoTerapeutica = "Cadastros.CadastrosGlobais.ProdutosAcoesTerapeutica";
                public const string CentroCusto = "Cadastros.CadastrosGlobais.CentroCustos";
                public const string GrauInstrucao = "Cadastros.CadastrosGlobais.GrausInstrucoes";
                public const string Indicacao = "Cadastros.CadastrosGlobais.Indicacoes";
                public const string Parentesco = "Cadastros.CadastrosGlobais.Parentescos";
                public const string TipoVinculoEmpregaticio = "Cadastros.CadastrosGlobais.TiposVinculosEmpregaticios";
                public const string TipoParticipacao = "Cadastros.CadastrosGlobais.TiposParticipacoes";
                public const string CapituloCID = "Cadastros.CadastrosGlobais.CapitulosCID";
                public const string GrupoCID = "Cadastros.CadastrosGlobais.GruposCID";
                public const string TipoSanguineo = "Cadastros.CadastrosGlobais.TiposSanguineos";
                public const string ElementoHtml = "Cadastros.CadastrosGlobais.ElementoHtml";
                public const string ElementoHtmlTipo = "Cadastros.CadastrosGlobais.ElementoHtmlTipo";
            }

            public static class CadastrosAssistenciais
            {
                public const string Assistenciais = "Cadastros.CadastrosAssistenciais.Assistenciais";
                public const string Atestados = "Cadastros.CadastrosAssistenciais.Atestados";
                public const string Atestado = "Cadastros.CadastrosAssistenciais.Atestado";
                public const string ModeloAtestado = "Cadastros.CadastrosAssistenciais.ModeloAtestado";
                public const string ModelosAtestados = "Cadastros.CadastrosAssistenciais.ModelosAtestados";
                public const string TipoAtestado = "Cadastros.CadastrosAssistenciais.TipoAtestado";
                public const string TiposAtestados = "Cadastros.CadastrosAssistenciais.TiposAtestados";
                public const string Prescricoes = "Cadastros.CadastrosAssistenciais.Prescricoes";
                public const string Divisoes = "Cadastros.CadastrosAssistenciais.Divisoes";
                public const string TiposRespostas = "Cadastros.CadastrosAssistenciais.TiposRespostas";
                public const string VelocidadesInfusoes = "Cadastros.CadastrosAssistenciais.VelocidadesInfusoes";
                public const string VelocidadeInfusao = "Cadastros.CadastrosAssistenciais.VelocidadeInfusao";
                public const string TiposRespostasConfiguracoes = "Cadastros.CadastrosAssistenciais.TiposRespostasConfiguracoes";
                public const string PrescricaoItem = "Cadastros.CadastrosAssistenciais.PrescricaoItem";
                public const string PrescricoesItens = "Cadastros.CadastrosAssistenciais.PrescricoesItens";
                public const string FormulaEstoque = "Cadastros.CadastrosAssistenciais.FormulaEstoque";
                public const string FormulasEstoques = "Cadastros.CadastrosAssistenciais.FormulasEstoques";
                public const string FormulaEstoqueItem = "Cadastros.CadastrosAssistenciais.FormulaEstoqueItem";
                public const string FormulasEstoquesItens = "Cadastros.CadastrosAssistenciais.FormulasEstoquesItens";
                public const string FormulaFaturamento = "Cadastros.CadastrosAssistenciais.FormulaFaturamento";
                public const string FormulasFaturamentos = "Cadastros.CadastrosAssistenciais.FormulasFaturamentos";
                public const string FormulaExameLaboratorial = "Cadastros.CadastrosAssistenciais.FormulaExameLaboratorial";
                public const string FormulasExamesLaboratoriais = "Cadastros.CadastrosAssistenciais.FormulasExamesLaboratoriais";
                public const string FormulaExameImagem = "Cadastros.CadastrosAssistenciais.FormulaExameImagem";
                public const string FormulasExamesImagens = "Cadastros.CadastrosAssistenciais.FormulasExamesImagens";
                public const string TipoControle = "Cadastros.CadastrosAssistenciais.TipoControle";
                public const string TiposControles = "Cadastros.CadastrosAssistenciais.TiposControles";
                public const string FormasAplicacoes = "Cadastros.CadastrosAssistenciais.FormasAplicacoes";
                public const string FormaAplicacao = "Cadastros.CadastrosAssistenciais.FormaAplicacao";
                public const string Frequencias = "Cadastros.CadastrosAssistenciais.Frequencias";
                public const string Frequencia = "Cadastros.CadastrosAssistenciais.Frequencia";
                public const string PrescricoesStatus = "Cadastros.CadastrosAssistenciais.PrescricoesStatus";
                public const string PrescricaoStatus = "Cadastros.CadastrosAssistenciais.PrescricaoStatus";
                public const string PrescricoesItensStatus = "Cadastros.CadastrosAssistenciais.PrescricoesItensStatus";
                public const string PrescricaoItemStatus = "Cadastros.CadastrosAssistenciais.PrescricaoItemStatus";
            }

            public static class CadastrosSuprimentos
            {
                public const string Produto = "Cadastros.CadastrosSuprimentos.Produto";
                public const string ProdutoPalavraChave = "Cadastros.CadastrosSuprimentos.PalavraChave";
                public const string ProdutoAcaoTerapeutica = "Cadastros.CadastrosSuprimentos.AcaoTerapeutica";
                public const string Grupo = "Cadastros.CadastrosSuprimentos.Grupo";
                public const string ProdutoLaboratorio = "Cadastros.CadastrosSuprimentos.ProdutoLaboratorio";
                public const string ProdutoPortaria = "Cadastros.CadastrosSuprimentos.Portaria";
                public const string ProdutoGrupoTratamento = "Cadastros.CadastrosSuprimentos.GruposTratamento";
                public const string ProdutoLocalizacao = "Cadastros.CadastrosSuprimentos.LocalizacaoProduto";
                public const string ProdutoUnidade = "Cadastros.CadastrosSuprimentos.Unidade";

                public const string Unidade = "Cadastros.CadastrosSuprimentos.Unidade";

                public const string ProdutoCodigoMedicamento = "Cadastros.CadastrosSuprimentos.CodigoMedicamento";
                public const string ProdutoEstoque = "Cadastros.CadastrosSuprimentos.Estoque";
                public const string ProdutoClasse = "Cadastros.CadastrosSuprimentos.Classe";
                public const string ProdutoSubClasse = "Cadastros.CadastrosSuprimentos.SubClasse";
                public const string ProdutoSubstancia = "Cadastros.CadastrosSuprimentos.Substancia";
                public const string ProdutoTipoUnidade = "Cadastros.CadastrosSuprimentos.TipoUnidade";
                public const string ProdutoSubstituicao = "Cadastros.CadastrosSuprimentos.ProdutoSubstituicao";
                public const string Kit = "Cadastros.CadastrosSuprimentos.Kit";
            }

            public static class CadastrosAtendimento
            {
                public const string Atendimento = "Cadastros.Atendimento";
                public const string TiposAtendimento = "Cadastros.Atendimento.TiposAtendimento";
                public const string TipoAtendimento = "Cadastros.Atendimento.TipoAtendimento";
                public const string TipoLocalChamada = "Cadastros.Atendimento.TipoLocalChamada";
                public const string MotivoAlta = "Cadastros.Atendimento.MotivoAlta";
                public const string Leito = "Cadastros.Atendimento.Leito";
                public const string LeitoStatus = "Cadastros.Atendimento.LeitoStatus";
                public const string LeitoCaracteristica = "Cadastros.Atendimento.LeitoCaracteristica";
                public const string LeitoServico = "Cadastros.Atendimento.LeitoServico";
                public const string UnidadeInternacao = "Cadastros.Atendimento.UnidadeInternacao";
                public const string UnidadeInternacaoTipo = "Cadastros.Atendimento.UnidadeInternacaoTipo";
                public const string Guia = "Cadastros.Atendimento.Guia";
                public const string PainelSenha = "Cadastros.Atendimento.PainelSenha";
                public const string Fila = "Cadastros.Atendimento.Fila";
                public const string MovimentoAutomatico = "Cadastros.Atendimento.MovimentoAutomatico";
                public const string ModeloTexto = "Cadastros.Atendimento.ModeloTexto";
            }

            public static class CadastrosFaturamento
            {
                public const string Faturamento = "Cadastros.Faturamento";
                public const string Item = "Cadastros.Faturamento.Item";
                public const string Kit = "Cadastros.Faturamento.Kit";
                public const string Tabela = "Cadastros.Faturamento.Tabela";
                public const string Grupo = "Cadastros.Faturamento.Grupo";
                public const string SubGrupo = "Cadastros.Faturamento.SubGrupo";
                public const string Brasindice = "Cadastros.Faturamento.Brasindice";
                public const string BrasApresentacao = "Cadastros.Faturamento.Apresentacao";
                public const string BrasItem = "Cadastros.Faturamento.Item";
                public const string TabelaPrecoConvenio = "Cadastros.Faturamento.TabelaPrecoConvenio";
                public const string FaturamentoItemAutorizacao = "Cadastros.Atendimento.FaturamentoItemAutorizacao";
                public const string Guia = "Cadastros.Faturamento.Guia";
                public const string Autorizacao = "Cadastros.Faturamento.Autorizacao";
            }

            public static class CadastrosDiagnostico
            {
                public const string Diagnostico = "Cadastros.Diagnostico";
                public const string ModeloLaudo = "Cadastros.Diagnostico.ModeloLaudo";
                public const string LaudoGrupo = "Cadastros.Diagnostico.LaudoGrupo";
                public const string Modalidade = "Cadastros.Diagnostico.Modalidade";
            }

            public static class CadastrosLeito
            {
                public const string Leito = "Cadastros.Leito";
                public const string TiposLeito = "Cadastros.Leito.TiposLeito";
                public const string TipoLeito = "Cadastros.Leito.TipoLeito";
            }

            public static class CadastrosDominioTiss
            {
                public const string DominioTiss = "Cadastros.DominioTiss";
                public const string TipoTabelaDominio = "Cadastros.DominioTiss.TipoTabelaDominio";
                public const string GrupoTipoTabelaDominio = "Cadastros.DominioTiss.GrupoTipoTabelaDominio";
                public const string TabelaDominio = "Cadastros.DominioTiss.TabelaDominio";
                public const string VersaoTiss = "Cadastros.DominioTiss.VersaoTiss";
            }


            public static class CadastrosLaboratorio
            {
                //public const string Equipamento = "Cadastros.CadastrosLaboratorio.Equipamento";
                //public const string Metodo = "Cadastros.CadastrosLaboratorio.Metodo";
                //public const string Tecnico = "Cadastros.CadastrosLaboratorio.Tecnico";
                //public const string Formata = "Cadastros.CadastrosLaboratorio.Formata";
                //public const string ItemResultado = "Cadastros.CadastrosLaboratorio.ItemResultado";
                public const string Tecnico = "Cadastros.CadastrosLaboratorio.Tecnico";
                public const string Material = "Cadastros.CadastrosLaboratorio.Material";
                public const string Metodo = "Cadastros.CadastrosLaboratorio.Metodo";
                public const string Unidade = "Cadastros.CadastrosLaboratorio.Unidade";
                public const string KitExame = "Cadastros.CadastrosLaboratorio.KitExame";
                public const string Setor = "Cadastros.CadastrosLaboratorio.Setor";
                public const string EquipamentoInterfaceamento = "Cadastros.CadastrosLaboratorio.EquipamentoInterfaceamento";
                public const string InformacoesExame = "Cadastros.CadastrosLaboratorio.InformacoesExame";
                public const string TabelaResultado = "Cadastros.CadastrosLaboratorio.TabelaResultado";
                public const string Tabela = "Cadastros.CadastrosLaboratorio.Tabela";
                public const string FormatacaoExame = "Cadastros.CadastrosLaboratorio.FormatacaoExame";
                public const string Exame = "Cadastros.CadastrosLaboratorio.Exame";
                public const string Mapas = "Cadastros.CadastrosLaboratorio.Mapas";
                public const string Laboratorio = "Cadastros.CadastrosLaboratorio.Laboratorio";
                public const string Resultado = "Cadastros.CadastrosLaboratorio.Resultado";
                public const string ResultadoExame = "Cadastros.CadastrosLaboratorio.ResultadoExame";
                public const string ItemResultado = "Cadastros.CadastrosLaboratorio.ItemResultado";
                public const string LaboratorioUnidade = "Cadastros.CadastrosLaboratorio.LaboratorioUnidade";
                public const string Cabecalho = "Cadastros.CadastrosLaboratorio.Cabecalho";
            }


            public static class Desenvolvimento
            {
                public const string ControleProducao = "Desenvolvimento.ControleProducao";
                public const string Evento = "Desenvolvimento.Evento";
            }

            public static class Manutencao
            {
                public const string Consultor = "Manutencao.Consultor";
                public const string ConsultorTabela = "Manutencao.Consultor.Tabela";
                public const string ConsultorTabelaCampo = "Manutencao.Consultor.TabelaCampo";
                public const string MailingTemplates = "Manutencao.MailingTemplates";
                public const string Guias = "Manutencao.Guias";
                public const string BIs = "Manutencao.BIs";
            }

            public static class AgendamentoConsultaMedicoDisponibilidades
            {
                public const string AgendamentoConsultaMedicoDisponibilidade = "Cadastros.Atendimento.AgendamentoConsultaMedicoDisponibilidade";

            }

            public static class Pacientes
            {
                public const string Paciente = "Cadastros.CadastrosGlobais.Pacientes.Paciente";
            }

            public static class Medicos
            {
                public const string Medico = "Cadastros.CadastrosGlobais.Medico.Medico";
            }

            public static class Especialidades
            {
                public const string Especialidade = "Cadastros.CadastrosGlobais.Especialidades.Especialidade";
            }

            public static class Intervalos
            {
                public const string Intervalo = "Cadastros.CadastrosGlobais.Intervalos.Intervalo";
            }

            public static class Laboratorio
            {
                //public const string Cadastro = "Laboratorio.Cadastros";
                public const string Coletas = "Laboratorio.Coleta";
                public const string Resultados = "Laboratorio.Resultados";
                public const string ConfirmacaoResultados = "Laboratorio.Confirmacao.Resultados";
                public const string EvolucaoResultados = "Laboratorio.EvolucaoResultados";
                public const string Painel = "Laboratorio.Painel";
                //public static class Cadastros
                //{
                //    public const string Tecnico = "Laboratorio.Cadastros.Tecnico";
                //    public const string Material = "Laboratorio.Cadastros.Material";
                //    public const string Metodo = "Laboratorio.Cadastros.Metodo";
                //    public const string Unidade = "Laboratorio.Cadastros.Unidade";
                //    public const string KitExames = "Laboratorio.Cadastros.KitExames";
                //    public const string SetorLaboratorio = "Laboratorio.Cadastros.SetorLaboratorio";
                //    public const string EquipamentoInterfaceamento = "Laboratorio.Cadastros.EquipamentoInterfaceamento";
                //    public const string InformacoesExame = "Laboratorio.Cadastros.InformacoesExame";
                //    public const string TabelaResultado = "Laboratorio.Cadastros.TabelaResultado";
                //    public const string FormatacaoExames = "Laboratorio.Cadastros.FormatacaoExames";
                //    public const string Exames = "Laboratorio.Cadastros.Exames";
                //    public const string MapaResultados = "Laboratorio.Cadastros.MapaResultados";
                //}
                public static class Relatorios
                {
                    public const string RelacaoResumidaExames = "Laboratorio.Relatorios.RelacaoResumidaExames";
                    public const string RelacaoDetalhadaExames = "Laboratorio.Relatorios.RelacaoDetalhadaExames";
                }
            }
        }

        public static class Frontend
        {
            public const string Home = "Frontend.Home";
            public const string About = "Frontend.About";
        }

    }
}