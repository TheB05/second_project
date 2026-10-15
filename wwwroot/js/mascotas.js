const loadingMessage = document.querySelector("#loading-message");
const errorMessage = document.querySelector("#error-message");
const tableContainer = document.querySelector("#pets-table-container");
const tableBody = document.querySelector("#pets-table-body");

async function loadPets() {
    try {
        const response = await fetch("/api/mascotas");

        if (!response.ok) {
            throw new Error(`La API respondió con el estado ${response.status}.`);
        }

        const pets = await response.json();

        for (const pet of pets) {
            const row = document.createElement("tr");

            for (const value of [pet.id, pet.nombre, pet.edad, pet.raza]) {
                const cell = document.createElement("td");
                cell.textContent = value;
                row.append(cell);
            }

            tableBody.append(row);
        }

        tableContainer.hidden = false;
    } catch (error) {
        errorMessage.textContent = "No se pudo cargar la lista de mascotas. Comprueba que la API esté en ejecución e inténtalo de nuevo.";
        errorMessage.hidden = false;
    } finally {
        loadingMessage.hidden = true;
    }
}

loadPets();
