(function ($) {
    app.modals.CoordenadaModal = function () {

        var _GuiaService = abp.services.app.guia;
        var _modalManager;
        var _$GuiaInformationForm = null;

        var isEditMode = $('#is-edit-mode').val(); // deveria se chamar guiaId
        var guia = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$GuiaInformationForm = _modalManager.getModal().find('form[name=GuiaInformationsForm]');
            _$GuiaInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1700px' });
            $('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };

            _GuiaService.obter(isEditMode)
             .done(function (guiaObtida) {
                 guia = guiaObtida;
                 //console.log(JSON.stringify(guia));
             })
            .always(function () {
            });
        };

        this.save = function () {

            if (!_$GuiaInformationForm.valid()) {
                return;
            }

            // Modelo Final enfim...
            var editaveis = [];
            $('.editavel').each(function () {
                var descricao = $(this).attr('data');
                var este = $(this).get(0);
                var imagem = $('#assinatura-digital-img').get(0); // corrigir id da imagem
                var coordenadaX = posicaoOffset(este).left - posicaoOffset(imagem).left;
                var coordenadaY = posicaoOffset(este).top - posicaoOffset(imagem).top;

                var novo = {
                    Descricao: descricao,
                    CoordenadaX: coordenadaX,
                    CoordenadaY: coordenadaY
                };

                editaveis.push(novo);
                //console.log('editaveis: ' + JSON.stringify(editaveis));
            });

            var editaveisString = JSON.stringify(editaveis);

            _GuiaService.atualizarCoordenadas(guia, editaveisString)
             .done(function (guiaSalva) {

                 abp.notify.info(app.localize('SavedSuccessfully'));
                 window.arquivo = undefined;
                 _modalManager.close();
                 abp.event.trigger('app.EditarGuiaModalSaved');
             })
            .always(function () {
                _modalManager.setBusy(false);
            });

        };

        function posicaoOffset(elemento) {
            var rect = elemento.getBoundingClientRect(),
            scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
            scrollTop = window.pageYOffset || document.documentElement.scrollTop;
            return { top: rect.top + scrollTop, left: rect.left + scrollLeft }
        }
    };
})(jQuery);