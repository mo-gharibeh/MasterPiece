// Function to populate the product grid
function renderProduct(element, container) {
    container.innerHTML += `
        <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.3s">
            <div class="property-item rounded overflow-hidden">
                <div class="position-relative overflow-hidden">
                    <a href="ProductDetails.html"><img class="img-fluid" src="${element.imageUrl}" alt=""></a>
                    <div class="bg-primary rounded text-white position-absolute start-0 top-0 m-4 py-1 px-3">For ${element.productType}</div>
                    <div class="bg-white rounded-top text-primary position-absolute start-0 bottom-0 mx-4 pt-1 px-3">${element.category.categoryName}</div>
                </div>
                <div class="p-4 pb-0">
                    <h5 class="text-primary mb-3">${element.price}JOD</h5>
                    <a class="d-block h5 mb-2" href="">${element.productName}</a>
                </div>                
            </div>
        </div>
    `;
}
                
                // <p><i class="fa fa-map-marker-alt text-primary me-2"></i>123 Street, Amman, Jordan</p>

// Fetch and display products by category
async function getProductsByCategoryId() {
    debugger;
    const selectedCategoryId = localStorage.getItem("categoryId");
    const categoryName = localStorage.getItem("categoryName");
    if (!selectedCategoryId) return;

    const url = `https://localhost:44398/api/Products/category/${selectedCategoryId}`;
    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch products for category");
        return;
    }

    let result = await response.json(); // Parse the response as JSON

    const container = document.getElementById("CardContainer");
    container.innerHTML = ''; // Clear existing products

    result.forEach(element => { // Iterate over the parsed JSON array
        container.innerHTML += `
        <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.3s">
            <div class="property-item rounded overflow-hidden">
                <div class="position-relative overflow-hidden">
                    <a onclick="saveProductId(${element.productId})"><img class="img-fluid" src="../BackEnd/Motostation/Motostation/Uploads/${element.imageUrl}" alt=""></a>
                    <div class="bg-primary rounded text-white position-absolute start-0 top-0 m-4 py-1 px-3">For ${element.productType}</div>
                    <div class="bg-white rounded-top text-primary position-absolute start-0 bottom-0 mx-4 pt-1 px-3">${categoryName}</div>
                </div>
                <div class="p-4 pb-0">
                    <h5 class="text-primary mb-3">${element.price} JOD</h5>
                    <a class="d-block h5 mb-2" onclick="saveProductId(${element.productId})">${element.productName}</a>
                </div>                
            </div>
        </div>
    `;
    });
}

// Function to save productId and redirect
function saveProductId(productId) {
    localStorage.setItem("productId", productId); // Save productId in local storage
    window.location.href = "ProductDetails.html"; // Redirect to the product details page
}

getProductsByCategoryId();
