(function () {
    $(function () {
        var _$GruposCIDTable = $('#GruposCIDTable');
        var _GruposCIDService = abp.services.app.grupoCID;
        var _$filterForm = $('#GruposCIDFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCID.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCID.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCID.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GruposCID/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/GruposCID/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarGrupoCIDModal'
        });

        _$GruposCIDTable.jtable({

            title: app.localize('GrupoCID'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _GruposCIDService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '33%',
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
                                    deleteGruposCID(data.record);
                                });
                        }

                        return $span;
                    }
                },
                diaMesAno: {
                    title: app.localize('DataGrupoCID'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.diaMesAno).format('L');
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '33%'
                }
            }
        });

        function getGruposCID(reload) {
            if (reload) {
                _$GruposCIDTable.jtable('reload');
            } else {
                _$GruposCIDTable.jtable('load', {
                    filtro: $('#GruposCIDTableFilter').val()
                });
            }
        }

        function deleteGruposCID(grupoCID) {

            abp.message.confirm(
                app.localize('DeleteWarning', grupoCID.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _GruposCIDService.excluir(grupoCID)
                            .done(function () {
                                getGruposCID(true);
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

        $('#CreateNewGrupoCIDButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarGruposCIDParaExcelButton').click(function () {
            _GruposCIDService
                .listarParaExcel({
                    filtro: $('#GruposCIDTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetGruposCIDButton, #RefreshGruposCIDListButton').click(function (e) {
            e.preventDefault();
            getGruposCID();
        });

        abp.event.on('app.CriarOuEditarGrupoCIDModalSaved', function () {
            getGruposCID(true);
        });

        getGruposCID();

        $('#GruposCIDTableFilter').focus();
    });
})();