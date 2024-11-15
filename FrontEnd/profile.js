
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

    if (posts.length === 1) {
        postsContainer.classList.add("single-photo");
    } else {
        postsContainer.classList.remove("single-photo");
    }

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
    // Store the postId in localStorage (or sessionStorage) to retain it across pages/modal
    localStorage.setItem('postId', postId);

    // Fetch the current post data
    fetch(`https://localhost:44398/api/Users/getPost/${postId}`)
        .then(response => response.json())
        .then(post => {
            // Populate the modal with the current post's data
            document.getElementById("editContent").value = post.content; // Set current content in textarea
            document.getElementById("currentImage").src = `../BackEnd/Motostation/Motostation/Uploads/${post.imageUrl}`; // Show current image

            // If you have an image input (to upload a new one), it will stay empty until the user selects a new image
        })
        .catch(err => console.error("Error fetching post:", err));
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
        closeModal();
        getPost(); // Refresh the posts list after deletion
        // if (response.ok) {
        //     alert("Photo deleted successfully!");
        // } else {
        //     alert("Failed to delete photo.");
        // }
        
    
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




async function editPost(e) {
    e.preventDefault(); 
    debugger

    const form = document.getElementById("editPostForm");
    const formData = new FormData(form);

    const postId = localStorage.getItem('postId'); // Get the postId from localStorage
    const apiEndpoint = `https://localhost:44398/api/Users/editPost/${postId}`;

    try {
        const response = await fetch(apiEndpoint, {
            method: "PUT",
            body: formData
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error("Error:", errorData);
            alert("Failed to update post: " + (errorData.message || "An error occurred"));
            return;
        }

        const result = await response.json();
        console.log("Success:", result);
        alert(result.message || "Post updated successfully!");

        // if (formData.get("imageUrl")) {
        //     const newImageUrl = URL.createObjectURL(formData.get("imageUrl"));
        //     document.getElementById("currentImage").src = newImageUrl;
        // }

        // إغلاق المودال بعد الحفظ
        const modal = new bootstrap.Modal(document.getElementById("editPostModal"));
        modal.hide();
    } catch (error) {
        console.error("Fetch error:", error);
        alert("An error occurred while updating the post.");
    }
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