
function agregarAlCarrito(Id) {
    var Nombre = document.getElementsByName("NombreUsuario");
    var Cantidad = document.getElementById("cantidad");
    var PrecioProducto = document.getElementById("PrecioProducto");
    var IdProducto = Id;
    //window.location.href = `/CarritoCompra/AddToCar?UserName=${Nombre[0].id},${Cantidad.value},${IdProducto}`;
    window.location.href = `/CarritoCompra/AddToCar?Orden=${Nombre[0].id},${Cantidad.value},${IdProducto},${PrecioProducto.text}`;
}

function sumarValor() {
    document.getElementById('cantidad').stepUp();
}

function restarValor() {
    var input = document.getElementById('cantidad');
    if (parseInt(input.value) > 1) {
        input.stepDown();
    }
}