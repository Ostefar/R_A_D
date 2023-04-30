// Wait for the DOM to be fully loaded before executing the script
document.addEventListener("DOMContentLoaded", function () {

    // Get a reference to the form and the button
    const form = document.getElementById("signup-form");
    const button = document.getElementById("signup-btn");

    // Add an event listener to the button
    button.addEventListener("click", function (event) {
        event.preventDefault();
        // Get the form data as a JSON object
        const formData = JSON.stringify({
            FirstName: form.FirstName.value,
            LastName: form.LastName.value,
            Email: form.Email.value,
            Phone: form.Phone.value,
            AddressLine1: form.AddressLine1.value,
            AddressLine2: form.AddressLine2.value,
            Country: form.Country.value,
            City: form.City.value,
            ZipCode: form.ZipCode.value,
        });

        // Create a new AJAX request
        const xhr = new XMLHttpRequest();

        // Set up the request parameters
        xhr.open("POST", "https://localhost:7111/User/CreateUser", true);
        xhr.setRequestHeader("Content-Type", "application/json");

        // Add an event listener to handle the response
        xhr.addEventListener("load", function () {
            if (xhr.status === 200) {
                // Success
                alert("Sign up successful!");
            } else {
                // Error
                alert("Error: " + xhr.responseText);
            }
        });

        // Send the AJAX request with the form data
        xhr.send(formData);
    });
});
