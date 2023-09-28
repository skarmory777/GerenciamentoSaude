(function () {

    $(function () {

        $('.modal-dialog').css('width', '1800px');
        
        var _$MovimentoTable = $('#MovimentoTable');
        var _movimentoService = abp.services.app.estoqueMovimento;
        var _movimentoValidacaoService = abp.services.app.movimentoValidacao;
        var _$filterForm = $('#PreMovimentoFilterForm');

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

       
        _$MovimentoTable.jtable({

            title: app.localize('Consignados'),
            paging: true,
            sorting: true,
            multiSorting: true,
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true,
            selecting: true,

            actions: {
                listAction: {
                    method: _movimentoService.listarBaixaConsignadosPendente
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
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
                            return posicionarDireita(data.record.quantidade);
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
                    width: '10%',
                    display: function (data) {
                        return data.record.unidade;
                    }
                },

                Fornecedor: {
                    title: app.localize('Fornecedor'),
                    width: '10%',
                    display: function (data) {
                            return data.record.fornecedor;
                    }
                },
                               
            },

            selectionChanged: function () {
                //Get all selected rows
                //var $selectedRows = _$MovimentoTable.jtable('selectedRows');
                //if ($selectedRows.length > 0) {

                //   
                //      //  $('#FornecedorId').val($selectedRows[0].data('record').fornecedorId)




                //    //Show selected rows
                //    $selectedRows.each(function () {
                //       

                //        var record = $(this).data('record');

                //        var id = record.fornecedorId;


                //        $('#fornecedorId').select('val', id).trigger('change');
                //    });
                //}
               

            }
            
        });

        var _selectedDateRange = {
            startDate: moment().add('-29', 'd').startOf('day'),
            endDate: moment().endOf('day')
        };

        selectSW('.selectForncedor', "/api/services/app/fornecedor/ListarDropdown");

        function getPreMovimentos(reload) {

            if (reload) {
                
                _$MovimentoTable.jtable('reload');
            } else {
                _$MovimentoTable.jtable('load', {
                    filtro: $('#BaixaTableFilter').val(),
                    fornecedorId: $('#fornecedorId').val(),
                    peridoDe: _selectedDateRange.startDate,
                    peridoAte: _selectedDateRange.endDate, 
                    isEntrada: false
                });
            }
        }

      

        function createRequestParams() {
            var prms = {};

            _$MovimentoTable.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

      

        $('#CreateNewBaixaValeButton').click(function () {
            var $selectedRows = _$MovimentoTable.jtable('selectedRows');


            var idsMovimentos = '';
            $selectedRows.each(function () {
                var record = $(this).data('record');
                idsMovimentos += record.id + '-';
            });

            $('#movimentosIds').val(idsMovimentos);
            _movimentoValidacaoService.validarFornecedoresMovimentosItemBaixaConsignados(idsMovimentos)
                 .done(function (data) {
                     if (data.length > 0) {
                         _ErrorModal.open({ erros: data });
                     }
                     else {
                         location.href = 'BaixaConsignados/CriarOuEditarModal/' + idsMovimentos;
                     }

                 })
        });

        $('#RefreshAtendimentosButton').click(function (e) {

            e.preventDefault();
            getPreMovimentos();

        });
              
        getPreMovimentos();

        $('#BaixaTableFilterForm').focus();
        
        _$filterForm.find('input.date-range-picker').daterangepicker(
           $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
           function (start, end, label) {
               _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
               _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
           });

    });
})();