$(document).ready(function () {
    const pagelinks = document.getElementsByClassName("page-link");
    for (var i = 0; i < pagelinks.length; i++) {
        pagelinks[i].addEventListener('click', function () {
            document.getElementById("overlay-spinner").removeAttribute("style");
        });
    }

    const tblopts = document.getElementsByClassName("btn-blue");
    for (var i = 0; i < tblopts.length; i++) {
        tblopts[i].addEventListener('click', function () {
            document.getElementById("overlay-spinner").removeAttribute("style");
        });
    }

    const newbienbtn = document.querySelector(".btn-primary");
    newbienbtn.addEventListener("click", event => {
        document.getElementById("overlay-spinner").removeAttribute("style");
    });
});

function SearchBienes() {
    document.getElementById("overlay-spinner").removeAttribute("style");
    document.searchBienes.submit();
}

if (result !== "") {
    if (result.substring(0, 5) == "ERROR") {
        Swal.fire('Error', result, 'error')
    }
}