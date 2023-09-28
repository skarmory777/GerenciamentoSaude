﻿(function () {
    $(function () {
        var _$PlanosTable = $('#PlanosTable');
        var _PlanosService = abp.services.app.plano;
        var _$filterForm = $('#PlanosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Plano.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Planos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Planos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPlanoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Planos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$PlanosTable.jtable({

            title: app.localize('Planos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _PlanosService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deletePlanos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Plano'),
                    width: '15%'
                },
                convenio: {
                    title: app.localize('Convenio'),
                    width: '15%',
                    sorting: false,
                    display: function (data) {
                        return data.record.convenio.nomeFantasia
                    }
                },
            }
        });

        function getPlanos(reload) {
            if (reload) {
                _$PlanosTable.jtable('reload');
            } else {
                _$PlanosTable.jtable('load', {
                    filtro: $('#PlanosTableFilter').val()
                });
            }
        }

        function deletePlanos(Plano) {

            abp.message.confirm(
                app.localize('DeleteWarning', Plano.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _PlanosService.excluir(Plano)
                            .done(function () {
                                getPlanos(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewPlanoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarPlanosParaExcelButton').click(function () {
            _PlanosService
                .listarParaExcel({
                    filtro: $('#PlanosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetPlanosButton, #RefreshPlanosListButton').click(function (e) {
            e.preventDefault();
            getPlanos();
        });

        abp.event.on('app.CriarOuEditarPlanoModalSaved', function () {
            getPlanos(true);
        });

        getPlanos();

        $('#PlanosTableFilter').focus();
    });
})();