(function ($) {
    app.modals.CriarOuEditarContaCorrente = function () {

        let _args = null;

        $(document).ready(function () {
            CamposRequeridos();
            $('#limiteCredito').mask('000.000.000,00', { reverse: true });
        });

        var _contaCorrenteService = abp.services.app.contaCorrente;

        var _modalManager;
        var _$tipoContaInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            
            _args = modalManager.getArgs();
            _modalManager = modalManager;
            App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
           
            $('input[name="DataAbertura"]').val(moment().format('L'));

            if (_args.id != undefined) {
                _contaCorrenteService.obter(_args.id)
                 .done(function (data) {                     
                     //$('#contaCorrenteId').val(data.id);
                     //$('#codigo').val(data.codigo);
                     //$('#descricao').val(data.descricao);
                     //$('#agencia').val(data.agencia);
                     //$('input[name="DataAbertura"]').val(moment(data.dataAbertura).format('L'));
                     //$('#nomeGerente').val(data.nomeGerente);
                     //$('#limiteCredito').val(data.limiteCredito);                     
                     //$('#observacao').val(data.observacao);
                       $('#isContaNaoOperacional')[0].checked = data.isContaNaoOperacional;
                     //$('.selectEmpresa option:selected').val(data.empresaId);
                     //$('.selectBanco option:selected').val(data.bancoId);
                     //$('.selectAgencia option:selected').val(data.agenciaId);
                     //$('.selectTipoContaCorrente option:selected').val(data.tipoContaCorrenteId);
                 });
            }

            _$tipoContaInformationsForm = _modalManager.getModal().find('form[name=contaCorrenteInformationsForm]');
            _$tipoContaInformationsForm.validate();
        };

        this.save = function () {

            if (!_$tipoContaInformationsForm.valid()) {
                return;
            }
            
            var tipoConta = _$tipoContaInformationsForm.serializeFormToObject();
            tipoConta.IsContaNaoOperacional = $('#isContaNaoOperacional')[0].checked;
            tipoConta.LimiteCredito = retirarMascara($('#limiteCredito').val());

            _modalManager.setBusy(true);

            App.startPageLoading({ animate: true });document.querySelector('.loadingCommon').style.display = null;
            _contaCorrenteService.criarOuEditar(tipoConta)
                 .done(function (data) {
                     App.stopPageLoading();document.querySelector('.loadingCommon').style.display = 'none';
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {
                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                     }
                 })
                .always(function () {
                    App.stopPageLoading();document.querySelector('.loadingCommon').style.display = 'none';
                    _modalManager.setBusy(false);
                });
        };       

        //$('input[name="DataAbertura"]')
        //   .on('input', function () {
        //      var date = $(this).val();              
        //   })
        //   .on('keyup', function () {           
        //   })
        //   .daterangepicker({
        //       "singleDatePicker": true,
        //       "showDropdowns": true,
        //       autoUpdateInput: false,
        //       changeYear: true,
        //       minDate: moment().add('year', -5),
        //       maxDate: moment(),
        //       showOn: "both",
        //       "locale": {
        //           "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
        //           "separator": " - ",
        //           "applyLabel": "Apply",
        //           "cancelLabel": "Cancel",
        //           "fromLabel": "From",
        //           "toLabel": "To",
        //           "customRangeLabel": "Custom",
        //           "daysOfWeek": [
        //               app.localize('Dom'),
        //               app.localize('Seg'),
        //               app.localize('Ter'),
        //               app.localize('Qua'),
        //               app.localize('Qui'),
        //               app.localize('Sex'),
        //               app.localize('Sab')
        //           ],
        //           "monthNames": [
        //               app.localize("Jan"),
        //               app.localize("Fev"),
        //               app.localize("Mar"),
        //               app.localize("Abr"),
        //               app.localize("Mai"),
        //               app.localize("Jun"),
        //               app.localize("Jul"),
        //               app.localize("Ago"),
        //               app.localize("Set"),
        //               app.localize("Out"),
        //               app.localize("Nov"),
        //               app.localize("Dez"),
        //           ],
        //           "firstDay": 0
        //       }
        //   },
        //   function (selDate) {
        //       $('input[name="DataAbertura"]').val(moment(selDate).format('L')).addClass('form-control edited');
        //   });

        $('input[name="DataAbertura"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
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
            $('input[name="DataAbertura"]').val(selDate.format('L')).addClass('form-control edited');
        });

        function retirarMascara(valor) {

            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');
            valor = valor.replace(',', '.');
            return valor;
        }

        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdown");        
        selectSW('.selectTipoContaCorrente', "/api/services/app/tipoContaCorrente/ListarDropdown");

        selectSW('.selectBanco', "/api/services/app/bancoAgencias/ListarDropdownBanco");
        $('#bancoId').change(function (e) {
            e.preventDefault();
            document.querySelector('#agenciaId').value = '';
            selectSW('.selectAgencia', "/api/services/app/bancoAgencias/ListarDropdownAgencia", $('#bancoId'));
        });

    };
})(jQuery);