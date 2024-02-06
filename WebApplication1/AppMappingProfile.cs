using AutoMapper;
using WebApplication1.Comments;
using WebApplication1.News;
using WebApplication1.Sessions;
using WebApplication1.Users;

namespace WebApplication1
{
    public class AppMappingProfile: Profile
    {
        public AppMappingProfile() 
        {
            CreateMap<UserEntity, UserDTO>();
            CreateMap<NewsEntity, NewsDTO>();
            CreateMap<CommentsEntity, CommentsDTO>();
            CreateMap<SessionTokenEntity, TokenDTO>();
        }
    }
}
