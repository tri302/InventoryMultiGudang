using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Repository
{
    public interface IPenugasanAsetRepository
    {
        Task<PagedResultPenugasanAset<PenugasanAsetIndexViewModel>> GetPagedAsync(string? search, int? status, int pageIndex, int pageSize);
        Task<PenugasanAset?> GetByIdAsync(int id);
        Task<PenugasanAsetFormViewModel?> GetViewModelByIdAsync(int id);
        Task CreateAsync(PenugasanAset model);
        Task UpdateAsync(PenugasanAset model);
        Task DeleteAsync(int id);
        Task<string> GenerateKodePenugasanAsync();
        Task ProcessPengembalianAsync(int id, DateTime tanggalKembali, List<PenugasanAsetDetailViewModel> updatedDetails);
        Task<List<PenerimaSelectOptionDto>> GetPenerimaOptionsAsync(TipePenerima tipe, string? search);        
        Task<List<AsetAvailableDto>> GetAvailableAssetsAsync(string? search, string? type);
        Task<AsetAvailableDto?> GetAssetBySerialAsync(string serialNumber);        
        Task<List<AsetAssignedOptionDto>> GetAssignedAssetsAsync(string? search);
        Task<AsetAssignedDetailDto?> GetAssignedAssetDetailAsync(int produkItemId);
        Task ProcessTransferAssetAsync(TransferAssetRequestDto dto);
        Task ApprovePenugasanAsync(int id);
        Task RejectPenugasanAsync(int id);       
        Task<string> GetNamaPenyerahAsync(string? loggedInUser); 
        Task<PenugasanAsetPrintViewModel?> GetPrintDepartmentViewModelAsync(int id, string? loggedInUser);
        Task<PenugasanAsetPrintKaryawanViewModel?> GetPrintKaryawanViewModelAsync(int id, string? loggedInUser);
        Task<PenugasanAsetPrintCustomerViewModel?> GetPrintCustomerViewModelAsync(int id, string? loggedInUser);
    }
}