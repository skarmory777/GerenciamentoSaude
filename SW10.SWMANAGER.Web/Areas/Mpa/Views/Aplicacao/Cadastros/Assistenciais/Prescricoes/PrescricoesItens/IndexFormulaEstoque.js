(function () {
    $(function () {
        /* Js do FormulaEstoque */
        var _$formulasEstoquesTable = $('#FormulasEstoquesTable');
        var _formulaEstoqueService = abp.services.app.formulaEstoque;
        var _estoqueService = abp.services.app.estoque;
        var _produtoService = abp.services.app.produto;
        var _unidadeService = abp.services.app.unidade;
        var _$formulaEstoquefilterForm = $('#FormulasEstoquesFilterForm');

        var _permissionsFormulaEstoque = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Delete')
        };

        //var _createOrEditFormulaEstoqueModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/PrescricoesItens/_CriarOuEditarFormulaEstoqueModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarFormulaEstoqueModal.js',
        //    modalClass: 'CriarOuEditarFormulaEstoqueModal'
        //});

        _$formulasEstoquesTable.jtable({

            title: app.localize('FormulaEstoque'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: retornarLista // _formulaEstoqueService.listar
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
                        var idGrid = data.record.idGridFormulasEstoque;
                        var $span = $('<span></span>');
                        if (_permissionsFormulaEstoque.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault()
                                    //_createOrEditFormulaEstoqueModal.open({ prescricaoItemId: data.record.prescricaoItemId, id: data.record.id, idGrid: data.record.idGridFormulasEstoque });
                                    //$('#FormulaEstoqueForm').load('/mpa/prescricoesItens/_CriarOuEditarFormulaEstoqueModal',
                                    //    {
                                    //        prescricaoItemId: $('#prescricao-item-id').val(),
                                    //        id: data.record.id,
                                    //        idGrid: data.record.idGridFormulasEstoque
                                    //    },
                                    //    function () {
                                    //        $('#id-grid-formulas-estoque').val(data.record.idGridFormulasEstoque);
                                    //        $('#estoque-origem-id').select2({
                                    //            ajax: {
                                    //                url: '/api/services/app/estoque/ResultDropdownList',
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
                                    //        $('#produto-id').select2({
                                    //            ajax: {
                                    //                url: '/api/services/app/produto/ListarProdutoDropdown',
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
                                    //        $('#unidade-requisicao-id').select2({
                                    //            ajax: {
                                    //                url: '/api/services/app/unidade/listarDropdown',
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

                                    var list = JSON.parse($('#formula-estoque-list').val());
                                    for (var i = 0; i < list.length; i++) {
                                        if (list[i].IdGridFormulasEstoque == idGrid) {
                                            var result = list[i];
                                            break;
                                        }
                                    }
                                    var $option;
                                    var $select;
                                    $('#id-grid-formulas-estoque').val(idGrid);
                                    $('#id-formula-estoque').val(result.Id);
                                    $('#codigo-formula-estoque').val(result.Codigo);
                                    $('#descricao-formula-estoque').val(result.descricao);
                                    if (result.EstoqueId != null) {
                                        _estoqueService.obter(result.EstoqueId).done(function (retorno) {
                                            $('#estoque-origem-id-formula-estoque')
                                                .append($('<option value="' + result.EstoqueId + '">' + retorno.codigo + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.EstoqueId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#estoque-origem-id-formula-estoque').val(null).change();
                                    }
                                    if (result.ProdutoId != null) {
                                        _produtoService.obter(result.ProdutoId).done(function (retorno) {
                                            $('#produto-id-formula-estoque')
                                                .append($('<option value="' + result.ProdutoId + '">' + retorno.codigo + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.ProdutoId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#produto-id-formula-estoque').val(null).change();
                                    }
                                    if (result.UnidadeId != null) {
                                        _unidadeService.obter(result.UnidadeId).done(function (retorno) {
                                            $('#unidade-requisicao-id-formula-estoque')
                                                .append($('<option value="' + result.UnidadeId + '">' + retorno.sigla + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.UnidadeId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#unidade-requisicao-id-formula-estoque').val(null).change();
                                    }
                                    if (result.IsPrincipal) {
                                        $('#is-principal-formula-estoque').attr('checked', 'checked');
                                    }
                                    else {
                                        $('#is-principal-formula-estoque').removeAttr('checked');
                                    }

                                });
                        }
                        if (_permissionsFormulaEstoque.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteFormulasEstoques(data.record);
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
                            return '<span>' + data.record.codigo + '</span>';
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

        function getFormulasEstoques(reload) {
            if (reload) {
                _$formulasEstoquesTable.jtable('reload');
            }
            else {
                _$formulasEstoquesTable.jtable('load', {
                    //filtro: $('#FormulasEstoquesTableFilter').val(),
                    prescricaoItemId: $('#prescricao-item-id').val()

                });
            }
        }

        function deleteFormulasEstoques(formulaEstoque) {
            abp.message.confirm(
                app.localize('DeleteWarning', formulaEstoque.descricao),
                function (isConfirmed) {
                    if ($('#formula-estoque-list').val() != '') {
                        lista = JSON.parse($('#formula-estoque-list').val());
                        //console.log(lista);
                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGridFormulasEstoque == formulaEstoque.idGridFormulasEstoque) {
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
                    $('#formula-estoque-list').val(JSON.stringify(lista));
                    localStorage["FormulaEstoqueList"] = JSON.stringify(lista);
                    abp.notify.info(app.localize('ListaAtualizada'));
                    getFormulasEstoques(true);
                }
            );
        }

        //function deleteFormulasEstoques(formulaEstoque) {
        //    abp.message.confirm(
        //        app.localize('DeleteWarning', formulaEstoque.descricao),
        //        function (isConfirmed) {
        //            if (isConfirmed) {
        //                _formulaEstoqueService.excluir(formulaEstoque)
        //                    .done(function () {
        //                        getFormulasEstoques(true);
        //                        abp.notify.success(app.localize('SuccessfullyDeleted'));
        //                    });
        //            }
        //        }
        //    );
        //}

        function createRequestParams() {
            var prms = {};
            _$formulaEstoquefilterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

        function retornarLista(filtro) {
            if ($('#formula-estoque-list').val() == '[]') {
                $('#formula-estoque-list').val('');
            }
            var list = $('#formula-estoque-list').val();
            if (list != '') {
                var js = list;
                var json = JSON.parse(js);
                var res = _formulaEstoqueService.listarJson(json);  //  '"{Result":"OK","Records":' + js + '}'
                return res;
            }
            else {
                var res = _formulaEstoqueService.listarPorPrescricaoItem($('#prescricao-item-id').val());
                return res;
            }
        }

        function obter(id) {
            var result = _formulaEstoqueService.obter(id);
            return result;
        }

        $('#CreateNewFormulaEstoqueButton').click(function (e) {
            e.preventDefault();
            var formulaEstoqueList = $('#formula-estoque-list').val();
            localStorage["FormulaEstoqueList"] = formulaEstoqueList;
            if (!localStorage["FormulaEstoqueList"] || (localStorage["FormulaEstoqueList"] && localStorage["FormulaEstoqueList"] == '[]')) {
                localStorage["FormulaEstoqueList"] = '';
            }
            if (localStorage["FormulaEstoqueList"] != '') {
                var lista = JSON.parse(localStorage["FormulaEstoqueList"]);
            }
            else {
                var lista = [];
            }
            $('#id-grid-formulas-estoque').val(lista.length + 1);
            $('#id-formula-estoque').val('0');
            $('#codigo-formula-estoque').val('');
            $('#descricao-formula-estoque').val('');
            $('#estoque-origem-id-formula-estoque').val(null).change();
            $('#produto-id-formula-estoque').val(null).change();
            $('#unidade-requisicao-id-formula-estoque').val(null).change();
            $('#is-principal-formula-estoque').removeAttr('checked');
            //$('#FormulaEstoqueForm').load('/mpa/prescricoesItens/_CriarOuEditarFormulaEstoqueModal',
            //    {
            //        prescricaoItemId: $('#prescricao-item-id').val(),
            //        idGrid: $('id-grid-formulas-estoque').val(),
            //    },
            //    function () {
            //        $('#estoque-origem-id-formula-estoque').select2({
            //            ajax: {
            //                url: '/api/services/app/estoque/ResultDropdownList',
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
            //        $('#produto-id-formula-estoque').select2({
            //            ajax: {
            //                url: '/api/services/app/produto/ListarProdutoDropdown',
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
            //        $('#unidade-requisicao-id-formula-estoque').select2({
            //            ajax: {
            //                url: '/api/services/app/unidade/listarDropdown',
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
            //_createOrEditFormulaEstoqueModal.open({ prescricaoItemId: $('#prescricao-item-id').val() });
        });

        $('#GetFormulasEstoquesButton, #RefreshFormulasEstoquesListButton').click(function (e) {
            e.preventDefault();
            getFormulasEstoques();
        });

        abp.event.on('app.CriarOuEditarFormulaEstoqueModalSaved', function () {
            getFormulasEstoques(true);
        });

        getFormulasEstoques();

        //código do criar/editar

    });
})();