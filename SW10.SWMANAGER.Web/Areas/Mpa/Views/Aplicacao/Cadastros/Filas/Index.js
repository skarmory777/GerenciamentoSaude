(function () {
    $(function () {
        var _$painelTableFilter = $('#painelTableFilter');
        var _filaService = abp.services.app.fila;
        var _$filaTable= $('#filaTable');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosAtendimentos.Fila.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosAtendimentos.Fila.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosAtendimentos.Fila.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Filas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Filas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFilasModal'
        });

        _$filaTable.jtable({

            title: app.localize('Filas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _filaService.listar
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
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteFila(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '30%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '30%'
                },
                
            }
        });

        function getFilas(reload) {
            if (reload) {
                _$filaTable.jtable('reload');
            } else {
                _$filaTable.jtable('load', {
                    filtro: $('#painelTableFilter').val()
                });
            }
        }

        function deleteFila(fila) {

            abp.message.confirm(
                app.localize('DeleteWarning', fila.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _filaService.excluir(fila.id)
                            .done(function () {
                                getFilas(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        //function createRequestParams() {
        //    var prms = {};
        //    _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
        //    return $.extend(prms);
        //}

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

        $('#createNewFilaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#GetPlanosButton, #RefreshPlanosListButton').click(function (e) {
            e.preventDefault();
            getFilas();
        });

        abp.event.on('app.CriarOuEditarFilaModalSaved', function () {
            getFilas(true);
        });

        getFilas();

       
    });
})();