(function ($) {
    app.modals.CriarOuEditarTipoRespostaConfiguracaoModal = function () {
        localStorage["FecharModal"] = false;
        var _tiposRespostasConfiguracoesService = abp.services.app.tipoRespostaConfiguracao;
        var _tipoRespostaConfiguracaoElementoHtmlService = abp.services.app.tipoRespostaConfiguracaoElementoHtml;
        var _modalManager;
        var _$TipoRespostaConfiguracaoInformationForm = null;
        var _$TiposRespostasConfiguracoesElementosHtmlTable = $('#TiposRespostasConfiguracoesElementosHtmlTable');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Delete')
        };

        var _createOrEditRelacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposRespostasConfiguracoes/_CriarOuEditarRelacaoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostasConfiguracoes/_CriarOuEditarRelacaoModal.js',
            modalClass: 'CriarOuEditarTipoRespostaConfiguracaoElementoHtmlModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$TipoRespostaConfiguracaoInformationForm = _modalManager.getModal().find('form[name=TipoRespostaConfiguracaoInformationsForm]');
            _$TipoRespostaConfiguracaoInformationForm.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
            if ($('#tipo-resposta-configuracao-id-principal').val() > 0) {
                $('#tabs-content').removeClass('hidden');
            }
        };

        this.save = function () {
            if (!_$TipoRespostaConfiguracaoInformationForm.valid()) {
                return;
            }
            var tipoRespostaConfiguracao = _$TipoRespostaConfiguracaoInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _tiposRespostasConfiguracoesService.criarOuEditar(tipoRespostaConfiguracao)
                 .done(function (data) {
                     $('#tipo-resposta-configuracao-id-principal').val(data.id);
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     if (localStorage["FecharModal"] == "true") {
                         _modalManager.close();
                     }
                     abp.event.trigger('app.CriarOuEditarTipoRespostaConfiguracaoModalSaved');
                     $('#tabs-content').removeClass('hidden');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function deleteTipoRespostaConfiguracaoElementoHtml(tipoRespostaConfiguracao) {
            abp.message.confirm(
                app.localize('DeleteWarning', tipoRespostaConfiguracao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _tiposRespostasConfiguracoesService.excluir(tipoRespostaConfiguracao)
                            .done(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                getTiposRespostasConfiguracoesElementosHtml();
                            });
                    }
                }
            );
        }

        function getTiposRespostasConfiguracoesElementosHtml() {
            _$TiposRespostasConfiguracoesElementosHtmlTable.jtable('load', {
                filtro: $('#TiposRespostasConfiguracoesElementosHtmlTableFilter').val(),
                tipoRespostaConfiguracaoId: $('#tipo-resposta-configuracao-id-principal').val()
            });
        }

        _$TiposRespostasConfiguracoesElementosHtmlTable.jtable({
            title: app.localize('TipoRespostaConfiguracaoElementoHtml'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _tipoRespostaConfiguracaoElementoHtmlService.listarPorTipoRespostaConfiguracao
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
                                        tipoRespostaConfiguracaoId: $('#tipo-resposta-configuracao-id-principal').val(),
                                        id: data.record.id
                                    });
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteTipoRespostaConfiguracaoElementoHtml(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%',
                    display: function (data) {
                        return data.record.elementoHtml.codigo
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%',
                    display: function (data) {
                        return data.record.elementoHtml.descricao
                    }
                },
            }
        });

        $('#CreateNewTipoRespostaConfiguracaoElementoHtmlButton').click(function (e) {
            e.preventDefault();
            _createOrEditRelacaoModal.open({ tipoRespostaConfiguracaoId: $('#tipo-resposta-configuracao-id-principal').val() });
        });

        $('#ExportarTiposRespostasConfiguracoesElementosHtmlParaExcelButton').click(function () {
            _tipoRespostaConfiguracaoElementoHtmlService
                .listarParaExcel({
                    filtro: $('#TiposRespostasConfiguracoesElementosHtmlTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposRespostasConfiguracoesElementosHtmlButton', '#RefreshTiposRespostasConfiguracoesElementosHtmlListButton').click(function (e) {
            e.preventDefault();
            getTiposRespostasConfiguracoesElementosHtml();
        });

        abp.event.on('app.CriarOuEditarTipoRespostaConfiguracaoElementoHtmlModalSaved', function () {
            getTiposRespostasConfiguracoesElementosHtml();
        });

        getTiposRespostasConfiguracoesElementosHtml();
    };
})(jQuery);