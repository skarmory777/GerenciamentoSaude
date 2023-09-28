(function () {
    $(function () {
        var _$TiposLeitoTable = $('#TiposLeitoTable');
        var _TiposLeitoService = abp.services.app.tipoLeito;
        var _$filterForm = $('#TiposLeitoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoLeito.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposLeito/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposLeito/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoLeitoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposLeito/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$TiposLeitoTable.jtable({

            title: app.localize('TipoLeito'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposLeitoService.listar
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
                                    deleteTiposLeito(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigoTiss: {
                    title: app.localize('CodigoTiss'),
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

        function getTiposLeito(reload) {
            if (reload) {
                _$TiposLeitoTable.jtable('reload');
            } else {
                _$TiposLeitoTable.jtable('load', {
                    filtro: $('#TiposLeitoTableFilter').val()
                });
            }
        }

        function deleteTiposLeito(tipoLeito) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoLeito.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposLeitoService.excluir(tipoLeito)
                            .done(function () {
                                getTiposLeito(true);
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

        $('#CreateNewTipoLeitoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposLeitoParaExcelButton').click(function () {
            _TiposLeitoService
                .listarParaExcel({
                    filtro: $('#TiposLeitoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposLeitoButton, #RefreshTiposLeitoListButton').click(function (e) {
            e.preventDefault();
            getTiposLeito();
        });

        abp.event.on('app.CriarOuEditarTipoLeitoModalSaved', function () {
            getTiposLeito(true);
        });

        getTiposLeito();

        $('#TiposLeitoTableFilter').focus();
    });
})();