function FormularioConfig() {
    var self = this;
    //$('#myModal').modal({ show: false });

    self.editType = ko.observable(celular(0, 0));
    self.formName = ko.observable("Form_1");
    self.formConfig = ko.observableArray([]);
    self.typeOption = ko.observableArray([]);
    //self.typeOption = ko.observableArray(['text', 'email', 'checkbox', 'datetime-local', 'radio', 'select']);

    self.editarCelula = function (celula) {
        //console.log(celula.Col1);
        //$('#editModal').modal('toggle');
    }

    self.addMultiOption = function (celula) {
        //console.log(celula.MultiOption);
        if (celula.MultiOption) {
            celula.MultiOption.push(ko.mapping.fromJS({ Opcao: "Nova Opção" }));
        }
    }

    for (var i = 0; i < 4; i++) {
        self.formConfig.push(ko.mapping.fromJS(criarLinha(i)));
    }
    
    self.typeOption.push(formType.text);
    self.typeOption.push(formType.select);
    self.typeOption.push(formType.radio);
    self.typeOption.push(formType.email);
    self.typeOption.push(formType.checkbox);
    self.typeOption.push(formType.datetime);
}


function criarLinha(linha, colspan = false) {
    if (colspan) {
        return {
            "Col1": new celular(linha, 0, colspan)
        }
    } else {
        return {
            "Col1": new celular(linha, 0),
            "Col2": new celular(linha, 1)
        }
    }
}

/*
Cria uma celula com as propriedades do campo por padrão o campo é Texto
@param linha Linha da celula
*/
function celular(linha, coluna, colspan = false, readonly = false) {
    return {
        "Id": "col_id_" + linha + "_" + coluna,
        "Name": "col_name_" + linha + "_" + coluna,
        "Label": "Campo " + linha + "_" + coluna,
        "Placeholder": "col_placeholder_" + linha + "_" + coluna,
        "Value": null,
        "Type": formType.text,
        "Colspan": colspan,
        "Readonly": readonly,
        "MultiOption": [{ Opcao: "Nova Opção" }]
    }
}

var formType = {
    "text": "text",
    "email": "email",
    "checkbox": "checkbox",
    "datetime": "datetime-local",
    "radio": "radio",
    "select": "select"
}