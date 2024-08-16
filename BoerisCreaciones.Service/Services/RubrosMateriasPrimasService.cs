using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Service.Services
{
    public class RubrosMateriasPrimasService : IRubrosMateriasPrimasService
    {
        private readonly IRubrosMateriasPrimasRepository _repository;

        public RubrosMateriasPrimasService(IRubrosMateriasPrimasRepository repository)
        {
            _repository = repository;
        }

        public List<RubroMateriaPrima> GetRawMaterialsCategories()
        {
            return _repository.GetRawMaterialsCategories();
        }

        public RubroMateriaPrima GetRawMaterialsCategory(int id)
        {
            return _repository.GetRawMaterialsCategory(id);
        }

        public void CreateRawMaterialCategory(string category)
        {
            category = char.ToUpper(category[0]) + category.Substring(1);
            _repository.CreateRawMaterialCategory(category);
        }

        public void ModifyRawMaterialCategory(RubroMateriaPrima category, List<string> attributesToChange)
        {
            category.nombre = char.ToUpper(category.nombre[0]) + category.nombre.Substring(1);
            _repository.ModifyRawMaterialCategory(category, attributesToChange);
        }

        public void DeleteRawMaterialCategory(int id)
        {
            _repository.DeleteRawMaterialCategory(id);
        }
    }
}
