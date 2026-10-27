const studentForm = document.querySelector("#student-form");
const studentSelect = document.querySelector("#student-select");
const courseSelect = document.querySelector("#course-select");
const enrollmentForm = document.querySelector("#enrollment-form");
const enrollButton = document.querySelector("#enroll-button");
const capacityMessage = document.querySelector("#capacity-message");
const enrolledStudentsMessage = document.querySelector("#enrolled-students-message");
const enrolledStudentsList = document.querySelector("#enrolled-students-list");
const studentErrorMessage = document.querySelector("#student-error-message");
const studentStatusMessage = document.querySelector("#student-status-message");
const enrollmentErrorMessage = document.querySelector("#enrollment-error-message");
const enrollmentStatusMessage = document.querySelector("#enrollment-status-message");
let courses = [];
let students = [];
let enrolledByCourse = new Map();

function showMessage(element, message) {
    element.textContent = message;
    element.hidden = false;
}

function clearMessage(element) {
    element.textContent = "";
    element.hidden = true;
}

async function fetchJson(url, options) {
    const response = await fetch(url, options);
    const body = response.status === 204 ? null : await response.json();

    if (!response.ok) {
        const error = new Error(
            body?.detail ?? body?.title ?? `La API respondió con el estado ${response.status}.`
        );
        error.status = response.status;
        error.validationErrors = Object.values(body?.errors ?? {}).flat();
        throw error;
    }

    return body;
}

function appendOptions(select, items, placeholder, format) {
    select.replaceChildren(new Option(placeholder, ""));
    for (const item of items) {
        select.add(new Option(format(item), item.id));
    }
}

function renderCourses() {
    appendOptions(
        courseSelect,
        courses,
        courses.length ? "Selecciona un curso" : "No hay cursos disponibles",
        course => {
            const ocupados = enrolledByCourse.get(course.id)?.length ?? 0;
            const disponibles = Math.max(course.cupoMaximo - ocupados, 0);
            return `${course.codigo} · ${course.nombre} — ${disponibles} cupos disponibles`;
        }
    );
    courseSelect.disabled = courses.length === 0;
    updateCourseDetails();
}

function renderStudents() {
    appendOptions(
        studentSelect,
        students,
        students.length ? "Selecciona un estudiante" : "No hay estudiantes registrados",
        student => `${student.matricula} · ${student.nombre}`
    );
    studentSelect.disabled = students.length === 0;
}

function updateCourseDetails() {
    const course = courses.find(item => item.id === Number(courseSelect.value));
    enrolledStudentsList.replaceChildren();
    enrolledStudentsList.hidden = true;

    if (!course) {
        capacityMessage.textContent = "Selecciona un curso para consultar sus cupos.";
        enrolledStudentsMessage.textContent = "Selecciona un curso para ver sus estudiantes inscritos.";
        enrollButton.disabled = true;
        return;
    }

    const enrolled = enrolledByCourse.get(course.id) ?? [];
    const disponibles = Math.max(course.cupoMaximo - enrolled.length, 0);
    capacityMessage.textContent = `Cupos disponibles: ${disponibles} de ${course.cupoMaximo}.`;
    enrollButton.disabled = disponibles === 0 || students.length === 0;

    if (enrolled.length === 0) {
        enrolledStudentsMessage.textContent = "Todavía no hay estudiantes inscritos en este curso.";
        return;
    }

    for (const student of enrolled) {
        const item = document.createElement("li");
        item.textContent = `${student.matricula} · ${student.nombre} (${student.correo})`;
        enrolledStudentsList.append(item);
    }

    enrolledStudentsMessage.textContent = `${enrolled.length} de ${course.cupoMaximo} cupos ocupados.`;
    enrolledStudentsList.hidden = false;
}

async function loadData() {
    try {
        [courses, students] = await Promise.all([
            fetchJson("/api/cursos"),
            fetchJson("/api/estudiantes")
        ]);
        enrolledByCourse = new Map(await Promise.all(courses.map(async course => [
            course.id,
            await fetchJson(`/api/cursos/${course.id}/estudiantes`)
        ])));
        renderStudents();
        renderCourses();
    } catch (error) {
        showMessage(enrollmentErrorMessage, "No se pudieron cargar los cursos o estudiantes. Comprueba que la API esté en ejecución e inténtalo de nuevo.");
        enrollButton.disabled = true;
    }
}

studentForm.addEventListener("submit", async event => {
    event.preventDefault();
    clearMessage(studentErrorMessage);
    clearMessage(studentStatusMessage);

    const estudiante = Object.fromEntries(new FormData(studentForm).entries());

    try {
        const nuevoEstudiante = await fetchJson("/api/estudiantes", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(estudiante)
        });
        students.push(nuevoEstudiante);
        renderStudents();
        studentSelect.value = String(nuevoEstudiante.id);
        studentForm.reset();
        showMessage(studentStatusMessage, "El estudiante se registró correctamente.");
    } catch (error) {
        const message = error.validationErrors?.length
            ? error.validationErrors.join(" ")
            : error.message;
        showMessage(studentErrorMessage, message);
    }
});

courseSelect.addEventListener("change", updateCourseDetails);

enrollmentForm.addEventListener("submit", async event => {
    event.preventDefault();
    clearMessage(enrollmentErrorMessage);
    clearMessage(enrollmentStatusMessage);

    try {
        const inscripcion = Object.fromEntries(new FormData(enrollmentForm).entries());
        await fetchJson("/api/inscripciones", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                estudianteId: Number(inscripcion.estudianteId),
                cursoId: Number(inscripcion.cursoId)
            })
        });
        enrolledByCourse.set(
            Number(inscripcion.cursoId),
            await fetchJson(`/api/cursos/${inscripcion.cursoId}/estudiantes`)
        );
        renderCourses();
        courseSelect.value = inscripcion.cursoId;
        updateCourseDetails();
        showMessage(enrollmentStatusMessage, "La inscripción se realizó correctamente.");
    } catch (error) {
        const message = error.validationErrors?.length
            ? error.validationErrors.join(" ")
            : error.message;
        showMessage(enrollmentErrorMessage, message);
    }
});

loadData();
