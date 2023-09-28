(function () {
    $(function () {
        var _$DisparoDeMensagemTable = $('#DisparoDeMensagemTable');
        var _disparoDeMensagemService = abp.services.app.disparoDeMensagem;
        var _$filterForm = $('#DisparoDeMensagemForm');
        $('body').addClass('page-sidebar-closed');
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.DisparoDeMensagem.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.DisparoDeMensagem.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.DisparoDeMensagem.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/DisparoDeMensagem/IndexCriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Apoios/DisparoDeMensagem/indexCriarOuEditar.js',
            modalClass: 'indexCriarOuEditarModal',
            modalId: 'indexCriarOuEditarModal',
            focusFunction: (_$modal) => { setTimeout(() => { _$modal.find('#paciente-id').get(0).focus(); }, 1) }
        });

        _$DisparoDeMensagemTable.jtable({
            title: app.localize('DisparoDeMensagem'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _disparoDeMensagemService.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '6%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit && _.isEmpty(data.record.dataInicioDisparo)) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    window.location = '/Mpa/DisparoDeMensagem/IndexCriarOuEditar?id='+data.record.id;
                                });
                        }
                        if (_permissions.delete && _.isEmpty(data.record.dataInicioDisparo)) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteDisparoDeMensagem(data.record);
                                });
                        }

                        return $span;
                    }
                },
                dataProgramada: {
                    title: app.localize('Data Programada'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataProgramada) {
                            return moment(data.record.dataProgramada).format('DD/MM/YYYY HH:mm:ss');
                        }
                    }
                },
                dataInicioDisparo: {
                    title: app.localize('Data Inicio Disparo'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataInicioDisparo) {
                            return moment(data.record.dataInicioDisparo).format('DD/MM/YYYY HH:mm:ss');
                        }
                    }
                },
                dataFinalDisparo: {
                    title: app.localize('Data Final Disparo'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataFinalDisparo) {
                            return moment(data.record.dataFinalDisparo).format('DD/MM/YYYY HH:mm:ss');
                        }
                    }
                },
                total: {
                    title: app.localize('Total'),
                    width: '10%',
                    display: function (data) {
                        return data.record.total;
                    }
                },
                totalEnviado: {
                    title: app.localize('total Enviado'),
                    width: '10%',
                    display: function (data) {
                        return data.record.totalEnviado;
                    }
                },
            }
        });

        function getDisparoDeMensagem(reload) {
            if (reload) {
                _$DisparoDeMensagemTable.jtable('load');
            } else {
                _$DisparoDeMensagemTable.jtable('load', {
                    filtro: $('#DisparoDeMensagemTableFilter').val()
                });
            }
        }

        function deleteDisparoDeMensagem(disparoDeMensagem) {

            abp.message.confirm(
                app.localize('DeleteWarning'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _disparoDeMensagemService.excluir(disparoDeMensagem.id)
                            .done(function () {
                                getDisparoDeMensagem(true);
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

        $('#CreateNewDisparoDeMensagemButton').click(function () {
            window.location = '/Mpa/DisparoDeMensagem/IndexCriarOuEditar';
        });

        $('#GetDisparoDeMensagemButton, #RefreshDisparoDeMensagemListButton').click(function (e) {
            e.preventDefault();
            getDisparoDeMensagem();
        });

        abp.event.on('app.CriarOuEditarDisparoDeMensagemModalSaved', function () {
            getDisparoDeMensagem(true);
        });

        getDisparoDeMensagem();

        $('#DisparoDeMensagemTableFilter').focus();
    });
})();