(function () {
    $(function () {
        var _$ElementosHtmlTable = $('#ElementosHtmlTable');
        var _ElementosHtmlService = abp.services.app.elementoHtml;
        var _$filterForm = $('#ElementosHtmlFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtml.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtml.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ElementoHtml.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ElementosHtml/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ElementosHtml/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarElementoHtmlModal'
        });

        _$ElementosHtmlTable.jtable({

            title: app.localize('ElementoHtml'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ElementosHtmlService.listar
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
                                    deleteElementosHtml(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%'
                },
                tamanho: {
                    title: app.localize('Tamanho'),
                    width: '20%'
                },
                isRequerido: {
                    title: app.localize('IsRequerido'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.isRequerido) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                isDesativado: {
                    title: app.localize('IsDesativado'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.isDesativado) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
            }
        });

        function getElementosHtml() {
            _$ElementosHtmlTable.jtable('load', {
                filtro: $('#ElementosHtmlTableFilter').val()
            });
        }

        function deleteElementosHtml(elementoHtml) {
            abp.message.confirm(
                app.localize('DeleteWarning', elementoHtml.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ElementosHtmlService.excluir(elementoHtml)
                            .done(function () {
                                getElementosHtml(true);
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

        $('#CreateNewElementoHtmlButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarElementosHtmlParaExcelButton').click(function () {
            _ElementosHtmlService
                .listarParaExcel({
                    filtro: $('#ElementosHtmlTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetElementosHtmlButton, #RefreshElementosHtmlListButton').click(function (e) {
            e.preventDefault();
            getElementosHtml();
        });

        abp.event.on('app.CriarOuEditarElementoHtmlModalSaved', function () {
            getElementosHtml();
        });

        getElementosHtml();

        $('#ElementosHtmlTableFilter').focus();
    });
})();