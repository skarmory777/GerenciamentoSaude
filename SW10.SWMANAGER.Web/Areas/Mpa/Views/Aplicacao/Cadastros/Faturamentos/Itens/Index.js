(function () {
    $(function () {

        var _$ItensTable = $('#ItensTable');
        var _ItensService = abp.services.app.faturamentoItem;
        var _$filterForm = $('#ItensFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Itens.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Itens.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Itens.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoItens/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Itens/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoItemModal'
        });

        
        _$ItensTable.jtable({

            title: app.localize('Itens'),
            paging: true,
            sorting: true,
            //multiSorting: true,

            actions: { listAction: { method: _ItensService.listar } },

            fields: {
                id: { key: true, list: false },

                actions: {
                    title: app.localize('Actions'),
                    width: '7%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                              //      //console.log(JSON.stringify(data));
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteItens(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                referencia: {
                    title: app.localize('Referencia'),
                    width: '6%'
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%',
                    display: function (data) {
                        var valor = data.record.codigo + ' - ' + data.record.descricao;
                        return valor;
                    }
                }
                ,
                grupo: {
                    title: app.localize('Grupo'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.grupo) {
                            return data.record.grupo.descricao;
                        }
                    }
                }
                ,
                subGrupo: {
                    title: app.localize('SubGrupo'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.subGrupo) {
                            return data.record.subGrupo.descricao;
                        }
                    }
                }
                ,
                tipo: {
                    title: app.localize('Tipo'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.grupo) {
                            if (data.record.grupo.tipoGrupo) {
                                return data.record.grupo.tipoGrupo.descricao;
                            }
                        }
                    }
                }
            }
        });

        _$ItensTable.jtable('option', 'pageSize', 25);

        function getItens(reload) {
            if (reload) {
                _$ItensTable.jtable('reload');
            } else {

                var tipo = $('#combo-tipo option:selected').val();
                var grupo = $('#combo-grupo option:selected').val();
                var subGrupo = $('#combo-subGrupo option:selected').val();

                _$ItensTable.jtable('load', {
                    filtro: $('#ItensTableFilter').val(),
                    tipoId: tipo,
                    grupoId: grupo,
                    subGrupoId: subGrupo
                });
            }
        }

        //function getItens() {
        //    _$ItensTable.jtable('load', createRequestParams());
        //}

        function deleteItens(item) {

            abp.message.confirm(
                app.localize('DeleteWarning', item.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ItensService.excluir(item)
                            .done(function () {
                                getItens(true);
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
            $('#AdvacedItensFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedItensFiltersArea').slideUp();
        });

        $('#CreateNewItemButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarItensParaExcelButton').click(function () {
            _ItensService
                .listarParaExcel({
                    filtro: $('#ItensTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetItensButton, #RefreshItensListButton').click(function (e) {
            e.preventDefault();
            getItens();
        });

        abp.event.on('app.CriarOuEditarItemModalSaved', function () {
            getItens(true);
        });

        getItens();

        $('#ItensTableFilter').focus();

    });
})();