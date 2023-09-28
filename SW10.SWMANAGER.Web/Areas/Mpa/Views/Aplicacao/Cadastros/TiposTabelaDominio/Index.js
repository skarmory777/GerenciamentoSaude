(function () {
    $(function () {
        var _$TiposTabelaDominioTable = $('#TiposTabelaDominioTable');
        var _TiposTabelaDominioService = abp.services.app.tipoTabelaDominio;
        var _$filterForm = $('#TiposTabelaDominioFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.TipoTabelaDominio.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.TipoTabelaDominio.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.TipoTabelaDominio.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposTabelaDominio/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposTabelaDominio/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoTabelaDominioModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposTabelaDominio/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$TiposTabelaDominioTable.jtable({

            title: app.localize('TipoTabelaDominio'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposTabelaDominioService.listar
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
                                    deleteTiposTabelaDominio(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '4%'
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

        function getTiposTabelaDominio(reload) {
            if (reload) {
                _$TiposTabelaDominioTable.jtable('reload');
            } else {
                _$TiposTabelaDominioTable.jtable('load', {
                    filtro: $('#TiposTabelaDominioTableFilter').val()
                });
            }
        }

        function deleteTiposTabelaDominio(tipoTabelaDominio) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoTabelaDominio.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposTabelaDominioService.excluir(tipoTabelaDominio)
                            .done(function () {
                                getTiposTabelaDominio(true);
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

        $('#CreateNewTipoTabelaDominioButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposTabelaDominioParaExcelButton').click(function () {
            _TiposTabelaDominioService
                .listarParaExcel({
                    filtro: $('#TiposTabelaDominioTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposTabelaDominioButton, #RefreshTiposTabelaDominioListButton').click(function (e) {
            e.preventDefault();
            getTiposTabelaDominio();
        });

        abp.event.on('app.CriarOuEditarTipoTabelaDominioModalSaved', function () {
            getTiposTabelaDominio(true);
        });

        getTiposTabelaDominio();

        $('#TiposTabelaDominioTableFilter').focus();
    });
})();