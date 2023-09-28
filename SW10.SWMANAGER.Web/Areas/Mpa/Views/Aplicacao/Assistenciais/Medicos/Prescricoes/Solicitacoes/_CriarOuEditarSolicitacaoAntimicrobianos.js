(function ($) {
    const solicitacaoAntimicrobianoAppService = abp.services.app.solicitacaoAntimicrobiano
    $(".add-cultura").click(addCultura)
    $('.checkbox-infeccao').on('change', function () {
        $(this).parents(".grupo-checkbox-infeccao").find(".checkbox-infeccao").not(this).prop('checked', false)
    })

    $('.add-cultura-date').each(function () {
        const el = $(this)
        el.daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: true,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        }, (selDate) => el.val(selDate.format('L')).addClass('form-control edited'))
    })


    $(".add-cultura-tipo.select2").each(function () {
        const el = $(this)
        el.select2({width: '300',
            placeholder: "Selecione uma opção",
            allowClear: true})
    });

    $(".add-status_resultado.select2").each(function () {
        const el = $(this)
        el.select2({width: '200',
            placeholder: "Selecione uma opção",
            allowClear: true})
    });
    
    $(".cbk-tipo-cultura").change(changeCbkTipoCultura);

    $("select").on("select2-close", function (e) { $(this).valid(); });
    
    function changeCbkTipoCultura(e) {
        const el = $(e.currentTarget);
        const resultPanel = el.parents(".tab-solicitacao")
        const addCulturaContent = resultPanel.find(".add-cultura-content")
        const culturaContent = resultPanel.find(".culturas-content")
        const navCulturas = resultPanel.find(".navCulturas")
        const tabsCulturas = resultPanel.find(".tabsCulturas")
        
        if(el.val() === "2") {
            addCulturaContent.hide();
            culturaContent.hide();
            navCulturas.empty();
            tabsCulturas.empty();
        } else if (el.val() === "1") {
            addCulturaContent.show();
            culturaContent.show();
        }
    }

    function randomizeInteger(min, max) {
        if (max == null) {
            max = (min == null ? Number.MAX_SAFE_INTEGER : min)
            min = 0
        }

        min = Math.ceil(min) // inclusive min
        max = Math.floor(max) // exclusive max

        if (min > max - 1) {
            throw new Error("Incorrect arguments.")
        }

        return min + Math.floor((max - min) * Math.random())
    }

    function addCultura(event) {
        const el = $(event.currentTarget)
        const form = el.parents("form");
        const resultPanel = el.parents(".tab-solicitacao")
        const addCulturaContent = el.parents(".add-cultura-content")
        
        const navCulturas = resultPanel.find(".navCulturas")
        const tabsCulturas = resultPanel.find(".tabsCulturas")
        const tiposResultados = JSON.parse($("[name=TipoResultados]").val())
        const tiposCulturas = JSON.parse($("[name=TipoCulturas]").val())

        if(!form.data("validator")) {
            form.validate({ignore:""});
        }
        
        let formValidator = form.validate();
        
        const fields = addCulturaContent.find("input,select");
        
        let isValid = true;
        fields.each(function() {
            const elEach = $(this);
            elEach.rules("add", { required: true});
            let eachValid = elEach.valid();
            
            if(!eachValid) {
                isValid = eachValid; 
            }
        })
        if(!isValid) {
            fields.each(function() {
                const elEach = $(this);
                elEach.rules("remove");
                delete formValidator.settings.rules[elEach.attr("name")];
            })
            return;
        }
        
        el.buttonBusy(true)
        const culturaId = el.data('id') + '_' + randomizeInteger() + '_nova_cultura'
        const date = resultPanel.find('.add-cultura-date').val() || moment().format("DD/MM/YYYY")
        const tipoId = resultPanel.find('.add-cultura-tipo').val() || ''
        let statusResultado = resultPanel.find('.add-status_resultado').val() || ''
        let textResultado = '';
        if(statusResultado == '0') {
            textResultado = 'Em andamento';
            statusResultado = false;
        } else if(statusResultado == '1') {
            textResultado = 'Finalizado';
            statusResultado = true;
        }
        
        let findTipo = _.find(tiposCulturas, (item) => item.Id == tipoId)
        const tipo = findTipo != undefined && findTipo != null ? findTipo.Descricao : ''
        
        let templateNav = `
            <li role="presentation" class="${culturaId}_li">
                <a href="#${culturaId}" role="tab" data-toggle="tab" class="nav-item">
                    <span>${date} -  ${tipo} - ${textResultado}</span>
                    <button type="button" class="close text-danger ${culturaId}_close_btn" style="margin-left: 10px;margin-top: 5px;" aria-label="Close">
                      <span aria-hidden="true" class="text-danger">&times;</span>
                    </button>
                </a>                   
            </li>`
        let templateResultados = ``
        _.forEach(tiposResultados, (item) => {
            var id = randomizeInteger(0,9999999);
            templateResultados += `
                <div class="col-md-4">
                    <div class="md-checkbox">
                        <input type="checkbox" id="${culturaId}_resultad_${id}" name="${culturaId}_resultado[]" value="${item.Id}" class="form-control md-check" />
                        <label for="${culturaId}_resultad_${id}">
                            <span class="inc"></span>
                            <span class="check"></span>
                            <span class="box"></span>
                            ${item.Descricao}
                        </label>
                    </div>
                </div>`
        })

        let templateTabCultura = `
            <div role="tabpanel" class="tab-pane col-md-12" id="${culturaId}">
                <input type="hidden" name="idCultura" value="${culturaId}" />
                <input type="hidden" name="${culturaId}_dataCultura" value="${moment(date, 'DD/MM/YYYY').toISOString()}" />
                <input type="hidden" name="${culturaId}_tipoId" value="${tipoId}" />
                <input type="hidden" name="${culturaId}_StatusResultado" value="${statusResultado}" />
                <div class="row">
                    <div class="form-group">
                        <label class="col-md-12 text-bold">
                            Resultado:
                        </label>
                    </div>
                </div>
                <div class="col-md-6">
                ${templateResultados}
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label for="justificativa" class="text-bold">Outros Resultados:</label>
                        <textarea name="${culturaId}_outrosResultados" class="form-control input-sm" rows="4"></textarea>
                    </div>
                </div>
            </div>`

        navCulturas.append(templateNav)
        tabsCulturas.append(templateTabCultura)

        resultPanel.find(`[href="#${culturaId}"]`).tab('show')
        resultPanel.find('.add-cultura-date').val(null).trigger("change")
        resultPanel.find('.add-cultura-tipo').val(null).trigger("change")
        resultPanel.find('.add-status_resultado').val(null).trigger("change")

        $(`.${culturaId}_close_btn`).click(function (event) {
            event.stopImmediatePropagation()
            navCulturas.find(`.${culturaId}_li`).remove()
            tabsCulturas.find(`#${culturaId}`).remove()
            if (resultPanel.find("a.nav-item").length) {
                $(resultPanel.find("a.nav-item")[0]).tab('show')
            }
        })

        el.buttonBusy(false)
    }
})(jQuery)