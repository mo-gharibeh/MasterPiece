let userId = localStorage.getItem('userId');

document.getElementById("editForm").addEventListener("submit", async function (e) {
    e.preventDefault(); 

    const form = e.target;
    const formData = new FormData(form); 

    const apiEndpoint = `https://localhost:44398/api/Users/editProfile/${userId}`;

    try {
        const response = await fetch(apiEndpoint, {
            method: "PUT",
            body: formData
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error("Error:", errorData);
            alert("Failed to update profile: " + errorData.message);
            return;
        }

        const result = await response.json();
        console.log("Success:", result);
        alert(result.message || "Profile updated successfully!");
    } catch (error) {
        console.error("Fetch error:", error);
        alert("An error occurred while updating the profile.");
    }
});



async function viewProfile(){
    debugger
    let profileImage = document.getElementById("profileImage");
    let name = document.getElementById("name");
    let phone = document.getElementById("phone");
    let address = document.getElementById("location");


    let response = await fetch(`https://localhost:44398/api/Users/${userId}`);
    if (!response.ok) {
        console.error("Failed to fetch user profile");
        return;
    }
    let user = await response.json();
    profileImage.src = "";
    profileImage.src =  `../BackEnd/Motostation/Motostation/Uploads/${user.profileImageUrl}`;
    name.textContent = "";
    name.textContent = user.fullName;
    // phone.textContent = "";
    phone.textContent += user.phoneNumber;
    // address.textContent = "";
    address.textContent += user.location;

    console.log(user);

}

viewProfile();



async function changePassword() {
    const userId = localStorage.getItem('userId');
    const currentPassword = document.getElementById('currentPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    const payload = {
        currentPassword: currentPassword,
        newPassword: newPassword,
        confirmPassword: confirmPassword
    };
    debugger
        // Validate password before continuing
        if (!validatePassword()) {
            return; // Stop submission if password is invalid
        }

    try {
        const response = await fetch(`https://localhost:44398/api/Users/changePassword/${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
                // Add authorization header if required
                // 'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(payload)
        });

        if (response.ok) {
            const result = await response.json();
            alert(result.message);
            // Optionally, redirect or reset the form
            document.getElementById('currentPassword').value = '';
            document.getElementById('newPassword').value = '';
            document.getElementById('confirmPassword').value = '';


        } else {
            const errorData = await response.json();
            alert('Failed to change password. ' + (errorData.message || 'Please try again.'));
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Something went wrong. Please check your connection and try again.');
    }
}

// clear localstorage when logout
        
function logout() {
    localStorage.clear();
    window.location.href = "register.html";
}


function validatePassword() {
    const password = document.getElementById("newPassword").value;
    const message = document.getElementById("messagePass");
    const requirements = {
        length: { regex: /.{8,}/, message: "At least 8 characters" },
        uppercase: { regex: /[A-Z]/, message: "One uppercase letter" },
        lowercase: { regex: /[a-z]/, message: "One lowercase letter" },
        number: { regex: /[0-9]/, message: "One number" },
        specialChar: { regex: /[\W_]/, message: "One special character" }
    };

    // Collect all unmet requirements
    const unmetRequirements = Object.values(requirements).filter(req => !req.regex.test(password));

    if (unmetRequirements.length > 0) {
        // Show unmet requirements
        message.textContent = `Password must contain: ${unmetRequirements.map(req => req.message).join(", ")}`;
        message.classList.add("error");
        message.classList.remove("success");
        return false; // Password is invalid
    } else {
        // If all requirements are met
        message.textContent = "";
        message.classList.remove("error");
        message.classList.add("success");
        return true; // Password is valid
    }
}



async function fetchSubscriptionStatus(userId) {
    debugger
    const response = await fetch(`https://localhost:44398/api/Subscription/checkSubscriptionStatus/${userId}`);
    const data = await response.json();

    const messageElement = document.getElementById("message");
    const countdownElement = document.getElementById("countdown");

    messageElement.innerText = data.message;
    

    if (data.message.includes("days remaining")) {
        // عرض عداد تنازلي
        const remainingDays = parseInt(data.message.match(/\d+/)[0], 10);
        countdownElement.innerText = `${remainingDays} days left!`;
    } else {
        messageElement.innerText = "You don't have an active subscription.";

        countdownElement.innerText = ""; // لا شيء
    }
}

// استدعاء الدالة
fetchSubscriptionStatus(userId); // استبدل 1 بـ userId
