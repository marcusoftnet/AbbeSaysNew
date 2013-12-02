using System;
using System.Collections.Generic;
using Nancy;
using Nancy.Testing;
using NSubstitute;
using NSubstitute.Core.Arguments;
using Should.Fluent;
using Xunit;
using System.Linq;

namespace AbbeSays.Tests.api
{

    public class crud_operations_on_kids
    {
        private Browser _browser;
        private readonly IKidRepository _repository;

        private static Action<BrowserContext> AcceptJSONBrowserContext
        {
            get
            {
                return context =>
                {
                    context.Header("Accept", "application/json");
                };
            }
        }

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
            _repository.GetKid(123).Returns(new Kid { Id = 123, Name = "Albert", BirthDate = DateTime.Parse("2008-10-24"), FamilyName = "Hammarberg" });

            // Act
            var resposne = _browser.Get("/Kid/123", AcceptJSONBrowserContext);

            // Assert
            resposne.StatusCode.Should().Equal(HttpStatusCode.OK);
            var kid = resposne.Body.DeserializeJson<Kid>();
            kid.Id.Should().Equal(123);
            kid.Name.Should().Equal("Albert");
            kid.FamilyName.Should().Equal("Hammarberg");
            kid.BirthDate.Should().Equal(DateTime.Parse("2008-10-24"));
        }

        [Fact]
        public void should_get_a_list_of_kids_for_a_family()
        {
            // Arrange
            _repository.GetKids("Hammarberg").Returns(new List<Kid>
            {
                new Kid{Id = 1, FamilyName = "Hammarberg", Name = "Albert"},
                new Kid{Id = 2, FamilyName = "Hammarberg", Name = "Arivd"},
                new Kid{Id = 3, FamilyName = "Hammarberg", Name = "Gustav"}
            });

            // Act
            var response = _browser.Get("/Kid/Family/Hammarberg", AcceptJSONBrowserContext);

            // Assert
            response.StatusCode.Should().Equal(HttpStatusCode.OK);
            var kids = response.Body.DeserializeJson<List<Kid>>();
            kids.Count.Should().Equal(3);
            kids.Any(k => k.FamilyName != "Hammarberg").Should().Equal(false);
        }

        [Fact]
        public void should_create_a_new_kid()
        {
            // Arrange
            var kid = new Kid { Id = 1, Name = "Albert", BirthDate = DateTime.Parse("2008-10-24"), FamilyName = "Hammarberg" };
            _repository.CreateKid(Arg.Any<Kid>()).Returns(kid);

            // Act
            var response = _browser.Post("/Kid", context =>
            {
                context.Header("Accept", "application/json");
                
                context.FormValue("Name", "Albert");
                context.FormValue("FamilyName", "Hammarberg");
                context.FormValue("BirthDate", "2008-10-24");
            });

            // Assert
            response.StatusCode.Should().Equal(HttpStatusCode.Created);
            _repository.Received(1).CreateKid(Arg.Any<Kid>());
            var k = response.Body.DeserializeJson<Kid>();
            k.Id.Should().Equal(kid.Id);
        }
    }
}
