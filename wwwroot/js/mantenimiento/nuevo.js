if (result !== "") {
    console.log(result);
    if (result.substring(0, 5) == "ERROR") {

        console.log(result.substring(0, 5))
        Swal.fire('Error', result, 'error')
    } else {
        Swal.fire('Exito', result, 'success')        
    }
}

$(function () {
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
            AddEquipoToMantenimientoPartialView(ui.item.value)
            $("#cps").val("");
        }
    });
})



function AddEquipoToMantenimientoPartialView(idBien) {
    $.ajax({
        url: AddBienToMantenimientoUrl + '?bienId=' + idBien,
        type: "GET",
        dataType: "html",
        success: function (data) {
            //console.log(data)       
            $('#partialTblBodyMattoAgregado').html(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Swal.fire('Error', 'Se ha producido un error Agregando el bien, HttpRequest: ' + XMLHttpRequest + ' textStatus: ' + textStatus + ' errorThrown:' + errorThrown, 'error')
        }
    });
}

$("#mantenimiento-form").submit(function () {
    document.getElementById("overlay-spinner").removeAttribute("style");
});