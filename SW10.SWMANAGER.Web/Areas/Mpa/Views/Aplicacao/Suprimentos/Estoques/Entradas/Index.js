(function () {

    $(function () {

        //remover isso

        var _$EntradasTable = $('#EntradasTable');
        var _EntradasService = abp.services.app.entrada;
        var _$filterForm = $('#EntradasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Entrada.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Entrada.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.Entrada.Delete')
        };

        //var _createOrEditModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/Entradas/CriarOuEditarModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Entradas/_CriarOuEditarModal.js',
        //    modalClass: 'CriarOuEditarEntradaModal'
        //});

        //var _userPermissionsModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Entradas/_PermissionsModal.js',
        //    modalClass: 'UserPermissionsModal'
        //});

        _$EntradasTable.jtable({

            title: app.localize('Entradas'),
            paging: true,
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

          
            actions: {
                listAction: {
                method: _EntradasService.listar
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
                                    //_createOrEditModal.open({ id: data.record.id });
                                    location.href = 'Entradas/CriarOuEditarModal/' + data.record.id
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-o"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteEntradas(data.record);
                                });
                        }

                        return $span;
                    }
                },
                numeroDocumento: {
                    title: app.localize('NumeroDocumento_abrev'),
                    width: '15%'
                },
                data: {
                    title: app.localize('Data'),
                    width: '15%'
                },
                acrescimoDesconto: {
                    title: app.localize('AcrescimoDesconto'),
                    width: '15%'
                },
                frete: {
                    title: app.localize('Frete'),
                    width: '15%'
                },
                valorDocumento: {
                    title: app.localize('ValorDocumento'),
                    width: '15%',
                    display: function (data) {
                        return 'R$ ' + number_format(data.record.valorDocumento, 2, ',', '.');
                    }
                },
            }
        });

        function getEntradas(reload) {
            if (reload) {
                _$EntradasTable.jtable('reload');
            } else {
                _$EntradasTable.jtable('load', {
                    filtro: $('#EntradasTableFilter').val()
                });
            }
        }

        function deleteEntradas(Entrada) {

            abp.message.confirm(
                app.localize('DeleteWarning', Entrada.numeroDocumento),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _EntradasService.excluir(Entrada)
                            .done(function () {
                                getEntradas(true);
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

        $('#CreateNewEntradaButton').click(function () {
            location.href = 'Entradas/CriarOuEditarModal/';
            //_createOrEditModal.open();
        });

        $('#ExportarEntradasParaExcelButton').click(function () {
            _EntradasService
                .listarParaExcel({
                    filtro: $('#EntradasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetEntradasButton, #RefreshEntradasListButton').click(function (e) {
            e.preventDefault();
            getEntradas();
        });

        abp.event.on('app.CriarOuEditarEntradaModalSaved', function () {
            getEntradas(true);
        });

        getEntradas();

        $('#EntradasTableFilter').focus();
    });
})();