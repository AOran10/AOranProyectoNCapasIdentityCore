$(document).ready(function () {
    Total();
});

function Total() {
    var Subtotales = document.querySelectorAll('.subtotal a');
    var Total = 0;

    Subtotales.forEach(function (subtotal) {
        dato = parseFloat(subtotal.textContent  );
        Total = Total + dato;
    })
    var contenedor = document.getElementById("contenedorTotal");
    var totalTemplate = `<h2>Total de: ${Total} pesos.</h2>`;
    $("#contenedorTotal").append(totalTemplate)
}