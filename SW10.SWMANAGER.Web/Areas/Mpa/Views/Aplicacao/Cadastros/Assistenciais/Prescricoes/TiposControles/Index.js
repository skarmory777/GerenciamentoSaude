(function () {
    $(function () {
        var _$TiposControlesTable = $('#TiposControlesTable');
        var _TiposControlesService = abp.services.app.tipoControle;
        var _$filterForm = $('#TiposControlesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoControle.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoControle.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoControle.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposControles/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposControles/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoControleModal'
        });

        _$TiposControlesTable.jtable({

            title: app.localize('TipoControle'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposControlesService.listar
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
                                    deleteTiposControles(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '20%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '60%'
                },
            }
        });

        function getTiposControles() {
            //if (reload) {
            //    _$TiposControlesTable.jtable('reload');
            //} else {
            _$TiposControlesTable.jtable('load', {
                filtro: $('#TiposControlesTableFilter').val()
            });
            //}
        }

        function deleteTiposControles(tipoControle) {
            abp.message.confirm(
                app.localize('DeleteWarning', tipoControle.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposControlesService.excluir(tipoControle)
                            .done(function () {
                                getTiposControles(true);
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

        $('#CreateNewTipoControleButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarTiposControlesParaExcelButton').click(function () {
            _TiposControlesService
                .listarParaExcel({
                    filtro: $('#TiposControlesTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposControlesButton, #RefreshTiposControlesListButton').click(function (e) {
            e.preventDefault();
            getTiposControles();
        });

        abp.event.on('app.CriarOuEditarTipoControleModalSaved', function () {
            getTiposControles();
        });

        getTiposControles();

        $('#TiposControlesTableFilter').focus();
    });
})();