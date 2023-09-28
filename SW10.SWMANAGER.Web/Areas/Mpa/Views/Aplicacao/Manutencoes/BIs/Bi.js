$('#exibir-bi-' + $('#bi-id').val()).on('click', function (e) {
    e.preventDefault();
    //console.log($('#frame-relatorio-despesas').attr('src'));
    if ($('#frame-bi-' + $('#bi-id').val()).attr('src') == undefined) {
        $('#bi-' + $('#bi-id').val()).removeClass('hidden');
        $('#frame-bi-' + $('#bi-id').val()).attr('src', $('#bi-url').val());
        if ($('#aba-dashboard').length > 0) {
            $('#aba-dashboard').trigger('click');

        }
    }
    else {
        $('#bi-' + $('#bi-id').val()).addClass('hidden');
        $('#frame-bi-' + $('#bi-id').val()).attr('src', null);
    }
    if ($('#show-bi-' + $('#bi-id').val()).hasClass('expand')) {
        $('#show-bi-' + $('#bi-id').val()).trigger('click');
    }
});
