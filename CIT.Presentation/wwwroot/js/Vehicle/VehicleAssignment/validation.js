const assignmentFormFields = {
    user: $("#usersSelect"),
    vehicle: $("#vehiclesSelect"),
    comment: $("#comment")
};

const assignmentValidationsStatus = {
    user: false,
    vehicle: false,
    comment: false
};

//Validación Usuario
const validateUser = function () {
    if (assignmentFormFields.user.val().length === 0) {
        setAssignmentInvalid(assignmentFormFields.user, "#userValidation", "<p>Debes seleccionar un usuario</p>");
        assignmentValidationsStatus.user = true;
    } else {
        setAssignmentValid(assignmentFormFields.user, "#userValidation");
        assignmentValidationsStatus.user = false;
    }
}

assignmentFormFields.user.on(CHANGE_EVENT, validateUser);
assignmentFormFields.user.on(FOCUSOUT_EVENT, validateUser);
//Validación Usuario


//Validación Vehículo
const validateVehicle = function () {
    if (assignmentFormFields.vehicle.val().length === 0) {
        setAssignmentInvalid(assignmentFormFields.vehicle, "#vehicleValidation", "<p>Debes seleccionar un vehículo</p>");
        assignmentValidationsStatus.vehicle = true;
    } else {
        setAssignmentValid(assignmentFormFields.vehicle, "#vehicleValidation");
        assignmentValidationsStatus.vehicle = false;
    }
}

assignmentFormFields.vehicle.on(CHANGE_EVENT, validateVehicle);
assignmentFormFields.vehicle.on(FOCUSOUT_EVENT, validateVehicle);
//Validación Vehículo


//Validación Comentario
const validateComment = function () {
    if (assignmentFormFields.comment.val().length === 0) {
        setAssignmentInvalid(assignmentFormFields.comment, "#commentValidation", "<p>Debes escribir un comentario</p>");
        assignmentValidationsStatus.comment = true;
    } else if (assignmentFormFields.comment.val().length > 150) {
        setAssignmentInvalid(assignmentFormFields.comment, "#commentValidation", "<p>Solo debes escribir 150 carácteres</p>");
        assignmentValidationsStatus.comment = true;
    } else {
        setAssignmentValid(assignmentFormFields.comment, "#commentValidation");
        assignmentValidationsStatus.comment = false;
    }
}

assignmentFormFields.comment.on(CHANGE_EVENT, validateComment);
assignmentFormFields.comment.on(FOCUSOUT_EVENT, validateComment);
//Validación Comentario

const setAssignmentInvalid = function (element, validationId, validationHtml) {
    element.removeClass('is-valid');
    element.addClass('is-invalid');
    $(validationId).html(validationHtml);
    $("#saveAssignmentBtn").prop('disabled', true);
}

const setAssignmentValid = function (element, validationId) {
    element.removeClass('is-invalid');
    element.addClass('is-valid');
    $(validationId).html("");
    $("#saveAssignmentBtn").prop('disabled', false);
}

const validateAssignmentOnClick = function () {
    validateUser();
    validateVehicle();
    validateComment();

    for (let field in assignmentValidationsStatus) {
        if (assignmentValidationsStatus[field]) {
            $("#finalValidation").html(`<p><i class="fas fa-exclamation"></i> Tienes errores de validación, por favor, completa bien los campos</p>`);
            $("#saveVehicleBtn").prop('disabled', true);
        } else {
            $("#finalValidation").html("");
            $("#saveVehicleBtn").prop('disabled', false);
        }
    }

    for (let field in assignmentValidationsStatus) {
        if (assignmentValidationsStatus[field]) 
            return true;
        else 
            return false;
    }

}