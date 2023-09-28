(function ($) {
    app.modals.DescontoModal = function () {

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        }


        $(document).ready(function () {
            debugger;
            $('#valorDesconto').mask('000.000.000,00', { reverse: true });
        });

        var procedimentosTable = $('#procedimentosTable');

        procedimentosTable.jtable({

            title: app.localize('Procedimentos'),
            //paging: true,
            //sorting: true,
            //multiSorting: true,

            //actions: {
            //    listAction: {
            //        method: abp.services.app.agendamentoSalaCirurgica.obterProcedimentos
            //    }
            //},


            fields: {
                Id: {
                    key: true,
                    list: false
                },
                //actions: {
                //    title: app.localize('Actions'),
                //    width: '5%',
                //    sorting: false,
                //    display: function (data) {
                //        var $span = $('<span></span>');
                //        // if (_permissions.edit) {
                //        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                //            .appendTo($span)
                //            .click(function () {
                //                _editModal.open({ id: data.record.Id });
                //            });
                //        //  }



                //        return $span;
                //    }
                //},

                Procedimento: {
                    title: app.localize('Procedimento'),
                    width: '80%',
                    display: function (data) {
                        debugger;
                        return data.record.Procedimento;
                    }
                },

                ValorSemDesconto: {
                    title: app.localize('Valor'),
                    width: '15%',
                    display: function (data) {
                        debugger;
                        return posicionarDireita(data.record.ValorSemDesconto.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
                    }
                },

                valorComDesconto: {
                    title: app.localize('Desconto'),
                    width: '15%',
                    display: function (data) {
                        return posicionarDireita(data.record.ValorComDesconto.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
                    }
                }


            }
        });

        function getProcedimentos() {
            procedimentosTable.jtable('load', {
                id: $('#id').val()

            });
        }

        // getProcedimentos();

        getRegistros();

        var lista = [];

        function getRegistros() {

            debugger;

            lista = JSON.parse($('#descontoJson').val());

            var allRows = procedimentosTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                procedimentosTable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];

                procedimentosTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }


        $('#valorDesconto').on('change', function () {

            var valorDesconto = $('#valorDesconto').val() != '' ? parseFloat(retirarMascara($('#valorDesconto').val())) : 0;
            var valorSemDesconto = $('#valorSemDesconto').val() != '' ? parseFloat(retirarMascara($('#valorSemDesconto').val())) : 0;

            if(valorDesconto > valorSemDesconto)
            {
                alert('O valor do desconto não pode ser maior que o valor total.');
            }
            else
            {
                lista = JSON.parse($('#descontoJson').val());

                var descontoParcela = valorDesconto / lista.length;

                for (var i = 0; i < lista.length; i++) {
                    var item = lista[i];
                    debugger;
                    item.ValorComDesconto =  parseFloat(item.ValorSemDesconto) -  descontoParcela;

                    procedimentosTable.jtable('updateRecord', {
                        record: item
                        , clientOnly: true
                    });
                }

                $('#descontoJson').val(JSON.stringify(lista));

            }


           
        });

        $('#btnSalvarDesconto').on('click', function () {

            debugger;
            _modalManager.setBusy(true);
            lista = JSON.parse($('#descontoJson').val());
            abp.services.app.agendamentoSalaCirurgica.atualizarDesconto($('#id').val(), retirarMascara($('#valorDesconto').val()))
             .done(function (data) {

                 if (data.errors.length > 0) {
                     _ErrorModal.open({ erros: data.errors });
                 }
                 else {
                   
                     _modalManager.close();
                 }

             })
             .always(function () {
                 _modalManager.setBusy(false);
             });
        


        });
    }
    })(jQuery);