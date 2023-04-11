using AutoMapper;
using PhoneBookBusinessLayer.ImplementationsOfManagers;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookDataLayer.InterfaceOfRepo;
using PhoneBookEntityLayer.Entities;
using PhoneBookEntityLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookBusinessLayer.ImplementatinOfManagers
{
    public class PhoneTypeManager : Manager<PhoneTypeViewModel, PhoneType, byte>, IPhoneTypeManager
    {

        public PhoneTypeManager(IPhoneTypeRepository repo, IMapper mapper) : base(repo, mapper, null)
        {

        }
    }
}
