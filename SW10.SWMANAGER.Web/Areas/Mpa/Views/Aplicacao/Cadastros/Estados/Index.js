(function () {
    $(function () {
        var _$EstadosTable = $('#EstadosTable');
        var _EstadosService = abp.services.app.estado;
        var _$filterForm = $('#EstadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Estado.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Estado.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Estado.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Estados/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Estados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarEstadoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Estados/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$EstadosTable.jtable({

            title: app.localize('Estados'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _EstadosService.listar
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
                                    deleteEstados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
                uf: {
                    title: app.localize('Uf'),
                    width: '15%'
                }
            }
        });

        function getEstados(reload) {
            if (reload) {
                _$EstadosTable.jtable('reload');
            } else {
                _$EstadosTable.jtable('load', {
                    filtro: $('#EstadosTableFilter').val()
                });
            }
        }

        function deleteEstados(estado) {
            abp.message.confirm(
                app.localize('DeleteWarning', estado.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _EstadosService.excluir(estado)
                            .done(function () {
                                getEstados(true);
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

        $('#CreateNewEstadoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarEstadosParaExcelButton').click(function () {
            _EstadosService
                .listarParaExcel({
                    filtro: $('#EstadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetEstadosButton, #RefreshEstadosListButton').click(function (e) {
            e.preventDefault();
            getEstados();
        });

        abp.event.on('app.CriarOuEditarEstadoModalSaved', function () {
            getEstados(true);
        });

        getEstados();

        $('#EstadosTableFilter').focus();
    });
})();