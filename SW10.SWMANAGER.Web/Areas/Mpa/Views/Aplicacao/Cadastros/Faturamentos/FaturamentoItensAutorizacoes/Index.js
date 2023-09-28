(function () {

    $(function () {

        //remover isso

        $('.modal-dialog').css('width', '1800px');

        var _$AutorizacaoTable = $('#AutorizacaoTable');
        var _faturamentoItemAutorizacaoService = abp.services.app.faturamentoItemAutorizacao;
        var _$filterForm = $('#CentralAutorizacaoForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.FaturamentoItemAutorizacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.FaturamentoItemAutorizacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.FaturamentoItemAutorizacao.Delete')
        };

        var _createOrEditFaturamentoItemAutorizacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'MPA/FaturamentoItensAutorizacoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/FaturamentoItensAutorizacoes/_CriarOuEditarModal.js'
        });

        _$AutorizacaoTable.jtable({

            title: app.localize('FaturamentoItemAutorizacao'),
            paging: true,
            sorting: true,
            multiSorting: true,


            actions: {
                listAction: {
                    method: _faturamentoItemAutorizacaoService.listar
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
                                    exibirAutorizacaoProcedimento(data.record);
                                });
                        }
                        return $span;
                    }
                },

                Convenio: {
                    title: app.localize('Convenio'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.convenio) {
                            return data.record.convenio.nomeFantasia;
                        }
                    }
                },

                Item: {
                    title: app.localize('Item'),
                    width: '30%',
                    display: function (data) {
                        if (data.record.faturamentoItem) {
                            return data.record.faturamentoItem.descricao;
                        }
                    }
                },

               
                GrupoItem: {
                    title: app.localize('Grupo'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.faturamentoGrupo) {
                            return data.record.faturamentoGrupo.descricao;
                        }
                    }
                },


                SubGrupoItem: {
                    title: app.localize('SubGrupo'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.faturamentoSubGrupo) {
                            return data.record.faturamentoSubGrupo.descricao;
                        }
                    }
                },
            }

        });

        function exibirAutorizacaoProcedimento(data) {
            location.href = 'FaturamentoItensAutorizacoes/CriarOuEditarModal?id=' + data.id;
        }

        function getAutorizacoes(reload) {
            if (reload) {
                _$AutorizacaoTable.jtable('reload');
            } else {
                _$AutorizacaoTable.jtable('load', {
                    filtro: $('#filtro').val(),
                    convenioId: $('#convenioId').val(),
                    faturamentoItemId: $('#faturamentoItemId').val()
                });
            }
        }

        function deletePreMovimentos(PreMovimento) {

            abp.message.confirm(
                app.localize('DeleteWarning', PreMovimento.documento),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _preMovimentoService.excluir(PreMovimento)
                            .done(function () {
                                getPreMovimentos();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

        $('#CreateNewPreMovimentoButton').click(function () {
            location.href = 'FaturamentoItensAutorizacoes/CriarOuEditarModal';
        });

        $('#RefreshAtendimentosButton').click(function (e) {

            e.preventDefault();
            getAutorizacoes();
        });


        getAutorizacoes();

        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selecFaturamentoGrupo', "/api/services/app/FaturamentoGrupo/ListarDropdown");
        selectSW('.selecFaturamentoSubGrupo', "/api/services/app/FaturamentoSubGrupo/ListarDropdown", $('#faturamentoGrupoId'));
        selectSWMultiplosFiltros('.selecFaturamentoItem', "/api/services/app/FaturamentoItem/ListarFaturamentoItemPorGrupoSubGrupoDropdown", ['faturamentoGrupoId', 'faturamentoSubGrupoId']);

    });
})();