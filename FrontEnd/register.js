    document.getElementById('registerForm').addEventListener('submit', function (e) {
        e.preventDefault();

        // Create FormData object and append form inputs by name
        const formData = new FormData(this); // Automatically picks up all form fields with 'name' attributes

        // Send form data to the server
        fetch('/api/user/register', {
            method: 'POST',
            body: formData // Sending FormData directly
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('OTP has been sent to your email');
                // Redirect or ask for OTP input
                window.location.href = "registerOtp.html";
            } else {
                alert('Error: ' + data.message);
            }
        })
        .catch(error => console.error('Error:', error));
    });

