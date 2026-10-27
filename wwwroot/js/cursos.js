const loadingMessage = document.querySelector("#loading-message");
const errorMessage = document.querySelector("#error-message");
const statusMessage = document.querySelector("#status-message");
const tableContainer = document.querySelector("#courses-table-container");
const tableBody = document.querySelector("#courses-table-body");
const courseForm = document.querySelector("#course-form");

function showError(message) {
    errorMessage.textContent = message;
    errorMessage.hidden = false;
    statusMessage.hidden = true;
}

function showStatus(message) {
    statusMessage.textContent = message;
    statusMessage.hidden = false;
    errorMessage.hidden = true;
}

function renderCourses(courses) {
    tableBody.replaceChildren();

    for (const course of courses) {
        const row = document.createElement("tr");

        for (const value of [
            course.id,
            course.codigo,
            course.nombre,
            course.area,
            course.horas,
            course.cupoMaximo,
            course.fechaInicio
        ]) {
            const cell = document.createElement("td");
            cell.textContent = value;
            row.append(cell);
        }

        const actionCell = document.createElement("td");
        const deleteButton = document.createElement("button");
        deleteButton.type = "button";
        deleteButton.textContent = "Eliminar";
        deleteButton.setAttribute("aria-label", `Eliminar curso ${course.nombre}`);
        deleteButton.addEventListener("click", () => deleteCourse(course.id));
        actionCell.append(deleteButton);
        row.append(actionCell);
        tableBody.append(row);
    }

    tableContainer.hidden = false;
}

async function loadCourses() {
    loadingMessage.hidden = false;
    errorMessage.hidden = true;

    try {
        const response = await fetch("/api/cursos");

        if (!response.ok) {
            throw new Error(`La API respondió con el estado ${response.status}.`);
        }

        renderCourses(await response.json());
        return true;
    } catch (error) {
        showError("No se pudieron cargar los cursos. Comprueba que la API esté en ejecución e inténtalo de nuevo.");
        return false;
    } finally {
        loadingMessage.hidden = true;
    }
}

async function createCourse(event) {
    event.preventDefault();
    errorMessage.hidden = true;
    statusMessage.hidden = true;

    const formData = new FormData(courseForm);
    const course = {
        codigo: formData.get("codigo"),
        nombre: formData.get("nombre"),
        area: formData.get("area"),
        horas: Number(formData.get("horas")),
        cupoMaximo: Number(formData.get("cupoMaximo")),
        fechaInicio: formData.get("fechaInicio")
    };

    try {
        const response = await fetch("/api/cursos", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(course)
        });

        if (!response.ok) {
            throw new Error(`La API respondió con el estado ${response.status}.`);
        }

        courseForm.reset();
        if (await loadCourses()) {
            showStatus("El curso se creó correctamente.");
        }
    } catch (error) {
        showError("No se pudo crear el curso. Revisa los datos e inténtalo de nuevo.");
    }
}

async function deleteCourse(id) {
    errorMessage.hidden = true;
    statusMessage.hidden = true;

    try {
        const response = await fetch(`/api/cursos/${id}`, { method: "DELETE" });

        if (!response.ok) {
            throw new Error(`La API respondió con el estado ${response.status}.`);
        }

        if (await loadCourses()) {
            showStatus("El curso se eliminó correctamente.");
        }
    } catch (error) {
        showError("No se pudo eliminar el curso. Inténtalo de nuevo.");
    }
}

courseForm.addEventListener("submit", createCourse);
loadCourses();
