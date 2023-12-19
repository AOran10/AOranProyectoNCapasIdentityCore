function changeEstatus(IdPedido) {
    var Estatuses = document.getElementById(IdPedido)
    var IdEstatus = Estatuses.value;
    var settings = {
        "url": '/PedidoAdmin/UpdateStatus',
        "method": "POST",
        data: { IdPedido, IdEstatus },
    };
    $.ajax(settings).done(function (result) {
        console.log(result);
        alert(result.errorMessage);
    }).fail(function (xhr, status, error) {
        alert('Error en la actualizacion.' + error);

    });
}