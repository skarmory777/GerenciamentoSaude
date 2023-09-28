(function () {
    $(function () {
        var _$TiposEntradaTable = $('#TiposEntradaTable');
        var _TiposEntradaService = abp.services.app.tipoEntrada;
        var _$filterForm = $('#TiposEntradaFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoEntrada.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoEntrada.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoEntrada.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposEntrada/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposEntrada/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoEntradaModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposEntrada/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$TiposEntradaTable.jtable({

            title: app.localize('TipoEntrada'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposEntradaService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '2%',
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
                                    deleteTiposEntrada(data.record);
                                });
                        }

                        return $span;
                    }
                },
              
                descricao: {
                    title: app.localize('Descricao'),
                    width: '8%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '2%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getTiposEntrada(reload) {
            if (reload) {
                _$TiposEntradaTable.jtable('reload');
            } else {
                _$TiposEntradaTable.jtable('load', {
                    filtro: $('#TiposEntradaTableFilter').val()
                });
            }
        }

        function deleteTiposEntrada(tipoEntrada) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoEntrada.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposEntradaService.excluir(tipoEntrada)
                            .done(function () {
                                getTiposEntrada(true);
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

        $('#CreateNewTipoEntradaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposEntradaParaExcelButton').click(function () {
            _TiposEntradaService
                .listarParaExcel({
                    filtro: $('#TiposEntradaTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposEntradaButton, #RefreshTiposEntradaListButton').click(function (e) {
            e.preventDefault();
            getTiposEntrada();
        });

        abp.event.on('app.CriarOuEditarTipoEntradaModalSaved', function () {
            getTiposEntrada(true);
        });

        getTiposEntrada();

        $('#TiposEntradaTableFilter').focus();
    });
})();