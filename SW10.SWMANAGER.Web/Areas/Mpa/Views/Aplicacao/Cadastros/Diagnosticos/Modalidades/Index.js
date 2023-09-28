(function () {
    $(function () {
        var _$ModalidadesTable = $('#ModalidadesTable');
        var _ModalidadesService = abp.services.app.modalidade;
        var _$filterForm = $('#ModalidadesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Cadastros.Modalidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Cadastros.Modalidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Cadastros.Modalidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Modalidades/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/Modalidades/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModalidadeModal'
        });

        _$ModalidadesTable.jtable({
            
            title: app.localize('Modalidades'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ModalidadesService.listar
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
                  //      if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                    //    }

                      //  if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteModalidades(data.record);
                                });
                        //}

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Código'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
                parecer: {
                    title: app.localize('Parecer'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.isParecer) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },

            }
        });

        function getModalidades(reload) {
            if (reload) {
                _$ModalidadesTable.jtable('reload');
            } else {
                _$ModalidadesTable.jtable('load', {
                    filtro: $('#ModalidadesTableFilter').val()
                });
            }
        }

        function deleteModalidades(Modalidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', Modalidade.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ModalidadesService.excluir(Modalidade)
                            .done(function () {
                                getModalidades(true);
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

        $('#CreateNewModalidadeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarModalidadesParaExcelButton').click(function () {
            _ModalidadesService
                .listarParaExcel({
                    filtro: $('#ModalidadesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetModalidadesButton, #RefreshModalidadesListButton').click(function (e) {
            e.preventDefault();
            getModalidades();
        });

        abp.event.on('app.CriarOuEditarModalidadeModalSaved', function () {
            getModalidades(true);
        });

        getModalidades();

        $('#ModalidadesTableFilter').focus();
    });
})();