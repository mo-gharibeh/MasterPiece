    document.querySelector('form').addEventListener('submit', async function(event) {
        event.preventDefault(); // Prevent the default form submission

        debugger
        const name = document.getElementById('name').value;
        const email = document.getElementById('email').value;
        const subject = document.getElementById('subject').value;
        const message = document.getElementById('message').value;

        const contactMessage = {
            name: name,
            email: email,
            subject: subject,
            content: message
        };

        const response = await fetch('https://localhost:44398/api/ContactMessages', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(contactMessage)
        });

        if (response.ok) {
            alert('Message sent successfully!');
            // Optionally, clear the form fields
            document.querySelector('form').reset();
        } else {
            alert('Failed to send message. Please try again later.');
        }
    });
