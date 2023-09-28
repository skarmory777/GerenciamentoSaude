(function () {
    $(function () {
        var _$MedicosTable = $('#MedicosTable');
        var _MedicosService = abp.services.app.medico;
        var _$filterForm = $('#MedicosFilterForm');
        var data = [];
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Medico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Medico.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Medico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Medicos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMedicoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$MedicosTable.jtable({

            title: app.localize('Medicos'),
            paging: true,
            sorting: true,
            multiSorting: false,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column

            actions: {
                listAction: {
                    method: _MedicosService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
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
                                    deleteMedicos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '8%'
                },
                nomeCompleto: {
                    title: app.localize('NomeCompleto'),
                    width: '25%'
                },
                rg: {
                    title: app.localize('Rg'),
                    width: '8%'
                },
                cpf: {
                    title: app.localize('Cpf'),
                    width: '8%'
                },
                nascimento: {
                    title: app.localize('Nascimento'),
                    width: '8%',
                    display: function (data) {
                        return moment(data.record.nascimento).format('L');
                    }
                },
                numeroConselho: {
                    title: app.localize('NumeroConselho'),
                    width: '8%'
                }
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = _$MedicosTable.jtable('selectedRows');
                //$('#SelectedRowList').empty();
                if ($selectedRows.length > 0) {
                    if (!$("#template").val() == '') {
                        $('#enviar').removeAttr('disabled');
                    }
                    data = [];
                    //Show selected rows
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        data.push({ destinatarioId: record.id, destinatarioTipo: "Medico", mailingTemplateId: $("#template").val() })
                    });
                }
                else {
                    data = [];
                    $('#enviar').attr('disabled', 'disabled');
                }
            },

        });

        function sendMail() {            
            $('#enviar').attr('disabled', 'disabled');
            //$.post("/mpa/enviaremail", { emails: JSON.stringify(data) }, function (response) {
            //    $('#enviar').removeAttr("disabled");
            //    abp.notify.success(app.localize('SucessoEmail'));
            //});
            $.ajax({
                data: {
                    'emais': JSON.stringify(data)
                },
                dataType: 'json',
                contentType: 'application/json',
                dataContext: 'application/json; charset=utf-8',
                url: '/mpa/EnviarEmail/Lote',
                async: true,
                type: 'GET',
                cache: false,
                success: function (data) {
                        abp.notify.success(app.localize('SucessoEmail'));
                },
                beforeSend: function () {
                    $('#enviar').buttonBusy(true)
                },
                complete: function () {
                    $('#enviar').buttonBusy(false)

                }

            });
        }

        $('#enviar').on('click', function (e) {
            e.preventDefault();
            //console.log(data);
            if (data.length > 0 && !$('#template').val() == 0) {
                sendMail();
            }
            else {
                abp.notify.error(app.localize('SelecionarMedicoTemplate'));
            }
        });

        function getMedicos(reload) {
            if (reload) {
                _$MedicosTable.jtable('reload');
            } else {
                _$MedicosTable.jtable('load', {
                    filtro: $('#MedicosTableFilter').val()
                });
            }
        }

        function deleteMedicos(Medico) {

            abp.message.confirm(
                app.localize('DeleteWarning', Medico.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MedicosService.excluir(Medico)
                            .done(function () {
                                getMedicos(true);
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

        $('#CreateNewMedicoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarMedicosParaExcelButton').click(function () {
            _MedicosService
                .listarParaExcel({
                    filtro: $('#MedicosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMedicosButton, #RefreshMedicosListButton').click(function (e) {
            e.preventDefault();
            getMedicos();
        });

        abp.event.on('app.CriarOuEditarMedicoModalSaved', function () {
            getMedicos(true);
        });

        getMedicos();

        $('#MedicosTableFilter').focus();

        //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });

    });
})();