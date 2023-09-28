(function () {
    $(function () {
        var _$CapitulosCIDTable = $('#CapitulosCIDTable');
        var _CapitulosCIDService = abp.services.app.capituloCID;
        var _$filterForm = $('#CapitulosCIDFilterForm');

        var _permissions = {                
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.CapituloCID.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.CapituloCID.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.CapituloCID.Deletee')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/CapitulosCID/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/CapitulosCID/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarCapituloCIDModal'
        });

        _$CapitulosCIDTable.jtable({

            title: app.localize('CapituloCID'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _CapitulosCIDService.listar
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
                                    deleteCapitulosCID(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '33%'
                },
                criacao: {
                    title: app.localize('Criacao'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.criacao).format('L');
                    }
                }
               
            }
        });

        function getCapitulosCID(reload) {
            if (reload) {
                _$CapitulosCIDTable.jtable('reload');
            } else {
                _$CapitulosCIDTable.jtable('load', {
                    filtro: $('#CapitulosCIDTableFilter').val()
                });
            }
        }

        function deleteCapitulosCID(capituloCID) {

            abp.message.confirm(
                app.localize('DeleteWarning', capituloCID.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _CapitulosCIDService.excluir(capituloCID)
                            .done(function () {
                                getCapitulosCID(true);
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

        $('#CreateNewCapituloCIDButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarCapitulosCIDParaExcelButton').click(function () {
            _CapitulosCIDService
                .listarParaExcel({
                    filtro: $('#CapitulosCIDTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetCapitulosCIDButton, #RefreshCapitulosCIDListButton').click(function (e) {
            e.preventDefault();
            getCapitulosCID();
        });

        abp.event.on('app.CriarOuEditarCapituloCIDModalSaved', function () {
            getCapitulosCID(true);
        });

        getCapitulosCID();

        $('#CapitulosCIDTableFilter').focus();
    });
})();