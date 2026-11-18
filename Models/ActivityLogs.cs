using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    [Table("ActivityLogs")]
    public class ActivityLog
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Causer { get; set; } // Pengguna yang melakukan aksi (User/Username)

        [Required]
        [MaxLength(50)]
        public string Event { get; set; } // Jenis event (CREATE, UPDATE, DELETE, LOGIN, dll)

        [MaxLength(500)]
        public string Description { get; set; } // Deskripsi detail aktivitas

        [MaxLength(150)]
        public string Subject { get; set; } // Objek/Tabel yang terkena dampak

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now; // Waktu kejadian
    }
}