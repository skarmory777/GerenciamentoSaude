(function () {

    $(function () {
        carregarDropDowns();
        $('#valor').mask('000.000.000,00', { reverse: true });
        $('.data-movimento').daterangepicker({
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
        });
    });

    function carregarDropDowns() {
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectContaCorrente', "/api/services/app/ContaCorrente/ListarDropdown");
        selectSW('.selectMeioPagamento', "/api/services/app/MeioPagamento/ListarDropdown");
    }

    $("#origemEmpresaId").change(function () {
        if ($("#origemEmpresaId").select2('data') != null && $("#origemEmpresaId").select2('data').length) {
            let optionId = $("#origemEmpresaId").val();
            let optionText = $("#origemEmpresaId").select2('data')[0].text;

            if ($('#destinoEmpresaId').find("option[value='" + optionId + "']").length) {
                $('#destinoEmpresaId').val(optionId).trigger('change');
            } else {
                // Create a DOM Option and pre-select by default
                var newOption = new Option(optionText, optionId, true, true);
                // Append it to the select
                $('#destinoEmpresaId').append(newOption).trigger('change');
            }
        }
    })

    $("#meioPagamentoId").change(function () {
        if ($("#meioPagamentoId").select2('data') != null && $("#meioPagamentoId").select2('data').length) {
            let optionId = $("#meioPagamentoId").val();
            let optionText = $("#meioPagamentoId").select2('data')[0].text;

            if ($('#destinoMeioPagamentoId').find("option[value='" + optionId + "']").length) {
                $('#destinoMeioPagamentoId').val(optionId).trigger('change');
            } else {
                // Create a DOM Option and pre-select by default
                var newOption = new Option(optionText, optionId, true, true);
                // Append it to the select
                $('#destinoMeioPagamentoId').append(newOption).trigger('change');
            }
        }
    })

    $('#origemNumero').change(function () {
        $('#destinoNumero').val($('#origemNumero').val());
    })

    $('#valor').change(function () {
        $('#destinoValor').val($('#valor').val());
    })

    $('#origemObservacao').change(function () {
        $('#destinoObservacao').val($('#origemObservacao').val());
    })


    $('.close-button').on('click', function () {
        location.href = '/mpa/ControleBancario';
    });

    $('.save-button').on('click', function (e) {
        var _controleBancarioService = abp.services.app.controleBancario;
        e.preventDefault();

        var transferenciaForm = $('form[name=TransferenciaForm]');
        transferenciaForm.validate();

        if (!transferenciaForm.valid()) {
            return;
        }

        if ($("#origemContaCorrenteId").val() == $("#destinoContaCorrenteId").val()) {
            alert("A conta de origem n\u00e3o pode ser a mesma do destino");
            return;
        }

        $('.save-button').prop("disabled", true);

        var request = {
            OrigemEmpresaId: $("#origemEmpresaId").val(),
            MeioPagamentoId: $("#meioPagamentoId").val(),
            OrigemContaCorrenteId: $("#origemContaCorrenteId").val(),
            OrigemDataMovimento: $("#origemDataMovimento").val(),
            OrigemNumero: $("#origemNumero").val(),
            OrigemObservacao: $("#origemObservacao").val(),
            Valor: retirarMascara($("#valor").val()),
            DestinoEmpresaId: $("#destinoEmpresaId").val(),
            DestinoContaCorrenteId: $("#destinoContaCorrenteId").val(),
            DestinoDataMovimento: $("#destinoDataMovimento").val(),
            DestinoNumero: $("#destinoNumero").val(),
            DestinoObservacao: $("#destinoObservacao").val()
        };

        _controleBancarioService.criarTransferencia(request)
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
})();