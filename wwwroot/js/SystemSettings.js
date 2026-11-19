$(document).ready(function () {
    loadTableData();

    // 1. Load Data ke Tabel (Read)
    function loadTableData() {
        $.ajax({
            url: '/SystemSettings/GetAllSettings',
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    let rows = '';
                    response.data.forEach(function (item) {
                        rows += `
                            <tr>
                                <td class="fw-bold">${item.siteName}</td>
                                <td>${item.companyEmail}</td>
                                <td>${item.companyPhone}</td>
                                <td class="text-center">
                                    <button type="button" class="btn btn-sm btn-warning text-white me-1 btn-edit" data-id="${item.id}">
                                        <i class="bi bi-pencil-square"></i> Edit
                                    </button>
                                    <button type="button" class="btn btn-sm btn-danger btn-delete" data-id="${item.id}">
                                        <i class="bi bi-trash"></i> Hapus
                                    </button>
                                </td>
                            </tr>
                        `;
                    });
                    $('#tableSettings tbody').html(rows);
                }
            },
            error: function () {
                toastr.error("Gagal memuat data tabel.");
            }
        });
    }

    // Fungsi untuk mereset form agar bersih total saat pertama kali modal dibuka
    function resetFormValidation() {
        let form = $('#formSettingModal')[0];
        form.reset();
        $('#SettingId').val(0);
        $('#settingModalLabel').text('Tambah Setting Baru');
        
        // Hapus kelas warna manual & sembunyikan semua pesan error
        $('.custom-input').removeClass('is-valid-custom is-invalid-custom');
        $('.error-message').hide();
    }

    // 2. Trigger Klik Tombol Tambah
    $('#btnAddModal').on('click', function () {
        resetFormValidation();
        $('#settingModal').modal('show');
    });

    // 3. Trigger Klik Tombol Edit
    $(document).on('click', '.btn-edit', function () {
        let id = $(this).data('id');
        resetFormValidation();
        
        $.ajax({
            url: `/SystemSettings/GetById?id=${id}`,
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    let data = response.data;
                    $('#SettingId').val(data.id);
                    $('#ModalSiteName').val(data.siteName);
                    $('#ModalSiteDescription').val(data.siteDescription);
                    $('#ModalCompanyAddress').val(data.companyAddress);
                    $('#ModalCompanyPhone').val(data.companyPhone);
                    $('#ModalCompanyEmail').val(data.companyEmail);

                    // Beri tanda hijau karena data sudah terisi dari database saat mode edit
                    $('.custom-input').each(function() {
                        if ($(this).val().trim() !== '') {
                            $(this).addClass('is-valid-custom');
                        }
                    });

                    $('#settingModalLabel').text('Edit Setting Sistem');
                    $('#settingModal').modal('show');
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error("Gagal mengambil data untuk diedit.");
            }
        });
    });

    // --- FUNGSI VALIDASI MANUAL (HIJAU / MERAH) ---
    function validateField($field) {
        let value = $field.val().trim();
        let isRequired = $field.prop('required');
        let type = $field.attr('type');
        let fieldId = $field.attr('id');
        let $errorMsg = $('#error-' + fieldId.replace('Modal', ''));

        // Jika tidak wajib dan kosong
        if (!isRequired && value === '') {
            $field.removeClass('is-invalid-custom').addClass('is-valid-custom');
            $errorMsg.hide();
            return true;
        }

        // Jika wajib tapi kosong
        if (isRequired && value === '') {
            $field.removeClass('is-valid-custom').addClass('is-invalid-custom');
            $errorMsg.show();
            return false;
        }

        // Validasi format email khusus
        if (type === 'email' && value !== '') {
            let emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                $field.removeClass('is-valid-custom').addClass('is-invalid-custom');
                $errorMsg.show();
                return false;
            }
        }

        // Jika valid (terisi & benar) -> HIJAU
        $field.removeClass('is-invalid-custom').addClass('is-valid-custom');
        $errorMsg.hide();
        return true;
    }

    // Live validation saat user mengetik atau keluar dari field
    $('#ModalSiteName, #ModalCompanyPhone, #ModalCompanyEmail').on('input blur', function () {
        validateField($(this));
    });

    // 4. Proses Submit Form dengan Peringatan Toastr
    $('#formSettingModal').on('submit', function (e) {
        e.preventDefault();

        let isValid = true;
        $('#formSettingModal [required]').each(function () {
            let fieldValid = validateField($(this));
            if (!fieldValid) {
                isValid = false;
            }
        });

        // Jika ada yang belum valid / kosong
        if (!isValid) {
            toastr.warning("Mohon lengkapi semua field bertanda bintang (*) dengan benar!", "Peringatan Validasi");
            return;
        }

        let formData = {
            Id: $('#SettingId').val(),
            SiteName: $('#ModalSiteName').val(),
            SiteDescription: $('#ModalSiteDescription').val(),
            CompanyAddress: $('#ModalCompanyAddress').val(),
            CompanyPhone: $('#ModalCompanyPhone').val(),
            CompanyEmail: $('#ModalCompanyEmail').val()
        };

        let isEdit = formData.Id > 0 && formData.Id != "0";
        let urlAction = isEdit ? '/SystemSettings/Update' : '/SystemSettings/Create';

        let $btn = $('#btnSaveModal');
        let originalText = $btn.html();
        $btn.prop('disabled', true).html('<span class="spinner-border spinner-border-sm"></span> Menyimpan...');

        $.ajax({
            url: urlAction,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $('#settingModal').modal('hide');
                    toastr.success(response.message, "Berhasil!");
                    loadTableData(); 
                } else {
                    toastr.error(response.message, "Gagal");
                }
            },
            error: function () {
                toastr.error("Terjadi kesalahan pada server.", "Error");
            },
            complete: function () {
                $btn.prop('disabled', false).html(originalText);
            }
        });
    });

    // 5. Event Klik Tombol Hapus
    $(document).on('click', '.btn-delete', function () {
        let id = $(this).data('id');

        Swal.fire({
            title: 'Apakah Anda yakin?',
            text: "Data yang dihapus tidak dapat dikembalikan!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Ya, Hapus!',
            cancelButtonText: 'Batal'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/SystemSettings/Delete?id=${id}`,
                    type: 'POST',
                    success: function (response) {
                        if (response.success) {
                            toastr.success(response.message, "Terhapus!");
                            loadTableData();
                        } else {
                            toastr.error(response.message, "Gagal");
                        }
                    },
                    error: function () {
                        toastr.error("Gagal menghapus data.");
                    }
                });
            }
        });
    });
});