function consultarNotas(username, password) {
    var settings = {
        "async": true,
        "crossDomain": true,
        "data": {
            "cnpj": "02746015000129",
            "grupo": "AMERICAN",
            "campos": "",
            "filtro": "dtemissao>=" + $('#filtro-data-inicio').val() + "&& dtemissao<=" + $('#filtro-data-fim').val()
        },
        "url": "https://managersaas.tecnospeed.com.br:8081/ManagerAPIWeb/nfe/consulta",
        "method": "GET",
        "headers": {
            "authorization": btoa(username + ":" + password),
            "cache-control": "no-cache"
        }
    }

    $.ajax(settings)
        .done(function (response) {
            //console.log(response);
        });
}