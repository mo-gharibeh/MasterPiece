localStorage.setItem("managerId", 6);
debugger
async function addEventDetailes(){
    let eventName = document.getElementById("eventName").value;
    let eventDate = document.getElementById("eventDate").value;
    let eventDescription = document.getElementById("eventDescription").value;
    let eventLocation = document.getElementById("eventLocation").value;

    debugger
    // Create an object to hold the data
    const eventData = {
        title: eventName,
        description: eventDescription,
        location: eventLocation,
        eventDate: eventDate,
        userId: localStorage.getItem('managerId')
    };
    try {
        // Send a POST request to the API endpoint
        const response = await fetch('https://localhost:44398/api/Events', {
            method: 'POST',
            headers: {  
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(eventData),
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }

        // Parse the response (if the API returns JSON data)
        // const result = await response.json();
        alert('Event added successfully!');

        // Optionally clear the form after successful submission
        document.getElementById('eventForm').reset();

    } catch (error) {
        console.error('Error adding event:', error);
        alert('There was an error adding the event. Please try again.');
    }
}