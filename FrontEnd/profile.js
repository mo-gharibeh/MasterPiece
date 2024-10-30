
const userId = localStorage.getItem("userId");

async function viewProfile(){
    debugger
    let coverImage = document.getElementById("coverImage");
    let profileImage = document.getElementById("profileImage");
    let name = document.getElementById("name");


    let response = await fetch(`https://localhost:44398/api/Users/${userId}`);
    if (!response.ok) {
        console.error("Failed to fetch user profile");
        return;
    }
    let user = await response.json();
    coverImage.src = "";
    coverImage.src =  `../BackEnd/Motostation/Motostation/Uploads/${user.coverImageUrl}`;
    profileImage.src = "";
    profileImage.src =  `../BackEnd/Motostation/Motostation/Uploads/${user.profileImageUrl}`;
    name.textContent = "";
    name.textContent = user.fullName;
    console.log(user);

}

viewProfile();


async function getPost() {
    let postsContainer = document.getElementById("postsContainer");

    let response = await fetch(`https://localhost:44398/api/Users/posts/userId/${userId}`);
    if (!response.ok) {
        console.error("Failed to fetch post");
        return;
    }
    let posts = await response.json();
    console.log("posts:", posts);

    postsContainer.innerHTML = "";
    posts.forEach(post => {
        postsContainer.innerHTML += `
        <div class="gallery-item">
            <img src="../BackEnd/Motostation/Motostation/Uploads/${post.imageUrl}" alt="User Photo" >
            <div onclick="openModal('../../BackEnd/Motostation/Motostation/Uploads/${post.imageUrl}', '${post.postId}')" class="gallery-overlay">View Photo</div>

        </div>
        `;
    });
}


getPost();

function editPhoto(postId) {
    // Code to edit the photo, for example, opening an edit form
    localStorage.setItem('postId', postId);
    console.log("Edit post with ID:", postId);
}

async function deletePhoto(postId) {
    if (confirm("Are you sure you want to delete this photo?")) {
        // Code to delete the photo
        let response = await fetch(`https://localhost:44398/api/Users/DeletePost?id=${postId}`, {
            method: "DELETE",
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if (response.ok) {
            alert("Photo deleted successfully!");
        } else {
            alert("Failed to delete photo.");
        }
        closeModal();
        getPost(); // Refresh the posts list after deletion
        
    
        console.log("Delete post with ID:", postId);
    }
}

function openModal(imageUrl, postId) {
    debugger 

    const modal = document.getElementById("imageModal");
    const modalImage = document.getElementById("modalImage");

    modal.style.display = "block";

    modalImage.src = imageUrl;

    document.getElementById("btns").innerHTML += `            
                <button onclick="editPhoto(${postId})" class="edit-button" data-bs-toggle="modal" data-bs-target="#editPostModal" >Edit</button>
                <button onclick="deletePhoto(${postId})" class="delete-button">Delete</button>
            `
}

function closeModal() {
    debugger 

    document.getElementById("btns").innerHTML = '';

    document.getElementById("imageModal").style.display = "none";
}

// function for edit and fetch data by API

async function editPost(postId) {
    debugger
    // Create FormData and append the post data
    let formData = new FormData();
    formData.append("Content", document.getElementById("editContent").value);
    formData.append("ImageUrl", document.getElementById("editImageFile").files[0]);

    // Perform the fetch request
    let response = await fetch(`https://localhost:44398/api/Users/editPost/${postId}`, {
        method: "PUT",
        body: formData // No need to set Content-Type; FormData handles it
    });

    if (!response.ok) {
        console.error("Failed to edit post");
        return;
    }

    let post = await response.json();
    console.log("Edited post: ", post);

    // Refresh posts, close modal, and show success message
    getPost();
    closeModal();
    alert("Post edited successfully!");
}



// For Adding a new post








// async function addPost() {
//     let title = document.getElementById("title").value;
//     let description = document.getElementById("description").value;
//     let image = document.getElementById("image").files[0];

//     let formData = new FormData();
//     formData.append("title", title);
//     formData.append("description", description);
//     formData.append("image", image);

//     let response = await fetch(`https://localhost:44398/api/Users/posts/userId/${userId}`, {
//         method: "POST",
//         body: formData
//     });

//     if (!response.ok) {
//         console.error("Failed to add post");
//         return;
//     }

//     let post = await response.json();
//     console.log("New post added: ", post);

//     // Clear form inputs
//     document.getElementById("title").value = "";
//     document.getElementById("description").value = "";
//     document.getElementById("image").value = "";

    
// }