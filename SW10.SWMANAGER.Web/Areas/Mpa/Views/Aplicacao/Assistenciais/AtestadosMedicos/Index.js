(function () {
    $(function () {
        var _$AtestadosMedicosTable = $('#AtestadosMedicosTable');
        var _mailingTemplateService = abp.services.app.mailingTemplate;
        var _$filterForm = $('#AtestadosMedicosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistenciais.AtestadoMedico.AtestadoMedico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistenciais.AtestadoMedico.AtestadoMedico.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistenciais.AtestadoMedico.AtestadoMedico.Delete')
        };

        $('input[type=checkbox]').on('click', function (e) {
            //e.preventDefault();
            //e.stopPropagation();
            var obj = $(this).attr('id');
            $('input[type=checkbox]').each(function () {
                $(this).removeAttr('checked');
            });
            $('#' + obj).attr('checked', 'checked');
            _mailingTemplateService.obter(obj)
            .done(function (data) {
                $('#atestado-container').Editor('setText', data.contentTemplate);
            });
        });

        $('#atestado-container').Editor();

        $(".accordion").accordion({
            active: false,
            collapsible: true,
            heightStyle: "content",
            //event: false
        });

        //var _createOrEditModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/AtestadosMedicos/CriarOuEditarModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/AtestadosMedicos/_CriarOuEditarModal.js',
        //    modalClass: 'CriarOuEditarAtestadoMedicoModal'
        //});

        //_$AtestadosMedicosTable.jtable({

        //    title: app.localize('AtestadoMedico'),
        //    paging: true,
        //    sorting: true,
        //    multiSorting: true,

        //    actions: {
        //        listAction: {
        //            method: _AtestadosMedicosService.listar
        //        }
        //    },

        //    fields: {
        //        id: {
        //            key: true,
        //            list: false
        //        },
        //        actions: {
        //            title: app.localize('Actions'),
        //            width: '2%',
        //            sorting: false,
        //            display: function (data) {
        //                var $span = $('<span></span>');
        //                if (_permissions.edit) {
        //                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
        //                        .appendTo($span)
        //                        .click(function () {
        //                            _createOrEditModal.open({ id: data.record.id });
        //                        });
        //                }

        //                if (_permissions.delete) {
        //                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
        //                        .appendTo($span)
        //                        .click(function () {
        //                            deleteAtestadosMedicos(data.record);
        //                        });
        //                }

        //                return $span;
        //            }
        //        },
        //        codAtestadoMedico: {
        //            title: app.localize('CodAtestadoMedico'),
        //            width: '4%'
        //        },
        //        descricao: {
        //            title: app.localize('Descricao'),
        //            width: '8%'
        //        },
        //        creationTime: {
        //            title: app.localize('CreationTime'),
        //            width: '2%',
        //            display: function (data) {
        //                return moment(data.record.creationTime).format('L');
        //            }
        //        }
        //    }
        //});

        //function getAtestadosMedicos(reload) {
        //    if (reload) {
        //        _$AtestadosMedicosTable.jtable('reload');
        //    } else {
        //        _$AtestadosMedicosTable.jtable('load', {
        //            filtro: $('#AtestadosMedicosTableFilter').val()
        //        });
        //    }
        //}

        //function deleteAtestadosMedicos(atestadoMedico) {

        //    abp.message.confirm(
        //        app.localize('DeleteWarning', atestadoMedico.descricao),
        //        function (isConfirmed) {
        //            if (isConfirmed) {
        //                _AtestadosMedicosService.excluir(atestadoMedico)
        //                    .done(function () {
        //                        getAtestadosMedicos(true);
        //                        abp.notify.success(app.localize('SuccessfullyDeleted'));
        //                    });
        //            }
        //        }
        //    );
        //}

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

        //$('#CreateNewAtestadoMedicoButton').click(function () {
        //    _createOrEditModal.open();
        //});

        //$('#ExportarAtestadosMedicosParaExcelButton').click(function () {
        //    _AtestadosMedicosService
        //        .listarParaExcel({
        //            filtro: $('#AtestadosMedicosTableFilter').val(),
        //            //sorting: $(''),
        //            maxResultCount:$('span.jtable-page-size-change select').val()
        //        })
        //        .done(function (result) {
        //            app.downloadTempFile(result);
        //        });
        //});

        //$('#GetAtestadosMedicosButton, #RefreshAtestadosMedicosListButton').click(function (e) {
        //    e.preventDefault();
        //    getAtestadosMedicos();
        //});

        //abp.event.on('app.CriarOuEditarAtestadoMedicoModalSaved', function () {
        //    getAtestadosMedicos(true);
        //});

        //getAtestadosMedicos();

        //$('#AtestadosMedicosTableFilter').focus();
    });
})();