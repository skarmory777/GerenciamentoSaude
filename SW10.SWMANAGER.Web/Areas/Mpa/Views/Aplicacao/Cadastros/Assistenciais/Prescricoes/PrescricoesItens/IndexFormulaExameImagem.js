﻿(function () {
    $(function () {
        /* Js Index Formula ExameImagem */
        var _$formulasExamesImagensTable = $('#FormulasExamesImagensTable');
        var _formulasFaturamentoService = abp.services.app.formulaFaturamento;
        var _fatItemService = abp.services.app.faturamentoItem;
        var _materialService = abp.services.app.material;
        var _$exameImagemfilterForm = $('#FormulasExamesImagensFilterForm');

        var _permissionsFormulaExameImagem = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameImagem.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameImagem.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameImagem.Delete')
        };

        _$formulasExamesImagensTable.jtable({

            title: app.localize('FormulaExameImagem'),
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
                        var idGrid = data.record.idGridFormulasExameImagem;
                        var $span = $('<span></span>');
                        if (_permissionsFormulaExameImagem.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault()
                                    var list = JSON.parse($('#formula-exame-imagem-list').val());
                                    for (var i = 0; i < list.length; i++) {
                                        if (list[i].IdGridFormulasExameImagem == idGrid) {
                                            var result = list[i];
                                            break;
                                        }
                                    }
                                    var $option;
                                    var $select;
                                    $('#id-grid-formulas-exame-imagem').val(idGrid);
                                    $('#id-formula-exame-imagem').val(result.Id);
                                    $('#codigo-formula-exame-imagem').val(result.Codigo);
                                    $('#descricao-formula-exame-imagem').val(result.Descricao);
                                    if (result.FaturamentoItemId != null) {
                                        _fatItemService.obter(result.FaturamentoItemId).done(function (retorno) {
                                            $('#faturamento-item-id-formula-exame-imagem')
                                                .append($('<option value="' + result.FaturamentoItemId + '">' + retorno.codigo + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.FaturamentoItemId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#faturamento-item-id-formula-exame-imagem').val(null).change();
                                    }
                                    if (result.MaterialId != null) {
                                        _materialService.obter(result.MaterialId).done(function (retorno) {
                                            $('#material-id-formula-exame-imagem')
                                                .append($('<option value="' + result.MaterialId + '">' + retorno.codigo + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.MaterialId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#material-id-formula-exame-imagem').val(null).change();
                                    }
                                    if (result.IsFatura) {
                                        $('#is-fatura-formula-exame-imagem').attr('checked', 'checked');
                                    }
                                    else {
                                        $('#is-fatura-formula-exame-imagem').removeAttr('checked');
                                    }
                                });
                        }

                        if (_permissionsFormulaExameImagem.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteFormulasExamesImagens(data.record);
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

        function getFormulasExamesImagens(reload) {
            if (reload) {
                _$formulasExamesImagensTable.jtable('reload');
            }
            else {
                _$formulasExamesImagensTable.jtable('load', {
                    filtro: $('#FormulasExamesImagensTableFilter').val(),
                    prescricaoItemId: $('#prescricao-item-id-formula-exame-imagem').val()

                });
            }
        }

        function deleteFormulasExamesImagens(formulaExameImagem) {
            abp.message.confirm(
                app.localize('DeleteWarning', formulaExameImagem.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        if ($('#formula-exame-imagem-list').val() != '') {
                            lista = JSON.parse($('#formula-exame-imagem-list').val());
                            for (var i = 0; i < lista.length; i++) {
                                if (lista[i].IdGridFormulasExameImagem == formulaExameImagem.idGridFormulasExameImagem) {
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
                        $('#formula-exame-imagem-list').val(JSON.stringify(lista));
                        localStorage["FormulaExameImagemList"] = JSON.stringify(lista);
                        abp.notify.info(app.localize('ListaAtualizada'));
                        getFormulasExamesImagens(true);
                    }
                }
            );


            //abp.message.confirm(
            //    app.localize('DeleteWarning', formulaExameImagem.descricao),
            //    function (isConfirmed) {
            //        if (isConfirmed) {
            //            _formulasFaturamentoService.excluir(formulaExameImagem)
            //                .done(function () {
            //                    getFormulasExamesImagens(true);
            //                    abp.notify.success(app.localize('SuccessfullyDeleted'));
            //                });
            //        }
            //    }
            //);
        }

        function retornarLista(filtro) {
            if ($('#formula-exame-imagem-list').val() != '') {
                var js = $('#formula-exame-imagem-list').val();
                var json = JSON.parse(js);
                var res = _formulasFaturamentoService.listarExameImagemJson(json);  //  '"{Result":"OK","Records":' + js + '}'
                return res;
            }
            else {
                var res = _formulasFaturamentoService.listarExameImagemPorPrescricaoItem({
                    id: $('#prescricao-item-id-formula-exame-imagem').val()
                });
                return res;
            }
        }

        $('#CreateNewFormulaExameImagemButton').click(function (e) {
            e.preventDefault();
            var formulaExameImagemList = $('#formula-exame-imagem-list').val();
            localStorage["FormulaExameImagemList"] = formulaExameImagemList;
            if (!localStorage["FormulaExameImagemList"] || (localStorage["FormulaExameImagemList"] && localStorage["FormulaExameImagemList"] == '[]')) {
                localStorage["FormulaExameImagemList"] = '';
            }
            if (localStorage["FormulaEstoqueList"] != '') {
                var lista = JSON.parse($('#formula-exame-imagem-list').val());
            }
            else {
                var lista = [];
            }
            $('#id-grid-formulas-exame-imagem').val(lista.length + 1);
            $('#id-formula-exame-imagem').val('0');
            $('#codigo-formula-exame-imagem').val('');
            $('#descricao-formula-exame-imagem').val('');
            $('#faturamento-item-id-formula-exame-imagem').val(null).change();
            $('#material-id-formula-exame-imagem').val(null).change();
            $('#is-fatura-formula-exame-imagem').removeAttr('checked');

            //_createOrEditFormulaExameImagemModal.open({ prescricaoItemId: $('#prescricao-item-id').val() });
            //$('#FormulaExameImagemForm').load('/mpa/prescricoesItens/_CriarOuEditarFormulaExameImagemModal',
            //    {
            //        prescricaoItemId: $('#prescricao-item-id-formula-exame-imagem').val(),
            //        idGrid: $('id-grid-formulas-exame-imagem').val(),
            //    },
            //    function () {
            //        $('#faturamento-item-id-formula-exame-imagem').select2({
            //            ajax: {
            //                url: '/api/services/app/faturamentoItem/ListarExameImagemDropdown',
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

            //        $('#material-id-formula-exame-imagem').select2({
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

        $('#ExportarFormulasExamesImagensParaExcelButton').click(function () {
            _formulasFaturamentoService
                .listarParaExcel({
                    filtro: $('#FormulasExamesImagensTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFormulasExamesImagensButton, #RefreshFormulasExamesImagensListButton').click(function (e) {
            e.preventDefault();
            getFormulasExamesImagens();
        });

        abp.event.on('app.CriarOuEditarFormulaExameImagemModalSaved', function () {
            getFormulasExamesImagens(true);
        });

        getFormulasExamesImagens();

    });
})();