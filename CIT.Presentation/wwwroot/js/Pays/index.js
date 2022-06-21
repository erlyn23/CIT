getUserPages('pagosLink');
getUserPermissions(5);

const payHeaders = {
    ...appHeaders,
    'Page': 'Pagos'
};

const getPays = function () {
    document.getElementById('date').valueAsDate = new Date();
    doRequest({
        url: '/Pay/GetPayments',
        method: 'GET',
        data: null,
        headers: { ...payHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) { onGetPays(data); },
        errorCallback: function (err) { onErrorHandler(err) }
    });
}

const onGetPays = function (data) {
    if (!hasAdd) {
        $("#openCreatePay").remove();
        $("#CreatePayModal").remove();
    }
    if (hasGet) {
        $("#paginator-container").pagination({
            dataSource: data,
            pageSize: 5,
            callback: function (data, pagination) {
                const html = templatePaysList(data);
                $("#paysList").html(html);
                setEditPayEvent(data);
                $("#loadingPays").css({ 'display': 'none' });
                $("#paysTable").removeClass('d-none');
            }
        });
    }
}

const templatePaysList = (pays) => {

    const tBody = $("#paysList");
    tBody.html("");
    let html = "";
    pays.forEach(pay => {
        const payDate = new Date(pay.date);
        const payDateView = `${payDate.getDate()}/${payDate.getMonth() + 1}/${payDate.getFullYear()}`;

        html += `<tr>
                    <td>${pay.id}</td>
                    <td>${payDateView}</td>
                    <td>${pay.pay}</td>
                    <td>
                        <button type="button" id="setPayDetailBtn-${pay.id}" class="btn btn-sm btn-success"><i class="fas fa-eye"></i></button>
                        <button type="button" id="editPayBtn-${pay.id}" class="btn btn-sm btn-warning"><i class="fas fa-edit"></i></button>
                        <button type="button" id="deletPaynBtn-${pay.id}" class="btn btn-sm btn-danger" onclick="deletePay(${pay.id})"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>`;
    });
    return html;
}

const setPayData = function (pay) {
    $("#createPayTitle").text("Editar pago");

    $("#payId").val(pay.id);
    $("#loanId").val(pay.loanId);

    $("#loanNameFilter").val(pay.loan.loanName);
    $("#loanNameFilter").attr('disabled', true);

    $("#userId").val(pay.userId);
    $("#userId").attr('disabled', true);

    document.getElementById("date").valueAsDate = new Date(pay.date);
    $("#pay").val(pay.pay);

    $("#CreatePayModal").modal('show');
}

const setPayDetailData = function (pay) {
    const payDate = new Date(pay.date);
    const payDateView = `${payDate.getDate()}/${payDate.getMonth() + 1}/${payDate.getFullYear()}`;

    $("#payIdView").text(pay.id);
    $("#loanIdView").text(pay.loan.id);
    $("#loanNameView").text(pay.loan.loanName);
    $("#dateView").text(payDateView);
    $("#payView").text(pay.pay);


    $("#PayModalDetail").modal('show');
}

const setEditPayEvent = function (payments) {
    payments.forEach(function (payment) {

        if (hasEdit) {
            $("#editPayBtn-" + payment.id).on('click', function () {
                setPayData(payment);
            });
        } else
            $("#editPayBtn-" + payment.id).remove();

        if (hasGet) {

            $("#setPayDetailBtn-" + payment.id).on('click', function () {
                setPayDetailData(payment);
            });

        } else $("#setPayDetailBtn-" + payment.id).remove();

        if (!hasDelete)
            $("#deletePayBtn-" + payment.id).remove();

    });
}

$("#savePayBtn").on('click', function () {
    if (!validateOnClick()) {
        $("#savePaymentBtn > span").text("Enviando datos, por favor espere...");
        $("#savePaymentBtn").prop('disabled', true);
        const newPay = {
            loanId: formFields.loanId.val(),
            userId: formFields.userId.val(),
            date: formFields.date.val(),
            pay: formFields.pay.val(),
        };

        if ($("#payId").val() !== "")
            newPay.id = $("#payId").val();

        let url = ($("#payId").val() !== "") ? '/Pay/UpdatePayment' : '/Pay/AddPayment';
        let operation = ($("#payId").val() !== "") ? 'Modificar' : 'Agregar';
        doRequest({
            url: url,
            method: 'POST',
            data: newPay,
            headers: { ...payHeaders, 'Operation': operation },
            successCallback: function (data) {
                onSuccessSavePayment(data);
            },
            errorCallback: function (err) {
                onError(err);
            }
        });
    }
});


const onSuccessSavePayment = function (data) {
    $("#savePayBtn > span").text("Guardar pago");
    $("#savePayBtn").prop('disabled', false);
    if (Array.isArray(data)) {
        $("#errorMessages").html("");
        for (let error of data) {
            let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${error}</p>`;
            $("#errorMessages").append(errorMsg);
            $("#CreatePayModal").modal('hide');

            setTimeout(function () {
                $("#ErrorMessagesModal").modal('show');
            }, 1000);
        }
    } else {
        $("form").eq(0).trigger('reset');
        for (let field in formFields) {
            formFields[field].removeClass('is-valid');
        }
        $("#errorMessages").html("");
        let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Pago guardado correctamente</p>`;
        $("#errorMessages").append(successMsg);
        $("#CreatePayModal").modal('hide');

        setTimeout(function () {
            $("#ErrorMessagesModal").modal('show');
        }, 1000);
        getPays();
    }
}

const onError = function (err) {
    $("#savePayBtn > span").text("Guardar pago");
    $("#savePayBtn").prop('disabled', false);
    $("#errorMessages").html("");
    let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>${err.responseText}</p>`;
    $("#errorMessages").append(errorMsg);
    $("#CreatePayModal").modal('hide');

    setTimeout(function () {
        $("#ErrorMessagesModal").modal('show');
    }, 1000);
}

const onErrorHandler = function (err) {
    $("#loadingPays").addClass("d-none");
    $("#errorMessage").removeClass("d-none");
    $("#errorMessage").html("<p>Ha ocurrido un error al obtener los pagos: " + err.responseText + "</p>")
}

$("#closePayFormBtn").on('click', function () {
    $("form").eq(0).trigger('reset');
});

getPays();