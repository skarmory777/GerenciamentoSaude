(function ($) {
    app.modals.AssociarUnidadeOrganizacionalModal = function () {
        var listDisp = [];
        var listDispDel = [];
        var listAssoc = [];
        var listAssocDel = [];
        var _modalManager;
        var _$UnidadesOrganizacionaisDisponiveisTable = $('#UnidadesOrganizacionaisDisponiveisTable');
        var _$UnidadesOrganizacionaisAssociadasTable = $('#UnidadesOrganizacionaisAssociadasTable');
        var _formConfigUnidadesOrganizacionaisService = abp.services.app.formConfigUnidadeOrganizacional;
        var _unidadesOrganizacionais = abp.services.app.unidadeOrganizacional;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            var form = {
                Id: $('#id').val(),
                FormConfigId: $('#form-id').val(),
                UnidadesIncluidas: $('#unidades-incluidas').val(),
                UnidadesRemovidas: $('#unidades-removidas').val(),
                CreatorUserId: abp.session.userId,
                IsDeleted: false,
                IsSistema: false
            };

            _modalManager.setBusy(true);
            _formConfigUnidadesOrganizacionaisService.criarOuEditar(form)
            .done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.AssociarUnidadeOrganizacionalSaved');
            })
            .always(function () {
                _modalManager.setBusy(false);
            });

        }
        //Unidades disponíveis

        _$UnidadesOrganizacionaisDisponiveisTable.jtable({
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _formConfigUnidadesOrganizacionaisService.listarUnidadeOrganizacionalSemForm //retornarListaUnidadesOrganizacionaisDisponiveis
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
                //Get all selected rows
                var $selectedRows = _$UnidadesOrganizacionaisDisponiveisTable.jtable('selectedRows');
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
                $('#unidades-incluidas').val(listDisp);
            }
        });

        function getUnidadesOrganizacionaisDisponiveis(reload) {
            if (reload) {
                _$UnidadesOrganizacionaisDisponiveisTable.jtable('reload');
            } else {
                _$UnidadesOrganizacionaisDisponiveisTable.jtable('load', {
                    filtro: $('#UnidadesOrganizacionaisDisponiveisTableFilter').val(),
                    principalId: $('#form-id').val()
                });
            }
        }

        $('#GetUnidadesOrganizacionaisDisponiveisButton, #RefreshUnidadesOrganizacionaisDisponiveisListButton').click(function (e) {
            e.preventDefault();
            getUnidadesOrganizacionaisDisponiveis();
        });

        abp.event.on('app.CriarOuEditarUnidadesOrganizacionaisDisponiveisModalSaved', function () {
            getUnidadesOrganizacionaisDisponiveis(true);
        });

        _$UnidadesOrganizacionaisAssociadasTable.jtable({
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _formConfigUnidadesOrganizacionaisService.listarUnidadeOrganizacionalPorForm //retornarListaUnidadesOrganizacionaisAssociadas
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
                //Get all rows
                var allRows = _$UnidadesOrganizacionaisAssociadasTable.find('.jtable-data-row')
                var allIds = [];
                allRows.each(function () {
                    allIds.push($(this).attr('data-record-key'))
                })
                //Get all selected rows
                var $selectedRows = _$UnidadesOrganizacionaisAssociadasTable.jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    var listaTemp = [];
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        listaTemp.push(record.id);
                        var id = record.id.toString();
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
                $('#unidades-removidas').val(allIds);
            },
            recordsLoaded: function (event, data) {
                $('#UnidadesOrganizacionaisAssociadasTable tr.jtable-data-row:not(.jtable-row-selected)').click();
            }
        });

        function getUnidadesOrganizacionaisAssociadas(reload) {
            if (reload) {
                _$UnidadesOrganizacionaisAssociadasTable.jtable('reload');
            } else {
                _$UnidadesOrganizacionaisAssociadasTable.jtable('load', {
                    filtro: $('#UnidadesOrganizacionaisAssociadasTableFilter').val(),
                    principalId: $('#form-id').val()
                });
            }
        }

        $('#GetUnidadesOrganizacionaisAssociadasButton, #RefreshUnidadesOrganizacionaisAssociadasListButton').click(function (e) {
            e.preventDefault();
            getUnidadesOrganizacionaisAssociadas();
        });

        abp.event.on('app.CriarOuEditarUnidadesOrganizacionaisAssociadasModalSaved', function () {
            getUnidadesOrganizacionaisAssociadas(true);
        });

        //function retornarListaUnidadesOrganizacionaisAssociadas() {
        //    return _formConfigUnidadesOrganizacionaisService.ListarUnidadeOrganizacionalComForm($('#form-id').val());
        //}

        getUnidadesOrganizacionaisDisponiveis();

        getUnidadesOrganizacionaisAssociadas();

        $('#UnidadesOrganizacionaisDisponiveisTableFilter').focus();

    };
})(jQuery);