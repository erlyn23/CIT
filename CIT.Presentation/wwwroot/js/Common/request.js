'use strict'

function doRequest({ url, method, data, headers, successCallback, errorCallback }) {
    const options = {
        method: method,
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (data) {
            successCallback(data);
        },
        error: function (err) {
            errorCallback(err);
        }
    };

    if (headers !== null) {
        options.headers = headers;
    }

    $.ajax(url, options);
};