using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IModuleControllerServices
    {
        public List<ModuleControllers> GetAllModuleController();

        public ModuleControllers GetModuleControllersByID(int id);

        public string AddModuleController(ModuleControllers moduleControllers);

        public string EditModuleController(ModuleControllers moduleControllers);

        public string DeleteModuleController(int id);
    }
}
