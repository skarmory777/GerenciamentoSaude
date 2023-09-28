//foto

var constraints = { audio: false, video: { width: 640, height: 480 } };
var video = document.querySelector("#vid");
var canvas = document.querySelector('#canvas');
var ctx = canvas.getContext('2d');
var localMediaStream = null;


// Older browsers might not implement mediaDevices at all, so we set an empty object first
if (navigator.mediaDevices === undefined) {
    navigator.mediaDevices = {};
}


// Some browsers partially implement mediaDevices. We can't just assign an object
// with getUserMedia as it would overwrite existing properties.
// Here, we will just add the getUserMedia property if it's missing.
if (navigator.mediaDevices.getUserMedia === undefined) {
    navigator.mediaDevices.getUserMedia = function (constraints) {

        // First get ahold of the legacy getUserMedia, if present
        var getUserMedia = (navigator.getUserMedia ||
          navigator.webkitGetUserMedia ||
          navigator.mozGetUserMedia);

        // Some browsers just don't implement it - return a rejected promise with an error
        // to keep a consistent interface
        if (!getUserMedia) {
            return Promise.reject(new Error('getUserMedia is not implemented in this browser'));
        }

        // Otherwise, wrap the call to the old navigator.getUserMedia with a Promise
        return new Promise(function (resolve, reject) {
            getUserMedia.call(navigator, constraints, resolve, reject);
        });
    }
}

navigator.mediaDevices.getUserMedia({ video: { width: 640, height: 480 } })
.then(function (stream) {
    video = document.querySelector('#vid');
    // Older browsers may not have srcObject
    video.src = window.URL.createObjectURL(stream);
    localMediaStream = stream;
    video.onloadedmetadata = function (e) {
        video.play();
    };
})
.catch(function (err) {
    abp.notify.info(err.name + ": " + err.message);
    //console.log(err.name + ": " + err.message);
});


var onCameraFail = function (e) {
    //console.log('Não funcionou ://// .', e);
};

function snapshot() {
    if (localMediaStream) {
        ctx.drawImage(video, 0, 0);
        var dados = {};
        var base64 = canvas.toDataURL("image/jpeg");
        dados.base64 = base64.substr(base64.indexOf(',') + 1, base64.length);
        var type = base64.substr(base64.indexOf(':') + 1, base64.indexOf('jpeg') - 1);
        $('#foto-blob').val(dados.base64);
        $('#foto-mime-type').val(type);
        $('#foto-medico').attr({
            'src': 'data:' + type + ';base64,' + dados.base64
        });
        localMediaStream.getTracks()[0].stop();
        video.src = "";
        $('#capturar-foto').html(app.localize('CapturarFoto'));
        $('#area-captura').html('').addClass('hidden');
    }
}
//navigator.getUserMedia = navigator.getUserMedia
//|| navigator.webkitGetUserMedia
////|| navigator.mozGetUserMedia
//|| navigator.mediaDevices.getUserMedia
//|| navigator.msGetUserMedia;
//window.URL = window.URL || window.webkitURL;
//navigator.getUserMedia({
//    video: true
//}, function (stream) {
//    video.src = window.URL.createObjectURL(stream);
//    localMediaStream = stream;
//}, onCameraFail);

$('#btn-snapshot').click(function (e) {
    e.preventDefault();
    snapshot();
});