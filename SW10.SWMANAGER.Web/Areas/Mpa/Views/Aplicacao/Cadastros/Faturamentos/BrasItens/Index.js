(function () {
    $(function () {
        var _$BrasItensTable = $('#BrasItensTable');
        var _BrasItensService = abp.services.app.faturamentoBrasItem;
        var _$filterForm = $('#BrasItensFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasItens.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasItens.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasItens.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoBrasItens/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasItens/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoBrasItemModal'
        });

        _$BrasItensTable.jtable({

            title: app.localize('BrasItens'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _BrasItensService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
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
                                    deleteBrasItens(data.record);
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
                    width: '50%'
                }
            }
        });

        function getBrasItens(reload) {
            if (reload) {
                _$BrasItensTable.jtable('reload');
            } else {
                _$BrasItensTable.jtable('load', {
                    filtro: $('#BrasItensTableFilter').val()
                });
            }
        }

        function deleteBrasItens(brasItem) {

            abp.message.confirm(
                app.localize('DeleteWarning', brasItem.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _BrasItensService.excluir(brasItem)
                            .done(function () {
                                getBrasItens(true);
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

        $('#CreateNewBrasItemButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarBrasItensParaExcelButton').click(function () {
            _BrasItensService
                .listarParaExcel({
                    filtro: $('#BrasItensTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetBrasItensButton, #RefreshBrasItensListButton').click(function (e) {
            e.preventDefault();
            getBrasItens();
        });

        abp.event.on('app.CriarOuEditarBrasItemModalSaved', function () {
            getBrasItens(true);
        });

        getBrasItens();

        $('#BrasItensTableFilter').focus();

        // Salvar BrasItem
        //function salvarBrasItem() {

        //    //var brasItemAppService = abp.

        //    _createOrEditModal.save();

        //    //var _$BrasItemInformationForm = $('form[name=BrasItemInformationsForm]');
        //    //var formData = _$BrasItemInformationForm.serialize();

        //    //var metodo = '/Mpa/FaturamentoBrasItens/SalvarBrasItem';
        //    //$.ajax({
        //    //    type: "POST",
        //    //    url: metodo,
        //    //    dataType: 'text',
        //    //    data: formData,
        //    //    success: function (result) {
        //    //        ////console.log('brasItem salvo');
        //    //        //$('#btn-fechar-modal').click();
        //    //        //$('#todas').click();
        //    //    },
        //    //    error: function (xhr, ajaxOptions, thrownError) {
        //    //        abp.notify.info(app.localize('Erro'));
        //    //    },
        //    //    beforeSend: function () {
        //    //    },
        //    //    complete: function () { }
        //    //});
        //}

    });
})();