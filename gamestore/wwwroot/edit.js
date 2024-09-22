document.addEventListener('DOMContentLoaded', () => {
    const urlParams = new URLSearchParams(window.location.search);
    const gameId = urlParams.get('id');

    if (gameId) {
        fetch(`/games/${gameId}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('gameId').value = data.id;
                document.getElementById('name').value = data.name;
                document.getElementById('genre').value = data.genreId;
                document.getElementById('price').value = data.price;
                document.getElementById('releaseDate').value = data.releaseDate;
            });
    }
    function logoutUser() {
        // Remove the token from localStorage
        localStorage.removeItem('authToken');

        // Optionally, redirect to the login page or home page
        window.location.href = 'welcome.html';
    }

    // Attach the event listener to the logout button
    document.getElementById('logoutButton').addEventListener('click', logoutUser);
    document.getElementById('editForm').addEventListener('submit', (event) => {
        event.preventDefault();

        const formData = new FormData(event.target);
        const game = {
            id: formData.get('id'),
            name: formData.get('name'),
            genreId: formData.get('genre'),
            price: formData.get('price'),
            releaseDate: formData.get('releaseDate')
        };

        fetch(`/games/${game.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(game)
        }).then(response => {
            if (response.ok) {
                window.location.href = 'index.html';
            }
        });
    });
});
