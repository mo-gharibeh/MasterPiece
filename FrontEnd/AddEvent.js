

const organizerId = 6;

function addEventDetails() {    
    debugger
    // Prevent the default form submission
    event.preventDefault();

    // Create a new FormData object
    const formData = new FormData(document.getElementById('eventForm'));

    formData.append('OrganizerId', organizerId); 

    // Send the data to the API
    fetch('https://localhost:44398/api/Events', { // Update with the correct API endpoint
        method: 'POST',
        body: formData,
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json();
    })
    .then(data => {
        // Handle success - show a message or redirect
        alert(data.message); // Display success message
        // Optionally, clear the form or redirect the user
        document.getElementById('eventForm').reset();
    })
    .catch(error => {
        // Handle error - show an error message
        console.error('There has been a problem with your fetch operation:', error);
        alert('Error adding event: ' + error.message);
    });
}
