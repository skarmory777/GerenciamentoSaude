(function ($) {

    app.modals.CriarOuEditarControleProducaoModal = function () {

        var _controleProducoesService = abp.services.app.controleProducao;

        var _modalManager;
        var _$controleProducaoInformationForm = null;

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ControleProducoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Desenvolvimento/ControleProducoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarControleProducaoModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$controleProducaoInformationForm = _modalManager.getModal().find('form[name=ControleProducaoInformationsForm]');
            _$controleProducaoInformationForm.validate();

            atualizarTabela();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
            if (!_$controleProducaoInformationForm.valid()) {
                abp.notify.error('invalid form');
                return;
            }

            var controleProducao = _$controleProducaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            _controleProducoesService.criarOuEditar(controleProducao)
                 .done(function (data) {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarControleProducaoModalSaved');
                     if (!editMode) {
                         _createOrEditModal.open({ id: data.id });
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        //$('input[name="Nascimento"]').daterangepicker({
        //    "singleDatePicker": true,
        //    "showDropdowns": true,
        //    autoUpdateInput: false,
        //    maxDate: new Date(),
        //    changeYear: true,
        //    yearRange: 'c-10:c+10',
        //    showOn: "both",
        //    "locale": {
        //        "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
        //        "separator": " - ",
        //        "applyLabel": "Apply",
        //        "cancelLabel": "Cancel",
        //        "fromLabel": "From",
        //        "toLabel": "To",
        //        "customRangeLabel": "Custom",
        //        "daysOfWeek": [
        //            app.localize('Dom'),
        //            app.localize('Seg'),
        //            app.localize('Ter'),
        //            app.localize('Qua'),
        //            app.localize('Qui'),
        //            app.localize('Sex'),
        //            app.localize('Sab')
        //        ],
        //        "monthNames": [
        //            app.localize("Jan"),
        //            app.localize("Fev"),
        //            app.localize("Mar"),
        //            app.localize("Abr"),
        //            app.localize("Mai"),
        //            app.localize("Jun"),
        //            app.localize("Jul"),
        //            app.localize("Ago"),
        //            app.localize("Set"),
        //            app.localize("Out"),
        //            app.localize("Nov"),
        //            app.localize("Dez"),
        //        ],
        //        "firstDay": 0
        //    }
        //},
        //function (selDate) {
        //    $('input[name="Nascimento"]').val(selDate.format('L')).addClass('form-control edited');
        //    obterIdade(selDate);
        //});

        function atualizarTabela() {
      //      $('#ControleProducaoConveniosTable').load('/ControleProducoes/_ControleProducaoConvenios?id=' + $('#id').val());
        }

        // Date pickers
        $('input[name="DataInicial"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date(),
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
            $('input[name="DataInicial"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('input[name="DataFinal"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date(),
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
            $('input[name="DataFinal"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('input[name="DataAprovacao"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date(),
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
            $('input[name="DataAprovacao"]').val(selDate.format('L')).addClass('form-control edited');
        });

     //   $("#status-input").after('%');

    };
})(jQuery);