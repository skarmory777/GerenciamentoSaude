(function () {
    $(function () {
           
            var _$filterForm = $('#PreAtendimentosFilterForm');

            //novo serviço pablo 20/10/2017
            var _AtendimentosService = abp.services.app.atendimento;

            var _permissions = {
                create: abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Create'),
                edit: abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Edit'),
                'delete': abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Delete')
            };

            var _createOrEditModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/PreAtendimentos/CriarOuEditarModal',
                scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/PreAtendimentos/_CriarOuEditarModal.js',
                modalClass: 'CriarOuEditarPreAtendimentoModal'
            });

            var _userPermissionsModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
                scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/PreAtendimentos/_PermissionsModal.js',
                modalClass: 'UserPermissionsModal'
            });

            var _$PreAtendimentosTable = $('#PreAtendimentosTable');

            _$PreAtendimentosTable.jtable({
                title: app.localize('PreAtendimentos'),
                paging: true,
                sorting: true,
                multiSorting: true,
                actions: {
                    listAction: {
                        method: _AtendimentosService.listarFiltroPreAtendimento
                    }
                },
                fields: {
                    id: {
                        key: true,
                        list: false
                    }
                    ,
                    actions: {
                        title: app.localize('Actions'),
                        width: '5%',
                        sorting: false,
                        display: function (data) {
                        var $span = $('<span></span>');

                        //if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="' +app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                           .appendTo($span)
                           .click(function () {
                               //waitingDialog.show();
                                   //editarAtendimento(data.record);
                               _createOrEditModal.open({ id: data.record.id
                                   });
                               });

                               //}
                    //if (_permissions.delete) {
                    $('<button class="btn btn-default btn-xs" title="' +app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                        .appendTo($span)
                            .click(function () {
                            deletePreAtendimentos(data.record);
                });
                        return $span;
                    }
                    },


                    tipoAtendimento: {
                        title: app.localize('TipoAtendimento'),
                        width: '7%',
                        display: function (data) {
                            if (data.record.atendimentoTipo) {
                                return data.record.atendimentoTipo.descricao;
                            }
                        }
                    }
                    ,
                    paciente: {
                        title: app.localize('Paciente'),
                        width: '9%',
                        display: function (data) {
                            if (data.record.paciente) {
                                return data.record.paciente.nomeCompleto;
                            }
                        }
                    }
                    ,
                    dataRegistro: {
                        title: app.localize('DataPreAtendimento'),
                        width: '3%',
                        display: function (data) {
                            return moment(data.record.dataPreatendimento).format('L');
                        }
                    }
                    ,
                    observacao: {
                        title: app.localize('Observacao'),
                        width: '9%'
                        ,
                        display: function (data) {
                            if (data.record) {
                                return data.record.observacao;
                            }
                        }
                    }
                }
            });

            function getPreAtendimentos(reload) {
                if (reload) {
                    _$PreAtendimentosTable.jtable('reload');
                } else {
                    _$PreAtendimentosTable.jtable('load', {
                        filtro: $('#PreAtendimentosTableFilter').val()
                    });
                }
            }

            function deletePreAtendimentos(PreAtendimento) {
                abp.message.confirm(
                    app.localize('DeleteWarning', PreAtendimento.nomeCompleto),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            _AtendimentosService.excluir(PreAtendimento.id)
                                .done(function () {
                                    getPreAtendimentos(true);
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

            $('#CreateNewPreAtendimentoButton').click(function () {
                _createOrEditModal.open();
            });

            $('#ExportarPreAtendimentosParaExcelButton').click(function () {
                _PreAtendimentosService
                    .listarParaExcel({
                        filtro: $('#PreAtendimentosTableFilter').val(),
                        //sorting: $(''),
                        maxResultCount: $('span.jtable-page-size-change select').val()
                    })
                    .done(function (result) {
                        app.downloadTempFile(result);
                    });
            });

            $('#GetPreAtendimentosButton, #RefreshPreAtendimentosListButton').click(function (e) {
                e.preventDefault();
                getPreAtendimentos();
            });

            abp.event.on('app.CriarOuEditarPreAtendimentoModalSaved', function () {
                getPreAtendimentos(true);
            });

            getPreAtendimentos();

            $('#PreAtendimentosTableFilter').focus();
    });
})();
