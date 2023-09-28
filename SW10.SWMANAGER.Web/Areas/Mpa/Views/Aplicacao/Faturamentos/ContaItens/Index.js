(function () {
    $(function () {
        var _$ContaItensTable = $('#ContaItensTable');
        var _ContaItensService = abp.services.app.contaItem;
        var _$filterForm = $('#ContaItensFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ContaItem.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ContaItem.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ContaItem.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ContaItens/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ContaItens/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarContaItemModal'
        });

        _$ContaItensTable.jtable({

            title: app.localize('ContaItens'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ContaItensService.listar
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
                                    deleteContaItens(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '15%'
                }
            }
        });

        function getContaItens(reload) {
            if (reload) {
                _$ContaItensTable.jtable('reload');
            } else {
                _$ContaItensTable.jtable('load', {
                    filtro: $('#ContaItensTableFilter').val(),
                    estadoId: $('#cbo-estados').val() === '' ? 0 : $('#cbo-estados').val()
                });
            }
        }

        function deleteContaItens(contaItem) {

            abp.message.confirm(
                app.localize('DeleteWarning', contaItem.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ContaItensService.excluir(contaItem)
                            .done(function () {
                                getContaItens(true);
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

        $('#CreateNewContaItemButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarContaItensParaExcelButton').click(function () {
            _ContaItensService
                .listarParaExcel({
                    filtro: $('#ContaItensTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetContaItensButton, #RefreshContaItensListButton').click(function (e) {
            e.preventDefault();
            getContaItens();
        });

        //$('#cbo-estados').change(function (e) {
        //    e.preventDefault();
        //    getContaItens();
        //});
        abp.event.on('app.CriarOuEditarContaItemModalSaved', function () {
            getContaItens(true);
        });

        getContaItens();

        $('#ContaItensTableFilter').focus();


    });
})();