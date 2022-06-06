using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.Models.TestData
{
    public enum AnimalType
    {
        Cat,
        Dog,
        Fish,
        Horse,
        Other
    }

    public enum ContainerType
    {
        Barrel,
        PaperCup,
        Glass,
        Mug
    }

    public class AnimalThemedContainer
    {
        public AnimalThemedContainer(
                Guid id,
                ContainerType containerType, 
                AnimalType animalType, 
                bool hasHandle, 
                double? weight = null)
        {
            Id = id;
            ContainerType = containerType;
            AnimalType = animalType;
            HasHandle = hasHandle;
            Weight = weight;
        }

        public Guid Id { get; }
        public ContainerType ContainerType { get; }
        public AnimalType AnimalType { get; }
        public bool HasHandle { get; }
        public double? Weight { get; }
    }

    public static class AnimalThemedContainerExt
    {
        public static IEnumerable<AnimalThemedContainer> 
                MakeSamples(int seed, int count)
        {
            var randy = new Random(seed);
            var animalCt = Enum.GetValues(typeof(AnimalType)).Length;
            var containerCt = Enum.GetValues(typeof(ContainerType)).Length;
            for(var i=0; i<count; i++)
            {
                var nextAnimal = (AnimalType)randy.Next(animalCt);
                var nextContainer = (ContainerType)randy.Next(containerCt);
                var nextHasHandle = (randy.Next(1) == 1) ? true : false;

                yield return new AnimalThemedContainer(
                    id: Guid.NewGuid(),
                    containerType: nextContainer,
                    animalType: nextAnimal,
                    hasHandle: nextHasHandle);
            }
        }

        public static IEnumerable<SampleProperty> GetSampleProperties(
            this AnimalThemedContainer atc)
        {
            yield return new SampleProperty(
                "ContainerType", 
                PropertyType.Int, 
                atc.ContainerType, 
                atc.ContainerType.ToString());

            yield return new SampleProperty(
                "AnimalType",
                PropertyType.Int,
                atc.ContainerType,
                atc.AnimalType.ToString());

            yield return new SampleProperty(
                "HasHandle",
                PropertyType.Bool,
                atc.ContainerType,
                atc.HasHandle.ToString());
        }
    }
}
