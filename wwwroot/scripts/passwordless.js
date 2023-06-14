/**
 * Inspired by a sample implementation in https://docs.passwordless.dev/guide/js-client.html
 */

async function registerToken(userData) {

    // Initiate the Passwordless client with your public api key. 
    // API_KEY is a global constant which is determined in the _Layout.cshtml
    const p = new Passwordless.Client({
        apiKey: API_KEY
    });
    
    try {

        // Fetch the registration token from the backend (ASP.NET minimal API)
        // REGISTER_TOKEN_BACKEND_URI is a global constant which is determined in the _Layout.cshtml
        const registerToken = await fetch(REGISTER_TOKEN_BACKEND_URI,
            {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userData)
            }).then(r => r.json());

        // Register the token with the end-user's device.
        const { token, error } = await p.register(registerToken.token);

        if (token) {
            console.log("Successfully registered WebAuthn. You can now sign in!");
        } else {
            alert("Token registration failed", error);
        }

        return token;

    } catch (e) {
        console.error("Error occured while token registration", e);
    }
}

async function signinWithDiscoverable() {

    //Initiate the Passwordless client with your public api key. 
    // API_KEY is a global constant which is determined in the _Layout.cshtml
    const p = new Passwordless.Client({
        apiKey: API_KEY,
    });

    try {

        // Enables browsers to suggest passkeys by opening a UI prompt (only works with discoverable passkeys)
        const { token, error } = await p.signinWithDiscoverable();

        if (error) {

            alert("Error occured during the sign-in or you cancelled the request.");
            console.log(error);
            return;
        }

        var signInTokenPayload = {
            "Token": token
        };

        // Call backend to verify the token.
        // VERIFY_SIGNIN_TOKEN_BACKEND_URI is a global constant which is determined in the _Layout.cshtml
        await fetch(VERIFY_SIGNIN_TOKEN_BACKEND_URI,
        {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(signInTokenPayload)
        }).then(function (res) {
            if (res.redirected) {
                window.location.href = res.url;
                return;
            }
        });

    } catch (e) {
        console.error("Error occured while sign in: ", e);
    }
}