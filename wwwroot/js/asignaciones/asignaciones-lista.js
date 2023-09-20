$(document).ready(function () {
    const pagebtns = document.getElementsByClassName("btn");
    for (var i = 0; i < pagebtns.length; i++) {
        pagebtns[i].addEventListener('click', function () {
            document.getElementById("overlay-spinner").removeAttribute("style");
        });
    }   
});

if (result !== "") {
    if (result.substring(0, 5) == "ERROR") {
        Swal.fire('Error', result, 'error')
    }
}

function DownloadAsignacionPDF(idAsignacion) {   
    var w = window.open(PdfAsignacionUrl + "?idAsignacion=" + idAsignacion)
    w.onbeforeunload = function () {
        document.getElementById('overlay-spinner').style.display = 'none';
    }    
}


let idAsigToPDF = 0;
function LoadModalTitleAndCode(idAsignacion)
{
    idAsigToPDF = idAsignacion;
    $('#modal-upload-pdf-title').text("Subir ficha de asignación firmada: A-"+idAsignacion)
}

function UploadAsignacionPDF()
{
    var fileUpload = $("#pdfInput").get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    console.log("-----------------");
    $.ajax({
        type: "POST",
        url: "/Asignacion/UploadAsignacionPDF?idAsignacion="+idAsigToPDF,
        contentType: false,
        processData: false,
        data: data,
        success: function (jres) {
            //console.log(jres);
            if (jres.msg == "Archivo subido con exito.") {              
                $('#pdfInput').text("Ningun archivo seleccionado");
                Swal.fire('¡ Cargado !', jres.msg, 'success').then((result) => {
                    location.reload();
                })                
            } else
            {
                $('#pdfInput').text("Ningun archivo seleccionado");
                Swal.fire('Error', jres.msg, 'error')                
            } 
            document.getElementById('overlay-spinner').style.display = 'none';
            ///-------------
        },
    });
    return false;
}

function DownloadUploadedAsignacionFirmadaPDF(idAsignacion)
{
    var w = window.open(PdfAsignacionFirmadaUrl + "?idAsignacion=" + idAsignacion)
    w.onbeforeunload = function () {
        document.getElementById('overlay-spinner').style.display = 'none';
    }   
}

function ReleaseBien(idAsignacion, idBien)
{
    $.ajax({
        type: "POST",
        url: "/Asignacion/ReleaseBienFromAsignacion?idAsignacion=" + idAsignacion + "&idBien=" + idBien,
        contentType: false,
        processData: false,        
        success: function (jres) {
            //console.log(jres);
            if (jres.msg == "Se liberó el bien correctamente") {                
                Swal.fire('¡ Exito !', jres.msg, 'success').then((result) => {
                    location.reload();
                })
            } else {                
                Swal.fire('Error', jres.msg, 'error')
            }
            document.getElementById('overlay-spinner').style.display = 'none';
            ///-------------
        },
    });
}              