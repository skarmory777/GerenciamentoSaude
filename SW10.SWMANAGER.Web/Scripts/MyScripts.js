(function ($) {

    // Comandos executados automaticamente ao carregar paginas princiapais (layout global)
    //CamposRequeridos();
    //$('#exibir-bi-' + $('#bi-id').val()).on('click', function (e) {
    //    e.preventDefault();
    //    //console.log($('#frame-relatorio-despesas').attr('src'));
    //    if ($('#frame-bi-' + $('#bi-id').val()).attr('src') == undefined) {
    //        $('#bi-' + $('#bi-id').val()).removeClass('hidden');
    //        $('#frame-bi-' + $('#bi-id').val()).attr('src', $('#bi-url').val());
    //    }
    //    else {
    //        $('#bi-' + $('#bi-id').val()).addClass('hidden');
    //        $('#frame-bi-' + $('#bi-id').val()).attr('src', null);
    //    }
    //});

    if ($.validator) {
        $.validator.addMethod("cpf", function (value, element) {
            value = jQuery.trim(value);

            value = value.replace('.', '');
            value = value.replace('.', '');
            cpf = value.replace('-', '');
            while (cpf.length < 11) cpf = "0" + cpf;
            var expReg = /^0+$|^1+$|^2+$|^3+$|^4+$|^5+$|^6+$|^7+$|^8+$|^9+$/;
            var a = [];
            var b = new Number;
            var c = 11;
            for (i = 0; i < 11; i++) {
                a[i] = cpf.charAt(i);
                if (i < 9) b += (a[i] * --c);
            }
            if ((x = b % 11) < 2) { a[9] = 0 } else { a[9] = 11 - x }
            b = 0;
            c = 11;
            for (y = 0; y < 10; y++) b += (a[y] * c--);
            if ((x = b % 11) < 2) { a[10] = 0; } else { a[10] = 11 - x; }

            var retorno = true;
            if ((cpf.charAt(9) != a[9]) || (cpf.charAt(10) != a[10]) || cpf.match(expReg)) retorno = false;

            return this.optional(element) || retorno;

        }, "Informe um CPF válido");

        $.validator.addMethod("cnpj", function (value, element) {

            var numeros, digitos, soma, i, resultado, pos, tamanho, digitos_iguais;
            if (value.length == 0) {
                return false;
            }

            value = value.replace(/\D+/g, '');
            digitos_iguais = 1;

            for (i = 0; i < value.length - 1; i++)
                if (value.charAt(i) != value.charAt(i + 1)) {
                    digitos_iguais = 0;
                    break;
                }
            if (digitos_iguais)
                return false;

            tamanho = value.length - 2;
            numeros = value.substring(0, tamanho);
            digitos = value.substring(tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0)) {
                return false;
            }
            tamanho = tamanho + 1;
            numeros = value.substring(0, tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }

            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

            return (resultado == digitos.charAt(1));
        }, "Informe um CNPJ válido");
    }

})(jQuery);

function SomenteNumero(campo, aDigits) {
    var digits = ["0123456789,", "0123456789ESAOesao", "0123456789", "0123456789:", "0123456789/", "0123456789-", "0123456789."];
    var campo_temp;
    var nomeCampo = '#' + campo.id;
    //for (var i = 0; i < campo.value.length; i++) {
    for (var i = 0; i < $(nomeCampo).val().length; i++) {
        campo_temp = $(nomeCampo).val().substring(i, i + 1);
        if (digits[aDigits].indexOf(campo_temp) == -1) {
            $(nomeCampo).val($(nomeCampo).val().substring(0, i));
            break;
        }
    }
}

function zeroEsquerda(num, length) {
    var str = "" + num
    var pad = ""
    for (var i = 0; i < length; i++) {
        pad += "0";
    }
    var ans = pad.substring(0, pad.length - str.length) + str
    return ans;
}

function ShowModal(url, title) {
    $('#SWManagerAlertModal .modal-body p').load(url, function () {
        $('#SWManagerAlertModal .modal-header h4').html(title);
        $('#SWManagerAlertModal').modal('show');
    });
}

function buscarCep(string) {
    $.ajax({
        data: {
            'cep': string
        },
        dataType: 'json',
        contentType: 'application/json',
        dataContext: 'application/json; charset=utf-8',
        url: '/mpa/Ceps/ConsultaCep',
        async: true,
        type: 'GET',
        cache: false,
        success: function (data) {
            var result = JSON.stringify(data.result);
            ////console.log(result);
            if (result.indexOf('ERRO') !== -1) {
                abp.notify.info(app.localize("Erro") + "<br />" + data.result.replace('ERRO:', ''));
                return false;
            }
            else {
                $('#cbo-paisid').append('<option value="' + data.result.paisId + '">' + data.result.estado.pais.nome + '</option>');
                $('#cbo-paisid').val(data.result.paisId);
                $('#cbo-paisid').trigger("change");

                $('#cbo-estadoid').append('<option value="' + data.result.estadoId + '">' + data.result.estado.nome + '</option>');
                $('#cbo-estadoid').val(data.result.estadoId);
                $('#cbo-estadoid').trigger("change");

                $('#cbo-cidadeid').append('<option value="' + data.result.cidadeId + '">' + data.result.cidade.nome + '</option>');
                $('#cbo-cidadeid').val(data.result.cidadeId);
                $('#cbo-cidadeid').trigger("change");

                $('#pais-search').val(data.result.estado.pais.nome).trigger("chosen:updated");
                $('#estado-search').val(data.result.estado.nome).trigger("chosen:updated");
                $('#cidade-search').val(data.result.cidade.nome).trigger("chosen:updated");

                $('#bairro').val(data.result.bairro).addClass("edited");
                $('#pais-id').val(data.result.paisId).trigger("chosen:updated");
                $('#estado-id').val(data.result.estadoId).trigger("chosen:updated");
                $('#cidade-id').val(data.result.cidadeId).trigger("chosen:updated");
                $('#logradouro').val(data.result.logradouro).addClass("edited");
                var _tipoLogradouroService = abp.services.app.tipoLogradouro;
                var _tipoLogradouro = _tipoLogradouroService.obter(data.result.tipoLogradouro.descricao)
                    .done(function (data) {
                        $('#tipo-logradouro-id')
                            .append('<option value="' + data.id + '" selected>' + data.descricao + '</option>')
                            .trigger("chosen:updated");
                    });
            }
        },
        beforeSend: function () {
            $('#btn-buscar-cep').buttonBusy(true);
        },
        complete: function () {
            $('#btn-buscar-cep').buttonBusy(false);

        }
    });
}

function IsDate(dateStr) {
    var matchArray = dateStr.match(datePat); // is the format ok?
    if (matchArray == null) {
        return false;
    }
    var locale = moment.locale();
    switch (locale) {
        case 'pt-br':
        case 'pt-BR':
        case 'PT-BR':
        case 'pt-Br':
            var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
            var matchArray = dateStr.match(datePat); // is the format ok?
            if (matchArray == null) {
                return false;
            }
            month = matchArray[3]; // p@rse date into variables
            day = matchArray[1];
            year = matchArray[5];
            break;
        case 'en':
        case 'En':
        case 'EN':
        case 'en-us':
        case 'En-US':
        case 'EN-US':
        case 'en-US':
        case 'en-Us':
            var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
            var matchArray = dateStr.match(datePat); // is the format ok?
            if (matchArray == null) {
                return false;
            }
            month = matchArray[1]; // p@rse date into variables
            day = matchArray[3];
            year = matchArray[5];
            break;
        default:
            var datePat = /^(\d{1,4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})$/;
            var matchArray = dateStr.match(datePat); // is the format ok?
            if (matchArray == null) {
                return false;
            }
            month = matchArray[5]; // p@rse date into variables
            day = matchArray[7];
            year = matchArray[1];
    }
    if (month < 1 || month > 12) { // check month range
        return false;
    }
    if (day < 1 || day > 31) {
        return false;
    }
    if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {
        return false;
    }
    if (month == 2) { // check for february 29th
        var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        if (day > 29 || (day == 29 && !isleap)) {
            return false;
        }
    }
    return true; // date is valid
}

