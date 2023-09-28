(function () {
    $(function () {
        var _$ConsultorTabelaCamposTable = $('#ConsultorTabelaCamposTable');
        var _ConsultorTabelaCamposService = abp.services.app.consultorTabelaCampo;
        var _$filterForm = $('#ConsultorTabelaCamposFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ConsultorTabelaCampos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelaCampos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarConsultorTabelaCampoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelaCampos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        
        _$ConsultorTabelaCamposTable.jtable({

            title: app.localize('ConsultorTabelaCampos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ConsultorTabelaCamposService.listarConsultorTabelaCampos
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
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
                                    deleteConsultorTabelaCampos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '5%'
                },
                campo: {
                    title: app.localize('Campo'),
                    width: '12%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                },
                consultorTipoDadoNF: {
                    title: app.localize('TipoDadoNF'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.consultorTipoDadoNF)
                            return data.record.consultorTipoDadoNF.descricao;
                    }
                },
                ele: {
                    title: app.localize('Ele'),
                    width: '4%'
                },
                tamanho: {
                    title: app.localize('Tamanho'),
                    width: '4%'
                },
                observacao: {
                    title: app.localize('Observacao'),
                    width: '15%'
                }
            }
        });
        



        function getConsultorTabelaCampos(reload) {
            if (reload) {
                _$ConsultorTabelaCamposTable.jtable('reload');
            } else {
                _$ConsultorTabelaCamposTable.jtable('load', {
                    filtro: $('#ConsultorTabelaCamposTableFilter').val()
                });
            }
        }

        function deleteConsultorTabelaCampos(ConsultorTabelaCampo) {

            abp.message.confirm(
                app.localize('DeleteWarning', ConsultorTabelaCampo.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ConsultorTabelaCamposService.excluir(ConsultorTabelaCampo)
                            .done(function () {
                                getConsultorTabelaCampos(true);
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

        $('#CreateNewConsultorTabelaCampoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarConsultorTabelaCamposParaExcelButton').click(function () {
            _ConsultorTabelaCamposService
                .listarParaExcel({
                    filtro: $('#ConsultorTabelaCamposTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetConsultorTabelaCamposButton, #RefreshConsultorTabelaCamposListButton').click(function (e) {
            e.preventDefault();
            getConsultorTabelaCampos();
        });

        abp.event.on('app.CriarOuEditarConsultorTabelaCampoModalSaved', function () {
            getConsultorTabelaCampos(true);
        });

        getConsultorTabelaCampos();

        $('#ConsultorTabelaCamposTableFilter').focus();
    });
})();