using VakantieParkBL.Exceptions;
using VakantieParkBL.Model;

namespace UnitTestsModel
{
    public class UnitTestPark
    {
        [Fact]
        public void Ctor_Invalid_WhenFaciliteitenIsEmpty()
        {
            int id = 1;
            string naam = "Programmeren Park";
            string locatie = "G-Blok";
            var faciliteiten = new List<Faciliteit>();

            Assert.Throws<ModelException>(() => new Park(id, naam, locatie, faciliteiten));
        }

        [Fact]
        public void Ctor_ShouldInitializeParkCorrectly()
        {
            int id = 1;
            string naam = "Sarmad Park";
            string locatie = "Waregem";
            List<Faciliteit> faciliteiten = new List<Faciliteit>
            {
                new Faciliteit(1, "Zwembad"),
                new Faciliteit(2, "Restaurant")
            };


            Park park = new Park(id, naam, locatie, faciliteiten);

            Assert.Equal(id, park.Id);
            Assert.Equal(naam, park.Naam);
            Assert.Equal(locatie, park.Locatie);
            Assert.Equal(faciliteiten.Count, park.Faciliteiten.Count);
            Assert.Contains(faciliteiten[0], park.Faciliteiten);
            Assert.Contains(faciliteiten[1], park.Faciliteiten);
        }

        [Fact]
        public void Ctor_Invalid_WhenLocatieIsNullOrEmpty()
        {
            int id = 1;
            string naam = "Mercator Park";
            string locatie = "";
            var faciliteiten = new List<Faciliteit> { new Faciliteit(1, "Zwembad") };


            Assert.Throws<ModelException>(() => new Park(id, naam, locatie, faciliteiten));
        }
    }
}