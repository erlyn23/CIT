let pages = [];
let operations = [];


const linkColor = document.getElementById('rolesLink')
document.querySelectorAll('.nav_link').forEach(l => l.classList.remove('active'));
linkColor.classList.add('active');


if (!localStorage.getItem('user')) window.href.location = '/Account/Index';

const appHeaders = {
    'content-type': 'application/json',
    'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user'))?.token}`,
    'Page': 'Roles'
};

const getRoles = () => {
    doRequest(
        {
            url: '/Roles/GetRoles',
            method: 'GET',
            data: null,
            headers: {
                ...appHeaders,
                'Operation': 'Obtener'
            },
            successCallback: function (data) { onGetRoles(data) },
            errorCallback: function (error) { onError(error) }
        }
    );
}

const onGetRoles = (data) => {
    $("#paginator-container").pagination({
        dataSource: data,
        callback: function (data, pagination) {
            const html = templateRolesList(data);
            $("#rolesList").html(html);
            $("#loadingRoles").css({ 'display': 'none' });
            $("#rolesTable").removeClass('d-none');
        }

    });
}

const templateRolesList = (roles) => {

    const tBody = $("#rolesList");
    tBody.html("");
    let html = "";
    roles.forEach(role => {
        html += `<tr>
                    <td>${role.roleId}</td>
                    <td>${role.role}</td>
                    <td>
                        <button type="button" class="btn btn-sm btn-danger" onclick="deleteRole(${role.roleId})"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>`;
    });
    return html;
}

const deleteRole = function (roleId) {
    $("#DeleteConfirmRoleModal").modal('show');

    $("#deleteRoleBtn").on('click', function () {
        doRequest(
            {
                url: '/Roles/DeleteRole/' + roleId,
                method: 'GET',
                data: null,
                headers: { ...appHeaders, 'Operation': 'Eliminar' },
                successCallback: function (data) {
                    $("#DeleteConfirmRoleModal").modal('hide');
                    alert(data);
                    getRoles();
                },
                errorCallback: function (error) { onError(error) }
            }
        );
    });
}

getRoles();


const getPages = () => {
    doRequest({
        url: '/Page/GetPages',
        method: 'GET',
        data: null,
        headers: {
            ...appHeaders,
            'Operation': 'Obtener'
        },
        successCallback: function (data) { onGetPages(data) },
        errorCallback: function (error) { onError(error) }
    });
}

const onGetPages = function (pages) {
    let html = "";
    pages.forEach((page, index) => {
        html += `<input type="checkbox" class="form-check-input mx-2" id="page-${page.pageId}" onchange="selectPage('${page.pageId}')" /><label class="form-check-label" for="page-${page.pageId}">${page.pageName}</label>`;
    });
    $("#pages").html(html);
}

const selectPage = function (pageId) {

    if ($("#page-" + pageId).is(':checked'))
        pages.push(pageId);
    else {
        const pageIndex = pages.indexOf(pageId);
        pages.splice(pageIndex, 1);
    }
}

getPages();

const getOperations = () => {
    doRequest({
        url: '/Operation/GetOperations',
        method: 'GET',
        data: null,
        headers: {
            ...appHeaders,
            'Operation': 'Obtener'
        },
        successCallback: function (data) { onGetOperations(data) },
        errorCallback: function (error) { onError(error) }
    });
}

const onGetOperations = function (operations) {
    let html = "";
    operations.forEach(operation => {
        html += `<input type="checkbox" class="form-check-input mx-2" onchange="selectOperation('${operation.operationId}')" id="operation-${operation.operationId}" /><label class="form-check-label" for="operation-${operation.operationId}">${operation.operationName}</label>`;
    });
    $("#operations").html(html);
}

const selectOperation = function (operationId) {
    if ($("#operation-" + operationId).is(':checked'))
        operations.push(operationId);
    else {
        const operationIndex = operations.indexOf(operationId);
        operations.splice(operationIndex, 1);
    }
}

getOperations();

$("#addOrUpdateRoleBtn").on('click', function () {
    let rolePermissions = [];

    pages.forEach(pageId => {
        operations.forEach(operationId => {
            rolePermissions.push({
                operationId: operationId,
                pageId: pageId
            });
        });
    });

    const newRole = {
        Role: $("#roleName").val(),
        RolePermissions: rolePermissions
    };

    doRequest(
        {
            url: '/Roles/CreateRole',
            method: 'POST',
            data: newRole,
            headers: {
                ...appHeaders,
                'Operation': 'Agregar'
            },
            successCallback: function (data) { onSaveRole(data) },
            errorCallback: function (error) { onError(error) }
        }
    );
});

const onSaveRole = function (data) {
    $("#roleForm").trigger('reset');
    pages = [];
    operations = [];
    getRoles();
}

const onError = function (error) {
    $("#DeleteConfirmRoleModal").modal('hide');
    $("#errorMessage").removeClass('d-none');
    $("#errorMessage").html("<p>" + error.responseText + "</p>");

    setTimeout(function () {
        $("#errorMessage").html("");
        $("#errorMessage").addClass('d-none');
    }, 7000);
}