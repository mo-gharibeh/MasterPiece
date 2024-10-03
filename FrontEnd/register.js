    document.getElementById('registerForm').addEventListener('submit', function (e) {
        e.preventDefault();
        debugger

        // Create FormData object and append form inputs by name
        const formData = new FormData(this); // Automatically picks up all form fields with 'name' attributes

        // Send form data to the server
        fetch('https://localhost:44398/api/user/register', {
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


    // For loged in users

    function login() {
        
        debugger
        const userName = document.getElementById('username').value;
        const password = document.getElementById('passwordLogin').value;

        fetch('https://localhost:44398/api/Users/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userName: userName,
                password: password
            })
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Login failed');
            }
            return response.json();
        })
        .then(data => {
            // تحقق إذا كان المستخدم موجود والدور Manager أو User
            if (data.role) {
                // إذا كان المستخدم مديراً، قم بتخزين بعض المعلومات في localStorage
                if (data.role === 'Manager') {
                    localStorage.setItem('userRole', data.role);
                    localStorage.setItem('userId', data.userId);
                }

                // تحويل المستخدم إلى صفحة الملف الشخصي
                window.location.href = 'profile.html';
            } else {
                alert('Login failed: User not found');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Login failed');
        });
    }


