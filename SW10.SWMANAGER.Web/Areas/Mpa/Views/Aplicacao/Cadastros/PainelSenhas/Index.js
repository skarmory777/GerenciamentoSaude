(function () {
    $(function () {
        var _$painelTableFilter = $('#painelTableFilter');
        var _painelService = abp.services.app.painel;
        var _$planosTable = $('#paineisTable');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosAtendimentos.PainelSenha.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosAtendimentos.PainelSenha.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosAtendimentos.PainelSenha.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PainelSenhas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/PainelSenhas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPainelSenhaModal'
        });

        _$planosTable.jtable({

            title: app.localize('PaineisSenhas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _painelService.listar
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
                                    deletePainel(data.record);
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

        function getPaineis(reload) {
            if (reload) {
                _$planosTable.jtable('reload');
            } else {
                _$planosTable.jtable('load', {
                    filtro: $('#painelTableFilter').val()
                });
            }
        }

        function deletePainel(painel) {

            abp.message.confirm(
                app.localize('DeleteWarning', painel.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _painelService.excluir(painel.id)
                            .done(function () {
                                getPaineis(true);
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

        $('#createNewPainelButton').click(function () {
            _createOrEditModal.open();
        });

        $('#GetPlanosButton, #RefreshPlanosListButton').click(function (e) {
            e.preventDefault();
            getPlanos();
        });

        abp.event.on('app.CriarOuEditarPainelModalSaved', function () {
            getPaineis(true);
        });

        getPaineis();

       
    });
})();