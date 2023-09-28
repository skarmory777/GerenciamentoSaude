(function () {
    $(function () {
        var _$ItensTabelaTable = $('#ItensTabelaTable');
        var _ItensTabelaService = abp.services.app.faturamentoItemTabela;
        var _$filterForm = $('#ItensTabelaFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.ItensTabela.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.ItensTabela.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.ItensTabela.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoItensTabela/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/ItensTabela/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoItemTabelaModal'
        });

        _$ItensTabelaTable.jtable({

            title: app.localize('ItensTabela'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ItensTabelaService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
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
                                    deleteItensTabela(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                codigo: {
                    title: app.localize('Codigo'),
                    width: '20%'
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                }
                //,
                //tipo: {
                //    title: app.localize('Tipo'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data.record.tipoItemTabela) {
                //            return data.record.tipoItemTabela.descricao;
                //        }
                //    }
                //}
                //,
                //isInclusaoManual: {
                //    title: app.localize('IsInclusaoManual'),
                //    width: '5%'
                //}
            }
        });

        function getItensTabela(reload) {
            if (reload) {
                _$ItensTabelaTable.jtable('reload');
            } else {
                _$ItensTabelaTable.jtable('load', {
                    filtro: $('#ItensTabelaTableFilter').val()
                });
            }
        }

        function deleteItensTabela(itemTabela) {

            abp.message.confirm(
                app.localize('DeleteWarning', itemTabela.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ItensTabelaService.excluir(itemTabela)
                            .done(function () {
                                getItensTabela(true);
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

        $('#CreateNewItemTabelaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarItensTabelaParaExcelButton').click(function () {
            _ItensTabelaService
                .listarParaExcel({
                    filtro: $('#ItensTabelaTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetItensTabelaButton, #RefreshItensTabelaListButton').click(function (e) {
            e.preventDefault();
            getItensTabela();
        });

        abp.event.on('app.CriarOuEditarItemTabelaModalSaved', function () {
            getItensTabela(true);
        });

        getItensTabela();

        $('#ItensTabelaTableFilter').focus();
    });
})();