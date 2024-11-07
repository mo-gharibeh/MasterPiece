
const userId = localStorage.getItem('userId');  


// Fetch and display posts with comments
async function loadPosts() {
    try {
        const response = await fetch('https://localhost:44398/api/Posts/getAllPost'); 
        const posts = await response.json();
        
        const postContainer = document.getElementById('post-container');
        postContainer.innerHTML = ''; 

        posts.forEach(post => {
            // Create post structure
            const postElement = document.createElement('div');
            postElement.classList.add('post');
            
            postElement.innerHTML = `
                <div class="post-header">
                    <img src="../BackEnd/Motostation/Motostation/Uploads/${post.userImage}" alt="User Image" class="user-image" style="width: 50px; height: 50px; border-radius: 50%;">
                    <strong>${post.username}</strong>
                </div>
                <img src="../BackEnd/Motostation/Motostation/Uploads/${post.imageUrl}" alt="Post Image">
                <p>${post.content}</p>
                <div class="like-comment">
                    <p style="font-size:15px">${post.likesCount} Likes ${post.commentsCount} Comments</p>
                    <button class="btn btn-primary" onclick="likePost(${post.postId})">👍 Like</button>
                    <button class="btn btn-link" onclick="showCommentInput(${post.postId})">💬 Comment</button>
                </div>
                <div class="comment-input" id="comment-input-${post.postId}" style="display:none;">
                    <input type="text" class="form-control" placeholder="Write a comment..." onkeydown="addComment(event, ${post.postId})">
                </div>
                <div class="comments" id="comments-container-${post.postId}">
                    <!-- Comments will be loaded here -->
                </div>
            `;

            postContainer.appendChild(postElement);
            loadComments(post.postId); // Load comments for each post
        });
    } catch (error) {
        console.error("Error loading posts:", error);
    }
}

// Fetch and display comments for a post
async function loadComments(postId) {
    try {
        const response = await fetch(`https://localhost:44398/api/Posts/${postId}/comments`);
        const comments = await response.json();
        
        const commentsContainer = document.getElementById(`comments-container-${postId}`);
        commentsContainer.innerHTML = ''; // Clear previous comments

        comments.forEach(comment => {
            const commentElement = document.createElement('div');
            commentElement.classList.add('comment');
            commentElement.innerHTML = `
                <img src="../BackEnd/Motostation/Motostation/Uploads/${comment.userImage}" alt="Commenter Image" class="comment-user-image" style="width: 30px; height: 30px; border-radius: 50%;" />
                <strong class="comment-user-name">${comment.username}</strong>
                <div style = "display: flex ;justify-content: space-between">
                    <p>${comment.commentText}</p>
                    <a onclick="deleteComment(${comment.commentId},${comment.userId},${postId})" class="fas fa-trash"></a>                                       
                </div>
                <hr>
            `;
            commentsContainer.appendChild(commentElement);
        });
    } catch (error) {
        console.error("Error loading comments:", error);
    }
}

async function deleteComment( commentId, commentUserId, postId){
    if (userId == commentUserId){
    alert("Are you sure you want to delete this comment?");
    // Implement delete comment logic here
    const url = `https://localhost:44398/api/Posts/comments/${commentId}`
    try {
        const response = await fetch(url, {
            method: 'DELETE'
        });
        if (response.ok) {
            // alert("Comment deleted successfully.");
            loadComments(postId); // Refresh comments to remove deleted comment
        } else 
        {
            alert("Failed to delete comment.");
        }
    } catch (error) {
        console.error("Error deleting comment:", error);
        alert("Failed to delete comment.");
    }
    }else{
        alert("You cannot delete this comment.");
    }
}
// Add a like to a post
async function likePost(postId) {
    const userId = localStorage.getItem('userId');
    if (!userId) {
        alert("User not logged in");
        return;
    }

    try {
        await fetch(`https://localhost:44398/api/Posts/addLike`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ postId, userId })
        });
        loadPosts(); // Refresh posts to show updated likes count
    } catch (error) {
        console.error("Error liking post:", error);
    }
}

// Show comment input box
function showCommentInput(postId) {
    document.getElementById(`comment-input-${postId}`).style.display = 'block';
}

// Function to add a comment to a post
async function addComment(event, postId) {
    if (event.key !== 'Enter') return; // Trigger only on "Enter" key

    const userId = localStorage.getItem('userId');
    const commentText = event.target.value;

    if (!userId || !commentText) {
        alert("User not logged in or comment is empty");
        return;
    }

    try {
        const response = await fetch('https://localhost:44398/api/Posts/addComment', { 
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                postId: postId,
                userId: userId,
                commentText: commentText
            })
        });

        if (response.ok) {
            event.target.value = ''; // Clear the input
            loadPosts(); // Reload posts to show the new comment
        } else {
            const errorData = await response.json();
            console.error("Error adding comment:", errorData.message);
        }
    } catch (error) {
        console.error("Error adding comment:", error);
    }
}


// Initialize posts on page load
document.addEventListener('DOMContentLoaded', loadPosts);
