'use strict'

function doRequest({ url, method, data, headers, successCallback, errorCallback }) {
    const options = {
        method: method,
        contentType: 'application/json',
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
    if (data !== null) {
        options.data = JSON.stringify(data);
    }

    $.ajax(url, options);
};