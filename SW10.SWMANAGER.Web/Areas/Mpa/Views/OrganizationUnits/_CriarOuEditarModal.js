(function ($) {
    app.modals.CriarOuEditarUnidadeOrganizacionalModal = function () {

        var _unidadesOrganizacionaisService = abp.services.app.unidadeOrganizacional;
        var _modalManager;
        var _$unidadeOrganizacionalInformationForm = null;

        // OrganizationUnit
        var _organizationUnitService = abp.services.app.organizationUnit;
        var _$form = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$unidadeOrganizacionalInformationForm = _modalManager.getModal().find('form[name=UnidadeOrganizacionalInformationsForm]');
            _$unidadeOrganizacionalInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css({ 'width': '30%', 'max-width': '600px' });

            // OrganizationUnits
            _$form = _modalManager.getModal().find('form[name=OrganizationUnitForm]');
            _$form.validate({ ignore: "" });
        };

        this.save = function () {

            if (!_$form.valid()) {
                return;
            }

            var parentId = $('#parent-id').val();
            var displayName = $('#display-name').val();
            var organizationUnit = { "ParentId": parentId, "DisplayName": displayName };

            _modalManager.setBusy(true);

            var modelId = $('#is-edit-mode').val();

            // Criacao
            if (modelId == 0) {

                _organizationUnitService.createOrganizationUnit(organizationUnit)
                    .done(function (result) {

                        // UnidadeOrganizacional
                        if (!_$unidadeOrganizacionalInformationForm.valid()) {
                            return;
                        }

                        var unidadeOrganizacional = _$unidadeOrganizacionalInformationForm.serializeFormToObject();
                        unidadeOrganizacional.organizationUnitId = result.id;

                        _unidadesOrganizacionaisService.criarOuEditar(unidadeOrganizacional)
                            .done(function () {
                                abp.event.trigger('app.CriarOuEditarUnidadeOrganizacionalModalSaved');
                            })
                            .always(function () {
                                _modalManager.setBusy(false);
                            });

                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.setResult(result);
                        _modalManager.close();
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            }
            // Edicao
            else {

                var organizationUnitId = $('#organization-unit-id').val();

                var createOrganizationUnit = {
                    id: organizationUnitId,
                    displayName: organizationUnit.DisplayName
                };

                _organizationUnitService.updateOrganizationUnit(createOrganizationUnit)
                    .done(function (result) {
                        if (!_$unidadeOrganizacionalInformationForm.valid()) {
                            return;
                        }

                        var unidadeOrganizacional = _$unidadeOrganizacionalInformationForm.serializeFormToObject();
                        unidadeOrganizacional.organizationUnitId = result.id;

                        _unidadesOrganizacionaisService.criarOuEditar(unidadeOrganizacional)
                            .done(function () {
                                abp.event.trigger('app.CriarOuEditarUnidadeOrganizacionalModalSaved');
                            })
                            .always(function () {
                                _modalManager.setBusy(false);
                            });

                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.setResult(result);
                        _modalManager.close();
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            }

        };


        selectSW('.selectEstoque', "/api/services/app/estoque/ResultDropdownList");

    };
})(jQuery);