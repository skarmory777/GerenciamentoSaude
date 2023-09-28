(function () {
    $(function () {
        var _$AvailiacaoMultidisciplinarTable = $('#AvailiacaoMultidisciplinarTable');
        var _availiacaoMultidisciplinarService = abp.services.app.availiacaoMultidisciplinar;
        var _$filterForm = $('#AvailiacaoMultidisciplinarFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Configuracoes.AvailiacaoMultidisciplinar.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Configuracoes.AvailiacaoMultidisciplinar.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Configuracoes.AvailiacaoMultidisciplinar.Delete')
        };

        _$AvailiacaoMultidisciplinarTable.jtable({

            title: app.localize('AvailiacaoMultidisciplinar'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _availiacaoMultidisciplinarService.listar
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
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="glyphicon glyphicon-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //_createOrEditModal.open({ id: data.record.id });
                                    location.href = '/Mpa/availiacaoMultidisciplinar/CriarOuEditarFormularioConfig/' + data.record.id;
                                });
                        }
                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '40%'
                },
                dataAlteracao: {
                    title: app.localize('DataAlteracao'),
                    width: '40%',
                    display: function (data) {
                        return moment(data.record.dataAlteracao).format('L LT');
                    }
                }
            }
        });

        function getAvailiacaoMultidisciplinar(reload) {
            if (reload) {
                _$AvailiacaoMultidisciplinarTable.jtable('reload');
            } else {
                _$AvailiacaoMultidisciplinarTable.jtable('load', {
                    filtro: $('#AvailiacaoMultidisciplinarTable').val()
                });
            }
        }

        function deleteAvailiacaoMultidisciplinar(availiacaoMultidisciplinar) {

            abp.message.confirm(
                app.localize('DeleteWarning', availiacaoMultidisciplinar.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _availiacaoMultidisciplinarService.excluir(availiacaoMultidisciplinar)
                            .done(function () {
                                getAvailiacaoMultidisciplinar(true);
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

        $('#CreateNewAvailiacaoMultidisciplinarButton').click(function () {
            //_createOrEditModal.open();
            location.href = '/Mpa/AvailiacaoMultidisciplinar/CriarFormulario';
        });

        $('#ExportarAvailiacaoMultidisciplinarParaExcelButton').click(function () {
            _GeradorFormulariosService
                .listarParaExcel({
                    filtro: $('#AvailiacaoMultidisciplinarTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetAvailiacaoMultidisciplinarButton, #RefreshAvailiacaoMultidisciplinarListButton').click(function (e) {
            e.preventDefault();
            getGeradorFormularios();
        });

        abp.event.on('app.CriarOuEditarGeradorFormularioModalSaved', function () {
            getAvailiacaoMultidisciplinar(true);
        });

        getAvailiacaoMultidisciplinar();

        $('#AvailiacaoMultidisciplinarTable').focus();
    });
})();