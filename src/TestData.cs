using System.Collections.Generic;

namespace Altinn.TestClient.Testdata
{
    // TODO Move data to JSON file and deserialize instead.
    public static class TT02
    {
        public static List<Person> GetTestPersons()
        {
            return new List<Person>()
            {
                // Tom Heis
                new Person("26899299344", "HEIS")
            };
        }

        public static List<Service> GetServiceCodes()
        {
            return new List<Service>()
            {
                new Service("3225", 45678)
            };
        }
    }

    public class Person
    {
        public string SSN { get; set; }
        public string LastName { get; set; }

        public Person(string ssn, string lastName)
        {
            SSN = ssn;
            LastName = lastName;
        }
    }

    public class Service
    {
        public string ServiceCode { get; set; }
        public int ServiceEditionCode { get; set; }

        public Service(string serviceCode, int serviceEditionCode)
        {
            ServiceCode = serviceCode;
            ServiceEditionCode = serviceEditionCode;
        }
    }


}