(function ($) {
    app.modals.CriarOuEditarGuiaModal = function () {

        var _GuiaService = abp.services.app.guia;
        var _GuiaCampoService = abp.services.app.guiaCampo;
        var _RelacaoGuiaCampoService = abp.services.app.relacaoGuiaCampo;
        var _modalManager;
        var _$GuiaInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$GuiaInformationForm = _modalManager.getModal().find('form[name=GuiaInformationsForm]');
            _$GuiaInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1700px' });
            $('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {

            var isEditMode = $('#is-edit-mode').val();

            if (!_$GuiaInformationForm.valid()) {
                return;
            }

            var tipoGuia = _$GuiaInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);

            // Conjuntos (classes)
            var campos = [];
            var campoSalvo;

            // Campos (classes)
            $('.drop-row').each(function () {

                // Checando se e classe ou propriedade
                if ($(this).attr('data-conjunto') === undefined) {
                    ////console.log('CONJUNTO UNDEFINED');
                } else {
                    ////console.log('NAO EH CONJUNTO');
                    return true;
                }

                var x = $(this).attr('data-x');          
                var y = $(this).attr('data-y');          

                if (x) {
                    x = x.replace(",", ".");
                } else {
                    x = 0;
                }
                if (y) {
                    y = y.replace(",", ".");
                } else {
                    y = 0;
                }

                //console.log('xy: ' + x + y);

                $(this).find('select').selectpicker('refresh');
                var descricao = $(this).find('select option:selected').first().text();
                var isConjunto = $(this).find('input[name=IsConjunto]').val();
                var maxElementos = $(this).find('input[name=MaximoElementos]').val();
                var isSubItem = $(this).attr('data-subitem');
                var subConjuntos = [];

                var novoCampo = {
                    Descricao: descricao,
                    CoordenadaX: x,
                    CoordenadaY: y,
                    IsConjunto: isConjunto,
                    MaximoElementos: maxElementos,
                    IsSubItem: isSubItem,
                    SubConjuntos: subConjuntos
                };

                // Procurando subConjuntos
                $(this).find('.drop-row').each(function () {

                    var x = $(this).attr('data-x');
                    var y = $(this).attr('data-y');

                    if (x) {
                        x = x.replace(",", ".");
                    } else {
                        x = 0;
                    }
                    if (y) {
                        y = y.replace(",", ".");
                    } else {
                        y = 0;
                    }

                    //console.log('xy: ' + x + y);

                    $(this).find('select').first().selectpicker('refresh');
                    var descricao = $(this).find('select option:selected').first().text();
                    var isConjunto = false;
                    var isSubItem = true;

                    var novoSubConjunto = {
                        Descricao: descricao,
                        CoordenadaX: x,
                        CoordenadaY: y,
                        IsConjunto: isConjunto,
                        IsSubItem: isSubItem,
                        ConjuntoId: 0
                    };
                    novoCampo.SubConjuntos.push(novoSubConjunto);
                });

                campos.push(novoCampo);
            });

            // Direto no service
            var camposString = JSON.stringify(campos);
            //console.log('campos:');
            //console.log(camposString);
            tipoGuia.camposJson = camposString;

            _GuiaService.criarOuEditar(tipoGuia)
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
    };
})(jQuery);