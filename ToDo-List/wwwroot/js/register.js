
import { rootAddress, authenticate } from "./common.js";

const registerUrl = rootAddress + "/api/auth/register";

let fingerprint = "";

import('https://openfpcdn.io/fingerprintjs/v4')
    .then(FingerprintJS => FingerprintJS.load())
    .then(fp => fp.get())
    .then(result => fingerprint = result.visitorId);

const userNameEl = document.querySelector(".auth-form_register-name");
const userEmailEl = document.querySelector(".auth-form_register-email");
const userPasswordEl = document.querySelector(".auth-form_register-password");
const userPassworRepeatdEl = document.querySelector(".auth-fform_register-password-repeat");
const authFormButtonEl = document.querySelector(".auth-form-buttons_btn");


authFormButtonEl.addEventListener('click', function (event) {
    event.preventDefault();

    let registerRequest = {
        name: userNameEl.value,
        email: userEmailEl.value,
        password: userPasswordEl.value,
        passwordRepeat: userPassworRepeatdEl.value,
        fingerprint: fingerprint
    }

    authenticate(registerUrl, registerRequest, rootAddress);
})