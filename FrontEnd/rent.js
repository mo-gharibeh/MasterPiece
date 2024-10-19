// Fetch and display products filtered by category
async function filterByCategory() {
    const selectedCategoryId = document.getElementById("categorySelect").value;
    const productName = document.getElementById("productNameSearch").value.trim().toLowerCase();

    const url = `https://localhost:44398/api/Products/filter?categoryId=${selectedCategoryId}&productName=${productName}`;
    
    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch filtered products");
        return;
    }

    let result = await response.json(); // Parse the response as JSON
    const container = document.getElementById("CardContainer");
    container.innerHTML = ''; // Clear existing products

    result.forEach(element => {
        container.innerHTML += `
        <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.3s">
            <div class="property-item rounded overflow-hidden">
                <div class="position-relative overflow-hidden">
                    <a onclick="saveProductId(${element.productId})">
                        <img class="img-fluid" src="../BackEnd/Motostation/Motostation/Uploads/${element.imageUrl}" alt="">
                    </a>
                    <div class="bg-primary rounded text-white position-absolute start-0 top-0 m-4 py-1 px-3">For ${element.productType}</div>
                    <div class="bg-white rounded-top text-primary position-absolute start-0 bottom-0 mx-4 pt-1 px-3">${element.categoryName}</div>
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

// Search products by name when the button is clicked
async function searchByName() {
    const productName = document.getElementById("productNameSearch").value.trim().toLowerCase();
    const selectedCategoryId = document.getElementById("categorySelect").value;

    const url = `https://localhost:44398/api/Products/filter?categoryId=${selectedCategoryId}&productName=${productName}`;
    
    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch filtered products");
        return;
    }

    let result = await response.json(); // Parse the response as JSON
    const container = document.getElementById("CardContainer");
    container.innerHTML = ''; // Clear existing products

    result.forEach(element => {
        container.innerHTML += `
        <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.3s">
            <div class="property-item rounded overflow-hidden">
                <div class="position-relative overflow-hidden">
                    <a onclick="saveProductId(${element.productId})">
                        <img class="img-fluid" src="../BackEnd/Motostation/Motostation/Uploads/${element.imageUrl}" alt="">
                    </a>
                    <div class="bg-primary rounded text-white position-absolute start-0 top-0 m-4 py-1 px-3">For ${element.productType}</div>
                    <div class="bg-white rounded-top text-primary position-absolute start-0 bottom-0 mx-4 pt-1 px-3">${element.categoryName}</div>
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

// Fetch categories for dropdown
async function fetchCategories() {
    const url = 'https://localhost:44398/api/Categories'; // Replace with your API endpoint for categories
    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch categories");
        return;
    }
    let categories = await response.json();
    const categorySelect = document.getElementById("categorySelect");

    categories.forEach(category => {
        categorySelect.innerHTML += `<option value="${category.categoryId}">${category.categoryName}</option>`;
    });
}

// Initial call to load categories
fetchCategories();
async function applyFilters() {
    const selectedCategoryId = document.getElementById("categorySelect").value;
    const productNameSearch = document.getElementById("productNameSearch").value.trim();

    const url = selectedCategoryId ? 
        `https://localhost:44398/api/Products/category/${selectedCategoryId}` : 
        `https://localhost:44398/api/Products/allProducts`; // Fetch all products if no category is selected

    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch products");
        return;
    }

    let products = await response.json();
    
    // Filter products by name if a search term is provided
    if (productNameSearch) {
        products = products.filter(product => 
            product.productName.toLowerCase().includes(productNameSearch.toLowerCase())
        );
    }

    const container = document.getElementById("CardContainer");
    container.innerHTML = ''; // Clear existing products

    products.forEach(element => { 
        renderProduct(element, container);
    });
}



// // Function to populate the product grid
// function renderProduct(element, container) {
//     container.innerHTML += `
//         <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.3s">
//             <div class="property-item rounded overflow-hidden">
//                 <div class="position-relative overflow-hidden">
//                     <a href="ProductDetails.html"><img class="img-fluid" src="${element.imageUrl}" alt=""></a>
//                     <div class="bg-primary rounded text-white position-absolute start-0 top-0 m-4 py-1 px-3">For ${element.productType}</div>
//                     <div class="bg-white rounded-top text-primary position-absolute start-0 bottom-0 mx-4 pt-1 px-3">${element.category.categoryName}</div>
//                 </div>
//                 <div class="p-4 pb-0">
//                     <h5 class="text-primary mb-3">${element.price} JOD</h5>
//                     <a class="d-block h5 mb-2" href="">${element.productName}</a>
//                 </div>                
//             </div>
//         </div>
//     `;
// }
                
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
