const getModalData = function () {
    doRequest({
        url: '/Users/GetUsers',
        method: 'GET',
        data: null,
        headers: { ...appHeaders, 'Page': 'Usuarios', 'Operation': 'Obtener' },
        successCallback: function (data) {
            let htmlUser = "";
            if ($("#assignmentId").val() === "" || $("#assignmentId").val() === null)
                htmlUser = `<option value="0" disabled selected>Selecciona un usuario</option>`;

            data.forEach(user => {
                htmlUser += `<option value="${user.id}">${user.id} - ${user.name} ${user.lastName}</option>`;
            });

            $("#usersSelect").html(htmlUser);
        },
        errorCallback: function (error) {

        }
    });

    doRequest({
        url: '/Vehicle/GetVehicles',
        method: 'GET',
        data: null,
        headers: { ...appHeaders, 'Page': 'Vehiculos', 'Operation': 'Obtener' },
        successCallback: function (data) {
            let htmlUser = "";
            if ($("#assignmentId").val() === "" || $("#assignmentId").val() === null)
                htmlUser = `<option value="0" disabled selected>Selecciona un vehículo</option>`;

            data.forEach(vehicle => {
                htmlUser += `<option value="${vehicle.id}">${vehicle.id} - ${vehicle.brand} ${vehicle.model}</option>`;
            });

            $("#vehiclesSelect").html(htmlUser);
        },
        errorCallback: function (error) {

        }
    });
}


const getAssignments = function () {
    doRequest({
        url: '/VehicleAssignment/GetVehicleAssignments',
        method: 'GET',
        data: null,
        headers: { ...vehicleHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) { onGetVehicleAssignments(data); },
        errorCallback: function (err) { onAssignmentErrorHandler(err) }
    });
}

const onGetVehicleAssignments = function (data) {
    if (!hasAdd) {
        $("#openCreateAssignment").remove();
        $("#CreateAssignmentModal").remove();
    }
    if (hasGet) {
        $("#paginator-container-2").pagination({
            dataSource: data,
            pageSize: 5,
            callback: function (data, pagination) {
                const html = templateAssignmentsList(data);
                $("#assignmentsList").html(html);
                setAssignmentEditEvent(data);
                $("#loadingAssignments").css({ 'display': 'none' });
                $("#assignmentsTable").removeClass('d-none');
            }
        });
    }
}

const templateAssignmentsList = (assingments) => {

    const tBody = $("#assignmentsList");
    tBody.html("");
    let html = "";
    assingments.forEach(assignment => {
        html += `<tr>
                    <td>${assignment.id}</td>
                    <td>${assignment.user.name} ${assignment.user.lastName}</td>
                    <td>${assignment.vehicle.brand} ${assignment.vehicle.model}</td>
                    <td>${assignment.assignmentDate}</td>
                    <td>
                        <button type="button" id="setAssignmentDetailBtn-${assignment.id}" class="btn btn-sm btn-success"><i class="fas fa-eye"></i></button>
                        <button type="button" id="editAssignmentBtn-${assignment.id}" class="btn btn-sm btn-warning"><i class="fas fa-edit"></i></button>
                        <button type="button" id="deleteAssignmentBtn-${assignment.id}" class="btn btn-sm btn-danger" onclick="deleteAssignment(${assignment.id})"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>`;
    });
    return html;
}

const setAssignmentData = function (assignment) {

    getModalData();
    $("#createAssignmentTitle").text("Editar asignación");

    $("#assignmentId").val(assignment.id);

    $("#user").val(assignment.user.id);
    $("#vehicle").val(assignment.vehicle.id);
    $("#comment").val(assignment.comment);

    $("#CreateAssignmentModal").modal('show');
}

const setAssignmentDetailData = function (assignment) {
    $("#assignmentIdView").text(assignment.id);
    $("#userView").text(`${assignment.user.id} - ${assignment.user.name} ${assignment.user.lastName}`);
    $("#vehicleView").text(`${assignment.vehicle.id} - ${assignment.vehicle.brand} ${assignment.vehicle.model}`);
    $("#dateView").text(assignment.assignmentDate);
    $("#commentView").text(assignment.comment);

    $("#AssignmentDetailModal").modal('show');
}

