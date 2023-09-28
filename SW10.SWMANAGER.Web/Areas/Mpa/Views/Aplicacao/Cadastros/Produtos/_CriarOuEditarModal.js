(function () {
    $(function () {

        //Vars
        //------------------------------------------------------------------------------------------------------------------------
        var _modalManager;

        //Formulários
        //------------------------------------------------------------------------------------------------------------------------
        var _$produtosInformationForm = $('#ProdutoInformationsForm');

        //Serviços
        //------------------------------------------------------------------------------------------------------------------------
        var _produtosService = abp.services.app.produto;
        var _unidadeService = abp.services.app.unidade;

        //------------------------------------------------------------------------------------------------------------------------
        //seleciona o item do combo de acordo com o valor da caixa de edicao de codigo
        function existeNoCombo(edit, combo) {
            var thevalue = edit;

            var exists = $(combo + ' option').filter(function () {
                return $(this).val() == thevalue;
            }).length;

            return exists;
        };

        function cargaSelect2(elemento) {
            elemento.select2({
                allowClear: true,
                placeholder: app.localize("SelecioneLista"),
                ajax: {
                    url: "/api/services/app/produto/ListarDcbDropdown",
                    dataType: 'json',
                    delay: 250,
                    method: 'Post',
                    data: function (params) {
                        //   //console.log('data: ', params, (params.page == undefined));
                        if (params.page == undefined)
                            params.page = '1';
                        //   //console.log('data: ', params);
                        return {
                            search: params.term,
                            page: params.page,
                            totalPorPagina: 10
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;

                        return {
                            results: data.result.items,
                            pagination: {
                                more: (params.page * 10) < data.result.totalCount
                            }
                        };
                    },
                    cache: true
                },
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                minimumInputLength: 1
            });

        };

        $(document).ready(function () {

            _produtosService = abp.services.app.produto;

            //------------------------------------------------------------------------------
            cargaSelect2($(".select2Dcb"));

            $('.selectpicker').selectpicker('refresh');

            $('#genero-id').val(3).change().selectpicker('refresh');

            //Identifica os campos requeridos 
            //CamposRequeridos();   //--> aguardando ajustes na função

            /*
            Fazendo a verificação se o campo medicamento está selecionado
            */
            $('#chk-is-medicamento').on('click', function () {

                var check = $(this).is(':checked');
                var str = '<div class="form-group">';

                if (check) {
                    str += '<input name="IsAtivo" id="chk-is-produto-ativo" type="checkbox" class="form-control icheck bloquearChecks checkbox-inline" value="true" />';
                    str += '<label for="IsAtivo">&nbsp;' + app.localize("MedicamentoControlado") + '</label>';
                    str += '</div>';
                    $('#content-medicamento-controlado').html(str).click();

                } else {
                    str += '<input name="IsAtivo" id="chk-is-produto-ativo" type="checkbox" class="form-control icheck bloquearChecks checkbox-inline" value="false" disabled />';
                    str += '<label for="IsAtivo">&nbsp;' + app.localize("MedicamentoControlado") + '</label>';
                    str += '</div>';
                    $('#content-medicamento-controlado').html(str);
                };
            });

            var temValor = $('#is-medicamento-controlado').val() != "" && $('#is-medicamento-controlado').val() != undefined;

            $('#chk-is-medicamento-controlado').attr('checked', temValor);

            //Define o label do botao Salvar dependendo se é um novo produto ou uma ediçao
            if ($('#id').val() == 0) {
                //se inclusao
                $('.save-button').html('<i class="fa fa-save"></i> Salvar');

                //Bloqueio das Abas após gravacao do produto
                $('#href_unidade').hide();
                $('#href_laboratorio').hide();
                $('#href_empresa').hide();
                $('#href_estoque').hide();
                $('#href_compra').hide();
                $('#href_saldo').hide();
                $('#href_portaria').hide();
            }
            else {

                if ($('#produtoPrincipalId').val() != null && $('#produtoPrincipalId').val() != "" && $('#produtoPrincipalId').val() != undefined) {
                    bloquearControles(true);
                };

                $('#href_laboratorio').hide();
                $('.save-button').html('<i class="fa fa-save"></i> Salvar');
            }

            //changes dos combos
            //------------------------------------------------------------------------------------------------------------------------
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

                    _unidadeService.getSiglaUnidadePeloId(idUnidade, { async: false, cache: false })
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
            });

            

            $('#cbo-dcb').on('change', function () {
                $('#codigo-dcb').val($('#cbo-dcb').val());
            });

            $('#cbo-classe').on('change', function () {
                $('#codigo-classe').val($('#cbo-classe').val());
            });

            $('#cbo-subclasse').on('change', function () {
                $('#codigo-subclasse').val($('#cbo-subclasse').val());
            })

            $('#codigo-produto-principal').on('blur', function () {

                debugger;


                if ((($('#codigo-produto-principal').val() == "") || ($('#codigo-produto-principal').val() == undefined)) && (($('#cbo-produto-principal').val() != "") && ($('#cbo-produto-principal').val() != undefined))) {
                    $('#codigo-produto-principal').val($('#cbo-produto-principal').val());
                } else {
                    if (($('#codigo-produto-principal').val() != "") && ($('#codigo-produto-principal').val() != undefined)) {
                        //var exists = existeNoCombo('#codigo-produto-principal', '#cbo-produto-principal');
                        var exists = existeNoCombo($('#codigo-produto-principal').val(), '#cbo-produto-principal');

                        if (exists == 1) {
                            $('#cbo-produto-principal')
                                .val($(this).val())
                                .change()
                                .selectpicker('refresh');

                        } else {
                            $('#codigo-produto-principal').val($('#cbo-produto-principal').val());

                            abp.notify.warn("Produto Mestre não encontrado", "");
                        };
                    };
                };
            });

            $('#codigo-dcb').on('blur', function () {
                if ((($('#codigo-dcb').val() == "") || ($('#codigo-dcb').val() == undefined)) && (($('#cbo-dcb').val() != "") && ($('#cbo-dcb').val() != undefined))) {
                    $('#codigo-dcb').val($('#cbo-dcb').val());
                } else {
                    if (($('#codigo-dcb').val() != "") && ($('#codigo-dcb').val() != undefined)) {
                        var exists = existeNoCombo($('#codigo-dcb').val(), '#cbo-dcb');

                        if (exists == 1) {
                            $('#cbo-dcb')
                                .val($(this).val())
                                .change()
                                .selectpicker('refresh');

                        } else {
                            $('#codigo-dcb').val($('#cbo-dcb').val());

                            abp.notify.warn("Código DCB não encontrado", "");
                        };
                    };
                };
            });

            $('#codigo-unidade-referencial').on('blur', function () {
                if ((($('#codigo-unidade-referencial').val() == "") || ($('#codigo-unidade-referencial').val() == undefined)) && (($('#cbo-unidade-referencial').val() != "") && ($('#cbo-unidade-referencial').val() != undefined))) {

                    //--------------------------------------------------
                    var sigla = "";
                    var codigo = $('#cbo-unidade-referencial').val();

                    if (codigo == "") {
                        $('#cbo-unidade-referencial').val("");
                    } else {
                        _unidadeService.getSiglaUnidadePeloId(codigo)
                            .done(function (data) {
                                sigla = data;
                                $('#codigo-unidade-referencial').val(sigla);
                            });
                    };
                    //--------------------------------------------------

                    //$('#codigo-unidade-referencial').val($('#cbo-unidade-referencial').val());

                } else {
                    if (($('#codigo-unidade-referencial').val() != "") && ($('#codigo-unidade-referencial').val() != undefined)) {
                        //retorna e armazena o id da unidade referencia
                        var idUnidade = 0;
                        _unidadeService.getIdUnidadelPorSigla($('#codigo-unidade-referencial').val(), true)
                            .done(function (data) {
                                //----------------------------------------
                                //procura no combo o option que tem o valor passado(id da unidade)
                                idUnidade = data;
                                var exists = existeNoCombo(idUnidade, '#cbo-unidade-referencial');
                                //if (exists != null) {
                                if (exists != 0) {
                                    $('#cbo-unidade-referencial')
                                        //.val($(this).val())
                                        .val(idUnidade)
                                        .change()
                                        .selectpicker('refresh');
                                } else {
                                    var sigla = "";
                                    var codigo = $('#cbo-unidade-referencial').val();

                                    if (codigo == "") {
                                        $('#codigo-unidade-referencial').val("");
                                    } else {
                                        _unidadeService.getSiglaUnidadePeloId(codigo)
                                            .done(function (data) {
                                                sigla = data;
                                                $('#codigo-unidade-referencial').val(sigla);
                                            });
                                    };

                                    abp.notify.warn("Unidade Referência não encontrada", "");
                                };
                            });
                    };
                };
            });

            $('#codigo-unidade-gerencial').on('blur', function () {

                var cod = $(this).val();
                //console.log(cod);

                if ((($('#codigo-unidade-gerencial').val() == "") || ($('#codigo-unidade-gerencial').val() == undefined)) && (($('#cbo-unidade-gerencial').val() != "") && ($('#cbo-unidade-gerencial').val() != undefined))) {
                    $('#codigo-unidade-gerencial').val($('#cbo-unidade-gerencial').val());
                } else {
                    if (($('#codigo-unidade-gerencial').val() != "") && ($('#codigo-unidade-gerencial').val() != undefined)) {

                        //retorna e armazena o id da unidade referencia
                        var idUnidade = 0;
                        _unidadeService.getIdUnidadelPorSigla($('#codigo-unidade-gerencial').val(), false, $('#cbo-unidade-referencial').val())
                            .done(function (data) {
                                //----------------------------------------
                                //procura no combo o option que tem o valor passado(id da unidade)
                                idUnidade = data;
                                var exists = existeNoCombo(idUnidade, '#cbo-unidade-gerencial');
                                if (exists != 0) {
                                    $('#cbo-unidade-gerencial')
                                        .val(idUnidade)
                                        .change()
                                        .selectpicker('refresh');
                                } else {
                                    //$('#codigo-grupo').val($('#cbo-grupo').val());

                                    var sigla = "";
                                    var codigo = $('#cbo-unidade-gerencial').val();

                                    if (codigo == "") {
                                        $('#codigo-unidade-gerencial').val("");
                                    } else {
                                        _unidadeService.getSiglaUnidadePeloId(codigo)
                                            .done(function (data) {
                                                sigla = data;

                                                //console.log(JSON.stringify(data));

                                                $('#codigo-unidade-gerencial').val(sigla);
                                            });
                                    };

                                    abp.notify.warn("Unidade Gerencial não encontrada", "");
                                };
                            });
                    };

                };

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

            //$('ul.ui-autocomplete').css('z-index', '2147483647');

            CamposRequeridos();
        });

        //preenche a caixa de edicao de codigo com o valor do combo
        $('#produtoMestreId').on('change', function () {

            debugger;
            //   $('#codigo-produto-principal').val($('#cbo-produto-principal').val());

            //se for novo produto, copiar dados do mestre para novo produto
            if (!($('#creatorUserId').val() > 0) && $('#produtoMestreId').val() != null && $('#produtoMestreId').val() != '') {
                _produtosService.obter($('#produtoMestreId').val())
                    .done(function (data) {
                        cargaProduto(data);
                    });
            } else if (!($('#creatorUserId').val() > 0) && ($('#produtoMestreId').val() == null)) {
                desbloquearControles();
            };

            //abp.notify.warn("Produto Mestre não encontrado", "");
        });

        //Gravacao do Produto
        //------------------------------------------------------------------------------------------------------------------------

        function _save() {

            $("#EstoqueLocalizacaoId").val(3);
            if ($('#cbo-classe').val() == "") {
                $('#cbo-classe').val(null);
            };

            if ($('#cbo-subclasse').val() == "") {
                $('#cbo-subclasse').val(null);
            };

            _$produtoInformationsForm = $('form[name=ProdutoInformationsForm]');
            _$produtoInformationsForm.validate();

            if (!_$produtoInformationsForm.valid()) {
                abp.notify.error(app.localize('ErroSalvar'));
                return
            }
            else {

                if ($('#is-prescricao-item').is(':checked') && ($('#divisao-id').val() == '' || $('#divisao-id').val() == null)) {
                    abp.notify.error(app.localize('InformarDivisao'));
                    $('#href_prescricao_item').trigger('click');
                    return
                }
                var produto = _$produtoInformationsForm.serializeFormToObject();

                _produtosService.criarGetId(produto)
                .done(function (data) {
                    //    //console.log(JSON.stringify(data));
                    $('.save-button').html('<i class="fa fa-save"></i> Salvar');

                    //abp.notify.info(app.localize('SavedSuccessfully'));

                    abp.notify.info(app.localize('Produto ' + $('#descricao').val() + ' salvo com sucesso.'));
                    window.location.href = "/Mpa/Produtos";
                    //    .delay(3400);

                    //abp.message.success(
                    //    'Produto ' + data.descricao + ' salvo com sucesso.\nContinue o cadastro se desejar',
                    //    app.localize('CreateNewProduto'));
                    //var prescricaoItem = $('form[name=PrescricaoItemInformationsForm]').serializeFormToObject();
                    //prescricaoItem.ProdutoId = data.id;
                    //var _prescricaoService = abp.services.app.prescricaoItem;
                    //_prescricaoService.criarOuEditar(prescricaoItem)
                    //.done(function (prescricaoItemData) {
                    //    window.setTimeout(function () {
                    //        location.href = '/mpa/produtos/CriarOuEditarModal/' + data.id
                    //    }, 2000);
                    //});

                    //  abp.event.trigger('app.CriarOuEditarAgendamentoConsultaModalSaved');
                    //////////////////////$('#id').val(data.id);
                    //////////////////////$('#codigo-produto').val(data.id);
                    //////////////////////$('#Numero').val(data.numero);

                    //////////////////////////Liberacao das Abas após gravacao do produto
                    //////////////////////$('#href_unidade').css('display', 'block');
                    //////////////////////$('#href_empresa').css('display', 'block');
                    //////////////////////$('#href_estoque').css('display', 'block');
                    //////////////////////$('#href_compra').css('display', 'block');

                    //$('#exTab3').show();
                    //$('#extTab4').hide();

                    //var unidadeId = $('#unidadeReferencia-id').val();
                    //var produtoId = $('#id').val();
                    //if (unidadeId != '') {
                    //    $.ajax({
                    //        url: "/mpa/produtos/InserirUnidadeReferencia?produtoId=" + produtoId + '&unidadeId=' + unidadeId + '&tipoUnidade=' + 1,
                    //        success: function (data) {
                    //            getProdutoUnidadeTipo();
                    //        }
                    //    });
                    //}
                    //var unidadeGerencialId = $('#unidadeGerencial-id').val();
                    //if (unidadeGerencialId != '') {
                    //    $.ajax({
                    //        url: "/mpa/produtos/InserirUnidadeReferencia?produtoId=" + produtoId + '&unidadeId=' + unidadeGerencialId + '&tipoUnidade=' + 2,
                    //        success: function (data) {
                    //            getProdutoUnidadeTipo();
                    //        }
                    //    });
                    //}

                })
                .always(function () {
                    $('#proximo-button').buttonBusy(false);
                });
            }
        }

        function salvar() {
            if ($('#is-prescricao-item').is(':checked') && ($('#divisao-id').val() == '' || $('#divisao-id').val() == null)) {
                abp.notify.error(app.localize('InformarDivisao'));
                $('#href_prescricao_item').trigger('click');
                return
            }

            $("#estoque-localizacao-id").val(2);

            if (($('#chk-is-medicamento').is(':checked')) && ($('#chk-is-medicamento-controlado').is(':checked'))) {
                $('#is-medicamento-controlado').val(true);

            } else {
                $('#is-medicamento-controlado').val(false);
            };

            var produtos = $('form[name=ProdutoInformationsForm]').serializeFormToObject();

            $(this).buttonBusy(true);

            _produtosService.criarOuEditar(produtos)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     abp.event.trigger('app.CriarOuEditarProdutoModalSaved');
                     if ($("#is-prescricao-item").prop("checked")) {
                         var prescricaoItem = $('form[name=PrescricaoItemInformationsForm]').serializeFormToObject();
                         prescricaoItem.ProdutoId = produtos.Id;
                         prescricaoItem.FaturamentoItemId = produtos.FaturamentoItemId;
                         var _prescricaoService = abp.services.app.prescricaoItem;
                         _prescricaoService.criarOuEditar(prescricaoItem)
                             .done(() => {
                                 window.setTimeout(() => {
                                     location.href = '/mpa/produtos'; //CriarOuEditarModal/' + data.id
                                 }, 500);
                             });
                     }
                     else {
                         location.href = '/mpa/produtos';
                     }

                     //location.href = '/mpa/produtos';
                 })
                .always(function () {
                    $(this).buttonBusy(false);
                });
        }

        //------------------------------------------------------------------------------------------------------------------------

        function AutoCompletar(idSearch, idCampo, url, cadastro, idPai) {
            var search = '#' + idSearch;
            var campo = '#' + idCampo;
            var campoPai = '#' + idPai;

            $(search)
            .autocomplete({
                minLength: 1,
                delay: 0,
                source: function (request, response) {
                    var term = $(search).val();
                    var fullUrl = url + '/?term=' + term + '&id=' + $(campoPai).val();
                    $.getJSON(fullUrl, function (data) {
                        if (data.length == 0) {
                            $(campo).val(0);
                            $(search).focus();
                            abp.notify.info(app.localize("ListaVazia"));
                            return false;
                        };
                        response($.map(data, function (item) {
                            $(campo).val(0);
                            return {
                                label: item.Nome,
                                value: item.Nome,
                                realValue: item.Id,

                            };
                        }));
                    });
                },
                select: function (event, ui) {
                    $(campo).val(ui.item.realValue);
                    //alert($(campo).val());
                    if (campo == "#cbo-grupo") {
                        $('#grupo-cbo-classe').val("");
                        $('#classe-search').val("");

                        $('#grupo-subClasse-id').val("");
                        $('#subClasse-search').val("");
                    } else if (campo == "#cbo-classe") {
                        $('#grupo-subClasse-id').val("");
                        $('#subClasse-search').val("");
                    }
                    $(search).val(ui.item.value);
                    //$('.save-button').removeAttr('disabled');
                    return false;
                },
                change: function (event, ui) {
                    event.preventDefault();
                    if (ui.item == null) {
                        //$('.save-button').attr('disabled', 'disabled');
                        $(campo).val(0);
                        $(search).val('').focus();
                        abp.notify.info(app.localize("AutoConpletInvalido").replace('$cadastro', cadastro));
                        return false;
                    }
                },
            });
        }

        function loadUnidades(id) {
            //function loadUnidades(sigla) {
            var myText = ''
            $.ajax({
                //url: "/Mpa/Produtos/GetUnidadesPorReferencia/" + id,
                url: "/Mpa/Produtos/GetUnidadesPorReferencia/?id=" + id + "&addPai=true",
                //url: "/Mpa/Produtos/GetUnidadesPorReferencia/?sigla=" + sigla,
                async: false,
                cache: false,
                success: function (data) {
                    myText += '<option value="">' + app.localize('SelecioneLista') + '</option>';

                    $.each(data.Options, function (index, element) {
                        myText += '<option value="' + element.DisplayText + '">' + element.Value + '</option>';
                    });
                    $('#cbo-unidade-gerencial')
                        .empty()
                        .append(myText)
                        .selectpicker('refresh');
                    //$('#cbo-unidade-gerencial').val('');
                    $('#cbo-unidade-gerencial').selectpicker('refresh');
                }
            });
        }

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

        //------------------------------------------------------------------------------------------------------------------------

        manipulacaoProdutoPrincipal($('#chk-is-principal'));

        function manipulacaoProdutoPrincipal(objCheck) {

            if ($(objCheck).is(':checked')) {
                $('#codigo-produto-principal').val("");
                $('#codigo-produto-principal').attr('Disabled', true);

                $("#cbo-produto-principal").prop('selectedIndex', 0);
                $("#cbo-produto-principal").val();

                $('#cbo-produto-principal').attr('disabled', true)
                $('#cbo-produto-principal').selectpicker('refresh');

            } else {
                $('#cbo-produto-principal').attr('Disabled', false);
                $('#cbo-produto-principal').selectpicker('refresh');

                $('#codigo-produto-principal').attr('Disabled', false);
            }

            $('.selectpicker').selectpicker('refresh');
        };

        //checkbox principal
        $('#chk-is-principal').on('change', function (e) {
            if (($('#id').val() == 0) && ($('#cbo-produto-principal').val() != 0)) {
                limparControles();
            };

            manipulacaoProdutoPrincipal($('#chk-is-principal'));

            //if ($(objCheck).is(':checked')) {
            //}

            //limparControles();
        });

        function manipulacaoMedicamentoControlado(objCheck) {
            //$('#chk-is-medicamento-controlado').attr('Disabled', !$(objCheck).is(':checked'));
            $('#chk-is-medicamento-controlado').attr('Disabled', !$('#chk-is-medicamento').is(':checked'));

            if (!$('#chk-is-medicamento').is(':checked')) {
                $('#chk-is-medicamento-controlado').attr('checked', false);
            };

            //if ($('#chk-is-medicamento').is(':checked')) {
            //    habilitaClique();
            //}
            //else {
            //    desabilitaClique();
            //}
        };

        manipulacaoMedicamentoControlado();

        function manipulacaoEtiqueta(objCheck) {
            $("#etiqueta-id").prop('selectedIndex', 0);
            $('#etiqueta-id').attr('Disabled', !$(objCheck).is(':checked'));
            $('#etiqueta-id').selectpicker('refresh');
        };

        $('#chk-is-etiqueta').on('change', function (e) {
            manipulacaoEtiqueta("#chk-is-etiqueta");
        });

        $('.close').on('click', function () {
            location.href = '/mpa/produtos';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/produtos';
        });

        $('.save-button').on('click', function (e) {
            e.preventDefault();
            $('#form-produto').submit();
        });

        $('#form-produto').on('submit', function (e) {
            e.preventDefault();
            if ($('#id').val() == 0) {
                _save();
            }
            else {
                salvar();
            }
        });

        //------------------------------------------------------------------------------------------------------------------------

        $('#descricao').focus();

        //------------------------------------------------------------------------------------------------------------------------

        function limparControles() {
            $('.config').attr('checked', false);

            //$('.combo').val(0).change().selectpicker('refresh');

            //$('#codigo-unidade-referencial').val("");

            $('#codigo-unidade-referencial').val(0).change().selectpicker('refresh'); 0

        };

        function bloquearControles(blockPrincipal) {
            $('#cbo-produto-principal').attr('disabled', blockPrincipal);
            $('#codigo-produto-principal').attr('disabled', blockPrincipal);
            $('#chk-is-principal').attr('disabled', blockPrincipal);

            $('#cbo-dcbid').addClass("bloquear");
            $('.bloquear').attr('disabled', true);
            $('.selectpicker').selectpicker('refresh');
        };

        function desbloquearControles() {
            $('.bloquear').attr('disabled', false);
            $('.selectpicker').selectpicker('refresh');
        };

        function cargaProduto(data) {

            debugger;
            var idGrupo = data.grupoId;
            var idClasse = data.grupoClasseId;

            cargaSelect2($(".dcbclass"));
            if (data.dcb != null) {
                $('#cbo-dcbid').append('<option value="' + data.dcb.id + '">' + data.dcb.descricao + '</option>');

                $('#cbo-dcbid').val(data.dcb.id);
                $('#cbo-dcbid').trigger("change");
                $('#codigo-dcb').val(data.dcb.id);
            }
            $('#cbo-grupo').val(idGrupo).change().selectpicker('refresh');
            $('#cbo-classe').val(idClasse).change().selectpicker('refresh');
            $('#cbo-subclasse').val(data.grupoSubClasseId).change().selectpicker('refresh');

            var idReferencial = null;
            _produtosService.obterUnidadePorTipo(data.id, 1, { async: false, cache: false })
                .done(function (data) {
                    idReferencial = data.id;
                    $('#cbo-unidade-referencial').val(idReferencial).change().selectpicker('refresh');
                    $('#cbo-unidade-gerencial').selectpicker('refresh');
                });

            _produtosService.obterUnidadePorTipo(data.id, 2, { async: false, cache: false })
                .done(function (response) {
                    idGerencial = response.id;
                    $('#cbo-unidade-gerencial').val(idGerencial);

                    $('#cbo-unidade-gerencial').trigger('change', function () {
                        $('#cbo-unidade-gerencial').selectpicker('refresh');
                    });
                });

            $('#genero-id').val(data.generoId).change().selectpicker('refresh');
            $('#etiqueta-id').val(data.etiquetaId).change().selectpicker('refresh');

            $('#chk-is-medicamento').attr('checked', data.isMedicamento).change();
            $('#is-medicamento-controlado').val(data.isMedicamentoControlado);
            $('#chk-is-medicamento-controlado').attr('checked', data.isMedicamentoControlado);

            $('#chk-is-controla-serie').attr('checked', data.isSerie);
            $('#chk-is-curva-abc').attr('checked', data.isCurvaABC);
            $('#chk-is-produto-ativo').attr('checked', data.isAtivo);
            $('#chk-is-lote').attr('checked', data.isLote);
            $('#chk-is-liberado-movimentacao').attr('checked', data.isLiberadoMovimentacao);
            $('#chk-is-bloqueio-compra').attr('checked', data.isBloqueioCompra);
            $('#chk-is-validade').attr('checked', data.isValidade);
            $('#chk-is-kit').attr('checked', data.isKit);
            $('#chk-is-opme').attr('checked', data.isOPME);
            $('#chk-is-consignado').attr('checked', data.isConsignado);
            $('#chk-is-padronizado').attr('checked', data.isPadronizado);

            bloquearControles();

            $('#descricao').focus();
        };

        //aplicarSelect2Padrao();

        selectSW('.selectFatItem', '/api/services/app/FaturamentoItem/ListarDropDown')
        selectSW('.selectContaAdministrativa', '/api/services/app/ContaAdministrativa/ListarContaAdministrivaDespesaTodasEmpresasDropdown')

        selectSW('.selectProdutoMestre', '/api/services/app/produto/ListarProdutoMestreDropdown')
        

        if ($('#is-prescricao-item').is(':checked')) {
            abp.notify.success($('#id-cad-produto').val());
            $('#lnk-prescricao-item').removeClass('hidden');
            $('#tab-prescricao-item').load('/mpa/prescricoesitens/_CriarOuEditar/', { produtoId: $('#id-cad-produto').val() }, function () {
                $('#tab-prescricao-item').removeClass('hidden');
                $('#href_prescricao_item').trigger('click');
                if ($('#codigo-prescricao-item').val() == '') {
                    $('#codigo-prescricao-item').val($('#codigo-produto').val())
                }
                if ($('#descricao-prescricao-item').val() == '') {
                    $('#descricao-prescricao-item').val($('#descricao').val())
                }
                setTimeout(() => { 
                    $('.close').unbind()
                    $('.close').on('click', function () {
                        location.href = '/mpa/produtos';
                    });
                    $('.close-button').unbind()
                    $('.close-button').on('click', function () {
                        location.href = '/mpa/produtos';
                    });

                    $('.save-button').unbind()
                    $('.save-button').on('click', function (e) {
                        e.preventDefault();
                        $('#form-produto').submit();
                    });
                },200)
            });
        }
        else {
            $('#lnk-prescricao-item').addClass('hidden');
            $('#tab-prescricao-item').addClass('hidden');
        }

        $('#is-prescricao-item').on('click', function () {
            if ($(this).is(':checked')) {
                $('#lnk-prescricao-item').removeClass('hidden');
                $('#tab-prescricao-item').load('/mpa/prescricoesitens/_CriarOuEditar/', { produtoId: $('#id-cad-produto').val() }, function () {
                    $('#tab-prescricao-item').removeClass('hidden');
                    $('#href_prescricao_item').trigger('click');
                });
            }
            else {
                $('#tab-prescricao-item').addClass('hidden');
                $('#lnk-prescricao-item').addClass('hidden');
            }
        });

        if ($('#is-faturamento-item').is(':checked')) {
            $('#produto-faturamento-item-id').removeAttr('disabled');
        }
        else {
            $('#produto-faturamento-item-id').attr('disabled', 'disabled');
        }

        $('#is-faturamento-item').on('click', function () {
            if ($(this).is(':checked')) {
                $('#produto-faturamento-item-id').removeAttr('disabled');
            }
            else {
                $('#produto-faturamento-item-id').attr('disabled', 'disabled');
            }
        });

    });
})();