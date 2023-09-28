(function () {
    app.modals.CriarOuEditarLaudoResultadoExamesModal = function () {
        var lista = [];

        $(function () {
            var _$laudoResultadoExamesLaboratorioTable = $('#laudoResultadoExamesLaboratorioTable');
            var _resultadoService = abp.services.app.resultado;
            var _resultadoLaudoService = abp.services.app.resultadoLaudo;

         
            function editarItem(record)
            {
                $('#item').val(record.DescricaoItem);
                $('#unidade').val(record.Unidade);
                $('#itemId').val(record.ItemId);
                
            }

            function exibirAtribuirValor(record)
            {
                //Tipo Resultado Numerico
                if (record.TipoResultadoId == 1) {
                    $('#valorAlfanumerico').val(record.Resultado)
                }
                    //Tipo Resultado AlfaNumerico
                else if (record.TipoResultadoId == 2) {
                    $('#valorAlfanumerico').val(record.Resultado)
                }
            }


            $('#inserirResultadoExame').on('click', function (e) {

               

                if ($('#itensJson').val() != '') {
                    lista = JSON.parse($('#itensJson').val());
                }

                for(var i=0; i< lista.length;i++)
                {
                    if(lista[i].ItemId == $('#itemId').val())
                    {
                        lista[i].Resultado = $('#valorAlfanumerico').val();


                        _$laudoResultadoExamesLaboratorioTable.jtable('updateRecord', {
                            record: lista[i]
                            , clientOnly: true
                        });



                        break;
                    }
                }

                $('#itensJson').val(JSON.stringify(lista));

             

                $('#item').val('');
                $('#unidade').val('');
                $('#itemId').val('');
                $('#valorAlfanumerico').val('')
            });

            $('#salvar').on('click', function (e) {


               

                $.ajax({
                    type: "POST",
                    url: '_resultadoLaudoService.criarOuEditarLista',
                    data: {input:  $('#itensJson').val()},
                    success: function (data) {

                        if (data.errors.length > 0) {
                            _ErrorModal.open({ erros: data.errors });
                        }
                        else {
                            abp.notify.info(app.localize('SavedSuccessfully'));
                            location.href = '/mpa/ResultadoLaboratorio';
                        }
                    }
                });


                //_resultadoLaudoService.criarOuEditarLista($('#itensJson').val(), $('#resultadoExameId').val())
               // .done(function (data) {
               //     if (data.errors.length > 0) {
               //         _ErrorModal.open({ erros: data.errors });
               //     }
               //     else {
               //         abp.notify.info(app.localize('SavedSuccessfully'));
               //         location.href = '/mpa/ResultadoLaboratorio';
               //     }
               // })
               //.always(function () {
               //});
               
            });

           

        });
    }
})();