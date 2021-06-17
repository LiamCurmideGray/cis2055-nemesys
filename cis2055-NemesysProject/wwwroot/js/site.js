// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function filterStatuses(statusType) {
    var x = document.getElementsByClassName("card " + statusType);
    var y = document.getElementsByClassName("card ");

    if (statusType != "all") {
        for (var i = 0; i < y.length; i++) {
            y[i].style.display = "none";
        }

        for (var i = 0; i < x.length; i++) {
            x[i].style.display = "block";
        }
    } else {
        for (var i = 0; i < y.length; i++) {
            y[i].style.display = "block";
        }
    }

   
}
