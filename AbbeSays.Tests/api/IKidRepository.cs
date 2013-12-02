using System;
using System.Collections.Generic;

namespace AbbeSays.Tests.api
{
    public interface IKidRepository
    {
        Kid GetKid(int id);

        IList<Kid> GetKids(string p);
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
    }
}