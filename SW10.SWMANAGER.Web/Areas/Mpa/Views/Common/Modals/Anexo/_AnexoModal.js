var formdata = new FormData();
var progress_bar_id = "#progress-wrp";

$(document).ready(function () {
    setInitialParamsFormData();
    $(progress_bar_id).hide();

    $("#btnupload").click(function () {
        $(progress_bar_id).show();
        $.ajax({
            url: abp.appPath + 'Mpa/Anexo/UploadFiles',
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: formdata,
            async: true,
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                
                // Upload progress
                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = (evt.loaded / evt.total) * 100;
                        progressHandling(percentComplete);                        
                    }
                }, false);

                // Download progress
                xhr.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = (evt.loaded / evt.total) * 100;
                        progressHandling(percentComplete);                        
                    }
                }, false);

                return xhr;
            },
            success: function (result) {
                if (result != "") {
                    alert(result?.result);
                    $("#anexoModalId").modal('hide');
                }
            },
            error: function (err) {
                alert(err.statusText);
                $("#anexoModalId").modal('hide');
            }
        });
    });

    chkatchtbl();
});

$("#fileInput").on("change", function () {
    var fileInput = document.getElementById('fileInput');
    //Iterating through each files selected in fileInput  
    for (i = 0; i < fileInput.files.length; i++) {

        var sfilename = fileInput.files[i].name;
        let srandomid = Math.random().toString(36).substring(7);

        formdata.append(sfilename, fileInput.files[i]);

        var markup = "<tr id='" + srandomid + "'>" +
            "<td>" + sfilename + "</td><td><a href='#' onclick='DeleteFile(\"" + srandomid + "\",\"" + sfilename +
            "\")'><span class='glyphicon glyphicon-remove red'></span></a></td></tr>";

        $("#FilesList tbody").append(markup);
    }
    chkatchtbl();
    $('#fileInput').val('');
});


function DeleteFile(Fileid, FileName, objectKey) {

    if (objectKey == null)
        return removerAnexoView(Fileid, FileName);

    $.ajax({
        url: abp.appPath + 'Mpa/Anexo/DeleteFile',
        type: "POST",
        processData: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ objectKey: objectKey }),
        async: false,
        success: function (result) {
            if (result != "") {
                removerAnexoView(Fileid, FileName);
                alert(result?.result);
            }
        },
        error: function (err) {
            alert(err.statusText);
        }
    });        
}

function chkatchtbl() {
    if ($('#FilesList tr').length > 1) {
        $("#FilesList").css("visibility", "visible");
    } else {
        $("#FilesList").css("visibility", "hidden");
    }
}

function removerAnexoView(Fileid, FileName) {
    formdata.delete(FileName);
    $("#" + Fileid).remove();
    chkatchtbl();
}

function setInitialParamsFormData() {
    if ($("#anexoListaId").val() != null)
        formdata.append("anexoListaId", $("#anexoListaId").val());

    if ($("#origemAnexoId").val() != null)
        formdata.append("origemAnexoId", $("#origemAnexoId").val());

    if ($("#origemAnexoTabela").val() != null)
        formdata.append("origemAnexoTabela", $("#origemAnexoTabela").val());
}

var progressHandling = function (percent) {
    $(progress_bar_id + " .progress-bar").css("width", +percent + "%");
    $(progress_bar_id + " .status").text(percent + "%");
};

