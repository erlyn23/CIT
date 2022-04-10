getUserPages('vehiculosLink');
getUserPermissions(7);
let allVehicles = [];

const vehicleHeaders = {
    ...appHeaders,
    'Page': 'Vehiculos'
};

$(document).ready(function () {
    if ($("#filterField").val() === null)
        $("#filterValue").attr('disabled', true);

    $("#filterField").html(`
<option value="" disabled selected>Selecciona una opción...</option>
 <option value="id">Id</option>
 <option value="brand">Marca</option>
 <option value="model">Modelo</option>
 <option value="year">Año</option>
 <option value="enrollment">Matrícula</option>
 <option value="licensePlate">Placa</option>`);
});

$("#filterField").on('change', function () {
    if ($("#filterField").val() === null)
        $("#filterValue").attr('disabled', true);
    else {
        $("#filterValue").removeAttr('disabled', false);
    }
});

$("#filterValue").on('keyup', function (e) {
    let filteredVehicles = [];
    if (e.target.value.length > 0) {
        filteredVehicles = allVehicles.filter(v => v[$("#filterField").val()]?.toString().toLowerCase().includes(e.target.value.toLowerCase()));
        setPagination(filteredVehicles, true);
    } else
        setPagination(allVehicles, false);

});

const getVehicles = function () {
    doRequest({
        url: '/Vehicle/GetVehicles',
        method: 'GET',
        data: null,
        headers: { ...vehicleHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) { onGetVehicles(data, false); },
        errorCallback: function (err) { onErrorHandler(err) }
    });
}

const onGetVehicles = function (data, isSearch) {
    if (!hasAdd) {
        $("#openCreateVehicle").remove();
        $("#CreteVehicleModal").remove();
    }
    if (hasGet) {
        setPagination(data, isSearch);
    }
}

const setPagination = (data, isSearch) => {
    if (!isSearch)
        allVehicles = data;
    $("#paginator-container").pagination({
        dataSource: data,
        pageSize: 5,
        callback: function (data, pagination) {
            const html = templateVehiclesList(data);
            $("#vehiclesList").html(html);
            setEditVehicleEvent(data);
            $("#loadingVehicles").css({ 'display': 'none' });
            $("#vehiclesTable").removeClass('d-none');
        }
    });
}

const templateVehiclesList = (vehicles) => {

    const tBody = $("#vehiclesList");
    tBody.html("");
    let html = "";

    if (vehicles.length === 0)
        html = "<p><i>No hay resultados para mostrar :(</i></p>";
    else {
        vehicles.forEach(vehicle => {
            html += `<tr>
                    <td>${vehicle.id}</td>
                    <td>${vehicle.brand}</td>
                    <td>${vehicle.model}</td>
                    <td>
                        <button type="button" id="setVehicleDetailBtn-${vehicle.id}" class="btn btn-sm btn-success"><i class="fas fa-eye"></i></button>
                        <button type="button" id="editVehicleBtn-${vehicle.id}" class="btn btn-sm btn-warning"><i class="fas fa-edit"></i></button>
                        <button type="button" id="deleteVehicleBtn-${vehicle.id}" class="btn btn-sm btn-danger" onclick="deleteVehicle(${vehicle.id})"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>`;
        });
    }
    return html;
}

const setVehicleData = function (vehicle) {
    $("#createVehicleTitle").text("Editar vehículo");

    $("#vehicleId").val(vehicle.id);

    $("#brand").val(vehicle.brand);
    $("#model").val(vehicle.model);
    $("#enrollment").val(vehicle.enrollment);
    $("#licensePlate").val(vehicle.licensePlate);
    $("#color").val(vehicle.color);
    $("#year").val(vehicle.year);

    $("#CreateVehicleModal").modal('show');
}

const setVehicleDetailData = function (vehicle) {
    $("#vehicleIdView").text(vehicle.id);
    $("#brandView").text(vehicle.brand);
    $("#modelView").text(vehicle.model);
    $("#enrollmentView").text(vehicle.enrollment);
    $("#licensePlateView").text(vehicle.licensePlate);
    $("#colorView").text(vehicle.color);
    $("#yearView").text(vehicle.year);

    $("#VehicleModalDetail").modal('show');
}

