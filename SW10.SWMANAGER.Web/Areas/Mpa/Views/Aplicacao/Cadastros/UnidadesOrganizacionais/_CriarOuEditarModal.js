//(function ($) {
//    app.modals.CriarOuEditarUnidadeOrganizacionalModal = function () {

//        var _unidadesOrganizacionaisService = abp.services.app.unidadeOrganizacional;
//        var _modalManager;
//        var _$unidadeOrganizacionalInformationForm = null;

//        // OrganizationUnit
//        var _organizationUnitService = abp.services.app.organizationUnit;
//        var _$form = null;

//        this.init = function (modalManager) {

//            _modalManager = modalManager;

//            _$unidadeOrganizacionalInformationForm = _modalManager.getModal().find('form[name=UnidadeOrganizacionalInformationsForm]');
//            _$unidadeOrganizacionalInformationForm.validate({ ignore: "" });

//            // OrganizationUnits
//            _$form = _modalManager.getModal().find('form[name=OrganizationUnitForm]');
//            _$form.validate({ ignore: "" });
//        };

//        this.save = function () {


//            var orgUnit = _$form.serializeFormToObject();


//            $.ajax({
//                type: 'post',
//                url: '/Mpa/OrganizationUnits/SalvarOrganizationUnit',
//                data: orgUnit,
//                dataType: 'json',
//                contentType: false,
//                processData: false,
//                success: function (response) {
//                    //console.log('SALVO SUCESSO org unit: ' + JSON.stringify(response));
//                },
//                error: function (error) {
//                    abp.notify.info(app.localize('Erro'));
//                    //console.log('ERRO AO SALVAR org unit: ' + JSON.stringify(error));
//                }
//            });


//            //if (!_$form.valid()) {
//            //    return;
//            //}

//            //var organizationUnit = _$form.serializeFormToObject();

//            //_modalManager.setBusy(true);
//            //_organizationUnitService.createOrganizationUnit(
//            //    organizationUnit
//            //).done(function (result) {



//            //    //console.log(JSON.stringify(result));

//            //    // UnidadeOrganizacional

//            //    //console.log('unit salva dentro DONE');

//            //    if (!_$unidadeOrganizacionalInformationForm.valid()) {
//            //        //console.log('invalid form');
//            //        return;
//            //    }

//            //    var unidadeOrganizacional = _$unidadeOrganizacionalInformationForm.serializeFormToObject();

//            //    _modalManager.setBusy(true);

//            //    //console.log('pos modal busy');

//            //    _unidadesOrganizacionaisService.criarOuEditar(unidadeOrganizacional)
//            //         .done(function () {
//            //             abp.notify.info(app.localize('SavedSuccessfully'));
//            //             _modalManager.close();
//            //             abp.event.trigger('app.CriarOuEditarUnidadeOrganizacionalModalSaved');
//            //         })
//            //        .always(function () {
//            //            _modalManager.setBusy(false);
//            //        });


//            //    abp.notify.info(app.localize('SavedSuccessfully'));
//            //    _modalManager.setResult(result);
//            //    _modalManager.close();
//            //}).always(function () {
//            //    _modalManager.setBusy(false);
//            //});

            
//        };
//    };
//})(jQuery);