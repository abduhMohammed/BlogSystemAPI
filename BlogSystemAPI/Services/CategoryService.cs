using AutoMapper;
using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.UnitOfWork;

namespace BlogSystemAPI.Services
{
    public class CategoryService
    {
        private readonly UnitWork unit;
        private readonly IMapper mapper;

        public CategoryService(UnitWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        public List<CategoryDTO> GetAll()
        {
            var categories = unit.CategoryRepository.GetAll();
            if (!categories.Any())
                return new List<CategoryDTO>();

            return mapper.Map<List<CategoryDTO>>(categories);
        }

        public CategoryDTO GetById(int id)
        {
            Category category = unit.CategoryRepository.GetById(id);
            if (category == null) return null;

            return mapper.Map<CategoryDTO>(category);
        }

        public CategoryDTO Add(CategoryDTO categoryDTO)
        {
            Category category = mapper.Map<Category>(categoryDTO);

            unit.CategoryRepository.Add(category);
            unit.Save();

            return mapper.Map<CategoryDTO>(category);
        }

        public String Update(CategoryDTO categoryDTO)
        {
            var existingCat = unit.CategoryRepository.GetById(categoryDTO.Id);

            if (existingCat == null) 
                return "Category not found.";

            if (existingCat.Name == categoryDTO.Name)
                return "No Changes";

            mapper.Map(categoryDTO, existingCat);

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