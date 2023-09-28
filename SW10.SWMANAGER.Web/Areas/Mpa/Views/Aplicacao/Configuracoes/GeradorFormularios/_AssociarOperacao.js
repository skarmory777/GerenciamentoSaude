(function ($) {
    app.modals.AssociarOperacaoModal = function () {
        var listDisp = [];
        var listDispDel = [];
        var listAssoc = [];
        var listAssocDel = [];
        var _modalManager;
        var _$OperacoesDisponiveisTable = $('#OperacoesDisponiveisTable');
        var _$OperacoesAssociadasTable = $('#OperacoesAssociadasTable');
        var _formConfigOperacoesService = abp.services.app.formConfigOperacao;
        var _operacoes = abp.services.app.operacao;

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
                OperacoesIncluidas: $('#operacoes-incluidas').val(),
                OperacoesRemovidas: $('#operacoes-removidas').val(),
                CreatorUserId: abp.session.userId,
                IsDeleted: false,
                IsSistema: false
            };

            _modalManager.setBusy(true);
            _formConfigOperacoesService.criarOuEditar(form)
            .done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.AssociarOperacaoSaved');
            })
            .always(function () {
                _modalManager.setBusy(false);
            });

        }
        //Operacoes disponíveis

        _$OperacoesDisponiveisTable.jtable({
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _formConfigOperacoesService.listarOperacaoSemForm //retornarListaOperacoesDisponiveis
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
                var $selectedRows = _$OperacoesDisponiveisTable.jtable('selectedRows');
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
                $('#operacoes-incluidas').val(listDisp);
            }
        });

        function getOperacoesDisponiveis(reload) {
            if (reload) {
                _$OperacoesDisponiveisTable.jtable('reload');
            } else {
                //Alerta de gambiarra. utilizei o campo empresaId para não precisar criar um novo "ListarInput.cs" com o campo moduloId
                _$OperacoesDisponiveisTable.jtable('load', {
                    filtro: $('#OperacoesDisponiveisTableFilter').val(),
                    empresaId: $('#modulo-id').val(),
                    principalId: $('#form-id').val()
                });
            }
        }

        $('#GetOperacoesDisponiveisButton, #RefreshOperacoesDisponiveisListButton').click(function (e) {
            e.preventDefault();
            getOperacoesDisponiveis();
        });

        abp.event.on('app.CriarOuEditarOperacoesDisponiveisModalSaved', function () {
            getOperacoesDisponiveis(true);
        });

        _$OperacoesAssociadasTable.jtable({
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _formConfigOperacoesService.listarOperacaoPorForm //retornarListaOperacoesAssociadas
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
                var allRows = _$OperacoesAssociadasTable.find('.jtable-data-row')
                var allIds = [];
                allRows.each(function () {
                    allIds.push($(this).attr('data-record-key'))
                })
                //Get all selected rows
                var $selectedRows = _$OperacoesAssociadasTable.jtable('selectedRows');
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
                $('#operacoes-removidas').val(allIds);
            },
            recordsLoaded: function (event, data) {
                $('#OperacoesAssociadasTable tr.jtable-data-row:not(.jtable-row-selected)').click();
            }
        });

        function getOperacoesAssociadas(reload) {
            if (reload) {
                _$OperacoesAssociadasTable.jtable('reload');
            } else {
                _$OperacoesAssociadasTable.jtable('load', {
                    filtro: $('#OperacoesAssociadasTableFilter').val(),
                    principalId: $('#form-id').val()
                });
            }
        }

        $('#GetOperacoesAssociadasButton, #RefreshOperacoesAssociadasListButton').click(function (e) {
            e.preventDefault();
            getOperacoesAssociadas();
        });

        abp.event.on('app.CriarOuEditarOperacoesAssociadasModalSaved', function () {
            getOperacoesAssociadas(true);
        });

        //function retornarListaOperacoesAssociadas() {
        //    return _formConfigOperacoesService.ListarOperacaoComForm($('#form-id').val());
        //}

        getOperacoesDisponiveis();

        getOperacoesAssociadas();

        $('#OperacoesDisponiveisTableFilter').focus();

        aplicarSelect2Padrao();

        $('.select2').css('width', '100%');
    };
})(jQuery);