(function ($) {
    app.modals.CriarOuEditarModeloLaudoModal = function () {

        var _ModelosModeloLaudosService = abp.services.app.modeloLaudo;

        var _modalManager;
        var _$ModeloLaudoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ModeloLaudoInformationForm = _modalManager.getModal().find('form[name=ModeloLaudoInformationsForm]');
            _$ModeloLaudoInformationForm.validate();
            $('.modal-dialog:last').css({ 'width': '70%', 'max-width': '800px' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
            //var textarea = $('#conteudo');
            //textarea.Editor();
            //textarea.Editor('setText', textarea.text());
           
            $('.text-editor').summernote({


                toolbar: [
                            ['style', ['bold', 'italic', 'underline']],
                            ['fontsize', ['fontsize']],
                            ['fontname', ['fontname']],
                            ['font', ['font', 'strikethrough', 'superscript', 'subscript']],
                            ['color', ['color']],
                            ['para', ['ul', 'ol', 'paragraph']],
                            ['height', ['height']],
                            ['misc', ['codeview', 'fullscreen']],
                            ['table', ['table']]
                ],
                height: 300,
                width: '90%',
                padding: 30
                // NA DOCUMENTACAO TEM VARIOS OUTROS BAGULHOS INTERESSANTES
            });
            var valorFormatacao = $('#conteudo').val();
            $("#formatacao").summernote("code", valorFormatacao);

    
        };

        this.save = function () {
            //var $textarea = $("#conteudo");
            //$textarea.text($textarea.Editor("getText"));
            if (!_$ModeloLaudoInformationForm.valid()) {
                return;
            }

            var modeloLaudo = _$ModeloLaudoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ModelosModeloLaudosService.criarOuEditar(modeloLaudo)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarModeloLaudoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };



      

    };
})(jQuery);