function barraData(n) {
    if (n.value.length == 2)
        c.value += '/';
    if (n.value.length == 5)
        c.value += '/';
}

function updateCriarOuEditarAgendamentoConsultaViewModel() {
    var myDate = $('input[name="DataAgendamento"]').val();
    var aDate = myDate.split('/');
    var oldMedicoId = $('#form-medico-id').length > 0 ? $('#form-medico-id') : 0;
    var oldMedicoEspecialidadeId = $('#form-medico-especialidade-id').length > 0 ? $('#form-medico-especialidade-id').val() : 0;
    var oldHoraAgendamento = $('#form-hora-agendamento').length > 0 ? $('#form-hora-agendamento').val() : '00:00';
    var oldAgendamentoConsultaMedicoDisponibilidadeId = $('#form-agendamento-consulta-medico-disponibilidade-id').length > 0 ? $('#form-agendamento-consulta-medico-disponibilidade-id').val() : 0;

    $('#div-medicos').load(
        '/mpa/AgendamentoConsultas/_MontarComboMedicos',
        {
            date: aDate[2] + '-' + aDate[1] + '-' + aDate[0]
        },
        function () {
            if (oldMedicoId > 0) {
                $('#form-medico-id').val(oldMedicoId);
            }
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('#form-medico-id').on('change', function (e) {
                e.preventDefault();
                var id = $(this).val();
                var myDate = $('input[name="DataAgendamento"]').val();
                var aDate = myDate.split('/');
                $('#div-medico-especialidades').load('/mpa/AgendamentoConsultas/_MontarComboMedicoEspecialidades',
                    {
                        medicoId: id,
                        date: aDate[2] + '-' + aDate[1] + '-' + aDate[0]
                    },
                    function () {
                        if (oldMedicoEspecialidadeId > 0) {
                            $('#form-medico-especialidade-id').val(oldMedicoId);
                        }
                        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' }).trigger("chosen:updated");
                        $('#form-medico-especialidade-id').on('change', function (e) {
                            e.preventDefault();
                            var medicoEspecialidadeId = $('#form-medico-especialidade-id').val();
                            var id = $('#id').length > 0 ? $('#id').val() : 0;
                            var myDate = $('input[name="DataAgendamento"]').val();
                            var aDate = myDate.split('/');
                            var medicoId = $('#form-medico-id').val();
                            $('#div-horarios').load('/mpa/AgendamentoConsultas/_MontarComboHorarios',
                                {
                                    date: aDate[2] + '-' + aDate[1] + '-' + aDate[0],
                                    medicoId: medicoId,
                                    medicoEspecialidadeId: medicoEspecialidadeId,
                                    id: id
                                },
                                function () {
                                    if (oldAgendamentoConsultaMedicoDisponibilidadeId > 0) {
                                        $('#agendamento-consulta-medico-disponibilidade-id')
                                            .val(
                                                $('#agendamento-consulta-medico-disponibilidade-id option')
                                                    .filter(function () {
                                                        return $(this).html() === oldHoraAgendamento;
                                                    })
                                                    .val()
                                            );
                                    }
                                    $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
                                    $('#form-agendamento-consulta-medico-disponibilidade-id').on('change', function (e) {
                                        e.preventDefault();
                                        var hora = $('#form-agendamento-consulta-medico-disponibilidade-id option:selected').text();
                                        var data = $('#form-data-agendamento').val();
                                        $('#form-hora-agendamento').val(data + ' ' + hora);
                                    });
                                }
                            );

                        });
                    }
                );
            });
        }
    );

}

function lerAtendimentoAmbulatorioEmergencia(id, origem, tipoAtendimento) {
    $('li a').attr('aria-expanded', 'false');
    $('#abas-' + tipoAtendimento + ' li').removeClass('active')
    $('#conteudo-abas-' + tipoAtendimento + ' div.tab-pane').removeClass('active')
    var url = origem == "assistencial" ? '/mpa/assistenciais/_leratendimento/' : 'Mpa/Atendimentos/Index?abaId=' + id;
    $('#conteudo-atendimento-' + id).load(url + id, function () {
        $('#link-atendimento-' + id).attr('aria-expanded', 'true');
        $(this).addClass("active")
        $('#conteudo-atendimento-' + id).addClass('active');


        localStorage["AtendimentoId"] = id;
        localStorage["TargetAssistencial"] = '#conteudo-atendimento-' + id;
    });
}

$('#abas-amb').on('click', ' li a .fa-close', function () {
    //    $('#abas-amb li').removeClass('active').removeAttr('aria-expanded');
    //    $('#conteudo-abas-amb div.tab-pane').removeClass('active').removeAttr('aria-expanded');
    var tabId = $(this).parents('li').children('a').attr('href');
    $(this).parents('li').remove('li').remove('div.tab-pane');
    $(tabId).remove();
    var atendimentoId = localStorage["AtendimentoId"];
    $('#link-atendimento-' + atendimentoId).trigger('click'); //.tab('show');
});

$('#abas-int').on('click', ' li a .fa-close', function () {
    //    $('#abas-int li').removeClass('active').removeAttr('aria-expanded');
    //    $('#conteudo-abas-int div.tab-pane').removeClass('active').removeAttr('aria-expanded');
    var tabId = $(this).parents('li').children('a').attr('href');
    $(this).parents('li').remove('li').remove('div.tab-pane');
    $(tabId).remove();

    //$('#link-principal-int').trigger('click'); //.tab('show');


    //atualizarAtendimento(id);

    var aId = tabId.split('-');
    var id = aId[1];

    if (tabId == "#conteudo-atendimento-Prontuário") {

        $('#link-atendimento-' + sessionStorage["id"]).trigger('click'); //.tab('show');
    }
    else if (tabId == "#conteudo-atendimento-Prescrição") {

        $('#link-atendimento-' + sessionStorage["id"]).trigger('click'); //.tab('show');
    }
    else {

        $('#link-atendimento-' + aId[4]).trigger('click'); //.tab('show');
    }
    // $('#conteudo-atendimento-' + aId[4]).remove();
    //    ////console.log('Removido');
    $('.jtable-row-selected').removeClass('jtable-row-selected');
    $(':checked').removeAttr('checked');
    localStorage.removeItem("AtendimentoId");
});





function atualizarAtendimento(id) {
    localStorage["AtendimentoId"] = id;
    localStorage["TargetAssistencial"] = '#conteudo-atendimento-' + id;
}

function fecharAtendimentoAmbulatorioEmergencia(id) {
    debugger;
    $('#atendimento-' + id).detach();
    $('#conteudo-atendimento-' + id).detach();
    //alert($('#abas a:first').attr('id'));
    if ($('#abas-amb').length > 0) {
        $('#abas-amb a:first').tab('show');
    }
    else {
        $('#abas-int a:first').tab('show');
    }
    localStorage.removeItem("AtendimentoId");
}

$('.favorito').on('click', function (e) {
    e.preventDefault();

    var _icon = $(this).attr('data-icone');
    var _nome = $(this).find('#fav-nome').val();
    var _url = $(this).find('#fav-url').val();

    var _data = {
        "UserId": 0,
        "DisplayName": _nome,
        "Icon": _icon,
        "Name": _nome,
        "Url": _url
    }

    ////console.log(JSON.stringify(_data));

    $.ajax({
        type: "POST",
        url: '/Mpa/Layout/Favoritar',
        data: _data,
        success: function (result) {
            if (result == '1') {
                $('.favorito span').removeClass('fa-star-o').addClass('fa-star');
            }
            else if (result == '0') {
                $('.favorito span').removeClass('fa-star').addClass('fa-star-o');
            }
            else {

            }
        },
    });
});

