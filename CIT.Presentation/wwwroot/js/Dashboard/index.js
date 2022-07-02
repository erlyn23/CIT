getUserPages('dashboardLink');
getUserPermissions(1);


const CHART_LINE_COLOR = '#FAC827';

const getDashboardData = function () {
    doRequest({
        url: '/Dashboard/GetDashboardData',
        method: 'GET',
        data: null,
        headers: { ...dashboardHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) {
            onGetData(data);
        },
        errorCallback: function (error) {
            onError(error);
        }
    })
}

const onError = function (error) {
    $("#loadingDashboard").addClass('d-none');
    $("#errorMessage").removeClass('d-none');
    $("#errorMessage").text(error.responseText);
}

const onGetData = function (data) {
    $("#loadingDashboard").addClass('d-none');
    const loanLabels = data.loanQuantityPerDay.map(function (loan) {
        return mapLabel(loan);
    });

    const paymentLabels = data.paymentQuantityPerDay.map(function (payment) {
        return mapLabel(payment);
    });


    let userLabels = [];
    let userData = [];

    if (data.userQuantityPerDay.length > 1) {
        userLabels = data.userQuantityPerDay.map(function (user) {
            return mapLabel(user);
        });

        userData = data.userQuantityPerDay.map(function (user) {
            return mapData(user);
        });

        $("#userTotalCountContainer").removeClass('d-none');
        const userChartData = doChartObject(userLabels, userData, 'Cantidad de usuarios registrados por día');
        const usersChart = new Chart(document.getElementById('usersChart'), userChartData);
    }

    let assignmentsLabels = [];
    let assignmentData = [];

    if (data.vehicleAssignmentsQuantityPerDay.length > 1) {

        assignmentsLabels = data.vehicleAssignmentsQuantityPerDay.map(function (assignment) {
            return mapLabel(assignment);
        });

        assignmentData = data.vehicleAssignmentsQuantityPerDay.map(function (assignment) {
            return mapData(assignment);
        });

        $("#assignmentTotalCountContainer").removeClass('d-none');
        const assignmentChartData = doChartObject(assignmentsLabels, assignmentData, 'Cantidad de asignaciones de vehículos hechas por día');
        const assignmentsChart = new Chart(document.getElementById('assignmentsChart'), assignmentChartData);
    }

    const loanData = data.loanQuantityPerDay.map(function (loan) {
        return mapData(loan);
    });

    let paymentData = data.paymentQuantityPerDay.map(function (payment) {
        return mapData(payment);
    });

    

    
    const loanChartData = doChartObject(loanLabels, loanData, 'Cantidad de préstamos hechos por día');
    const paymentChartData = doChartObject(paymentLabels, paymentData, 'Cantidad de pagos hechos por día');
    
    const loansChart = new Chart(document.getElementById('loansChart'), loanChartData);
    const paymentsChart = new Chart(document.getElementById('paymentsChart'), paymentChartData);

    $("#totalUsers").text(data.totalUsersQuantity);
    $("#totalLoans").text(data.totalLoansQuantity);
    $("#totalPayments").text(data.totalPaymentsQuantity);
    $("#totalAssignments").text(data.totalVehicleAssignmentsQuantity);
}

const mapLabel = function (perDay) {
    const formattedDate = new Date(perDay.day);

    return `Día ${formattedDate.getDate()}`;
}

const mapData = function (perDay) {
    return perDay.count;
}

const doChartObject = function (labels, data, title) {
    return {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: title,
                data: data,
                fill: false,
                borderColor: CHART_LINE_COLOR,
                tension: 0.1
            }]
        }
    }
}

getDashboardData();

const getProfilePhoto = function () {
    doRequest({
        url: `/Dashboard/GetProfilePhoto`,
        method: 'GET',
        headers: appHeaders,
        data: null,
        successCallback: function (data) {
            $("#profileNavPhoto").attr('src', data.path);
        },
        errorCallback: function (error) {
            if (error.statusCode === 404)
                $("#profileNavPhoto").attr('src', 'https://flyclipart.com/thumb2/circle-user-png-icon-free-download-133446.png');
        }
    });
}

getProfilePhoto();