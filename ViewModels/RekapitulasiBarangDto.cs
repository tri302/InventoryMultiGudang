public class RekapitulasiBarangDto
{
    public string Sku { get; set; }
    public string NamaProduk { get; set; }
    public string Kategori { get; set; }
    public decimal TotalStokSistem { get; set; }
    public decimal TotalStokFisik { get; set; }
    public decimal SelisihAkumulasi { get; set; }
    public int FrekuensiSelisih { get; set; }
}