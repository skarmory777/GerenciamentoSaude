(function ($) {
    app.modals.CriarOuEditarTipoRespostaModal = function () {
        localStorage["FecharModal"] = false;
        var _tipoRespostaTipoRespostaConfiguracaoService = abp.services.app.tipoRespostaTipoRespostaConfiguracao;
        var _tipoRespostaService = abp.services.app.tipoResposta;
        var _modalManager;
        var _$formTipoResposta = null;
        var _$TiposRespostasTiposRespostasConfiguracoesTable = $('#TiposRespostasTiposRespostasConfiguracoesTable');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Delete')
        };

        var _createOrEditRelacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposRespostas/_CriarOuEditarRelacaoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostas/_CriarOuEditarRelacaoModal.js',
            modalClass: 'CriarOuEditarTipoRespostaTipoRespostaConfiguracaoModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$formTipoResposta = _modalManager.getModal().find('form[name=TipoRespostaInformationsForm]');
            _$formTipoResposta.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
            if ($('#tipo-resposta-id-principal').val() > 0) {
                $('#tabs-content').removeClass('hidden');
            }
        };

        this.save = function () {
            if (!_$formTipoResposta.valid()) {
                return;
            }
            var tipoResposta = _$formTipoResposta.serializeFormToObject();
            _modalManager.setBusy(true);
            _tipoRespostaService.criarOuEditar(tipoResposta)
                 .done(function (data) {
                     $('#tipo-resposta-id-principal').val(data.id);
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     if (localStorage["FecharModal"] == "true") {
                         _modalManager.close();
                     }
                     abp.event.trigger('app.CriarOuEditarTipoRespostaModalSaved');
                     $('#tabs-content').removeClass('hidden');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function deleteTipoRespostaTipoRespostaConfiguracao(tipoRespostaConfiguracao) {
            abp.message.confirm(
                app.localize('DeleteWarning', tipoRespostaConfiguracao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _tipoRespostaTipoRespostaConfiguracaoService.excluir(tipoRespostaConfiguracao)
                            .done(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                getTiposRespostasTiposRespostasConfiguracoes();
                            });
                    }
                }
            );
        }

        function getTiposRespostasTiposRespostasConfiguracoes() {
            _$TiposRespostasTiposRespostasConfiguracoesTable.jtable('load', {
                filtro: $('#TiposRespostasTiposRespostasConfiguracoesTableFilter').val(),
                tipoRespostaId: $('#tipo-resposta-id-principal').val()
            });
        }

        _$TiposRespostasTiposRespostasConfiguracoesTable.jtable({
            title: app.localize('TiposRespostasTiposRespostasConfiguracoesTable'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _tipoRespostaTipoRespostaConfiguracaoService.listarPorTipoResposta
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditRelacaoModal.open({
                                        tipoRespostaPrincipalId: $('#tipo-resposta-id-principal').val(),
                                        id: data.record.id
                                    });
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteTipoRespostaTipoRespostaConfiguracao(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%',
                    display: function (data) {
                        return data.record.tipoRespostaConfiguracao.codigo
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%',
                    display: function (data) {
                        return data.record.tipoRespostaConfiguracao.descricao
                    }
                },
            }
        });

        //$("#tipo-resposta-id").select2({
        //    ajax: {
        //        url: "/api/services/app/TipoRespostaConfiguracao/listarDropdown",
        //        dataType: 'json',
        //        delay: 250,
        //        method: 'Post',
        //        data: function (params) {
        //            //   //console.log('data: ', params, (params.page == undefined));
        //            if (params.page == undefined)
        //                params.page = '1';
        //            //   //console.log('data: ', params);
        //            return {
        //                search: params.term,
        //                page: params.page,
        //                totalPorPagina: 10
        //            };
        //        },
        //        processResults: function (data, params) {
        //            params.page = params.page || 1;
        //            return {
        //                results: data.result.items,
        //                pagination: {
        //                    more: (params.page * 10) < data.result.totalCount
        //                }
        //            };
        //        },
        //        cache: true
        //    },
        //    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //    minimumInputLength: 0
        //});

        $('#CreateNewTipoRespostaTipoRespostaConfiguracaoButton').click(function (e) {
            e.preventDefault();
            _createOrEditRelacaoModal.open({ tipoRespostaPrincipalId: $('#tipo-resposta-id-principal').val() });
        });

        $('#ExportarTiposRespostasTiposRespostasConfiguracoesParaExcelButton').click(function () {
            _tipoRespostaTipoRespostaConfiguracaoService
                .listarParaExcel({
                    filtro: $('#TiposRespostasTiposRespostasConfiguracoesTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposRespostasTiposRespostasConfiguracoesButton', '#RefreshTiposRespostasTiposRespostasConfiguracoesListButton').click(function (e) {
            e.preventDefault();
            getTiposRespostasTiposRespostasConfiguracoes();
        });

        abp.event.on('app.CriarOuEditarTipoRespostaTipoRespostaConfiguracaoModalSaved', function () {
            getTiposRespostasTiposRespostasConfiguracoes();
        });

        getTiposRespostasTiposRespostasConfiguracoes();

    };
})(jQuery);