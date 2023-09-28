// SMWE Savior - Objeto/Modulo para auxiliar e evitar codificacoes repetitivas

var SmweSavior = (function () {
    // PUBLIC

    // Combos
    var setCombo = function (comboSel2, id, servico) {
     //   debugger

        if (!servico) {
            console.log('Serviço não definido.');
            return;
        }
         
        servico.obter(id)
            .done(function (data) {

                if (!data)
                    return;
                
                var txt = data.nome || data.descricao || data.nomeFantasia || data.nomeCompleto;
 
                var option = new Option(txt, data.id, true, true);
                comboSel2.append(option).trigger('change');
                comboSel2.trigger({
                    type: 'select2:select',
                    params: {
                        data: data
                    }
                });
            });
    };

    var resetCombo = function (comboSel2) {
        comboSel2.empty().trigger('change');
    }

    // Formatacao (String, mascaras, dinheiro...)
    var formataDinheiro = function (n) {
        return n.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.");
    }

    // 'exibeHora': bool
    var formataData = function (data, exibeHora) {
        if (exibeHora) {
            return moment(data).format("L LT");
        } else {
            return moment(data).format("L");
        }
    }

    // Auxiliares JTable
    var jtPopular = function (jtable, filtro, reload) {
        if (reload) {
            jtable.jtable('reload');
        } else {
            jtable.jtable('load', filtro);
        }
    }

    var jtExcluirItem = function (item, jtable, filtro, servico) {
        abp.message.confirm(
            app.localize('DeleteWarning', item.codigo || item.descricao || item.nomeCompleto || item.nomeFantasia || 'Registro selecionado'),
            function (isConfirmed) {
                if (isConfirmed) {
                    abp.services.app[servico].excluir(item)
                        .done(function () {
                            jtPopular(jtable, true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    var jtExcluirBtn = function (item, jtable, filtro, servico) {
        return $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
            .click(function () {
                SmweSavior.jtExcluirItem(item, jtable, filtro, servico);
            });
    }

    // Auxiliares usabilidade
    var piqueEsconde = function (exibidor, omissor, alvo) {
        // Gera um 'toggle' para o alvo, baseado em dois itens, um para exibi-lo e outro para omiti-lo
        // Os parametros devem ser strings no format CSS Selector

        $(exibidor).click(function () {
            $(exibidor).hide();
            $(omissor).show();
            $(alvo).slideDown();
        });

        $(omissor).click(function () {
            $(omissor).hide();
            $(exibidor).show();
            $(alvo).slideUp();
        });
    }

    // Filtros
    var createRequestParams = function (form) {

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        form.find('.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        var prms = {};
        form.serializeArray().map(function (x) { prms[x.name] = x.value; });
        return $.extend(prms);
    }

    // Retornando funcoes e objetos acessiveis publicamente
    return {
        // Combos
        setCombo: setCombo,
        resetCombo: resetCombo,
        // Formatacao
        formataDinheiro: formataDinheiro,
        formataData: formataData,
        // JTable
        jtPopular: jtPopular,
        jtExcluirItem: jtExcluirItem,
        jtExcluirBtn: jtExcluirBtn,
        // Usabilidade
        piqueEsconde: piqueEsconde,
        // Filtros
        createRequestParams: createRequestParams
    };

})();

// Extensao da JTable para necessidades Smwe
(function ($) {

    if (!$ || !$.hik || !$.hik.jtable) {
        return;
    }

    // Overrides
    $.extend(true, $.hik.jtable.prototype.options, {
        //   selecao: function (event, data) { }
    });

    // Extendendo prototipo
    $.extend(true, $.hik.jtable.prototype, {

        // Auxiliar para funcao 'novosRegistros'
        novaLinha: function (record) {
            var $tr = $('<tr></tr>')
                .addClass('jtable-data-row')
                .attr('data-record-key', record.id)
                .data('record', record);
            this._addCellsToRowUsingRecord($tr);
            return $tr;
        },

        // Adiciona novos registros a jtable sem precisar ir ao back-end, apenas para visualizacao local
        novosRegistros: function (records) {
            var self = this;
            $.each(records, function (index, record) {
                self._addRow(self.novaLinha(record));
            });
            self._refreshRowStyles();
        },

        // Retorna o resgistro selecionado na jtable (nao serve para 'multiselect', pois retorna apenas um ou o ultimo entre os selecionados)
        registroSelecionado: function () {
            var record;
            var contasSelecionadas = this._getSelectedRows();
            if (contasSelecionadas.length > 0) {
                contasSelecionadas.each(function () {
                    record = $(this).data('record');
                });
            }
            return record;
        },

        // Retorna o resgistro selecionado na jtable (nao serve para 'multiselect', pois retorna apenas um ou o ultimo entre os selecionados)
        registrosSelecionados: function () {
        var record;
        var contasSelecionadas = this._getSelectedRows();
        var retorno = {};
        retorno.registros = [];
        if (contasSelecionadas.length > 0) {
            contasSelecionadas.each(function () {
                record = $(this).data('record');
                retorno.registros.push(record);
            });
        }
        return retorno;
    }

        // Exemplo de uso 'novosRegistros'
        // record.id = 1; // O registro DEVE ter um Id definido!
        // record.pacienteNome = 'Gilbert The Great';
        // var records = [];
        // records.push(record);
        // _$ContasMedicasTable.jtable('novosRegistros', records);

    });

})(jQuery);
// Fim - Extensao JTable

// JQUERY FN EXTEND
jQuery.fn.extend({
    // CRUDs sem modal
    // Botao de criar ou editar - Alternar icoes
    swBtnCrudAlternaIcone: function () {
        if (this.hasClass('fa fa-plus')) {
            this.removeClass('fa fa-plus');
            this.addClass('fa fa-check');
        } else {
            this.removeClass('fa fa-check');
            this.addClass('fa fa-plus');
        }
    },

    // Checkboxes
    // Verificar o valor atual do checkbox
    swChkValor: function () {
        return this.is(':checked');
    },

    // Setar o checkbox
    swChkSet: function (valor) {
        if (this.is(':checked') != valor) {
            this.click();
        }
    },

    // Transformar 2 checkboxs em Radio (um clica o outro desclica)
    swChecks2Radio: function (chk2Selector) {
        var chk1 = this;
        var chk2 = $(chk2Selector);

        chk1.on('click', function () {
            var v = !$(this).is(':checked');
            if (chk2.is(':checked') != v) {
                chk2.click();
            }
        });

        chk2.on('click', function () {
            var v = !$(this).is(':checked');
            if (chk1.is(':checked') != v) {
                chk1.click();
            }
        });

    },

    // Combos
    swCboReset: function () {
        this.empty().trigger('change');
    },

    // Atribui valor a qualquer input ou select2 com o valor passado. Se nao passar valor nenhum, reseta o elemento (ex:val(''))
    // Ao setar select2, necessario que o elemento(tag HTML) possua atributo 'data-servico' (indicando nome do servico que fornece o registro)
    swSetCampo: function (valor, servico) {
      //  debugger
        var nodeName = this[0].nodeName;
        switch (nodeName) {
            case 'SELECT':
                valor && SmweSavior.setCombo(this, valor, servico) || SmweSavior.resetCombo(this);
                break;
            case 'INPUT':
                var tipo = this[0].type;

                switch (tipo) {
                    case 'text':
                        valor && this.val(valor) || this.val('');
                        break;
                    case 'hidden':
                        valor && this.val(valor) || this.val('');
                        break;
                    case 'checkbox':
                        if (this.is(':checked') != valor) {
                            this.click();
                        }
                        break;
                };

                break;
            default:
                break;
        }
    },

    // Auxiliares UI
    swPiqueEsconde: function (exibidor, omissor) {
        // Gera um 'toggle' para o alvo, baseado em dois itens, um para exibi-lo e outro para omiti-lo
        // Os parametros devem ser strings no format CSS Selector
        var self = $(this);
        $(exibidor).click(function () {
            $(exibidor).hide();
            $(omissor).show();
            self.slideDown();
        });

        $(omissor).click(function () {
            $(omissor).hide();
            $(exibidor).show();
            self.slideUp();
        });
    },

    // Setar o select2 buscando do servico.obter
    //swCboSet: function (id, servico) {
    //    servico.obter(id)
    //        .done(function (data) {
    //            var option = new Option(data.descricao || data.nomeFantasia || data.nome || data.nomeCompleto, data.id, true, true);
    //            AQUI DEVERIA SER 'SELF E NAO THIS - deve ser pos isso q nao funciona
    //              $(this).append(option).trigger('change');
    //            $(this).trigger({
    //                type: 'select2:select',
    //                params: {
    //                    data: data
    //                }
    //            });
    //        });
    //}
    //,
    // Resetar o select2

});
