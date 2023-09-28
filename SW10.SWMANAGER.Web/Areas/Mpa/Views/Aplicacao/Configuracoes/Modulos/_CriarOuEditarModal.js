(function ($) {
    app.modals.CriarOuEditarModuloModal = function () {

        var _modulosService = abp.services.app.modulo;

        var _modalManager;
        var _$moduloInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$moduloInformationForm = _modalManager.getModal().find('form[name=ModuloInformationsForm]');
            _$moduloInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('div.form-group select').addClass('form-control selectpicker');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            if (!_$moduloInformationForm.valid()) {
                return;
            }

            var modulo = _$moduloInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _modulosService.criarOuEditar(modulo)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarModuloModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        var _$OperacoesTable = $('#OperacoesTable');
        var _OperacoesService = abp.services.app.operacao;
        var _$filterForm = $('#OperacoesFilterForm');

        var _permissionsOperacoes = {
            create: abp.auth.hasPermission('Pages.Tenant.Configuracoes.Operacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Configuracoes.Operacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Configuracoes.Operacao.Delete')
        };

        var _createOrEditOperacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Operacoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Configuracoes/Operacoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarOperacaoModal'
        });

        _$OperacoesTable.jtable({

            title: app.localize('Operacoes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _OperacoesService.listarPorModulo
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
                        if (_permissionsOperacoes.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditOperacaoModal.open({ moduloId: $('#modulo-id').val(), id: data.record.id });
                                });
                        }

                        if (_permissionsOperacoes.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteOperacoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                },

            }
        });

        function getOperacoes(reload) {
            if (reload) {
                _$OperacoesTable.jtable('reload');
            } else {
                _$OperacoesTable.jtable('load', {
                    filtro: $('#OperacoesTableFilter').val(),
                    moduloId: $('#modulo-id').val()
                });
            }
        }

        function deleteOperacoes(Operacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', Operacao.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _OperacoesService.excluir(Operacao)
                            .done(function () {
                                getOperacoes(true);
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

        $('#CreateNewOperacaoButton').click(function (e) {
            e.preventDefault();
            _createOrEditOperacaoModal.open({ moduloId: $('#modulo-id').val() });
        });

        $('#ExportarOperacoesParaExcelButton').click(function () {
            _OperacoesService
                .listarParaExcel({
                    filtro: $('#OperacoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetOperacoesButton, #RefreshOperacoesListButton').click(function (e) {
            e.preventDefault();
            getOperacoes();
        });

        abp.event.on('app.CriarOuEditarOperacaoModalSaved', function () {
            getOperacoes();
        });

        getOperacoes();

        $('#OperacoesTableFilter').focus();

        aplicarSelect2Padrao();
        CamposRequeridos();
        aplicarDateSingle();
        aplicarDateRange();

    };
})(jQuery);