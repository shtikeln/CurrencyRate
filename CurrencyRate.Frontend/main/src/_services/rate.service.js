import { BehaviorSubject } from 'rxjs';

import config from 'config';
import { authHeader, handleResponse } from '@/_helpers';

export const rateService = {
    convertSum
};

function convertSum(date, sum) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader(),
        //body: JSON.stringify({ username, password })
    };

    var d = new Date(date);

    return fetch(`${config.apiUrl}/exchange/${sum}/fordate/${d.toLocaleDateString("ru-RU")}`, requestOptions)
        .then(handleResponse);
}
