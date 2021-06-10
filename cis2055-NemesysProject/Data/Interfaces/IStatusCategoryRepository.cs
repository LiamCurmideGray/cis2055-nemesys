using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Models;

namespace cis2055_NemesysProject.Data.Interfaces
{
   public interface IStatusCategoryRepository
    {
        IEnumerable<StatusCategory> GetAllStatusCategories();

        StatusCategory GetStatusCategoryById(int statusCategoryId);

        void CreateStatusCategory(StatusCategory entity);

        void UpdateStatusCategory(StatusCategory entity);

        void DeleteStatusCategory(int statusCategoryId);
    }
}
