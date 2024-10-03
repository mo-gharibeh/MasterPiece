let userId = localStorage.getItem('userId');


async function saveChanges() {
debugger
    let formData = new FormData(document.getElementById('editForm'));
    const url = `https://localhost:44398/api/Users/editProfile/${userId}`;
    let response = await fetch(url, { 
        method: 'PUT',
        body: formData
    });
    console.log(response);
    debugger 
    
    // Check the response status
    if (response.ok) {
        let result = await response.json();
        alert('Profile updated successfully!');
        console.log(result);
    } else {
        // Handle errors 
        const errorData = await response.json();
        alert('Failed to update profile. Please try again.');
        console.error(errorData);
    }
}
