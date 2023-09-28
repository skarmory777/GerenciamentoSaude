(function ($) {
    app.modals.CriarOuEditarLeitoModal = function () {

        var _LeitoService = abp.services.app.leito;

        var _modalManager;
        var _$LeitoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$LeitoInformationForm = _modalManager.getModal().find('form[name=LeitoInformationsForm]');
            //_$LeitoInformationForm = _modalManager.getModal().find('form[name=LeitoInformationsForm]');
            _$LeitoInformationForm.validate();

            $('.modal-dialog').css({ 'width': '70%', 'max-width': '600px' });
            $('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
            
            if (!_$LeitoInformationForm.valid()) {
                //console.log("aqui");
                return;
            }

            //console.log('salvando');

            var leito = _$LeitoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _LeitoService.criarOuEditar(leito)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarLeitoModalSaved');
                     
                   // falta dar reload na tabela

                     function getLeitos(reload) {
                         if (reload) {
                             $('#OuMembersTable').jtable('reload');
                         }
                         else {
                             $('#OuMembersTableTable').jtable('load', {
                                 filtro: $('#LeitosFilter').val()
                             });
                         }
                     }
                     
                     getLeitos(true);

                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        

    };
})(jQuery);