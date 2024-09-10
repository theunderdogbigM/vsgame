document.addEventListener('DOMContentLoaded', () => {
    fetchGames();

    function fetchGames() {
        fetch('/games')
            .then(response => response.json())
            .then(data => {
                const tableBody = document.getElementById('gameTableBody');
                tableBody.innerHTML = '';
                data.forEach(game => {
                    const row = document.createElement('tr');
                    row.innerHTML = `
                        <td>${game.id}</td>
                        <td>${game.name}</td>
                        <td>${game.genre}</td>
                        <td>${game.price}</td>
                        <td>${game.releaseDate}</td>
                        <td>
                            <a href="edit.html?id=${game.id}" class="edit-button">Edit</a>
                            <button class="delete-button" onclick="deleteGame(${game.id})">Delete</button>
                        </td>
                    `;
                    tableBody.appendChild(row);
                });
            });
    }

    window.deleteGame = function(id) {
        fetch(`/games/${id}`, {
            method: 'DELETE'
        }).then(response => {
            if (response.ok) {
                fetchGames();
            }
        });
    };
});
