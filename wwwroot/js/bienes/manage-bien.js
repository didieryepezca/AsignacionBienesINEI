$("#manage-bien-form").submit(function () {
    document.getElementById("overlay-spinner").removeAttribute("style");
});

if (result !== "") {
    if (result.substring(0, 5) == "ERROR") {
        Swal.fire('Error', result, 'error')
    }
}