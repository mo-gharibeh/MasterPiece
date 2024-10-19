let userId = localStorage.getItem('userId');


async function saveChanges() {
debugger
    let formData = new FormData(document.getElementById('editForm'));
    let response = await fetch(`https://localhost:44398/api/Users/editProfile/${userId}`, { 
        method: 'PUT',
        headers: {
                'Content-Type': 'application/json'},
        body: formData
    });
    console.log(response);
    debugger 
    
    // Check the response status
    if (response.ok) {
        let result = await response.json();
        alert('Profile updated successfully!');
        window.location.href = 'profile.html';  
        console.log(result);
    } else {
        // Handle errors 
        const errorData = await response.json();
        alert('Failed to update profile. Please try again.');
        console.error(errorData);
    }
}


async function viewProfile(){
    debugger
    let profileImage = document.getElementById("profileImage");
    let name = document.getElementById("name");


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
        } else {
            const errorData = await response.json();
            alert('Failed to change password. ' + (errorData.message || 'Please try again.'));
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Something went wrong. Please check your connection and try again.');
    }
}
