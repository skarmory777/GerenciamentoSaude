(function ($) {
    app.modals.CriarOuEditarEstoqueModal = function () {

        var _estoqueService = abp.services.app.estoque;

        var _modalManager;
        var _$EstoqueInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$EstoqueInformationForm = _modalManager.getModal().find('form[name=EstoqueInformationsForm]');
            _$EstoqueInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '1000px' });

            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };

            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {

            if (!_$EstoqueInformationForm.valid()) {
                return;
            }

            var estoque = _$EstoqueInformationForm.serializeFormToObject();
            var objs = SMWETagsMultiSelecionados(true);
            estoque.estoquesGrupo = objs;

            //-------------------------------------------------------

            _modalManager.setBusy(true);
            _estoqueService.criarOuEditar(estoque)
                 .done(function () {

                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("input[name=Descricao]").val("");

                         $('#swmulti').multiSelect('deselect_all');

                         $("input[name=Descricao]").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarEstoqueModalSaved');

                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

    };
})(jQuery);