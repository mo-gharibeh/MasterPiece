// Base API URL
const baseUrl = 'https://localhost:44398/api/Events';

// Function to fetch all events
async function fetchAllEvents() {
    try {
        const response = await fetch(`${baseUrl}`);
        if (!response.ok) throw new Error("Failed to fetch events");
        
        const events = await response.json();
        displayEvents(events, 'All Events');
    } catch (error) {
        console.error("Error fetching all events:", error);
    }
}

// Function to fetch free events
async function fetchFreeEvents() {
    try {
        const response = await fetch(`${baseUrl}/free`);
        if (!response.ok) throw new Error("Failed to fetch free events");

        const events = await response.json();
        displayEvents(events, 'Free Events');
    } catch (error) {
        console.error("Error fetching free events:", error);
    }
}

// Function to fetch paid events
async function fetchPaidEvents() {
    try {
        const response = await fetch(`${baseUrl}/paid`);
        if (!response.ok) throw new Error("Failed to fetch paid events");

        const events = await response.json();
        displayEvents(events, 'Paid Events');
    } catch (error) {
        console.error("Error fetching paid events:", error);
    }
}

// Function to display events on the webpage
function displayEvents(events, title) {
    // Clear previous content
    const eventsContainer = document.getElementById('eventsContainer');
    const eventsTitle = document.getElementById('eventsTitle');
    
    // Set title
    eventsTitle.innerHTML = `<h2 class="mb-4">${title}</h2>`;

    // Clear container
    eventsContainer.innerHTML = "";

    // Iterate over events and create cards
    events.forEach(element => {
        const eventCard = document.createElement('div');
        eventCard.classList.add('col-md-4', 'mb-4'); 

        eventCard.innerHTML = `
            <div class="card h-100">
                <img src="../BackEnd/Motostation/Motostation/Uploads/${element.coverImageUrl}" 
                    class="card-img-top" alt="${element.title}">
                <div class="card-body d-flex flex-column">
                    <h5 class="card-title">${element.title}</h5>
                    <p class="card-text"><i class="fas fa-map-marker-alt"></i> ${element.location}</p>
                    <p class="card-text">${element.description}</p>
                    <p class="card-text"><small class="text-muted">Date: ${new Date(element.startDate).toLocaleDateString()}</small></p>
                    <p class="card-text"><strong>Registration Fee:</strong> ${element.registrationFee > 0 ? `$${element.registrationFee}` : "Free"}</p>
                    <button onclick="saveEventId(${element.eventId})" class="btn btn-primary mt-auto">View Event</button>
                </div>
            </div>
        `;

        // Append card to container
        eventsContainer.appendChild(eventCard);
    });
}



fetchAllEvents();

// Event listeners for fetching different types of events
document.getElementById('fetchAllEventsBtn').addEventListener('click', fetchAllEvents);
document.getElementById('fetchFreeEventsBtn').addEventListener('click', fetchFreeEvents);
document.getElementById('fetchPaidEventsBtn').addEventListener('click', fetchPaidEvents);


function saveEventId(eventId) {
    localStorage.setItem("eventId", eventId); 
    window.location.href = "eventDetails.html";
}



// += `
// <div class="col-md-4 mb-4">
//     <div class="card h-100">
//         <img src="../BackEnd/Motostation/Motostation/Uploads/${element.coverImageUrl}" 
//             class="card-img-top" alt="${element.title}">
//         <div class="card-body d-flex flex-column">
//             <h5 class="card-title">${element.title}</h5>
//             <p class="card-text"><i class="fas fa-map-marker-alt"></i> ${element.location}</p>
//             <p class="card-text">${element.description}</p>
//             <p class="card-text"><small class="text-muted">Date: ${new Date(element.startDate).toLocaleDateString()}</small></p>
//             <button onclick="saveEventId(${element.eventId})" class="btn btn-primary mt-auto">View Event</button>
//         </div>
// ///     </div>
// </div>
// `;

{/* <img src="../BackEnd/Motostation/Motostation/Uploads/${element.coverImageUrl}" 
class="card-img-top" alt="${element.title}">
<div class="card-body d-flex flex-column">
<h5 class="card-title">${element.title}</h5>
<p class="card-text"><i class="fas fa-map-marker-alt"></i> ${element.location}</p>
<p class="card-text">${element.description}</p>
<p class="card-text"><small class="text-muted">Start Date: ${new Date(element.startDate).toLocaleDateString()}</small></p>
<p class="card-text"><small class="text-muted">End Date: ${new Date(element.endDate).toLocaleDateString()}</small></p>
<p class="card-text"><strong>Registration Fee:</strong> ${element.registrationFee > 0 ? `$${element.registrationFee}` : "Free"}</p>
<p class="card-text"><strong>Type:</strong> ${element.eventType || 'N/A'}</p>
<p class="card-text"><strong>Status:</strong> ${element.status}</p>
<p class="card-text"><strong>Capacity:</strong> ${element.capacity}</p>
<button onclick="saveEventId(${element.eventId})" class="btn btn-primary mt-auto">View Event</button>
</div> */}