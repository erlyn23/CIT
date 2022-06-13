getUserPages('pagosLink');
getUserPermissions(5);

const loanHeaders = {
    ...appHeaders,
    'Page': 'Pagos'
};

const getLoans = function () {
    doRequest({
        url: '/Pay/GetPayments',
        method: 'GET',
        data: null,
        headers: { ...loanHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) { onGetLoans(data); },
        errorCallback: function (err) { onErrorHandler(err) }
    });
}

const onGetLoans = function (data) {
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
                setEditLoanEvent(data);
                $("#loadingPays").css({ 'display': 'none' });
                $("#loansTable").removeClass('d-none');
            }
        });
    }
}

const templatePaysList = (pays) => {

    const tBody = $("#paysList");
    tBody.html("");
    let html = "";
    loans.forEach(pay => {
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
    $("#loanId").attr('disabled', true);

    $("#loanName").val(pay.loan.loanName);
    $("#loanName").attr('disabled', true);

    $("#userId").val(pay.userId);
    $("#userId").attr('disabled', true);

    document.getElementById("date").valueAsDate = new Date(pay.date);
    $("#pay").val(pay.pay);

    $("#CreateLoanModal").modal('show');
}

const setPayDetailData = function (loan) {
    //const startDate = new Date(loan.startDate);
    //const startDateView = `${startDate.getDate()}/${startDate.getMonth() + 1}/${startDate.getFullYear()}`;

    //const endDate = new Date(loan.endDate);
    //const endDateView = `${endDate.getDate()}/${endDate.getMonth() + 1}/${endDate.getFullYear()}`;


    //$("#loanIdView").text(loan.id);
    //$("#duesQuantityView").text(loan.duesQuantity);
    //$("#totalLoanView").text(loan.totalLoan);
    //$("#startDateView").text(startDateView);
    //$("#endDateView").text(endDateView);
    //$("#payDayView").text(loan.payDay);
    //$(`#interestRateView`).text(`${loan.interestRate * 100}%`);
    //$("#mensualPayView").text(loan.mensualPay);


    $("#LoanModalDetail").modal('show');
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

$("#savePaymentBtn").on('click', function () {
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
        let operation = ($("#loanId").val() !== "") ? 'Modificar' : 'Agregar';
        doRequest({
            url: url,
            method: 'POST',
            data: newLoan,
            headers: { ...loanHeaders, 'Operation': operation },
            successCallback: function (data) {
                onSuccessSaveLoan(data);
            },
            errorCallback: function (err) {
                onError(err);
            }
        });
    }
});


const onSuccessSaveLoan = function (data) {
    $("#saveLoanBtn > span").text("Guardar préstamo");
    $("#saveLoanBtn").prop('disabled', false);
    if (Array.isArray(data)) {
        $("#errorMessages").html("");
        for (let error of data) {
            let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${error}</p>`;
            $("#errorMessages").append(errorMsg);
            $("#CreateLoanModal").modal('hide');

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
        let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Préstamo guardado correctamente</p>`;
        $("#errorMessages").append(successMsg);
        $("#CreateLoanModal").modal('hide');

        setTimeout(function () {
            $("#ErrorMessagesModal").modal('show');
        }, 1000);
        getLoans();
    }
}

const onError = function (err) {
    $("#saveLoanBtn > span").text("Guardar préstamo");
    $("#saveLoanBtn").prop('disabled', false);
    $("#errorMessages").html("");
    let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>${err.responseText}</p>`;
    $("#errorMessages").append(errorMsg);
    $("#CreateLoanModal").modal('hide');

    setTimeout(function () {
        $("#ErrorMessagesModal").modal('show');
    }, 1000);
}

const onErrorHandler = function (err) {
    $("#loadingLoans").addClass("d-none");
    $("#errorMessage").removeClass("d-none");
    $("#errorMessage").html("<p>Ha ocurrido un error al obtener préstamos: " + err.responseText + "</p>")
}

$("#closeLoanFormBtn").on('click', function () {
    $("form").eq(0).trigger('reset');
});

getLoans();