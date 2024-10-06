
const userId = localStorage.getItem("userId");

async function viewProfile(){
    
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
    let post = await response.json();
    console.log("posts : ", post);

    postsContainer.innerHTML = "";
    post.forEach(post => {
        postsContainer.innerHTML += `
        <div class="gallery-item">
            <img src="../BackEnd/Motostation/Motostation/Uploads/${post.imageUrl}" alt="User Photo">
            <div class="gallery-overlay">View Photo</div>
        </div>
        `
    });


}

getPost();


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