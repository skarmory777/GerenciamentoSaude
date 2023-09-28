(function () {
    $(function () {
        var _$AutorizacoesTable = $('#AutorizacaoTable');
        var _AutorizacoesService = abp.services.app.faturamentoAutorizacao;
        var _$filterForm = $('#AutorizacoesFilterForm');

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoAutorizacoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Autorizacoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoAutorizacaoModal'
        });

        _$AutorizacoesTable.jtable({

            title: app.localize('Autorizacoes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: { listAction: { method: _AutorizacoesService.listar } },
            fields: {
                id: { key: true, list: false },

                actions: {
                    title: app.localize('Actions'),
                    width: '7%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                   //     if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                    //    }
                   //     if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteAutorizacoes(data.record);
                                });
                 //       }
                        return $span;
                    }
                }
                ,
                mensagem: {
                    title: app.localize('Mensagem'),
                    width: '34%'
                }
                ,
                vigencia: {
                    title: app.localize('Vigencia'),
                    width: '16%',
                    display: function (data) {
                        var dataInicial = moment(data.record.dataInicial).format('L');
                        var dataFinal;
                        if (data.record.dataFinal) {
                            dataFinal = moment(data.record.dataFinal).format('L LT');
                        } else {
                            dataFinal = 'N/A';
                        }
                        
                        var retorno = 'Início: ' + dataInicial + ' | ' + 'Fim: ' + dataFinal;
                        return retorno;
                    }
                }
                ,
                config: {
                    title: app.localize('Config'),
                    width: '12%',
                    display: function (data) {                       
                        var retorno;

                        if (data.record.isAutorizacao) {
                            retorno = 'Autorização.';
                        }
                        if (data.record.isLiberacao) {
                            retorno = 'Liberação.';
                        }
                        if (data.record.isJustificativa) {
                            retorno = 'Justificativa.';
                        }
                        if (data.record.IsBloqueio) {
                            retorno = 'Bloqueio.';
                        }

                        return retorno;
                    }
                }
                ,
                tipo: {
                    title: app.localize('Tipo'),
                    width: '8%',
                    display: function (data) {
                        var retorno;

                        if (data.record.isAmbulatorio) {
                            retorno = 'Ambulatório';
                        }
                        else if (data.record.isInternacao) {
                            retorno = 'Internação';
                        }
                  
                        return retorno;
                    }
                }
            }
        });

        function getAutorizacoes(reload) {
            if (reload) {
                _$AutorizacoesTable.jtable('reload');
            } else {
                _$AutorizacoesTable.jtable('load', {
                    filtro: $('#AutorizacoesTableFilter').val()
                });
            }
        }

        function deleteAutorizacoes(autorizacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', autorizacao.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _AutorizacoesService.excluir(autorizacao)
                            .done(function () {
                                getAutorizacoes(true);
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

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewAutorizacaoButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarAutorizacoesParaExcelButton').click(function () {
            _AutorizacoesService
                .listarParaExcel({
                    filtro: $('#AutorizacoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetAutorizacoesButton, #RefreshAutorizacoesListButton').click(function (e) {
            e.preventDefault();
            getAutorizacoes();
        });

        abp.event.on('app.CriarOuEditarAutorizacaoModalSaved', function () {
            getAutorizacoes(true);
        });

        getAutorizacoes();

        $('#AutorizacoesTableFilter').focus();
    });
})();