
$(document).ready(function () {
    $("#ddlArea").change(function () {
        $("#ddlDepartamento").empty();
        $.ajax({
            type: 'POST',
            url: '/ProductoController/GetDepartamento',
            dataType: 'json',
            data: { IdArea: $("#ddlArea").val() },
            success: function (departamentos) {
                $("#ddlDepartamento").append('<option value="0">' + '--Seleccione un departamento--' + '</option>');
                $.each(departamentos, function (i, departamentos) {
                    $("#ddlDepartamento").append('<option value="'
                        + departamentos.IdArea + '">'
                        + departamentos.Nombre + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed.' + ex);
            }
        });
    });
});

function changeDepartamento() {
    $("#ddlDepartamento").empty();
    $.ajax({
        type: 'POST',
        url: '/Producto/GetDepartamento',
        dataType: 'json',
        data: { IdArea: $("#ddlArea").val() },
        success: function (departamentos) {
            $("#ddlDepartamento").append('<option value="0">' + '--Seleccione un departamento--' + '</option>');
            $.each(departamentos, function (i, departamentos) {
                $("#ddlDepartamento").append('<option value="'
                    + departamentos.idDepartamento + '">'
                    + departamentos.nombre + '</option>');
            });
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
}