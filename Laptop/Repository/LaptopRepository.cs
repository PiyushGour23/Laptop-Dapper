using AutoMapper;
using Dapper;
using Laptop.Data;
using Laptop.DataModels;
using Laptop.Interface;
using Laptop.Models;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Laptop.Repository
{
    public class LaptopRepository : ILaptopRepository
    {
        private readonly DapperDbContext _dapperdbContext;
        private readonly IMapper _mapper;
        private readonly ILogger <LaptopRepository> _logger;

        public LaptopRepository(DapperDbContext dapperdbContext, IMapper mapper, ILogger<LaptopRepository> logger)
        {
            _dapperdbContext = dapperdbContext ?? throw new ArgumentNullException(nameof(dapperdbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<EquipmentModel>> GetLaptopDetails()
        {
            string sqlquery = "SELECT * FROM Equipment";
            using (var db = _dapperdbContext.CreateConnection())
            {
                var emplist = await db.QueryAsync<EquipmentModel>(sqlquery);
                return emplist.ToList();
            }
        }

        public async Task<List<EquipmentModel>> GetEquipmentByName(string Name)
        {
            string sqlquery = "sp_getequipmentbyname";
            using (var db = _dapperdbContext.CreateConnection())
            {
                var emplist = await db.QueryAsync<EquipmentModel>(sqlquery, new {Name}, commandType:CommandType.StoredProcedure);
                return emplist.ToList();
            }
        }

        public async Task<EquipmentModel> GetLaptopById(int id)
        {
            string sqlquery = "SELECT * FROM Equipment WHERE Id = @Id";
            using (var db = _dapperdbContext.CreateConnection())
            {
                var emplist = await db.QueryFirstOrDefaultAsync<EquipmentModel>(sqlquery, new {id});
                return emplist;
            }
        }

        public async Task<string> Create(EquipmentModel model)
        {
            string sqlquery = "INSERT INTO Equipment (Name,Description,ManufactureDate,ManufactureTime) values (@Name,@Description,@ManufactureDate,@ManufactureTime)";
            string response = string.Empty;
            var parameters = new DynamicParameters();
            parameters.Add("name", model.Name);
            parameters.Add("description", model.Description);
            parameters.Add("manufacturedate", model.ManufactureDate.ToString());
            parameters.Add("manufacturetime", model.ManufactureTime.ToString());
            using (var db = _dapperdbContext.CreateConnection())
            {
                await db.ExecuteAsync(sqlquery, parameters);
                response = "pass";
            }
            return response;
        }

        public async Task<string> Update(EquipmentModel model, int id)
        {
            string sqlquery = "UPDATE Equipment SET Name=@Name, Description=@Description, ManufactureDate=@ManufactureDate, ManufactureTime=@ManufactureTime where id=@id";
            string response = string.Empty;
            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            parameters.Add("name", model.Name);
            parameters.Add("description", model.Description);
            parameters.Add("manufacturedate", model.ManufactureDate.ToString());
            parameters.Add("manufacturetime", model.ManufactureTime.ToString());
            using (var db = _dapperdbContext.CreateConnection())
            {
                await db.ExecuteAsync(sqlquery, parameters);
                response = "pass";
            }
            return response;
        }

        public async Task<string> Delete(int id)
        {
            string sqlquery = "DELETE FROM Equipment WHERE Id = @Id";
            string response = string.Empty;
            using (var db = _dapperdbContext.CreateConnection())
            {
                var emplist = await db.ExecuteAsync(sqlquery, new { id });
                response = "pass";
            }
            return response;
        }


        public async Task<List<BanksModel>> GetAllBanks()
        {
            string sqlquery = "SELECT * FROM Bank";
            using (var db = _dapperdbContext.CreateConnection())
            {
                var emplist = await db.QueryAsync<Banks>(sqlquery);
                var datatypes = _mapper.Map<List<BanksModel>>(emplist);
                return datatypes;

            }
        }
        
    }
}
