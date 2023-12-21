$(document).ready(function () {
    renderProductos();
});

function renderProductos() {
    $("#productos").empty(); 
    var area = document.getElementById("ddlArea").value;
    var depa = document.getElementById("ddlDepartamento").value;
    var consultaAbierta = document.getElementById("Busqueda").value;

    var IdArea = area != "" ? parseInt(area): 0;
    var IdDepartamento = depa != "" ? parseInt(depa) : 0;

    var consulta = {
        "consultaAbierta": consultaAbierta,
        "idArea": IdArea,
        "idDepartamento": IdDepartamento
    }
    
    var settings = {
        type: 'POST',
        url: 'http://localhost:5286/api/Producto/getall',
        dataType: 'json',
        data: JSON.stringify(consulta),
        contentType: "application/json; charset=uft-8",
    };
    $.ajax(settings).done(function (result) {
        $.each(result.objects, function (i, producto) {
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
        success: function (result) {
            departamentos = result.objects;
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