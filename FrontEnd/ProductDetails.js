async function loadProductDetails() {
    const productId = localStorage.getItem('productId');    
    const url = `https://localhost:44398/api/Products/${productId}`;
    
    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch product details");
        return;
    }
    let categoryName = localStorage.getItem('categoryName');
    const product = await response.json();
    
    // Populate product data into HTML elements
    document.getElementById("productImage").src = product.imageUrl;
    document.getElementById("productName").textContent = product.productName;
    document.getElementById("productCategory").textContent = categoryName ;  // Handle null category
    document.getElementById("productBrand").textContent = product.brand;
    document.getElementById("productCondition").textContent = product.productCondition;
    document.getElementById("productType").textContent = "For " + product.productType;
    document.getElementById("productPrice").textContent = `${product.price} JOD`;
    document.getElementById("stockQuantity").textContent = `${product.stockQuantity} available`;
    document.getElementById("productDescription").textContent = product.description;
    
    // Handle optional fields like rental price and seller info
    if (product.productType === "Rent") {
        document.getElementById("rentalPrice").textContent = `${product.rentalPrice ? product.rentalPrice : "N/A"}/day`;
    } else {
        // Hide rental price if the product is for sale only
        document.getElementById("rentalPriceContainer").style.display = 'none';
    }
    
    // Handle seller info
    if (product.seller) {
        document.getElementById("sellerName").textContent = product.seller.name;
        document.getElementById("sellerContact").textContent = `${product.seller.email} | ${product.seller.phoneNumber}`;
    } else {
        document.getElementById("sellerName").textContent = "Mohammad";
        document.getElementById("sellerContact").textContent = "0778945623";
    }
}

// Call the function to load the product details
loadProductDetails();
