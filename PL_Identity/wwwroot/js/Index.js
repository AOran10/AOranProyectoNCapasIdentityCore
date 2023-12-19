$(document).ready(function () {
    renderProductos();
});

function renderProductos() {
    $("#productos").empty(); 
    var IdArea = document.getElementById("ddlArea").value;
    var IdDepartamento = document.getElementById("ddlDepartamento").value;
    var Nombre = document.getElementById("Busqueda").value;

    var settings = {
        "url": '/Home/GetProductosIndex',
        "method": "POST",
        data: { IdArea, IdDepartamento, Nombre },
    };
    $.ajax(settings).done(function (result) {
        $.each(result, function (i, producto) {
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
    }).fail(function (xhr, status, error) {
        alert('Error en la actualizacion.' + error);

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

function changeDepartamento() {
    $("#ddlDepartamento").empty();
    $.ajax({
        type: 'POST',
        url: '/Home/GetDepartamento',
        dataType: 'json',
        data: { IdArea: $("#ddlArea").val() },
        success: function (departamentos) {
            $("#ddlDepartamento").append('<option value="0">' + '--Seleccione un departamento--' + '</option>');
            $.each(departamentos, function (i, departamentos) {
                $("#ddlDepartamento").append('<option value="'
                    + departamentos.idDepartamento + '">'
                    + departamentos.nombre + '</option>');
            });
            renderProductos();
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
}