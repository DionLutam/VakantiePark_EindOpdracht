using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Exceptions;
using VakantieParkBL.Model;

namespace UnitTestsModel
{
    public class UnitTestHuis
    {
        [Theory]
        [InlineData(-1, "Dunantlaan", 10, 4, "NegatiefIdPark")] 
        [InlineData(1, "", 10, 4, "StraatIsLeegPark")]              
        [InlineData(1, "Sint-Pietersplein", -10, 4, "NegatiefHuisnummerPark")] 
        [InlineData(1, "Korenmarkt", 10, 0, "CapaciteitMinderdan1Park")]
        [InlineData(1, "Korenmarkt", 10, 0, null)]
        public void Ctor_HuisInvalid(int id, string straat, int nummer, int capaciteit, string parkNaam)
        {

            Park park = parkNaam != null ? new Park(1, "Park", "City", new List<Faciliteit> {new Faciliteit( 1,"zwembad")}) : null;


            Assert.Throws<ModelException>(() => new Huis(id, straat, nummer, true, capaciteit, park));

        }

        [Fact]
        public void Ctor_HuisValid_ShouldInitializeCorrectly()
        {
            int id = 1;
            string straat = "Korenmarkt";
            int nummer = 10;
            bool IsActief = true;
            int capaciteit = 4;
            Park park = new Park(1, "Citadel Park", "Gent", new List<Faciliteit> { new Faciliteit(1, "zwembad") });

            Huis huis = new Huis(id, straat, nummer, IsActief, capaciteit, park);

            Assert.Equal(id, huis.ID);
            Assert.Equal(straat, huis.Straat);
            Assert.Equal(nummer, huis.Nummer);
            Assert.Equal(IsActief, huis.IsActief);
            Assert.Equal(capaciteit, huis.Capaciteit);
            Assert.Equal(park, huis.Park);
        }



    }
}
