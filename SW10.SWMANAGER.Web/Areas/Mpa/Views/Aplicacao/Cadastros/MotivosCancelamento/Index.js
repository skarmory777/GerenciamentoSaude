(function ()
{
    $(function ()
    {
        var _$MotivosCancelamentoTable = $('#MotivosCancelamentoTable');
        var _MotivosCancelamentoService = abp.services.app.motivoCancelamento;
        var _$filterForm = $('#MotivosCancelamentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCancelamento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCancelamento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCancelamento.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/MotivosCancelamento/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/MotivosCancelamento/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMotivoCancelamentoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/MotivosCancelamento/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$MotivosCancelamentoTable.jtable({

            title: app.localize('MotivoCancelamento'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction:
                {
                    method: _MotivosCancelamentoService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '15%',
                    sorting: false,
                    display: function (data)
                    {
                        var $span = $('<span></span>');
                        if (_permissions.edit)
                        {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function ()
                                {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete)
                        {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function ()
                                {
                                    deleteMotivosCancelamento(data.record);
                                });
                        }
                        return $span;
                    }
                },
                descricao: { title: app.localize('Descricao'), width: '70%' },
                isAtivo:
                {
                    title: app.localize('Ativo'),
                    width: '15%',
                    display: function (data)
                    {
                        if (data.record.isAtivo)
                        { return '<span class="label label-success">' + app.localize('Yes') + '</span>'; }
                        else
                        { return '<span class="label label-default">' + app.localize('No') + '</span>';  }
                    }
                }
            }
        });

        function getMotivosCancelamento(reload)
        {
            if (reload)
            {
                _$MotivosCancelamentoTable.jtable('reload');
            }
            else
            {
                _$MotivosCancelamentoTable.jtable('load', { filtro: $('#MotivosCancelamentoTableFilter').val() });
            }
        }

        function deleteMotivosCancelamento(MotivoCancelamento)
        {
            abp.message.confirm(
                app.localize('DeleteWarning', MotivoCancelamento.descricao),
                function (isConfirmed)
                {
                    if (isConfirmed)
                    {
                        _MotivosCancelamentoService.excluir(MotivoCancelamento)
                            .done(function ()
                            {
                                getMotivosCancelamento(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams()
        {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

        $('#ShowAdvancedFiltersSpan').click(function ()
        {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function ()
        {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewMotivoCancelamentoButton').click(function ()
        {
            _createOrEditModal.open();
        });

        $('#ExportarMotivosCancelamentoParaExcelButton').click(function ()
        {
            _MotivosCancelamentoService
                .listarParaExcel
                ({
                    filtro: $('#MotivosCancelamentoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result)
                {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMotivosCancelamentoButton, #RefreshMotivosCancelamentoListButton').click(function (e)
        {
            e.preventDefault();
            getMotivosCancelamento();
        });

        abp.event.on('app.CriarOuEditarMotivoCancelamentoModalSaved', function ()
        {
            getMotivosCancelamento(true);
        });

        getMotivosCancelamento();

        $('#MotivosCancelamentoTableFilter').focus();
    });
})();