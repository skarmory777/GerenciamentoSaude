﻿(function () {
    $(function () {
        var _$meioPagamentoTable = $('#meioPagamentoTable');
        var _meioPagamentoService = abp.services.app.meioPagamento;
        var _$meioPagamentoFilterForm = $('#meioPagamentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.MeioPagamento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.MeioPagamento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.MeioPagamento.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/MeiosPagamentos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/MeioPagamento/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMeioPagamentoModal'
        });


        _$meioPagamentoTable.jtable({

            title: app.localize('MeioPagamento'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _meioPagamentoService.listar
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
                                    deleteRegistro(data.record);
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

        function getRegistros(reload) {
            if (reload) {
                _$meioPagamentoTable.jtable('reload');
            } else {
                _$meioPagamentoTable.jtable('load', {
                    filtro: $('#tableFilter').val()
                });
            }
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _meioPagamentoService.excluir(record)
                            .done(function () {
                                getRegistros(true);
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

        $('#CreateNewButton').click(function () {
            _createOrEditModal.open();
        });

      
        $('#GetFeriadosButton, #RefreshFeriadosListButton').click(function (e) {
            e.preventDefault();
            getRegistros();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getRegistros(true);
        });

        getRegistros();

        $('#tableFilter').focus();



    });
})();