// Imagens
function lerImagemForm(input, dataField, mimeTypeField, imageTag) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var dados = {};
            var base64 = e.target.result;
            dados.base64 = base64.substr(base64.indexOf(',') + 1, base64.length);
            var type = base64.substr(base64.indexOf(':') + 1, base64.indexOf(';') - 5);
            $('#' + dataField).val(dados.base64);
            $('#' + mimeTypeField).val(type);
            $('#' + imageTag).attr({
                'src': 'data:' + type + ';base64,' + dados.base64
            });
        }
        reader.readAsDataURL(input.files[0]);
    }
}

// Gerenciamento de abas
function LerParcial(id, url, parametro) {
    if ($('#' + id).data('reload') == '0') {
        $('#' + id).data('reload', '1');
        sessionStorage["TargetConteudo"] = id;
        $.ajaxSetup({ cache: true, async: true });

        criarNewAba(sessionStorage["id"],
            sessionStorage["dataRegistro"],
            sessionStorage["codigoAtendimento"],
            sessionStorage["paciente"],
            url,
            id);
    } else {
        criarNewAba(sessionStorage["id"], sessionStorage["dataRegistro"], sessionStorage["codigoAtendimento"], sessionStorage["paciente"], url, id);
    }
}


function criarNewAba(id, dataRegistro, codigoAtendimento, paciente, url, pagina, idPrescricao) {

    if (id !== null) {

        if (idPrescricao == undefined) {
            idPrescricao = 0;
        }
        localStorage["AtendimentoId"] = id;
        localStorage["DataAtendimento"] = moment(dataRegistro).format();
        var format = url.split('/');

        var ans = codigoAtendimento !== null ? zeroEsquerda(codigoAtendimento, '10') : '';
        sessionStorage["ControleAba"] = url;

        if (pagina == "Prescrição") {

            if (sessionStorage["NovaPrescricao"] == "S") {
                $('<li id="atendimento-' + pagina + '-' + '0" name="Atendimento-' + id + '-' + 0 + '"><a id="link-atendimento-' + id + '-' + '0" href="#conteudo-atendimento-' + pagina + '-' + idPrescricao + '" data-toggle="tab" title="' + paciente + '" onclick="atualizarAtendimento(' + "'" + id + "'" + ');"> ' + pagina + ' - NOVA' + '</a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            }
            else {
                sessionStorage["NovaPrescricao"] = "N";
                $('<li id="atendimento-' + pagina + '-' + idPrescricao + '" name="Atendimento-' + id + '-' + idPrescricao + '"><a id="link-atendimento-' + id + '-' + idPrescricao + '" href="#conteudo-atendimento-' + pagina + '-' + idPrescricao + '" data-toggle="tab" title="' + paciente + '" onclick="atualizarAtendimento(' + "'" + id + "'" + ');"> ' + pagina + ' - ' + moment(sessionStorage["dataPrescricao"]).format('DD/MM/YYYY HH:mm') + '</a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            }


            $('<div class="tab-pane" id="conteudo-atendimento-' + pagina + '-' + idPrescricao + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]);//.load(url);


            $('<iframe" id="formulario-dinamico-area-' + id + '-' + idPrescricao + '" style="padding:5px;">').appendTo('#conteudo-atendimento-' + pagina + '-' + idPrescricao).load(url);

            //$('#conteudo-atendimento-' + pagina + idPrescricao).addClass('active');
            //$('#atendimento-' + pagina + idPrescricao).addClass('active'); // 233

            //$('#conteudo-atendimento-' + 'tab-horizontal-' + id + '-0-5').removeClass('active'); //2781
            //$('#atendimento-' + 'tab-horizontal-' + id + '-0-5').removeClass('active');

            setTimeout(() => {
                $("#link-atendimento-" + id + '-' + idPrescricao).tab("show");
            }, 200);

        }
        else if (pagina == "Prontuário") {
            pagina = pagina.replace("á", "a");
            $('<li id="atendimento-' + pagina + '-' + idPrescricao + '" name="Atendimento-' + id + '-' + idPrescricao + '"><a id="link-atendimento-' + id + '-' + idPrescricao + '" href="#conteudo-atendimento-' + pagina + '-' + idPrescricao + '" data-toggle="tab" title="' + paciente + '" onclick="atualizarAtendimento(' + "'" + id + "'" + ');"> ' + pagina + '  ' + moment(sessionStorage["dataAdmissao"]).format('DD/MM/YYYY HH:mm') + '<i class="fa fa-close"></i></a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);

            $('<div class="tab-pane" id="conteudo-atendimento-' + pagina + '-' + idPrescricao + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).html("<iframe name='FormularioDinamicoArea22' frameborder='0' id='formulario-dinamico-area22' src='" + url + "' class='embed-responsive-item' />");

            //$('#conteudo-atendimento-' + pagina).addClass('active');
            //$('#atendimento-' + pagina).addClass('active'); // 233

            //$('#conteudo-atendimento-' + 'tab-horizontal-' + id + '-1-0').removeClass('active'); //2781
            //$('#atendimento-' + 'tab-horizontal-' + id + '-1-0').removeClass('active');

            setTimeout(() => {
                $("#link-atendimento-" + id + '-' + idPrescricao).tab("show");
            }, 200);


        }
        else if (pagina == "Receituário") {
            var idReceituario = (url.split(name + '=')[2] || '').split('&')[0];
            pagina = pagina.replace("á", "a");
            $('<li id="atendimento-' + pagina + '-' + idReceituario + '" name="Atendimento-' + id + '-' + idReceituario + '"><a id="link-atendimento-' + id + "-" + idReceituario + '" href="#conteudo-atendimento-' + pagina + "-" + idReceituario + '" data-toggle="tab" title="' + paciente + '" onclick="atualizarAtendimento(' + "'" + id + "'" + ');"> ' + pagina + '  ' + '<i class="fa fa-close"></i></a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            $('<div class="tab-pane" id="conteudo-atendimento-' + pagina + '-' + idReceituario + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load(url);

            //$('#conteudo-atendimento-' + pagina).addClass('active');
            //$('#atendimento-' + pagina).addClass('active'); // 233

            //$('#conteudo-atendimento-' + id).removeClass('active'); //2781
            //$('#atendimento-' + id).removeClass('active');

            setTimeout(() => {
                $("#link-atendimento-" + id + '-' + idReceituario).tab("show");
            }, 3000);
        }
        else if (format[3] == "MedicoAlta") {
            $('<li id="atendimento-' + pagina + '" name="Atendimento-' + id + '"><a id="link-atendimento-' + id + '" href="#conteudo-atendimento-' + pagina + '" data-toggle="tab" title="' + paciente + '" onclick="atualizarAtendimento(' + "'" + id + "'" + ');"> ' + format[3] + '  ' + '<i class="fa fa-close"></i></a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            $('<div class="tab-pane" id="conteudo-atendimento-' + pagina + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load('/Mpa/AtendimentoLeitoMov/_AltaModal?atendimentoId=' + id);


            $('#conteudo-atendimento-' + pagina).addClass('active');
            $('#atendimento-' + pagina).addClass('active'); // 233

            $('#conteudo-atendimento-' + id).removeClass('active'); //2781
            $('#atendimento-' + id).removeClass('active');

        }
        else {
            $('<li id="atendimento-' + pagina + '" name="Atendimento-' + id + '"><a id="link-atendimento-' + id + '" href="#conteudo-atendimento-' + pagina + '" data-toggle="tab" title="' + paciente + '" onclick="atualizarAtendimento(' + "'" + id + "'" + ');"> ' + format[3] + '  ' + '<i class="fa fa-close"></i></a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            $('<div class="tab-pane" id="conteudo-atendimento-' + pagina + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load(url);

            $('#conteudo-atendimento-' + pagina).addClass('active');
            $('#atendimento-' + pagina).addClass('active'); // 233

            $('#conteudo-atendimento-' + id).removeClass('active'); //2781
            $('#atendimento-' + id).removeClass('active');
        }



    }
}


function gerenciarAba(parametros) {
    console.log(parametros);
    if (parametros == null || parametros == '') {
        return;
    }

    var parametrosAba = {
        id: sessionStorage["id"],
        dataRegistro: sessionStorage["dataRegistro"],
        codigoAtendimento: sessionStorage["codigoAtendimento"],
        paciente: sessionStorage["paciente"],
        url: parametros.url,
        pagina: parametros.id,
        idPrescricao: parametros.idPrescricao,
        tabName: parametros.tabName,
        tabIcon: parametros.tabIcon
    }
    if ($('#' + parametros.id).data('reload') == '0' || !$('[href="#conteudo-atendimento-' + parametrosAba.pagina + '"]').length) {
        $('#' + parametros.id).data('reload', '1');
        sessionStorage["TargetConteudo"] = parametros.id;
        $.ajaxSetup({ cache: true, async: true });
        criarAba(parametrosAba);
    } else {
        console.log(parametrosAba);
        $('[href="#conteudo-atendimento-' + parametrosAba.pagina + '"]').tab('show');
        abp.event.trigger('reloadTab', { pagina: parametros.id });
    }
}


function criarAba(parametrosAba) {

    if (parametrosAba.id !== null) {

        if (parametrosAba.idPrescricao == undefined) {
            parametrosAba.idPrescricao = 0;
        }

        localStorage["AtendimentoId"] = parametrosAba.id;
        localStorage["DataAtendimento"] = moment(parametrosAba.dataRegistro).format();
        var format = parametrosAba.url.split('/');

        var ans = parametrosAba.codigoAtendimento !== null ? zeroEsquerda(parametrosAba.codigoAtendimento, '10') : '';
        sessionStorage["ControleAba"] = parametrosAba.url;

        if (parametrosAba.pagina == "Prescrição") {

            if (sessionStorage["NovaPrescricao"] == "S") {
                $('<li id="atendimento-' + parametrosAba.pagina + '0" name="Atendimento-' + parametrosAba.id + 0 + '"><a id="link-atendimento-' + parametrosAba.id + '0 " href="#conteudo-atendimento-' + parametrosAba.pagina + parametrosAba.idPrescricao + '" data-toggle="tab" title="' + parametrosAba.paciente + '" style="font-size: 14px;" onclick="atualizarAtendimento(' + "'" + parametrosAba.id + "'" + ');"> ' + "<i class='" + parametrosAba.tabIcon + "' style='padding-right:5px'> </i>" + parametrosAba.tabName + ' - NOVA' + '</a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            }
            else {
                sessionStorage["NovaPrescricao"] = "N";
                $('<li id="atendimento-' + parametrosAba.pagina + parametrosAba.idPrescricao + '" name="Atendimento-' + parametrosAba.id + parametrosAba.idPrescricao + '"><a id="link-atendimento-' + parametrosAba.id + parametrosAba.idPrescricao + '" href="#conteudo-atendimento-' + parametrosAba.pagina + parametrosAba.idPrescricao + '" data-toggle="tab" title="' + parametrosAba.paciente + '"  style="font-size: 14px;" onclick="atualizarAtendimento(' + "'" + parametrosAba.id + "'" + ');"> ' + "<i class='" + parametrosAba.tabIcon + "' style='padding-right:5px'> </i>" + parametrosAba.tabName + ' - ' + moment(sessionStorage["dataPrescricao"]).format('DD/MM/YYYY HH:mm') + '</a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            }

            $('<div class="tab-pane" id="conteudo-atendimento-' + parametrosAba.pagina + parametrosAba.idPrescricao + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]);//.load(url);
            $('<iframe" id="formulario-dinamico-area-' + parametrosAba.id + '-' + parametrosAba.idPrescricao + '" style="padding:5px;">').appendTo('#conteudo-atendimento-' + parametrosAba.pagina + parametrosAba.idPrescricao).load(parametrosAba.url + "?id=" + parametrosAba.id);

            $('[href="#conteudo-atendimento-tab-horizontal-' + parametrosAba.id + '-1-5"]').tab('show');

        }
        else if (parametrosAba.pagina == "Prontuário") {

            $('<li id="atendimento-' + parametrosAba.pagina + '" name="Atendimento-' + parametrosAba.id + 0 + '"><a id="link-atendimento-' + parametrosAba.id + 0 + '" href="#conteudo-atendimento-' + parametrosAba.pagina + '" data-toggle="tab" title="' + parametrosAba.paciente + '"  style="font-size: 14px;" onclick="atualizarAtendimento(' + "'" + parametrosAba.id + "'" + ');"> ' + "<i class='" + parametrosAba.tabIcon + "' style='padding-right:5px'> </i>" + parametrosAba.tabName + '  ' + moment(sessionStorage["dataAdmissao"]).format('DD/MM/YYYY HH:mm') + '<i class="fa fa-close"></i></a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);

            $('<div class="tab-pane" id="conteudo-atendimento-' + parametrosAba.pagina + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).html("<iframe name='FormularioDinamicoArea22' frameborder='0' id='formulario-dinamico-area22' src='" + parametrosAba.url + "?id=" + parametrosAba.id + "' class='embed-responsive-item' />");

            $('[href="#conteudo-atendimento-tab-horizontal-' + parametrosAba.id + '-1-0"]').tab('show');

        }
        else if (format[3] == "MedicoAlta") {
            $('<li id="atendimento-' + parametrosAba.pagina + '" name="Atendimento-' + parametrosAba.id + '"><a id="link-atendimento-' + parametrosAba.id + '" href="#conteudo-atendimento-' + parametrosAba.pagina + '" data-toggle="tab" title="' + parametrosAba.paciente + '"  style="font-size: 14px;" onclick="atualizarAtendimento(' + "'" + parametrosAba.id + "'" + ');"> ' + "<i class='" + parametrosAba.tabIcon + "' style='padding-right:5px'> </i>" + parametrosAba.tabName + '  ' + '<i class="fa fa-close"></i></a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            $('<div class="tab-pane" id="conteudo-atendimento-' + parametrosAba.pagina + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load('/Mpa/AtendimentoLeitoMov/_AltaModal?atendimentoId=' + parametrosAba.id);
            $('[href="#conteudo-atendimento-' + parametrosAba.pagina + '"]').tab('show');

        }
        else {
            $('<li id="atendimento-' + parametrosAba.pagina + '" name="Atendimento-' + parametrosAba.id + '"><a id="link-atendimento-' + parametrosAba.id + '" href="#conteudo-atendimento-' + parametrosAba.pagina + '" data-toggle="tab" title="' + parametrosAba.paciente + '"  style="font-size: 14px;" onclick="atualizarAtendimento(' + "'" + parametrosAba.id + "'" + ');"> ' + "<i class='" + parametrosAba.tabIcon + "' style='padding-right:5px' > </i>" + parametrosAba.tabName + '  ' + '<i class="fa fa-close"></i></a></li>').appendTo('#abas-' + localStorage["TipoAtendimento"]);
            $('<div class="tab-pane" id="conteudo-atendimento-' + parametrosAba.pagina + '" style="padding:5px;">').appendTo('#conteudo-abas-' + localStorage["TipoAtendimento"]).load(parametrosAba.url + "?id=" + parametrosAba.id);

            $('[href="#conteudo-atendimento-' + parametrosAba.pagina + '"]').tab('show');
        }
    }
}





// Insere asterisco em todos os campos requeridos (Rodrigo - 06/07/17)
// Funciona para elmentos cuja label esteja imediatamente antes do form-control
function CamposRequeridos() {
    $('.form-control').each(function () {

        // convertendo para elemento javascript para usar funcao hassAttribute
        var elementoJS = $(this)[0];
        var requerido = elementoJS.hasAttribute("required");

        // revertendo para elemento Jquery para usar funcao prev e after
        var elementoJquery = $(elementoJS);
        if (requerido) {

            if (elementoJquery.closest(".form-group").find("label").length && !elementoJquery.closest(".form-group").find("label").find("b.required-label").length) {
                elementoJquery.closest(".form-group").find("label").append("<b class='required-label' style='color:#ff0000'> *</b>");
            }
            else {
                if (elementoJquery.prevAll('label').length && !elementoJquery.prevAll('label').find("b.required-label").length)
                    elementoJquery.prevAll('label').after("<b class='required-label' style='color:#ff0000'> *</b>");
            }
        }
        else {
            if (elementoJquery.closest(".form-group").find("label").length) {
                elementoJquery.closest(".form-group").find("label").find("b.required-label").remove();
            }
            elementoJquery.prevAll('label').find("b.required-label").remove();
        }
    });
}

function CriarAutoComplete(idSearch, idCampo, url, cadastro) {

    var search = '#' + idSearch;
    var campo = '#' + idCampo;

    $(search)
        .autocomplete({
            minLength: 3,
            delay: 0,
            source: function (request, response) {
                var term = $(search).val();
                // var url = ;
                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.length == 0) {
                        $(campo).val(0);
                        $(search).focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data, function (item) {
                        $(campo).val(0);
                        return {
                            label: item.Nome,
                            value: item.Nome,
                            realValue: item.Id,

                        };
                    }));
                });
            },
            select: function (event, ui) {
                $(campo).val(ui.item.realValue);
                $(search).val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $(campo).val(0);
                    $(search).val('').focus();
                    abp.notify.info(app.localize("AutoCompleteInvalido").replace('$cadastro', cadastro));
                    return false;
                }
            },
        });
}

function posicionarDireita(str) {
    return "<div style=\"float: right;\">" + str + "</div>";

}

function newForm() {
    console.log("newForm");
    $('input[type=text]').val('');
    $('input[type=checkbox]').removeAttr('checked');
    $('input[type=radio]').removeAttr('checked');
    $('textarea').val('');
    var date = moment().format('L');
    var dateSingle = $('.date-single-picker');
    dateSingle.each(function (index) {
        var obj = $(this);

        obj.daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            //autoUpdateInput: false,
            //maxDate: new Date(),
            changeYear: true,
            //yearRange: 'c-10:c+10',
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
        },
            function (start, end, label) {
                $(this).val(moment(end).format('L'));
                obj.trigger('input');
                obj.trigger('change');
            });
    });
    var _selectedDateRange = {
        startDate: moment().add('-6', 'day').startOf('day'),
        endDate: moment().endOf('day')
    };

    var dateRange = $('.date-range-picker');
    dateRange.each(function (index) {
        var obj = $(this);

        obj.daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                obj.val(start + ' - ' + end);
                obj.trigger('input');
                obj.trigger('change');
            }
        );
    });
}

function editForm() {
    console.log("editForm");
    var dateSingle = $('.date-single-picker');

    dateSingle.each(function (index) {
        var obj = $(this);
        ////console.log(obj.prop('id'));
        ////console.log(obj.val());
        obj.daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            //autoUpdateInput: false,
            //maxDate: new Date(),
            changeYear: true,
            //yearRange: 'c-10:c+10',
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
        },
            function (start, end, label) {
                obj.val(moment(end).format('L'));
                obj.trigger('input');
                obj.trigger('change');
            });
        //obj.on('apply.daterangepicker', function (ev, picker) {
        //    $(this).val(picker.endDate.format('L'));
        //    //console.log(obj.val());
        //});

        //obj.on('cancel.daterangepicker', function (ev, picker) {
        //    $(this).val('').change();
        //});

    });

    var _selectedDateRange = {
        startDate: moment().add('-6', 'day').startOf('day'),
        endDate: moment().endOf('day')
    };

    var dateRange = $('.date-range-picker');
    dateRange.each(function (index) {
        var obj = $(this);
        ////console.log(obj.prop('id'));
        ////console.log(obj.val());
        obj.daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                obj.val(start + ' - ' + end);
                obj.trigger('input');
                obj.trigger('change');
            });
        //obj.on('apply.daterangepicker', function (ev, picker) {
        //    $(this).val(picker.startDate.format('L') + ' - ' + picker.endDate.format('L'));
        //    //console.log($(this).val()).change();
        //});

    });
}

