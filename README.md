<div align="center">

# 📦 Inventory Multi Gudang Modern
**Sistem Manajemen Aset IT, Inventaris Kantor & Multi-Gudang Real-Time**

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.x-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![Database](https://img.shields.io/badge/Database-MySQL%20%7C%20MariaDB%20%7C%20PostgreSQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)](#)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)

🌐 **[Lihat Live Demo Aplikasi](http://imgm.runasp.net/)** • 📖 **[Konsep Dasar](#-konsep-dasar)** • 🚀 **[Fitur Unggulan](#-fitur-unggulan)** • 🛠️ **[Instalasi](#-panduan-instalasi)**

</div>

---

## 📋 Tentang Produk

**Inventory Multi Gudang Modern** adalah aplikasi manajemen aset dan inventaris universal yang dirancang untuk membantu berbagai perusahaan dan perkantoran dalam mengelola perangkat IT (*IT Asset Management*), fasilitas/infrastruktur kantor, pergudangan (*multi-gudang*), pelacakan nomor seri (*Serial Number*), serta mencatat konsumsi barang operasional secara presisi dan *real-time*.

Dibangun di atas teknologi modern **.NET Core MVC (.NET 9)** dengan desain antarmuka premium bergaya dashboard **Stripe / Linear** yang bersih, responsif, dan profesional.

---

## 🎯 Masalah Utama Yang Dipecahkan (Core Problems Solved)

Banyak perusahaan mengalami kerugian operasional dan administratif akibat tata kelola inventaris yang kurang terstruktur. Aplikasi ini hadir untuk mengatasi **5 masalah kritis** berikut:

| Masalah Operasional | Dampak Bisnis | Solusi Sistem |
| :--- | :--- | :--- |
| 🔍 **Kehilangan Jejak Aset**<br>*(Asset Lost)* | Laptop, monitor, atau modem berpindah tangan antar-karyawan tanpa pencatatan sehingga "hilang" saat audit. | Pelacakan **Serial Number (SN)** terpadu dan *Direct Transfer Form* dengan *audit trail* instan. |
| 🚫 **Checkout Tanpa Otorisasi**<br>*(Inventory Leakage)* | Staf gudang mengeluarkan perangkat mahal tanpa persetujuan tertulis dari pimpinan. | **Assignment Approval Workflow** (`pending_approval`, `approved`, `rejected`). Stok tidak terpotong sebelum disetujui admin. |
| ⏰ **Peminjaman Terlupakan**<br>*(Overdue Loans)* | Karyawan meminjam proyektor/modem cadangan berhari-hari tanpa pengingat pengembalian. | **Overdue Loan Warning System** otomatis di panel *navbar* global dengan pembaruan status berkala. |
| 📂 **Silo Riwayat Kondisi Aset**<br>*(Scattered Records)* | Sulit melacak frekuensi kerusakan perangkat, riwayat servis, hingga harga perolehan awal. | **Serial History Timeline** yang merekam kronologi lengkap: *receipt, assignment, loan, transfer, repair,* hingga *disposal*. |
| 📱 **Verifikasi Fisik Lambat**<br>*(Slow Verification)* | Staf harus mengetik nomor seri panjang secara manual saat melakukan pengecekan di lapangan. | **Global QR Barcode Scanner** berbasis kamera langsung di aplikasi & fitur **Print Labels** ramah cetak. |

---

## 📖 Konsep Dasar

Bagi pengguna baru, berikut adalah tiga pilar utama cara kerja sistem ini:

* 🏢 **Multi-Warehouse (Multi-Gudang):** Mengelola banyak lokasi penyimpanan sekaligus—baik gudang fisik (seperti *Ruang Server* atau *Gudang Utama*) maupun gudang virtual (seperti status *"Transit"* saat barang sedang dikirim antar-cabang).
* 🏷️ **Serial Number (SN) Tracking:** Setiap unit perangkat bernilai tinggi (laptop, monitor, router) memiliki nomor seri unik yang dipantau pergerakannya secara individual.
* 🔄 **Stock Mutation (Mutasi Stok):** Setiap aktivitas barang masuk, keluar, peminjaman, maupun transfer otomatis tercatat dalam kartu stok kronologis (*Audit Trail*) lengkap dengan nama penanggung jawab dan keterangan transaksi.

---

## 🚀 Fitur Unggulan

* 🏭 **Manajemen Gudang Terintegrasi:** Pengaturan gudang fisik dan virtual, status aktif/nonaktif, serta penanggung jawab (PIC) di tiap lokasi.
* 📦 **Katalog Produk Cerdas:** Pengelompokan barang berdasarkan tipe (*Inventory* untuk stok umum, *Consumable* untuk barang habis pakai, dan *Asset* untuk inventaris kerja) lengkap dengan satuan unit (*UoM*).
* ⚡ **Auto-Generate SKU & Kode Gudang:** Pembuatan Kode Gudang (contoh: `GDG-JKT-01`) dan nomor SKU secara otomatis dan berurutan (*sequential*).
* ✅ **Asset Assignment Approval:** Alur verifikasi penugasan barang berseri maupun non-seri kepada Karyawan, Departemen, atau Klien sebelum stok resmi berkurang.
* 📅 **Peminjaman Cerdas & Monitoring Overdue:** Peminjaman aset jangka pendek ber-SN dengan pencarian cepat, pelacakan jatuh tempo, serta notifikasi peringatan di *navbar*.
* 🔀 **Transfer Langsung (Direct Transfer):** Pemindahan aset secara langsung antar-karyawan atau antar-departemen (misal: *Andi → Budi*, *IT → Operational*) tanpa perlu proses *return* ke gudang terlebih dahulu.
* ⏳ **Siklus Hidup Aset (Lifecycle Timeline):** Visualisasi lini masa lengkap untuk setiap nomor seri (*Receipt, Assignment, Loan, Transfer, Repair, Disposal, Lost*).
* 📷 **Global QR Scanner & Cetak Label:** Pemindai QR Code via kamera perangkat langsung dari *navbar* untuk cek riwayat barang di lapangan, disertai template cetak stiker QR.
* 📊 **Laporan Aset Konsolidasi (5 Dimensi):** Dashboard analitik interaktif yang menyajikan sebaran aset per Departemen, per Karyawan, per Customer, utilitas stok, dan daftar peminjaman terlambat.
* 🎛️ **Filter Multi-Gudang Real-Time:** Penyaringan data instan berdasarkan gudang aktif yang terintegrasi di 7 modul utama.
* 🔐 **Role-Based Access Control (RBAC):** Pembatasan hak akses menu, tombol aksi, dan navigasi *sidebar* berdasarkan peran pengguna (*Super Admin, Manager, Staff*).
* 🛡️ **Audit Activity Log:** Pencatatan log keamanan mendalam yang merekam detail perubahan data sebelum dan sesudah diperbarui (*Old vs New Payloads*).

---

## 🛠️ Tech Stack & Persyaratan Sistem

Aplikasi ini dibangun menggunakan kombinasi teknologi modern untuk menjamin performa tinggi, keamanan data, dan kemudahan penggunaan:

| Kategori | Teknologi | Fungsi dalam Aplikasi | Versi Minimal |
| :--- | :--- | :--- | :--- |
| **Backend & Framework** | ![.NET](https://img.shields.io/badge/.NET_Core_MVC-512BD4?style=flat-square&logo=dotnet&logoColor=white) | Arsitektur utama aplikasi web (*Model-View-Controller*), pemrosesan logika bisnis, keamanan, dan *routing*. | `.NET 9.0` |
| **Frontend & UI** | ![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=flat-square&logo=bootstrap&logoColor=white) ![HTML5](https://img.shields.io/badge/HTML5_&_CSS3-E34F26?style=flat-square&logo=html5&logoColor=white) | Membangun tata letak antarmuka yang responsif, modern, dan rapi di berbagai ukuran layar. | `Bootstrap 5.x` |
| **Interaktivitas & Async** | ![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=flat-square&logo=javascript&logoColor=black) ![jQuery](https://img.shields.io/badge/jQuery_&_AJAX-0769AD?style=flat-square&logo=jquery&logoColor=white) | Manipulasi DOM dinamis, pemindaian QR kamera, validasi form, dan pertukaran data asinkron tanpa *reload* halaman. | `ES6+ / jQuery 3.x` |
| **Notifikasi UI** | 🔔 **Toastr.js** | Menampilkan notifikasi *pop-up* (sukses, peringatan, error) yang interaktif dan elegan kepada pengguna. | `v2.1+` |
| **Database Engine** | ![MySQL](https://img.shields.io/badge/MySQL_/_MariaDB-4479A1?style=flat-square&logo=mysql&logoColor=white) ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white) | Penyimpanan data relasional untuk master barang, mutasi stok, nomor seri, dan *audit log*. | `MySQL ^8.0` / `MariaDB ^10.4` |
| **Runtime & SDK** | ⚙️ **.NET SDK & Hosting Bundle** | Lingkungan kompilasi kode (*SDK*) saat pengembangan dan *runtime* server (*Hosting Bundle*) saat *deployment*. | `.NET 9.0 SDK / Runtime`[cite: 2] |
| **Containerization** | ![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white) *(Opsional)* | Membungkus aplikasi ke dalam *container* agar mudah dijalankan di server tanpa instalasi *Hosting Bundle* secara manual. | `v20.10+` |

---

## 🌐 Live Demo

Ingin mencoba langsung fitur-fitur di atas? Kunjungi halaman demo resmi aplikasi melalui tautan berikut:

👉 **[http://imgm.runasp.net/](http://imgm.runasp.net/)**

---

## 💻 Panduan Instalasi (Lokal)

1. **Clone Repositori**
   ```bash
   git clone [https://github.com/username-anda/InventoryMultiGudang.git](https://github.com/username-anda/InventoryMultiGudang.git)
   cd InventoryMultiGudang
