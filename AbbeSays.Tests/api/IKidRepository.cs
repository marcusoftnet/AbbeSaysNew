using System;
using System.Collections.Generic;

namespace AbbeSays.Tests.api
{
    public interface IKidRepository
    {
        Kid GetKid(int id);

        IList<Kid> GetKids(string p);

        Kid CreateKid(Kid kid);
    }

    public class KidRepository : IKidRepository
    {
        public Kid GetKid(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Kid> GetKids(string p)
        {
            throw new NotImplementedException();
        }

        public Kid CreateKid(Kid kid)
        {
            throw new NotImplementedException();
        }
    }
}