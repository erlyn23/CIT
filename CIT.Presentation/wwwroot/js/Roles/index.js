let selectedRolePermissions = [];
let pagesCounter = 0;
let pagesIds = [];
let operationsIds = [];


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
        pageSize: 5,
        callback: function (data, pagination) {
            const html = templateRolesList(data);
            $("#rolesList").html(html);
            setRoleEditEvent(data);
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
                        <button type="button" id="editRoleBtn-${role.roleId}" class="btn btn-sm btn-warning"><i class="fas fa-edit"></i></button>
                        <button type="button" class="btn btn-sm btn-danger" onclick="deleteRole(${role.roleId})"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>`;
    });
    return html;
}

const setRoleEditEvent = function (roles) {
    roles.forEach(role => {
        $("#editRoleBtn-" + role.roleId).on('click', function () {
            setRoleData(role);
        });
    });
}

const setRoleData = function (role) {
    resetSelection();
    $("#action-title").text("Guardar cambios");
    $("#form-title").text("Modificar rol");
    $("#roleId").val(role.roleId);
    $("#roleName").val(role.role);

    const uniquePagesIds = [... new Set(role.rolePermissions.map(rolePermission => rolePermission.pageId))];
    const uniqueOperationsIds = [... new Set(role.rolePermissions.map(rolePermission => rolePermission.operationId))];
    uniquePagesIds.forEach((rolePermissionPageId) => {
        $("#page-" + rolePermissionPageId).prop('checked', true);
        uniqueOperationsIds.forEach(rolePermissionOperationId => {
            $("#operation-" + rolePermissionPageId + "-" + rolePermissionOperationId).prop('checked', true);
        });
        selectPage(rolePermissionPageId);
    });
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
        successCallback: function (data) { onGetPages(data); },
        errorCallback: function (error) { onError(error) }
    });
}


const onGetPages = function (pages) {
    let html = "";
    pages.forEach((page) => {
        html += `
                    <div class="col-md-3">
                        <div class="card mb-2">
                            <div class="card-header">
                                <input type="checkbox" class="form-check-input mx-2" id="page-${page.pageId}" onchange="selectPage('${page.pageId}')" /><label class="form-check-label" for="page-${page.pageId}">${page.pageName}</label>
                            </div>
                            <div class="card-body" id="roleOperations-${page.pageId}">
                               
                            </div>
                        </div>
                    </div>`;
        pagesIds.push(page.pageId);
    });
    $("#pages").html(html);

    getOperations();
}

const selectPage = function (pageId) {
    if ($("#page-" + pageId).is(":checked")) {
        selectedRolePermissions.push({
            pageId: pageId,
            operations: []
        });
    }
    else {
        for (let i = 0; i < selectedRolePermissions.length; i++) {
            if (selectedRolePermissions[i].pageId === pageId) {
                selectedRolePermissions.splice(i, 1);
            }
            operationsIds.forEach(operationId => {
                $("#operation-" + pageId + "-" + operationId).prop('checked', false);
            });
        }
    }
}

const getOperations = () => {
    doRequest({
        url: '/Operation/GetOperations',
        method: 'GET',
        data: null,
        headers: {
            ...appHeaders,
            'Operation': 'Obtener'
        },
        successCallback: function (data) { onGetOperations(data); },
        errorCallback: function (error) { onError(error) }
    });
}

const onGetOperations = function (operations) {
    let html = "";

    operations.forEach(operation => {
        html += `
                    <div class="form-group">
                        <input type="checkbox" class="form-check-input mx-2" id="operation-%s-${operation.operationId}" /><label class="form-check-label" for="operation-%s-${operation.operationId}">${operation.operationName}</label>
                    </div>
                    `;
        operationsIds.push(operation.operationId);
    });
    for (let i = 0; i < pagesIds.length; i++) {
        $("#roleOperations-" + pagesIds[i]).html(html.replaceAll("%s", pagesIds[i]));
    }
}

getPages();

$("#addOrUpdateRoleBtn").on('click', function () {

    const roleId = $("#roleId").val();

    permissionsToSend = [];
    permissionsToDelete = [];
    selectedRolePermissions.forEach((rolePermission, index) => {
        operationsIds.forEach((operationId, opIndex) => {
            let operationCheck = $("#operation-" + rolePermission.pageId + "-" + operationId);
            if (operationCheck.is(":checked")) {
                permissionsToSend.push({
                    pageId: rolePermission.pageId,
                    operationId: operationId
                });
            } else if (roleId.length !== 0 && !operationCheck.is(':checked')) {
                permissionsToDelete.push({
                    pageId: rolePermission.pageId,
                    operationId: operationId,
                    roleId: $("#roleId").val()
                });
            }
        });
    });

    const newRole = {
        RoleId: (roleId.length != 0) ? roleId : 0,
        Role: $("#roleName").val(),
        RolePermissions: permissionsToSend,
        ToDelete: (roleId.length != 0) ? permissionsToDelete : []
    };

    doRequest(
        {
            url: (roleId.length != 0) ? '/Roles/UpdateRole' : '/Roles/CreateRole',
            method: 'POST',
            data: newRole,
            headers: {
                ...appHeaders,
                'Operation': (roleId.length != 0) ? 'Modificar' : 'Agregar'
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
    $("#form-title").text("Agregar nuevo rol");
    $("#action-title").text("Agregar rol");
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

$("#clearCurrentChanges").on('click', function () {
    resetSelection();
});

const resetSelection = function () {
    $("#roleId").val("");
    $("#roleName").val("");
    $("#form-title").text("Agregar nuevo rol");
    $("#action-title").text("Agregar rol");
    const checkboxElements = document.querySelectorAll("input[type=checkbox]");

    checkboxElements.forEach(cb => {
        cb.checked = false;
    })

    selectedRolePermissions = [];
    rolePermissionsToDelete = [];
}