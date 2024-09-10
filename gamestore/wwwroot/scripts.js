document.addEventListener('DOMContentLoaded', () => {
    fetchGames();

    function fetchGames() {
        fetch('/games')
            .then(response => response.json())
            .then(data => {
                const tableBody = document.getElementById('gameTableBody');
                tableBody.innerHTML = '';
                data.forEach(game => {
                    // Attempt to parse releaseDate as a date and format it, fallback to 'N/A'
                    const releaseDate = game.releaseDate ? new Date(game.releaseDate).toISOString().split('T')[0] : 'N/A';
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
