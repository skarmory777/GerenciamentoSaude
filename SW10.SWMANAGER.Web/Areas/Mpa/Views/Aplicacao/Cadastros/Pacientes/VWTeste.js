(function () {
    $(function () {
        var _$vwTestesTable = $('#VWTestesTable');
        var _vwTestesService = abp.services.app.paciente;
        var _$filterForm = $('#VWTestesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.VWTeste.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.VWTeste.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.VWTeste.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Pacientes/VWTesteDetalhes.cshtml',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/VWTestes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarVWTesteModal'
        });

        _$vwTestesTable.jtable({

            title: app.localize('VWTestes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _vwTestesService.listarResumo
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                //actions: {
                //    title: app.localize('Actions'),
                //    width: '12%',
                //    sorting: false,
                //    display: function (data) {
                //        var $span = $('<span></span>');
                //        if (_permissions.edit) {
                //            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                //                .appendTo($span)
                //                .click(function () {
                //                    _createOrEditModal.open({ id: data.record.id });
                //                });
                //        }

                //        if (_permissions.delete) {
                //            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                //                .appendTo($span)
                //                .click(function () {
                //                    deleteVWTestes(data.record);
                //                });
                //        }

                //        return $span;
                //    }
                //},
                pacienteId: {
                    title: app.localize('PacienteId'),
                    width: '15%'
                },
                nomePaciente: {
                    title: app.localize('NomePaciente'),
                    width: '8%'
                },
                cidadeId: {
                    title: app.localize('CidadeId'),
                    width: '8%'
                },
                nomeCidade: {
                    title: app.localize('NomeCidade'),
                    width: '8%'
                    //display: function (data) {
                    //    return moment(data.record.nascimento).format('L');
                    //}
                },
                estadoId: {
                    title: app.localize('EstadoId'),
                    sorting: false,
                    width: '15%'
                    //display: function (data) {
                    //    return data.record.telefone1;
                    //}
                },
                nomeEstado: {
                    title: app.localize('NomeEstado'),
                    width: '15%'
                }
            }
        });

        function getVWTestes(reload) {
            if (reload) {
                _$vwTestesTable.jtable('reload');
            } else {
                _$vwTestesTable.jtable('load', {
                    filtro: $('#VWTestesTableFilter').val()
                });
            }
        }

        //function deleteVWTestes(VWTeste) {

        //    abp.message.confirm(
        //        app.localize('DeleteWarning', VWTeste.primeiroNome),
        //        function (isConfirmed) {
        //            if (isConfirmed) {
        //                _vwTestesService.excluir(VWTeste)
        //                    .done(function () {
        //                        getVWTestes(true);
        //                        abp.notify.success(app.localize('SuccessfullyDeleted'));
        //                    });
        //            }
        //        }
        //    );
        //}

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

        //$('#CreateNewVWTesteButton').click(function () {
        //    _createOrEditModal.open();
        //});

        //$('#ExportarVWTestesParaExcelButton').click(function () {
        //    _vwTestesService
        //        .listarParaExcel({
        //            filtro: $('#VWTestesTableFilter').val(),
        //            //sorting: $(''),
        //            maxResultCount: $('span.jtable-page-size-change select').val()
        //        })
        //        .done(function (result) {
        //            app.downloadTempFile(result);
        //        });
        //});

        $('#GetVWTestesButton, #RefreshVWTestesListButton').click(function (e) {
            e.preventDefault();
            getVWTestes();
        });

        abp.event.on('app.CriarOuEditarVWTesteModalSaved', function () {
            getVWTestes(true);
        });

        getVWTestes();

        $('#VWTestesTableFilter').focus();
    });
})();