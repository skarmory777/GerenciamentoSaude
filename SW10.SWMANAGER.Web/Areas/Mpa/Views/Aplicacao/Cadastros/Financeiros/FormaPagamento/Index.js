(function () {
    $(function () {
        var _$formaPagamentoTable = $('#FormaPagamentoTable');
        var _formaPagamento = abp.services.app.formaPagamento;
        var _$formaPagamentoFilterForm = $('#formaPagamentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.FormaPagamento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.FormaPagamento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.FormaPagamento.Delete')
        };

    

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FormaPagamentos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/FormaPagamento/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFormaPagamentoModal'
        });




        _$formaPagamentoTable.jtable({

            title: app.localize('FormaPagamento'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _formaPagamento.listar
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
                                    deleteFormaPagamento(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '33%'
                },
             
                descricao: {
                    title: app.localize('Descricao'),
                    width: '66%'
                }
            }
        });

        function getFormaPagamentos(reload) {
            if (reload) {
                _$formaPagamentoTable.jtable('reload');
            } else {
                _$formaPagamentoTable.jtable('load', {
                    filtro: $('#formaPagamentoTableFilter').val()
                });
            }
        }

        function deleteFormaPagamento(formaPagamento) {

            abp.message.confirm(
                app.localize('DeleteWarning', formaPagamento.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _formaPagamento.excluir(formaPagamento)
                            .done(function () {
                                getFormaPagamentos(true);
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

        $('#CreateNewFormaPagamentoButton').click(function () {
            _createOrEditModal.open();
        });

      
        $('#GetFeriadosButton, #RefreshFeriadosListButton').click(function (e) {
            e.preventDefault();
            getFormaPagamentos();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getFormaPagamentos(true);
        });

        getFormaPagamentos();

        $('#FeriadosTableFilter').focus();


        function deleteFeriados(feriado) {

            abp.message.confirm(
                app.localize('DeleteWarning', feriado.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _FeriadosService.excluir(feriado)
                            .done(function () {
                                getFeriados(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

    });
})();