const setEditVehicleEvent = function (vehicles) {
    vehicles.forEach(function (vehicle) {

        if (hasEdit) {
            $("#editVehicleBtn-" + vehicle.id).on('click', function () {
                setVehicleData(vehicle);
            });
        } else
            $("#editVehicleBtn-" + vehicle.id).remove();

        if (hasGet) {

            $("#setVehicleDetailBtn-" + vehicle.id).on('click', function () {
                setVehicleDetailData(vehicle);
            });

        } else $("#setVehicleDetailBtn-" + vehicle.id).remove();

        if (!hasDelete)
            $("#deleteVehicleBtn-" + vehicle.id).remove();

    });
}

$("#saveVehicleBtn").on('click', function () {
    if (!validateOnClick()) {
        $("#saveVehicleBtn > span").text("Enviando datos, por favor espere...");
        $("#saveVehicleBtn").prop('disabled', true);
        const newVehicle = {
            brand: formFields.brand.val(),
            model: formFields.model.val(),
            enrollment: formFields.enrollment.val(),
            licensePlate: formFields.licensePlate.val(),
            color: formFields.color.val(),
            year: formFields.year.val()
        };

        if ($("#vehicleId").val() !== "") {
            newVehicle.id = $("#vehicleId").val();
        }

        let url = ($("#vehicleId").val() !== "") ? '/Vehicle/UpdateVehicle' : '/Vehicle/SaveVehicle';
        let operation = ($("#vehicleId").val() !== "") ? 'Modificar' : 'Agregar';
        doRequest({
            url: url,
            method: 'POST',
            data: newVehicle,
            headers: { ...vehicleHeaders, 'Operation': operation },
            successCallback: function (data) {
                onSuccessSaveVehicle(data);
            },
            errorCallback: function (err) {
                onError(err);
            }
        });
    }
});


const onSuccessSaveVehicle = function (data) {
    $("#saveVehicleBtn > span").text("Guardar vehículo");
    $("#saveVehicleBtn").prop('disabled', false);
    if (Array.isArray(data)) {
        $("#errorMessages").html("");
        for (let error of data) {
            let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${error}</p>`;
            $("#errorMessages").append(errorMsg);
            $("#CreateUserModal").modal('hide');

            setTimeout(function () {
                $("#ErrorMessagesModal").modal('show');
            }, 1000);
        }
    } else {
        $("form").eq(0).trigger('reset');
        for (let field in formFields) {
            formFields[field].removeClass('is-valid');
        }

        $("#CreateVehicleModal").modal('hide');
        $("#errorMessages").html("");
        let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Vehículo guardado correctamente</p>`;
        $("#errorMessages").append(successMsg);

        setTimeout(function () {
            $("#ErrorMessagesModal").modal('show');
        }, 1000);
        getVehicles();
    }
}

const onError = function (err) {

    $("#CreateVehicleModal").modal('hide');
    $("#saveVehicleBtn > span").text("Guardar vehículo");
    $("#saveVehicleBtn").prop('disabled', false);
    $("#errorMessages").html("");
    let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>${err.responseText}</p>`;
    $("#errorMessages").append(errorMsg);

    setTimeout(function () {
        $("#ErrorMessagesModal").modal('show');
    }, 1000);
}

const deleteVehicle = function (vehicleId) {
    $("#DeleteConfirmVehicleModal").modal('show');

    $("#deleteVehicleBtn").on('click', function () {
        doRequest(
            {
                url: '/Vehicle/DeleteVehicle/' + vehicleId,
                method: 'GET',
                data: null,
                headers: { ...vehicleHeaders, 'Operation': 'Eliminar' },
                successCallback: function (data) {
                    $("#DeleteConfirmVehicleModal").modal('hide');
                    alert(data);
                    getVehicles();
                },
                errorCallback: function (error) { onErrorHandler(error) }
            }
        );
    });
}


const onErrorHandler = function (err) {
    $("#loadingVehicles").addClass("d-none");
    $("#errorMessage").removeClass("d-none");
    $("#errorMessage").html("<p>Ha ocurrido un error al obtener vehículos: " + err.responseText + "</p>")
}


$("#closeVehicleFormBtn").on('click', function () {
    $("form").eq(0).trigger('reset');
    for (let field in formFields) {
        formFields[field].removeClass('is-valid');
        formFields[field].removeClass('is-invalid');

        validationsStatus[field] = false;
    }
    $("#uploadedPhoto").removeAttr("src");
    $("#createVehicleTitle").text('Crear nuevo vehículo');
});
getVehicles();