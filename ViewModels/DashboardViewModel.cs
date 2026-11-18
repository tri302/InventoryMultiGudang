using System;
using System.Collections.Generic;

namespace InventoryMultiGudang.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalValuasiFinansial { get; set; }
        public int TotalUnitAsetSn { get; set; }
        public int TotalAsetCustody { get; set; }
        public int TotalPeminjamanOverdue { get; set; }
        public int TotalVarianSku { get; set; }
        public int TotalStokFisik { get; set; }
        public int TotalGudangAktif { get; set; }
        public int TotalAsetDipinjam { get; set; }
        public int TotalStokKritis { get; set; }
        public int TotalVarianHabis { get; set; }

        public List<OverdueLoanViewModel> OverdueLoanList { get; set; } = new();
        public List<StokKritisViewModel> StokKritisList { get; set; } = new();
        public List<GudangDropdownViewModel> ListGudang { get; set; } = new();
        public List<OverduePeminjamanViewModel> ListOverdue { get; set; } = new();
        public List<StokKritisViewModel> ListStokKritis { get; set; } = new();
        public List<AktivitasMutasiViewModel> ListAktivitasMutasi { get; set; } = new();
        public List<PenguasaanDepartemenViewModel> PenguasaanDepartemenList { get; set; } = new();
        public KomposisiValuasiTipeViewModel KomposisiValuasiTipe { get; set; } = new();

        // Properti Paging
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        // Data Grafik Mutasi Harian (Line/Area Chart)
        public GrafikMutasiHarianViewModel GrafikMutasi { get; set; } = new();
        // Data Grafik Status Aset (Donut Chart)
        public GrafikStatusAsetViewModel GrafikStatusAset { get; set; } = new();
        public List<ValuasiGudangViewModel> ValuasiGudangList { get; set; } = new();
        public List<DistribusiKategoriViewModel> DistribusiKategoriList { get; set; } = new();

    }

    public class OverdueLoanViewModel
    {
        public string NomorDokumen { get; set; } = string.Empty;
        public string NamaPeminjam { get; set; } = string.Empty;
        public string NamaBarang { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string BatasKembali { get; set; } = string.Empty;
        public int HariKeterlambatan { get; set; }
    }

    public class StokKritisViewModel
    {
        public string NamaProduk { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string NamaGudang { get; set; } = string.Empty;
        public int StokSaatIni { get; set; }
        public int SafetyStock { get; set; }
        public int Persentase { get; set; }
    }

    public class PenguasaanDepartemenViewModel
    {
        public string NamaDepartemen { get; set; } = string.Empty;
        public int JumlahAset { get; set; }
    }

    public class KomposisiValuasiTipeViewModel
    {
        public decimal ValuasiInventory { get; set; }
        public decimal ValuasiConsumable { get; set; }
        public decimal ValuasiAsset { get; set; }
        public decimal TotalValuasi => ValuasiInventory + ValuasiConsumable + ValuasiAsset;
    }

    public class ValuasiGudangViewModel
    {
        public string NamaGudang { get; set; } = string.Empty;
        public decimal Valuasi { get; set; }
    }

    public class DistribusiKategoriViewModel
    {
        public string KategoriName { get; set; } = string.Empty;
        public int VarianSku { get; set; }
        public int JumlahAset { get; set; }
    }

    public class GrafikMutasiHarianViewModel
    {
        public List<string> Dates { get; set; } = new();
        public List<int> StockIn { get; set; } = new();
        public List<int> StockOut { get; set; } = new();
        public List<int> Transfer { get; set; } = new();
        public List<int> Opname { get; set; } = new();
    }

    public class GrafikStatusAsetViewModel
    {
        public int ReadyGudang { get; set; }
        public int AssignedCustody { get; set; }
        public int OnLoan { get; set; }
        public int UnderRepair { get; set; }
        public int Lost { get; set; }
        public int SoldDisposal { get; set; }
        public int InTransit { get; set; }
        public int TotalAset => ReadyGudang + AssignedCustody + OnLoan + UnderRepair + Lost + SoldDisposal + InTransit;
    }

    public class OverduePeminjamanViewModel
    {
        public string KodeDokumen { get; set; }
        public string NamaPeminjam { get; set; }
        public string NamaBarang { get; set; }
        public string SerialNumber { get; set; }
        public string BatasKembali { get; set; }
        public int HariKeterlambatan { get; set; }
    }

    public class AktivitasMutasiViewModel
    {
        public string NamaProduk { get; set; }
        public string Sku { get; set; }
        public string NamaGudang { get; set; }
        public string TipeTransaksi { get; set; }
        public int QtyMutasi { get; set; }
        public string SaldoStok { get; set; }
        public string NomorReferensi { get; set; }
        public string WaktuPencatatan { get; set; }

    }
}