
(function ($) {
    $(function () {

        $(document).ready(function () {

            $('#totalDocumento').mask('000.000.000,00', { reverse: true });
            $('#ICMSPer').mask('000.000.000,00', { reverse: true });
            $('#valorICMS').mask('000.000.000,00', { reverse: true });
            $('#DescontoPer').mask('000.000.000,00', { reverse: true });
            $('#ValorDesconto').mask('000.000.000,00', { reverse: true });
            $('#ValorAcrescimo').mask('000.000.000,00', { reverse: true });
            $('#frete').mask('000.000.000,00', { reverse: true });
            $('#FretePer').mask('000.000.000,00', { reverse: true });
            $('#ValorFrete').mask('000.000.000,00', { reverse: true });

            CamposRequeridos();

        });

        $('.modal-dialog').css('width', '1800px');

        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        $.validator.setDefaults({ ignore: ":hidden:not(select)" });

        // validation of chosen on change
        $('ul.ui-autocomplete').css('z-index', '2147483647 !important');


        //$('#totalDocumento').change(function () {
        //    CalcularValorICMS();
        //});

        //$('#ICMSPer').change(function () {
        //    CalcularValorICMS();
        //});

        function CalcularValorICMS() {
            var valorIcms = parseFloat($('#totalDocumento').val()) * parseFloat($('#ICMSPer').val()) / 100;
            $('#valorICMS').val(valorIcms);

        }


        //$('#totalProdutoId, #freteId, #ValorDesconto, #ValorAcrescimo').change(function () {
        //    CalcularTotalProduto();
        //});


        function CalcularTotalProduto() {
            var totalProduto = ($('#totalProdutoId').val() != '') ? parseFloat($('#totalProdutoId').val()) : 0;

            var valorFrete = ($('#ValorFrete').val() != '') ? parseFloat($('#ValorFrete').val()) : 0;
            var valorDesconto = ($('#ValorDesconto').val() != '') ? parseFloat($('#ValorDesconto').val()) : 0;
            var valorAcrescimo = ($('#ValorAcrescimo').val() != '') ? parseFloat($('#ValorAcrescimo').val()) : 0;

            var valorTotalProduto = totalProduto + valorFrete - valorDesconto + valorAcrescimo;
            $('#totalDocumento').val(valorTotalProduto);
            CalcularValorICMS();
        }

        function CalcularValorFrete() {
            var valorDesconto = 0;
            if ($('#FretePer').val() != '') {
                var valorDesconto = parseFloat($('#freteId').val()) * parseFloat($('#FretePer').val()) / 100;
            }
            var valorFrete = parseFloat($('#freteId').val()) - valorDesconto;
            $('#ValorFrete').val(valorFrete);
        }

        function CalcularValorDesconto() {
            var valorDesconto = parseFloat($('#totalDocumento').val()) * parseFloat($('#DescontoPer').val()) / 100;
            $('#ValorDesconto').val(valorDesconto);
        }

        $('#empresa-search').autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                var term = $('#empresa-search').val();
                var url = '/mpa/empresas/autocompleteDescricao';


                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#empresa-Id').val(0);
                        $("#empresa-search").focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#empresa-Id').val(0);
                        return {
                            label: item.nome,
                            value: item.nome,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#empresa-Id').val(ui.item.realValue);
                $('#empresa-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#empresa-Id').val(0);
                    $("#empresa-search").val('').focus();
                    abp.notify.info(app.localize("EstadoInvalido"));
                    return false;
                }
            },
        });

        $('#fornecedor-search').autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                var term = $('#fornecedor-search').val();
                var url = '/mpa/fornecedores/autocomplete';


                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#fornecedor-Id').val(0);
                        $("#fornecedor-search").focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#fornecedor-Id').val(0);
                        return {
                            label: item.nome,
                            value: item.nome,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#fornecedor-Id').val(ui.item.realValue);
                $('#fornecedor-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#fornecedor-Id').val(0);
                    $("#fornecedor-search").val('').focus();
                    abp.notify.info(app.localize("EstadoInvalido"));
                    return false;
                }
            },
        });

        $('#centroCusto-search').autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                var term = $('#centroCusto-search').val();
                var url = '/mpa/centrosCustos/autocomplete';

                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#centroCusto-Id').val(0);
                        $("#centroCusto-search").focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#centroCusto-Id').val(0);
                        return {
                            label: item.nome,
                            value: item.nome,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#centroCusto-Id').val(ui.item.realValue);
                $('#centroCusto-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#centroCusto-Id').val(0);
                    $("#centroCusto-search").val('').focus();
                    abp.notify.info(app.localize("CentroCustoInvalido"));
                    return false;
                }
            },
        });

        $('#Movimento').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#movimento').val(moment().format("L LT"));
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };

        var iValidador = {
            init: function () {
                // Execute seus códigos iniciais
                // ...
                //alert('Entrou no validador agora!');
                // Chame as funções desejadas...
                iValidador.outraFuncao();
            },
            outraFuncao: function () {
                // Códigos desejados...
            }
        };


        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _movimentoService = abp.services.app.estoqueMovimento;

        var _estoquePreMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _$EstoquePreMovimentoItemTable = $('#EstoquePreMovimentoItemTable');
        var _$ItensValeTable = $('#ItensValeTable');

        var _createOrEditPreMovimentoItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/baixaVales/CriarOuEditarMovimentoItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/BaixaVales/_CriarOuEditarPreMovimentoItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoItemModal'
        });

        var _createOrEditLoteValidadeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/InformarLoteValidadeModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_InformarLoteValidade.js',
            modalClass: 'EstoquePreMovimentoLoteValidadeProduto'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

      

        $('#btn-buscarNotas').click(function (e) {
            e.preventDefault()
        });

        $('#btn-novo-PreMovimentoItem').click(function (e) {
            e.preventDefault()

            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

            preMovimento.ValorICMS = retirarMascara(preMovimento.ValorICMS);
            preMovimento.TotalDocumento = retirarMascara(preMovimento.TotalDocumento);
            preMovimento.ICMSPer = retirarMascara(preMovimento.ICMSPer);
            preMovimento.DescontoPer = retirarMascara(preMovimento.DescontoPer);
            preMovimento.ValorDesconto = retirarMascara(preMovimento.ValorDesconto);
            preMovimento.AcrescimoDecrescimo = retirarMascara(preMovimento.AcrescimoDecrescimo);
            preMovimento.FretePer = retirarMascara(preMovimento.FretePer);
            preMovimento.ValorFrete = retirarMascara(preMovimento.ValorFrete);
            preMovimento.Frete = retirarMascara(preMovimento.Frete);
            preMovimento.ValorICMS = retirarMascara(preMovimento.ValorICMS);
            preMovimento.IsEntrada = true;

            //  _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            if ($('#id').val() == '' || $('#id').val() == '0') {

                _preMovimentoService.criarGetIdEntrada(preMovimento)
                      .done(function (data) {
                          abp.notify.info(app.localize('SavedSuccessfully'));
                          $('#id').val(data.id);

                          _createOrEditPreMovimentoItemModal.open({ preMovimentoId: $('#id').val(), id: 0 });

                      })
                     .always(function () {
                         //  _modalManager.setBusy(false);
                     });
            }
            else {

                _createOrEditPreMovimentoItemModal.open({ preMovimentoId: $('#id').val(), id: 0 });


                //location.href = '/Mpa/PreMovimentos/CriarOuEditarPreMovimentoItemModal';
            }
        });

        $('#salvar-PreMovimento').click(function (e) {
            e.preventDefault()

           
            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var movimento = _$preMovimentoInformationsForm.serializeFormToObject();
           
            movimento.TotalDocumento = retirarMascara(movimento.TotalDocumento);
            movimento.ICMSPer = retirarMascara(movimento.ICMSPer);
            movimento.DescontoPer = retirarMascara(movimento.DescontoPer);
            movimento.ValorDesconto = retirarMascara(movimento.ValorDesconto);
            movimento.AcrescimoDecrescimo = retirarMascara(movimento.AcrescimoDecrescimo);
            movimento.FretePer = retirarMascara(movimento.FretePer);
            movimento.ValorFrete = retirarMascara(movimento.ValorFrete);
            movimento.Frete = retirarMascara(movimento.Frete);
            movimento.ValorICMS = retirarMascara(movimento.ValorICMS);
            movimento.IsEntrada = true;
            //  _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            _movimentoService.criarOuEditar(movimento, $('#movimentosIds').val())
                  .done(function (data) {

                      if (data.errors.length > 0) {
                          _ErrorModal.open({ erros: data.errors });
                      }
                      else {
                          $('#id').val(data.returnObject.id);
                          $('#estoquePreMovimentoId').val(data.returnObject.estoquePreMovimentoId);
                            //  location.href = '/mpa/baixaVales';
                      }

                  })
                 .always(function () {
                     //  _modalManager.setBusy(false);
                 });
        });

       
        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        function salvar(e) {

        }


        $('input[name="Movimento"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
           function (selDate) {
               $('input[name="Movimento"]').val(selDate.format('L')).addClass('form-control edited');
               // obterIdade(selDate);
           });

        $('input[name="Emissao"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
         function (selDate) {
             $('input[name="Emissao"]').val(selDate.format('L')).addClass('form-control edited');
         });


        abp.event.on('app.EditarBaixaValeItemModalSaved', function () {
            getEstoquePreMovimentoItemTable();
            getItensValeTable();
        });

        var _estoqueMovimentoService = abp.services.app.estoqueMovimento;

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;

        };

        $('.close').on('click', function () {
            location.href = '/mpa/baixaVales';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/baixaVales';
        });

        _$EstoquePreMovimentoItemTable.jtable
        ({
            title: app.localize('Vales'),
            paging: true,
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,
            actions:
            {
                listAction:
                {
                    method: _estoqueMovimentoService.listarMovimentosValeSelecionados
                },
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },

                Empresa: {
                    title: app.localize('Empresa'),
                    width: '15%',
                    display: function (data) {
                        return data.record.empresa;
                    }
                },

                Fornecedor: {
                    title: app.localize('Fornecedor'),
                    width: '15%',
                    display: function (data) {
                        return data.record.fornecedor;
                    }
                },

                Emissao: {
                    title: app.localize('Emissao'),
                    width: '15%',
                    display: function (data) {
                        return moment(data.record.emissao).format('L');
                    }
                },
              
                Documento: {
                    title: app.localize('Documento'),
                    width: '10%',
                    display: function (data) {
                        return data.record.documento;
                    }
                },

                Valor: {
                    title: app.localize('Valor'),
                    width: '10%',
                    display: function (data) {
                        return posicionarDireita(data.record.valorDocumento.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
                    }
                }

            }
        });


        _$ItensValeTable.jtable
    ({
        title: app.localize('Item'),
        paging: true,
        sorting: true,
        edit: false,
        create: false,
        multiSorting: true,
        actions:
        {
            listAction:
            {
                method: _estoqueMovimentoService.listarItensValesSelecionados
            },
        },
        fields:
        {
            id: {
                key: true,
                list: false
            },
            actions: {
                title: app.localize('Actions'),
                width: '11%',
                sorting: false,
                display: function (data) {
                    var $span = $('<span></span>');
               
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                _createOrEditPreMovimentoItemModal.open({ id: data.record.id, preMovimentoId: $('#id').val() });
                            });

                    if (data.record.isValidade || data.record.isLote) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('LoteValidade') + '"><i class="fa fa-edit"></i></button>')
                           .appendTo($span)
                           .click(function (e) {
                               e.preventDefault();
                               abrirPreMovimentoItemLoteValidade(data.record);
                           });
                    }
                    return $span;
                }
            },
            PreMovimentoId: {
                type: 'hidden',
                defaultValue: function (data) {
                    return $('#id').val();
                },
            },

            ProdutoId: {
                title: app.localize('Produto'),
                width: '30%',
                display: function (data) {
                    if (data.record.produto) {
                        return data.record.produto;
                    }
                }
            },
            NumeroSerie: {
                title: app.localize('NumeroSerie'),
                width: '10%',
                display: function (data) {
                    if (data.record.numeroSerie) {
                        return data.record.numeroSerie;
                    }
                }
            },

            quantidade: {
                title: app.localize('Quantidade'),
                width: '10%',
                display: function (data) {
                    if (data.record.quantidade) {
                        return posicionarDireita(data.record.quantidade.toFixed(2));
                    }
                }
            },

            CustoUnitario: {
                title: app.localize('CustoUnitario'),
                width: '10%',
                display: function (data) {
                    if (data.record.custoUnitario) {
                        return posicionarDireita(data.record.custoUnitario.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
                    }
                }
            },

            CustoTotal: {
                title: app.localize('CustoTotal'),
                width: '10%',
                display: function (data) {
                    if (data.record.custoTotal) {
                        return posicionarDireita(data.record.custoTotal.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
                    }
                }
            },

            Unidade: {
                title: app.localize('Unidade'),
                width: '20%',
                display: function (data) {
                    if (data.record.unidade) {
                        return data.record.unidade;
                    }
                }
            },
        }
    });



        function deletePreMovimentoItem(preMovimentoItem) {

            abp.message.confirm(
                app.localize('DeleteWarning', preMovimentoItem.produto.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _estoquePreMovimentoItemService.excluir(preMovimentoItem.id)
                            .done(function () {
                                getEstoquePreMovimentoItemTable(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }


        function getEstoquePreMovimentoItemTable(reload) {

            if (reload) {
                _$EstoquePreMovimentoItemTable.jtable('reload');
            } else {
               
                _$EstoquePreMovimentoItemTable.jtable('load', { filtro: $('#movimentosIds').val() });
            }
        }

        function getItensValeTable(reload) {

            if (reload) {
                _$ItensValeTable.jtable('reload');
            } else {
               
                _$ItensValeTable.jtable('load', { filtro: $('#movimentosIds').val() });
            }
        }



        
       

        function abrirPreMovimentoItemLoteValidade(preMovimentoItem) {
            _createOrEditLoteValidadeProdutoModal.open({ preMovimentoItemId: preMovimentoItem.id, produtoId: preMovimentoItem.produtoId });

        }


        getEstoquePreMovimentoItemTable();
        getItensValeTable();

        $('#AplicacaoDiretaId').on('click', function (e) {


            var checkbox = e.target;
            if (checkbox.checked) {
                $('#divPaciente').show();
            }
            else {
                $('#divPaciente').hide();
            }

        });







        selectSW('.selectForncedor', "/api/services/app/fornecedor/ListarDropdown");
        selectSW('.selectCFOP', "/api/services/app/cfop/ListarDropdown");




        //function selectSW(classe, url) {

        //    $(classe).select2({
        //        allowClear: true,
        //        placeholder: "Informe um produto",
        //        ajax: {
        //            url:url ,
        //            dataType: 'json',
        //            delay: 250,
        //            method: 'Post',

        //            data: function (params) {
        //               
        //                //   //console.log('data: ', params, (params.page == undefined));
        //                if (params.page == undefined)
        //                    params.page = '1';
        //                //   //console.log('data: ', params);
        //                return {
        //                    search: params.term,
        //                    page: params.page,
        //                    totalPorPagina: 10
        //                };
        //            },
        //            processResults: function (data, params) {
        //                params.page = params.page || 1;
        //               

        //                return {
        //                    results: data.result.items,
        //                    pagination: {
        //                        more: (params.page * 10) < data.result.totalCount
        //                    }
        //                };
        //            },
        //            cache: true
        //        },
        //        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //        minimumInputLength: 1
        //    });

        //}


    });

})(jQuery);