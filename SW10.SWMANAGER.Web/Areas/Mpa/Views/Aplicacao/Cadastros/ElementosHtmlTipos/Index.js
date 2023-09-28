(function () {
    $(function () {
        var _$ElementosHtmlTiposTable = $('#ElementosHtmlTiposTable');
        var _ElementosHtmlTiposService = abp.services.app.elementoHtmlTipo;
        var _$filterForm = $('#ElementosHtmlTiposFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtmlTipo.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtmlTipo.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtmlTipo.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ElementosHtmlTipos/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ElementosHtmlTipos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarElementoHtmlTipoModal'
        });

        _$ElementosHtmlTiposTable.jtable({

            title: app.localize('ElementoHtmlTipo'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ElementosHtmlTiposService.listar
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
                                    deleteElementosHtmlTipos(data.record);
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
                    width: '40%'
                },
                htmlHelper: {
                    title: app.localize('Tamanho'),
                    width: '20%'
                },
            }
        });

        function getElementosHtmlTipos() {
            _$ElementosHtmlTiposTable.jtable('load', {
                filtro: $('#ElementosHtmlTiposTableFilter').val()
            });
        }

        function deleteElementosHtmlTipos(elementoHtmlTipo) {
            abp.message.confirm(
                app.localize('DeleteWarning', elementoHtmlTipo.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ElementosHtmlTiposService.excluir(elementoHtmlTipo)
                            .done(function () {
                                getElementosHtmlTipos(true);
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

        $('#CreateNewElementoHtmlTipoButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarElementosHtmlTiposParaExcelButton').click(function () {
            _ElementosHtmlTiposService
                .listarParaExcel({
                    filtro: $('#ElementosHtmlTiposTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetElementosHtmlTiposButton, #RefreshElementosHtmlTiposListButton').click(function (e) {
            e.preventDefault();
            getElementosHtmlTipos();
        });

        abp.event.on('app.CriarOuEditarElementoHtmlTipoModalSaved', function () {
            getElementosHtmlTipos();
        });

        getElementosHtmlTipos();

        $('#ElementosHtmlTiposTableFilter').focus();
    });
})();