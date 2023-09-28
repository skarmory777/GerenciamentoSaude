(function () {
    $(function () {
        var _$DivisoesTable = $('#DivisoesTable');
        var _DivisoesService = abp.services.app.divisao;
        var _$filterForm = $('#DivisoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Divisoes/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarDivisaoModal'
        });

        _$DivisoesTable.jtable({

            title: app.localize('Divisao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _DivisoesService.listar
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
                                    deleteDivisoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%'
                },
                //isMedico: {
                //    title: app.localize('IsMedico'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data.record.isMedico) {
                //            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default">' + app.localize('No') + '</span>';
                //        }
                //    }
                //},
                //isEnfermagem: {
                //    title: app.localize('IsEnfermagem'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data.record.isEnfermagem) {
                //            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default">' + app.localize('No') + '</span>';
                //        }
                //    }
                //},
                //isDivisaoPrincipal: {
                //    title: app.localize('IsDivisaoPrincipal'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data.record.isDivisaoPrincipal) {
                //            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default">' + app.localize('No') + '</span>';
                //        }
                //    }
                //}
            }
        });

        function getDivisoes() {
            //if (reload) {
            //    _$DivisoesTable.jtable('reload');
            //} else {
            _$DivisoesTable.jtable('load', {
                filtro: $('#DivisoesTableFilter').val()
            });
            //}
        }

        function deleteDivisoes(divisao) {
            abp.message.confirm(
                app.localize('DeleteWarning', divisao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _DivisoesService.excluir(divisao)
                            .done(function () {
                                getDivisoes(true);
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

        $('#CreateNewDivisaoButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarDivisoesParaExcelButton').click(function () {
            _DivisoesService
                .listarParaExcel({
                    filtro: $('#DivisoesTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetDivisoesButton, #RefreshDivisoesListButton').click(function (e) {
            e.preventDefault();
            getDivisoes();
        });

        abp.event.on('app.CriarOuEditarDivisaoModalSaved', function () {
            getDivisoes();
        });

        getDivisoes();

        $('#DivisoesTableFilter').focus();
    });
})();