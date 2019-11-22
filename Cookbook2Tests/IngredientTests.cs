using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cookbook2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cookbook2.Tests
{
    [TestClass()]
    public class IngredientTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Ingredient.LoadPossibleUnits();
        }

        [TestMethod()]
        public void IngredientTest()
        {
            Ingredient ingredient = new Ingredient();
            Assert.AreEqual(0, ingredient.Amount);
            Assert.AreEqual(null, ingredient.Item);
            Assert.AreEqual(null, ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void IngredientTest1()
        {
            Ingredient ingredient = new Ingredient("fresh strawberries diced\t3\tcups");
            Assert.AreEqual(3, ingredient.Amount);
            Assert.AreEqual("fresh strawberries diced", ingredient.Item);
            Assert.AreEqual("cups", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void IngredientTest2()
        {
            Ingredient ingredient = new Ingredient("black pepper");
            Assert.AreEqual(0, ingredient.Amount);
            Assert.AreEqual("black pepper", ingredient.Item);
            Assert.AreEqual("units", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void IngredientTest5()
        {
            Ingredient ingredient = new Ingredient("eggs\t3");
            Assert.AreEqual(3, ingredient.Amount);
            Assert.AreEqual("eggs", ingredient.Item);
            Assert.AreEqual("units", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void IngredientTest3()
        {
            Ingredient ingredient = new Ingredient("");
            Assert.AreEqual(0, ingredient.Amount);
            Assert.AreEqual(null, ingredient.Item);
            Assert.AreEqual(null, ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void IngredientTest4()
        {
            Ingredient ingredient = new Ingredient(null);
            Assert.AreEqual(0, ingredient.Amount);
            Assert.AreEqual(null, ingredient.Item);
            Assert.AreEqual(null, ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void TryToParseFromString()
        {
            Ingredient ingredient = new Ingredient();
            Assert.IsTrue(ingredient.TryToParseFromString("3 cups fresh strawberries diced"));
            Assert.AreEqual(3, ingredient.Amount);
            Assert.AreEqual("fresh strawberries diced", ingredient.Item);
            Assert.AreEqual("cups", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void TryToParseFromString1()
        {
            Ingredient ingredient = new Ingredient();
            Assert.IsTrue(ingredient.TryToParseFromString("1/2-3/4 cup sugar"));
            Assert.AreEqual(0, ingredient.Amount);
            Assert.AreEqual(null, ingredient.Item);
            Assert.AreEqual(null, ingredient.Units);
            Assert.AreEqual("1/2-3/4 cup sugar", ingredient.Unparsed);
        }

        [TestMethod()]
        public void TryToParseFromString2()
        {
            Ingredient ingredient = new Ingredient();
            Assert.IsTrue(ingredient.TryToParseFromString("1/2 teaspoon salt"));
            Assert.AreEqual(0.5, ingredient.Amount);
            Assert.AreEqual("salt", ingredient.Item);
            Assert.AreEqual("teaspoon", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void TryToParseFromString3()
        {
            Ingredient ingredient = new Ingredient();
            Assert.IsTrue(ingredient.TryToParseFromString("1/2 tsp. salt"));
            Assert.AreEqual(0.5, ingredient.Amount);
            Assert.AreEqual("salt", ingredient.Item);
            Assert.AreEqual("teaspoon", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void TryToParseFromString4()
        {
            Ingredient ingredient = new Ingredient();
            Assert.IsTrue(ingredient.TryToParseFromString("1½ cups water"));
            Assert.AreEqual(1.5, ingredient.Amount);
            Assert.AreEqual("water", ingredient.Item);
            Assert.AreEqual("cups", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void TryToParseFromString5()
        {
            Ingredient ingredient = new Ingredient();
            Assert.IsTrue(ingredient.TryToParseFromString("1 stick butter melted"));
            Assert.AreEqual(1, ingredient.Amount);
            Assert.AreEqual("butter melted", ingredient.Item);
            Assert.AreEqual("sticks", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void TryToParseFromString6()
        {
            Ingredient ingredient = new Ingredient();
            Assert.IsTrue(ingredient.TryToParseFromString("1 Stick butter melted"));
            Assert.AreEqual(1, ingredient.Amount);
            Assert.AreEqual("butter melted", ingredient.Item);
            Assert.AreEqual("sticks", ingredient.Units);
            Assert.AreEqual(null, ingredient.Unparsed);
        }

        [TestMethod()]
        public void TryToParseFromString7()
        {
            Ingredient ingredient = new Ingredient();
            Assert.IsFalse(ingredient.TryToParseFromString("black pepper"));
            Assert.AreEqual(0, ingredient.Amount);
            Assert.AreEqual(null, ingredient.Item);
            Assert.AreEqual(null, ingredient.Units);
            Assert.AreEqual("black pepper", ingredient.Unparsed);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Ingredient ingredient = new Ingredient();
            Assert.AreEqual("\t0", ingredient.ToString());
        }


        [TestMethod()]
        public void ToStringTest1()
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Unparsed = "qwerty";
            Assert.AreEqual(ingredient.Unparsed, ingredient.ToString());
        }

        [TestMethod()]
        public void ToStringTest2()
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Amount = 3;
            ingredient.Units = "cups";
            ingredient.Item = "flour";
            Assert.AreEqual("flour\t3\tcups", ingredient.ToString());
        }

        [TestMethod()]
        public void ToStringTest3()
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Amount = 3;
            ingredient.Units = "units";
            ingredient.Item = "apples";
            Assert.AreEqual("apples\t3", ingredient.ToString());
        }

        [TestMethod()]
        public void ToStringTest5()
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Amount = 3;
            ingredient.Item = "apples";
            Assert.AreEqual("apples\t3", ingredient.ToString());
        }

        [TestMethod()]
        public void ToStringTest4()
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Item = "black paper";
            Assert.AreEqual("black paper\t0", ingredient.ToString());
        }
    }
}