
(function ($) {
    app.modals.CriarOuEditarFilasModal = function () {

        var _filaService = abp.services.app.fila;

        $(document).ready(function () {
            // CamposRequeridos();
        });


        var _modalManager;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $('.modal-dialog').css({ 'min-width': '800px'});
            //_$contaAdministrativaInformationsForm = _modalManager.getModal().find('form[name=contaAdministrativaInformationsForm]');
            //_$contaAdministrativaInformationsForm.validate();
        };

        this.save = function () {


            _$filaInformationsForm = _modalManager.getModal().find('form[name=FilaInformationsForm]');
            _$filaInformationsForm.validate();

            if (!_$filaInformationsForm.valid()) {
                return;
            }

            var fila = _$filaInformationsForm.serializeFormToObject();

          
            _modalManager.setBusy(true);
            _filaService.criarOuEditar(fila)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFilaModalSaved');
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };



        //$('input[name="HoraZera"]').daterangepicker({
        //    //"singleDatePicker": true,
        //    //"showDropdowns": true,
        //    //autoUpdateInput: false,
        //    //changeYear: true,
        //    //yearRange: 'c-10:c+10',
        //    //showOn: "both",
        //    timePicker: true,
        //    timePicker24Hour: true,
        //    datePicker: false,
            
        //},
        //    function (selDate) {
        //        $('input[name="HoraZera"]').val(selDate.format('L')).addClass('form-control edited');
        //    });



        $('.minhacor').minicolors({
            defaults: {
                animationSpeed: 50,
                animationEasing: 'swing',
                change: null,
                changeDelay: 0,
                control: 'hue',
                defaultValue: '',
                format: 'hex',
                hide: null,
                hideSpeed: 100,
                inline: false,
                keywords: '',
                letterCase: 'lowercase',
                opacity: false,
                position: 'bottom left',
                show: null,
                showSpeed: 100,
                theme: 'default',
                swatches: []
            }
        });


        selectSW('.selectTipoLocalChamada', "/api/services/app/TipoLocalChamada/ListarTipoLocalChamadaDropdown");
        selectSW('.selectEmpresa', "/api/services/app/Empresa/ListarDropdown");


    };
})(jQuery);