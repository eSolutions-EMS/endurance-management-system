using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnduranceJudge.Tools.Console
{
    public class UserDto
        {
            public int Id { get; set; }
            public IEnumerable<GroupDto> Groups { get; set; }
        }
        public class GroupDto
        {
            public int Id { get; set; }
            public UserDto User { get; set; }
        }
        public class User
        {
            public int Id { get; set; }
            public ICollection<Group> Groups { get; set; }
        }
        public class Group
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public User User { get; set; }
        }
        public class DataContext : DbContext
        {
            public DbSet<User> Users { get; set; }
            public DbSet<Group> Groups { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
                base.OnConfiguring(optionsBuilder);
            }
        }
        public class UserService
        {
            private readonly DataContext dbContext;
            private readonly IMapper mapper;
            public UserService(DataContext dbContext, IMapper mapper)
            {
                this.dbContext = dbContext;
                this.mapper = mapper;
            }
            public void Save(UserDto dto)
            {
                var entity = this.dbContext.Users.FirstOrDefault(x => x.Id == dto.Id);
                if (entity == null)
                {
                    entity = this.mapper.Map<User>(dto);
                    this.dbContext.Add(entity);
                }
                else
                {
                    this.mapper.Map(dto, entity);
                    this.dbContext.Update(entity);
                }

                this.dbContext.SaveChanges();
            }
        }
        public class AutoMapperCollectionsDemo
        {
            public static void Run()
            {
                var services = ConfigureServices();
                var dbContext = services.GetService<DataContext>();
                SeedData(dbContext);

                var userService = services.GetService<UserService>();

                var userDto = new UserDto
                {
                    Id = 1,
                    Groups = new List<GroupDto>
                    {
                        new GroupDto { Id = 1 },
                        new GroupDto { Id = 2 },
                    }
                };

                userService!.Save(userDto);
            }

            private static IServiceProvider ConfigureServices()
                => new ServiceCollection()
                    .AddDbContext<DataContext>()
                    .AddAutoMapper(
                        config =>
                        {
                            config.AddCollectionMappers();
                            config.CreateProfile(
                                "profile",
                                profile =>
                                {
                                    profile.CreateMap<UserDto, User>()
                                        .EqualityComparison(((dto, user) => dto.Id == user.Id));
                                    profile.CreateMap<GroupDto, Group>()
                                        .EqualityComparison(((dto, group) => dto.Id == group.Id))
                                        .ForMember(x => x.User, opt => opt.Ignore())
                                        .ForMember(x => x.UserId, opt => opt.Ignore());
                                });
                        },
                        Assembly.GetExecutingAssembly())
                    .AddTransient<UserService, UserService>()
                    .BuildServiceProvider();

            private static void SeedData(DataContext dbContext)
            {
                var user = new User
                {
                    Id = 1,
                    Groups = new List<Group>
                    {
                        new Group { Id = 1 },
                        new Group { Id = 2 },
                        new Group { Id = 3 },
                    }
                };

                dbContext!.Users.Add(user);
                dbContext.SaveChanges();
            }
        }
}
