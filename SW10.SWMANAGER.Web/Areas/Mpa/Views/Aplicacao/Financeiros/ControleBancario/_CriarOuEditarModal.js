(function () {

    $(function () {
        var _controleBancarioService = abp.services.app.controleBancario;
        carregarDropDowns();
        $('#valor').mask('000.000.000,00', { reverse: true });
        $('#dataMovimento').mask('00/00/0000', { reverse: true });
        $('#dataCompensado').mask('00/00/0000', { reverse: true });
        $('#dataConsolidado').mask('00/00/0000', { reverse: true });

        $("#dataMovimento").val(moment().format('DD/MM/YYYY'));

        var _anexosModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Anexo/OpenModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Common/Modals/Anexo/_AnexoModal.js',
            modalId: 'anexoModalId'
        });

        $('#btnAnexosDocumento').on('click', function (e) {
            e.preventDefault();
            _anexosModal.open({ anexoListaId: $("#anexoListaIdDocumento").val(), origemAnexoId: $("#id").val(), origemAnexoTabela: 'findocumento' });
        });
        //Add calendar into date inputs
        $('#dataMovimento').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: true,
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
            function (selDate) {
                $('#dataMovimento').val(selDate.format('L')).addClass('form-control edited');
            });
        $('#dataCompensado').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: true,
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            locale: {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
            function (selDate) {
                $('#dataCompensado').val(selDate.format('L')).addClass('form-control edited');
            });
        $('#dataConsolidado').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
            function (selDate) {
                $('#dataConsolidado').val(selDate.format('L')).addClass('form-control edited');
            });


        function carregarDropDowns() {
            selectSW('.selectPessoa', "/api/services/app/sisPessoa/ListarDropdownPessoa");
            selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
            selectSW('.selectContaCorrente', "/api/services/app/ContaCorrente/ListarDropdown");
            selectSW('.selectMeioPagamento', "/api/services/app/MeioPagamento/ListarDropdown");
            selectSW('.selectContaAdministrativaEmpresa', "/api/services/app/ContaAdministrativa/ListarContaAdministrivaPorEmpresaDropdown", $('#empresaRateioId'));
            selectSW('.selectCentroCusto', "/api/services/app/CentroCusto/ListarDropdownCodigoCentroCustoPorContaAdministrativa", $('#contaAdministrativaId'));
        }

        $("#empresaId").change(function () {
            if ($("#empresaId").select2('data') != null && $("#empresaId").select2('data').length) {
                let optionId = $("#empresaId").val();
                let optionText = $("#empresaId").select2('data')[0].text;

                if ($('#empresaRateioId').find("option[value='" + optionId + "']").length) {
                    $('#empresaRateioId').val(optionId).trigger('change');
                } else {
                    // Create a DOM Option and pre-select by default
                    var newOption = new Option(optionText, optionId, true, true);
                    // Append it to the select
                    $('#empresaRateioId').append(newOption).trigger('change');
                }
            }
        });

        $("#valor").change(function () {
            $('#valorRateio').val($('#valor').val());
        });

        $("input[name=tipoLancamento]").change(function () {
            $("#centroCustoId").empty();
            $("#contaAdministrativaId").empty();

            if ($("input[name=tipoLancamento]:checked").val() == "True") {
                selectSW('.selectContaAdministrativaEmpresa', "/api/services/app/ContaAdministrativa/ListarContaAdministrivaPorEmpresaDropdown", $('#empresaRateioId'));
            }
            else {
                console.log("false")
                selectSW('.selectContaAdministrativaEmpresa', "/api/services/app/ContaAdministrativa/ListarContaAdministrivaDespesaDropdown", $('#empresaRateioId'));
            }
        })

        $('.close-button').on('click', function () {
            location.href = '/mpa/ControleBancario';
        });

        $('.save-button').on('click', function (e) {
            e.preventDefault();

            var controleBancarioForm = $('form[name=controleBancarioForm]');
            controleBancarioForm.validate();

            if (!controleBancarioForm.valid()) {
                return;
            }

            $('.save-button').prop("disabled", true);

            var request = {
                IsCredito: $("input[name=tipoLancamento]:checked").val(),
                PessoaId: $("#pessoaId").val(),
                EmpresaId: $("#empresaId").val(),
                MeioPagamentoId: $("#meioPagamentoId").val(),
                Numero: $("#numero").val(),
                DataMovimento: $("#dataMovimento").val(),
                DataCompensado: $("#dataCompensado").val(),
                DataConsolidado: $("#dataConsolidado").val(),
                ValorQuitacao: retirarMascara($("#valor").val()),
                ContaCorrenteId: $("#contaCorrenteId").val(),
                TipoQuitacaoId: 2,
                Observacao: $("#ObservacaoQuitacao").val(),
                RateioJson: $('#rateioJson').val()
            };

            _controleBancarioService.criarLancamento(request)
                .done(function (data) {
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        abp.notify.info(app.localize('SavedSuccessfully'));

                        location.href = '/mpa/ControleBancario';
                    }
                })
                .always(function () {
                    $('.save-button').prop("disabled", false);
                });


        });
    });
})();