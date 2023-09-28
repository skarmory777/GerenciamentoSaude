(function () {
    $(function () {
        /* Js Index Formula Faturamento */
        var _$formulasFaturamentosTable = $('#FormulasFaturamentosTable');
        var _formulasFaturamentosService = abp.services.app.formulaFaturamento;
        var _fatItemService = abp.services.app.faturamentoItem;
        var _materialService = abp.services.app.material;
        var _$faturamentofilterForm = $('#FormulasFaturamentosFilterForm');

        var _permissionsFormulaFaturamento = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaFaturamento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaFaturamento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaFaturamento.Delete')
        };

        _$formulasFaturamentosTable.jtable({

            title: app.localize('FormulaFaturamento'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: retornarLista
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
                        var idGrid = data.record.idGridFormulasFaturamento;
                        var $span = $('<span></span>');
                        if (_permissionsFormulaFaturamento.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault()
                                    var list = JSON.parse($('#formula-faturamento-list').val());
                                    for (var i = 0; i < list.length; i++) {
                                        if (list[i].IdGridFormulasFaturamento == idGrid) {
                                            var result = list[i];
                                            break;
                                        }
                                    }
                                    var $option;
                                    var $select;
                                    $('#id-grid-formulas-faturamento').val(idGrid);
                                    $('#id-formula-faturamento').val(result.Id);
                                    $('#codigo-formula-faturamento').val(result.Codigo);
                                    $('#descricao-formula-faturamento').val(result.Descricao);
                                    if (result.FaturamentoItemId != null) {
                                        _fatItemService.obter(result.FaturamentoItemId).done(function (retorno) {
                                            $('#faturamento-item-id-formula-faturamento')
                                                .append($('<option value="' + result.FaturamentoItemId + '">' + retorno.codigo + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.FaturamentoItemId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#faturamento-item-id-formula-faturamento').val(null).change();
                                    }
                                    if (result.MaterialId != null) {
                                        _materialService.obter(result.MaterialId).done(function (retorno) {
                                            $('#material-id-formula-faturamento')
                                                .append($('<option value="' + result.MaterialId + '">' + retorno.codigo + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.MaterialId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#material-id-formula-faturamento').val(null).change();
                                    }
                                    if (result.IsFatura) {
                                        $('#is-fatura-formula-faturamento').attr('checked', 'checked');
                                    }
                                    else {
                                        $('#is-fatura-formula-faturamento').removeAttr('checked');
                                    }
                                });
                        }
                        if (_permissionsFormulaFaturamento.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteFormulasFaturamentos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.isDeleted == true) {
                            return '<span style="text-decoration:line-through;color:red;">' + data.record.codigo + '</span>';
                        }
                        else {
                            return '<span">' + data.record.codigo + '</span>';
                        }
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '70%',
                    display: function (data) {
                        if (data.record.isDeleted) {
                            return '<span style="text-decoration:line-through;color:red;">' + data.record.descricao + '</span>';
                        }
                        else {
                            return data.record.descricao;
                        }
                    }
                },
            }
        });

        function getFormulasFaturamentos(reload) {
            if (reload) {
                _$formulasFaturamentosTable.jtable('reload');
            }
            else {
                _$formulasFaturamentosTable.jtable('load', {
                    //filtro: $('#FormulasFaturamentosTableFilter').val(),
                    prescricaoItemId: $('#prescricao-item-id-formula-faturamento').val()

                });
            }
        }

        function deleteFormulasFaturamentos(formulaFaturamento) {
            abp.message.confirm(
                app.localize('DeleteWarning', formulaFaturamento.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        if ($('#formula-faturamento-list').val() != '') {
                            lista = JSON.parse($('#formula-faturamento-list').val());
                            for (var i = 0; i < lista.length; i++) {
                                if (lista[i].IdGridFormulasFaturamento == formulaFaturamento.idGridFormulasFaturamento) {
                                    if (lista[i].IsDeleted) {
                                        lista[i].IsDeleted = false;
                                        lista[i].DeleterUserId = '';
                                    }
                                    else {
                                        lista[i].IsDeleted = true;
                                        lista[i].DeleterUserId = abp.session.userId;
                                    }
                                    break;
                                }
                            }
                        }
                        $('#formula-faturamento-list').val(JSON.stringify(lista));
                        localStorage["FormulaFaturamentoList"] = JSON.stringify(lista);
                        abp.notify.info(app.localize('ListaAtualizada'));
                        getFormulasFaturamentos(true);
                    }
                }
            );


            //abp.message.confirm(
            //    app.localize('DeleteWarning', formulaFaturamento.descricao),
            //    function (isConfirmed) {
            //        if (isConfirmed) {
            //            _formulasFaturamentosService.excluir(formulaFaturamento)
            //                .done(function () {
            //                    getFormulasFaturamentos(true);
            //                    abp.notify.success(app.localize('SuccessfullyDeleted'));
            //                });
            //        }
            //    }
            //);
        }

        function retornarLista(filtro) {
            if ($('#formula-faturamento-list').val() != '') {
                var js = $('#formula-faturamento-list').val();
                var json = JSON.parse(js);
                var res = _formulasFaturamentosService.listarFaturamentoJson(json);  //  '"{Result":"OK","Records":' + js + '}'
                return res;
            }
            else {
                var res = _formulasFaturamentosService.listarFaturamentoPorPrescricaoItem({
                    //filtro: $('#FormulasFaturamentosTableFilter').val(),
                    id: $('#prescricao-item-id-formula-faturamento').val()
                });
                return res;
            }
        }

        $('#CreateNewFormulaFaturamentoButton').click(function (e) {
            e.preventDefault();
            var formulaFaturamentoList = $('#formula-faturamento-list').val();
            localStorage["FormulaFaturamentoList"] = formulaFaturamentoList;
            if (!localStorage["FormulaFaturamentoList"] || (localStorage["FormulaFaturamentoList"] && localStorage["FormulaFaturamentoList"] == '[]')) {
                localStorage["FormulaFaturamentoList"] = '';
            }
            if (localStorage["FormulaEstoqueList"] != '') {
                var lista = JSON.parse($('#formula-faturamento-list').val());
            }
            else {
                var lista = [];
            }
            $('#id-grid-formulas-faturamento').val(lista.length + 1);
            $('#id-formula-faturamento').val('0');
            $('#codigo-formula-faturamento').val('');
            $('#descricao-formula-faturamento').val('');
            $('#id-grid-formulas-faturamento').val('');
            $('#faturamento-item-id-formula-faturamento').val(null).change();
            $('#material-id-formula-faturamento').val(null).change();
            $('#is-fatura-formula-faturamento').removeAttr('checked');

            //_createOrEditFormulaFaturamentoModal.open({ prescricaoItemId: $('#prescricao-item-id').val() });
            //$('#FormulaFaturamentoForm').load('/mpa/prescricoesItens/_CriarOuEditarFormulaFaturamentoModal',
            //    {
            //        prescricaoItemId: $('#prescricao-item-id-formula-faturamento').val(),
            //        idGrid: $('id-grid-formulas-faturamento').val(),
            //    },
            //    function () {
            //        $('#faturamento-item-id-formula-faturamento').select2({
            //            ajax: {
            //                url: '/api/services/app/FaturamentoItem/ListarFatItemDropdown',
            //                dataType: 'json',
            //                delay: 250,
            //                method: 'Post',
            //                data: function (params) {
            //                    if (params.page == undefined)
            //                        params.page = '1';
            //                    return {
            //                        search: params.term,
            //                        page: params.page,
            //                        totalPorPagina: 10
            //                    };
            //                },
            //                processResults: function (data, params) {
            //                    params.page = params.page || 1;
            //                    return {
            //                        results: data.result.items,
            //                        pagination: {
            //                            more: (params.page * 10) < data.result.totalCount
            //                        }
            //                    };
            //                },
            //                cache: true
            //            },
            //            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            //            minimumInputLength: 0
            //        })
            //        $('#material-id-formula-faturamento').select2({
            //            ajax: {
            //                url: '/api/services/app/material/listarDropdown',
            //                dataType: 'json',
            //                delay: 250,
            //                method: 'Post',
            //                data: function (params) {
            //                    if (params.page == undefined)
            //                        params.page = '1';
            //                    return {
            //                        search: params.term,
            //                        page: params.page,
            //                        totalPorPagina: 10
            //                    };
            //                },
            //                processResults: function (data, params) {
            //                    params.page = params.page || 1;
            //                    return {
            //                        results: data.result.items,
            //                        pagination: {
            //                            more: (params.page * 10) < data.result.totalCount
            //                        }
            //                    };
            //                },
            //                cache: true
            //            },
            //            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            //            minimumInputLength: 0
            //        })
            //    });
        });

        $('#ExportarFormulasFaturamentosParaExcelButton').click(function () {
            _formulasFaturamentosService
                .listarParaExcel({
                    //filtro: $('#FormulasFaturamentosTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFormulasFaturamentosButton, #RefreshFormulasFaturamentosListButton').click(function (e) {
            e.preventDefault();
            getFormulasFaturamentos();
        });

        abp.event.on('app.CriarOuEditarFormulaFaturamentoModalSaved', function () {
            getFormulasFaturamentos(true);
        });

        getFormulasFaturamentos();

        /* Fim Js Formula Faturamento */
    });
})();