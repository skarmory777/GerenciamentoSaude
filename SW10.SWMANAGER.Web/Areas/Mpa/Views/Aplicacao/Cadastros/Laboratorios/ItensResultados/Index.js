(function () {
    $(function () {
        var _$ItemResultadosTable = $('#ItemResultadosTable');
        var _ItemResultadosService = abp.services.app.itemResultado;
        var _$filterForm = $('#ItemResultadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ItemResultado.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ItemResultado.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ItemResultado.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ItensResultados/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ItensResultados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarItemResultadoModal'
        });

        _$ItemResultadosTable.jtable({

            title: app.localize('ItensResultados'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ItemResultadosService.listar
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
                                    deleteItemResultados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Código'),
                    width: '5%'
                },
                descricao: {
                    title: app.localize('Item'),
                    width: '25%'
                },


                unidade: {
                    title: app.localize('Unidade'),
                    width: '15%',
                    display: function (data) {
                       
                        if (data.record.laboratorioUnidade) {
                            return data.record.laboratorioUnidade.descricao;
                        }
                    }
                },

                casaDecimal: {
                    title: app.localize('Decimal'),
                    width: '5%'
                },
                tipoResultado: {
                    title: app.localize('TipoResultado'),
                    width: '15%',
                    display: function (data) {
                       
                        if (data.record.tipoResultado) {
                            return data.record.tipoResultado.descricao;
                        }
                    }
                },
                referencia: {
                    title: app.localize('Referencia'),
                    width: '15%'
                },
                isInterface: {
                    title: app.localize('IsInterface'),
                    width: '15%',
                    display: function (data) {
                        var booleana = 'No';
                        var classe = 'label-default';
                        if (data.record.isInterface) {
                            booleana = "Yes";
                            classe = 'label-success';
                        }
                        return '<div style="text-align:center;">' + '<span class="label ' + classe + ' content-center text-center">' + app.localize(booleana) + '</span>' + '</div>';
                    }
                },
                'interface': {
                    title: app.localize('Interface'),
                    width: '15%'
                }


            }
        });

        function getItemResultados(reload) {
            if (reload) {
                _$ItemResultadosTable.jtable('reload');
            } else {
                _$ItemResultadosTable.jtable('load', {
                    filtro: $('#ItemResultadosTableFilter').val()
                });
            }
        }

        function deleteItemResultados(ItemResultado) {

            abp.message.confirm(
                app.localize('DeleteWarning', ItemResultado.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ItemResultadosService.excluir(ItemResultado)
                            .done(function () {
                                getItemResultados(true);
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

        $('#CreateNewItemResultadoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarItemResultadosParaExcelButton').click(function () {
            _ItemResultadosService
                .listarParaExcel({
                    filtro: $('#ItemResultadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetItemResultadosButton, #RefreshItemResultadosListButton').click(function (e) {
            e.preventDefault();
            getItemResultados();
        });

        abp.event.on('app.CriarOuEditarItemResultadoModalSaved', function () {
            getItemResultados(true);
        });

        getItemResultados();

        $('#ItemResultadosTableFilter').focus();


    });
})();