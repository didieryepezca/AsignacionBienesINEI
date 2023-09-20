$(document).ready(function () {
    const pagebtns = document.getElementsByClassName("btn");
    for (var i = 0; i < pagebtns.length; i++) {
        pagebtns[i].addEventListener('click', function () {
            document.getElementById("overlay-spinner").removeAttribute("style");
        });
    }

    $('#tblMttos').DataTable();
});