function aplicarDateSingle() {
    let dateSingle = $('.date-single-picker');
    dateSingle.each(function (index) {

        let obj = $(this);
        if (obj.data("daterangepicker") != undefined) {
            return;
        }

        let options = {
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            changeYear: true,
            //yearRange: 'c-10:c+10',
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
        };

        if(obj.data('minDate')) {
            options.minDate = moment(obj.data('minDate'),'DD/MM/YYYY');
        }

        obj.daterangepicker(options, function (start, end, label) {
            obj.val(moment(end).format('L'));
        });
    });

    aplicaMascaraDateTime();
}

function aplicaMascaraDateTime() {
    $(".date-single-picker.date-time").mask("00/00/0000 00:00:00");

    $(".date-single-picker:not(.date-time)").mask("00/00/0000");

}

function aplicarDateRange() {
    var _selectedDateRange = {
        startDate: moment().add('-6', 'day').startOf('day'),
        endDate: moment().endOf('day')
    };

    var dateRange = $('.date-range-picker');
    dateRange.each(function (index) {
        var obj = $(this);
        if (obj.data("daterangepicker") != undefined) {
            return;
        }

        obj.daterangepicker({
            minDate: moment().add(-10, 'year'),
            maxDate: moment().add(10, 'year'),
            startDate: moment().add('-6', 'day').startOf('day'),
            endDate: moment().endOf('day'),
            showDropdowns: true,
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Aplicar",
                "cancelLabel": "Cancelar",
                "fromLabel": "De",
                "toLabel": "Até",
                "customRangeLabel": "Personalizado",
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
            },
            //$.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function(start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                obj.val(start + ' - ' + end);
                obj.trigger('input');
                obj.trigger('change');
            }
        });
    });
}

