(function () {
    $(function () {
        var _$ControleProducoesTable = $('#ControleProducoesTable');
        var _ControleProducoesService = abp.services.app.controleProducao;
        var _$filterForm = $('#ControleProducoesFilterForm');

        //alert(JSON.stringify(_ControleProducoesService));

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ControleProducoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Desenvolvimento/ControleProducoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarControleProducaoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Desenvolvimento/ControleProducoes/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$ControleProducoesTable.jtable({

            title: app.localize('ControleProducoes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ControleProducoesService.listarTodos
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
                                    deleteControleProducoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                }
            }
        });

        function getControleProducoes(reload) {
            if (reload) {
                _$ControleProducoesTable.jtable('reload');
            } else {
                _$ControleProducoesTable.jtable('load', {
                    filtro: $('#ControleProducoesTableFilter').val(),
                });
            }
        }

        function deleteControleProducoes(controleProducao) {
            abp.message.confirm(
                app.localize('DeleteWarning', controleProducao.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ControleProducoesService.excluir(controleProducao)
                            .done(function () {
                                getControleProducoes(true);
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

        $('#CreateNewControleProducaoButton').click(function () {
            _createOrEditModal.open();
        });

        //$('#ExportarControleProducoesParaExcelButton').click(function () {
        //    _ControleProducoesService
        //        .listarParaExcel({
        //            filtro: $('#ControleProducoesTableFilter').val(),
        //            //sorting: $(''),
        //            maxResultCount: $('span.jtable-page-size-change select').val()
        //        })
        //        .done(function (result) {
        //            app.downloadTempFile(result);
        //        });
        //});

        $('#GetControleProducoesButton, #RefreshControleProducoesListButton').click(function (e) {
            e.preventDefault();
            getControleProducoes();
        });

       
        abp.event.on('app.CriarOuEditarControleProducaoModalSaved', function () {
            getControleProducoes(true);
        });

        getControleProducoes();

        $('#ControleProducoesTableFilter').focus();


    });
})();