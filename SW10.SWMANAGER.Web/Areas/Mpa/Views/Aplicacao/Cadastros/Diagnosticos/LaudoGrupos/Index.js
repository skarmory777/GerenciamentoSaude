(function () {
    $(function () {
        var _$LaudoGruposTable = $('#LaudoGruposTable');
        var _LaudoGruposService = abp.services.app.laudoGrupo;
        var _$filterForm = $('#LaudoGruposFilterForm');

        var _permissions = {
            create:true,// abp.auth.hasPermission('Pages.Tenant.Assistencial.LaudoMedico.Create'),
            edit: true,//abp.auth.hasPermission('Pages.Tenant.Assistencial.LaudoMedico.Edit'),
            'delete': true//abp.auth.hasPermission('Pages.Tenant.Assistencial.LaudoMedico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LaudoGrupos/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/LaudoGrupos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarLaudoGrupoModal'
        });

        _$LaudoGruposTable.jtable({

            title: app.localize('LaudoGrupos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _LaudoGruposService.listar
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
                     //   if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                     //   }
                      //  if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteLaudoGrupos(data.record);
                                });
                   //     }

                        return $span;
                    }
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                }
                //,
                //grupo: {
                //    title: app.localize('Grupo'),
                //    width: '20%',
                //    display: function (data) {
                //        if (data.record.grupo) {
                //            return data.record.grupo.descricao;
                //        }
                //    }
                //}
                ,
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '30%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getLaudoGrupos(reload) {
            if (reload) {
                _$LaudoGruposTable.jtable('reload');
            } else {
                _$LaudoGruposTable.jtable('load', {
                    filtro: $('#LaudoGruposTableFilter').val()
                });
            }
        }

        function deleteLaudoGrupos(modeloLaudo) {

            abp.message.confirm(
                app.localize('DeleteWarning', modeloLaudo.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _LaudoGruposService.excluir(modeloLaudo)
                            .done(function () {
                                getLaudoGrupos(true);
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

        $('#CreateNewLaudoGrupoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarLaudoGruposParaExcelButton').click(function () {
            _LaudoGruposService
                .listarParaExcel({
                    filtro: $('#LaudoGruposTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetLaudoGruposButton, #RefreshLaudoGruposListButton').click(function (e) {
            e.preventDefault();
            getLaudoGrupos();
        });

        abp.event.on('app.CriarOuEditarLaudoGrupoModalSaved', function () {
            getLaudoGrupos(true);
        });

        getLaudoGrupos();

        $('#LaudoGruposTableFilter').focus();
    });
})();