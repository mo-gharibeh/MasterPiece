async function GitAllCategories(){
    debugger
    
    let response = await fetch("https://localhost:44398/api/Categories");
    let result = await response.json();

    console.log(result);

    var CardContainer = document.getElementById("CardContainer");

    result.forEach(element =>{
        CardContainer.innerHTML +=
        `
        <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.3s">
            <div class="property-item rounded overflow-hidden">
                <div class="position-relative overflow-hidden">
                    <a onclick="saveCategoryId(${element.categoryId}, '${element.categoryName}')"><img class="img-fluid" src="../BackEnd/Motostation/Motostation/Uploads/${element.imageUrl}" alt=""></a>
                    <!-- <div class="bg-primary rounded text-white position-absolute start-0 top-0 m-4 py-1 px-3">For Sell</div> -->
                    <div class="bg-white rounded-top text-primary position-absolute start-0 bottom-0 mx-4 pt-1 px-3">${element.categoryName}</div>
                </div>
                <div class="p-4 pb-0">
                    <!-- <h5 class="text-primary mb-3">$3,345</h5> -->
                    <a class="d-block h5 mb-2" onclick="saveCategoryId(${element.categoryId}, ${element.categoryName})">${element.description}</a>
                    <!-- <p><i class="fa fa-map-marker-alt text-primary me-2"></i>123 Street, Amman, Jordan</p> -->
                </div>                                    
            </div>
        </div>
        `
    })

}

GitAllCategories();

function saveCategoryId(id, name){
    localStorage.setItem("categoryId", id);
    localStorage.setItem("categoryName", name);
    window.location.href = "rent.html";  // Redirect to Property Listing page after saving category id in local storage.
}

async function GitAllContact(){
    debugger
    
    let response = await fetch("https://localhost:44398/api/ContactMessage");
    let result = await response.json();

    console.log(result);

    var TestimonialContainer = document.getElementById("TestimonialContainer");

    result.forEach(element =>{
        TestimonialContainer.innerHTML +=
        `
        <div class="testimonial-item bg-light rounded p-3">
            <div class="bg-white border rounded p-4">
                <p>${element.content}</p>
                <div class="d-flex align-items-center">
                    <img class="img-fluid flex-shrink-0 rounded" src="img/testimonial-1.jpg" style="width: 45px; height: 45px;">
                    <div class="ps-3">
                        <h6 class="fw-bold mb-1">${element.name}</h6>
                        <small>${element.subject}</small>
                    </div>
                </div>
            </div>
        </div>
        `
    })
    

}

GitAllContact();