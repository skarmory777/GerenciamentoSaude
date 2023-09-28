(function () {
    $(function () {
        var _$MetodoTable = $('#MetodoTable');
        var _MetodoService = abp.services.app.cadastros;
        var _$filterForm = $('#MetodoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Metodo.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Metodo.Edit'),
            delete: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Metodo.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Laboratorio/Cadastros/MetodosCriarOuEditarModal',
            scriptUrl: abp.appPath + 'scripts/laboratorio/cadastros/metodo/_criaroueditarmodal.js',
            modalClass: 'CriarOuEditarMetodoModal'
        });


        _$MetodoTable.jtable({

            title: app.localize('Metodo'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _MetodoService.paginarMetodo
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
                                    deleteMetodo(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                }
            }
        });

        function getMetodo(reload) {
            if (reload) {
                _$MetodoTable.jtable('reload');
            } else {
                _$MetodoTable.jtable('load', {
                    filtro: $('#MetodoTableFilter').val()
                });
            }
        }

        function deleteMetodo(metodo) {

            abp.message.confirm(
                app.localize('DeleteWarning', metodo.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MetodoService.excluir(metodo)
                            .done(function () {
                                getMetodo(true);
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

        $('#CreateNewMetodoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarMetodoParaExcelButton').click(function () {
            _MetodoService
                .listarParaExcel({
                    filtro: $('#MetodoTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMetodoButton, #RefreshMetodoListButton').click(function (e) {
            e.preventDefault();
            getMetodo();
        });

        abp.event.on('app.CriarOuEditarMetodoModalSaved', function () {
            getMetodo(true);
        });

        getMetodo();

        $('#MetodoTableFilter').focus();
    });
})();