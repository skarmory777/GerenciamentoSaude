(function () {
    $(function () {
       
        var _$movimentoAutomaticoTable = $('#movimentoAutomaticoTable');
        var _movimentoAutomaticoService = abp.services.app.modeloTexto;
        var _$movimentoAutomaticoFilterForm = $('#movimentoAutomaticoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.ModeloTextos.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.ModeloTextos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.ModeloTextos.Delete')
        };



        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ModeloTexto/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/ModeloTextos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoLocalChamadaModal'
        });




        _$movimentoAutomaticoTable.jtable({

            title: app.localize('ListaModeloTexto'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _movimentoAutomaticoService.listar
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
                                    debugger;
                                    //_createOrEditModal.open(data.record.id);
                                    window.localStorage.setItem('gridEmpresaGuia', JSON.stringify(data));
                                    location.href = 'ModeloTexto/CriarOuEditarModal/' + data.record.id;
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
                _$movimentoAutomaticoTable.jtable('reload');
            } else {
                _$movimentoAutomaticoTable.jtable('load', {
                    filtro: $('#tableFilter').val()
                });
            }
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _movimentoAutomaticoService.excluir(record.id)
                            .done(function () {
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
            location.href = 'ModeloTexto/CriarOuEditarModal/';
        });



        $('#buscarButton').click(function (e) {
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