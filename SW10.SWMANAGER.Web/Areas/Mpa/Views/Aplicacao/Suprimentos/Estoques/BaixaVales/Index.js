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

            title: app.localize('Vales'),
            paging: true,
            sorting: true,
            multiSorting: true,
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true,
            selecting: true,

            actions: {
                listAction: {
                    method: _movimentoService.listarBaixaValePendente
                }
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
                        return moment(data.record.dataEmissaoSaida).format('L');
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
            _movimentoValidacaoService.validarFornecedoresBaixaVale(idsMovimentos)
                 .done(function (data) {
                     if (data.length > 0) {
                         _ErrorModal.open({ erros: data });
                     }
                     else {
                         location.href = 'BaixaVales/CriarOuEditarModal/' + idsMovimentos;
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