$(document).ready(function () {
    renderProductos();
});

function renderProductos() {
    $("#productos").empty();
    $.ajax({
        type: 'GET',
        url: '/Home/GetProductosIndex',
        dataType: 'json',
        //data: { IdArea: $("#ddlArea").val() },
        success: function (productos) {

            $.each(productos, function (i, producto) {
                var id = producto.id;
                var nombre = producto.nombre;
                var imagen = producto.imagen;
                var precio = producto.precio;
                var descripcion = producto.descripcion;
                var cardTemplate = `
                        <div class="col-md-3">
                            <div class="cardProduct" id="${id}" onclick="ProductoGet(this.id)">
                                <div class="cardProductHead">
                                    <img src="data:image;base64,${imagen}" class="img-fluid rounded-start" alt="${nombre}" >
                                </div>
                                <div class="cardProductBody">
                                    <div class="nombreProducto"> 
                                        <p>${nombre}</p>
                                    </div>
                                    <div class="precioProducto">
                                        <p>$ ${precio}.00</p>
                                    </div>
                                </div>
                            </div>
                       </div>
                    `;
                $("#productos").append(cardTemplate);
            });
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
}
function ProductoGet(Id) {
    window.location.href = `/Home/ProductoGet?Id=${Id}`;
}
function OpenCar(UserName) {
    window.location.href = `/CarritoCompra/Index?UserName=${UserName}`;
}
function OpenBag(UserName) {
    window.location.href = `/PedidoUser/Index?UserName=${UserName}`;
}