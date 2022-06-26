using System.Linq;
using PlateWorld.Models.TestData;
using PlateWorld.Models.BasicTypes;
using System.Collections.ObjectModel;
using PlateWorld.Mvvm.Utils;

namespace PlateWorld.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cs = _PropertySets.MakePhs();
            var propes = cs.AllProperties().ToList();
            var csb = _PropertySets.MakeCypSubstrates();
            var lbse = cs.AllProperties().Select(prop => prop.AsList()).ToList();
            var ubse = lbse.Comby(csb.AllProperties().ToList()).ToList();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var cs = _PropertySets.MakePhs();
            var phProps = cs.AllProperties().ToList();
            var csb = _PropertySets.MakeCypSubstrates();
            var cypProps = csb.AllProperties().ToList();
            var rpn = _PropertySets.MakeReplicateNumbers();
            var rpnProps = rpn.AllProperties().ToList();

            var llprops = new List<List<IProperty>> { phProps , cypProps, rpnProps };
            var res = llprops.powerJoin();
        }

        [TestMethod]
        public void MakeSamples()
        {
            var cs = _PropertySets.MakePhs();
            var csb = _PropertySets.MakeCypSubstrates();
            var rpn = _PropertySets.MakeReplicateNumbers();

            var llprops = new List<IPropertySet> { cs, csb, rpn };
            var res = llprops.AllSamples(1).ToList();
        }

        [TestMethod]
        public void GetPropsFromSamples()
        {
            var cs = _PropertySets.MakePhs();
            var csb = _PropertySets.MakeCypSubstrates();
            var rpn = _PropertySets.MakeReplicateNumbers();

            var llprops = new List<IPropertySet> { cs, csb, rpn };
            var allSamples = llprops.AllSamples(1).ToList();

            var allPropertySets = allSamples.GetPropertySets().ToArray();
            Assert.AreEqual(allPropertySets.Length, 3);
        }

        [TestMethod]
        public void ObservableCollectionReplace()
        {
            var oc = new ObservableCollection<int>(new[] {1, 2, 3, 4});
            var ocB = oc.Replace(3, 7, new Func<int, int, bool>((i, j) => i == j));


        }
    }
}