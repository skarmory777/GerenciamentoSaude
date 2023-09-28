(function () {
    $(function () {

        var _$ProdutosTable = $('#ProdutosTable');
        var _ProdutosService = abp.services.app.produto;
        var _$filterForm = $('#ProdutosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.Produto.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.Produto.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.Produto.Delete')
        };

        var _ErrorModal = new app.ModalManager({ viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros' });
        _$ProdutosTable.jtable(
            {
                title: app.localize('Produtos'),
                paging: true,
                sorting: true,
                edit: false,
                create: false,
                pageSize: 10,
                multiSorting: true,
                actions: {
                    listAction: {
                        method: _ProdutosService.listar
                    }
                },

                fields: {
                    id: {
                        key: true,
                        list: false
                    },
                    actions: {
                        title: app.localize('Actions'),
                        width: '4%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');
                            if (_permissions.edit) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                    .appendTo($span)
                                    .click(function () {
                                        //_createOrEditModal.open({ id: data.record.id });
                                        location.href = 'Produtos/CriarOuEditarModal/' + data.record.id
                                    });
                            }

                            if (_permissions.delete) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                    .appendTo($span)
                                    .click(function () {
                                        deleteProdutos(data.record);
                                    });
                            }

                            return $span;
                        }
                    },
                    codigo: {
                        title: app.localize('Codigo'),
                        width: '3%',
                    },
                    isAtivo: {
                        title: app.localize('IsAtivo'),
                        width: '3%',
                        listClass: 'Centralizado',
                        display: function (data) {
                            if (data.record.isAtivo) {
                                return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                            } else {
                                return '<span class="label label-default">' + app.localize('No') + '</span>';
                            }
                        }
                    },

                    isPrincipal: {
                        title: app.localize('Principal'),
                        width: '5%',
                        listClass: 'Centralizado',
                        display: function (data) {
                            if (data.record.isPrincipal) {
                                return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                            } else {
                                return '<span class="label label-default">' + app.localize('No') + '</span>';
                            }
                        }
                    },


                    descricao: {
                        title: app.localize('Descricao'),
                        width: '20%',

                    },
                    descricaoResumida: {
                        title: app.localize('DescricaoResumida'),
                        width: '15%'
                    },
                    creationTime: {
                        title: app.localize('CreationTime'),
                        width: '5%',
                        display: function (data) {
                            return moment(data.record.creationTime).format('L');
                        }
                    },

                    DCB: {
                        title: app.localize('DCBextenso'),
                        width: '15%',
                        display: function (data) {
                            if (data.record.dcb) {
                                return data.record.dcb.descricao;
                            }
                        }
                    }

                    ,

                    Grupo: {
                        title: app.localize('Grupo'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.grupo) {
                                return data.record.grupo.descricao;
                            }
                        }
                    }
                    ,

                    Classe: {
                        title: app.localize('Classe'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.classe) {
                                return data.record.classe.descricao;
                            }
                        }
                    }
                    ,

                    SubCalsse: {
                        title: app.localize('SubClasse'),
                        width: '80%',
                        display: function (data) {
                            if (data.record.subClasse) {
                                return data.record.subClasse.descricao;
                            }
                        }
                    }
                }
            });

        function getProdutos(reload) {
            updateFilter();
            if (reload) {
                _$ProdutosTable.jtable('reload');
            } else {
                _$ProdutosTable.jtable('load', getFilter());

                //_$ProdutosTable.jtable('load', {
                //    filtro: $('#ProdutosTableFilter').val(),
                //    grupoId: $('#cbo-grupo').val(),
                //    grupoClasseId: $('#cbo-classe').val(),
                //    grupoSubClasseId: $('#cbo-subclasse').val(),
                //    DCBId: $('#cbo-dcb').val(),
                //    FiltroPrincipal: $('#cbo-filtro-principal').val(),
                //    FiltroStatus: $('#cbo-filtro-status').val()
                //});
            }
        }

        function deleteProdutos(Produto) {

            abp.message.confirm(
                app.localize('DeleteWarning', Produto.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosService.excluir(Produto.id)
                            .done(function (data) {

                                if (data.errors.length > 0) {
                                    _ErrorModal.open({ erros: data.errors });
                                }
                                else {

                                    getProdutos(true);
                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                }
                            });

                    }
                }
            );
        }

        function createRequestParams() {
            //let prms = {};
            //_$filterForm.serializeArray().map((x) => {prms[x.name] = x.value; });
            //$.extend(prms);
            return customSerializeArray(_$filterForm);
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

        $('#CreateNewProdutoButton').click(function () {
            location.href = 'Produtos/CriarOuEditarModal/';
            //_createOrEditModal.open();
        });

        $('#ExportarProdutosParaExcelButton').click(function () {
            _ProdutosService
                .listarParaExcel({
                    filtro: $('#ProdutosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#RefreshProdutossButton, #RefreshProdutosListButton').click(function (e) {
            e.preventDefault();
            getProdutos();
        });

        abp.event.on('app.CriarOuEditarProdutoModalSaved', function () {
            getProdutos(true);
        });

        $('#ProdutosTableFilter').focus();





        function loadClasses(id) {
            var myText = ''
            var textSubClasse = ''
            $.ajax({
                url: "/Mpa/Produtos/GetClasses/" + id,
                async: false,
                success: function (data) {
                    myText += '<option value="">' + app.localize('SelecioneLista') + '</option>';

                    $.each(data.Options, function (index, element) {
                        //myText += '<option value="' + element.DisplayText + '">' + element.DisplayText + ' - ' + element.Value + '</option>';
                        myText += '<option value="' + element.DisplayText + '">' + element.Value + '</option>';
                    });

                    $('#cbo-classe')
                        .empty()
                        .append(myText)
                        //.attr('required', 'required')
                        .selectpicker('refresh');
                    $('#codigo-classe').val('');

                    //limpa o combo de subclasses
                    textSubClasse = '<option value="">' + app.localize('SelecioneLista') + '</option>';
                    $('#cbo-subclasse')
                        .empty()
                        .append(textSubClasse)
                        .selectpicker('refresh');
                    $('#codigo-subclasse').val('');
                }
            });
        }

        function loadSubClasses(id) {
            var myText = ''
            $.ajax({
                url: "/Mpa/Produtos/GetSubClasses/" + id,
                async: false,
                success: function (data) {
                    myText += '<option value="">' + app.localize('SelecioneLista') + '</option>';

                    $.each(data.Options, function (index, element) {
                        //myText += '<option value="' + element.DisplayText + '">' + element.DisplayText + ' - ' + element.Value + '</option>';
                        myText += '<option value="' + element.DisplayText + '">' + element.Value + '</option>';
                    });
                    $('#cbo-subclasse')
                        .empty()
                        .append(myText)
                        .trigger("chosen:updated");

                    $('#cbo-subclasse').selectpicker('refresh');
                }
            });
        }


        function loadFields() {

            $(".select2").on("change", function () {
                updateFilter();
                getProdutos();
            });

            $('#codigo-grupo').on('blur', function () {



                if ((($('#codigo-grupo').val() == "") || ($('#codigo-grupo').val() == undefined)) && (($('#cbo-grupo').val() != "") && ($('#cbo-grupo').val() != undefined))) {
                    $('#codigo-grupo').val($('#cbo-grupo').val());
                } else {
                    if (($('#codigo-grupo').val() != "") && ($('#codigo-grupo').val() != undefined)) {
                        var exists = existeNoCombo($('#codigo-grupo').val(), '#cbo-grupo');

                        if (exists == 1) {
                            $('#cbo-grupo')
                                .val($(this).val())
                                .change()
                                .selectpicker('refresh');

                        } else {
                            $('#codigo-grupo').val($('#cbo-grupo').val());

                            abp.notify.warn("Grupo não encontrado", "");
                        };
                    };
                };
            });

            $('#codigo-classe').on('blur', function () {
                if ((($('#codigo-classe').val() == "") || ($('#codigo-classe').val() == undefined)) && (($('#cbo-classe').val() != "") && ($('#cbo-classe').val() != undefined))) {
                    $('#codigo-classe').val($('#cbo-classe').val());
                } else {
                    if (($('#codigo-classe').val() != "") && ($('#codigo-classe').val() != undefined)) {
                        var exists = existeNoCombo($('#codigo-classe').val(), '#cbo-classe');

                        if (exists == 1) {
                            $('#cbo-classe')
                                .val($(this).val())
                                .change()
                                .selectpicker('refresh');

                        } else {
                            $('#codigo-classe').val($('#cbo-classe').val());

                            abp.notify.warn("Classe não encontrada", "");
                        };
                    };

                };

            });

            $('#codigo-subclasse').on('blur', function () {
                if ((($('#codigo-subclasse').val() == "") || ($('#codigo-subclasse').val() == undefined)) && (($('#cbo-subclasse').val() != "") && ($('#cbo-subclasse').val() != undefined))) {
                    $('#codigo-subclasse').val($('#cbo-subclasse').val());
                } else {
                    if (($('#codigo-subclasse').val() != "") && ($('#codigo-subclasse').val() != undefined)) {
                        var exists = existeNoCombo($('#codigo-subclasse').val(), '#cbo-subclasse');

                        if (exists == 1) {
                            $('#cbo-subclasse')
                                .val($(this).val())
                                .change()
                                .selectpicker('refresh');

                        } else {
                            $('#codigo-subclasse').val($('#cbo-subclasse').val());

                            abp.notify.warn("SubClasse não encontrada", "");
                        };
                    };

                };

            });
            $('.selectpicker').on('change', function (e) {
                e.preventDefault();

                //unidadereferencia
                //-----------------------------------------------------------
                if ($(this).attr('id') == 'cbo-unidade-referencial') {

                    //Pega a sigla da unidade
                    var sigla = "";
                    var idUnidade = $(this).val();

                    if (idUnidade == null || idUnidade == "") {
                        idUnidade = 0;
                    };

                    _unidadeService.getSiglaUnidadePeloId(idUnidade)
                        .done(function (data) {
                            //carga no combo de unidades gerenciais
                            loadUnidades(idUnidade);
                            $('#codigo-unidade-gerencial').val("");

                            sigla = data;
                            $('#codigo-unidade-referencial').val(sigla);
                        });
                }

                //unidadeGerencial
                //-----------------------------------------------------------
                if ($(this).attr('id') == 'cbo-unidade-gerencial') {

                    //Pega a sigla da unidade
                    var sigla = "";
                    var idUnidade = $(this).val();

                    if (idUnidade == null || idUnidade == "") {
                        idUnidade = 0;
                    };

                    _unidadeService.getSiglaUnidadePeloId(idUnidade)
                        .done(function (data) {
                            sigla = data;
                            $('#codigo-unidade-gerencial').val(sigla);
                        });
                }

                //grupo
                //-----------------------------------------------------------
                if ($(this).attr('id') == 'cbo-grupo') {
                    loadClasses($(this).val());
                    $('#codigo-grupo').val($('#cbo-grupo').val());
                }

                //classe
                //-----------------------------------------------------------
                if ($(this).attr('id') == 'cbo-classe') {
                    loadSubClasses($(this).val());
                    $('#codigo-classe').val($('#cbo-classe').val());
                }

                updateFilter();
                getProdutos();
            });

            //$(".select2Dcb").select2({
            //    allowClear: true,
            //    placeholder: "Informe um DCB",
            //    ajax: {
            //        url: "/api/services/app/produto/ListarDcbDropdown",
            //        dataType: 'json',
            //        delay: 250,
            //        method: 'Post',

            //        data: function (params) {

            //            //   //console.log('data: ', params, (params.page == undefined));
            //            if (params.page == undefined)
            //                params.page = '1';
            //            //   //console.log('data: ', params);
            //            return {
            //                search: params.term,
            //                page: params.page,
            //                totalPorPagina: 10
            //            };
            //        },
            //        processResults: function (data, params) {
            //            params.page = params.page || 1;
            //            return {
            //                results: data.result.items,
            //                pagination: {
            //                    more: (params.page * 10) < data.result.totalCount
            //                }
            //            };
            //        },
            //        cache: true
            //    },
            //    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            //    minimumInputLength: 1
            //});
            selectSWWithDefaultValue('.select2Dcb', "/api/services/app/produto/ListarDcbDropdown", undefined, { minimumInputLength: 1 });
            selectSWWithDefaultValue('.selectGrupo', "/api/services/app/Grupo/ListarDropdown");
            selectSWWithDefaultValue('.selectClasse', "/api/services/app/GrupoClasse/ListarDropdown", $('#grupoId'));
            selectSWWithDefaultValue('.selectSubClasse', "/api/services/app/GrupoSubClasse/ListarDropdown", $('#grupoClasseId'));

            $('#grupoId').on('change', function (e) {
                e.preventDefault();
                selectSWWithDefaultValue('.selectClasse', "/api/services/app/GrupoClasse/ListarDropdown", $('#grupoId'));
            });

            $('#grupoClasseId').on('change', function (e) {
                e.preventDefault();
                selectSWWithDefaultValue('.selectSubClasse', "/api/services/app/GrupoSubClasse/ListarDropdown", $('#grupoClasseId'));
            });
        }

        function updateFilter() {
            sessionStorage.setItem("produtoIndexFilter", JSON.stringify(createRequestParams()));
        }

        function getFilter() {
            return JSON.parse(sessionStorage.getItem("produtoIndexFilter"));
        }

        function loadFilter() {
            if (!sessionStorage.hasOwnProperty("produtoIndexFilter")) {
                updateFilter();
            }

            var filter = getFilter();

            $.each(filter, (index, value) => {
                let el = $(`[name="${index}"]`);
                if (el.data("select2") != undefined) {
                    $(`[name="${index}"]`).trigger("select2:selectById", value);
                }
                else {
                    $(`[name="${index}"]`).val(value);
                }
            });

            //getProdutos();
        }

        function customSerializeArray(form) {
            var array = {};
            const $form = $(form);
            $form.find("input, select").each((index, item) => {
                let jItem = $(item);
                if (!jItem.attr("name")) {
                    return;
                }
                array[`${jItem.attr("name")}`] = jItem.val();
            });
            return array;
        }


        loadFields();
        loadFilter();

        setTimeout(() => {
            getProdutos()
        }, 0);
        

    });
})();