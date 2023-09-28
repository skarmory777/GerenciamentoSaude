(function () {
    $(function () {
        var _$bancoAgenciasFilterFormTable = $('#bancoAgenciasTable');
        var _bancoAgenciasService = abp.services.app.bancoAgencias;
        var _$bancoAgenciasFilterFormFilterForm = $('#bancoAgenciasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.BancoAgencias.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.BancoAgencias.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.BancoAgencias.Delete')
        };



        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/BancoAgencias/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/BancoAgencias/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarBancoAgenciasModal'
        });




        _$bancoAgenciasFilterFormTable.jtable({

            title: app.localize('EditBancoAgencias'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _bancoAgenciasService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {                                    
                                    deleteRegistro(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '20%'
                },

                descricao: {
                    title: app.localize('Descricao'),
                    width: '35%'
                }
                //,

                //GrupoDRE: {
                //    title: app.localize('GrupoDRE'),
                //    width: '35%',
                //    display: function (data) {
                //        if (data.record.grupoDRE) {
                //            return data.record.grupoDRE.descricao;
                //        }
                //    }
                //},
            }
        });

        function getRegistros(reload) {
            if (reload) {
                _$bancoAgenciasFilterFormTable.jtable('reload');
            } else {
                _$bancoAgenciasFilterFormTable.jtable('load', {
                    filtro: $('#tableFilter').val()
                });
            }
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
                        _bancoAgenciasService.excluir(record)
                            .done(function () {
                                App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                                getRegistros(true);
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

        $('#CreateNewButton').click(function () {
            _createOrEditModal.open();
        });


        $('#GetFeriadosButton, #RefreshFeriadosListButton').click(function (e) {
            e.preventDefault();
            getRegistros();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getRegistros(true);
        });

        getRegistros();

        $('#tableFilter').focus();

    });
})();