(function () {
    $(function () {
        /* Js do FormulaEstoque */
        var _$medicosEspecialidadesTable = $('#FormulasEstoquesTable');
        var _medicoEspecialidadeService = abp.services.app.medicoEspecialidade;

        var _$medicoEspecialidadefilterForm = $('#MedicosEspecialidadesFilterForm');

        //var _permissionsFormulaEstoque = {
        //    create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Create'),
        //    edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Edit'),
        //    'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormulaEstoque.Delete')
        //};

        
        _$medicosEspecialidadesTable.jtable({

            title: app.localize('MedicoEspecialidade'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: retornarLista // _medicoEspecialidadeService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
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

        function getMedicosEspecialidades(reload) {
            if (reload) {
                _$medicosEspecialidadesTable.jtable('reload');
            }
            else {
                _$medicosEspecialidadesTable.jtable('load', {
                    //filtro: $('#FormulasEstoquesTableFilter').val(),
                    medicoEspecialidadeId: $('#medico-especialidade-id').val()

                });
            }
        }

        //function deleteFormulasEstoques(formulaEstoque) {
        //    abp.message.confirm(
        //        app.localize('DeleteWarning', formulaEstoque.descricao),
        //        function (isConfirmed) {
        //            if ($('#medico-especialidade-list').val() != '') {
        //                lista = JSON.parse($('#medico-especialidade-list').val());
        //                //console.log(lista);
        //                for (var i = 0; i < lista.length; i++) {
        //                    if (lista[i].idGridMedicosEspecialidade == formulaEstoque.idGridMedicosEspecialidade) {
        //                        if (lista[i].IsDeleted) {
        //                            lista[i].IsDeleted = false;
        //                            lista[i].DeleterUserId = '';
        //                        }
        //                        else {
        //                            lista[i].IsDeleted = true;
        //                            lista[i].DeleterUserId = abp.session.userId;
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //            $('#medico-especialidade-list').val(JSON.stringify(lista));
        //            localStorage["FormulaEstoqueList"] = JSON.stringify(lista);
        //            abp.notify.info(app.localize('ListaAtualizada'));
        //            getMedicosEspecialidades(true);
        //        }
        //    );
        //}

        function createRequestParams() {
            var prms = {};
            _$medicoEspecialidadefilterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

        function retornarLista(filtro) {
            if ($('#medico-especialidade-list').val() == '[]') {
                $('#medico-especialidade-list').val('');
            }
            var list = $('#medico-especialidade-list').val();
            if (list != '') {
                var js = list;
                var json = JSON.parse(js);
                var res = _medicoEspecialidadeService.listarJson(json);  //  '"{Result":"OK","Records":' + js + '}'
                return res;
            }
            else {
                var res = _medicoEspecialidadeService.listarPorPrescricaoItem($('#prescricao-item-id').val());
                return res;
            }
        }

        function obter(id) {
            var result = _medicoEspecialidadeService.obter(id);
            return result;
        }

        //$('#CreateNewFormulaEstoqueButton').click(function (e) {
        //    e.preventDefault();
        //    var formulaEstoqueList = $('#medico-especialidade-list').val();
        //    localStorage["FormulaEstoqueList"] = formulaEstoqueList;
        //    if (!localStorage["FormulaEstoqueList"] || (localStorage["FormulaEstoqueList"] && localStorage["FormulaEstoqueList"] == '[]')) {
        //        localStorage["FormulaEstoqueList"] = '';
        //    }
        //    if (localStorage["FormulaEstoqueList"] != '') {
        //        var lista = JSON.parse(localStorage["FormulaEstoqueList"]);
        //    }
        //    else {
        //        var lista = [];
        //    }
        //    $('#id-grid-formulas-estoque').val(lista.length + 1);
        //    $('#id-formula-estoque').val('0');
        //    $('#codigo-formula-estoque').val('');
        //    $('#descricao-formula-estoque').val('');
        //    $('#estoque-origem-id-formula-estoque').val(null).change();
        //    $('#produto-id-formula-estoque').val(null).change();
        //    $('#unidade-requisicao-id-formula-estoque').val(null).change();
        //    $('#is-principal-formula-estoque').removeAttr('checked');
        //    //$('#FormulaEstoqueForm').load('/mpa/prescricoesItens/_CriarOuEditarFormulaEstoqueModal',
        //    //    {
        //    //        prescricaoItemId: $('#prescricao-item-id').val(),
        //    //        idGrid: $('id-grid-formulas-estoque').val(),
        //    //    },
        //    //    function () {
        //    //        $('#estoque-origem-id-formula-estoque').select2({
        //    //            ajax: {
        //    //                url: '/api/services/app/estoque/ResultDropdownList',
        //    //                dataType: 'json',
        //    //                delay: 250,
        //    //                method: 'Post',
        //    //                data: function (params) {
        //    //                    if (params.page == undefined)
        //    //                        params.page = '1';
        //    //                    return {
        //    //                        search: params.term,
        //    //                        page: params.page,
        //    //                        totalPorPagina: 10
        //    //                    };
        //    //                },
        //    //                processResults: function (data, params) {
        //    //                    params.page = params.page || 1;

        //    //                    return {
        //    //                        results: data.result.items,
        //    //                        pagination: {
        //    //                            more: (params.page * 10) < data.result.totalCount
        //    //                        }
        //    //                    };
        //    //                },
        //    //                cache: true
        //    //            },
        //    //            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //    //            minimumInputLength: 0
        //    //        })
        //    //        $('#produto-id-formula-estoque').select2({
        //    //            ajax: {
        //    //                url: '/api/services/app/produto/ListarProdutoDropdown',
        //    //                dataType: 'json',
        //    //                delay: 250,
        //    //                method: 'Post',
        //    //                data: function (params) {
        //    //                    if (params.page == undefined)
        //    //                        params.page = '1';
        //    //                    return {
        //    //                        search: params.term,
        //    //                        page: params.page,
        //    //                        totalPorPagina: 10
        //    //                    };
        //    //                },
        //    //                processResults: function (data, params) {
        //    //                    params.page = params.page || 1;

        //    //                    return {
        //    //                        results: data.result.items,
        //    //                        pagination: {
        //    //                            more: (params.page * 10) < data.result.totalCount
        //    //                        }
        //    //                    };
        //    //                },
        //    //                cache: true
        //    //            },
        //    //            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //    //            minimumInputLength: 0
        //    //        })
        //    //        $('#unidade-requisicao-id-formula-estoque').select2({
        //    //            ajax: {
        //    //                url: '/api/services/app/unidade/listarDropdown',
        //    //                dataType: 'json',
        //    //                delay: 250,
        //    //                method: 'Post',
        //    //                data: function (params) {
        //    //                    if (params.page == undefined)
        //    //                        params.page = '1';
        //    //                    return {
        //    //                        search: params.term,
        //    //                        page: params.page,
        //    //                        totalPorPagina: 10
        //    //                    };
        //    //                },
        //    //                processResults: function (data, params) {
        //    //                    params.page = params.page || 1;

        //    //                    return {
        //    //                        results: data.result.items,
        //    //                        pagination: {
        //    //                            more: (params.page * 10) < data.result.totalCount
        //    //                        }
        //    //                    };
        //    //                },
        //    //                cache: true
        //    //            },
        //    //            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //    //            minimumInputLength: 0
        //    //        })
        //    //    });
        //    //_createOrEditFormulaEstoqueModal.open({ prescricaoItemId: $('#prescricao-item-id').val() });
        //});

        $('#getMedicosEspecialidadesButton, #RefreshFormulasEstoquesListButton').click(function (e) {
            e.preventDefault();
            getMedicosEspecialidades();
        });

        abp.event.on('app.CriarOuEditarFormulaEstoqueModalSaved', function () {
            getMedicosEspecialidades(true);
        });

        getMedicosEspecialidades();

        //código do criar/editar

    });
})();