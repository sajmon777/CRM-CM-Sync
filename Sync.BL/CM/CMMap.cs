using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sync.BL.Abstract;
using AutoMapper;
using createsend_dotnet;

namespace Sync.BL.CM
{
    class CMMap: IMap
    {
        public void CreateMaps()
        {
            Mapper.CreateMap<BasicClient, Entities.Client>().
                     ForMember(dto => dto.ClientId, opt => opt.MapFrom(src => src.ClientID)).
                     ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name));
            Mapper.CreateMap<BasicList, Entities.List>().
                    ForMember(dto => dto.ListId, opt => opt.MapFrom(src => src.ListID)).
                    ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name));
            Mapper.CreateMap<SubscriberDetail, Entities.Subscriber>().
                    ForMember(dto => dto.E_mail, opt => opt.MapFrom(src => src.EmailAddress)).
                    ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name));
            Mapper.CreateMap<Entities.Subscriber, SubscriberDetail>().
                    ForMember(dto => dto.EmailAddress, opt => opt.MapFrom(src => src.E_mail)).
                    ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
