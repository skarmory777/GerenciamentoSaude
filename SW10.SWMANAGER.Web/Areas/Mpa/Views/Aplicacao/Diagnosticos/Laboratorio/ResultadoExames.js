(function () {
    $(function () {
        var _$resultadoExamesLaboratorioTable = $('#resultadoExamesLaboratorioTable');
        var _$laudoResultadoExamesLaboratorioTable = $('#laudoResultadoExamesLaboratorioTable');
        var _resultadoService = abp.services.app.resultado;
        var _resultadoLaudoService = abp.services.app.resultadoLaudo;

        var _formataItemService = abp.services.app.formataItem;



        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ResultadoLaboratorio/CriarOuEditarLaudoResultadoExame',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/LaudoResultadoExames.js',
            modalClass: 'CriarOuEditarLaudoResultadoExamesModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        var _ModalVisualizarExame = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ResultadoLaboratorio/ModalVisualizarExamePorRegistroColeta',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/_ModalVisualizacaoResultado.js',
            modalClass: 'ModalVisualizacaoResultado'
        });





        _$resultadoExamesLaboratorioTable.jtable({

            title: app.localize('Exames'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,




            //rowInserted: function (event, data) {
            //    if (data) {
            //        //if (data.record.status==1) {
            //        //    data.row[0].cells[2].setAttribute('color', data.record.corLancamentoLetra);
            //        //}

            //        if (data.record.status==1) {
            //            data.row[0].cells[2].setAttribute('bgcolor', '#daed0b');

            //        }else  if (data.record.status==2) {
            //            data.row[0].cells[2].setAttribute('bgcolor', '#138440');
            //        }else if (data.record.status==3) {
            //            data.row[0].cells[2].setAttribute('bgcolor', '#0a3bed');
            //        }else if (data.record.status==4) {
            //            data.row[0].cells[2].setAttribute('bgcolor', '#0aedca');
            //        }



            //    }

            //},


            actions: {
                listAction: {
                    method: _resultadoService.listarExamesPorColeta
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '6%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                _createOrEditModal.open({ id: data.record.resultadoExameId });
                                // location.href = 'GestaoLaudos/CriarOuEditarModal/' + data.record.id
                            });


                        return $span;
                    }
                },

                //Status: {
                //    title: app.localize('Status'),
                //    width: '10%',
                //    display: function (data) {

                //        if (data.record.status == 1) {
                //            return app.localize('Pendente');
                //        }
                //        else if (data.record.status == 2) {
                //            return app.localize('Parecer');
                //        } else if (data.record.status == 3) {
                //            return app.localize('Laudo');
                //        } else if (data.record.status == 4) {
                //            return app.localize('LaudoRevisado');
                //        }

                //    }
                //},




                Exame: {
                    title: app.localize('Exame'),
                    width: '20%',
                    display: function (data) {
                        return data.record.exame;
                    }
                },




            }
        });

        function getRegistros(reload) {

            if (reload) {
                _$resultadoExamesLaboratorioTable.jtable('reload');
            } else {
                _$resultadoExamesLaboratorioTable.jtable('load', {
                    // filtro: $('#tableFilter').val()
                    coletaId: $('#coletaId').val()
                });
            }
        }

        getRegistros();


        _$laudoResultadoExamesLaboratorioTable.jtable({

            title: app.localize('Exames'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,
            //paging: true,


            fields: {
                GridId: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '6%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        if (data.record.ExameStatusId != 4) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    editarItem(data.record);
                                });
                        }
                        return $span;
                    }
                },


                Exame: {
                    title: app.localize('Exame'),
                    width: '20%',
                    display: function (data) {
                        return data.record.Exame;
                    }
                },

                Item: {
                    title: app.localize('Item'),
                    width: '20%',
                    display: function (data) {
                        return data.record.DescricaoItem;
                    }
                },

                resultado: {
                    title: app.localize('Resultado'),
                    width: '20%',
                    display: function (data) {
                        return data.record.ResultadoVisualizacao;
                    }
                },

                Unidade: {
                    title: app.localize('Unidade'),
                    width: '20%',
                    display: function (data) {
                        return data.record.Unidade;
                    }
                },
                Referencia: {
                    title: app.localize('Referencia'),
                    width: '20%',
                    display: function (data) {
                        return data.record.Referencia;
                    }
                },

            }
        });

        function getRegistrosLaudo(reload) {



            lista = JSON.parse($('#itensJson').val());

            var allRows = _$laudoResultadoExamesLaboratorioTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');

                _$laudoResultadoExamesLaboratorioTable.jtable('deleteRecord', { key: id, clientOnly: true });

            });

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                _$laudoResultadoExamesLaboratorioTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getRegistrosLaudo();

        function editarItem(record) {

            debugger;

            $('#exame').val(record.Exame);
            $('#exameId').val(record.ExameId);
            $('#item').val(record.DescricaoItem);
            $('#unidade').val(record.Unidade);
            $('#itemId').val(record.ItemId);
            //   $('#valorAlfanumerico').val(record.Resultado)
            $('#gridId').val(record.GridId);
            $('#tipoResultadoId').val(record.TipoResultadoId);

            exibirAtribuirValor(record);
        }

        function exibirAtribuirValor(record) {
            $('#valorAlfanumerico').attr('readonly', false);

            //Tipo Resultado Numerico
            //if (record.TipoResultadoId == 1) {
            switch (record.TipoResultadoId) {
                case 1:
                    $('#valorNumerico').val(record.Resultado);

                    $('#divValorNumerico').show();
                    $('#valorAlfanumerico').hide();
                    $('#divValorMemo').hide();
                    $('#divValorTabela').hide();

                    var casaDecimal = "000000000";

                    if (record.CasaDecimal > 0) {
                        casaDecimal = casaDecimal + ',';
                        casaDecimal = casaDecimal.padEnd((casaDecimal.length + record.CasaDecimal), '0');
                    }
                    $('#valorNumerico').mask(casaDecimal, { reverse: true });


                    $('#valorNumerico').focus();
                    break;
                    //}
                    //Tipo Resultado AlfaNumerico
                    //else if (record.TipoResultadoId == 2) {
                case 2:
                    $('#valorAlfanumerico').val(record.Resultado);

                    $('#valorAlfanumerico').show();
                    $('#divValorNumerico').hide();
                    $('#divValorMemo').hide();
                    $('#divValorTabela').hide();

                    $('#valorAlfanumerico').focus();
                    break;
                    //}
                    //Tipo Calculado
                    //else if (record.TipoResultadoId == 3) {
                case 3:
                    $('#valorAlfanumerico').val(record.Resultado);

                    $('#valorAlfanumerico').show();
                    $('#divValorNumerico').hide();
                    $('#divValorTabela').hide();
                    $('#divValorMemo').hide();
                    $('#valorAlfanumerico').attr('readonly', true);
                    $('#valorAlfanumerico').focus();



                    // _formataItemService.calcularFormula($('#itensJson').val(), record.ItemId);

                    $.ajax({
                        type: "post",
                        url: "/mpa/ResultadoLaboratorio/calcularFormula",
                        data: { input: $('#itensJson').val(), itemResultadoId: record.ItemId },
                        success: function (result) {

                        }
                    });

                    break;

                    //}
                    //Tipo Tabela
                    //else if (record.TipoResultadoId == 4) {
                case 4:
                    $('#valorAlfanumerico').hide();
                    $('#divValorNumerico').hide();
                    $('#divValorMemo').hide();
                    $('#divValorTabela').show();



                    selectSW('.selectTabelaResultado', "/api/services/app/TabelaResultado/ListarDropdown", { valor: record.TabelaId });

                    $('#valorTabela').append($("<option>")
                                     .val(record.Resultado)
                                    .text(record.ResultadoVisualizacao)
                                            ).val(record.Resultado)
                                             .trigger("change");

                    $('#divValorTabela').focus();
                    break;
                default:
                    $('#valorAlfanumerico').hide();
                    $('#divValorNumerico').hide();
                    $('#divValorTabela').hide();
                    $('#divValorMemo').show();

            }
        }


        $('#inserirResultadoExame').on('click', function (e) {

            debugger;


            var record;
            if ($('#itensJson').val() != '') {
                lista = JSON.parse($('#itensJson').val());
            }

            for (var i = 0; i < lista.length; i++) {
                if (lista[i].GridId == $('#gridId').val()) {
                    // lista[i].Resultado = $('#valorAlfanumerico').val();

                    var val = $('#tipoResultadoId').val();
                    switch (val) {
                        //Tipo Resultado Numerico
                        //if ($('#tipoResultadoId').val() == '1') {
                        case "1":
                            lista[i].Resultado = $('#valorNumerico').val();

                            lista[i].ResultadoVisualizacao = $('#valorNumerico').val();
                            break;
                            //}
                            //Tipo Resultado AlfaNumerico
                            //else if ($('#tipoResultadoId').val() == '2') {
                        case "2":
                            lista[i].Resultado = $('#valorAlfanumerico').val();
                            lista[i].ResultadoVisualizacao = $('#valorAlfanumerico').val();
                            break;
                            //}
                        case "4":
                            //else if ($('#tipoResultadoId').val() == 4) {
                            lista[i].Resultado = $('#valorTabela').val();

                            var tabela = $('#valorTabela').select2('data');
                            if (tabela && tabela.length > 0) {

                                lista[i].ResultadoVisualizacao = tabela[0].text;
                            }
                            break;
                            //}
                            //Tipo Resultado Memo
                        default:
                            //else if ($('#tipoResultadoId').val() == 4) {
                            lista[i].Resultado = $('#divValorMemo').val();
                            break;

                            //}
                    }



                    _$laudoResultadoExamesLaboratorioTable.jtable('updateRecord', {
                        record: lista[i]
                        , clientOnly: true
                    });

                    record = lista[i];


                    break;
                }
            }

            $('#itensJson').val(JSON.stringify(lista));

            var gridIdMax = Math.max.apply(null, getFields(lista, 'GridId'));
            var idGrid = parseInt($('#gridId').val());

            if (parseInt(gridIdMax) > idGrid) {

                editarItem(lista[idGrid]);

            }
            else {

                $('#item').val('');
                $('#unidade').val('');
                $('#itemId').val('');
                $('#valorAlfanumerico').val('')
                $('#valorNumerico').val('')
                $('#exame').val('');
                $('#exameId').val('');
                $('#gridId').val('');
                $('#valorTabela').val(null).trigger("change");
            }
        });


        $('#salvar').on('click', function (e) {
            e.preventDefault();

            $.ajax({
                type: "post",
                url: "/mpa/ResultadosExames/criarOuEditarLista",
                data: { input: $('#itensJson').val(), coletaId: $('#coletaId').val() },
                success: function (result) {

                    if (result.result.errors.length > 0) {
                        _ErrorModal.open({ erros: result.result.errors });
                    }
                    else {
                        abp.notify.info(app.localize('SavedSuccessfully'));

                        _ModalVisualizarExame.open({ coletaId: $('#coletaId').val() });

                        // location.href = '/mpa/ResultadoLaboratorio';
                    }
                }
            });

            //_resultadoLaudoService.criarOuEditarLista($('#itensJson').val())
            //    .done(function (data) {
            //        if (data.errors.length > 0) {
            //            _ErrorModal.open({ erros: data.errors });
            //        }
            //        else {
            //            abp.notify.info(app.localize('SavedSuccessfully'));
            //            location.href = '/mpa/ResultadoLaboratorio';
            //        }
            //    })
            //    .always(function () {
            //    });

        });


        //abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
        //    getRegistros(true);
        //});

        function getFields(input, field) {
            var output = [];
            for (var i = 0; i < input.length; ++i)
                output.push(input[i][field]);
            return output;
        }

        getRegistros();

        selectSW('.selectTabelaResultado', "/api/services/app/TabelaResultado/ListarDropdown", { valor: '0' });

        $('.key').keydown(function (e) {
           
            if (e.key == "Enter") {
                $("#inserirResultadoExame").focus();
            }
        });


        $('.close-button').click(function (e) {

            location.href = '/mpa/ResultadoLaboratorio';
        });


    });
})();