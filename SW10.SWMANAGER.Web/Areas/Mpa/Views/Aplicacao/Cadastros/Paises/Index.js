(function () {
    $(function () {
        var _$PaisesTable = $('#PaisesTable');
        var _PaisesService = abp.services.app.pais;
        var _$filterForm = $('#PaisesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Pais.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Pais.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Pais.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Paises/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Paises/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPaisModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Paises/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$PaisesTable.jtable({

            title: app.localize('Paises'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _PaisesService.listar
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
                                    deletePaises(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
                sigla: {
                    title: app.localize('Sigla'),
                    width: '15%'
                }
            }
        });

        function getPaises(reload) {
            if (reload) {
                _$PaisesTable.jtable('reload');
            } else {
                _$PaisesTable.jtable('load', {
                    filtro: $('#PaisesTableFilter').val()
                });
            }
        }

        function deletePaises(pais) {

            abp.message.confirm(
                app.localize('DeleteWarning', pais.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _PaisesService.excluir(pais)
                            .done(function () {
                                getPaises(true);
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

        $('#CreateNewPaisButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarPaisesParaExcelButton').click(function () {
            _PaisesService
                .listarParaExcel({
                    filtro: $('#PaisesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetPaisesButton, #RefreshPaisesListButton').click(function (e) {
            e.preventDefault();
            getPaises();
        });

        abp.event.on('app.CriarOuEditarPaisModalSaved', function () {
            getPaises(true);
        });

        getPaises();

        $('#PaisesTableFilter').focus();
    });
})();