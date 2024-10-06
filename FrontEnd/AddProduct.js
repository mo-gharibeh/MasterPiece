document.addEventListener('DOMContentLoaded', function() {
const categoryDropdown = document.getElementById('CategoryID');
const form = document.getElementById('addProductForm');

// Fetch categories from the API
async function loadCategories() {
    try {
    const response = await fetch('https://localhost:44398/api/Categories');
    const data = await response.json();
    // Populate category dropdown
    categoryDropdown.innerHTML = '<option value="" selected>Select Category</option>';
    data.forEach(category => {
        const option = document.createElement('option');
        option.value = category.categoryId;
        option.textContent = category.categoryName;
        categoryDropdown.appendChild(option);
    });
    } catch (error) {
    console.error('Error fetching categories:', error);
    }
}

// Call the function to load categories when the page loads
loadCategories();
let userId = localStorage.getItem('userId');


// Handle form submission
form.addEventListener('submit', async function(event) {
    debugger    

    event.preventDefault();

    // Create FormData object to gather the form data
    const formData = new FormData(form);
    formData.append('SellerId', userId); // Add user ID to the form data

    // try {
    const response = await fetch('https://localhost:44398/api/Products', {
        method: 'POST',
        body: formData,  // Send the form data directly
    });

    if (!response.ok) {
        throw new Error('Failed to add product');
    }

    const result = await response.json();
    console.log('Product added successfully:', result);
    alert('Product added successfully!');
    // } catch (error) {
    // console.error('Error adding product:', error);
    // alert('Error adding product.');
    // }
});
});


    document.getElementById("ProductType").addEventListener("change", function () {
        var rentalDurationDiv = document.getElementById("rentalDurationDiv");
        if (this.value === "Rent") {
            rentalDurationDiv.style.display = "block";
        } else {
            rentalDurationDiv.style.display = "none";
        }
    });
