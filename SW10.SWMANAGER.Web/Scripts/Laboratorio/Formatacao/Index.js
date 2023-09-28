(function () {
    $(function () {
        var _$FormatacaoTable = $('#FormatacaoTable');
        var _FormatacaoService = abp.services.app.formatacao;
        var _$filterForm = $('#FormatacaoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Formatacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Formatacao.Edit'),
            delete: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Formatacao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Laboratorio/formatacao/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'scripts/laboratorio/formatacao/_criaroueditarmodal.js',
            modalClass: 'CriarOuEditarFormatacaoModal'
        });


        _$FormatacaoTable.jtable({

            title: app.localize('Formatacao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _FormatacaoService.paginar
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
                                    deleteFormatacao(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '15%'
                }
            }
        });

        function getFormatacao(reload) {
            if (reload) {
                _$FormatacaoTable.jtable('reload');
            } else {
                _$FormatacaoTable.jtable('load', {
                    filtro: $('#FormatacaoTableFilter').val()
                });
            }
        }

        function deleteFormatacao(formatacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', formatacao.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _FormatacaoService.excluir(formatacao)
                            .done(function () {
                                getFormatacao(true);
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

        $('#CreateNewFormatacaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarFormatacaoParaExcelButton').click(function () {
            _FormatacaoService
                .listarParaExcel({
                    filtro: $('#FormatacaoTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFormatacaoButton, #RefreshFormatacaoListButton').click(function (e) {
            e.preventDefault();
            getFormatacao();
        });

        abp.event.on('app.CriarOuEditarFormatacaoModalSaved', function () {
            getFormatacao(true);
        });

        getFormatacao();

        $('#FormatacaoTableFilter').focus();
    });
})();