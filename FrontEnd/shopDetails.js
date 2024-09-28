// Example: Fetching store data from an API and updating the HTML dynamically
async function loadStoreDetails() {
    const storeId = localStorage.getItem('storeId');

    try {
        // Fetch store data from the API (make sure to replace the URL with your actual API endpoint)
        let response = await fetch(`https://localhost:44398/api/Stores/${storeId}`); // Use the correct store ID or endpoint
        if (!response.ok) {
            throw new Error("Failed to fetch store details");
        }

        // Parse the response data
        let store = await response.json();

        // Dynamically update the HTML with the fetched store data
        document.getElementById("storeName").textContent = store.storeName;
        document.getElementById("storeAddress").textContent = store.address;
        document.getElementById("storePhone").textContent = store.phone;
        document.getElementById("storeEmail").textContent = store.email;
        document.getElementById("storeWorkingHours").textContent = store.workingHours;
        // document.getElementById("storeLocation").textContent = store.location;
        // document.getElementById("storeDescription").textContent = store.description; // If you have a description in the API

        // Update the image if available
        if (store.storeImageUrl) {
            document.getElementById("storeImage").src = `../BackEnd/Motostation/Motostation/Uploads/${store.storeImageUrl}`;
        }
    } catch (error) {
        console.error("Error loading store details:", error);
    }
}

// Call the function when the page loads
loadStoreDetails();
