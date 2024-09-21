// Handle form submission
document.getElementById('createUserForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Prevent form from submitting the traditional way

    // Get form values
    const name = document.getElementById('name').value.trim();
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value.trim();
    const activity = document.getElementById('activity').value.trim();

    // Client-side validation
    if (!name || !email || !password) {
        displayMessage('Please fill out all required fields.', 'error');
        return;
    }

    // Basic email validation (regex)
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(email)) {
        displayMessage('Please enter a valid email address.', 'error');
        return;
    }

    // Password strength validation (at least 6 characters as an example)
    if (password.length < 6) {
        displayMessage('Password must be at least 6 characters long.', 'error');
        return;
    }

    // Create user object
    const user = {
        name: name,
        email: email,
        password: password,
        activity: activity || null // If activity is not provided, set it to null
    };

    try {
        // Send user data to the backend via POST request
        const response = await fetch('/users', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        });

        // Check if the response is OK
        if (!response.ok) {
            const errorData = await response.json(); // Parse error message from backend
            const errorMessage = errorData.message || 'Failed to create user';
            throw new Error(errorMessage);
        }

        const result = await response.json();

        // Display success message
        displayMessage('User created successfully!', 'success');
        clearForm();
    } catch (error) {
        displayMessage('Error: ' + error.message, 'error');
    }
});

// Function to display result messages
function displayMessage(message, type) {
    const resultMessage = document.getElementById('resultMessage');
    resultMessage.textContent = message;
    resultMessage.className = type; // Use the type ('success' or 'error') to apply specific styles
}

// Function to clear form after successful submission
function clearForm() {
    document.getElementById('createUserForm').reset();
}
