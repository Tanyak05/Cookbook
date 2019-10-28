using Cookbook;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class IngredientUnitTest
    {



        [TestMethod]
        public void ParseFromString()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("Ingredients");
            Assert.AreEqual(0, ingr.Amount, "amount problem");
            Assert.AreEqual(null, ingr.Item, "item problem");
            Assert.AreEqual(null, ingr.Units, "units problem");
            Assert.AreEqual("Ingredients", ingr.Unparsed, "unparsed problem");
        }

        [TestMethod]
        public void ParseFromString1()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("Eggs 12");
            Assert.AreEqual(12, ingr.Amount, "amount problem");
            Assert.AreEqual("Eggs", ingr.Item, "item problem");
            Assert.AreEqual(Ingredient.DefaultUnit, ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [TestMethod]
        public void ParseFromString2()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("250g plain flour");
            Assert.AreEqual(250, ingr.Amount, "amount problem");
            Assert.AreEqual("plain flour", ingr.Item, "item problem");
            Assert.AreEqual("g", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [TestMethod]
        public void ParseFromString3()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("3 teaspoons baking powder");
            Assert.AreEqual(3, ingr.Amount, "amount problem");
            Assert.AreEqual("baking powder", ingr.Item, "item problem");
            Assert.AreEqual("teaspoon", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [TestMethod]
        public void ParseFromString4()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("1/2 teaspoon salt");
            Assert.AreEqual(0.5, ingr.Amount, "amount problem");
            Assert.AreEqual("salt", ingr.Item, "item problem");
            Assert.AreEqual("teaspoon", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [TestMethod]
        public void ParseFromString5()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("orange juice 180ml");
            Assert.AreEqual(180, ingr.Amount, "amount problem");
            Assert.AreEqual("ml", ingr.Item, "item problem");
            Assert.AreEqual("", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [TestMethod]
        public void ParseFromString6()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("1 egg");
            Assert.AreEqual(1, ingr.Amount, "amount problem");
            Assert.AreEqual("egg", ingr.Item, "item problem");
            Assert.AreEqual(Ingredient.DefaultUnit, ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [TestMethod]
        public void ParseFromString7()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("orange zest 1 tablespoon ");
            Assert.AreEqual("1", ingr.Amount, "amount problem");
            Assert.AreEqual("orange zest", ingr.Item, "item problem");
            Assert.AreEqual("tablespoon", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [TestMethod]
        public void ParseFromString8()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("orange zest 1 1/2 tablespoon ");
            Assert.AreEqual("1", ingr.Amount, "amount problem");
            Assert.AreEqual("orange zest", ingr.Item, "item problem");
            Assert.AreEqual("tablespoon", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }
    }

}
