using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.UnitOfWork;

namespace BlogSystemAPI.Services
{
    public class CategoryService
    {
        UnitWork unit;
        public CategoryService(UnitWork unit)
        {
            this.unit = unit;
        }

        public List<CategoryDTO> GetAll()
        {
            List<Category> categories = unit.CategoryRepository.GetAll();
            if (categories.Count == 0)
                return new List<CategoryDTO>();

            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            foreach(var category in categories)
            {
                CategoryDTO categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                };
                categoryDTOs.Add(categoryDTO);
            }
            return categoryDTOs;
        }

        public CategoryDTO GetById(int id)
        {
            Category category = unit.CategoryRepository.GetById(id);
            if (category == null) return null;
            else
            {
                CategoryDTO categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                };
                return categoryDTO;
            }
        }

        public CategoryDTO Add(CategoryDTO categoryDTO)
        {
            Category category = new Category()
            {
                Id = categoryDTO.Id,
                Name = categoryDTO.Name
            };

            unit.CategoryRepository.Add(category);
            unit.Save();

            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public String Update(int id, CategoryDTO categoryDTO)
        {
            var existingCat = unit.CategoryRepository.GetById(id);

            if (existingCat == null) return "Not Found";
            if (existingCat.Id != id) return "Your ID is not match with the Saved Id";

            if (existingCat.Name == categoryDTO.Name)
                return "No Changes";

            existingCat.Name = categoryDTO.Name;

            unit.CategoryRepository.Update(existingCat);
            unit.Save();

            return "Updated";
        }

        public bool Delete(int id)
        {
            var existingCateg = unit.CategoryRepository.GetById(id);

            if (existingCateg == null) return false;

            unit.CategoryRepository.Delete(existingCateg);
            unit.Save();

            return true;
        }
    }
}