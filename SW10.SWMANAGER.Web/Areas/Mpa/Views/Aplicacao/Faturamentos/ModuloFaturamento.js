var ModuloFaturamento = (function () {


    var contaService = abp.services.app.conta;
    // Permissoes
    var permissoes = {
        create: abp.auth.hasPermission('Pages.Tenant.Faturamento.Contas.Create'),
        edit: abp.auth.hasPermission('Pages.Tenant.Faturamento.Contas.Edit'),
        'delete': abp.auth.hasPermission('Pages.Tenant.Faturamento.Contas.Delete')
    };

    // Modais
    var modalCrudContasMedicas = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/ContasMedicas/CriarOuEditarModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/_CriarOuEditarModal.js',
        modalClass: 'CriarOuEditarContaMedicaModal'
    });

    var modalConferenciaConta = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/EntregaContas/ConferenciaModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/EntregaContas/conferencia_modal.js',
        modalClass: 'ConferenciaModal'
    });

    // Objetos
    var form = {};

    var filtroJTable = {
        filtro: $('#ContasMedicasTableFilter').val(),
        empresaId: $('#comboEmpresa option:selected').val(),
        convenioId: $('#comboConvenio option:selected').val(),
        pacienteId: $('#comboPaciente option:selected').val(),
        medicoId: $('#comboMedico option:selected').val(),
        guiaId: $('#filtroTipoGuia option:selected').val(),
        IsEmergencia: $('#tipo-atendimento-amb').is(':checked'),
        IsInternacao: $('#tipo-atendimento-int').is(':checked'),

        get() {
            this.filtro = $('#ContasMedicasTableFilter').val();
            this.empresaId = $('#comboEmpresa option:selected').val();
            this.convenioId = $('#comboConvenio option:selected').val();
            this.pacienteId = $('#comboPaciente option:selected').val();
            this.medicoId = $('#comboMedico option:selected').val();
            this.guiaId = $('#filtroTipoGuia option:selected').val();
            this.IsEmergencia = $('#tipo-atendimento-amb').is(':checked');
            this.IsInternacao = $('#tipo-atendimento-int').is(':checked');

            return this;
        }
    };

    // Filtros jtable
    var jtableEntregaContasFiltro = {

        Filtro: $('#ContasMedicasTableFilter').val(),
        EmpresaId: $('#comboEmpresa').val(),
        PacienteId: $('#comboPaciente').val(),
        ConvenioId: $('#comboConvenio').val(),
        MedicoId: $('#comboMedico').val(),
        NumeroGuia: $('#num-guia').val(),
        IsEmergencia: $('#is-ambulatorio-ent').is('checked'),
        IsInternacao: $('#is-internacao-ent').is('checked'),
        ApenasConferidas: $('#filtro-conferidas-entregar').swChkValor(),
        UsuarioId: $('#combo-entregar-usuario').val(),
        IsEmergencia: $('#is-ambulatorioemergencia-ent').swChkValor(),
        IsInternacao: $('#is-internacao-ent').swChkValor(),

        get() {
            this.Filtro = $('#ContasMedicasTableFilter').val();
            this.EmpresaId = $('#comboEmpresa').val();
            this.PacienteId = $('#comboPaciente').val();
            this.ConvenioId = $('#comboConvenio').val();
            this.MedicoId = $('#comboMedico').val();
            this.NumeroGuia = $('#num-guia').val();
            this.IsEmergencia = $('#is-ambulatorio-ent').swChkValor();
            this.IsInternacao = $('#is-internacao-ent').is('checked');
            this.ApenasConferidas = $('#filtro-conferidas-entregar').swChkValor();
            this.UsuarioId = $('#combo-entregar-usuario').val();
            this.IsEmergencia = $('#is-ambulatorioemergencia-ent').swChkValor();
            this.IsInternacao = $('#is-internacao-ent').swChkValor();

            return this;
        }
    };

    var jtableConferenciaContasFiltro = {

        Filtro: $('#ConfContasMedicasTableFilter').val(),
        EmpresaId: $('#comboEmpresaConf').val(),
        PacienteId: $('#comboPacienteConf').val(),
        ConvenioId: $('#comboConvenioConf').val(),
        MedicoId: $('#comboMedicoConf').val(),
        NumeroGuia: $('#num-guia-conf').val(),
        IsEmergencia: $('#is-ambulatorioemergencia-conf').swChkValor(),
        IsInternacao: $('#is-internacao-conf').swChkValor(),
        Periodo: $('#drxdateRangeConf').val(),
        IgnoraData: $('#ignora-periodo-conf').swChkValor(),

        get() {
            this.Filtro = $('#ConfContasMedicasTableFilter').val();
            this.EmpresaId = $('#comboEmpresaConf').val();
            this.PacienteId = $('#comboPacienteConf').val();
            this.ConvenioId = $('#comboConvenioConf').val();
            this.MedicoId = $('#comboMedicoConf').val();
            this.NumeroGuia = $('#num-guia-conf').val();
            this.IsEmergencia = $('#is-ambulatorioemergencia-conf').swChkValor();
            this.IsInternacao = $('#is-internacao-conf').swChkValor();
            this.Periodo = $('#drxdateRangeConf').val();
            this.IgnoraData = $('#ignora-periodo-conf').swChkValor();

            return this;
        }
    };

    var jtableGerarLotesFiltro = {

        Filtro: $('#ContasMedicasTableFilterGerarLotes').val(),
        EmpresaId: $('#comboEmpresa-gerar-lotes').val(),
        PacienteId: $('#comboPaciente-gerar-lotes').val(),
        ConvenioId: $('#comboConveniogerar-lotes').val(),
        MedicoId: $('#comboMedico-gerar-lotes').val(),
        NumeroGuia: $('#num-guia-gerar-lotes').val(),
        IsEmergencia: $('#is-ambulatorio-ger').is('checked'),
        IsInternacao: $('#is-internacao-ger').is('checked'),
        TipoGuiaId: $('#filtroTipoGuia-gerar-lotes').val(),

        get() {
            this.Filtro = $('#ContasMedicasTableFilterGerarLotes').val();
            this.EmpresaId = $('#comboEmpresa-gerar-lotes').val();
            this.PacienteId = $('#comboPaciente-gerar-lotes').val();
            this.ConvenioId = $('#comboConvenio-gerar-lotes').val();
            this.MedicoId = $('#comboMedico-gerar-lotes').val();
            this.NumeroGuia = $('#num-guia-gerar-lotes').val();
            this.IsEmergencia = $('#is-ambulatorio-ger').swChkValor();
            this.IsInternacao = $('#is-internacao-ger').swChkValor();
            this.TipoGuiaId = $('#filtroTipoGuia-gerar-lotes').val();

            return this;
        }
    };

    var jtableLotesGeradosFiltro = {

        Filtro: $('#ContasMedicasTableFilterLotesGerados').val(),
        EmpresaId: $('#comboEmpresa-lotes-gerados').val(),
        PacienteId: $('#comboPaciente-lotes-gerados').val(),
        ConvenioId: $('#comboConvenio-lotes-gerados').val(),
        MedicoId: $('#comboMedico-lotes-gerados').val(),
        NumeroGuia: $('#num-guia-lotes-gerados').val(),
        IsEmergencia: $('#is-ambulatorio-lot').is('checked'),
        IsInternacao: $('#is-internacao-lot').is('checked'),

        get() {
            this.Filtro = $('#ContasMedicasTableFilterLotesGerados').val();
            this.EmpresaId = $('#comboEmpresa-lotes-gerados').val();
            this.PacienteId = $('#comboPaciente-lotes-gerados').val();
            this.ConvenioId = $('#comboConvenio-lotes-gerados').val();
            this.MedicoId = $('#comboMedico-lotes-gerados').val();
            this.NumeroGuia = $('#num-guia-lotes-gerados').val();
            this.IsEmergencia = $('#is-ambulatorio-lot').swChkValor();
            this.IsInternacao = $('#is-internacao-lot').swChkValor();

            return this;
        }
    };

    // Metodos
    var jtPopular = function (jtable, reload) {
        SmweSavior.jtPopular(jtable, filtroJTable.get(), reload)
    };

    var gerarForm = function (_form) {
        form = _form;
        form.campos = form.find('input, select');
    };

    var resetarForm = function () {
        form.campos.each(function () {
            var name = $(this).attr('name');
            if (name != "AtendimentoId") {
                $(this).swSetCampo();
            }
        });

        // campos problematicos
        $('#comboGuia').empty().trigger('change');
        $('#conta-id').val('');
    };
    // Fim de ResetarForm()

    var setarForm = function (conta) {
        var atd = $('#atendimento-id');

        if (atd.length || atd.length != 0)
            atd.swSetCampo(conta.atendimentoId);

        $('#inp-empresa').val(conta.empresaNome);
        $('#inp-data-atd').val(moment(conta.dataIncio).format('L'));
        $('#conta-id').val(conta.id);
        $('#paciente-id').val(conta.pacienteId);
        $('#empresa-id').val(conta.empresaId);

        if (!form.campos)
            return;

        form.campos.each(function () {
            var name = $(this).attr('name');
            var nomeServico = $(this).attr('data-servico');
            var servico = abp.services.app[nomeServico];

            if (name && name != "Empresa" && name != "DataAtendimento" && name != "AtendimentoId") {
               
                if (name == "FatGuiaId") {

                    var prop = name.charAt(0).toLowerCase() + name.slice(1);
                    var valor = conta[prop];
                    if (valor != null) {
                        SmweSavior.setCombo($('#comboGuia'), valor, abp.services.app.faturamentoGuia);
                    }
                } else {
                    var prop = name.charAt(0).toLowerCase() + name.slice(1);
                    var valor = conta[prop];
                   // $(this).swSetCampo(valor, servico);
                }
            }
        });

        $('#ContaItensTable').jtable('load', {
            filtro: $('#conta-id').val(),
            CalculoContaItemInput: {
                Conta: {
                    EmpresaId: conta.empresaId,
                    ConvenioId: conta.convenioId,
                    PlanoId: conta.planoId,
                },
            }
        });
        
        $('#KitsTable').jtable('load', {
            filtro: $('#conta-id').val()
        });


        selecionarRegistroSelect2('comboTipoLeito', conta.tipoLeitoId, conta.tipoLeitoDescricao);
        selecionarRegistroSelect2('cbo-conta-convenio', conta.convenioId, conta.convenioNome);
        selecionarRegistroSelect2('cbo-conta-plano', conta.planoId, conta.planoNome);
        selecionarRegistroSelect2('cbo-conta-medico', conta.medicoId, conta.medicoNome);

        $('input[name="DataIncio"]').val(moment(conta.dataIncio).format('L'));

        if (conta.dataFim != '' && conta.dataFim != null) {

            $('input[name="DataFim"]').val(moment(conta.dataFim).format('L'));
        }
        else
        {
            $('input[name="DataFim"]').val('');
        }

        if (conta.validadeCarteira != '' && conta.validadeCarteira != null) {

            $('input[name="ValidadeCarteira"]').val(moment(conta.validadeCarteira).format('L'));
        }

        if (conta.dataAutorizacao != '' && conta.dataAutorizacao != null) {

            $('input[name="DataAutorizacao"]').val(moment(conta.dataAutorizacao).format('L'));
        }


        $('#titular').val(conta.titular);
        $('#codDependente').val(conta.codDependente);
        $('#guiaOperadora').val(conta.guiaOperadora);
        $('#senhaAutorizacao').val(conta.senhaAutorizacao);
        

        contaService.obterValorContaRegistrado(conta.id)
                     .done(function (valorTaotal) {

                         $('#conta-total').val(formatarValor(valorTaotal));
                     });


    };
    // Fim de SetarForm()

    return {
        // Objetos
        permissoes: permissoes,
        // Modais
        modalCrudContasMedicas: modalCrudContasMedicas,
        modalConferenciaConta: modalConferenciaConta,
        // Filtros jtable
        jtableEntregaContasFiltro: jtableEntregaContasFiltro,
        jtableConferenciaContasFiltro: jtableConferenciaContasFiltro,
        jtableGerarLotesFiltro: jtableGerarLotesFiltro,
        jtableLotesGeradosFiltro: jtableLotesGeradosFiltro,
        // Metodos
        gerarForm: gerarForm,
        setarForm: setarForm,
        resetarForm: resetarForm,
        jtPopular: jtPopular
    };
    // Fim de 'return'

})();