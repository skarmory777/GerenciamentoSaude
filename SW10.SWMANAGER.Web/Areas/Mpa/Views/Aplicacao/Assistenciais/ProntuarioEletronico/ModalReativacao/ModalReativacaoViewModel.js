(function ($) {

    app.modals.ReativacaoProntuarioEletronicoModal = function () {
        const tbListagemProntuariosInativos = $("#tbListagemProntuariosInativos");
        const prontuarioEletronicoAppService = abp.services.app.prontuarioEletronico;
        let _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $('.modal-dialog').css('min-width', '80vw');
            
            tbListagemProntuariosInativos.jtable({
                paging: true,
                sorting: true,
                multiSorting: true,
                selecting: true,
                selectingCheckboxes: true,
                actions: {
                    listAction: {
                        method: prontuarioEletronicoAppService.listarInativos
                    }
                },
                fields: {
                    id: {
                        key: true,
                        list: false
                    },
                    actions: {
                        title: app.localize('Actions'),
                        width: '10%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Reativar') + '"><i class="fa fa-check-circle"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _modalJustificativa.open({ prontuarioEletronicoId: data.record.id });
                                });
                            return $span;
                        }
                    },
                    codigo: {
                        title: app.localize('Formulario'),
                        width: '10%',
                        display: function (data) {
                            return data.record.formulario;
                        }
                    },
                    dataAdmissao: {
                        title: app.localize('Data'),
                        width: '10%',
                        display: function (data) {
                            return moment(data.record.dataAdmissao).format('L LT');
                        }
                    },
                    medico: {
                        title: app.localize('Medico'),
                        width: '15%',

                    },
                    'AssProntuario.CreatorUserId': {
                        title: app.localize('Usuário'),
                        width: '15%',
                        display: function (data) {
                            return app.localize(data.record.usuario);
                        }
                    },
                    unidadeOrganizacional: {
                        title: app.localize('UnidadeOrganizacional'),
                        width: '15%',

                    },
                },
                selectionChanged: function () {
                    var $selectedRows = tbListagemProntuariosInativos.jtable('selectedRows');

                    if ($selectedRows.length > 0) {
                        $('#criarRegistroButton').enable(true);

                        $selectedRows.each(function () {
                            var record = $(this).data('record');
                            exibirRelatorio(record.id);
                        });
                    }
                },
                recordsLoaded: function (event, data) {
                    selectRecord();
                }
            });
            reloadTable(_modalManager.getArgs().operacaoId, _modalManager.getArgs().atendimentoId);


            abp.event.off('UpdateModalJustificativaViewModel', UpdateModalJustificativaViewModel)
            abp.event.on('UpdateModalJustificativaViewModel', UpdateModalJustificativaViewModel)
        };

        var reloadTable = function (operacaoId, atendimentoId) {
            if (tbListagemProntuariosInativos.length != 0) {
                tbListagemProntuariosInativos.jtable('load', { operacaoId, atendimentoId })
                selectRecord();
            }
        };

        function selectRecord() {
            if (tbListagemProntuariosInativos.find(".jtable-main-container tr.jtable-data-row:first input[type=checkbox]").length === 0) {
                return exibirRelatorio(0);
            }
            tbListagemProntuariosInativos.find(".jtable-main-container tr.jtable-data-row:first input[type=checkbox]").trigger('click');
        }

        function UpdateModalJustificativaViewModel() {
            reloadTable(_modalManager.getArgs().operacaoId, _modalManager.getArgs().atendimentoId);
        }

        function exibirRelatorio(registroId) {
            var action = "ObterArquivoNomePorIdEOperacao?registroId=" + registroId + "&operacaoId=" + sessionStorage["OperacaoId"];
            var caminho = "/mpa/RegistroArquivo/" + action;
            $.post(caminho).then(res => {
                var path = window.location.href.split(window.location.pathname)[0].split("//")[1];
                $("#file-frame-modal-reativacao").attr("src", "//" + path + "/libs/pdfjs/web/viewer.html?file=" + res + "&locale=pt-BR");
            });
        }

        var _modalJustificativa = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProntuariosEletronicos/JustificativaProntuarioEletronico',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/ModalJustificativa/ModalJustificativaViewModel.js',
            modalClass: 'JustificativaProntuarioEletronicoModal'
        });
    }
})(jQuery);