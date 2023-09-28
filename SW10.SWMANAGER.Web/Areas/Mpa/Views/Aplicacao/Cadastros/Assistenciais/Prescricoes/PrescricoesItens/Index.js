(function () {
    $(function () {
        var _$prescricoesItensTable = $('#PrescricoesItensTable');
        var _prescricoesItensService = abp.services.app.prescricaoItem;
        var _$filterForm = $('#PrescricoesItensFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItem.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItem.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItem.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PrescricoesItens/CriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPrescricaoItemModal'
        });

        var _produtosModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PrescricoesItens/_Produtos',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_produtos.js',
            modalClass: 'ProdutosModal'
        });

        var _laboratoriosModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PrescricoesItens/_Laboratorios',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_laboratorios.js',
            modalClass: 'LaboratoriosModal'
        });

        var _imagensModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PrescricoesItens/_Imagens',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_imagens.js',
            modalClass: 'ImagensModal'
        });


        _$prescricoesItensTable.jtable({
            title: app.localize('PrescricaoItem'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _prescricoesItensService.listar
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
                                .click(function (e) {
                                    e.preventDefault();
                                    //_createOrEditModal.open({ id: data.record.id });
                                    localStorage.removeItem("FormulaEstoqueList");
                                    localStorage.removeItem("FormulaFaturamentoList");
                                    localStorage.removeItem("FormulaExameLaboratorialList");
                                    localStorage.removeItem("FormulaExameImagemList");
                                    location.href = '/mpa/prescricoesitens/CriarOuEditar/' + data.record.id;
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deletePrescricoesItens(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '20%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '60%'
                },
            }
        });


        $("#PrescricoesItensTableFilter").on('keypress', function (e) {
            e.stopImmediatePropagation();
            if (e.which === 13) {
                getPrescricoesItens();
                return false;
            }
        });

        function getPrescricoesItens() {
            _$prescricoesItensTable.jtable('load', {
                filtro: $('#PrescricoesItensTableFilter').val()
            });
        }

        function deletePrescricoesItens(prescricaoItem) {
            abp.message.confirm(
                app.localize('DeleteWarning', prescricaoItem.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _prescricoesItensService.excluir(prescricaoItem.id)
                            .done(function () {
                                getPrescricoesItens(true);
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

        $('#CreateNewPrescricaoItemButton').click(function (e) {
            e.preventDefault();
            localStorage.removeItem("FormulaEstoqueList");
            localStorage.removeItem("FormulaFaturamentoList");
            localStorage.removeItem("FormulaExameLaboratorialList");
            localStorage.removeItem("FormulaExameImagemList");
            //_createOrEditModal.open();
            location.href = '/mpa/prescricoesitens/CriarOuEditar'
        });

        $('#ExportarPrescricoesItensParaExcelButton').click(function () {
            _prescricoesItensService
                .listarParaExcel({
                    filtro: $('#PrescricoesItensTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetPrescricoesItensButton, #RefreshPrescricoesItensListButton').click(function (e) {
            e.preventDefault();
            getPrescricoesItens();
        });

        abp.event.on('app.CriarOuEditarPrescricaoItemModalSaved', function () {
            getPrescricoesItens();
        });

        abp.event.on('app.ProdutosModalSaved', function () {
            getPrescricoesItens();
        });

        getPrescricoesItens();

        $('#PrescricoesItensTableFilter').focus();

        $('#produtos').on('click', function (e) {
            e.preventDefault();
            _produtosModal.open();
        });

        $('#exames-laboratoriais').on('click', function (e) {
            e.preventDefault();
            _laboratoriosModal.open();
        });

        $('#exames-imagem').on('click', function (e) {
            e.preventDefault();
            _imagensModal.open();
        });
    });
})();