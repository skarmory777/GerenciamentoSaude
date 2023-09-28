(function ($) {
    app.modals.CriarOuEditarUnidadeModal = function () {

     var _unidadeService = abp.services.app.unidade;

     var _modalManager;
     var _$UnidadeInformationForm = null;

     function retirarMascara(valor) {
         while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
         while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

         valor = valor.replace(',', '.');

         return valor;
     }

     function soNums(e) {

         //teclas adicionais permitidas (tab,delete,backspace,setas direita e esquerda)
         //keyCodesPermitidos = new Array(8, 9, 37, 39, 46);
         keyCodesPermitidos = new Array(8, 9, 16, 37, 39, 188, 46);

         //numeros e 0 a 9 do teclado alfanumerico
         for (x = 48; x <= 57; x++) {
             keyCodesPermitidos.push(x);
         }

         //numeros e 0 a 9 do teclado numerico
         for (x = 96; x <= 105; x++) {
             keyCodesPermitidos.push(x);
         }

         //Pega a tecla digitada
         keyCode = e.which;

         //alert(keyCode);

         //Verifica se a tecla digitada é permitida
         if ($.inArray(keyCode, keyCodesPermitidos) != -1) {
             return true;
         }
         return false;
     }

     this.init = function (modalManager) {
         _modalManager = modalManager;

         _$UnidadeInformationForm = _modalManager.getModal().find('form[name=UnidadeFilhaInformationsForm]');
         _$UnidadeInformationForm.validate();

         //fator
         //---------------------------------------------------------------------------------------------------------------------
         $('#fator-filha').bind('keydown', soNums);

         $('.modal-dialog').css({ 'width': '100%', 'max-width': '800px' });
         //$('div.form-group select').addClass('form-control selectpicker');
         //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
         $.fn.modal.Constructor.prototype.enforceFocus = function () { };
         //Ativando autocomplete no combobox
         //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
         $('ul.ui-autocomplete').css('z-index', '2147483647');

         if (($('#UnidadeReferenciaId').val() == "") || ($('#UnidadeReferenciaId').val() == null) || ($('#UnidadeReferenciaId').val() == undefined)) {
            $('#UnidadeReferenciaId').val($('#Id').val());
         };
         //debugger;
         if (($('#creatorUserId').val() != "") && ($('#creatorUserId').val() != null) && ($('#creatorUserId').val() != undefined)) {
             _unidadeService.obter($('#UnidadeReferenciaId').val())
                .done(function (data) {
                    //debugger;
                    $('#UndRefId').val(data.id + " - " + data.descricao);
                    $('#unidade-principal').show();
                });
         };

         $('#sigla').focus();
     };

     this.save = function () {
         if (!_$UnidadeInformationForm.valid()) {
             return;
         }

         var valor_fator = retirarMascara($('#fator-filha').val());

         $('#fator-filha').val(valor_fator);

         var unidade = _$UnidadeInformationForm.serializeFormToObject();

         _modalManager.setBusy(true);

         _unidadeService.criarOuEditar(unidade)
              .done(function (data) {
                  //debugger;

                  abp.notify.info(app.localize('SavedSuccessfully'));

                  _modalManager.close();

                  $('#sigla').focus();
              })
             .always(function () {
                 _modalManager.setBusy(false);

                 abp.event.trigger('app.CriarOuEditarUnidadeModalSaved');

                 abp.event.trigger('app.CriarOuEditarUnidadeFilhaModalSaved');
             });
     };
};
})(jQuery);