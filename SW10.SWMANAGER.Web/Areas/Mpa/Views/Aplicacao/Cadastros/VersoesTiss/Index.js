(function () {
    $(function () {
        var _$VersoesTissTable = $('#VersoesTissTable');
        var _VersoesTissService = abp.services.app.versaoTiss;
        var _$filterForm = $('#VersoesTissFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.VersaoTiss.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.VersaoTiss.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.VersaoTiss.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/VersoesTiss/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/VersoesTiss/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarVersaoTissModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/VersoesTiss/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        
        _$VersoesTissTable.jtable({

            title: app.localize('VersoesTiss'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _VersoesTissService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
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
                                    deleteVersoesTiss(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },               
                dataInicio: {
                    title: app.localize('DataInicio'),
                    width: '15%'
                },
                dataFim: {
                    title: app.localize('DataFim'),
                    width: '15%'
                }
            }
        });
        
        function getVersoesTiss(reload) {

            if (reload) {
                _$VersoesTissTable.jtable('reload');
            } else {
                _$VersoesTissTable.jtable('load', {
                    filtro: $('#VersoesTissTableFilter').val()
                });
            }
        }

        function deleteVersoesTiss(VersaoTiss) {

            abp.message.confirm(
                app.localize('DeleteWarning', VersaoTiss.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _VersoesTissService.excluir(VersaoTiss)
                            .done(function () {
                                getVersoesTiss(true);
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

        //$('#ShowAdvancedFiltersSpan').click(function () {
        //    $('#ShowAdvancedFiltersSpan').hide();
        //    $('#HideAdvancedFiltersSpan').show();
        //    $('#AdvacedAuditFiltersArea').slideDown();
        //});

        //$('#HideAdvancedFiltersSpan').click(function () {
        //    $('#HideAdvancedFiltersSpan').hide();
        //    $('#ShowAdvancedFiltersSpan').show();
        //    $('#AdvacedAuditFiltersArea').slideUp();
        //});

        $('#CreateNewVersaoTissButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarVersoesTissParaExcelButton').click(function () {
            _VersoesTissService
                .listarParaExcel({
                    filtro: $('#VersoesTissTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetVersoesTissButton, #RefreshVersoesTissListButton').click(function (e) {
            e.preventDefault();
            getVersoesTiss();
        });

        abp.event.on('app.CriarOuEditarVersaoTissModalSaved', function () {
            getVersoesTiss(true);
        });

        getVersoesTiss();

        $('#VersoesTissTableFilter').focus();
    });
})();