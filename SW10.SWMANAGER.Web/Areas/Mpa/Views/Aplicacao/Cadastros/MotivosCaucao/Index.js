(function ()
{
    $(function ()
    {
        var _$MotivosCaucaoTable = $('#MotivosCaucaoTable');
        var _MotivosCaucaoService = abp.services.app.motivoCaucao;
        var _$filterForm = $('#MotivosCaucaoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCaucao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCaucao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.MotivoCaucao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/MotivosCaucao/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/MotivosCaucao/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMotivoCaucaoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/MotivosCaucao/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$MotivosCaucaoTable.jtable({

            title: app.localize('MotivoCaucao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction:
                {
                    method: _MotivosCaucaoService.listar
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
                                    deleteMotivosCaucao(data.record);
                                });
                        }
                        return $span;
                    }
                },
                descricao: { title: app.localize('Descricao'), width: '85%' }
            }
        });

        function getMotivosCaucao(reload)
        {
            if (reload)
            {
                _$MotivosCaucaoTable.jtable('reload');
            }
            else
            {
                _$MotivosCaucaoTable.jtable('load', { filtro: $('#MotivosCaucaoTableFilter').val() });
            }
        }

        function deleteMotivosCaucao(MotivoCaucao)
        {
            abp.message.confirm(
                app.localize('DeleteWarning', MotivoCaucao.descricao),
                function (isConfirmed)
                {
                    if (isConfirmed)
                    {
                        _MotivosCaucaoService.excluir(MotivoCaucao)
                            .done(function ()
                            {
                                getMotivosCaucao(true);
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

        $('#CreateNewMotivoCaucaoButton').click(function ()
        {
            _createOrEditModal.open();
        });

        $('#ExportarMotivosCaucaoParaExcelButton').click(function ()
        {
            _MotivosCaucaoService
                .listarParaExcel
                ({
                    filtro: $('#MotivosCaucaoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result)
                {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMotivosCaucaoButton, #RefreshMotivosCaucaoListButton').click(function (e)
        {
            e.preventDefault();
            getMotivosCaucao();
        });

        abp.event.on('app.CriarOuEditarMotivoCaucaoModalSaved', function ()
        {
            getMotivosCaucao(true);
        });

        getMotivosCaucao();

        $('#MotivosCaucaoTableFilter').focus();
    });
})();