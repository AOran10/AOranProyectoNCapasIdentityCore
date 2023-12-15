function changeEstatus(IdPedido) {
    var Estatuses = document.getElementById(IdPedido)
    var IdEstatus = Estatuses.value;
    //$.ajax({
    //    type: 'POST',
    //    dataType: 'json',
    //    url: '/PedidoAdmin/UpdateStatus',
    //    data: { IdPedido, IdEstatus },
    //    success: {

    //    },
    //    error: function (ex) {
    //        alert('Failed.' + ex);
    //    }
    //})
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