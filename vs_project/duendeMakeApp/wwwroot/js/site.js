// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.addEventListener("DOMContentLoaded", function () {
    var checkboxes = document.querySelectorAll(".btn-group input[type='checkbox']");

checkboxes.forEach(function (checkbox) {
    checkbox.addEventListener("change", function () {
        if (this.checked) {
            this.closest('label').classList.add('active');
        } else {
            this.closest('label').classList.remove('active');
        }
    });
    });
});

//para buscar sobre un div
$(document).ready(function () {
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myDIV #content").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});


// Para buscar sobre una tabla
$(document).ready(function () {
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});
