﻿(function ($) {
    app.modals.CriarOuEditarElementoHtmlModal = function () {

        var _elementosHtmlService = abp.services.app.elementoHtml;

        var _modalManager;
        var _$ElementoHtmlInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ElementoHtmlInformationForm = _modalManager.getModal().find('form[name=ElementoHtmlInformationsForm]');
            _$ElementoHtmlInformationForm.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
        };

        this.save = function () {
            if (!_$ElementoHtmlInformationForm.valid()) {
                return;
            }

            var elementoHtml = _$ElementoHtmlInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _elementosHtmlService.criarOuEditar(elementoHtml)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarElementoHtmlModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        $("#elemento-html-tipo-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/elementoHtmlTipo/listarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    //   //console.log('data: ', params, (params.page == undefined));
                    if (params.page == undefined)
                        params.page = '1';
                    //   //console.log('data: ', params);
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0
        });

    };
})(jQuery);