
// Get All Shops from the database and return them as a dynamic cards   

async function GetAllStores(){
    debugger
    const response = await fetch('https://localhost:44398/api/Stores/GetAllStores');
    if (!response.ok) {
        console.error("Failed to fetch stores");
        return;
    }
    const result = await response.json();

    const container = document.getElementById("CardContainer");

    container.innerHTML = ''; // Clear existing products

    result.forEach(element => { // Iterate over the parsed JSON array
        container.innerHTML += `
        <div class="col-md-4">
            <div class="card mb-4 shop-card">
                <img class="card-img-top" style="height: 300px;" src="../BackEnd/Motostation/Motostation/Uploads/${element.storeImageUrl}" alt="Shop Image">
                <div class="card-body">
                    <h5 class="card-title">${element.storeName}</h5>
                    <p class="card-text"><i class="fas fa-map-marker-alt"></i>${element.address}</p>
                    <p class="card-text"><i class="fas fa-clock"></i> ⏰ ${element.workingHours}</p>
                    <a onclick="saveStoreId(${element.storeId})" class="btn btn-primary">View Details</a>
                </div>
            </div>
        </div>
        `;
    });   
}

function saveStoreId(storeId) {
    localStorage.setItem("storeId", storeId); 
    window.location.href = "shopDetails.html"; // Redirect to the shop details page
}
GetAllStores();



    
    
















