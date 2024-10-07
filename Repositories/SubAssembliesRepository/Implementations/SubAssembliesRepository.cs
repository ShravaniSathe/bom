using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using bom.Models.SubAssemblies;
using bom.Repositories.SubAssemblies.Abstractions;
using Dapper;

namespace bom.Repositories.SubAssemblies.Implementations
{
    public class SubAssembliesRepository : Repository<SubAssemblie>, ISubAssembliesRepository
    {
        public SubAssembliesRepository(string connectionString) :
            base(connectionString)
        {
        }

        public async Task<SubAssemblie> AddSubAssemblyAsync(SubAssemblie subAssembly)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    // Insert the SubAssemblie first
                    const string insertSubAssemblieSql = @"
                INSERT INTO dbo.SubAssemblies 
                    (ItemMasterSalesId, ItemName, UOM, CostPerUnit, Level, ParentSubAssemblyId, PType) 
                VALUES 
                    (@ItemMasterSalesId, @ItemName, @UOM, @CostPerUnit, @Level, @ParentSubAssemblyId, @PType); 
                SELECT CAST(SCOPE_IDENTITY() as int)";

                    var id = await db.QueryAsync<int>(insertSubAssemblieSql, new
                    {
                        ItemMasterSalesId = subAssembly.ItemMasterSalesId,
                        ItemName = subAssembly.ItemName,
                        UOM = subAssembly.UOM,
                        CostPerUnit = subAssembly.CostPerUnit,
                        Level = subAssembly.Level,
                        ParentSubAssemblyId = subAssembly.ParentSubAssemblyId,
                        PType = subAssembly.PType
                    }, transaction: tran);

                    // Set the generated SubAssembly ID
                    subAssembly.Id = id.Single();

                    // Insert RawMaterials for the SubAssembly
                    if (subAssembly.RawMaterials != null && subAssembly.RawMaterials.Any())
                    {
                        const string insertRawMaterialSql = @"
                    INSERT INTO dbo.ItemMasterRawMaterials
                        (SubAssemblyId, ItemName, ItemCode, Grade, UOM, Quantity, Level, PType, CostPerUnit)
                    VALUES
                        (@SubAssemblyId, @ItemName, @ItemCode, @Grade, @UOM, @Quantity, @Level, @PType, @CostPerUnit);";

                        foreach (var rawMaterial in subAssembly.RawMaterials)
                        {
                            rawMaterial.SubAssemblyId = subAssembly.Id; // Set the correct SubAssemblyId for each raw material

                            await db.ExecuteAsync(insertRawMaterialSql, new
                            {
                                SubAssemblyId = rawMaterial.SubAssemblyId,
                                ItemName = rawMaterial.ItemName,
                                ItemCode = rawMaterial.ItemCode,
                                Grade = rawMaterial.Grade,
                                UOM = rawMaterial.UOM,
                                Quantity = rawMaterial.Quantity,
                                Level = rawMaterial.Level,
                                PType = rawMaterial.PType,
                                CostPerUnit = rawMaterial.CostPerUnit
                            }, transaction: tran);
                        }
                    }

                    // Insert Child SubAssemblies (if any)
                    if (subAssembly.ChildSubAssemblies != null && subAssembly.ChildSubAssemblies.Any())
                    {
                        foreach (var childSubAssembly in subAssembly.ChildSubAssemblies)
                        {
                            childSubAssembly.ParentSubAssemblyId = subAssembly.Id; // Set the parent subassembly ID
                            await AddSubAssemblyRecursiveAsync(childSubAssembly, tran);  // Recursive call to insert child subassembly with the same transaction
                        }
                    }

