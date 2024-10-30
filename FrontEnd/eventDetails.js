window.onload = async function () {
    const eventId = localStorage.getItem('eventId'); 

    debugger
    if (!eventId) {
        alert("Event ID not found!");
        return;
    }

    try {
        const response = await fetch(`https://localhost:44398/api/Events/${eventId}`);
        if (!response.ok) {
            throw new Error(`Error fetching event: ${response.statusText}`);
        }

        const eventData = await response.json();

        // Populate the HTML with the event data
        document.getElementById('eventTitle').textContent = eventData.title;
        document.getElementById('eventDate').textContent = new Date(eventData.startDate).toLocaleDateString();
        document.getElementById('eventLocation').textContent = eventData.location;
        document.getElementById('eventCapacity').textContent = eventData.capacity;
        document.getElementById('eventFee').textContent = `${eventData.registrationFee.toFixed(2)} JOD`;
        document.getElementById('eventDescription').textContent = eventData.description;
        
        // Set the event image if available
        document.getElementById('eventImage').src = '../BackEnd/Motostation/Motostation/Uploads/' + eventData.coverImageUrl ;
    } catch (error) {
        console.error('Error fetching event details:', error);
        alert('There was an error fetching event details. Please try again later.');
    }
};
