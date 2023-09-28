(function ($) {
    app.modals.AssociarEspecialidadeModal = function () {
        var listDisp = [];
        var listDispDel = [];
        var listAssoc = [];
        var listAssocDel = [];
        var _modalManager;
        var _$EspecialidadesDisponiveisTable = $('#EspecialidadesDisponiveisTable');
        var _$EspecialidadesAssociadasTable = $('#EspecialidadesAssociadasTable');
        var _formConfigEspecialidadesService = abp.services.app.formConfigEspecialidade;
        var _especialidades = abp.services.app.especialidade;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            especialidadesIncluidas();
            especialidadesAssociadas();
            var form = {
                Id: $('#id').val(),
                FormConfigId: $('#form-id').val(),
                EspecialidadesIncluidas: $('#especialidades-incluidas').val(),
                EspecialidadesRemovidas: $('#especialidades-removidas').val(),
                CreatorUserId: abp.session.userId,
                IsDeleted: false,
                IsSistema: false
            };

            _modalManager.setBusy(true);
            _formConfigEspecialidadesService.criarOuEditar(form)
            .done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.AssociarEspecialidadeSaved');
            })
            .always(function () {
                _modalManager.setBusy(false);
            });

        }
        //Especialidades disponíveis

        _$EspecialidadesDisponiveisTable.jtable({
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _formConfigEspecialidadesService.listarEspecialidadeSemForm //retornarListaEspecialidadesDisponiveis
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '20%',
                    display: function (data) {
                        return data.record.codigo;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '60%',
                    display: function (data) {
                        return data.record.descricao;
                    }
                }
            },
            selectionChanged: function () {
                especialidadesIncluidas();
                //Get all selected rows
                //var $selectedRows = _$EspecialidadesDisponiveisTable.jtable('selectedRows');
                //if ($selectedRows.length > 0) {
                //    var listaTemp = [];
                //    $selectedRows.each(function () {
                //        var record = $(this).data('record');
                //        listaTemp.push(record.id);
                //    });
                //    listDisp = listaTemp;
                //}
                //else {
                //    listDisp = [];
                //}
                //$('#especialidades-incluidas').val(listDisp);
            }
        });


        function especialidadesIncluidas() {
            var $selectedRows = _$EspecialidadesDisponiveisTable.jtable('selectedRows');
            if ($selectedRows.length > 0) {
                var listaTemp = [];
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    listaTemp.push(record.id);
                });
                listDisp = listaTemp;
            }
            else {
                listDisp = [];
            }
            $('#especialidades-incluidas').val(listDisp);
        }

        function getEspecialidadesDisponiveis(reload) {
            if (reload) {
                _$EspecialidadesDisponiveisTable.jtable('reload');
            } else {
                //Alerta de gambiarra. utilizei o campo empresaId para não precisar criar um novo "ListarInput.cs" com o campo moduloId
                _$EspecialidadesDisponiveisTable.jtable('load', {
                    filtro: $('#EspecialidadesDisponiveisTableFilter').val(),
                    empresaId: $('#modulo-id').val(),
                    principalId: $('#form-id').val()
                });
            }
        }

        $('#GetEspecialidadesDisponiveisButton, #RefreshEspecialidadesDisponiveisListButton').click(function (e) {
            e.preventDefault();
            getEspecialidadesDisponiveis();
        });

        abp.event.on('app.CriarOuEditarEspecialidadesDisponiveisModalSaved', function () {
            getEspecialidadesDisponiveis(true);
        });

        _$EspecialidadesAssociadasTable.jtable({
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _formConfigEspecialidadesService.listarEspecialidadePorForm //retornarListaEspecialidadesAssociadas
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '20%',
                    display: function (data) {
                        return data.record.codigo;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '60%',
                    display: function (data) {
                        return data.record.descricao;
                    }
                }
            },
            selectionChanged: function () {
                especialidadesAssociadas();
                //Get all rows
                //var allRows = _$EspecialidadesAssociadasTable.find('.jtable-data-row')
                //var allIds = [];
                //allRows.each(function () {
                //    allIds.push($(this).data('record-key'));
                //})
                ////Get all selected rows
                //var $selectedRows = _$EspecialidadesAssociadasTable.jtable('selectedRows');
                //if ($selectedRows.length > 0) {
                //    var listaTemp = [];
                //    $selectedRows.each(function () {
                //        var record = $(this).data('record');
                //        listaTemp.push(record.id);
                //        var id = record.id.toString();
                //        var index = allIds.indexOf(id);
                //        if (index > -1) {
                //            allIds.splice(index, 1);
                //        }
                //    });
                //    listAssoc = listaTemp;
                //}
                //else {
                //    listAssoc = [];
                //}
                //$('#especialidades-removidas').val(allIds);
            },
            recordsLoaded: function (event, data) {
                $('#EspecialidadesAssociadasTable tr.jtable-data-row:not(.jtable-row-selected)').click();
            }
        });

        function especialidadesAssociadas() {
            var allRows = _$EspecialidadesAssociadasTable.find('.jtable-data-row')
            var allIds = [];
            allRows.each(function () {
                allIds.push($(this).data('record-key'));
            })
            //Get all selected rows
            var $selectedRows = _$EspecialidadesAssociadasTable.jtable('selectedRows');
            if ($selectedRows.length > 0) {
                var listaTemp = [];
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    listaTemp.push(record.id);
                    //var id = record.id.toString();
                    var id = record.id;//.toString();
                    var index = allIds.indexOf(id);
                    if (index > -1) {
                        allIds.splice(index, 1);
                    }
                });
                listAssoc = listaTemp;
            }
            else {
                listAssoc = [];
            }
            $('#especialidades-removidas').val(allIds);
        }
        function getEspecialidadesAssociadas(reload) {
            if (reload) {
                _$EspecialidadesAssociadasTable.jtable('reload');
            } else {
                _$EspecialidadesAssociadasTable.jtable('load', {
                    filtro: $('#EspecialidadesAssociadasTableFilter').val(),
                    principalId: $('#form-id').val()
                });
            }
        }

        $('#GetEspecialidadesAssociadasButton, #RefreshEspecialidadesAssociadasListButton').click(function (e) {
            e.preventDefault();
            getEspecialidadesAssociadas();
        });

        abp.event.on('app.CriarOuEditarEspecialidadesAssociadasModalSaved', function () {
            getEspecialidadesAssociadas(true);
        });

        //function retornarListaEspecialidadesAssociadas() {
        //    return _formConfigEspecialidadesService.ListarEspecialidadeComForm($('#form-id').val());
        //}

        getEspecialidadesDisponiveis();

        getEspecialidadesAssociadas();

        $('#EspecialidadesDisponiveisTableFilter').focus();

        aplicarSelect2Padrao();

        $('.select2').css('width', '100%');
    };
})(jQuery);