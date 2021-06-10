using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Data.Interfaces;
using cis2055_NemesysProject.Models;
using Microsoft.EntityFrameworkCore;

namespace cis2055_NemesysProject.Data.Repositories
{
    public class StatusCategoryRepository : IStatusCategoryRepository
    {

        private readonly cis2055nemesysContext context;

        public StatusCategoryRepository(cis2055nemesysContext _context)
        {
            try
            {
                context = _context;

            } catch(Exception ex)
            {
                throw;
            }
        }

        public void CreateStatusCategory(StatusCategory entity)
        {
            try
            {
                context.StatusCategories.Add(entity);
                context.SaveChanges();
            } catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteStatusCategory(int statusCategoryId)
        {
            try
            {
              var statusCategory = context.StatusCategories.FirstOrDefault(st => st.StatusId == statusCategoryId);

                if(statusCategory != null)
                {

                    context.StatusCategories.Remove(statusCategory);
                    context.SaveChanges();
                   
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<StatusCategory> GetAllStatusCategories()
        {
            try
            {
                return context.StatusCategories.OrderBy(st => st.StatusId); 

            } catch (Exception ex)
            {
                throw;
            }
        }

        public StatusCategory GetStatusCategoryById(int statusCategoryId)
        {
            return context.StatusCategories.FirstOrDefault(p => p.StatusId == statusCategoryId);
        }

        public void UpdateStatusCategory(StatusCategory entity)
        {
            try
            {
                var existingstatusCategory = context.StatusCategories.SingleOrDefault(bp => bp.StatusId == entity.StatusId);
                if (existingstatusCategory != null)
                {
                    existingstatusCategory.StatusType = entity.StatusType;
                    
                    context.Entry(existingstatusCategory).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
