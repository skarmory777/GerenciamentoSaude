(function () {
    $(function () {
        var _$ModelosLaudosTable = $('#ModelosLaudosTable');
        var _ModelosLaudosService = abp.services.app.modeloLaudo;
        var _$filterForm = $('#ModelosLaudosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.LaudoMedico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.LaudoMedico.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.LaudoMedico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ModelosLaudos/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/ModelosLaudos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModeloLaudoModal'
        });

        _$ModelosLaudosTable.jtable({

            title: app.localize('ModelosLaudos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ModelosLaudosService.listar
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
                                    deleteModelosLaudos(data.record);
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
                ,
                grupo: {
                    title: app.localize('Grupo'),
                    width: '20%',
                    display: function (data) {
                        if (data.record.laudoGrupo) {
                            return data.record.laudoGrupo.descricao;
                        }
                    }
                }
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

        function getModelosLaudos(reload) {
            if (reload) {
                _$ModelosLaudosTable.jtable('reload');
            } else {
                _$ModelosLaudosTable.jtable('load', {
                    filtro: $('#ModelosLaudosTableFilter').val()
                });
            }
        }

        function deleteModelosLaudos(modeloLaudo) {

            abp.message.confirm(
                app.localize('DeleteWarning', modeloLaudo.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ModelosLaudosService.excluir(modeloLaudo)
                            .done(function () {
                                getModelosLaudos(true);
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

        $('#CreateNewModeloLaudoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarModelosLaudosParaExcelButton').click(function () {
            _ModelosLaudosService
                .listarParaExcel({
                    filtro: $('#ModelosLaudosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetModelosLaudosButton, #RefreshModelosLaudosListButton').click(function (e) {
            e.preventDefault();
            getModelosLaudos();
        });

        abp.event.on('app.CriarOuEditarModeloLaudoModalSaved', function () {
            getModelosLaudos(true);
        });

        getModelosLaudos();

        $('#ModelosLaudosTableFilter').focus();
    });
})();