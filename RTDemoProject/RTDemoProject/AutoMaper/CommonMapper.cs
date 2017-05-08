using AutoMapper;
using RTDemoProject.Entities.POCOs;

namespace RTDemoProject.AutoMaper
{
    public class CommonMapper
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.AddProfile(new MainProfile());
                //x.CreateMap<T, E>();
                
            });
            return config;
        }
    }
}
