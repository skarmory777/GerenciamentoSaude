(function () {
    $(function () {
        var _$MailingTemplatesTable = $('#MailingTemplatesTable');
        var _MailingTemplatesService = abp.services.app.mailingTemplate;
        var _$filterForm = $('#MailingTemplatesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Manutencao.MailingTemplates.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Manutencao.MailingTemplates.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Manutencao.MailingTemplates.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/MailingTemplates/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Manutencoes/MailingTemplates/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMailingTemplateModal'
        });

        var data = [];

        _$MailingTemplatesTable.jtable({

            title: app.localize('MailingTemplates'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _MailingTemplatesService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '2%',
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
                                    deleteMailingTemplates(data.record);
                                });
                        }

                        return $span;
                    }
                },
                name: {
                    title: app.localize('Nome'),
                    width: '20%'
                },
                titulo: {
                    title: app.localize('Titulo'),
                    width: '20%'
                },
                emailSaida: {
                    title: app.localize('EmailSaida'),
                    width: '20%'
                },
                nomeSaida: {
                    title: app.localize('NomeSaida'),
                    width: '20%'
                }
            },

        });


        function getMailingTemplates(reload) {
            if (reload) {
                _$MailingTemplatesTable.jtable('reload');
            } else {
                _$MailingTemplatesTable.jtable('load', {
                    filtro: $('#MailingTemplatesTableFilter').val()
                });
            }
        }

        function deleteMailingTemplates(mailingTemplate) {

            abp.message.confirm(
                app.localize('DeleteWarning', mailingTemplate.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MailingTemplatesService.excluir(mailingTemplate)
                            .done(function () {
                                getMailingTemplates(true);
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

        $('#CreateNewMailingTemplateButton').click(function () {
            _createOrEditModal.open();
        });

        //$('#ExportarMailingTemplatesParaExcelButton').click(function () {
        //    _MailingTemplatesService
        //        .listarParaExcel({
        //            filtro: $('#MailingTemplatesTableFilter').val(),
        //            //sorting: $(''),
        //            maxResultCount:$('span.jtable-page-size-change select').val()
        //        })
        //        .done(function (result) {
        //            app.downloadTempFile(result);
        //        });
        //});

        $('#GetMailingTemplatesButton, #RefreshMailingTemplatesListButton').click(function (e) {
            e.preventDefault();
            getMailingTemplates();
        });

        abp.event.on('app.CriarOuEditarMailingTemplateModalSaved', function () {
            getMailingTemplates(true);
        });

        getMailingTemplates();

        $('#MailingTemplatesTableFilter').focus();
    });
})();