                    CommitTransaction(tran);
                    return subAssembly;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw; // Rethrow the exception to propagate the error
                }
            }
        }

        private async Task AddSubAssemblyRecursiveAsync(SubAssemblie subAssembly, IDbTransaction tran)
        {
            // Insert the SubAssemblie
            const string insertSubAssemblieSql = @"
        INSERT INTO dbo.SubAssemblies 
            (ItemMasterSalesId, ItemName, UOM, CostPerUnit, Level, ParentSubAssemblyId, PType) 
        VALUES 
            (@ItemMasterSalesId, @ItemName, @UOM, @CostPerUnit, @Level, @ParentSubAssemblyId, @PType); 
        SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await db.QueryAsync<int>(insertSubAssemblieSql, new
            {
                ItemMasterSalesId = subAssembly.ItemMasterSalesId,
                ItemName = subAssembly.ItemName,
                UOM = subAssembly.UOM,
                CostPerUnit = subAssembly.CostPerUnit,
                Level = subAssembly.Level,
                ParentSubAssemblyId = subAssembly.ParentSubAssemblyId,
                PType = subAssembly.PType
            }, transaction: tran);

            // Set the generated SubAssembly ID
            subAssembly.Id = id.Single();

            // Insert RawMaterials for the SubAssembly
            if (subAssembly.RawMaterials != null && subAssembly.RawMaterials.Any())
            {
                const string insertRawMaterialSql = @"
            INSERT INTO dbo.ItemMasterRawMaterials
                (SubAssemblyId, ItemName, ItemCode, Grade, UOM, Quantity, Level, PType, CostPerUnit)
            VALUES
                (@SubAssemblyId, @ItemName, @ItemCode, @Grade, @UOM, @Quantity, @Level, @PType, @CostPerUnit);";

                foreach (var rawMaterial in subAssembly.RawMaterials)
                {
                    rawMaterial.SubAssemblyId = subAssembly.Id; // Set the correct SubAssemblyId for each raw material

                    await db.ExecuteAsync(insertRawMaterialSql, new
                    {
                        SubAssemblyId = rawMaterial.SubAssemblyId,
                        ItemName = rawMaterial.ItemName,
                        ItemCode = rawMaterial.ItemCode,
                        Grade = rawMaterial.Grade,
                        UOM = rawMaterial.UOM,
                        Quantity = rawMaterial.Quantity,
                        Level = rawMaterial.Level,
                        PType = rawMaterial.PType,
                        CostPerUnit = rawMaterial.CostPerUnit
                    }, transaction: tran);
                }
            }

            // Insert Child SubAssemblies (if any)
            if (subAssembly.ChildSubAssemblies != null && subAssembly.ChildSubAssemblies.Any())
            {
                foreach (var childSubAssembly in subAssembly.ChildSubAssemblies)
                {
                    childSubAssembly.ParentSubAssemblyId = subAssembly.Id; // Set the parent subassembly ID for the child
                    await AddSubAssemblyRecursiveAsync(childSubAssembly, tran);  // Recursive call for child subassemblies
                }
            }
        }




        public async Task<bool> DeleteSubAssemblyAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteSubAssemblyById";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new { Id = id },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<List<SubAssemblie>> GetAllSubAssembliesAsync()
        {
            const string storedProcedureName = "dbo.GetAllSubAssemblies";

            var result = await db.QueryAsync(storedProcedureName,
                                              commandType: CommandType.StoredProcedure
                                              ).ConfigureAwait(false);

            var retVal = result.ToList();

            List<SubAssemblie> subAssembliesList = new List<SubAssemblie>();
            foreach (var rawSubAssemblieObj in retVal)
            {
                SubAssemblie subAssemblie = await GetSubAssemblieObjectFromResult(rawSubAssemblieObj);
                subAssembliesList.Add(subAssemblie);
            }

            return subAssembliesList;
        }

        public async Task<SubAssemblie> GetSubAssemblyAsync(int id)
        {
            const string storedProcedureName = "dbo.GetSubAssemblyById";

            var result = await db.QueryAsync(storedProcedureName,
                                             new { Id = id },
                                             commandType: CommandType.StoredProcedure
                                             ).ConfigureAwait(false);

            if (result != null && result.Any())
            {
                SubAssemblie subAssemblie = await GetSubAssemblieObjectFromResult(result.FirstOrDefault()).ConfigureAwait(false);
                return subAssemblie;
            }
            else
            {
                return null;
            }
        }

        public async Task<SubAssemblie> UpdateSubAssemblyAsync(SubAssemblie subAssembly)
        {
            const string storedProcedureName = "dbo.UpdateSubAssembly";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new
                                               {
                                                   SubAssemblyId = subAssembly.Id,
                                                   NewItemMasterSalesId = subAssembly.ItemMasterSalesId,
                                                   NewItemName = subAssembly.ItemName,
                                                   NewUOM = subAssembly.UOM,
                                                   NewCostPerUnit = subAssembly.CostPerUnit,
                                                   NewLevel = subAssembly.Level,
                                                   NewParentSubAssemblyId = subAssembly.ParentSubAssemblyId,
                                                   NewPType = subAssembly.PType
                                               },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return subAssembly;
        }

        private async Task<SubAssemblie> GetSubAssemblieObjectFromResult(dynamic rawSubAssemblieObject)
        {
            SubAssemblie subAssemblie = new SubAssemblie
            {
                Id = rawSubAssemblieObject?.Id ?? 0,
                ItemMasterSalesId = rawSubAssemblieObject?.ItemMasterSalesId ?? 0,
                ItemName = rawSubAssemblieObject?.ItemName ?? string.Empty,
                UOM = rawSubAssemblieObject?.UOM ?? string.Empty,
                CostPerUnit = rawSubAssemblieObject?.CostPerUnit ?? 0,
                Level = rawSubAssemblieObject?.Level ?? 0,
                ParentSubAssemblyId = rawSubAssemblieObject?.ParentSubAssemblyId,
                PType = rawSubAssemblieObject?.PType ?? string.Empty
            };

            return subAssemblie;
        }
    }
}
