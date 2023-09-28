﻿(function () {
    $(function () {
        var _$contaAdministrativa = $('#contaAdministrativaTable');
        var _contaAdministrativaService = abp.services.app.contaAdministrativa;
        var _$contaAdministrativaFilterForm = $('#contaAdministrativaFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.ContaAdministrativa.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.ContaAdministrativa.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.ContaAdministrativa.Delete')
        };

    

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ContasAdministrativas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/ContaAdministrativa/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarContaAdministrativaModal'
        });




        _$contaAdministrativa.jtable({

            title: app.localize('ContaAdministrativa'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _contaAdministrativaService.listar
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
                _$contaAdministrativa.jtable('reload');
            } else {
                _$contaAdministrativa.jtable('load', {
                    filtro: $('#tableFilter').val()
                });
            }
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _contaAdministrativaService.excluir(record)
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