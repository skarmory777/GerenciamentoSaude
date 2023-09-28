(function () {
    $(function () {
        var _$LeitoServicosTable = $('#LeitoServicosTable');
        var _LeitoServicosService = abp.services.app.leitoServico;
        var _$filterForm = $('#LeitoServicosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoServico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoServico.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoServico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LeitoServicos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/LeitoServicos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarLeitoServicoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/LeitoServicos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$LeitoServicosTable.jtable({

            title: app.localize('LeitoServicos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _LeitoServicosService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '7%',
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
                                    deleteLeitoServicos(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                codigo: {
                    title: app.localize('Codigo'),
                    width: '8%'
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                }
                ,
                ramal: {
                    title: app.localize('Ramal'),
                    width: '10%'
                }
                //,
                //isBloqueioAtendimento: {
                //    title: app.localize('IsBloqueio'),
                //    width: '8%',
                //    display: function (data) {
                //        var $span = $('<span></span>');
                //        if (data.record.isBloqueioAtendimento == true) {
                //           $('<input id="chk-isbloqueio" class="md-check" type="checkbox" value="true" checked="true" />')
                //                .appendTo($span);
                //        }
                //        else {
                //            $('<input id="chk-isbloqueio" class="md-check" type="checkbox" value="false" checked="false" />')
                //                 .appendTo($span);
                //        }
                //        return $span;
                //    }
                //}
            }
        });

        function getLeitoServicos(reload) {
            if (reload) {
                _$LeitoServicosTable.jtable('reload');
            } else {
                _$LeitoServicosTable.jtable('load', {
                    filtro: $('#LeitoServicosTableFilter').val()
                });
            }
        }

        function deleteLeitoServicos(LeitoServico) {

            abp.message.confirm(
                app.localize('DeleteWarning', LeitoServico.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _LeitoServicosService.excluir(LeitoServico)
                            .done(function () {
                                getLeitoServicos(true);
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

        $('#CreateNewLeitoServicoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarLeitoServicosParaExcelButton').click(function () {
            _LeitoServicosService
                .listarParaExcel({
                    filtro: $('#LeitoServicosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetLeitoServicosButton, #RefreshLeitoServicosListButton').click(function (e) {
            e.preventDefault();
            getLeitoServicos();
        });

        abp.event.on('app.CriarOuEditarLeitoServicoModalSaved', function () {
            getLeitoServicos(true);
        });

        getLeitoServicos();

        $('#LeitoServicosTableFilter').focus();
    });
})();