//LOAD PARA ABERTURAS DAS ABAS DE ATENDIMENTO (PABLO)
var waitingDialog = waitingDialog || (function ($) {
    'use strict';

    // PADRÃO
    //var $dialog = $(
    //    '<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
    //    '<div class="modal-dialog modal-m">' +
    //    '<div class="modal-content">' +
    //        '<div class="modal-header"><h3 style="margin:0;"></h3></div>' +
    //        '<div class="modal-body">' +
    //            '<div class="fa fa-spinner fa-spin fa-3x fa-fw"" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
    //        '</div>' +
    //    '</div></div></div>');

    //CUSTOMIZADO
    var $dialog = $(
        '<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
        '<div class="modal-dialog modal-m">' +
        '<div class="modal-body">' +
        '<div class="fa fa-spinner fa-pulse fa-5x fa-fw" style="margin-bottom:0;margin-left:40%;"><div class="progress-bar" style="width: 100%;"></div></div>' +
        '</div>' +
        '</div></div></div>');

    return {
        //'<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>'
        show: function (message, options) {
            // Assigning defaults
            if (typeof options === 'undefined') {
                options = {};
            }
            if (typeof message === 'undefined') {
                message = 'Carregando...';
            }
            var settings = $.extend({
                dialogSize: 'm',
                progressType: '',
                onHide: null // This callback runs after the dialog was hidden
            }, options);

            // Configuring dialog
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h3').text(message);
            // Adding callbacks
            if (typeof settings.onHide === 'function') {
                $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                    settings.onHide.call($dialog);
                });
            }
            // Opening dialog
            $dialog.modal();
        },
        /**
         * Closes dialog
         */
        hide: function () {
            $dialog.modal('hide');
        }
    };







})(jQuery);

