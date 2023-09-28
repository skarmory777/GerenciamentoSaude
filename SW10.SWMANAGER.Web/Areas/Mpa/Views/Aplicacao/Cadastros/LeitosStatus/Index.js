(function () {
    $(function () {
        var _$LeitosStatusTable = $('#LeitosStatusTable');
        var _LeitosStatusService = abp.services.app.leitoStatus;
        var _$filterForm = $('#LeitosStatusFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoStatus.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoStatus.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoStatus.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LeitosStatus/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/LeitosStatus/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarLeitoStatusModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/LeitosStatus/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$LeitosStatusTable.jtable({

            title: app.localize('LeitosStatus'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _LeitosStatusService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
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
                                    deleteLeitosStatus(data.record);
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
                },
                isBloqueioAtendimento: {
                    title: app.localize('IsBloqueio'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.isBloqueioAtendimento) {
                            return '<div style="text-align:center;">' + '<span class="label label-success content-center text-center">' + app.localize('Yes') + '</span>' + '</div>';
                        } else {
                            return '<div style="text-align:center;">' + '<span class="label label-default content-center text-center">' + app.localize('No') + '</span>' + '</div>';
                        }
                    }
                    
                }
                ,
                cor: {
                    title: app.localize('Cor'),
                    width: '8%',
                    display: function (data) {
                        var cor = data.record.cor;
                        return '<div style="text-align:center;">   <span style="display:inline-block; width:20px; height:20px;  text-align:center; background-color: ' + cor + '; border-radius: 25px;">  </span>  </div>  ';
                    }

                }
            }
        });

        function getLeitosStatus(reload) {
            if (reload) {
                _$LeitosStatusTable.jtable('reload');
            } else {
                _$LeitosStatusTable.jtable('load', {
                    filtro: $('#LeitosStatusTableFilter').val()
                });
            }
        }

        function deleteLeitosStatus(LeitoStatus) {

            abp.message.confirm(
                app.localize('DeleteWarning', LeitoStatus.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _LeitosStatusService.excluir(LeitoStatus)
                            .done(function () {
                                getLeitosStatus(true);
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

        $('#CreateNewLeitoStatusButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarLeitosStatusParaExcelButton').click(function () {
            _LeitosStatusService
                .listarParaExcel({
                    filtro: $('#LeitosStatusTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetLeitosStatusButton, #RefreshLeitosStatusListButton').click(function (e) {
            e.preventDefault();
            getLeitosStatus();
        });

        abp.event.on('app.CriarOuEditarLeitoStatusModalSaved', function () {
            getLeitosStatus(true);
        });

        getLeitosStatus();

        $('#LeitosStatusTableFilter').focus();
    });
})();