const form = document.querySelector("#sum-form");
const errorMessage = document.querySelector("#error-message");
const resultMessage = document.querySelector("#result-message");

form.addEventListener("submit", async (event) => {
    event.preventDefault();
    errorMessage.hidden = true;
    resultMessage.hidden = true;

    const formData = new FormData(form);

    try {
        const response = await fetch("/api/sumadora", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify({
                numero1: Number(formData.get("numero1")),
                numero2: Number(formData.get("numero2"))
            })
        });

        if (!response.ok) {
            throw new Error("No se pudieron sumar los números. Verifica los valores e inténtalo de nuevo.");
        }

        const data = await response.json();
        resultMessage.textContent = `Resultado: ${data.resultado}`;
        resultMessage.hidden = false;
    } catch (error) {
        errorMessage.textContent = error.message || "No se pudo conectar con la API.";
        errorMessage.hidden = false;
    }
});
