using Sync.BL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Xrm;

namespace Sync.BL.CRM
{
    class CRMMap : IMap
    {
        public void CreateMaps()
        {
            Mapper.CreateMap<List, Entities.List>().
                      ForMember(dto => dto.ListId, opt => opt.MapFrom(src => src.ListId)).
                      ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.ListName));
            Mapper.CreateMap<Contact, Entities.Subscriber>().
                         ForMember(dto => dto.E_mail, opt => opt.MapFrom(src => src.EMailAddress1)).
                         ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.FirstName));
            Mapper.CreateMap<Entities.Subscriber, Contact>().
                             ForMember(dto => dto.EMailAddress1, opt => opt.MapFrom(src => src.E_mail)).
                             ForMember(dto => dto.FirstName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
