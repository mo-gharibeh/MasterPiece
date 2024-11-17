localStorage.setItem("managerId", 6);

let storeName = document.getElementById("storeName");
let address = document.getElementById("address");
let phone = document.getElementById("phone");
let email = document.getElementById("email");
let workingHours = document.getElementById("workingHours");
let storeImage = document.getElementById("storeImage"); // File input
let storeLocation  = document.getElementById("storeLocation");


// Store the store information including image in the database
async function getStoreDetails() {
    debugger
    // Create a FormData object to send the data, including the file
    let formData = new FormData();
    formData.append("StoreName", storeName.value);
    formData.append("Address", address.value);
    formData.append("Phone", phone.value);
    formData.append("Email", email.value);
    formData.append("WorkingHours", workingHours.value);
    formData.append("Location", storeLocation.value);
    formData.append("ManagerId", localStorage.getItem("managerId"));

    // Handle the file input (storeImage)
    if (storeImage.files.length > 0) {
        formData.append("StoreImageUrl", storeImage.files[0]); // Send the image file
    }

    try {
        let response = await fetch("https://localhost:44398/api/Stores", {
            method: 'POST',
            body: formData // FormData handles file upload and form fields
        });

        if (response.ok) {
            let result = await response.json();
            console.log("Store details successfully saved:", result);
        } else {
            console.error("Failed to save store details:", response.status, response.statusText);
        }
    } catch (error) {
        console.error("Error occurred while saving store details:", error);
    }
}
