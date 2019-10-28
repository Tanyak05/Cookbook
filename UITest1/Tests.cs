using System;
using System.IO;
using System.Linq;
using Cookbook;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest1
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;
        private Ingredient ingr;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            //app = AppInitializer.StartApp(platform);

            ingr = new Ingredient();
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }


         [Test]
        public void ParseFromString()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("Ingredients");
            Assert.AreEqual(0, ingr.Amount, "amount problem");
            Assert.AreEqual(null, ingr.Item, "item problem");
            Assert.AreEqual(null, ingr.Units, "units problem");
            Assert.AreEqual("Ingredients", ingr.Unparsed, "unparsed problem");
        }

        [Test]
        public void ParseFromString1()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("Eggs 12");
            Assert.AreEqual(12, ingr.Amount, "amount problem");
            Assert.AreEqual("Eggs", ingr.Item, "item problem");
            Assert.AreEqual(Ingredient.DefaultUnit, ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [Test]
        public void ParseFromString2()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("250g plain flour");
            Assert.AreEqual(250, ingr.Amount, "amount problem");
            Assert.AreEqual("plain flour", ingr.Item, "item problem");
            Assert.AreEqual("g", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [Test]
        public void ParseFromString3()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("3 teaspoons baking powder");
            Assert.AreEqual(3, ingr.Amount, "amount problem");
            Assert.AreEqual("baking powder", ingr.Item, "item problem");
            Assert.AreEqual("teaspoon", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [Test]
        public void ParseFromString4()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("1/2 teaspoon salt");
            Assert.AreEqual(0.5, ingr.Amount, "amount problem");
            Assert.AreEqual("salt", ingr.Item, "item problem");
            Assert.AreEqual("teaspoon", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [Test]
        public void ParseFromString5()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("orange juice 180ml");
            Assert.AreEqual(180, ingr.Amount, "amount problem");
            Assert.AreEqual("ml", ingr.Item, "item problem");
            Assert.AreEqual("", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [Test]
        public void ParseFromString6()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("1 egg");
            Assert.AreEqual(1, ingr.Amount, "amount problem");
            Assert.AreEqual("egg", ingr.Item, "item problem");
            Assert.AreEqual(Ingredient.DefaultUnit, ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [Test]
        public void ParseFromString7()
        {
            Ingredient ingr = new Ingredient();
            ingr.TryToParseFromString("orange zest 1 tablespoon ");
            Assert.AreEqual("1", ingr.Amount, "amount problem");
            Assert.AreEqual("orange zest", ingr.Item, "item problem");
            Assert.AreEqual("tablespoon", ingr.Units, "units problem");
            Assert.AreEqual(null, ingr.Unparsed, "unparsed problem");
        }

        [Test]
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
