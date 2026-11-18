"use strict";
const KartuStok = (() => {
    const state = {
        gudangId: null,
        produkId: null
    };

    const urls = {
        searchGudang: "/KartuStok/SearchGudang",
        searchProduk: "/KartuStok/SearchProduk",
        details: "/KartuStok/Details",
        report: "/KartuStok/ReportKartuStok",
        initialGudang: "/KartuStok/GetInitialGudang",
        initialProduk: "/KartuStok/GetInitialProduk"
    };

    function init() {
        initSelect2();
        bindEvents();
        loadInitialDropdowns();
    }

    function initSelect2() {
        // Select2 untuk Gudang dengan AJAX (Limit 5 data awal & live search)
        $("#selectGudang").select2({
            width: '100%',
            placeholder: '-- Pilih Gudang --',
            allowClear: true,
            ajax: {
                url: urls.searchGudang,
                dataType: 'json',
                delay: 250,
                data: (params) => ({ term: params.term }),
                processResults: (res) => ({
                    results: (res.data || []).map(item => ({ id: item.id, text: item.text }))
                })
            }
        });

        // Select2 untuk Produk dengan AJAX berdasarkan Gudang yang dipilih
        $("#selectProduk").select2({
            width: '100%',
            placeholder: '-- Pilih SKU atau Nama Produk --',
            allowClear: true,
            ajax: {
                url: urls.searchProduk,
                dataType: 'json',
                delay: 250,
                data: (params) => ({ 
                    gudangId: state.gudangId, 
                    term: params.term 
                }),
                processResults: (res) => ({
                    results: (res.data || []).map(item => ({ id: item.id, text: item.text }))
                })
            }
        });
    }

    // Memuat 5 data awal ke dropdown tanpa melakukan auto-select (dibiarkan kosong)
    function loadInitialDropdowns() {
        $.get(urls.initialGudang).done(res => {
            const $s = $("#selectGudang");
            $s.empty().append('<option value="">-- Pilih Gudang --</option>');
            if (res.success && res.data && res.data.length > 0) {
                res.data.forEach(item => {
                    $s.append(new Option(item.text, item.id, false, false));
                });
            }
            $s.val(null).trigger('change.select2');
        });

        $.get(urls.initialProduk).done(res => {
            const $s = $("#selectProduk");
            $s.empty().append('<option value="">-- Pilih SKU atau Nama Produk --</option>');
            if (res.success && res.data && res.data.length > 0) {
                res.data.forEach(item => {
                    $s.append(new Option(item.text, item.id, false, false));
                });
            }
            $s.val(null).trigger('change.select2');
        });
    }

    function bindEvents() {
        $("#selectGudang").on("change", function () {
            const val = $(this).val();
            const newGudangId = val ? parseInt(val) : null;

            // Jika gudang berubah, reset produk yang sebelumnya dipilih
            if (state.gudangId !== newGudangId) {
                state.gudangId = newGudangId;
                resetProduk();
            }

            if (!state.gudangId) {
                $("#selectProduk").prop("disabled", true);
            } else {
                $("#selectProduk").prop("disabled", false);
            }

            evaluateAndLoad();
        });

        $("#selectProduk").on("change", function () {
            const val = $(this).val();
            state.produkId = val ? parseInt(val) : null;
            evaluateAndLoad();
        });

        $(document).off("click", "#btnCetakJurnal").on("click", "#btnCetakJurnal", function (e) {
            e.preventDefault();
            if (!state.gudangId || !state.produkId) {
                showError("Silakan pilih Gudang dan Produk terlebih dahulu.");
                return;
            }
            const reportUrl = `${urls.report}?gudangId=${state.gudangId}&produkId=${state.produkId}`;
            window.open(reportUrl, '_blank');
        });
    }

    function evaluateAndLoad() {
        const $btn = $("#btnCetakJurnal");
        if (state.gudangId && state.produkId) {
            $btn.prop("disabled", false).show();
            loadDetails();
        } else {
            $btn.prop("disabled", true).hide();
            showInitialState();
        }
    }

    function resetProduk() {
        state.produkId = null;
        $("#selectProduk").val(null).trigger('change.select2');
        // Kosongkan option produk agar memuat ulang sesuai gudang baru saat diklik
        $("#selectProduk").empty().append('<option value="">-- Pilih SKU atau Nama Produk --</option>');
    }

    function loadDetails() {
        showLoading();
        $.get(urls.details, { gudangId: state.gudangId, produkId: state.produkId })
            .done(res => {
                if (!res || !res.success || !res.data) {
                    showLoadFailed(res && res.message ? res.message : "Gagal memuat histori.");
                    return;
                }
                renderDetails(res.data);
            })
            .fail(() => showLoadFailed("Gagal memuat histori."));
    }

    function showLoading() {
        $("#kartuStokDetailsContainer").html('<div class="text-center py-5"><div class="spinner-border text-primary"></div></div>');
    }

    function showInitialState() {
        $("#kartuStokDetailsContainer").html(`
            <div class="empty-state-card shadow-sm">
                <div class="text-center p-4" style="max-width: 440px; margin: 0 auto;">
                    <div class="info-circle-icon mb-3">
                        <i class="bi bi-info fs-4 text-primary"></i>
                    </div>
                    <h5 class="fw-bold text-indigo-dark mb-2" style="font-size: 1.1rem;">
                        Buka Kartu Stok
                    </h5>
                    <p class="text-muted small mb-0 lh-base">
                        Silakan pilih <span class="fw-semibold">Gudang</span> dan <span class="fw-semibold">Produk</span> terlebih dahulu melalui form di atas untuk melihat jurnal riwayat pergerakan stok.
                    </p>
                </div>
            </div>
        `);
    }

    function showError(msg) {
        if (typeof toastr !== "undefined") toastr.error(msg);
    }

    function showLoadFailed(msg) {
        showError(msg);
        $("#kartuStokDetailsContainer").html(`
            <div class="empty-state-card text-center p-4">
                <i class="bi bi-exclamation-triangle fs-1 text-danger"></i>
                <h6 class="fw-bold mt-3">${escapeHtml(msg)}</h6>
            </div>
        `);
    }

    function renderDetails(data) {
        const container = $("#kartuStokDetailsContainer");
        const summary = data.summary;
        let rowsHtml = !data.rows || data.rows.length === 0 ?
            `<div class="text-center py-5"><i class="bi bi-database-exclamation fs-1 text-muted"></i><h6 class="fw-bold mt-3">Belum ada data untuk ditampilkan !</h6><p class="text-muted small mb-0">Belum terdapat histori mutasi stok untuk produk dan gudang yang dipilih.</p></div>` :
            buildRowsHtml(data.rows);

        const html = `
            <div class="mb-4">
                <p class="text-muted small mb-0">${escapeHtml(data.kodeGudang)} - ${escapeHtml(data.namaGudang)} | ${escapeHtml(data.sku)} - ${escapeHtml(data.namaProduk)}</p>
            </div>
            <div class="row g-3 mb-4">
                <div class="col-6 col-md-3"><div class="summary-card-custom shadow-sm"><span class="form-label d-block mb-1">Stok Awal</span><div class="qty-display">${summary.stokAwal} <span class="fs-6 text-muted fw-normal">${escapeHtml(data.satuan)}</span></div></div></div>
                <div class="col-6 col-md-3"><div class="summary-card-custom shadow-sm"><span class="form-label d-block mb-1">Total Masuk</span><div class="qty-display positive">+${summary.totalMasuk} <span class="fs-6 text-muted fw-normal">${escapeHtml(data.satuan)}</span></div></div></div>
                <div class="col-6 col-md-3"><div class="summary-card-custom shadow-sm"><span class="form-label d-block mb-1">Total Keluar</span><div class="qty-display negative">-${summary.totalKeluar} <span class="fs-6 text-muted fw-normal">${escapeHtml(data.satuan)}</span></div></div></div>
                <div class="col-6 col-md-3"><div class="summary-card-custom shadow-sm"><span class="form-label d-block mb-1">Stok Akhir</span><div class="qty-display">${summary.stokAkhir} <span class="fs-6 text-muted fw-normal">${escapeHtml(data.satuan)}</span></div></div></div>
            </div>
            <div class="card border-0 shadow-sm rounded-3 overflow-hidden" style="border: 1px solid var(--border-color) !important;">
                <div class="card-header bg-white border-bottom py-3">
                    <div class="d-flex align-items-center gap-2 text-indigo-dark fw-bold" style="font-size: 0.85rem; letter-spacing: 0.05em; text-transform: uppercase;">
                        <i class="bi bi-clock-history text-primary"></i> Histori Mutasi
                    </div>
                </div>
                <div class="table-responsive">${rowsHtml}</div>
            </div>
        `;
        container.html(html);
    }

    function buildRowsHtml(rows) {
        let html = `<table class="table table-mutasi align-middle mb-0"><thead><tr><th style="width: 15%;">Tanggal & Waktu</th><th style="width: 15%;" class="text-center">Tipe</th><th style="width: 8%;" class="text-center">Jumlah</th><th style="width: 10%;" class="text-center">Alur Stok</th><th style="width: 15%;">Referensi</th><th style="width: 12%;">Pencatat</th><th style="width: 25%;">Keterangan</th></tr></thead><tbody>`;
        rows.forEach(row => {
            const quantityClass = row.jumlah >= 0 ? "text-success" : "text-danger";
            const quantityDisplay = row.jumlah >= 0 ? `+${row.jumlah}` : row.jumlah;
            html += `<tr>
                <td><div class="d-flex align-items-center gap-1 text-muted font-monospace" style="font-size: 0.8rem;"><i class="bi bi-calendar3"></i> ${formatDate(row.tanggalPosting)}</div></td>
                <td class="text-center"><span class="badge-mutasi-custom ${escapeHtml(row.badgeClass)}"><i class="bi ${escapeHtml(row.badgeIcon || 'bi-arrow-left-right')}"></i> ${escapeHtml(row.tipeTransaksi)}</span></td>
                <td class="text-center fw-bold ${quantityClass}">${quantityDisplay}</td>
                <td class="text-center font-monospace text-muted" style="font-size: 0.8rem;">${row.stokSebelum} → ${row.stokSesudah}</td>
                <td><div class="fw-semibold font-monospace" style="font-size: 0.8rem;">${escapeHtml(row.nomorReferensi)}</div>${row.kodeLokasi ? `<small class="text-muted font-monospace" style="font-size: 0.7rem;">${escapeHtml(row.kodeLokasi)}</small>` : ''}</td>
                <td><span class="text-muted"><i class="bi bi-person-circle"></i> ${escapeintahHtmlSafe(row.pembuat)}</span></td>
                <td><small class="text-muted">${escapeHtml(row.catatan || '')}</small></td>
            </tr>`;
        });
        html += `</tbody></table>`;
        return html;
    }

    function escapeintahHtmlSafe(val) {
        return escapeHtml(val);
    }

    function formatDate(value) {
        if (!value) return "-";
        const date = new Date(value);
        if (Number.isNaN(date.getTime())) return escapeHtml(value);
        const day = String(date.getDate()).padStart(2, "0");
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "Mei", "Jun", "Jul", "Agu", "Sep", "Okt", "Nov", "Des"];
        return `${day} ${monthNames[date.getMonth()]} ${date.getFullYear()}, ${String(date.getHours()).padStart(2, "0")}.${String(date.getMinutes()).padStart(2, "0")}`;
    }

    function escapeHtml(value) {
        if (value === null || value === undefined) return "";
        return String(value).replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&#039;");
    }

    return { init };
})();

$(document).ready(() => {
    KartuStok.init();
});