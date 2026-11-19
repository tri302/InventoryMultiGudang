public class ReparasiAssetViewModel
    {
        public int Id { get; set; }
        public int ProdukItemId { get; set; }
        public string SerialNumber { get; set; }
        public string NamaBarang { get; set; }
        public string TanggalMasuk { get; set; }
        public string TanggalKeluar { get; set; }
        public string Keluhan { get; set; }
        public decimal Biaya { get; set; }
        public int StatusReparasi { get; set; }
        public string StatusLabel { get; set; }
    }