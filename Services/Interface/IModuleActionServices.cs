using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IModuleActionServices
    {
        public ModuleActions GetModuleActionsByID(int id);
        public List<ModuleActions> GetAllModuleActions();
        public string AddModuleActions(ModuleActions moduleActions);
        public string EditModuleActions(ModuleActions moduleActions);
        public string DeleteModuleActions(int id);
    }
}
