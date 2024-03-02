
import { rootAddress, authenticate } from "./common.js";

const logInUrl = rootAddress + "/api/auth/login";

let fingerprint = "";

import('https://openfpcdn.io/fingerprintjs/v4')
    .then(FingerprintJS => FingerprintJS.load())
    .then(fp => fp.get())
    .then(result => fingerprint = result.visitorId);

const userEmailEl = document.querySelector(".auth-form_login-email");
const userPasswordEl = document.querySelector(".auth-form_login-password");
const authFormButtonEl = document.querySelector(".auth-form-buttons_btn");


authFormButtonEl.addEventListener('click', function (event) {
    event.preventDefault();

    let loginRequest = {
        email: userEmailEl.value,
        password: userPasswordEl.value,
        fingerprint: fingerprint
    }

    authenticate(logInUrl, loginRequest, rootAddress);
})