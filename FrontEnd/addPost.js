
// var post = new Post
// {
//     UserId = postDto.UserId,
//     Content = postDto.Content,
//     ImageUrl = postDto.ImageUrl.FileName, // Save only the file name in the database
// };

let userId = localStorage.getItem('userId');
document.getElementById('postForm').addEventListener('submit', async function(event) {
    event.preventDefault(); // Prevent the default form submission

    debugger
    
    const form = document.getElementById('postForm');
    const formData = new FormData(form); // Automatically handles both text and file inputs
    // add the userId to the formData object
    formData.append('UserId', userId);
    const url = "https://localhost:44398/api/Users/addPost";
    try {
        const response = await fetch(url, {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error('Failed to create post');
        }

        const data = await response.json();
        console.log('Post created:', data);
        alert('Post created successfully!');
        form.reset(); // Reset the form after successful submission
    } catch (error) {
        console.error('Error creating post:', error);
        alert('Failed to create post.');
    }
});
