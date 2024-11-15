const organizerId = 6;
let freeActivities = [];
let restStops = [];

function addFreeActivity() {
    const activityInput = document.getElementById('freeActivityInput');
    const activity = activityInput.value.trim();
    if (activity) {
        freeActivities.push(activity);
        const li = document.createElement('li');
        li.textContent = activity;
        document.getElementById('freeActivitiesList').appendChild(li);
        activityInput.value = ""; // Clear input
    }
}

function addRestStop() {
    const restStopInput = document.getElementById('restStopInput');
    const restStop = restStopInput.value.trim();
    if (restStop) {
        restStops.push(restStop);
        const li = document.createElement('li');
        li.textContent = restStop;
        document.getElementById('restStopsList').appendChild(li);
        restStopInput.value = ""; // Clear input
    }
}

function addEventDetails(event) {    
    event.preventDefault();

    const formData = new FormData(document.getElementById('eventForm'));
    formData.append('OrganizerId', organizerId);
    formData.append('FreeActivities', JSON.stringify(freeActivities)); // Save as JSON array
    formData.append('RestStops', JSON.stringify(restStops));           // Save as JSON array

    fetch('https://localhost:44398/api/Events', {
        method: 'POST',
        body: formData,
    })
    .then(response => {
        if (!response.ok) throw new Error('Network response was not ok ' + response.statusText);
        return response.json();
    })
    .then(data => {
        alert(data.message);
        document.getElementById('eventForm').reset();
        freeActivities = []; // Reset arrays after submission
        restStops = [];
        document.getElementById('freeActivitiesList').innerHTML = '';
        document.getElementById('restStopsList').innerHTML = '';
    })
    .catch(error => {
        console.error('There has been a problem with your fetch operation:', error);
        alert('Error adding event: ' + error.message);
    });
}
