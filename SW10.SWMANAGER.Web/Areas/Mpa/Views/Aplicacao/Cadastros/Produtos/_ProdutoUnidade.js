(function () {
    $(function () {

        //DIVs Abas
        //------------------------------------------------------------------------------------------------------------------------
        var _$ProdutoUnidadeTable = $('#ProdutoUnidadeTable');

        //Serviços
        //------------------------------------------------------------------------------------------------------------------------
        var _produtosUnidadeService = abp.services.app.produtoUnidade;
        var _produtosService = abp.services.app.produto;

        //ModalManagers
        //------------------------------------------------------------------------------------------------------------------------
        var _createOrEditProdutoUnidadeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Produtos/CriarOuEditarProdutoUnidadeModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoUnidadeModal.js',
            modalClass: 'CriarOuEditarProdutoUnidadeModal'
        });

        //Grid
        //------------------------------------------------------------------------------------------------------------------------
        _$ProdutoUnidadeTable.jtable({
            title: app.localize('Unidade'),
            paging: true,
            sorting: true,
            useBootstrap: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _produtosService.listarProdutosUnidadesPorProduto
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
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        //  if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                _createOrEditProdutoUnidadeModal.open({ id: data.record.id, unidadeReferencialId: $('#cbo-unidade-referencial').val(), unidadegerencialId: $('#cbo-unidade-gerencial').val(), idProduto: $("#id").val() });
                            });
                        // }
                       
                        if (data.record.tipo && !(data.record.tipo.id == 1 || data.record.tipo.id ==2)) {
                            // if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteProdutoUnidade(data.record);
                                });
                            // }
                        }

                        return $span;
                    }
                },
                //unidadeId: {
                //    title: app.localize('Codigo'),
                //    width: '7%'
                //},
                isAtivo: {
                    title: app.localize('IsAtivo'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.isAtivo) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                isPrescricao: {
                    title: app.localize('IsPrescricao'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.isPrescricao) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                tipo: {
                    title: app.localize('Tipo'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.tipo) {
                            return data.record.tipo.descricao;
                        }
                    }
                },
                sigla: {
                    title: app.localize('Sigla'),
                    width: '9%',
                    display: function (data) {
                        if (data.record.unidade) {
                            return data.record.unidade.sigla;
                        }

                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '54%',
                    display: function (data) {
                        if (data.record.unidade) {
                            return data.record.unidade.descricao;
                        }

                    }
                }
            }
        });

        function deleteProdutoUnidade(ProdutoUnidade) {
            //debugger;
            //console.log(ProdutoUnidade);
            abp.message.confirm(
            app.localize('DeleteWarning', ProdutoUnidade.unidade.descricao),
            function (isConfirmed) {
                //app.localize('DeleteWarning', ProdutoPortaria.descricao)
                //console.log(isConfirmed);
                if (isConfirmed) {
                    //if (isConfirmed) {
                    //_produtosUnidadeService.excluir(ProdutoUnidade.id)
                    _produtosUnidadeService.excluir(ProdutoUnidade)
                        .done(function () {
                            //console.log('delete');
                            sessionStorage['ProdutoId'] = ProdutoUnidade.produtoId;
                            getProdutoUnidadeTable(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            }
            );
        }

        function getProdutoUnidadeTable(reload) {
            if (reload) {
                _$ProdutoUnidadeTable.jtable('reload');
            } else {
                _$ProdutoUnidadeTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        //getProdutoUnidadeTable();

        //Botao que persiste as relações de produtos com outros cadastros
        //------------------------------------------------------------------------------------------------------------------------
        $('#btn-novo-produto-unidade').click(function (e) {
            e.preventDefault();

            var unidadeReferencial = $('#cbo-unidade-referencial').val();
            var unidadeGerencial = $('#cbo-unidade-gerencial').val();

            if ((unidadeReferencial == "") || (unidadeReferencial == undefined)) {
                unidadeReferencial = 0;
            };

            if ((unidadeGerencial == "") || (unidadeGerencial == undefined)) {
                unidadeGerencial = 0;
            };

            if (unidadeReferencial == 0) {
                abp.message.info("Para adicionar uma unidade ao produto é necessário informar a Unidade Referencial.");
                $('#codigo-unidade-referencial').focus();
            } else {
                _createOrEditProdutoUnidadeModal.open({ id: null, unidadeReferencialId: unidadeReferencial, unidadegerencialId: unidadeGerencial, idProduto: $("#id").val() });
            };

        });

        //------------------------------------------------------------------------------------------------------------------------
        //Atualizacoes das Grids das Abas após Salvar Inclusoes/Alteracoes
        abp.event.on('app.CriarOuEditarProdutoUnidadeModalSaved', function () {
            //getProdutos(true);
            getProdutoUnidadeTable(true);
        });
        $('#href_unidade').on('click', function (e) {
            e.preventDefault();
            getProdutoUnidadeTable();
        })
    });
})();