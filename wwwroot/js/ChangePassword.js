$(document).ready(function () {
    $('#changePasswordForm').on('submit', function (e) {
        e.preventDefault();

        var form = $(this);
        var submitBtn = form.find('button[type="submit"]');
        var token = $('input[name="__RequestVerificationToken"]').val();

        var formData = {
            CurrentPassword: $('#CurrentPassword').val(),
            NewPassword: $('#NewPassword').val(),
            ConfirmPassword: $('#ConfirmPassword').val()
        };

        submitBtn.prop('disabled', true).text('Menyimpan...');

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(formData),
            headers: {
                "RequestVerificationToken": token
            },
            success: function (response) {
                // Cek berdasarkan properti Status dari ApiResponse<T> C#
                // Cek juga secara case-insensitive atau fleksibel mengantisipasi camelCase serialization (.NET)
                var status = response.status || response.Status;
                var message = response.message || response.Message;
                var redirectUrl = response.redirectUrl || response.RedirectUrl || '/Dashboard/Index';

                if (status === "Success") {
                    toastr.success(message || "Kata sandi berhasil diubah!");
                    
                    setTimeout(function () {
                        window.location.href = redirectUrl;
                    }, 1500);
                } else {
                    toastr.error(message || "Gagal mengubah kata sandi.");
                    submitBtn.prop('disabled', false).text('Simpan Perubahan');
                }
            },
            error: function (xhr) {
                toastr.error("Terjadi kesalahan pada server.");
                submitBtn.prop('disabled', false).text('Simpan Perubahan');
            }
        });
    });
});