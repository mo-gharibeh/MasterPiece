window.onload = async function () {
    debugger
    const eventId = localStorage.getItem('eventId'); 

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
        document.getElementById('eventDate').textContent = new Date(eventData.eventDate).toLocaleDateString();
        document.getElementById('eventLocation').textContent = eventData.location;
        document.getElementById('eventDescription').textContent = eventData.description;

        // Optional: If you have event-specific images
        // document.getElementById('eventImage').src = eventData.imageUrl || 'img/event-image-placeholder.jpg';
    } catch (error) {
        console.error('Error fetching event details:', error);
        alert('There was an error fetching event details. Please try again later.');
    }
};
