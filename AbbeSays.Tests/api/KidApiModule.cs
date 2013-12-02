using Nancy;

namespace AbbeSays.Tests.api
{
    public class KidApiModule : NancyModule
    {
        public KidApiModule(IKidRepository kidRepository)
        {
            Get["/Kid/{id}"] = parameter => kidRepository.GetKid(parameter.Id);
            Get["/Kid/Family/{family}"] = parameter => kidRepository.GetKids(parameter.Family);
        }
    }
}