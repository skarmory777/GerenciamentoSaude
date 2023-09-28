(function ($) {
    app.modals.CriarOuEditarMailingTemplateModal = function () {

        var _MailingTemplatesService = abp.services.app.mailingTemplate;

        var _modalManager;
        var _$MailingTemplateInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$MailingTemplateInformationForm = _modalManager.getModal().find('form[name=MailingTemplateInformationsForm]');
            _$MailingTemplateInformationForm.validate();
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
            //Ativando autocomplete no combobox
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            var textarea = $('#content-template');
            //textarea.Editor();
            //textarea.Editor('setText', textarea.text());
            $('#InsertImage').css('z-index', '2147483647');
            $('#InsertLink').css('z-index', '2147483646');
        };

        this.save = function () {
            //var $textarea = $("#content-template");
            //$textarea.text($textarea.Editor("getText"));
            if (!_$MailingTemplateInformationForm.valid()) {
         //       return;
            }

            var mailingTemplate = _$MailingTemplateInformationForm.serializeFormToObject();

            // testando summernote
            mailingTemplate.ContentTemplate = $('#content-template').summernote('code');
            // fim teste summernote

            _modalManager.setBusy(true);
            _MailingTemplatesService.criarOuEditar(mailingTemplate)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarMailingTemplateModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $("#classes").change(function () {
            $("#campos-disponiveis").val($(this).val()).addClass('edited');
        });

    };
})(jQuery);