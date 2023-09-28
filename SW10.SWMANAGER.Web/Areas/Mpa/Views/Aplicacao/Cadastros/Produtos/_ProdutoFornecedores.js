(function () {
    $(function () {
        var _produtosService = abp.services.app.produto;
        var _$ProdutoFornecedoresTable = $('#ProdutoFornecedoresTable');

        _$ProdutoFornecedoresTable.jtable({
            title: app.localize('Fornecedores'),
            paging: true,
            sorting: true,
            useBootstrap: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _produtosService.listarProdutoFornecedores
                }
            },

            fields:
            {
                id: {
                    key: true,
                    list: false
                },
                unidadeId: {
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '3%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                deleteProdutoFornecedor(data.record);
                            });

                        return $span;
                    }
                },
                CnpjNota: {
                    title: app.localize('CNPJ'),
                    width: '10%',
                    display: function (data) {
                        console.log('data:', data);
                        if (data.record.cnpjNota) {
                            return data.record.cnpjNota;
                        }
                    }
                },
                NomeFantasia: {
                    title: app.localize('NomeFantasia'),
                    width: '20%',
                    display: function (data) {
                        if (data.record.nomeFantasia) {
                            return data.record.nomeFantasia;
                        }
                    }
                },
                RazaoSocial: {
                    title: app.localize('RazaoSocial'),
                    width: '20%',
                    display: function (data) {
                        if (data.record.razaoSocial) {
                            return data.record.razaoSocial;
                        }
                    }
                },
            }
        });

        function getProdutoFornecedoresTable(reload) {
            if (reload) {
                _$ProdutoFornecedoresTable.jtable('reload');
            } else {
                _$ProdutoFornecedoresTable.jtable('load', { id: $('#id').val() });
            }
        }

        $('#href_fornecedores').on('click', function (e) {
            e.preventDefault();
            getProdutoFornecedoresTable();
        })

        function deleteProdutoFornecedor(produtoFornecedor) {
            abp.message.confirm(
                app.localize('DeleteWarning', app.localize('EsteRegistro')),
                function (isConfirmed) {
                    if (isConfirmed) {
                        var produtoId = $('#id').val();
                        var cnpj = produtoFornecedor.cnpjNota;                        
                        _produtosService.excluirProdutoFornecedor(produtoId, cnpj)
                            .done(function () {
                                getProdutoFornecedoresTable(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }
    });
})();