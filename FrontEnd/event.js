
async function GetAllEvents(){
    debugger
    const response = await fetch('https://localhost:44398/api/Events');
    if (!response.ok) {
        console.error("Failed to fetch events");
        return;
    }
    const result = await response.json();

    const container = document.getElementById("CardContainer");

    container.innerHTML = ''; // Clear existing products

    result.forEach(element => { // Iterate over the parsed JSON array
        container.innerHTML += `
        <div class="col-md-4 mb-4">
            <div class="card">
                <img src="https://israel-taxi.com/wp-content/uploads/2024/07/road-to-the-dead-sea.jpg" class="card-img-top" alt="Scenic coastal highway with ocean views">
                <div class="card-body">
                    <h5 class="card-title">${element.title}</h5>
                    <p class="card-text"><i class="fas fa-map-marker-alt"></i>${element.location} ${element.eventDate}</p>
                    <p class="card-text">${element.description}</p>
                    <a onclick="saveEventId(${element.eventId})" class="btn btn-primary">View Event</a>
                </div>
            </div>
        </div>
        `;
    });   
}

function saveEventId(eventId) {
    localStorage.setItem("eventId", eventId); 
    window.location.href = "eventDetails.html"; // Redirect to the shop details page
}
GetAllEvents();


