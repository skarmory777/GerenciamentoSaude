(function () {
    $(function () {
        var _$TabelasDominioTable = $('#TabelasDominioTable');
        var _TabelasDominioService = abp.services.app.tabelaDominio;
        var _$filterForm = $('#TabelasDominioFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.TabelaDominio.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.TabelaDominio.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.TabelaDominio.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TabelasDominio/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TabelasDominio/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTabelaDominioModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TabelasDominio/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$TabelasDominioTable.jtable({

            title: app.localize('TabelaDominio'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TabelasDominioService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
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
                                    deleteTabelasDominio(data.record);
                                });
                        }

                        return $span;
                    }
                },
                tipoTabelaDominio: {
                    title: app.localize('TipoTabelaDominio'),
                    width: '12%',
                    display: function (data) {
                        return data.record.tipoTabelaDominio.descricao;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '20%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getTabelasDominio(reload) {
            if (reload) {
                _$TabelasDominioTable.jtable('reload');
            } else {
                _$TabelasDominioTable.jtable('load', {
                    filtro: $('#TabelasDominioTableFilter').val(),
                    tipoTabelaId: $('#cbo-tipos-tabela').val() === '' ? 0 : $('#cbo-tipos-tabela').val(),
                    versaoTissId: $('#cbo-versoes-tiss').val() === '' ? 0 : $('#cbo-versoes-tiss').val()
                });
            }
        }

        function deleteTabelasDominio(tabelaDominio) {

            abp.message.confirm(
                app.localize('DeleteWarning', tabelaDominio.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TabelasDominioService.excluir(tabelaDominio)
                            .done(function () {
                                getTabelasDominio(true);
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

        $('#CreateNewTabelaDominioButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTabelasDominioParaExcelButton').click(function () {
            _TabelasDominioService
                .listarParaExcel({
                    filtro: $('#TabelasDominioTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTabelasDominioButton, #RefreshTabelasDominioListButton').click(function (e) {
            e.preventDefault();
            getTabelasDominio();
        });

        abp.event.on('app.CriarOuEditarTabelaDominioModalSaved', function () {
            getTabelasDominio(true);
        });

        getTabelasDominio();

        $('#TabelasDominioTableFilter').focus();
    });
})();