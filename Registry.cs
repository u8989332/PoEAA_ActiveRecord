using System.Collections.Generic;

namespace PoEAA_ActiveRecord
{
    internal class Registry
    {
        private static readonly Registry Instance = new Registry();
        private readonly Dictionary<int, Person> _personsMap = new Dictionary<int, Person>();

        private Registry()
        {

        }

        public static void AddPerson(Person person)
        {
            Instance._personsMap.Add(person.Id, person);
        }

        public static Person GetPerson(int id)
        {
            if (Instance._personsMap.ContainsKey(id))
            {
                return Instance._personsMap[id];
            }

            return null;
        }
    }
}