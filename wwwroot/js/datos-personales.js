const loadingMessage = document.querySelector("#loading-message");
const errorMessage = document.querySelector("#error-message");
const personalData = document.querySelector("#personal-data");

async function loadPersonalData() {
    try {
        const response = await fetch("/api/datosPersonales");

        if (!response.ok) {
            throw new Error(`La API respondió con el estado ${response.status}.`);
        }

        const data = await response.json();
        document.querySelector("#name").textContent = data.nombre;
        document.querySelector("#birth-date").textContent = data.fecha_nacimiento;
        document.querySelector("#address").textContent = data.direccion;
        personalData.hidden = false;
    } catch (error) {
        errorMessage.textContent = "No se pudieron cargar los datos personales. Comprueba que la API esté en ejecución e inténtalo de nuevo.";
        errorMessage.hidden = false;
    } finally {
        loadingMessage.hidden = true;
    }
}

loadPersonalData();
