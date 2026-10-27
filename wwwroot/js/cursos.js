const loadingMessage = document.querySelector("#loading-message");
const errorMessage = document.querySelector("#error-message");
const statusMessage = document.querySelector("#status-message");
const tableContainer = document.querySelector("#courses-table-container");
const tableBody = document.querySelector("#courses-table-body");
const courseForm = document.querySelector("#course-form");
const submitCourseButton = document.querySelector("#submit-course-button");
const cancelEditButton = document.querySelector("#cancel-edit-button");
let editingCourseId = null;

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

    if (courses.length === 0) {
        const row = document.createElement("tr");
        const cell = document.createElement("td");
        cell.colSpan = 8;
        cell.textContent = "Todavía no hay cursos registrados.";
        row.append(cell);
        tableBody.append(row);
    }

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
        actionCell.className = "table-actions";
        const editButton = document.createElement("button");
        editButton.type = "button";
        editButton.textContent = "Editar";
        editButton.setAttribute("aria-label", `Editar curso ${course.nombre}`);
        editButton.addEventListener("click", () => editCourse(course));
        const deleteButton = document.createElement("button");
        deleteButton.type = "button";
        deleteButton.textContent = "Eliminar";
        deleteButton.setAttribute("aria-label", `Eliminar curso ${course.nombre}`);
        deleteButton.addEventListener("click", () => deleteCourse(course.id));
        actionCell.append(editButton);
        actionCell.append(deleteButton);
        row.append(actionCell);
        tableBody.append(row);
    }

    tableContainer.hidden = false;
}

function editCourse(course) {
    editingCourseId = course.id;
    courseForm.elements.namedItem("codigo").value = course.codigo;
    courseForm.elements.namedItem("nombre").value = course.nombre;
    courseForm.elements.namedItem("area").value = course.area;
    courseForm.elements.namedItem("horas").value = course.horas;
    courseForm.elements.namedItem("cupoMaximo").value = course.cupoMaximo;
    courseForm.elements.namedItem("fechaInicio").value = course.fechaInicio;
    submitCourseButton.textContent = "Guardar cambios";
    cancelEditButton.hidden = false;
    courseForm.scrollIntoView({ behavior: "smooth", block: "start" });
}

function resetCourseForm() {
    courseForm.reset();
    editingCourseId = null;
    submitCourseButton.textContent = "Crear curso";
    cancelEditButton.hidden = true;
}

async function getApiError(response, fallback) {
    if (response.status === 404) {
        return "El curso no existe o ya fue eliminado.";
    }

    if (response.status === 400) {
        try {
            const problem = await response.json();
            const validationErrors = Object.values(problem.errors ?? {}).flat();
            return validationErrors.length > 0
                ? validationErrors.join(" ")
                : "Revisa los datos ingresados e inténtalo de nuevo.";
        } catch {
            return "Revisa los datos ingresados e inténtalo de nuevo.";
        }
    }

    return fallback;
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
    const course = Object.fromEntries(formData.entries());
    course.horas = Number(course.horas);
    course.cupoMaximo = Number(course.cupoMaximo);

    try {
        const isEditing = editingCourseId !== null;
        const response = await fetch(
            isEditing ? `/api/cursos/${editingCourseId}` : "/api/cursos",
            {
                method: isEditing ? "PUT" : "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(course)
            }
        );

        if (!response.ok) {
            showError(await getApiError(response, "No se pudo guardar el curso. Inténtalo de nuevo."));
            return;
        }

        resetCourseForm();
        if (await loadCourses()) {
            showStatus(isEditing ? "El curso se actualizó correctamente." : "El curso se creó correctamente.");
        }
    } catch (error) {
        showError("No se pudo guardar el curso. Comprueba la conexión e inténtalo de nuevo.");
    }
}

async function deleteCourse(id) {
    errorMessage.hidden = true;
    statusMessage.hidden = true;

    try {
        const response = await fetch(`/api/cursos/${id}`, { method: "DELETE" });

        if (!response.ok) {
            showError(await getApiError(response, "No se pudo eliminar el curso. Inténtalo de nuevo."));
            return;
        }

        if (await loadCourses()) {
            showStatus("El curso se eliminó correctamente.");
        }
    } catch (error) {
        showError("No se pudo eliminar el curso. Inténtalo de nuevo.");
    }
}

courseForm.addEventListener("submit", createCourse);
cancelEditButton.addEventListener("click", resetCourseForm);
loadCourses();
