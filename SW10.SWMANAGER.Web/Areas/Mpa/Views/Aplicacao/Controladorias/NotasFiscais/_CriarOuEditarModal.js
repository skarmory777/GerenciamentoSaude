(function ($) {
    app.modals.CriarOuEditarNotaFiscalModal = function () {

        var _NotasFiscaisService = abp.services.app.notaFiscal;

        var _modalManager;
        var _$NotaFiscalInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$NotaFiscalInformationForm = _modalManager.getModal().find('form[name=NotaFiscalInformationsForm]');
            _$NotaFiscalInformationForm.validate();
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $(".accordion").accordion({
                active: false,
                collapsible: true,
                heightStyle: "content",
                navigation: true,
                autoHeight: false
                //event: false
            });
        };

        $('#btn-confirmar-nota').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: {
                    chaveAcesso: $('#chave-acesso').val()
                },
                dataType: 'json',
                contentType: 'application/json',
                dataContext: 'application/json;charset=utf-8',
                url: '/mpa/NotasFiscais/ConfirmarNota',
                async: true,
                type: 'GET',
                cache: false,
                success: function (data) {
                    var result = JSON.stringify(data.result);
                    if (result.indexOf('ERRO') !== -1) {
                        abp.notify.error(app.localize("Erro") + "<br />" + data.result.replace('ERRO:', ''));
                        return false;
                    }
                    else {
                        abp.notify.success(app.localize("NotaConfirmada"));
                    }
                },
                beforeSend: function () {
                    $('#btn-confirmar-nota').buttonBusy(true)
                },
                complete: function () {
                    $('#btn-confirmar-nota').buttonBusy(false)

                }
            });
        })
        //this.save = function () {
        //    if (!_$NotaFiscalInformationForm.valid()) {
        //        return;
        //    }

        //    var notaFiscal = _$NotaFiscalInformationForm.serializeFormToObject();

        //    _modalManager.setBusy(true);
        //    _NotasFiscaisService.criarOuEditar(notaFiscal)
        //         .done(function () {
        //             abp.notify.info(app.localize('SavedSuccessfully'));
        //             _modalManager.close();
        //             abp.event.trigger('app.CriarOuEditarNotaFiscalModalSaved');
        //             //location.reload();//seguindo o projeto pronto
        //         })
        //        .always(function () {
        //            _modalManager.setBusy(false);
        //        });
        //};
    };
})(jQuery);