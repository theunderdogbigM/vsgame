document.addEventListener('DOMContentLoaded', () => {
    fetchGames();


    function logoutUser() {
        // Remove the token from localStorage
        localStorage.removeItem('authToken');

        // Optionally, redirect to the login page or home page
        window.location.href = 'welcome.html';
    }

    // Attach the event listener to the logout button
    document.getElementById('logoutButton').addEventListener('click', logoutUser);
    function fetchGames() {
        fetch('/games')
            .then(response => response.json())
            .then(data => {
                console.log(data); // Add this line to inspect the response in the console
                const tableBody = document.getElementById('gameTableBody');
                tableBody.innerHTML = '';
                data.forEach(game => {
                    const releaseDate = game.releaseDate || 'N/A';
                    const row = document.createElement('tr');
                    row.innerHTML = `
                        <td>${game.id}</td>
                        <td>${game.name}</td>
                        <td>${game.genre}</td>
                        <td>${game.price}</td>
                        <td>${releaseDate}</td>
                        <td>
                            <a href="edit.html?id=${game.id}" class="edit-button">Edit</a>
                            <button class="delete-button" onclick="deleteGame(${game.id})">Delete</button>
                        </td>
                    `;
                    tableBody.appendChild(row);
                });
            })
            
            .catch(error => {
                console.error('Error fetching games:', error);
            });
    }

    window.deleteGame = function(id) {
        if (confirm('Are you sure you want to delete this game?')) {
            fetch(`/games/${id}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    fetchGames(); // Refresh the game list after successful deletion
                } else {
                    alert('Failed to delete the game');
                }
            }).catch(error => {
                console.error('Error:', error);
                alert('An error occurred while deleting the game');
            });
        }
    };
});

document.getElementById('deleteUserButton').addEventListener('click', async function() {
    const emailToDelete = document.getElementById('emailToDelete').value; // Email from input
    const token = localStorage.getItem('authToken'); // Token from localStorage

    if (emailToDelete && token) {
        const confirmDelete = confirm("Are you sure you want to delete your account? This action cannot be undone.");

        if (confirmDelete) {
            try {
                // First, fetch the user details by email to get the user ID
                const usersResponse = await fetch(`/users`);
                const users = await usersResponse.json();

                // Find the user by email
                const user = users.find(u => u.email === emailToDelete);
                if (!user) {
                    alert('User not found');
                    return;
                }

                const userId = user.id;

                // Now make the delete request using the user ID
                const response = await fetch(`/users/${userId}`, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}` // Include auth token
                    }
                });

                if (response.ok) {
                    alert('Your account has been deleted successfully.');
                    localStorage.removeItem('authToken'); // Remove token
                    localStorage.removeItem('userId'); // Clear user ID
                    window.location.href = 'welcome.html'; // Redirect to welcome page
                } else {
                    const errorMessage = await response.text(); // Get error message from response
                    alert('Failed to delete account: ' + errorMessage);
                }
            } catch (error) {
                console.error('Error:', error);
                alert('An error occurred while deleting your account.');
            }
        }
    } else {
        alert('Email and token are required. Please log in again.');
    }
});




