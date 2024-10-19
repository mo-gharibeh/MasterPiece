    // fro register a new user when clic on signup() button and save the data in database by API FromForm
    // by  using FormData 
    //

    document.getElementById('signupForm').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent form submission
        const formData = new FormData(document.getElementById('signupForm'));
        debugger
        try {
            // Send registration request to the server
            const response = await fetch('https://localhost:44398/api/Users/register', {
                method: 'POST',
                body: formData
            });
            const data = await response.json();
            
            if (response.ok && data.success) {
                // Show OTP modal if registration is successful

                // openModal();   // Show modal
                alert('User Registration complete.');
                window.location.href = "register.html";          ////// editing 

                document.getElementById('statusMessage').textContent = 'OTP sent to your email.';
            } else {
                // Handle registration failure
                document.getElementById('statusMessage').textContent = data.message || 'Registration failed.';
            }
        } catch (error) {
            console.error('Error during registration:', error);
            document.getElementById('statusMessage').textContent = 'Error occurred during registration.';
        }
    });
    
    // Function to open the modal
    function openModal() {
        document.getElementById('otpModal').style.display = 'block';
    }
    
    // Function to close the modal
    function closeModal() {
        document.getElementById('otpModal').style.display = 'none';
    }
    
    // Function to confirm OTP
    async function confirmOtp() {
        
        const email = document.getElementById('email').value;
        const otp = document.getElementById('otp').value;
    
        try {
            const response = await fetch('https://localhost:44398/api/Users/confirm-otp', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Email: email, Otp: otp })
            });
    
            const data = await response.json();
    
            if (response.ok && data.success) {
                document.getElementById('statusMessage').textContent = 'OTP confirmed. Registration complete.';
                window.location.href = "register.html";
                closeModal(); // Close modal upon successful OTP confirmation
            } else {
                document.getElementById('statusMessage').textContent = data.message || 'Invalid OTP.';
            }
        } catch (error) {
            console.error('Error during OTP confirmation:', error);
            document.getElementById('statusMessage').textContent = 'Error occurred during OTP confirmation.';
        }
    }
    












    // For loged in users
    document.getElementById('loginForm').addEventListener('submit', login);
    async function login(event) {
        event.preventDefault(); // Prevents the default form submission behavior
        debugger
        
        const userName = document.getElementById('username').value.trim();
        const password = document.getElementById('passwordLogin').value.trim();
    
        if (!userName || !password) {
            alert('Please enter both username and password.');
            return;
        }
    
        try {
            const response = await fetch('https://localhost:44398/api/Users/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    userName: userName,
                    password: password
                })
            });
    
            if (!response.ok) {
                throw new Error('Login failed');
            }
    
            const data = await response.json();
            debugger

            if(data.role == "Admin") {
                localStorage.setItem('userId', data.userId);
                window.location.href = "Admin/dashboard.html";
            }
            else if (data.role) {
                debugger
                // Store user ID in localStorage
                localStorage.setItem('userId', data.userId);
                localStorage.setItem('isLogedin', true);
                
                // If user is a manager, store role as well
                if (data.role === 'Manager') {
                    localStorage.setItem('userRole', data.role);
                }

                // Redirect to the profile page
                window.location.href = 'profile.html';
            } else {
                alert('Login failed: User not found');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Login failed');
        }
    }
    


