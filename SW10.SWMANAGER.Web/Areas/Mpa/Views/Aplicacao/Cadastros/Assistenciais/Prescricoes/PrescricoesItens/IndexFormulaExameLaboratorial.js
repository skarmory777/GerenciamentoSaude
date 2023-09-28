(function () {
    $(function () {
        /* Js Index Formula ExameLaboratorial */
        var _$formulasExamesLaboratoriaisTable = $('#FormulasExamesLaboratoriaisTable');
        var _formulasFaturamentoService = abp.services.app.formulaFaturamento;
        var _fatItemService = abp.services.app.faturamentoItem;
        var _materialService = abp.services.app.material;
        var _$exameLaboratorialfilterForm = $('#FormulasExamesLaboratoriaisFilterForm');

        var _permissionsFormulaExameLaboratorial = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameLaboratorial.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameLaboratorial.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaExameLaboratorial.Delete')
        };

        _$formulasExamesLaboratoriaisTable.jtable({

            title: app.localize('FormulaExameLaboratorial'),
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
                        var idGrid = data.record.idGridFormulasExameLaboratorial;
                        var $span = $('<span></span>');
                        if (_permissionsFormulaExameLaboratorial.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault()
                                    var list = JSON.parse($('#formula-exame-laboratorial-list').val());
                                    for (var i = 0; i < list.length; i++) {
                                        if (list[i].IdGridFormulasExameLaboratorial == idGrid) {
                                            var result = list[i];
                                            break;
                                        }
                                    }
                                    var $option;
                                    var $select;
                                    $('#id-grid-formulas-exame-laboratorial').val(idGrid);
                                    $('#id-formula-exame-laboratorial').val(result.Id);
                                    $('#codigo-formula-exame-laboratorial').val(result.Codigo);
                                    $('#descricao-formula-exame-laboratorial').val(result.Descricao);
                                    if (result.FaturamentoItemId != null) {
                                        _fatItemService.obter(result.FaturamentoItemId).done(function (retorno) {
                                            $('#faturamento-item-id-formula-exame-laboratorial')
                                                .append($('<option value="' + result.FaturamentoItemId + '">' + retorno.codigo + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.FaturamentoItemId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#faturamento-item-id-formula-exame-laboratorial').val(null).change();
                                    }
                                    if (result.MaterialId != null) {
                                        _materialService.obter(result.MaterialId).done(function (retorno) {
                                            $('#material-id-formula-exame-laboratorial')
                                                .append($('<option value="' + result.MaterialId + '">' + retorno.codigo + ' - ' + retorno.descricao + '</option>'))
                                                .val(result.MaterialId)
                                                .trigger('change');
                                        });
                                    }
                                    else {
                                        $('#material-id-formula-exame-laboratorial').val(null).change();
                                    }
                                    if (result.IsFatura) {
                                        $('#is-fatura-formula-exame-laboratorial').attr('checked', 'checked');
                                    }
                                    else {
                                        $('#is-fatura-formula-exame-laboratorial').removeAttr('checked');
                                    }
                                });
                        }
                        if (_permissionsFormulaExameLaboratorial.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteFormulasExamesLaboratoriais(data.record);
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

        function getFormulasExamesLaboratoriais(reload) {
            if (reload) {
                _$formulasExamesLaboratoriaisTable.jtable('reload');
            }
            else {
                _$formulasExamesLaboratoriaisTable.jtable('load', {
                    //filtro: $('#FormulasExamesLaboratoriaisTableFilter').val(),
                    prescricaoItemId: $('#prescricao-item-id-formula-exame-laboratorial').val()

                });
            }
        }

        function deleteFormulasExamesLaboratoriais(formulaExameLaboratorial) {
            abp.message.confirm(
                app.localize('DeleteWarning', formulaExameLaboratorial.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        if ($('#formula-exame-laboratorial-list').val() != '') {
                            lista = JSON.parse($('#formula-exame-laboratorial-list').val());
                            for (var i = 0; i < lista.length; i++) {
                                if (lista[i].IdGridFormulasExameLaboratorial == formulaExameLaboratorial.idGridFormulasExameLaboratorial) {
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
                        $('#formula-exame-laboratorial-list').val(JSON.stringify(lista));
                        localStorage["FormulaExameLaboratorialList"] = JSON.stringify(lista);
                        abp.notify.info(app.localize('ListaAtualizada'));
                        getFormulasExamesLaboratoriais(true);
                    }
                }
            );


            //abp.message.confirm(
            //    app.localize('DeleteWarning', formulaExameLaboratorial.descricao),
            //    function (isConfirmed) {
            //        if (isConfirmed) {
            //            _formulasFaturamentoService.excluir(formulaExameLaboratorial)
            //                .done(function () {
            //                    getFormulasExamesLaboratoriais(true);
            //                    abp.notify.success(app.localize('SuccessfullyDeleted'));
            //                });
            //        }
            //    }
            //);
        }

        function retornarLista(filtro) {
            if ($('#formula-exame-laboratorial-list').val() != '') {
                var js = $('#formula-exame-laboratorial-list').val();
                var json = JSON.parse(js);
                var res = _formulasFaturamentoService.listarExameLaboratorialJson(json);  //  '"{Result":"OK","Records":' + js + '}'
                return res;
            }
            else {
                var res = _formulasFaturamentoService.listarExameLaboratorialPorPrescricaoItem({
                    //filtro: $('#FormulasExamesLaboratoriaisTableFilter').val(),
                    id: $('#prescricao-item-id-formula-exame-laboratorial').val()
                });
                return res;
            }
        }

        $('#CreateNewFormulaExameLaboratorialButton').click(function (e) {
            e.preventDefault();
            var formulaExameLaboratorialList = $('#formula-exame-laboratorial-list').val();
            localStorage["FormulaExameLaboratorialList"] = formulaExameLaboratorialList;
            if (!localStorage["FormulaExameLaboratorialList"] || (localStorage["FormulaExameLaboratorialList"] && localStorage["FormulaExameLaboratorialList"] == '[]')) {
                localStorage["FormulaExameLaboratorialList"] = '';
            }
            if (localStorage["FormulaEstoqueList"] != '') {
                var lista = JSON.parse($('#formula-exame-laboratorial-list').val());
            }
            else {
                var lista = [];
            }
            $('#id-grid-formulas-exame-laboratorial').val(lista.length + 1);
            $('#id-formula-exame-laboratorial').val('0');
            $('#codigo-formula-exame-laboratorial').val('');
            $('#descricao-formula-exame-laboratorial').val('');
            $('#faturamento-item-id-formula-exame-laboratorial').val(null).change();
            $('#material-id-formula-exame-laboratorial').val(null).change();
            $('#is-fatura-formula-exame-laboratorial').removeAttr('checked');

            //_createOrEditFormulaExameLaboratorialModal.open({ prescricaoItemId: $('#prescricao-item-id').val() });
            //$('#FormulaExameLaboratorialForm').load('/mpa/prescricoesItens/_CriarOuEditarFormulaExameLaboratorialModal',
            //    {
            //        prescricaoItemId: $('#prescricao-item-id-formula-exame-laboratorial').val(),
            //        idGrid: $('id-grid-formulas-exame-laboratorial').val(),
            //    },
            //    function () {
            //        $('#faturamento-item-id-formula-exame-laboratorial').select2({
            //            ajax: {
            //                url: '/api/services/app/faturamentoItem/ListarExameLaboratorialDropdown',
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
            //        $('#material-id-formula-exame-laboratorial').select2({
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

        $('#ExportarFormulasExamesLaboratoriaisParaExcelButton').click(function () {
            _formulasFaturamentoService
                .listarParaExcel({
                    //filtro: $('#FormulasExamesLaboratoriaisTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFormulasExamesLaboratoriaisButton, #RefreshFormulasExamesLaboratoriaisListButton').click(function (e) {
            e.preventDefault();
            getFormulasExamesLaboratoriais();
        });

        abp.event.on('app.CriarOuEditarFormulaExameLaboratorialModalSaved', function () {
            getFormulasExamesLaboratoriais(true);
        });

        getFormulasExamesLaboratoriais();

        /* Fim Js Formula ExameLaboratorial */
    });
})();