const setAssignmentEditEvent = function (assignments) {
    assignments.forEach(function (assignment) {

        if (hasEdit) {
            $("#editAssignmentBtn-" + assignment.id).on('click', function () {
                setAssignmentData(assignment);
            });
        } else
            $("#editAssignmentBtn-" + assignment.id).remove();

        if (hasGet) {

            $("#setAssignmentDetailBtn-" + assignment.id).on('click', function () {
                setAssignmentDetailData(assignment);
            });

        } else $("#setAssignmentDetailBtn-" + assignment.id).remove();

        if (!hasDelete)
            $("#deleteAssignmentBtn-" + assignment.id).remove();

    });
}

$("#saveAssignmentBtn").on('click', function () {
    if (!validateAssignmentOnClick()) {
        $("#saveAssignmentBtn > span").text("Enviando datos, por favor espere...");
        $("#saveAssignmentBtn").prop('disabled', true);
        const newAssignment = {
            userId: assignmentFormFields.user.val(),
            vehicleId: assignmentFormFields.vehicle.val(),
            comment: assignmentFormFields.comment.val()
        };

        if ($("#assignmentId").val() !== "") {
            newAssignment.id = $("#assignmentId").val();
        }

        let url = ($("#assignmentId").val() !== "") ? '/VehicleAssignment/UpdateAssignment' : '/VehicleAssignment/AssignVehicle';
        let operation = ($("#assignmentId").val() !== "") ? 'Modificar' : 'Agregar';
        doRequest({
            url: url,
            method: 'POST',
            data: newAssignment,
            headers: { ...vehicleHeaders, 'Operation': operation },
            successCallback: function (data) {
                onSuccessSaveAssignment(data);
                getAssignments();
            },
            errorCallback: function (err) {
                onAssignmentError(err);
            }
        });
    }
});


const onSuccessSaveAssignment = function (data) {
    $("#saveAssignmentBtn > span").text("Guardar asignación");
    $("#saveAssignmentBtn").prop('disabled', false);
    if (Array.isArray(data)) {
        $("#errorMessages").html("");
        for (let error of data) {
            let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${error}</p>`;
            $("#errorMessages").append(errorMsg);
            $("#CreateAssignmentModal").modal('hide');

            setTimeout(function () {
                $("#ErrorMessagesModal").modal('show');
            }, 1000);
        }
    } else {
        $("form").eq(0).trigger('reset');
        for (let field in formFields) {
            formFields[field].removeClass('is-valid');
        }

        $("#CreateAssignmentModal").modal('hide');
        $("#errorMessages").html("");
        let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Asignación hecha correctamente</p>`;
        $("#errorMessages").append(successMsg);

        setTimeout(function () {
            $("#ErrorMessagesModal").modal('show');
        }, 1000);
        getVehicles();
    }
}

const onAssignmentError = function (err) {

    $("#CreateAssignmentModal").modal('hide');
    $("#saveAssignmentBtn > span").text("Guardar asignación");
    $("#saveAssignmentBtn").prop('disabled', false);
    $("#errorMessages").html("");
    let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>${err.responseText}</p>`;
    $("#errorMessages").append(errorMsg);

    setTimeout(function () {
        $("#ErrorMessagesModal").modal('show');
    }, 1000);
}

const deleteAssignment = function (assignmentId) {
    $("#DeleteConfirmAssignmentModal").modal('show');

    $("#deleteAssignmentBtn").on('click', function () {
        doRequest(
            {
                url: '/VehicleAssignment/DeleteAssignment/' + assignmentId,
                method: 'GET',
                data: null,
                headers: { ...vehicleHeaders, 'Operation': 'Eliminar' },
                successCallback: function (data) {
                    $("#DeleteConfirmAssignmentModal").modal('hide');
                    alert(data);
                    getAssignments();
                },
                errorCallback: function (error) { onAssignmentErrorHandler(error) }
            }
        );
    });
}


const onAssignmentErrorHandler = function (err) {
    $("#loadingAssignments").addClass("d-none");
    $("#errorMessage").removeClass("d-none");
    $("#errorMessage").html("<p>Ha ocurrido un error al obtener asignaciones: " + err.responseText + "</p>")
}


$("#closeAssignmentForm").on('click', function () {
    $("form").eq(1).trigger('reset');
});

$("#openCreateAssignment").on('click', function () {
    getModalData();
});
getAssignments();