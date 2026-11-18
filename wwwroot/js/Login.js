// Konfigurasi Toastr
toastr.options = {
  closeButton: true,
  progressBar: true,
  positionClass: "toast-top-center",
  showDuration: "300",
  hideDuration: "1000",
  timeOut: "4000",
  extendedTimeOut: "1000",
};

// Toggle Password Visibility
// for demo purpose, you can use the following code to toggle the password visibility in the login form. 
// This will allow users to see the password they are typing if they choose to do so.
$("#btnTogglePassword").on("click", function () {
  const passwordInput = $("#Password");
  const eyeIcon = $("#eyeIcon");
  const type = passwordInput.attr("type") === "password" ? "text" : "password";
  passwordInput.attr("type", type);
  eyeIcon.toggleClass("bi-eye bi-eye-slash");
});

// Autofill akun demo sesuai role
// This function is used to fill in the demo credentials for different user roles.
function fillDemo(email, password) {
  $("#Email").val(email);
  $("#Password").val(password);
  toastr.info("Kredensial demo " + email + " diisi!");
}

// Handle AJAX Submit Form Login
$("#formLogin").on("submit", async function (e) {
  e.preventDefault();

  const form = $(this);
  const url = form.data("url") || "/Account/ProcessLogin";
  const email = $("#Email").val().trim();
  const password = $("#Password").val();
  const rememberMe = $("#RememberMe").is(":checked");
  const returnUrl = $("#ReturnUrl").val();
  const token = $('input[name="__RequestVerificationToken"]').val();

  if (!email || !password) {
    toastr.warning("Email dan password harus diisi.");
    return;
  }

  // Lock Button Loading
  const btnSubmit = $("#btnSubmit");
  const btnText = $("#btnText");
  const btnSpinner = $("#btnSpinner");

  btnSubmit.prop("disabled", true);
  btnText.text("Memverifikasi...");
  btnSpinner.removeClass("d-none");

  try {
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        RequestVerificationToken: token,
      },
      body: JSON.stringify({
        email: email,
        password: password,
        rememberMe: rememberMe,
        returnUrl: returnUrl,
      }),
    });

    const data = await response.json();

    // Handle Response Toastr Berdasarkan Status JSON BE
    switch (data.status) {
      case "Success":
        toastr.success(data.message || "Login Berhasil!");
        setTimeout(() => {
          window.location.href = data.redirectUrl || "/Dashboard";
        }, 1200);
        break;
      case "NotFound":
        toastr.warning(data.message || "Email tidak ditemukan.");
        break;
      case "Failed":
        toastr.error(data.message || "Login gagal.");
        break;
      case "Error":
        toastr.error(data.message || "Terjadi kesalahan sistem.");
        break;
      default:
        toastr.error("Respon tidak dikenal dari server.");
        break;
    }
  } catch (err) {
    console.error(err);
    toastr.error("Gagal terhubung ke server. Periksa koneksi internet Anda.");
  } finally {
    // Restore Button State
    btnSubmit.prop("disabled", false);
    btnText.text("Sign In");
    btnSpinner.addClass("d-none");
  }
});
