using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    [Table("reparasi_aset")]
    public class ReparasiAsset
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Unit Aset / Serial Number wajib dipilih.")]
        public int ProdukItemId { get; set; }

        [Required(ErrorMessage = "Tanggal masuk wajib diisi.")]
        public DateTime TanggalMasuk { get; set; } = DateTime.Now;

        public DateTime? TanggalKeluar { get; set; }

        [Required(ErrorMessage = "Keluhan kerusakan wajib diisi.")]
        [StringLength(500, ErrorMessage = "Keluhan maksimal 500 karakter.")]
        public string Keluhan { get; set; }

        [Column(TypeName = "decimal(18,2)]")]
        [Range(0, 999999999, ErrorMessage = "Biaya tidak valid.")]
        public decimal Biaya { get; set; }

        // Status: 0 = Proses Reparasi, 1 = Selesai, 2 = Dibatalkan
        [Required]
        public int StatusReparasi { get; set; } = 0;

        public string KondisiBarang { get; set; } = "Rusak";
    }
}