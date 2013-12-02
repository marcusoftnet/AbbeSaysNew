using System;
using Nancy;
using Nancy.Testing;
using NSubstitute;
using Should.Fluent;
using Xunit;

namespace AbbeSays.Tests.api
{
    public class crud_operations_on_kids
    {
        private Browser _browser;
        private readonly IKidRepository _repository;

        public crud_operations_on_kids()
        {
            _repository = Substitute.For<IKidRepository>();
            _browser = new Browser(with =>
            {
                with.Module<KidApiModule>();
                with.Dependency<IKidRepository>(_repository);
            });
        }

        [Fact]
        public void should_get_an_existing_kid()
        {
            // Arrange
            _repository.GetKid(123).Returns(new Kid {Id = 123, Name = "Albert", BirthDate =DateTime.Parse("2008-10-24")});

            // Act
            var resposne = _browser.Get("/Kid/123", context =>
            {
                context.Header("Accept", "application/json");
            });

            // Assert
            resposne.StatusCode.Should().Equal(HttpStatusCode.OK);
            var kid = resposne.Body.DeserializeJson<Kid>();
            kid.Id.Should().Equal(123);
            kid.Name.Should().Equal("Albert");
            kid.BirthDate.Should().Equal(DateTime.Parse("2008-10-24"));
        }
    }

    public class Kid
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public interface IKidRepository
    {
        Kid GetKid(int id);
    }

    public class KidRepository : IKidRepository
    {
        public Kid GetKid(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class KidApiModule : NancyModule
    {
        public KidApiModule(IKidRepository kidRepository)
        {
            Get["/Kid/{id}"] = parameter =>
            {
                return kidRepository.GetKid(parameter.Id);
            };
        }
    }
}
