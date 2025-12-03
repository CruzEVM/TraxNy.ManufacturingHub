// ========== Utilidades generales ==========
function getTimestamp() {
    const now = new Date();
    return now.toLocaleTimeString("es-MX", { hour12: false });
}

function addEventLog(message, level = "info") {
    const container = document.getElementById("eventLogContainer");
    if (!container) return;

    const line = document.createElement("p");

    let colorClass = "text-white/70";
    if (level === "ok") colorClass = "text-green-400";
    if (level === "warn") colorClass = "text-yellow-400";
    if (level === "error") colorClass = "text-red-400";
    if (level === "action") colorClass = "text-primary";

    line.className = colorClass;
    line.innerHTML = `<span class="text-primary/80">[${getTimestamp()}]</span> ${message}`;

    container.appendChild(line);
    container.scrollTop = container.scrollHeight;
}

// ========== Lógica por pantalla ==========
document.addEventListener("DOMContentLoaded", () => {
    const esLingote = !!document.getElementById("tituloLingote");
    const esDiamante = !!document.getElementById("tituloDiamante");
    const esInventario = !!document.getElementById("inventoryList");
    const esUsuarios = !!document.getElementById("userList");

    // ======== LINGOTE =========
    if (esLingote) {
        const purezaSpan = document.getElementById("purezaValue");
        const estadoSpan = document.getElementById("estadoLingote");
        const progressFill = document.getElementById("progressFill");

        const btnAjustar = document.getElementById("btnAjustarPureza");
        const btnEnfriar = document.getElementById("btnForzarEnfriamiento");
        const btnQC = document.getElementById("btnMarcarQC");

        if (progressFill) {
            const widthOriginal = progressFill.style.width || "66%";
            progressFill.style.width = "0%";
            setTimeout(() => {
                progressFill.style.transition = "width 1.8s ease-out";
                progressFill.style.width = widthOriginal;
            }, 200);
        }

        if (btnAjustar && purezaSpan) {
            btnAjustar.addEventListener("click", () => {
                const actual = parseFloat(purezaSpan.textContent.replace(",", "."));
                const input = prompt(
                    "Nueva pureza para el lingote (ej. 999.5):",
                    isNaN(actual) ? "" : actual
                );
                if (input === null) return;

                const valor = Number(input.replace(",", "."));
                if (isNaN(valor) || valor <= 0) {
                    alert("Valor no válido.");
                    return;
                }

                purezaSpan.textContent = valor.toFixed(1);
                addEventLog(
                    `Pureza de lingote ajustada a ${valor.toFixed(1)} por operador.`,
                    "action"
                );
            });
        }

        if (btnEnfriar && estadoSpan && progressFill) {
            btnEnfriar.addEventListener("click", () => {
                estadoSpan.innerHTML = `
          <span class="size-2 rounded-full bg-blue-400 animate-pulse"></span>
          Enfriamiento Forzado
        `;
                estadoSpan.classList.remove("bg-primary/20", "text-primary");
                estadoSpan.classList.add("bg-blue-500/20", "text-blue-300");

                progressFill.style.transition = "width 1.2s ease-out";
                progressFill.style.width = "100%";

                addEventLog("Ciclo de enfriamiento forzado activado para lingote.", "warn");
            });
        }

        if (btnQC) {
            btnQC.addEventListener("click", () => {
                addEventLog("Lingote marcado para inspección de Calidad (QC).", "ok");
                btnQC.classList.remove("bg-white/10");
                btnQC.classList.add("bg-green-600/30");
            });
        }

        addEventLog("Pantalla de Lingote cargada correctamente.", "info");
    }

    // ======== DIAMANTE =========
    if (esDiamante) {
        const chip = document.getElementById("diamondPhaseTag");
        const rateSpan = document.getElementById("diamondRateValue");
        const tempSpan = document.getElementById("diamondTempValue");
        const pressureSpan = document.getElementById("diamondPressureValue");
        const etaLabel = document.getElementById("diamondEtaLabel");
        const progressFill = document.getElementById("diamondProgressFill");

        const btnParams = document.getElementById("btnDiamondParams");
        const btnTemplado = document.getElementById("btnDiamondTemplado");
        const btnHarvest = document.getElementById("btnDiamondHarvest");

        if (progressFill) {
            const widthOriginal = progressFill.style.width || "45%";
            progressFill.style.width = "0%";
            setTimeout(() => {
                progressFill.style.transition = "width 1.8s ease-out";
                progressFill.style.width = widthOriginal;
            }, 200);
        }

        if (btnParams && rateSpan && tempSpan && pressureSpan) {
            btnParams.addEventListener("click", () => {
                const newRate = prompt(
                    "Nueva tasa de crecimiento (µm/h):",
                    rateSpan.textContent.trim()
                );
                if (newRate === null) return;

                const newTemp = prompt(
                    "Nueva temperatura de reactor (°C):",
                    tempSpan.textContent.trim()
                );
                if (newTemp === null) return;

                const newPress = prompt(
                    "Nueva presión (GPa):",
                    pressureSpan.textContent.trim()
                );
                if (newPress === null) return;

                rateSpan.textContent = newRate;
                tempSpan.textContent = newTemp;
                pressureSpan.textContent = newPress;

                addEventLog(
                    `Parámetros de reactor actualizados: tasa ${newRate} µm/h, T=${newTemp} °C, P=${newPress} GPa.`,
                    "action"
                );
            });
        }

        if (btnTemplado && chip && progressFill) {
            btnTemplado.addEventListener("click", () => {
                chip.innerHTML = `
          <span class="size-2 rounded-full bg-orange-400 animate-pulse"></span>
          Templado Activo
        `;
                chip.classList.remove("bg-accent-blue/20", "text-accent-blue");
                chip.classList.add("bg-orange-500/20", "text-orange-300");

                progressFill.style.transition = "width 1.2s ease-out";
                progressFill.style.width = "100%";

                addEventLog("Ciclo de templado iniciado para el lote de diamante.", "warn");
            });
        }

        if (btnHarvest && etaLabel) {
            btnHarvest.addEventListener("click", () => {
                const horas = prompt(
                    "¿En cuántas horas estimas la cosecha del diamante?",
                    "24"
                );
                if (horas === null) return;

                const hNum = Number(horas);
                if (isNaN(hNum) || hNum <= 0) {
                    alert("Valor no válido.");
                    return;
                }

                etaLabel.textContent = `${hNum.toFixed(1)} h`;
                addEventLog(
                    `Cosecha del diamante programada aproximadamente en ${hNum.toFixed(1)} horas.`,
                    "ok"
                );
            });
        }

        addEventLog("Pantalla de Diamante cargada correctamente.", "info");
    }

    // ======== INVENTARIO =========
    if (esInventario) {
        const searchInput = document.getElementById("inventorySearch");
        const chips = document.querySelectorAll(".inventory-status-chip");
        const items = document.querySelectorAll(".inventory-item");
        const btnAdd = document.getElementById("btnAddInventoryItem");

        function applyInventoryFilters() {
            const text = (searchInput?.value || "").toLowerCase();
            const activeChip = document.querySelector(".inventory-status-chip.bg-primary");
            const statusFilter = activeChip ? activeChip.getAttribute("data-status") : "all";

            items.forEach(item => {
                const name = (item.getAttribute("data-name") || "").toLowerCase();
                const id = (item.getAttribute("data-id") || "").toLowerCase();
                const loc = (item.getAttribute("data-location") || "").toLowerCase();
                const status = item.getAttribute("data-status");

                const matchesText =
                    !text ||
                    name.includes(text) ||
                    id.includes(text) ||
                    loc.includes(text);

                const matchesStatus =
                    statusFilter === "all" || status === statusFilter;

                item.style.display = matchesText && matchesStatus ? "flex" : "none";
            });
        }

        if (searchInput) {
            searchInput.addEventListener("input", applyInventoryFilters);
        }

        chips.forEach(chip => {
            chip.addEventListener("click", () => {
                chips.forEach(c =>
                    c.classList.remove("bg-primary", "text-background-dark")
                );
                chips.forEach(c =>
                    c.classList.add("bg-primary/20", "text-white")
                );

                chip.classList.remove("bg-primary/20", "text-white");
                chip.classList.add("bg-primary", "text-background-dark");

                applyInventoryFilters();
            });
        });

        if (btnAdd) {
            btnAdd.addEventListener("click", () => {
                const id = prompt("ID del nuevo item (ej. G-NEW-001):");
                if (!id) return;
                const nombre = prompt("Nombre / descripción corta del item:");
                if (!nombre) return;
                const ubicacion = prompt("Ubicación (ej. Vault 3):", "Vault 3");
                if (!ubicacion) return;

                const status = prompt(
                    "Estatus (in-stock / reserved / hold / low):",
                    "in-stock"
                ) || "in-stock";

                const cont = document.getElementById("inventoryList");
                if (!cont) return;

                const wrapper = document.createElement("div");
                wrapper.className =
                    "inventory-item flex gap-4 bg-background-light dark:bg-background-dark py-3 justify-between border-b border-primary/20";
                wrapper.setAttribute("data-id", id);
                wrapper.setAttribute("data-name", nombre);
                wrapper.setAttribute("data-location", ubicacion);
                wrapper.setAttribute("data-status", status);

                let badgeText = "En stock", badgeClasses = "bg-green-500/20 text-green-400", dot = "bg-green-500";
                if (status === "reserved") {
                    badgeText = "Reservado";
                    badgeClasses = "bg-yellow-500/20 text-yellow-400";
                    dot = "bg-yellow-500";
                } else if (status === "hold") {
                    badgeText = "Quality Hold";
                    badgeClasses = "bg-red-500/20 text-red-400";
                    dot = "bg-red-500";
                } else if (status === "low") {
                    badgeText = "Bajo stock";
                    badgeClasses = "bg-orange-500/20 text-orange-400";
                    dot = "bg-orange-500";
                }

                wrapper.innerHTML = `
          <div class="flex items-start gap-4">
            <div class="bg-center bg-no-repeat aspect-square bg-cover rounded-lg size-[70px]"
                 style='background-image: url("https://lh3.googleusercontent.com/aida-public/AB6AXuBVLF2PjfJD1EPzEV3QnvDW2n4-6GcmADxlCPw13zgV6RzpZTlrPsTVpE-xE2DpqTEWILDGmCKIREN0mxsHfbYCxSANG8SEUF4jMZ7l80ynjyFozD4eX911MdAVVzaquTWaqjRH2vMwtOS-22tI5T8Y8_rU4GdiH22vfJ_o73ejkHf9APoMLEJuRa6C3XSCSBftgPhc4_6GJ8444BR3LVDWnm8Zmchh_zjFNoQt6eIdgW7tsJxMi0dt8F7MUtA92rBlFqFrCWOSb-GD");'></div>
            <div class="flex flex-1 flex-col justify-center">
              <p class="text-white text-base font-medium leading-normal">
                ${id} - ${nombre}
              </p>
              <p class="text-white/60 text-sm font-normal leading-normal">
                Ubicación: ${ubicacion}
              </p>
            </div>
          </div>
          <div class="shrink-0 flex items-center">
            <div class="flex items-center gap-2 rounded-full ${badgeClasses} px-3 py-1">
              <div class="size-2 rounded-full ${dot}"></div>
              <p class="text-xs font-medium">${badgeText}</p>
            </div>
          </div>
        `;

                cont.appendChild(wrapper);
                addEventLog(`Nuevo item agregado al inventario: ${id} - ${nombre}.`, "ok");
            });
        }

        addEventLog("Pantalla de Inventario cargada correctamente.", "info");
    }

    // ======== USUARIOS =========
    if (esUsuarios) {
        const searchInput = document.getElementById("userSearch");
        const chips = document.querySelectorAll(".user-role-chip");
        const items = document.querySelectorAll(".user-item");
        const btnAdd = document.getElementById("btnAddUser");

        function applyUserFilters() {
            const text = (searchInput?.value || "").toLowerCase();
            const activeChip = document.querySelector(".user-role-chip.bg-primary");
            const roleFilter = activeChip ? activeChip.getAttribute("data-role") : "all";

            items.forEach(item => {
                const name = (item.getAttribute("data-name") || "").toLowerCase();
                const role = item.getAttribute("data-role") || "";
                const desc = (item.textContent || "").toLowerCase();

                const matchesText =
                    !text || name.includes(text) || desc.includes(text);
                const matchesRole =
                    roleFilter === "all" || role === roleFilter;

                item.style.display = matchesText && matchesRole ? "flex" : "none";
            });
        }

        if (searchInput) {
            searchInput.addEventListener("input", applyUserFilters);
        }

        chips.forEach(chip => {
            chip.addEventListener("click", () => {
                chips.forEach(c =>
                    c.classList.remove("bg-primary", "text-background-dark")
                );
                chips.forEach(c =>
                    c.classList.add("bg-primary/20", "text-white")
                );

                chip.classList.remove("bg-primary/20", "text-white");
                chip.classList.add("bg-primary", "text-background-dark");

                applyUserFilters();
            });
        });

        // Cambiar rol (ciclo supervisor -> operator -> qc)
        document.querySelectorAll(".btn-toggle-role").forEach(btn => {
            btn.addEventListener("click", () => {
                const item = btn.closest(".user-item");
                if (!item) return;

                const currentRole = item.getAttribute("data-role");
                const name = item.getAttribute("data-name") || "Usuario";

                let nextRole = "operator";
                if (currentRole === "operator") nextRole = "qc";
                else if (currentRole === "qc") nextRole = "supervisor";

                item.setAttribute("data-role", nextRole);
                addEventLog(
                    `Rol de ${name} cambiado de ${currentRole} a ${nextRole}.`,
                    "action"
                );
                applyUserFilters();
            });
        });

        // Activar / desactivar usuario
        document.querySelectorAll(".btn-toggle-active").forEach(btn => {
            btn.addEventListener("click", () => {
                const item = btn.closest(".user-item");
                if (!item) return;

                const name = item.getAttribute("data-name") || "Usuario";
                const status = item.getAttribute("data-status") || "inactive";
                const newStatus = status === "active" ? "inactive" : "active";
                item.setAttribute("data-status", newStatus);

                const badge = item.querySelector("span.inline-flex");
                if (badge) {
                    if (newStatus === "active") {
                        badge.className =
                            "inline-flex items-center rounded-full bg-green-500/20 px-3 py-1 text-xs text-green-400";
                        badge.textContent = "Activo";
                        btn.innerHTML =
                            '<span class="material-symbols-outlined text-sm">power_settings_new</span> Desactivar';
                    } else {
                        badge.className =
                            "inline-flex items-center rounded-full bg-red-500/20 px-3 py-1 text-xs text-red-400";
                        badge.textContent = "Inactivo";
                        btn.innerHTML =
                            '<span class="material-symbols-outlined text-sm">power_settings_new</span> Activar';
                    }
                }

                addEventLog(
                    `Usuario ${name} cambiado a estado ${newStatus}.`,
                    newStatus === "active" ? "ok" : "warn"
                );
            });
        });

        // Agregar nuevo usuario rápido
        if (btnAdd) {
            btnAdd.addEventListener("click", () => {
                const name = prompt("Nombre del nuevo usuario:");
                if (!name) return;
                const role = prompt(
                    "Rol (supervisor / operator / qc):",
                    "operator"
                ) || "operator";
                const desc = prompt(
                    "Descripción / área (ej. Operador Reactor • Turno C):",
                    "Operador general"
                ) || "Operador general";

                const list = document.getElementById("userList");
                if (!list) return;

                const wrapper = document.createElement("div");
                wrapper.className =
                    "user-item flex items-center justify-between py-3 border-b border-primary/20";
                wrapper.setAttribute("data-name", name);
                wrapper.setAttribute("data-role", role);
                wrapper.setAttribute("data-status", "active");

                wrapper.innerHTML = `
          <div class="flex items-center gap-3">
            <div class="rounded-full bg-primary/20 size-10 flex items-center justify-center">
              <span class="material-symbols-outlined text-primary">person</span>
            </div>
            <div>
              <p class="text-white text-base font-medium leading-normal">
                ${name}
              </p>
              <p class="text-white/60 text-sm leading-tight">
                ${desc}
              </p>
            </div>
          </div>
          <div class="flex items-center gap-3">
            <span class="inline-flex items-center rounded-full bg-green-500/20 px-3 py-1 text-xs text-green-400">
              Activo
            </span>
            <button class="btn-toggle-role inline-flex items-center gap-1 rounded-full bg-white/10 px-3 py-1 text-xs text-white">
              <span class="material-symbols-outlined text-sm">swap_horiz</span>
              Rol
            </button>
          </div>
        `;

                list.appendChild(wrapper);
                addEventLog(`Nuevo usuario agregado: ${name} (${role}).`, "ok");
                applyUserFilters();
            });
        }

        addEventLog("Pantalla de Usuarios cargada correctamente.", "info");
    }
});
