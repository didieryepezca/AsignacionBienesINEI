$(document).ready(function () {
    $('#tblResponsables').DataTable();  
});    

$("#asignacion-form").submit(function () {
    document.getElementById("overlay-spinner").removeAttribute("style");
});

//$("#btnPickResponsable").click(function (e) {
//    //$('#form').submit();
//    console.log("Entro al Click del PartialView")
//    $('#modal-responsables').load('/Asignacion/GetResponsablesModal');
//});

if (result !== "") {
    console.log(result);
    if (result.substring(0, 5) == "ERROR") {

        console.log(result.substring(0, 5))
        Swal.fire('Error', result, 'error')
    } else
    {
        Swal.fire('Exito', result, 'success')
        DownloadAsignacionPDF(PdfAsignacionUrl);
    }
}

function DownloadAsignacionPDF(downloadUrl) {
    console.log("Entro");
    window.open(downloadUrl)
}

function SetResponsable(idResp, nombres, dni, odei, username)
{
    $('#txtUserId').val(idResp);
    $('#txtOdei').val(odei);
    $('#txtDNI').val(dni);
    $('#txtUsuarioNombres').val(nombres);
    $('#txtUserName').val(username);
    //console.log("Responsable seleccionado: " + idRresp);
    loadAsignacionesPorUsuario(idResp)
}

function loadAsignacionesPorUsuario(idResp)
{
    $.ajax({
        url: ReporteAsignacionesPorUsuarioUrl + '?userId=' + idResp,
        type: "GET",
        dataType: "html",
        success: function (data) {
            //console.log(data)       
            $('#partialTblBodyRptAsignacionesUsuario').html(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Swal.fire('Error', 'Se ha producido un error Agregando el bien, HttpRequest: ' + XMLHttpRequest + ' textStatus: ' + textStatus + ' errorThrown:' + errorThrown, 'error')
        }
    });
}

$(function ()
{
    $("#cps").autocomplete({
        source: function (req, res) {          
            $.ajax({                
                url: searchBienesAutoCompleteUrl + '&controlPatrimonial=' + $("#cps").val(),
                type: "GET",
                dataType: "json",
                success: function (data) {
                    //console.log(data)
                    res($.map(data, function (item) {
                        return {
                            label: item.label,
                            mykey: item.id,
                            value: item.id
                        };
                    }));
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {                    
                    Swal.fire('Error', 'Se ha producido un error buscando los bienes, HttpRequest: ' + XMLHttpRequest + ' textStatus: ' + textStatus + ' errorThrown:' + errorThrown, 'error')                    
                }
            });
        },
        minLength: 1,
        delay: 500,
        select: function (event, ui) {              
            AddEquipoToAsignacionPartialView(ui.item.value)            
            $("#cps").val("");
        }
    });
})

function AddEquipoToAsignacionPartialView(idBien)
{     
    $.ajax({
        url: AddBienToNuevaAsignacionUrl + '?bienId=' + idBien,
        type: "GET",
        dataType: "html",
        success: function (data) {
            //console.log(data)       
            $('#partialTblBodyAsignacionesAgregadas').html(data);              
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Swal.fire('Error', 'Se ha producido un error Agregando el bien, HttpRequest: ' + XMLHttpRequest + ' textStatus: ' + textStatus + ' errorThrown:' + errorThrown, 'error')                     
        }
    });
}

function RemoveEquipoFromAsignacionPartialView(idBien)
{
    $.ajax({
        url: RemoveBienFromNuevaAsignacionUrl + '?bienId=' + idBien,
        type: "GET",
        dataType: "html",
        success: function (data) {
            //console.log(data)
            $('#partialTblBodyAsignacionesAgregadas').html(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Swal.fire('Error', 'Se ha producido un error quitando el bien, HttpRequest: ' + XMLHttpRequest + ' textStatus: ' + textStatus + ' errorThrown:' + errorThrown, 'error')
        }
    });
}