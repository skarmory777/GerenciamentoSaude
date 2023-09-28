(function () {
    $(function () {
        var _$BrasApresentacoesTable = $('#BrasApresentacoesTable');
        var _BrasApresentacoesService = abp.services.app.faturamentoBrasApresentacao;
        var _$filterForm = $('#BrasApresentacoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasApresentacoes.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasApresentacoes.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasApresentacoes.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoBrasApresentacoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasApresentacoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoBrasApresentacaoModal'
        });
;
        _$BrasApresentacoesTable.jtable({

            title: app.localize('BrasApresentacoes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _BrasApresentacoesService.listar
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
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteBrasApresentacoes(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                codigo: {
                    title: app.localize('Codigo'),
                    width: '15%'
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '45%'
                }
            }
        });

        function getBrasApresentacoes(reload) {
            if (reload) {
                _$BrasApresentacoesTable.jtable('reload');
            } else {
                _$BrasApresentacoesTable.jtable('load', {
                    filtro: $('#BrasApresentacoesTableFilter').val()
                });
            }
        }

        function deleteBrasApresentacoes(brasApresentacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', brasApresentacao.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _BrasApresentacoesService.excluir(brasApresentacao)
                            .done(function () {
                                getBrasApresentacoes(true);
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

        $('#CreateNewBrasApresentacaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarBrasApresentacoesParaExcelButton').click(function () {
            _BrasApresentacoesService
                .listarParaExcel({
                    filtro: $('#BrasApresentacoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetBrasApresentacoesButton, #RefreshBrasApresentacoesListButton').click(function (e) {
            e.preventDefault();
            getBrasApresentacoes();
        });

        abp.event.on('app.CriarOuEditarBrasApresentacaoModalSaved', function () {
            getBrasApresentacoes(true);
        });

        getBrasApresentacoes();

        $('#BrasApresentacoesTableFilter').focus();

        // Salvar BrasApresentacao
        //function salvarBrasApresentacao() {

        //    //var brasApresentacaoAppService = abp.

        //    _createOrEditModal.save();

        //    //var _$BrasApresentacaoInformationForm = $('form[name=BrasApresentacaoInformationsForm]');
        //    //var formData = _$BrasApresentacaoInformationForm.serialize();

        //    //var metodo = '/Mpa/FaturamentoBrasApresentacoes/SalvarBrasApresentacao';
        //    //$.ajax({
        //    //    type: "POST",
        //    //    url: metodo,
        //    //    dataType: 'text',
        //    //    data: formData,
        //    //    success: function (result) {
        //    //        ////console.log('brasApresentacao salvo');
        //    //        //$('#btn-fechar-modal').click();
        //    //        //$('#todas').click();
        //    //    },
        //    //    error: function (xhr, ajaxOptions, thrownError) {
        //    //        abp.notify.info(app.localize('Erro'));
        //    //    },
        //    //    beforeSend: function () {
        //    //    },
        //    //    complete: function () { }
        //    //});
        //}

    });
})();