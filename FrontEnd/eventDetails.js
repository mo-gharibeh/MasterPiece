document.addEventListener("DOMContentLoaded", function() {
    const eventId = localStorage.getItem('eventId');

    async function fetchEventDetails(eventId) {
        debugger
        try {
            const response = await fetch(`https://localhost:44398/api/Events/${eventId}`);
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const event = await response.json();
            displayEventDetails(event);
        } catch (error) {
            console.error("Error fetching event details:", error);
        }
    }

    function displayEventDetails(event) {
        // Basic Event Information
        document.getElementById("eventTitle").textContent = event.title;
        document.getElementById("eventImage").src +=  event.coverImageUrl;
        document.getElementById("eventLocation").textContent = `Location: ${event.location}`;
        document.getElementById("eventDates").textContent = `From ${event.startDate} to ${event.endDate}`;
        // document.getElementById("eventCapacity").textContent = `Capacity: ${event.capacity}`;
        document.getElementById("eventFee").textContent = event.isPaid ? `Fee: $${event.registrationFee}` : "Free Event";
        document.getElementById("eventStatus").textContent = `Status: ${event.status}`;
        document.getElementById("eventDescription").textContent = event.description;

        // Start and End Locations
        const startLocationElement = document.getElementById("startLocation");
        const endLocationElement = document.getElementById("endLocation");

        if (event.startLocation) {
            startLocationElement.textContent = `Start Location: ${event.startLocation}`;
        } else {
            startLocationElement.textContent = "Start Location: Not Provided";
        }

        if (event.endLocation) {
            endLocationElement.textContent = `End Location: ${event.endLocation}`;
        } else {
            endLocationElement.textContent = "End Location: Not Provided";
        }

        // Activities and Rest Stops
        const activitiesList = document.getElementById("eventActivities");
        const restStopsList = document.getElementById("eventRestStops");

        // Populate activities and rest stops from JSON
        JSON.parse(event.freeActivities || "[]").forEach(activity => {
            const li = document.createElement("li");
            li.textContent = activity;
            activitiesList.appendChild(li);
        });

        JSON.parse(event.restStops || "[]").forEach(stop => {
            const li = document.createElement("li");
            li.textContent = stop;
            restStopsList.appendChild(li);
        });
    }

    fetchEventDetails(eventId);
});
