using Laptop.Models;

namespace Laptop.Interface
{
    public interface ILaptopRepository
    {
        Task<List<EquipmentModel>> GetLaptopDetails();
        Task<List<EquipmentModel>> GetEquipmentByName(string Name);
        Task<EquipmentModel> GetLaptopById(int id);
        Task<string> Create(EquipmentModel model);
        Task<string> Update(EquipmentModel model, int id);
        Task<string> Delete(int id);
        Task<List<BanksModel>> GetAllBanks();
    }
}