// SWMWE Helpers
function selectSW(properties, url, elementoFiltro) {
    if (!_.isObject(properties)) {
        return _DefaultSelectSW(properties, url, elementoFiltro);
    }
    return _PropertySelectSW(properties);
}

function _DefaultSelectSW(classe, url, elementoFiltro) {
    $(classe).css('width', '100%');
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };



    function filtrar() {
        if (elementoFiltro) {
            var retorno = null;
            if (elementoFiltro.valor != undefined) {
                retorno = elementoFiltro.valor;
            } else if ((elementoFiltro != undefined) && (elementoFiltro != null) && (elementoFiltro != 0) && (elementoFiltro != '0')) {
                if (elementoFiltro.val()) {
                    retorno = elementoFiltro.val();
                } else {
                    retorno = null;// elementoFiltro;
                }
            } else if (elementoFiltro.val()) {
                retorno = elementoFiltro.val();
            }
            return retorno;
        }
        else {
            return null;
        }
    }

    $(classe).select2({
        allowClear: true,
        placeholder: app.localize("SelecioneLista"),
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            method: 'Post',
            data: function (params) {
                //   //console.log('data: ', params, (params.page == undefined));
                if (params.page == undefined)
                    params.page = '1';
                //   //console.log('data: ', params);

                return {
                    search: params.term,
                    page: params.page,
                    totalPorPagina: 10,
                    filtro: filtrar()
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                return {
                    results: data.result.items,
                    pagination: {
                        more: (params.page * 10) < data.result.totalCount
                    }
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 0
    });
}

function _PropertySelectSW(properties) {
    const classe = properties.classe;
    $(classe).css('width', '100%');
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };



    function filtrar() {
        if (properties.elementoFiltro) {
            let elementoFiltro = properties.elementoFiltro;
            var retorno = null;
            if (elementoFiltro.valor != undefined) {
                retorno = elementoFiltro.valor;
            } else if ((elementoFiltro != undefined) && (elementoFiltro != null) && (elementoFiltro != 0) && (elementoFiltro != '0')) {
                if (elementoFiltro.val()) {
                    retorno = elementoFiltro.val();
                } else {
                    retorno = null;// elementoFiltro;
                }
            } else if (elementoFiltro.val()) {
                retorno = elementoFiltro.val();
            }
            return retorno;
        }
        else {
            return null;
        }
    }

    $(classe).select2({
        allowClear: true,
        placeholder: app.localize("SelecioneLista"),
        ajax: {
            url: properties.url,
            dataType: 'json',
            delay: 250,
            method: 'Post',
            data: function (params) {
                //   //console.log('data: ', params, (params.page == undefined));
                if (params.page == undefined) {
                    params.page = '1';
                }
                //   //console.log('data: ', params);
                const _data = {
                    search: params.term,
                    page: params.page,
                    totalPorPagina: 10,
                    filtro: filtrar()
                };

                if (properties.dataHandler && _.isFunction(properties.dataHandler)) {
                    return properties.dataHandler(_data, properties);
                }
                return _data;
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                return {
                    results: data.result.items,
                    pagination: {
                        more: (params.page * 10) < data.result.totalCount
                    }
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 0
    });

    if (properties.onChange) {
        $(classe).on("change", properties.onChange);
    }
}




// SWMWE Helpers
function selectSWWithDefaultValue(classe, url, elementoFiltro, select2Options) {
    const select2 = $(classe);
    select2.css('width', '100%');
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };

    function filtrar() {
        function filtroArray(elementoFiltro) {
            let result = [];
            _.forEach(elementoFiltro, (data,index) => {
                result.push(retornaElemento(data));
            })
            return result;
        }
        function retornaElemento(el) {
            let result = null;
            if (el.valor != undefined) {
                result = el.valor;
            } else if(typeof(el) == 'string' ){
                result = el;
            }
            else if ((el != undefined) && (el != null) && (el != 0) && (el != '0')) {
                if (el.val()) {
                    result = el.val();
                } else {
                    result = null;
                }
            } else if (el.val()) {
                result = el.val();
            }
            return result;
        }
        if (elementoFiltro) {
            if(_.isArray(elementoFiltro)) {
                return filtroArray(elementoFiltro);
            }
            else {
                return retornaElemento(elementoFiltro);
            }
        }
        else {
            return null;
        }
    }
    const ajax = {
        url: url,
        dataType: 'json',
        delay: 250,
        method: 'Post',

        data: function (params) {
            //   //console.log('data: ', params, (params.page == undefined));
            if (params.page == undefined)
                params.page = '1';
            //   //console.log('data: ', params);
            const result = {
                search: params.term,
                page: params.page,
                totalPorPagina: 10
            };

            const filtro = filtrar();
            if (filtro && filtro !== "") {
                result.filtro = filtro;
                result.filtros = filtro;
            }

            return result;
        },
        processResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.result.items,
                pagination: {
                    more: (params.page * 10) < data.result.totalCount
                }
            };
        },
        cache: true
    };
    const defaultSelect2Options = {
        allowClear: true,
        placeholder: app.localize("SelecioneLista"),
        ajax,
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 0
    };

    if (!_.isUndefined(select2Options) && _.isObject(select2Options)) {
        _.extend(defaultSelect2Options, select2Options);
    }

    select2.select2(defaultSelect2Options)
        .on("select2:selectById", (event, id) => {
        let data = {
            id: id,
            page: 1,
            totalPorPagina:10
        }
        $.ajax({
            url: ajax.url,
            dataType: ajax.dataType,
            delay: ajax.delay,
            method: ajax.method,
            data: data
        }).then((res) => {
            if (res.result && res.result.items) {
                let item = _.find(res.result.items,(x)=> x.id == id);
                if(item) {
                    $(`<option></option`).val(item.id).text(item.text).attr('selected', 'selected').appendTo($(classe));
                }
                select2.trigger("change");
            }
        });
    })
        .on("select2:selectByIds", (event, ...id) => {
        let dataResult = { filtros: JSON.stringify(id) };
        $.ajax({
            url: ajax.url,
            dataType: ajax.dataType,
            delay: ajax.delay,
            method: ajax.method,
            data: dataResult
        }).then((res) => {
            if (res.result && res.result.items) {
                if (!_.isArray(id)) {
                    var item = res.result.items[0];
                    //$(classe).select2('data', item);
                    $(`<option></option`).val(item.id).text(item.text).attr('selected', 'selected').appendTo($(classe));
                }
                else {
                    _.each(res.result.items, (item) => {
                        if (_.find(id, (containItem) => containItem.id == item.id)) {
                            $(`<option></option`).val(item.id).text(item.text).attr('selected', 'selected').appendTo($(classe));
                        }
                    })
                }

                select2.trigger("change");
            }
        });
    });
    
    if(select2.data("value")) {
        select2.trigger("select2:selectById", select2.data("value"));
    }

}


function selectSWMultiple(classe, url, elementoFiltro) {



    $(classe).css('width', '100%');
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };



    function filtrar() {



        if (elementoFiltro) {
            var retorno = null;
            if (elementoFiltro.valor != undefined) {
                retorno = elementoFiltro.valor;
            } else if ((elementoFiltro != undefined) && (elementoFiltro != null) && (elementoFiltro != 0) && (elementoFiltro != '0')) {
                if (elementoFiltro.val()) {
                    retorno = elementoFiltro.val();
                } else {
                    retorno = null;// elementoFiltro;
                }
            } else if (elementoFiltro.val()) {
                retorno = elementoFiltro.val();
            }
            return retorno;
        }
        else {
            return null;
        }
    }

    $(classe).select2({
        allowClear: true,
        multiple: true,
        placeholder: app.localize("SelecioneLista"),
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            method: 'Post',

            data: function (params) {
                //   //console.log('data: ', params, (params.page == undefined));
                if (params.page == undefined)
                    params.page = '1';
                //   //console.log('data: ', params);

                return {
                    search: params.term,
                    page: params.page,
                    totalPorPagina: 10,
                    filtro: filtrar()
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                return {
                    results: data.result.items,
                    pagination: {
                        more: (params.page * 10) < data.result.totalCount
                    }
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 0
    });

}

function select2MestreFor(mestreId, dependenteClasse, dependenteServicoMetodo) {
    $("#" + mestreId).on("change", function () {
        var mestreId = $(this).val();

        ////console.log('meestreId: ' + mestreId);

        var classeDep = '.' + dependenteClasse;
        var path = '/api/services/app/' + dependenteServicoMetodo;

        ////console.log('classeDependente: ' + classeDep);
        ////console.log('path: ' + path);
        ////console.log('mestreId: ' + mestreId);

        selectSW(classeDep, path, mestreId);
    });
}

// Select2 exclusivo para ContasMedicasPlano
function selectSWPlano(classe, url, elementoFiltro) {
    $(classe).css('width', '100%');
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    function filtrar() {
        if (elementoFiltro) {
            var retorno = null;
            if (elementoFiltro.valor != undefined) {
                retorno = elementoFiltro.valor;
            } else if ((elementoFiltro != undefined) && (elementoFiltro != null) && (elementoFiltro != 0) && (elementoFiltro != '0')) {
                if (elementoFiltro.val()) {
                    retorno = elementoFiltro.val();
                } else {
                    retorno = null;
                }
            } else if (elementoFiltro.val()) {
                retorno = elementoFiltro.val();
            }
            return retorno;
        }
        else {
            return null;
        }
    }

    $(classe).select2({
        allowClear: true,
        placeholder: app.localize("SelecioneLista"),
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            method: 'Post',

            data: function (params) {
                //   //console.log('data: ', params, (params.page == undefined));
                if (params.page == undefined)
                    params.page = '1';
                //   //console.log('data: ', params);

                return {
                    search: params.term,
                    page: params.page,
                    totalPorPagina: 10,
                    filtro: filtrar()
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                return {
                    results: data.result.items,
                    pagination: {
                        more: (params.page * 10) < data.result.totalCount
                    }
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 0
    });

}

function select2MestreForPlano(mestreId, dependenteClasse, dependenteServicoMetodo) {
    $("#" + mestreId).on("change", function () {
        var mestreId = $(this).val();

        ////console.log('meestreId: ' + mestreId);

        var classeDep = '.' + dependenteClasse;
        var path = '/api/services/app/' + dependenteServicoMetodo;

        ////console.log('classeDependente: ' + classeDep);
        ////console.log('path: ' + path);
        ////console.log('mestreId: ' + mestreId);

        selectSWPlano(classeDep, path, mestreId);
    });
}
// Fim - select2 exclusivo

function aplicarSelect2Padrao() {
    $('.select2').each(function () {
        var url = '/api/services/app/' + $(this)
            .attr('name');
        url = url
            .replace('1', '')
            .replace('2', '')
            .replace('3', '')
            .replace('4', '');
        //+ '/listarDropdown';
        var url1 = url.substr(0, url.length - 2) + '/listarDropdown';
        var that = $(this);
        that.select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: url1,
                dataType: 'json',
                delay: 1000,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0,
        });
    });
    $('.select2').css('width', '100%');
}


function applicarCamposComCapitalize() {
    $(".capitalcase").on('input',
        function () {
            $(this).val(app.capitalize($(this).val()));
        });
}

function selectSWMultiplosFiltros(classe, url, filtros) {
    $(classe).css('width', '100%');
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };

    function filtrar() {
        var filtroValores = [];
        for (i = 0; i < filtros.length; i++) {
            var campo = (filtros[i].valor != undefined) ? filtros[i].valor : filtroCampo(filtros[i]);
            filtroValores.push(campo);
        }
        return filtroValores;
    }

    function filtroCampo(campo) {
        if (campo.startsWith("#") || campo.startsWith(".")) {
            return $(campo).val();
        }
        return $("#" + campo).val();
    }


    $(classe).select2({
        allowClear: true,
        placeholder: app.localize("SelecioneLista"),
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            method: 'Post',

            data: function (params) {
                if (params.page == undefined)
                    params.page = '1';

                return {
                    search: params.term,
                    page: params.page,
                    totalPorPagina: 10,
                    filtros: filtrar()
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                return {
                    results: data.result.items,
                    pagination: {
                        more: (params.page * 10) < data.result.totalCount
                    }
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 0
    });

}

function setActivePage(pageName) {

    sessionStorage["ActivePage"] = pageName;
    $.ajaxSetup({ cache: false, async: false });

    $.ajax({
        url: '/mpa/operacoes/DefinirOperacaoAtual?name=' + pageName,
        dataType: 'json',
        success: function (data) {
            if (data && data != null) {
                sessionStorage["OperacaoId"] = data.Id;
            }
        },
    });


}

function formatarValor(valor) {

    if (valor != '' && valor != null) {
        var numero = parseFloat(valor).toFixed(2).split('.');
        numero[0] = numero[0].split(/(?=(?:...)*$)/).join('.');
        return numero.join(',');

    }
    return '';

}

function formatarValor4(valor) {

    if (valor != '' && valor != null) {
        var numero = parseFloat(valor).toFixed(4).split('.');
        numero[0] = numero[0].split(/(?=(?:...)*$)/).join('.');
        return numero.join(',');

    }
    return '';

}

function RetirarMascaraPadrao(valor) {
    if (valor) {
        regexp = /[^\w]/g;
        valor = valor.replace(regexp, "");
    }
    return valor;
}

function retirarMascara(valor) {
    while (valor.indexOf('.') != -1) valor = valor.replace('.', '');
    while (valor.indexOf(' ') != -1) valor = valor.replace(' ', '');
    valor = valor.replace(',', '.');
    return valor;
}

// Print function (require the reportviewer client ID)
function printReport(report_ID) {
    var rv1 = $('#' + report_ID);
    var iDoc = rv1.parents('html');
    // Reading the report styles
    var styles = iDoc.find("head style[id$='ReportControl_styles']").html();
    if ((styles == undefined) || (styles == '')) {
        iDoc.find('head script').each(function () {
            var cnt = $(this).html();
            var p1 = cnt.indexOf('ReportStyles":"');
            if (p1 > 0) {
                p1 += 15;
                var p2 = cnt.indexOf('"', p1);
                styles = cnt.substr(p1, p2 - p1);
            }
        });
    }
    if (styles == '') { alert("Cannot generate styles, Displaying without styles.."); }
    styles = '<style type="text/css">' + styles + "</style>";

    // Reading the report html
    var table = rv1.find("div[id$='_oReportDiv']");
    if (table == undefined) {
        alert("Report source not found.");
        return;
    }

    // Generating a copy of the report in a new window
    var docType = '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/loose.dtd">';
    var docCnt = styles + table.parent().html();
    var docHead = '<head><title>Printing ...</title><style>body{margin:5;padding:0;}</style></head>';
    var winAttr = "location=yes, statusbar=no, directories=no, menubar=no, titlebar=no, toolbar=no, dependent=no, width=720, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
    var newWin = window.open("", "_blank", winAttr);
    writeDoc = newWin.document;
    writeDoc.open();
    writeDoc.write(docType + '<html>' + docHead + '<body onload="window.print();">' + docCnt + '</body></html>');
    writeDoc.close();

    // The print event will fire as soon as the window loads
    newWin.focus();
    // uncomment to autoclose the preview window when printing is confirmed or canceled.
    // newWin.close();
};

function addPrintButton(ctl) {
    var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/App/Reserved.ReportViewerWebControl.axd?OpType=Resource&Version=10.0.40219.1&Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
    var innerTable = '<table title="Print" onclick="javascript:PrintFunc(\'' + ctl + '\'); return false;" id="do_print" style="cursor: default;">' + innerTbody + '</table>'
    var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';
    $("#ctl00_cphMain_rvReportMain_ctl05 > div").append(outerDiv);
}

function preventWhiteSpace(str) {
    var notWhitespaceTestRegex = /[^\s]{1,}/;
    return String(str).search(notWhitespaceTestRegex) != -1;
}

function removerAcentos(str) {
    if (str != null && str != undefined) {
        var result = str.toUpperCase();
        result = result.replace('Á', 'A');
        result = result.replace('Â', 'A');
        result = result.replace('Ã', 'A');
        result = result.replace('À', 'A');
        result = result.replace('Ä', 'A');
        result = result.replace('É', 'E');
        result = result.replace('È', 'E');
        result = result.replace('Ê', 'E');
        result = result.replace('Ë', 'E');
        result = result.replace('Í', 'I');
        result = result.replace('Ì', 'I');
        result = result.replace('Î', 'I');
        result = result.replace('Ï', 'I');
        result = result.replace('Ó', 'O');
        result = result.replace('Ò', 'O');
        result = result.replace('Ô', 'O');
        result = result.replace('Õ', 'O');
        result = result.replace('Ö', 'O');
        result = result.replace('Ú', 'U');
        result = result.replace('Ù', 'U');
        result = result.replace('Û', 'U');
        result = result.replace('Ü', 'U');
        result = result.replace('Ç', 'C');
        result = result.replace('Ñ', 'N');
        return result;
    }
    else {
        return str;
    }
}

function minimizarMenu() {
    $('body').addClass('page-sidebar-closed');
    $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
}

function definirHorarios(intervalo, horaIni) {
    debugger;
    let str = '';
    data = new Date();
    let aHoraIni = horaIni.split(':');
    let i = parseInt(aHoraIni[0]);

    let loops = 24 / parseInt(intervalo);
    if (loops != Infinity) {
        while (loops > 0) {
            var item = zeroEsquerda(i, 2);
            if(_.isNaN(item) || item == "NaN") {
                item = "00";
            }
            if (str.indexOf(item + ':00') == -1) {
                str += item + ':00' + ' ';
            }
            i += parseInt(intervalo);
            loops--;
            if (i >= 24) {
                i = (i - 24);
            }
        }
    } 
    const len = str.length;
    str = str.substring(0, len - 1);
    return str;
}

function selecionarRegistroSelect2(campo, id, descricao) {
    $('#' + campo).append($("<option>").val(id)
        .text(descricao)
    )
        .val(id)
        .trigger("change");
}

function alertSw(titulo, mensagem, icone) {
    swal({
        title: titulo,
        text: mensagem,
        type: icone
    });
}

window.alert = function (titulo, mensagem, icone) {
    swal({
        title: titulo,
        text: mensagem,
        type: icone
    });
}

function create_UUID() {
    var dt = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (dt + Math.random() * 16) % 16 | 0;
        dt = Math.floor(dt / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
}

function errorHandler(errors, title, type = "error") {
    title = title ?? 'Erro ao dar confirmar a solicitação';
    
    
    let content = $(`<div class="container">
                            <div class="row">
                                <div class="col-md-3"><strong>Codigo:</strong></div>
                                <div class="col-md-9"><strong>Mensagem</strong></div>
                            </div>
                        </div>`);
    _.forEach(errors,(item) => {
        let erroItem = `<div class="row"> 
                                <div class="col-md-3"><h3 style="margin: 0;font-weight: bold;">${item.codigoErro || ''}</h3></div>
                                <div class="col-md-9">${item.descricao}</div>
                            </div>`;
        content.append($(erroItem));
    } );
    let uuid = create_UUID();
    let modal = $(`
            <div class=" custom-error-modal ${uuid} modal fade"  tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
              <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header" style="min-height: auto;padding: 15px 10px;margin: 0 !important;">
                        <button type="button" style="margin-right: 5px;margin-top: 10px !important;font-size: 14px !important;color: #F80E3F;" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title"> ${title} </h4>
                    </div>
                  <div class="modal-body">
                    ${content.html()}
                  </div>
                </div>
              </div>
            </div>`);
    $("body").append(modal);
    $(`.${uuid}`).modal().on('hidden.bs.modal', function (e) {
        $(`.custom-error-modal.${uuid}`).remove();
    });
}

function createSingleDatePicker(content) {
    
    const defaultProperties = {
        "singleDatePicker": true,
        "showDropdowns": true,
        //autoUpdateInput: false,
        //maxDate: new Date(),
        changeYear: true,
        //yearRange: 'c-10:c+10',
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
    };
    let query;
    if(content !== null && content !== undefined) {
        query = content.find('.date-picker');
    } else {
        query = $('.date-picker')
    }
    const formats = ["DD/MM/YYYY HH:mm:ss", "YYYY/DD/MM HH:mm:ss" ];
    query.each(function (index) {
        let obj = $(this);
        let extendProperties = {};
        if((obj).hasClass("time-picker")) {
            extendProperties.timePicker = true;
            extendProperties.locale = {
                format :moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY HH:mm:ss" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY HH:mm:ss" : "YYYY-MM-DD HH:mm:ss"
            }
        }
        obj.daterangepicker(_.extend({},defaultProperties, extendProperties),
            function (start, end, label) {
                let momentObj;
                if(start === undefined || start == null) {
                    obj.val(null);
                    obj.trigger('input');
                    obj.trigger('change');
                    obj.trigger("apply.daterangepicker");
                    return;
                }
                
                formats.every((formatValue) => {
                    debugger;
                    momentObj =  moment(start,formatValue,true);
                    if(momentObj.isValid()) {
                        obj.val(momentObj.format('DD/MM/YYYY HH:mm:ss'));
                        obj.trigger('input');
                        obj.trigger('change');
                        obj.trigger("apply.daterangepicker");
                        return false;
                    }
                })
                if(momentObj && !momentObj.isValid()) {
                    obj.val(momentObj.format('DD/MM/YYYY HH:mm:ss'));
                    obj.trigger('input');
                    obj.trigger('change');
                    obj.trigger("apply.daterangepicker");
                }
                return true;
            });
    });
}


function createFaqHelper(urlPath, element) {
    if (urlPath == null || urlPath == undefined || urlPath == "") {
        return;
    }

    var _faqHelperModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/FaqHelper',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Apoios/FaqHelper/faqHelper.js',
        modalClass: 'faqHelperModal',
        modalId: `faqHelperModal`,
        focusFunction: (_$modal) => { }
    });

    $(`<button class="btn open-faq" style="height: 100%;"><i class="fa-2x far fa-question-circle" style="height: 100%;padding-top: 12px;color: #3598dc;"></i></button>`).appendTo(element);
    $(element).find(".open-faq").on("click", function () {
        debugger;
        _faqHelperModal.open({ urlPath });
    });
}

$(document).ready(function () {
    $(".faq-helper").each(function () {
        createFaqHelper($(this).data("url"), this);
    })
})