
// var post = new Post
// {
//     UserId = postDto.UserId,
//     Content = postDto.Content,
//     ImageUrl = postDto.ImageUrl.FileName, // Save only the file name in the database
// };
let userId = localStorage.getItem('userId');

document.getElementById('postForm').addEventListener('submit', async function(event) {
    event.preventDefault(); 

    const form = document.getElementById('postForm');
    const formData = new FormData(form);

    const userId = localStorage.getItem('userId');
    if (!userId) {
        alert('Please login first.');
        return;
    }
    formData.append('UserId', userId);

    try {
       
        const response = await fetch('https://localhost:44398/api/Users/addPost', {
            method: 'POST',
            body: formData,
        });

        if (!response.ok) {
            const errorMessage = await response.text();
            alert(`Failed to add post: ${errorMessage}`);
            return;
        }

        const result = await response.json();
        alert(result.message); 
        form.reset(); 
        location.href = "profile.html"; 
    } catch (error) {
        alert(`An error occurred: ${error.message}`);
    }
});
