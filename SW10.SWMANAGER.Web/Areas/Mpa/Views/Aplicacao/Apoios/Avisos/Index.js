(function () {
    $(function () {
        const avisoTable = $('#AvisoTable');
        const avisoService = abp.services.app.avisos;
        const filterForm = $('#AvisoForm');
        $('body').addClass('page-sidebar-closed');
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        const permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Aviso.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Aviso.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Aviso.Delete')
        };

        const createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Aviso/IndexCriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Apoios/Aviso/indexCriarOuEditar.js',
            modalClass: 'indexCriarOuEditarModal',
            modalId: 'indexCriarOuEditarModal',
        });

        avisoTable.jtable({
            title: app.localize('Aviso'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: avisoService.listar
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
                        let $span = $('<span></span>');
                        if (permissions.edit && _.isEmpty(data.record.dataInicioDisparo)) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    window.location = '/Mpa/Avisos/IndexCriarOuEditar?id='+data.record.id;
                                });
                        }
                        if (permissions.delete && _.isEmpty(data.record.dataInicioDisparo)) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteAviso(data.record);
                                });
                        }

                        return $span;
                    }
                },
                titulo: {
                    title: app.localize('Titulo Aviso'),
                    width: '10%',
                    display: function (data) {
                        return data.record.titulo;
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
                totalEnviado: {
                    title: app.localize('total Enviado'),
                    width: '10%',
                    display: function (data) {
                        return data.record.totalEnviado;
                    }
                },
            }
        });

        function getAviso(reload) {
            if (reload) {
                avisoTable.jtable('load');
            } else {
                avisoTable.jtable('load', {
                    filtro: $('#AvisoTableFilter').val()
                });
            }
        }

        function deleteAviso(aviso) {
            abp.message.confirm(
                app.localize('DeleteWarning'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        avisoService.excluir(aviso.id)
                            .done(function () {
                                getAviso(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
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

        $('#CreateNewAvisoButton').click(function () {
            window.location = '/Mpa/Avisos/IndexCriarOuEditar';
        });

        $('#GetAvisoButton, #RefreshAvisoListButton').click(function (e) {
            e.preventDefault();
            getAviso();
        });

        abp.event.on('app.CriarOuEditarAvisoModalSaved', function () {
            getAviso(true);
        });

        getAviso();

        $('#AvisoTableFilter').focus();
    });
})();